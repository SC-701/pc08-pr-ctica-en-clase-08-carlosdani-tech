using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IProductoDA
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoResponse> Obtener(Guid Id);
        Task<IEnumerable<Categoria>> ObtenerCategorias();
        Task<IEnumerable<SubCategoria>> ObtenerSubCategorias(Guid idCategoria);
        Task<Guid> Agregar(ProductoRequest producto);
        Task<Guid> Editar(Guid Id, ProductoRequest producto);
        Task<Guid> Eliminar(Guid Id);
    }
}
