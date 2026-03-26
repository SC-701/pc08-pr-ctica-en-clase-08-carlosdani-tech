using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Microsoft.Extensions.Logging;

namespace Servicios;

public class TipoCambioServicio : ITipoCambioServicio
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguracion _config;
    private readonly ILogger<TipoCambioServicio> _logger;
    private const string ConfigSection = "BancoCentralCR";

    public TipoCambioServicio(HttpClient httpClient, IConfiguracion configuracion, ILogger<TipoCambioServicio> logger)
    {
        _httpClient = httpClient;
        _config = configuracion;
        _logger = logger;
    }

    public async Task<decimal> ObtenerTipoCambioAsync()
    {
        var urlBase = _config.ObtenerValor(ConfigSection, "UrlBase");
        var token = _config.ObtenerValor(ConfigSection, "BearerToken");

        var (inicio, fin) = ObtenerRangoFechas();
        var url = $"{urlBase}?fechaInicio={Uri.EscapeDataString(inicio)}&fechaFin={Uri.EscapeDataString(fin)}&idioma=ES";

        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var resp = await _httpClient.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        var tipoCambio = ExtraerUltimoTipoCambioValido(json);

        return tipoCambio ?? throw new InvalidOperationException("No se pudo obtener un tipo de cambio válido del BCCR.");
    }

    private static (string inicio, string fin) ObtenerRangoFechas()
    {
        var hoy = ObtenerFechaCostaRica(DateTime.UtcNow);
        var inicio = hoy.AddDays(-30);
        return (inicio.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture),
                hoy.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
    }

    private static DateTime ObtenerFechaCostaRica(DateTime utcNow)
    {
        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz).Date;
        }
        catch
        {
            return DateTime.Now.Date;
        }
    }

    private decimal? ExtraerUltimoTipoCambioValido(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("datos", out var datos)) return null;

            var contenedores = datos.ValueKind switch
            {
                JsonValueKind.Array => datos.EnumerateArray().ToArray(),
                JsonValueKind.Object => new[] { datos },
                _ => Array.Empty<JsonElement>()
            };

            foreach (var cont in contenedores)
            {
                if (!cont.TryGetProperty("indicadores", out var inds) || inds.ValueKind != JsonValueKind.Array) continue;

                foreach (var ind in inds.EnumerateArray().OrderByDescending(NombreScore))
                {
                    if (!ind.TryGetProperty("series", out var series) || series.ValueKind != JsonValueKind.Array) continue;

                    var mejor = series.EnumerateArray()
                        .Select(s => (Fecha: FechaSerie(s), Valor: ValorSerie(s)))
                        .Where(x => x.Valor.HasValue && x.Valor.Value > 0)
                        .OrderByDescending(x => x.Fecha)
                        .FirstOrDefault();

                    if (mejor.Valor.HasValue) return mejor.Valor;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parseando JSON del BCCR");
            return null;
        }
    }

    private static int NombreScore(JsonElement indicador)
    {
        var nombre = indicador.TryGetProperty("nombreIndicador", out var n) && n.ValueKind == JsonValueKind.String
            ? n.GetString()?.ToLowerInvariant() ?? ""
            : "";
        if (nombre.Contains("venta")) return 2;
        if (nombre.Contains("compra")) return 1;
        return 0;
    }

    private static DateTime FechaSerie(JsonElement serie)
    {
        if (serie.TryGetProperty("fecha", out var f) && f.ValueKind == JsonValueKind.String &&
            DateTime.TryParse(f.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
            return dt;
        return DateTime.MinValue;
    }

    private static decimal? ValorSerie(JsonElement serie)
    {
        if (!serie.TryGetProperty("valorDatoPorPeriodo", out var v)) return null;
        if (v.ValueKind == JsonValueKind.Number && v.TryGetDecimal(out var n)) return n;
        if (v.ValueKind == JsonValueKind.String)
        {
            var s = v.GetString();
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var inv)) return inv;
            if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("es-CR"), out var es)) return es;
        }
        return null;
    }
}