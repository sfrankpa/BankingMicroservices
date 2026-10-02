/* ============================================================
   BANKING MICROSERVICES
   BASE DE DATOS CONSOLIDADA

   La solución utiliza dos bases de datos independientes:

   CustomerService -> CustomerDb
   AccountService  -> AccountDb

   La separación de bases mantiene la independencia de cada
   microservicio. Este archivo consolida ambos scripts para
   facilitar la revisión y entrega del ejercicio.
   ============================================================ */


/* ============================================================
   CUSTOMER SERVICE
   DATABASE: CustomerDb
   ============================================================ */

IF DB_ID('CustomerDb') IS NULL
BEGIN
    CREATE DATABASE CustomerDb;
END
GO

USE CustomerDb;
GO

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



/* ============================================================
   ACCOUNT SERVICE
   DATABASE: AccountDb
   ============================================================ */

IF DB_ID('AccountDb') IS NULL
BEGIN
    CREATE DATABASE AccountDb;
END
GO

USE AccountDb;
GO

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

/* ============================================================
   DATOS INICIALES DE PRUEBA
   ============================================================ */


/* ============================================================
   CUSTOMER DATABASE
   Cliente: Jose Lema
   ============================================================ */

USE CustomerDb;
GO

IF NOT EXISTS (
    SELECT 1
    FROM Personas
    WHERE Identificacion = '1100000001'
)
BEGIN
    SET IDENTITY_INSERT Personas ON;

    INSERT INTO Personas
    (
        Id,
        Nombre,
        Genero,
        Edad,
        Identificacion,
        Direccion,
        Telefono,
        Discriminator,
        Contrasena,
        Estado
    )
    VALUES
    (
        1,
        N'Jose Lema',
        N'Masculino',
        35,
        N'1100000001',
        N'Otavalo sn y principal',
        N'098254785',
        N'Cliente',
        N'1234',
        1
    );

    SET IDENTITY_INSERT Personas OFF;
END
GO


/* ============================================================
   ACCOUNT DATABASE
   Referencia del cliente
   ============================================================ */

USE AccountDb;
GO

IF NOT EXISTS (
    SELECT 1
    FROM ClientesReferencia
    WHERE ClienteId = 1
)
BEGIN
    INSERT INTO ClientesReferencia
    (
        ClienteId,
        Nombre,
        Identificacion,
        Estado
    )
    VALUES
    (
        1,
        N'Jose Lema',
        N'1100000001',
        1
    );
END
GO


/* ============================================================
   CUENTA INICIAL
   ============================================================ */

IF NOT EXISTS (
    SELECT 1
    FROM Cuentas
    WHERE NumeroCuenta = '478758'
)
BEGIN
    INSERT INTO Cuentas
    (
        NumeroCuenta,
        TipoCuenta,
        SaldoInicial,
        SaldoActual,
        Estado,
        ClienteId
    )
    VALUES
    (
        N'478758',
        N'Ahorros',
        2000.00,
        2025.00,
        1,
        1
    );
END
GO


/* ============================================================
   MOVIMIENTOS INICIALES
   ============================================================ */

DECLARE @CuentaId INT;

SELECT @CuentaId = Id
FROM Cuentas
WHERE NumeroCuenta = '478758';


IF NOT EXISTS (
    SELECT 1
    FROM Movimientos
    WHERE CuentaId = @CuentaId
      AND Valor = 600.00
      AND Fecha = '2026-10-01T10:00:00'
)
BEGIN
    INSERT INTO Movimientos
    (
        Fecha,
        TipoMovimiento,
        Valor,
        Saldo,
        CuentaId
    )
    VALUES
    (
        '2026-10-01T10:00:00',
        N'Deposito',
        600.00,
        2600.00,
        @CuentaId
    );
END;


IF NOT EXISTS (
    SELECT 1
    FROM Movimientos
    WHERE CuentaId = @CuentaId
      AND Valor = -575.00
      AND Fecha = '2026-10-01T12:00:00'
)
BEGIN
    INSERT INTO Movimientos
    (
        Fecha,
        TipoMovimiento,
        Valor,
        Saldo,
        CuentaId
    )
    VALUES
    (
        '2026-10-01T12:00:00',
        N'Retiro',
        -575.00,
        2025.00,
        @CuentaId
    );
END;
GO

