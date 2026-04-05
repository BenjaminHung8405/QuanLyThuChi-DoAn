using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedDateToBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Branches', 'CreatedDate') IS NULL
BEGIN
    ALTER TABLE [Branches]
    ADD [CreatedDate] datetime2 NOT NULL
        CONSTRAINT [DF_Branches_CreatedDate] DEFAULT (GETDATE());
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Branches', 'CreatedDate') IS NOT NULL
BEGIN
    DECLARE @constraintName NVARCHAR(128);

    SELECT @constraintName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Branches' AND c.name = 'CreatedDate';

    IF @constraintName IS NOT NULL
        EXEC(N'ALTER TABLE [Branches] DROP CONSTRAINT [' + @constraintName + N']');

    ALTER TABLE [Branches] DROP COLUMN [CreatedDate];
END
");
        }
    }
}
