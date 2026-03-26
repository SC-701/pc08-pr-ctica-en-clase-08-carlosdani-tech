CREATE TABLE [dbo].[Categorias] (
    [Id]     UNIQUEIDENTIFIER NOT NULL,
    [Nombre] VARCHAR (MAX)    NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED ([Id] ASC)
);

