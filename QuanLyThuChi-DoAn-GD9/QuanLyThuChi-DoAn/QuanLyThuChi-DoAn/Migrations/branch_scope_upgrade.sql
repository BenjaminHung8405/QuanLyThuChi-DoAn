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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [LogId] bigint NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [UserId] int NULL,
        [UserName] nvarchar(100) NOT NULL,
        [ActionType] nvarchar(20) NOT NULL,
        [TableName] nvarchar(50) NOT NULL,
        [RecordId] nvarchar(50) NOT NULL,
        [OldValues] nvarchar(max) NOT NULL,
        [NewValues] nvarchar(max) NOT NULL,
        [ActionDate] datetime2 NOT NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([LogId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Permissions] (
        [PermissionCode] nvarchar(50) NOT NULL,
        [PermissionName] nvarchar(100) NOT NULL,
        [GroupName] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([PermissionCode])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Tenants] (
        [TenantId] int NOT NULL IDENTITY,
        [TenantCode] nvarchar(50) NOT NULL,
        [TenantName] nvarchar(255) NOT NULL,
        [TaxCode] nvarchar(50) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [IsActive] bit NOT NULL,
        [ExpireDate] datetime2 NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Tenants] PRIMARY KEY ([TenantId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Branches] (
        [BranchId] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [BranchName] nvarchar(255) NOT NULL,
        [Address] nvarchar(500) NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Branches] PRIMARY KEY ([BranchId]),
        CONSTRAINT [FK_Branches_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Partners] (
        [PartnerId] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [PartnerName] nvarchar(255) NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [Address] nvarchar(500) NOT NULL,
        [Type] nvarchar(20) NOT NULL,
        [InitialDebt] decimal(18,0) NOT NULL,
        CONSTRAINT [PK_Partners] PRIMARY KEY ([PartnerId]),
        CONSTRAINT [FK_Partners_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Roles] (
        [RoleId] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [RoleName] nvarchar(100) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId]),
        CONSTRAINT [FK_Roles_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [TransactionCategories] (
        [CategoryId] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [CategoryName] nvarchar(100) NOT NULL,
        [Type] nvarchar(10) NOT NULL,
        CONSTRAINT [PK_TransactionCategories] PRIMARY KEY ([CategoryId]),
        CONSTRAINT [FK_TransactionCategories_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [CashFunds] (
        [FundId] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [BranchId] int NOT NULL,
        [FundName] nvarchar(100) NOT NULL,
        [AccountNumber] nvarchar(50) NOT NULL,
        [Balance] decimal(18,0) NOT NULL,
        CONSTRAINT [PK_CashFunds] PRIMARY KEY ([FundId]),
        CONSTRAINT [FK_CashFunds_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CashFunds_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Debts] (
        [DebtId] bigint NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [BranchId] int NOT NULL,
        [PartnerId] int NOT NULL,
        [DebtType] nvarchar(20) NOT NULL,
        [TotalAmount] decimal(18,0) NOT NULL,
        [PaidAmount] decimal(18,0) NOT NULL,
        [DueDate] datetime2 NULL,
        [Status] nvarchar(20) NOT NULL,
        [Notes] nvarchar(500) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Debts] PRIMARY KEY ([DebtId]),
        CONSTRAINT [FK_Debts_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Debts_Partners_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [Partners] ([PartnerId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Debts_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [RolePermissions] (
        [RoleId] int NOT NULL,
        [PermissionCode] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionCode]),
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionCode] FOREIGN KEY ([PermissionCode]) REFERENCES [Permissions] ([PermissionCode]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [UserId] int NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [BranchId] int NULL,
        [RoleId] int NOT NULL,
        [Username] nvarchar(50) NOT NULL,
        [PasswordHash] nvarchar(500) NOT NULL,
        [FullName] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
        CONSTRAINT [FK_Users_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([BranchId]),
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Users_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [Transactions] (
        [TransId] bigint NOT NULL IDENTITY,
        [TenantId] int NOT NULL,
        [FundId] int NOT NULL,
        [CategoryId] int NOT NULL,
        [PartnerId] int NULL,
        [DebtId] bigint NULL,
        [TransDate] datetime2 NOT NULL,
        [Amount] decimal(18,0) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [TransType] nvarchar(10) NOT NULL,
        [RefNo] nvarchar(50) NOT NULL,
        [CreatedBy] int NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [TransferRefId] bigint NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([TransId]),
        CONSTRAINT [FK_Transactions_CashFunds_FundId] FOREIGN KEY ([FundId]) REFERENCES [CashFunds] ([FundId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Transactions_Debts_DebtId] FOREIGN KEY ([DebtId]) REFERENCES [Debts] ([DebtId]),
        CONSTRAINT [FK_Transactions_Partners_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [Partners] ([PartnerId]),
        CONSTRAINT [FK_Transactions_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Transactions_TransactionCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [TransactionCategories] ([CategoryId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Transactions_Transactions_TransferRefId] FOREIGN KEY ([TransferRefId]) REFERENCES [Transactions] ([TransId]),
        CONSTRAINT [FK_Transactions_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE TABLE [TransactionAttachments] (
        [AttachmentId] bigint NOT NULL IDENTITY,
        [TransId] bigint NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [FilePath] nvarchar(max) NOT NULL,
        [UploadedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_TransactionAttachments] PRIMARY KEY ([AttachmentId]),
        CONSTRAINT [FK_TransactionAttachments_Transactions_TransId] FOREIGN KEY ([TransId]) REFERENCES [Transactions] ([TransId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Branches_TenantId] ON [Branches] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CashFunds_BranchId] ON [CashFunds] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CashFunds_TenantId] ON [CashFunds] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Debts_BranchId] ON [Debts] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Debts_PartnerId] ON [Debts] ([PartnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Debts_TenantId] ON [Debts] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Partners_TenantId] ON [Partners] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RolePermissions_PermissionCode] ON [RolePermissions] ([PermissionCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Roles_TenantId] ON [Roles] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_TransactionAttachments_TransId] ON [TransactionAttachments] ([TransId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_TransactionCategories_TenantId] ON [TransactionCategories] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_CategoryId] ON [Transactions] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_CreatedBy] ON [Transactions] ([CreatedBy]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_DebtId] ON [Transactions] ([DebtId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_FundId] ON [Transactions] ([FundId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_PartnerId] ON [Transactions] ([PartnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_TenantId] ON [Transactions] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_TransferRefId] ON [Transactions] ([TransferRefId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_BranchId] ON [Users] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_TenantId] ON [Users] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325083946_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325083946_InitialCreate', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325114603_AddIsActiveToPartner'
)
BEGIN
    ALTER TABLE [Partners] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325114603_AddIsActiveToPartner'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325114603_AddIsActiveToPartner', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260327034401_AddIsActiveToCategory'
)
BEGIN
    ALTER TABLE [TransactionCategories] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260327034401_AddIsActiveToCategory'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260327034401_AddIsActiveToCategory', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260328054252_AddIsActiveToTransaction'
)
BEGIN
    ALTER TABLE [Transactions] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260328054252_AddIsActiveToTransaction'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260328054252_AddIsActiveToTransaction', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330065025_AddBranchIdToTransaction'
)
BEGIN

    IF COL_LENGTH('Transactions', 'BranchId') IS NULL
    BEGIN
        ALTER TABLE [Transactions] ADD [BranchId] int NOT NULL CONSTRAINT [DF_Transactions_BranchId] DEFAULT 0;
    END

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330065025_AddBranchIdToTransaction'
)
BEGIN

    IF NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = 'IX_Transactions_BranchId'
          AND object_id = OBJECT_ID('Transactions')
    )
    BEGIN
        CREATE INDEX [IX_Transactions_BranchId] ON [Transactions]([BranchId]);
    END

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330065025_AddBranchIdToTransaction'
)
BEGIN

    IF NOT EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = 'FK_Transactions_Branches_BranchId'
    )
    BEGIN
        ALTER TABLE [Transactions] WITH CHECK
        ADD CONSTRAINT [FK_Transactions_Branches_BranchId]
        FOREIGN KEY([BranchId]) REFERENCES [Branches]([BranchId]) ON DELETE NO ACTION;
    END

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330065025_AddBranchIdToTransaction'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260330065025_AddBranchIdToTransaction', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330074507_AddIsActiveToCashFund'
)
BEGIN

    IF COL_LENGTH('CashFunds', 'IsActive') IS NULL
    BEGIN
        ALTER TABLE [CashFunds] ADD [IsActive] bit NOT NULL CONSTRAINT [DF_CashFunds_IsActive] DEFAULT 0;
    END

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330074507_AddIsActiveToCashFund'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260330074507_AddIsActiveToCashFund', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Tenants_TenantId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'TenantId');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Users] ALTER COLUMN [TenantId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    ALTER TABLE [TransactionCategories] ADD [BranchId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    ALTER TABLE [Partners] ADD [BranchId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN

    UPDATE p
    SET p.BranchId = COALESCE(d.BranchId, bTenant.BranchId, bAny.BranchId)
    FROM Partners p
    OUTER APPLY (
        SELECT TOP 1 d.BranchId
        FROM Debts d
        WHERE d.PartnerId = p.PartnerId AND d.TenantId = p.TenantId
        ORDER BY d.DebtId DESC
    ) d
    OUTER APPLY (
        SELECT TOP 1 b.BranchId
        FROM Branches b
        WHERE b.TenantId = p.TenantId
        ORDER BY b.BranchId
    ) bTenant
    OUTER APPLY (
        SELECT TOP 1 b2.BranchId
        FROM Branches b2
        ORDER BY b2.BranchId
    ) bAny
    WHERE p.BranchId IS NULL;

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN

    UPDATE c
    SET c.BranchId = COALESCE(t.BranchId, bTenant.BranchId, bAny.BranchId)
    FROM TransactionCategories c
    OUTER APPLY (
        SELECT TOP 1 t.BranchId
        FROM Transactions t
        WHERE t.CategoryId = c.CategoryId AND t.TenantId = c.TenantId
        ORDER BY t.TransId DESC
    ) t
    OUTER APPLY (
        SELECT TOP 1 b.BranchId
        FROM Branches b
        WHERE b.TenantId = c.TenantId
        ORDER BY b.BranchId
    ) bTenant
    OUTER APPLY (
        SELECT TOP 1 b2.BranchId
        FROM Branches b2
        ORDER BY b2.BranchId
    ) bAny
    WHERE c.BranchId IS NULL;

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TransactionCategories]') AND [c].[name] = N'BranchId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [TransactionCategories] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [TransactionCategories] ALTER COLUMN [BranchId] int NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Partners]') AND [c].[name] = N'BranchId');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Partners] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Partners] ALTER COLUMN [BranchId] int NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    CREATE INDEX [IX_TransactionCategories_BranchId] ON [TransactionCategories] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    CREATE INDEX [IX_Partners_BranchId] ON [Partners] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    ALTER TABLE [Partners] ADD CONSTRAINT [FK_Partners_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    ALTER TABLE [TransactionCategories] ADD CONSTRAINT [FK_TransactionCategories_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [Tenants] ([TenantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260401020943_AddBranchIdToPartnerAndTransactionCategory'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260401020943_AddBranchIdToPartnerAndTransactionCategory', N'10.0.5');
END;

COMMIT;
GO

