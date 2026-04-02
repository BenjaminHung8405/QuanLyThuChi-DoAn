using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchIdToPartnerAndTransactionCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "TransactionCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Partners",
                type: "int",
                nullable: true);

            // Backfill Partner.BranchId from Debts, then fallback to first branch in tenant.
            migrationBuilder.Sql(@"
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
");

            // Backfill TransactionCategory.BranchId from Transactions, then fallback to first branch in tenant.
            migrationBuilder.Sql(@"
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
");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "TransactionCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Partners",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategories_BranchId",
                table: "TransactionCategories",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_BranchId",
                table: "Partners",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Branches_BranchId",
                table: "Partners",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategories_Branches_BranchId",
                table: "TransactionCategories",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Branches_BranchId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategories_Branches_BranchId",
                table: "TransactionCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategories_BranchId",
                table: "TransactionCategories");

            migrationBuilder.DropIndex(
                name: "IX_Partners_BranchId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "TransactionCategories");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Partners");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
