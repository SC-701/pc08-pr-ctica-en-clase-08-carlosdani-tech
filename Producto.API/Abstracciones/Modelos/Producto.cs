using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "La propiedad nombre es requerida")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "La propiedad nombre debe tener entre 10 y 50 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La propiedad descripcion es requerida")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "La propiedad descripcion debe tener entre 10 y 200 caracteres")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La propiedad precio es requerida")]
        [Range(typeof(decimal), "0,01", "9999999999", ErrorMessage = "La propiedad precio debe ser mayor a 0")]
        [JsonPropertyOrder(1)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La propiedad stock es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "La propiedad stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La propiedad codigoBarras es requerida")]
        [RegularExpression(@"^\d{8,14}$", ErrorMessage = "El Codigo de Barras debe contener solo dígitos y tener entre 8 y 14 caracteres")]
        public string CodigoBarras { get; set; } = null!;
    }

    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "La propiedad idSubCategoria es requerida")]
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoDetalle
    {
        [Required(ErrorMessage = "La propiedad id es requerida")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "La propiedad idSubCategoria es requerida")]
        public Guid IdSubCategoria { get; set; }

        [Required(ErrorMessage = "La propiedad idCategoria es requerida")]
        public Guid IdCategoria { get; set; }

        [Required(ErrorMessage = "La propiedad subCategoria es requerida")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La propiedad subCategoria debe tener entre 2 y 100 caracteres")]
        public string SubCategoria { get; set; } = null!;

        [Required(ErrorMessage = "La propiedad categoria es requerida")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La propiedad categoria debe tener entre 2 y 100 caracteres")]
        public string Categoria { get; set; } = null!;
    }

    public class ProductoDetalle : ProductoBase
    {
        [JsonPropertyOrder(2)]
        public decimal PrecioUSD { get; set; }
    }

    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class SubCategoria
    {
        public Guid Id { get; set; }
        public Guid IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
