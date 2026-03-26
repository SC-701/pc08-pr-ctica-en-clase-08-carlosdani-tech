CREATE PROCEDURE [dbo].[ObtenerCategorias]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id,
        c.Nombre
    FROM dbo.Categorias c
    ORDER BY c.Nombre;
END
