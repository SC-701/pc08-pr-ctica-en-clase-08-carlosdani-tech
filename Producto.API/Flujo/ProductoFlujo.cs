using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;

namespace Flujo;

public class ProductoFlujo : IProductoFlujo
{
    public IProductoDA _productoDA;
    private readonly IProductoReglas _productoReglas;

    public ProductoFlujo(IProductoDA productoDA, IProductoReglas productoReglas)
    {
        _productoDA = productoDA;
        _productoReglas = productoReglas;
    }

    public Task<Guid> Agregar(ProductoRequest producto)
    {
        return _productoDA.Agregar(producto);
    }

    public Task<Guid> Editar(Guid Id, ProductoRequest producto)
    {
        return _productoDA.Editar(Id, producto);
    }

    public Task<Guid> Eliminar(Guid Id)
    {
        return _productoDA.Eliminar(Id);
    }

    public async Task<IEnumerable<ProductoResponse>> Obtener()
    {
        var productos = await _productoDA.Obtener();
        return await _productoReglas.CalcularPrecioUSD(productos);
    }

    public async Task<ProductoResponse> Obtener(Guid Id)
    {
        var producto = await _productoDA.Obtener(Id);
        if (producto == null)
        {
            return producto;
        }

        producto.PrecioUSD = await _productoReglas.CalcularPrecioUSD(producto.Precio);
        return producto;
    }

    public Task<IEnumerable<Categoria>> ObtenerCategorias()
    {
        return _productoDA.ObtenerCategorias();
    }

    public Task<IEnumerable<SubCategoria>> ObtenerSubCategorias(Guid idCategoria)
    {
        return _productoDA.ObtenerSubCategorias(idCategoria);
    }
}
