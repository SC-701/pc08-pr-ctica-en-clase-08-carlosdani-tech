namespace Abstracciones.Interfaces.Servicios;

public interface ITipoCambioServicio
{
    Task<decimal> ObtenerTipoCambioAsync();
}
