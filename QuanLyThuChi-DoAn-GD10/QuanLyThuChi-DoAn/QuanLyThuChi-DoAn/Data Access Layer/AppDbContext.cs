using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLyThuChi_DoAn.Data_Access_Layer
{
    public class AppDbContext : DbContext
    {
        // Khai báo 14 bảng vào EF Core
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CashFund> CashFunds { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public override int SaveChanges()
        {
            var deletedAuditLogs = ChangeTracker.Entries<AuditLog>()
                                                .Where(e => e.State == EntityState.Deleted);
            if (deletedAuditLogs.Any())
            {
                throw new InvalidOperationException("🔥 CẢNH BÁO BẢO MẬT: Nỗ lực rà phá Audit Log bị chặn đứng! Không thể xóa nhật ký.");
            }
            return base.SaveChanges();
        }

        public override async System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            var deletedAuditLogs = ChangeTracker.Entries<AuditLog>()
                                                .Where(e => e.State == EntityState.Deleted);
            if (deletedAuditLogs.Any())
            {
                throw new InvalidOperationException("🔥 CẢNH BÁO BẢO MẬT: Nỗ lực rà phá Audit Log bị chặn đứng! Không thể xóa nhật ký.");
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Thay "TEN_MAY_TINH_CUA_BAN" bằng tên Server SQL của bạn (ví dụ: .\SQLEXPRESS)
                string connectionString = @"Server=.\SQLEXPRESS;Database=CashFlowDB;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleCode)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role
                    {
                        RoleId = 1,
                        RoleCode = "SUPERADMIN",
                        RoleName = "SuperAdmin",
                        Description = "Quản trị hệ thống tối cao",
                        PriorityLevel = 100,
                        IsActive = true
                    },
                    new Role
                    {
                        RoleId = 2,
                        RoleCode = "TENANTADMIN",
                        RoleName = "TenantAdmin",
                        Description = "Giám đốc công ty",
                        PriorityLevel = 80,
                        IsActive = true
                    },
                    new Role
                    {
                        RoleId = 3,
                        RoleCode = "BRANCHMANAGER",
                        RoleName = "BranchManager",
                        Description = "Quản lý chi nhánh",
                        PriorityLevel = 50,
                        IsActive = true
                    },
                    new Role
                    {
                        RoleId = 4,
                        RoleCode = "STAFF",
                        RoleName = "Staff",
                        Description = "Nhân viên",
                        PriorityLevel = 10,
                        IsActive = true
                    });

            // 1. Cấu hình Khóa chính kết hợp (Composite Key) cho bảng RolePermissions
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionCode });

            // 2. Chống lỗi Cascade Delete (Ngăn xóa dây chuyền gây vòng lặp)
            // Lệnh này chuyển toàn bộ các Khóa ngoại (Foreign Keys) sang chế độ Restrict
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }


        }
    }
}