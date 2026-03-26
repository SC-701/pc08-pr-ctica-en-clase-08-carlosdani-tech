using Autorizacion.Abstracciones.DA;
using Autorizacion.Abstracciones.Modelos;
using System.Data.SqlClient;
using Dapper;


namespace Autorizacion.DA
{
    public class SeguridadDA : ISeguridadDA
    {
        IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public SeguridadDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = repositorioDapper.ObtenerRepositorioDapper();
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesxUsuario(Usuario usuario)
        {
            string sql = @"[ObtenerPerfilesxUsuario]";
            var resultado = await _sqlConnection.QueryAsync<Perfil>(sql, new { CorreoElectronico = usuario.CorreoElectronico, NombreUsuario = usuario.NombreUsuario });
            return resultado;
        }

        public async Task<Usuario> ObtenerInformacionUsuario(Usuario usuario)
        {
            string sql = @"[ObtenerUsuario]";
            var resultado = await _sqlConnection.QueryAsync<Usuario>(sql, new { CorreoElectronico = usuario.CorreoElectronico, NombreUsuario = usuario.NombreUsuario });
            return resultado.FirstOrDefault();
        }
    }
}
