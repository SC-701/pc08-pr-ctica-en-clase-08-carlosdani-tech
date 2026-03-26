
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	Edita un producto existente
-- =============================================
CREATE PROCEDURE [dbo].[EditarProducto]
	@Id             UNIQUEIDENTIFIER,
	@IdSubCategoria UNIQUEIDENTIFIER,
	@Nombre         VARCHAR(MAX),
	@Descripcion    VARCHAR(MAX),
	@Precio         DECIMAL(18,2),
	@Stock          INT,
	@CodigoBarras   VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		UPDATE dbo.Producto
		SET
			IdSubCategoria = @IdSubCategoria,
			Nombre         = @Nombre,
			Descripcion    = @Descripcion,
			Precio         = @Precio,
			Stock          = @Stock,
			CodigoBarras   = @CodigoBarras
		WHERE Id = @Id;

		-- Devuelve el Id editado (igual que tu SP de Producto)
		SELECT @Id;
	COMMIT TRANSACTION
END
