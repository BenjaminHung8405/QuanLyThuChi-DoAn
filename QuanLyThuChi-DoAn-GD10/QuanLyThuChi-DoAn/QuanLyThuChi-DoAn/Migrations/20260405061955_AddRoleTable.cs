using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyThuChi_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_Roles_Tenants_TenantId'
      AND parent_object_id = OBJECT_ID(N'[dbo].[Roles]')
)
BEGIN
    ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [FK_Roles_Tenants_TenantId];
END;");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Roles_TenantId'
      AND object_id = OBJECT_ID(N'[dbo].[Roles]')
)
BEGIN
    DROP INDEX [IX_Roles_TenantId] ON [dbo].[Roles];
END;");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Roles', 'TenantId') IS NOT NULL
   AND COL_LENGTH('dbo.Roles', 'PriorityLevel') IS NULL
BEGIN
    EXEC sp_rename N'[dbo].[Roles].[TenantId]', N'PriorityLevel', N'COLUMN';
END;");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Roles', 'PriorityLevel') IS NULL
BEGIN
    ALTER TABLE [dbo].[Roles]
    ADD [PriorityLevel] INT NOT NULL
        CONSTRAINT [DF_Roles_PriorityLevel] DEFAULT (3);
END;");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Roles]')
      AND name = N'Description'
      AND is_nullable = 0
)
BEGIN
    ALTER TABLE [dbo].[Roles]
    ALTER COLUMN [Description] NVARCHAR(255) NULL;
END;");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Roles', 'IsActive') IS NULL
BEGIN
    ALTER TABLE [dbo].[Roles]
    ADD [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Roles_IsActive] DEFAULT (1);
END;");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Roles', 'IsActive') IS NOT NULL
BEGIN
    UPDATE [dbo].[Roles]
    SET [IsActive] = 1
    WHERE [IsActive] = 0;
END;");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Roles', 'RoleCode') IS NULL
BEGIN
    ALTER TABLE [dbo].[Roles]
    ADD [RoleCode] NVARCHAR(50) NULL;
END;");

            migrationBuilder.Sql(@"
UPDATE r
SET [PriorityLevel] = CASE
    WHEN UPPER(ISNULL(r.[RoleCode], '')) = 'SUPERADMIN' OR UPPER(ISNULL(r.[RoleName], '')) = 'SUPERADMIN' THEN 0
    WHEN UPPER(ISNULL(r.[RoleCode], '')) = 'TENANTADMIN' OR UPPER(ISNULL(r.[RoleName], '')) IN ('TENANTADMIN', 'ADMIN') THEN 1
    WHEN UPPER(ISNULL(r.[RoleCode], '')) = 'BRANCHMANAGER' OR UPPER(ISNULL(r.[RoleName], '')) = 'BRANCHMANAGER' THEN 2
    WHEN UPPER(ISNULL(r.[RoleCode], '')) = 'STAFF' OR UPPER(ISNULL(r.[RoleName], '')) = 'STAFF' THEN 3
    ELSE 3
END
FROM [dbo].[Roles] r;");

            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [dbo].[Roles] ON;

IF EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleId] = 1)
BEGIN
    UPDATE [dbo].[Roles]
    SET [RoleCode] = 'SUPERADMIN',
        [RoleName] = 'SuperAdmin',
        [Description] = N'Quản trị hệ thống tối cao',
        [PriorityLevel] = 0,
        [IsActive] = 1
    WHERE [RoleId] = 1;
END
ELSE
BEGIN
    INSERT INTO [dbo].[Roles] ([RoleId], [RoleCode], [RoleName], [Description], [PriorityLevel], [IsActive])
    VALUES (1, 'SUPERADMIN', 'SuperAdmin', N'Quản trị hệ thống tối cao', 0, 1);
END;

IF EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleId] = 2)
BEGIN
    UPDATE [dbo].[Roles]
    SET [RoleCode] = 'TENANTADMIN',
        [RoleName] = 'TenantAdmin',
        [Description] = N'Giám đốc công ty',
        [PriorityLevel] = 1,
        [IsActive] = 1
    WHERE [RoleId] = 2;
END
ELSE
BEGIN
    INSERT INTO [dbo].[Roles] ([RoleId], [RoleCode], [RoleName], [Description], [PriorityLevel], [IsActive])
    VALUES (2, 'TENANTADMIN', 'TenantAdmin', N'Giám đốc công ty', 1, 1);
END;

IF EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleId] = 3)
BEGIN
    UPDATE [dbo].[Roles]
    SET [RoleCode] = 'BRANCHMANAGER',
        [RoleName] = 'BranchManager',
        [Description] = N'Quản lý chi nhánh',
        [PriorityLevel] = 2,
        [IsActive] = 1
    WHERE [RoleId] = 3;
END
ELSE
BEGIN
    INSERT INTO [dbo].[Roles] ([RoleId], [RoleCode], [RoleName], [Description], [PriorityLevel], [IsActive])
    VALUES (3, 'BRANCHMANAGER', 'BranchManager', N'Quản lý chi nhánh', 2, 1);
END;

IF EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleId] = 4)
BEGIN
    UPDATE [dbo].[Roles]
    SET [RoleCode] = 'STAFF',
        [RoleName] = 'Staff',
        [Description] = N'Nhân viên',
        [PriorityLevel] = 3,
        [IsActive] = 1
    WHERE [RoleId] = 4;
END
ELSE
BEGIN
    INSERT INTO [dbo].[Roles] ([RoleId], [RoleCode], [RoleName], [Description], [PriorityLevel], [IsActive])
    VALUES (4, 'STAFF', 'Staff', N'Nhân viên', 3, 1);
END;

SET IDENTITY_INSERT [dbo].[Roles] OFF;");

            migrationBuilder.Sql(@"
UPDATE [dbo].[Roles]
SET [RoleCode] = CASE [RoleId]
    WHEN 1 THEN 'SUPERADMIN'
    WHEN 2 THEN 'TENANTADMIN'
    WHEN 3 THEN 'BRANCHMANAGER'
    WHEN 4 THEN 'STAFF'
    ELSE CONCAT('ROLE_', [RoleId])
END
WHERE [RoleCode] IS NULL OR LTRIM(RTRIM([RoleCode])) = '';");

            migrationBuilder.Sql(@"
;WITH DuplicateCodes AS
(
    SELECT [RoleId], [RoleCode],
           ROW_NUMBER() OVER (PARTITION BY [RoleCode] ORDER BY [RoleId]) AS rn
    FROM [dbo].[Roles]
)
UPDATE r
SET [RoleCode] = CONCAT('ROLE_', r.[RoleId])
FROM [dbo].[Roles] r
INNER JOIN DuplicateCodes d ON d.[RoleId] = r.[RoleId]
WHERE d.rn > 1;");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Roles]')
      AND name = N'RoleCode'
      AND is_nullable = 1
)
BEGIN
    ALTER TABLE [dbo].[Roles]
    ALTER COLUMN [RoleCode] NVARCHAR(50) NOT NULL;
END;");

            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Roles_RoleCode'
      AND object_id = OBJECT_ID(N'[dbo].[Roles]')
)
BEGIN
    CREATE UNIQUE INDEX [IX_Roles_RoleCode] ON [dbo].[Roles] ([RoleCode]);
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleCode",
                table: "Roles");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleCode",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "PriorityLevel",
                table: "Roles",
                newName: "TenantId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId",
                table: "Roles",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Tenants_TenantId",
                table: "Roles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
