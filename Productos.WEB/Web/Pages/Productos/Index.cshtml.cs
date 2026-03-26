using System.Net;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Productos
{
    [Authorize]
    public class IndexModel : ProductoPageModelBase
    {
        public IList<ProductoResponse> Productos { get; set; } = new List<ProductoResponse>();

        public string? ErrorMessage { get; set; }

        public IndexModel(IConfiguracion configuracion) : base(configuracion)
        {
        }

        public async Task OnGet()
        {
            string endpoint = ObtenerEndpoint("ObtenerProductos");

            try
            {
                using var cliente = CrearClienteConToken();
                var respuesta = await cliente.GetAsync(endpoint);

                if (respuesta.StatusCode == HttpStatusCode.NoContent)
                {
                    Productos = new List<ProductoResponse>();
                    return;
                }

                respuesta.EnsureSuccessStatusCode();
                Productos = await LeerJsonAsync<List<ProductoResponse>>(respuesta) ?? new List<ProductoResponse>();
            }
            catch (HttpRequestException)
            {
                ErrorMessage = $"No se pudo conectar con Producto.API en {endpoint}. Verifica que el API este ejecutandose.";
            }
        }
    }
}
