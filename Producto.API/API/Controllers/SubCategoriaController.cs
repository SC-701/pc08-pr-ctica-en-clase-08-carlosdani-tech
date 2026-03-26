using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriaController : ControllerBase
    {
        private readonly IProductoFlujo _productoFlujo;

        public SubCategoriaController(IProductoFlujo productoFlujo)
        {
            _productoFlujo = productoFlujo;
        }

        [HttpGet("{idCategoria}")]
        public async Task<IActionResult> ObtenerPorCategoria([FromRoute] Guid idCategoria)
        {
            var resultado = await _productoFlujo.ObtenerSubCategorias(idCategoria);
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}
