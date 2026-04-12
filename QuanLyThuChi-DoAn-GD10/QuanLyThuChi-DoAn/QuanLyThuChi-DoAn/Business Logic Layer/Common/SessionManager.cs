using System;
using System.Diagnostics;

namespace QuanLyThuChi_DoAn.BLL.Common
{
    public enum UserRole
    {
        SuperAdmin = 1,
        TenantAdmin = 2,
        BranchManager = 3,
        Staff = 4
    }

    /// <summary>
    /// Lưu trữ thông tin phiên làm việc của người dùng hiện tại
    /// </summary>
    public static class SessionManager
    {
        // Thông tin định danh
        public static int UserId { get; set; } = 0;
        public static string Username { get; set; } = string.Empty;
        public static string FullName { get; set; } = string.Empty;

        // Thông tin tổ chức (Cực kỳ quan trọng cho Multi-tenant)
        // Cho phép NULL để phân biệt SuperAdmin (null) và Tenant Manager (có TenantId)
        private static int? _tenantId = null;
        public static int? TenantId
        {
            get => _tenantId;
            set
            {
                _tenantId = value;
                Debug.WriteLine($"==== TENANT ID CHANGED TO: {_tenantId} ====");
            }
        }

        private static int? _branchId = null;
        public static int? BranchId
        {
            get => _branchId;
            set
            {
                _branchId = value;
                Debug.WriteLine($"==== BRANCH ID CHANGED TO: {_branchId} ====");
            }
        } // Null nếu là SuperAdmin hệ thống
        public static string BranchName { get; set; } = string.Empty; // Tên chi nhánh để hiển thị

        // Fixed context (được thiết lập lúc Login) - dùng để "khóa cứng" scope của user
        public static int? FixedTenantId { get; set; } = null;
        public static int? FixedBranchId { get; set; } = null;

        // Ánh xạ Current xịn hơn (tuân thủ naming mới)
        public static int? CurrentTenantId
        {
            get => TenantId;
            set => TenantId = value;
        }

        public static int? CurrentBranchId
        {
            get => BranchId;
            set => BranchId = value;
        }

        // Thông tin phân quyền
        public static int RoleId { get; set; } = 0;
        public static string RoleCode { get; set; } = string.Empty;
        public static string CurrentRoleCode
        {
            get => RoleCode;
            set => RoleCode = value ?? string.Empty;
        }

        // Số càng lớn quyền càng cao: 100=SuperAdmin, 80=TenantAdmin, 50=BranchManager, 10=Staff
        public static int CurrentPriorityLevel { get; set; } = int.MaxValue;
        public static string RoleName { get; set; } = string.Empty;

        // Helper: map RoleId to enum for clearer checks
        public static UserRole RoleEnum
        {
            get => (UserRole)RoleId;
            set => RoleId = (int)value;
        }

        public static bool IsSuperAdmin =>
            RoleId == (int)UserRole.SuperAdmin
            || string.Equals(RoleCode, "SUPERADMIN", StringComparison.OrdinalIgnoreCase)
            || string.Equals(RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        public static bool IsTenantAdmin =>
            RoleId == (int)UserRole.TenantAdmin
            || string.Equals(RoleCode, "TENANTADMIN", StringComparison.OrdinalIgnoreCase)
            || string.Equals(RoleName, "TenantAdmin", StringComparison.OrdinalIgnoreCase)
            || string.Equals(RoleName, "Admin", StringComparison.OrdinalIgnoreCase);

        public static bool IsBranchManager =>
            RoleId == (int)UserRole.BranchManager
            || string.Equals(RoleCode, "BRANCHMANAGER", StringComparison.OrdinalIgnoreCase)
            || string.Equals(RoleName, "BranchManager", StringComparison.OrdinalIgnoreCase)
            || string.Equals(RoleName, "Manager", StringComparison.OrdinalIgnoreCase);

        public static bool IsStaff =>
            RoleId == (int)UserRole.Staff
            || string.Equals(RoleCode, "STAFF", StringComparison.OrdinalIgnoreCase)
            || string.Equals(RoleName, "Staff", StringComparison.OrdinalIgnoreCase);

        public static bool CanChangeTenantContext => IsSuperAdmin;
        public static bool CanChangeBranchContext => IsSuperAdmin || IsTenantAdmin;
        public static bool CanApproveDebt => IsSuperAdmin || IsTenantAdmin || IsBranchManager;
        public static bool CanManageUsers => IsSuperAdmin || IsTenantAdmin;
        public static bool CanManageBranches => IsSuperAdmin || IsTenantAdmin;
        public static bool CanViewSummaryReports => IsSuperAdmin || IsTenantAdmin;
        public static bool CanTransferInterBranch => IsTenantAdmin || IsBranchManager;

        public static int CurrentUserId
        {
            get => UserId;
            set => UserId = value;
        }

        public static string Role
        {
            get => RoleName;
            set => RoleName = value;
        }

        /// <summary>
        /// Kiểm tra xem đã có người dùng đăng nhập hay chưa
        /// </summary>
        public static bool IsLoggedIn => UserId > 0;

        /// <summary>
        /// Xóa sạch thông tin khi đăng xuất
        /// </summary>
        public static void Logout()
        {
            UserId = 0;
            Username = string.Empty;
            FullName = string.Empty;
            TenantId = null;
            BranchId = null;
            FixedTenantId = null;
            FixedBranchId = null;
            RoleId = 0;
            RoleCode = string.Empty;
            CurrentPriorityLevel = int.MaxValue;
            RoleName = string.Empty;
        }

        // Hỗ trợ legacy: lấy giá trị int với fallback 0
        public static int CurrentTenantIdValue => TenantId ?? 0;
        public static int CurrentBranchIdValue => BranchId ?? 0;
    }
}