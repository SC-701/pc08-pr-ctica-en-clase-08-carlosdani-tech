using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IProductoReglas
    {
        Task<decimal> CalcularPrecioUSD(decimal precioCRC);
        Task<IEnumerable<ProductoResponse>> CalcularPrecioUSD(IEnumerable<ProductoResponse> productos);
    }
}