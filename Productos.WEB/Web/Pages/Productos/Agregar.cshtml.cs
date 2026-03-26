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
    public class AgregarModel : ProductoPageModelBase
    {
        [BindProperty]
        public ProductoRequest Producto { get; set; } = new();

        [BindProperty]
        public Guid CategoriaSeleccionada { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<SelectListItem> SubCategorias { get; set; } = new();

        public AgregarModel(IConfiguracion configuracion) : base(configuracion)
        {
        }

        public async Task<IActionResult> OnGet()
        {
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

            string endpoint = ObtenerEndpoint("AgregarProducto");
            using var cliente = CrearClienteConToken();
            var respuesta = await cliente.PostAsJsonAsync(endpoint, Producto);
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
                    Selected = subCategoria.Id == Producto.IdSubCategoria
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

            if (Producto.IdSubCategoria == Guid.Empty)
            {
                ModelState.AddModelError("Producto.IdSubCategoria", "La subcategoria es requerida.");
            }
        }
    }
}
