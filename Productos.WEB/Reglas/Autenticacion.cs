// Reglas/Autenticacion.cs
using Abstracciones.Modelos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Reglas
{
    public static class Autenticacion
    {
        // Genera el hash SHA-256 de la contraseña como array de bytes
        public static byte[] GenerarHash(string contrasenia)
        {
            using (SHA256 shaHash = SHA256.Create())
            {
                byte[] bytes = shaHash.ComputeHash(Encoding.UTF8.GetBytes(contrasenia));
                return bytes;
            }
        }

        // Convierte el array de bytes a string hexadecimal (para enviar al API)
        public static string ObtenerHash(byte[] hash)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        // Deserializa el JWT string en un objeto JwtSecurityToken con claims accesibles
        public static JwtSecurityToken? leerToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            return jsonToken;
        }

        // Extrae Name, NameIdentifier, Email del token y agrega el AccessToken como claim
        public static List<Claim> GenerarClaims(JwtSecurityToken? jwtToken, string accessToken)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,
                jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value));
            claims.Add(new Claim(ClaimTypes.NameIdentifier,
                jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));
            claims.Add(new Claim(ClaimTypes.Email,
                jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value));
            claims.Add(new Claim("Token", accessToken));
            return claims;
        }
    }
}