using System;

namespace QuanLyThuChi_DoAn.BLL.Common
{
    /// <summary>
    /// Lưu trữ thông tin phiên làm việc của người dùng hiện tại
    /// </summary>
    public static class SessionManager
    {
        // Thông tin định danh
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }

        // Thông tin tổ chức (Cực kỳ quan trọng cho Multi-tenant)
        public static int TenantId { get; set; }
        public static int? BranchId { get; set; } // Null nếu là SuperAdmin hệ thống

        // Thông tin phân quyền
        public static int RoleId { get; set; }
        public static string RoleName { get; set; }

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
            TenantId = 0;
            BranchId = null;
            RoleId = 0;
            RoleName = string.Empty;
        }
    }
}