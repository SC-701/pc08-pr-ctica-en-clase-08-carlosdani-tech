using Abstracciones.Interfaces.Reglas;
using Microsoft.Extensions.Configuration;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private readonly IConfiguration _configuration;

        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerValor(string clave) =>
            _configuration[clave] ?? throw new InvalidOperationException($"No se encontró '{clave}'.");

        public string ObtenerValor(string seccion, string nombre)
        {
            var valor = _configuration[$"{seccion}:{nombre}"];
            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException($"No se encontró '{seccion}:{nombre}'.");
            return valor;
        }
    }
}