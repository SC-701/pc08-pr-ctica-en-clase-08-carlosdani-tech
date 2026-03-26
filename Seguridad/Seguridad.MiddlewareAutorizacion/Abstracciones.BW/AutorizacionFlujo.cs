using Autorizacion.Abstracciones.Flujo;
using Autorizacion.Abstracciones.DA;
using Autorizacion.Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorizacion.Flujo
{
    public class AutorizacionFlujo : IAutorizacionFlujo
    {
        private ISeguridadDA _seguridadDA;

        public AutorizacionFlujo(ISeguridadDA seguridadDA)
        {
            _seguridadDA = seguridadDA;
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesxUsuario(Usuario usuario)
        {
            return await _seguridadDA.ObtenerPerfilesxUsuario(usuario);
        }

        public async Task<Usuario> ObtenerUsuario(Usuario usuario)
        {
            return await _seguridadDA.ObtenerInformacionUsuario(usuario);
        }
    }
}
