
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	Obtiene un producto por Id con SubCategoria y Categoria
-- =============================================
CREATE PROCEDURE [dbo].[ObtenerProducto]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		p.Id,
		p.IdSubCategoria,
		c.Id AS IdCategoria,
		p.Nombre,
		p.Descripcion,
		p.Precio,
		p.Stock,
		p.CodigoBarras,
		sc.Nombre AS SubCategoria,
		c.Nombre  AS Categoria
	FROM dbo.Producto p
	INNER JOIN dbo.SubCategorias sc ON sc.Id = p.IdSubCategoria
	INNER JOIN dbo.Categorias c     ON c.Id  = sc.IdCategoria
	WHERE p.Id = @Id;
END
