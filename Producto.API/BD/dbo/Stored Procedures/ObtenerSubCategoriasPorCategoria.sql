CREATE PROCEDURE [dbo].[ObtenerSubCategoriasPorCategoria]
    @IdCategoria UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sc.Id,
        sc.IdCategoria,
        sc.Nombre
    FROM dbo.SubCategorias sc
    WHERE sc.IdCategoria = @IdCategoria
    ORDER BY sc.Nombre;
END
