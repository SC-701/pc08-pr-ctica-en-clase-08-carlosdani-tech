// NUEVO: Abstracciones/Modelos/Seguridad/Login.cs
using System.ComponentModel.DataAnnotations;

public class LoginBase
    {
        
        public string? NombreUsuario { get; set; }

        public string? PasswordHash { get; set; }
        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; }
    }

        public class Login : LoginBase 
{
    [Required]
    public Guid Id { get; set; }
}

public class LoginRequest : LoginBase
    {
        [Required]
        public string Password { get; set; }
}