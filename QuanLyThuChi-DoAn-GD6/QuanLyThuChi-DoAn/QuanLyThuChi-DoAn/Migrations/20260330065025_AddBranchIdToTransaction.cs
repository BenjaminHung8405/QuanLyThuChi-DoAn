using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchIdToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Transactions', 'BranchId') IS NULL
BEGIN
    ALTER TABLE [Transactions] ADD [BranchId] int NOT NULL CONSTRAINT [DF_Transactions_BranchId] DEFAULT 0;
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Transactions_BranchId'
      AND object_id = OBJECT_ID('Transactions')
)
BEGIN
    CREATE INDEX [IX_Transactions_BranchId] ON [Transactions]([BranchId]);
END
");

            migrationBuilder.Sql(@"
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
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Transactions_Branches_BranchId'
)
BEGIN
    ALTER TABLE [Transactions] DROP CONSTRAINT [FK_Transactions_Branches_BranchId];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Transactions_BranchId'
      AND object_id = OBJECT_ID('Transactions')
)
BEGIN
    DROP INDEX [IX_Transactions_BranchId] ON [Transactions];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Transactions', 'BranchId') IS NOT NULL
BEGIN
    ALTER TABLE [Transactions] DROP COLUMN [BranchId];
END
");
        }
    }
}
