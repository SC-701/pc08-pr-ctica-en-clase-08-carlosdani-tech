
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	Elimina un producto por Id
-- =============================================
CREATE PROCEDURE [dbo].[EliminarProducto]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		DELETE
		FROM dbo.Producto
		WHERE Id = @Id;

		-- Devuelve el Id eliminado (igual que tu SP de Producto)
		SELECT @Id;
	COMMIT TRANSACTION
END
