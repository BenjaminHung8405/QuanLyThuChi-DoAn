using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxesAndTransactionTaxBreakdown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "Transactions",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "Transactions",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TaxId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE [Transactions]
                SET [SubTotal] = [Amount],
                    [TaxAmount] = 0
                WHERE [SubTotal] = 0");

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TaxName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.TaxId);
                    table.ForeignKey(
                        name: "FK_Taxes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TaxId",
                table: "Transactions",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_TenantId",
                table: "Taxes",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Taxes_TaxId",
                table: "Transactions",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "TaxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Taxes_TaxId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TaxId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Transactions");
        }
    }
}
