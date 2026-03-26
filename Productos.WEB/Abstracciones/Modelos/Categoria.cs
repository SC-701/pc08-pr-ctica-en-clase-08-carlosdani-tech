using System.ComponentModel;

namespace Abstracciones.Modelos
{
    public class Categoria
    {
        public Guid Id { get; set; }

        [DisplayName("Categoria")]
        public string Nombre { get; set; } = string.Empty;
    }
}
