using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionAndDebtSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Transactions', 'CategoryNameSnapshot') IS NULL
BEGIN
    ALTER TABLE [Transactions]
    ADD [CategoryNameSnapshot] nvarchar(100) NOT NULL
        CONSTRAINT [DF_Transactions_CategoryNameSnapshot] DEFAULT N'';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Transactions', 'PartnerNameSnapshot') IS NULL
BEGIN
    ALTER TABLE [Transactions]
    ADD [PartnerNameSnapshot] nvarchar(255) NULL;
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Debts', 'PartnerNameSnapshot') IS NULL
BEGIN
    ALTER TABLE [Debts]
    ADD [PartnerNameSnapshot] nvarchar(255) NOT NULL
        CONSTRAINT [DF_Debts_PartnerNameSnapshot] DEFAULT N'';
END
");

            migrationBuilder.Sql(@"
UPDATE t
SET t.[CategoryNameSnapshot] = ISNULL(NULLIF(LTRIM(RTRIM(c.[CategoryName])), N''), N'Khác')
FROM [Transactions] t
LEFT JOIN [TransactionCategories] c ON c.[CategoryId] = t.[CategoryId]
WHERE ISNULL(NULLIF(LTRIM(RTRIM(t.[CategoryNameSnapshot])), N''), N'') = N'';
");

            migrationBuilder.Sql(@"
UPDATE t
SET t.[PartnerNameSnapshot] = NULLIF(LTRIM(RTRIM(p.[PartnerName])), N'')
FROM [Transactions] t
LEFT JOIN [Partners] p ON p.[PartnerId] = t.[PartnerId]
WHERE t.[PartnerId] IS NOT NULL
  AND (t.[PartnerNameSnapshot] IS NULL OR LTRIM(RTRIM(t.[PartnerNameSnapshot])) = N'');
");

            migrationBuilder.Sql(@"
UPDATE d
SET d.[PartnerNameSnapshot] = ISNULL(NULLIF(LTRIM(RTRIM(p.[PartnerName])), N''), N'Đối tác không xác định')
FROM [Debts] d
LEFT JOIN [Partners] p ON p.[PartnerId] = d.[PartnerId]
WHERE ISNULL(NULLIF(LTRIM(RTRIM(d.[PartnerNameSnapshot])), N''), N'') = N'';
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Transactions', 'CategoryNameSnapshot') IS NOT NULL
BEGIN
    ALTER TABLE [Transactions] DROP CONSTRAINT IF EXISTS [DF_Transactions_CategoryNameSnapshot];
    ALTER TABLE [Transactions] DROP COLUMN [CategoryNameSnapshot];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Transactions', 'PartnerNameSnapshot') IS NOT NULL
BEGIN
    ALTER TABLE [Transactions] DROP COLUMN [PartnerNameSnapshot];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Debts', 'PartnerNameSnapshot') IS NOT NULL
BEGIN
    ALTER TABLE [Debts] DROP CONSTRAINT IF EXISTS [DF_Debts_PartnerNameSnapshot];
    ALTER TABLE [Debts] DROP COLUMN [PartnerNameSnapshot];
END
");
        }
    }
}
