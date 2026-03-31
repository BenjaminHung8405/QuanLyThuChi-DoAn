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
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CashFunds",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CashFunds");
        }
    }
}
