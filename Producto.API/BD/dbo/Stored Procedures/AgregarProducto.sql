-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AgregarProducto]
	-- Add the parameters for the stored procedure here
    @Id AS UNIQUEIDENTIFIER,
    @IdSubCategoria AS UNIQUEIDENTIFIER,
    @Nombre AS VARCHAR(50),
    @Descripcion AS VARCHAR(50),
    @Precio AS DECIMAL (18, 2),
    @Stock AS INT,
    @CodigoBarras AS VARCHAR (MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    	BEGIN TRANSACTION
		INSERT INTO [dbo].[Producto]
			   ([Id]
			   ,[IdSubCategoria]
			   ,[Nombre]
			   ,[Descripcion]
			   ,[Precio]
			   ,[Stock]
			   ,[CodigoBarras])
		 VALUES
				(@Id,
				@IdSubCategoria,
				@Nombre,
				@Descripcion,
				@Precio,
				@Stock,
				@CodigoBarras)
		SELECT @Id 
	COMMIT TRANSACTION
END