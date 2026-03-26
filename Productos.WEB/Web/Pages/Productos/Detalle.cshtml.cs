using System.Net;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages.Productos
{
    [Authorize]
    public class DetalleModel : ProductoPageModelBase
    {
        public ProductoResponse Producto { get; set; } = new();

        public DetalleModel(IConfiguracion configuracion) : base(configuracion)
        {
        }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty)
            {
                return NotFound();
            }

            string endpoint = string.Format(ObtenerEndpoint("ObtenerProducto"), id.Value);
            using var cliente = CrearClienteConToken();
            var respuesta = await cliente.GetAsync(endpoint);

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            respuesta.EnsureSuccessStatusCode();
            Producto = await LeerJsonAsync<ProductoResponse>(respuesta) ?? new ProductoResponse();
            return Page();
        }
    }
}
