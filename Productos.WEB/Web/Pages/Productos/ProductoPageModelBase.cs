using System.Net.Http.Headers;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Productos
{
    public abstract class ProductoPageModelBase : PageModel
    {
        protected static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IConfiguracion _configuracion;

        protected ProductoPageModelBase(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        protected string ObtenerEndpoint(string nombreMetodo)
        {
            return _configuracion.ObtenerMetodo("ApiEndPoints", nombreMetodo);
        }

        protected HttpClient CrearClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Token");
            var cliente = new HttpClient();

            if (!string.IsNullOrWhiteSpace(tokenClaim?.Value))
            {
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenClaim.Value);
            }

            return cliente;
        }

        protected static async Task<T?> LeerJsonAsync<T>(HttpResponseMessage respuesta)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(contenido, JsonOptions);
        }
    }
}
