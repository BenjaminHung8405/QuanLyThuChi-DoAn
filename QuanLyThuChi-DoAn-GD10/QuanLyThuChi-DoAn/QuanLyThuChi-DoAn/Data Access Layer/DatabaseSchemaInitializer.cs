using Microsoft.EntityFrameworkCore;

namespace QuanLyThuChi_DoAn.Data_Access_Layer
{
    public static class DatabaseSchemaInitializer
    {
        public static void EnsureDatabaseReady()
        {
            using var context = new AppDbContext();

            // Apply pending migrations first.
            context.Database.Migrate();

            // Self-heal critical snapshot columns in case migration history is out of sync.
            context.Database.ExecuteSqlRaw(@"
IF COL_LENGTH('dbo.Transactions', 'CategoryNameSnapshot') IS NULL
BEGIN
    ALTER TABLE [dbo].[Transactions]
    ADD [CategoryNameSnapshot] nvarchar(100) NOT NULL
        CONSTRAINT [DF_Transactions_CategoryNameSnapshot] DEFAULT N'';
END;

IF COL_LENGTH('dbo.Transactions', 'PartnerNameSnapshot') IS NULL
BEGIN
    ALTER TABLE [dbo].[Transactions]
    ADD [PartnerNameSnapshot] nvarchar(255) NULL;
END;

IF COL_LENGTH('dbo.Debts', 'PartnerNameSnapshot') IS NULL
BEGIN
    ALTER TABLE [dbo].[Debts]
    ADD [PartnerNameSnapshot] nvarchar(255) NOT NULL
        CONSTRAINT [DF_Debts_PartnerNameSnapshot] DEFAULT N'';
END;
");

            // Backfill existing rows to keep reporting and debt screens stable.
            context.Database.ExecuteSqlRaw(@"
UPDATE t
SET t.[CategoryNameSnapshot] = ISNULL(NULLIF(LTRIM(RTRIM(c.[CategoryName])), N''), N'Khac')
FROM [dbo].[Transactions] t
LEFT JOIN [dbo].[TransactionCategories] c ON c.[CategoryId] = t.[CategoryId]
WHERE ISNULL(NULLIF(LTRIM(RTRIM(t.[CategoryNameSnapshot])), N''), N'') = N'';

UPDATE t
SET t.[PartnerNameSnapshot] = NULLIF(LTRIM(RTRIM(p.[PartnerName])), N'')
FROM [dbo].[Transactions] t
LEFT JOIN [dbo].[Partners] p ON p.[PartnerId] = t.[PartnerId]
WHERE t.[PartnerId] IS NOT NULL
  AND (t.[PartnerNameSnapshot] IS NULL OR LTRIM(RTRIM(t.[PartnerNameSnapshot])) = N'');

UPDATE d
SET d.[PartnerNameSnapshot] = ISNULL(NULLIF(LTRIM(RTRIM(p.[PartnerName])), N''), N'Doi tac khong xac dinh')
FROM [dbo].[Debts] d
LEFT JOIN [dbo].[Partners] p ON p.[PartnerId] = d.[PartnerId]
WHERE ISNULL(NULLIF(LTRIM(RTRIM(d.[PartnerNameSnapshot])), N''), N'') = N'';
");
        }
    }
}