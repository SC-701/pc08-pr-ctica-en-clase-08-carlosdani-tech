using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

public class ProductoReglas : IProductoReglas
{
    private readonly ITipoCambioServicio _tipoCambioServicio;

    public ProductoReglas(ITipoCambioServicio tipoCambioServicio)
    {
        _tipoCambioServicio = tipoCambioServicio;
    }

    public async Task<decimal> CalcularPrecioUSD(decimal precioCRC)
    {
        var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambioAsync();
        if (tipoCambio <= 0)
            throw new InvalidOperationException("El tipo de cambio proporcionado no es válido.");

        return Math.Round(precioCRC / tipoCambio, 2, MidpointRounding.AwayFromZero);
    }

    public async Task<IEnumerable<ProductoResponse>> CalcularPrecioUSD(IEnumerable<ProductoResponse> productos)
    {
        var lista = productos?.ToList() ?? new List<ProductoResponse>();
        if (!lista.Any()) return lista;

        var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambioAsync();
        if (tipoCambio <= 0)
            throw new InvalidOperationException("El tipo de cambio proporcionado no es válido.");

        foreach (var p in lista)
            p.PrecioUSD = Math.Round(p.Precio / tipoCambio, 2, MidpointRounding.AwayFromZero);

        return lista;
    }
}
