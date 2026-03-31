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
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BranchId",
                table: "Transactions",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Branches_BranchId",
                table: "Transactions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Branches_BranchId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BranchId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Transactions");
        }
    }
}
