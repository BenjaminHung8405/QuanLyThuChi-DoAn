using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToCashFund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('CashFunds', 'IsActive') IS NULL
BEGIN
    ALTER TABLE [CashFunds] ADD [IsActive] bit NOT NULL CONSTRAINT [DF_CashFunds_IsActive] DEFAULT 0;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('CashFunds', 'IsActive') IS NOT NULL
BEGIN
    ALTER TABLE [CashFunds] DROP COLUMN [IsActive];
END
");
        }
    }
}
