using Abstracciones.Interfaces.Reglas;
using Abstracciones.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;

namespace Web.Pages.Cuenta
{
    public class RegistroModel : PageModel
    {
        [BindProperty]
        public Usuario usuario { get; set; } = new();

        private readonly IConfiguracion _configuracion;

        public RegistroModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Productos/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.Equals(usuario.Password, usuario.ConfirmarPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("usuario.ConfirmarPassword", "Las contrasenas no coinciden.");
                return Page();
            }

            try
            {
                var hash = Autenticacion.GenerarHash(usuario.Password);
                usuario.PasswordHash = Autenticacion.ObtenerHash(hash);

                string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "Registro");
                using var cliente = new HttpClient();
                var respuesta = await cliente.PostAsJsonAsync<UsuarioBase>(endpoint, usuario);

                if (!respuesta.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo completar el registro.");
                    return Page();
                }

                return RedirectToPage("/Seguridad/Login");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No fue posible comunicarse con el servicio de seguridad.");
                return Page();
            }
        }
    }
}
