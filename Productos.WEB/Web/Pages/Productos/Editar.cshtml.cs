using System.Net;
using System.Net.Http.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Productos
{
    [Authorize]
    public class EditarModel : ProductoPageModelBase
    {
        [BindProperty]
        public ProductoResponse Producto { get; set; } = new();

        [BindProperty]
        public Guid CategoriaSeleccionada { get; set; }

        [BindProperty]
        public Guid SubCategoriaSeleccionada { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<SelectListItem> SubCategorias { get; set; } = new();

        public EditarModel(IConfiguracion configuracion) : base(configuracion)
        {
        }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty)
            {
                return NotFound();
            }

            string endpoint = string.Format(ObtenerEndpoint("ObtenerProducto"), id.Value);
            using var cliente = CrearClienteConToken();
            var respuesta = await cliente.GetAsync(endpoint);

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            respuesta.EnsureSuccessStatusCode();
            Producto = await LeerJsonAsync<ProductoResponse>(respuesta) ?? new ProductoResponse();
            CategoriaSeleccionada = Producto.IdCategoria;
            SubCategoriaSeleccionada = Producto.IdSubCategoria;

            await CargarFormularioAsync();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ValidarSeleccionCatalogos();

            if (!ModelState.IsValid)
            {
                await CargarFormularioAsync();
                return Page();
            }

            string endpoint = string.Format(ObtenerEndpoint("EditarProducto"), Producto.Id);
            using var cliente = CrearClienteConToken();
            var respuesta = await cliente.PutAsJsonAsync(endpoint, new ProductoRequest
            {
                IdSubCategoria = SubCategoriaSeleccionada,
                Nombre = Producto.Nombre,
                Descripcion = Producto.Descripcion,
                Precio = Producto.Precio,
                Stock = Producto.Stock,
                CodigoBarras = Producto.CodigoBarras
            });
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaId)
        {
            var subcategorias = await ObtenerSubCategoriasAsync(categoriaId);
            return new JsonResult(subcategorias);
        }

        private async Task CargarFormularioAsync()
        {
            await CargarCategoriasAsync();

            SubCategorias = new List<SelectListItem>();
            if (CategoriaSeleccionada != Guid.Empty)
            {
                var subcategorias = await ObtenerSubCategoriasAsync(CategoriaSeleccionada);
                SubCategorias = subcategorias.Select(subCategoria => new SelectListItem
                {
                    Value = subCategoria.Id.ToString(),
                    Text = subCategoria.Nombre,
                    Selected = subCategoria.Id == SubCategoriaSeleccionada
                }).ToList();
            }
        }

        private async Task CargarCategoriasAsync()
        {
            string endpoint = ObtenerEndpoint("ObtenerCategorias");
            using var cliente = CrearClienteConToken();
            var respuesta = await cliente.GetAsync(endpoint);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                Categorias = new List<SelectListItem>();
                return;
            }

            respuesta.EnsureSuccessStatusCode();
            var categorias = await LeerJsonAsync<List<Categoria>>(respuesta) ?? new List<Categoria>();

            Categorias = categorias.Select(categoria => new SelectListItem
            {
                Value = categoria.Id.ToString(),
                Text = categoria.Nombre,
                Selected = categoria.Id == CategoriaSeleccionada
            }).ToList();
        }

        private async Task<List<SubCategoria>> ObtenerSubCategoriasAsync(Guid categoriaId)
        {
            string endpoint = string.Format(ObtenerEndpoint("ObtenerSubCategorias"), categoriaId);
            using var cliente = CrearClienteConToken();
            var respuesta = await cliente.GetAsync(endpoint);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                return new List<SubCategoria>();
            }

            respuesta.EnsureSuccessStatusCode();
            return await LeerJsonAsync<List<SubCategoria>>(respuesta) ?? new List<SubCategoria>();
        }

        private void ValidarSeleccionCatalogos()
        {
            if (CategoriaSeleccionada == Guid.Empty)
            {
                ModelState.AddModelError(nameof(CategoriaSeleccionada), "La categoria es requerida.");
            }

            if (SubCategoriaSeleccionada == Guid.Empty)
            {
                ModelState.AddModelError(nameof(SubCategoriaSeleccionada), "La subcategoria es requerida.");
            }
        }
    }
}
