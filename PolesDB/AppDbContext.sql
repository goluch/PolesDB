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
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE TABLE [Persons] (
        [Id] int NOT NULL,
        [Forename] nvarchar(max) NOT NULL,
        [Surname] nvarchar(max) NOT NULL,
        [BirthDate] datetime2 NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [Earnings] int NOT NULL,
        [MotherId] int NULL,
        [FatherId] int NULL,
        [PartnerId] int NULL,
        [PersonId] int NULL,
        CONSTRAINT [PK_Persons] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Persons_Persons_FatherId] FOREIGN KEY ([FatherId]) REFERENCES [Persons] ([Id]),
        CONSTRAINT [FK_Persons_Persons_MotherId] FOREIGN KEY ([MotherId]) REFERENCES [Persons] ([Id]),
        CONSTRAINT [FK_Persons_Persons_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [Persons] ([Id]),
        CONSTRAINT [FK_Persons_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE TABLE [Companies] (
        [Id] int NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [BossId] int NOT NULL,
        CONSTRAINT [PK_Companies] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Companies_Persons_BossId] FOREIGN KEY ([BossId]) REFERENCES [Persons] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE TABLE [Employments] (
        [Id] int NOT NULL,
        [CompanyId] int NOT NULL,
        [EmploeeId] int NOT NULL,
        [Contract] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Employments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Employments_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Employments_Persons_EmploeeId] FOREIGN KEY ([EmploeeId]) REFERENCES [Persons] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Companies_BossId] ON [Companies] ([BossId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Employments_CompanyId] ON [Employments] ([CompanyId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Employments_EmploeeId] ON [Employments] ([EmploeeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Persons_FatherId] ON [Persons] ([FatherId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Persons_MotherId] ON [Persons] ([MotherId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Persons_PartnerId] ON [Persons] ([PartnerId]) WHERE [PartnerId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Persons_PersonId] ON [Persons] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240805210729_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240805210729_InitialCreate', N'8.0.7');
END;
GO

COMMIT;
GO