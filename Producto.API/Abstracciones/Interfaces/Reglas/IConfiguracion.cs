namespace Abstracciones.Interfaces.Reglas
{
    public interface IConfiguracion
    {
        string ObtenerValor(string clave);
        string ObtenerValor(string seccion, string nombre);
    }
}