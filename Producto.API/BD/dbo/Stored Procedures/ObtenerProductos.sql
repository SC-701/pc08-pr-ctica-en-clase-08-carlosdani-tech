-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObtenerProductos
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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
    INNER JOIN dbo.Categorias c     ON c.Id  = sc.IdCategoria;
END
