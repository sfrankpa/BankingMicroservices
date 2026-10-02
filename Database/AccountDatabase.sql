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
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE TABLE [ClientesReferencia] (
        [Id] int NOT NULL IDENTITY,
        [ClienteId] int NOT NULL,
        [Nombre] nvarchar(150) NOT NULL,
        [Identificacion] nvarchar(20) NOT NULL,
        [Estado] bit NOT NULL,
        CONSTRAINT [PK_ClientesReferencia] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE TABLE [Cuentas] (
        [Id] int NOT NULL IDENTITY,
        [NumeroCuenta] nvarchar(20) NOT NULL,
        [TipoCuenta] nvarchar(20) NOT NULL,
        [SaldoInicial] decimal(18,2) NOT NULL,
        [SaldoActual] decimal(18,2) NOT NULL,
        [Estado] bit NOT NULL,
        [ClienteId] int NOT NULL,
        CONSTRAINT [PK_Cuentas] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE TABLE [Movimientos] (
        [Id] int NOT NULL IDENTITY,
        [Fecha] datetime2 NOT NULL,
        [TipoMovimiento] nvarchar(30) NOT NULL,
        [Valor] decimal(18,2) NOT NULL,
        [Saldo] decimal(18,2) NOT NULL,
        [CuentaId] int NOT NULL,
        CONSTRAINT [PK_Movimientos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Movimientos_Cuentas_CuentaId] FOREIGN KEY ([CuentaId]) REFERENCES [Cuentas] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ClientesReferencia_ClienteId] ON [ClientesReferencia] ([ClienteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ClientesReferencia_Identificacion] ON [ClientesReferencia] ([Identificacion]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Cuentas_NumeroCuenta] ON [Cuentas] ([NumeroCuenta]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    CREATE INDEX [IX_Movimientos_CuentaId] ON [Movimientos] ([CuentaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002004319_InitialAccountDb'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261002004319_InitialAccountDb', N'8.0.31');
END;
GO

COMMIT;
GO

