using System.ComponentModel;

namespace Abstracciones.Modelos
{
    public class SubCategoria
    {
        public Guid Id { get; set; }

        public Guid IdCategoria { get; set; }

        [DisplayName("Subcategoria")]
        public string Nombre { get; set; } = string.Empty;
    }
}
