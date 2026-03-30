using System;

namespace QuanLyThuChi_DoAn.BLL.Common
{
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
        public static int? TenantId { get; set; } = null;
        public static int? BranchId { get; set; } = null; // Null nếu là SuperAdmin hệ thống
        public static string BranchName { get; set; } = string.Empty; // Tên chi nhánh để hiển thị

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
        public static string RoleName { get; set; } = string.Empty;

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
            RoleId = 0;
            RoleName = string.Empty;
        }

        // Hỗ trợ legacy: lấy giá trị int với fallback 0
        public static int CurrentTenantIdValue => TenantId ?? 0;
        public static int CurrentBranchIdValue => BranchId ?? 0;
    }
}