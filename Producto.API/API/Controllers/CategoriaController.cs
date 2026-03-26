using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IProductoFlujo _productoFlujo;

        public CategoriaController(IProductoFlujo productoFlujo)
        {
            _productoFlujo = productoFlujo;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _productoFlujo.ObtenerCategorias();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}
