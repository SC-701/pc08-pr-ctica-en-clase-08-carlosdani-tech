using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace API.Autenticacion
{
    public class JwtBearerPassthroughHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public JwtBearerPassthroughHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var valorHeader = authorizationHeader.ToString();
            if (!valorHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var token = valorHeader["Bearer ".Length..].Trim();
            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("El token es requerido."));
            }

            try
            {
                var claims = LeerClaims(token);
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex.Message));
            }
        }

        private static IReadOnlyCollection<Claim> LeerClaims(string token)
        {
            var segmentos = token.Split('.');
            if (segmentos.Length < 2)
            {
                throw new InvalidOperationException("El token no tiene un formato JWT valido.");
            }

            var payload = DecodificarBase64Url(segmentos[1]);
            using var documento = JsonDocument.Parse(payload);

            var claims = new List<Claim>();
            DateTimeOffset? expiracion = null;

            foreach (var propiedad in documento.RootElement.EnumerateObject())
            {
                if (propiedad.NameEquals("exp") && TryObtenerExpiracion(propiedad.Value, out var exp))
                {
                    expiracion = exp;
                    continue;
                }

                AgregarClaim(claims, propiedad.Name, propiedad.Value);
            }

            if (expiracion.HasValue && expiracion.Value <= DateTimeOffset.UtcNow)
            {
                throw new InvalidOperationException("El token expiro.");
            }

            if (!claims.Any())
            {
                throw new InvalidOperationException("El token no contiene claims utilizables.");
            }

            return claims;
        }

        private static void AgregarClaim(List<Claim> claims, string nombre, JsonElement valor)
        {
            var nombreNormalizado = NormalizarClaim(nombre);

            switch (valor.ValueKind)
            {
                case JsonValueKind.Array:
                    foreach (var item in valor.EnumerateArray())
                    {
                        AgregarClaim(claims, nombre, item);
                    }
                    break;
                case JsonValueKind.String:
                    AgregarClaimSiTieneValor(claims, nombreNormalizado, valor.GetString());
                    break;
                case JsonValueKind.Number:
                    AgregarClaimSiTieneValor(claims, nombreNormalizado, valor.GetRawText());
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    AgregarClaimSiTieneValor(claims, nombreNormalizado, valor.GetBoolean().ToString());
                    break;
                case JsonValueKind.Object:
                    AgregarClaimSiTieneValor(claims, nombreNormalizado, valor.GetRawText());
                    break;
            }
        }

        private static void AgregarClaimSiTieneValor(List<Claim> claims, string tipo, string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                claims.Add(new Claim(tipo, valor));
            }
        }

        private static string NormalizarClaim(string nombre)
        {
            return nombre.ToLowerInvariant() switch
            {
                "sub" => ClaimTypes.NameIdentifier,
                "nameid" => ClaimTypes.NameIdentifier,
                "nameidentifier" => ClaimTypes.NameIdentifier,
                "unique_name" => ClaimTypes.Name,
                "name" => ClaimTypes.Name,
                "email" => ClaimTypes.Email,
                "role" => ClaimTypes.Role,
                "roles" => ClaimTypes.Role,
                _ => nombre
            };
        }

        private static bool TryObtenerExpiracion(JsonElement elemento, out DateTimeOffset expiracion)
        {
            if (elemento.ValueKind == JsonValueKind.Number && elemento.TryGetInt64(out var segundosEpoch))
            {
                expiracion = DateTimeOffset.FromUnixTimeSeconds(segundosEpoch);
                return true;
            }

            if (elemento.ValueKind == JsonValueKind.String && long.TryParse(elemento.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out segundosEpoch))
            {
                expiracion = DateTimeOffset.FromUnixTimeSeconds(segundosEpoch);
                return true;
            }

            expiracion = default;
            return false;
        }

        private static string DecodificarBase64Url(string valor)
        {
            var base64 = valor.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
