using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El nombre debe tener entre 10 y 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripcion es requerida.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "La descripcion debe tener entre 10 y 200 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es requerido.")]
        [Range(typeof(decimal), "0,01", "9999999999", ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El codigo de barras es requerido.")]
        [RegularExpression(@"^\d{8,14}$", ErrorMessage = "El codigo de barras debe contener entre 8 y 14 digitos.")]
        [DisplayName("Codigo de barras")]
        public string CodigoBarras { get; set; } = string.Empty;
    }

    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "La subcategoria es requerida.")]
        [DisplayName("Subcategoria")]
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoDetalle : ProductoBase
    {
        [DisplayName("Precio en USD")]
        public decimal PrecioUSD { get; set; }
    }

    public class ProductoResponse : ProductoDetalle
    {
        public Guid Id { get; set; }

        public Guid IdSubCategoria { get; set; }

        public Guid IdCategoria { get; set; }

        [DisplayName("Subcategoria")]
        public string SubCategoria { get; set; } = string.Empty;

        [DisplayName("Categoria")]
        public string Categoria { get; set; } = string.Empty;
    }
}
