// NUEVO: Abstracciones/Modelos/Seguridad/Token.cs
namespace Abstracciones.Modelos.Seguridad
{
    public class Token
    {
        public bool ValidacionExitosa { get; set; }
        public string? AccessToken { get; set; }
    }
}