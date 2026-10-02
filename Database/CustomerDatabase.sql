IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004131_InitialCustomerDb'
)
BEGIN
    CREATE TABLE [Personas] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(150) NOT NULL,
        [Genero] nvarchar(20) NOT NULL,
        [Edad] int NOT NULL,
        [Identificacion] nvarchar(20) NOT NULL,
        [Direccion] nvarchar(250) NOT NULL,
        [Telefono] nvarchar(20) NOT NULL,
        [Discriminator] nvarchar(8) NOT NULL,
        [Contrasena] nvarchar(100) NULL,
        [Estado] bit NULL,
        CONSTRAINT [PK_Personas] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004131_InitialCustomerDb'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Personas_Identificacion] ON [Personas] ([Identificacion]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004131_InitialCustomerDb'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261002004131_InitialCustomerDb', N'8.0.31');
END;
GO

COMMIT;
GO

