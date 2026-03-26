using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;

namespace Web.Pages.Cuenta
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginRequest loginInfo { get; set; } = new();

        [BindProperty]
        public Token? token { get; set; }

        private readonly IConfiguracion _configuracion;

        public LoginModel(IConfiguracion configuracion)
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

            try
            {
                var hash = Autenticacion.GenerarHash(loginInfo.Password);
                loginInfo.PasswordHash = Autenticacion.ObtenerHash(hash);
                loginInfo.NombreUsuario = loginInfo.CorreoElectronico.Split("@")[0];

                string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "Login");
                using var client = new HttpClient();
                var respuesta = await client.PostAsJsonAsync(endpoint, new LoginBase
                {
                    NombreUsuario = loginInfo.NombreUsuario,
                    CorreoElectronico = loginInfo.CorreoElectronico,
                    PasswordHash = loginInfo.PasswordHash
                });

                if (!respuesta.IsSuccessStatusCode)
                {
                    token = new Token { ValidacionExitosa = false };
                    return Page();
                }

                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var contenido = await respuesta.Content.ReadAsStringAsync();
                token = JsonSerializer.Deserialize<Token>(contenido, opciones);

                if (token?.ValidacionExitosa == true && !string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    JwtSecurityToken? jwtToken = Autenticacion.leerToken(token.AccessToken);
                    var claims = Autenticacion.GenerarClaims(jwtToken, token.AccessToken);
                    await EstablecerAutenticacionAsync(claims);

                    var urlRedirigir = $"{HttpContext.Request.Query["ReturnUrl"]}";
                    if (string.IsNullOrWhiteSpace(urlRedirigir))
                    {
                        return RedirectToPage("/Productos/Index");
                    }

                    return Redirect(urlRedirigir);
                }

                token ??= new Token { ValidacionExitosa = false };
                return Page();
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No fue posible comunicarse con el servicio de seguridad.");
                token = new Token { ValidacionExitosa = false };
                return Page();
            }
        }

        private async Task EstablecerAutenticacionAsync(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
    }
}
