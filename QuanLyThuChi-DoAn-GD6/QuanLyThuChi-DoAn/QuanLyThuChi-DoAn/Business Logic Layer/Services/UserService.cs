using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class UserService
    {
        private readonly BaseRepository<User> _userRepo;
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _userRepo = new BaseRepository<User>(context);
            _context = context;
        }

        /// <summary>
        /// Xác thực đăng nhập và nạp dữ liệu vào phiên làm việc
        /// Sử dụng ADO.NET với IsDBNull checks để tránh lỗi "Data is Null"
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return false;
                }

                // Get connection string from DbContext
                string connectionString = _context.Database.GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL Query with JOIN to get Role and Branch information
                    string query = @"
                        SELECT TOP 1
                            u.UserId,
                            u.TenantId,
                            u.BranchId,
                            u.RoleId,
                            u.Username,
                            u.PasswordHash,
                            u.FullName,
                            u.IsActive,
                            r.RoleName,
                            b.BranchName
                        FROM Users u
                        INNER JOIN Roles r ON u.RoleId = r.RoleId
                        LEFT JOIN Branches b ON u.BranchId = b.BranchId
                        WHERE u.Username = @username AND u.IsActive = 1
                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return false; // User not found or inactive
                            }

                            // 🛡️ Get column ordinals for efficient access
                            int userIdOrdinal = reader.GetOrdinal("UserId");
                            int tenantIdOrdinal = reader.GetOrdinal("TenantId");
                            int branchIdOrdinal = reader.GetOrdinal("BranchId");
                            int roleIdOrdinal = reader.GetOrdinal("RoleId");
                            int usernameOrdinal = reader.GetOrdinal("Username");
                            int passwordHashOrdinal = reader.GetOrdinal("PasswordHash");
                            int fullNameOrdinal = reader.GetOrdinal("FullName");
                            int roleNameOrdinal = reader.GetOrdinal("RoleName");
                            int branchNameOrdinal = reader.GetOrdinal("BranchName");

                            // 🛡️ Read values safely using IsDBNull checks
                            int userId = reader.GetInt32(userIdOrdinal);
                            int tenantId = reader.IsDBNull(tenantIdOrdinal) ? 0 : reader.GetInt32(tenantIdOrdinal);
                            int? branchId = reader.IsDBNull(branchIdOrdinal) ? (int?)null : reader.GetInt32(branchIdOrdinal);
                            int roleId = reader.GetInt32(roleIdOrdinal);
                            string usernameTrim = reader.IsDBNull(usernameOrdinal) ? string.Empty : reader.GetString(usernameOrdinal);
                            string passwordHash = reader.IsDBNull(passwordHashOrdinal) ? string.Empty : reader.GetString(passwordHashOrdinal);
                            string fullName = reader.IsDBNull(fullNameOrdinal) ? string.Empty : reader.GetString(fullNameOrdinal);
                            string roleName = reader.IsDBNull(roleNameOrdinal) ? "Unknown" : reader.GetString(roleNameOrdinal);
                            string branchName = reader.IsDBNull(branchNameOrdinal) ? string.Empty : reader.GetString(branchNameOrdinal);

                            // Validate password hash exists
                            if (string.IsNullOrWhiteSpace(passwordHash))
                            {
                                System.Diagnostics.Debug.WriteLine($"[AUTH] User '{username}' has no password hash set.");
                                return false;
                            }

                            // Verify password using BCrypt
                            try
                            {
                                bool isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);

                                if (isValid)
                                {
                                    // Debug: show values retrieved from DB before assigning to SessionManager
                                    System.Diagnostics.Debug.WriteLine($"[AUTH-DBG] DB values -> tenantId={tenantId}, branchId={(branchId.HasValue ? branchId.Value.ToString() : "null")}, roleId={roleId}, roleName={roleName}");

                                    // ✅ Authentication successful - populate SessionManager with safe values
                                    SessionManager.UserId = userId;
                                    SessionManager.Username = usernameTrim ?? string.Empty;
                                    SessionManager.FullName = fullName ?? string.Empty;

                                    // Set role info first and treat numeric roleId as authoritative
                                    SessionManager.RoleId = roleId;
                                    SessionManager.RoleName = roleName ?? "Unknown";

                                    // Warn if DB has inconsistent roleId/roleName
                                    if (roleName == "SuperAdmin" && !SessionManager.IsSuperAdmin)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[AUTH-WARN] RoleName says SuperAdmin but roleId={roleId} != SuperAdmin. Using roleId as source-of-truth.");
                                    }

                                    // SuperAdmin không thuộc tenant nào trong ngữ cảnh (null), còn user khác phải xác định tenant rõ.
                                    if (SessionManager.IsSuperAdmin)
                                    {
                                        SessionManager.TenantId = null;
                                        SessionManager.BranchId = null;
                                        SessionManager.BranchName = string.Empty;
                                        SessionManager.FixedTenantId = null;
                                        SessionManager.FixedBranchId = null;
                                        SessionManager.CurrentTenantId = null;
                                        SessionManager.CurrentBranchId = null;
                                    }
                                    else
                                    {
                                        if (tenantId <= 0)
                                        {
                                            throw new InvalidOperationException("Tài khoản chưa được gán Tenant hợp lệ.");
                                        }

                                        SessionManager.TenantId = tenantId;

                                        if (SessionManager.IsTenantAdmin)
                                        {
                                            SessionManager.BranchId = null;
                                            SessionManager.BranchName = "Tất cả chi nhánh";
                                            SessionManager.FixedTenantId = tenantId;
                                            SessionManager.FixedBranchId = null;
                                            SessionManager.CurrentTenantId = tenantId;
                                            SessionManager.CurrentBranchId = null;
                                        }
                                        else if (SessionManager.IsBranchManager || SessionManager.IsStaff)
                                        {
                                            if (!branchId.HasValue || branchId.Value <= 0)
                                            {
                                                throw new InvalidOperationException("Tài khoản chưa được gán Chi nhánh hợp lệ.");
                                            }

                                            SessionManager.BranchId = branchId.Value;
                                            SessionManager.BranchName = branchName ?? string.Empty;
                                            SessionManager.FixedTenantId = tenantId;
                                            SessionManager.FixedBranchId = branchId.Value;
                                            SessionManager.CurrentTenantId = tenantId;
                                            SessionManager.CurrentBranchId = branchId.Value;
                                        }
                                        else
                                        {
                                            SessionManager.BranchId = branchId;
                                            SessionManager.BranchName = branchName ?? string.Empty;
                                            SessionManager.FixedTenantId = tenantId;
                                            SessionManager.FixedBranchId = branchId;
                                            SessionManager.CurrentTenantId = tenantId;
                                            SessionManager.CurrentBranchId = branchId;
                                        }
                                    }

                                    System.Diagnostics.Debug.WriteLine($"[AUTH] ✅ User '{username}' authenticated successfully. Role: {SessionManager.RoleName}, Branch: {branchName ?? "None"}");
                                }

                                return isValid;
                            }
                            catch (ArgumentException argEx)
                            {
                                // BCrypt verification failed - corrupted hash
                                System.Diagnostics.Debug.WriteLine($"[AUTH] ❌ BCrypt verification failed: {argEx.Message}");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                System.Diagnostics.Debug.WriteLine($"[AUTH] ❌ SQL Error: {sqlEx.Number} - {sqlEx.Message}");
                throw new InvalidOperationException($"Database error during authentication: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AUTH] ❌ Authentication error: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tạo người dùng mới với mật khẩu được mã hóa
        /// </summary>
        public void CreateUser(User newUser, string plainPassword)
        {
            // 🔐 Deep Security: Kiểm tra quyền tạo người dùng
            // Chỉ SuperAdmin hoặc BranchManager (của chi nhánh tương ứng) mới có quyền
            if (SessionManager.RoleName != "SuperAdmin" && SessionManager.RoleName != "BranchManager")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo người dùng!");
            }

            // Kiểm tra: BranchManager chỉ được tạo người dùng cho chi nhánh của họ
            if (SessionManager.RoleName == "BranchManager" && newUser.BranchId != SessionManager.BranchId)
            {
                throw new UnauthorizedAccessException("Bạn chỉ có quyền tạo người dùng cho chi nhánh của mình!");
            }

            // Thực hiện băm mật khẩu trước khi lưu xuống Database
            if (!SessionManager.TenantId.HasValue)
                throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            newUser.TenantId = SessionManager.TenantId.Value; // Ép buộc theo Tenant hiện tại

            _userRepo.Add(newUser);
            _userRepo.Save();
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        public void UpdateUser(User user)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được sửa thông tin người dùng khác
            if (SessionManager.RoleName != "SuperAdmin" && SessionManager.UserId != user.UserId)
            {
                throw new UnauthorizedAccessException("Bạn chỉ có quyền sửa thông tin của chính mình!");
            }

            if (!SessionManager.TenantId.HasValue)
                throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

            user.TenantId = SessionManager.TenantId.Value;
            _userRepo.Update(user);
            _userRepo.Save();
        }

        /// <summary>
        /// Xóa người dùng (Soft Delete hoặc Hard Delete)
        /// </summary>
        public void DeleteUser(int userId)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được xóa người dùng
            if (SessionManager.RoleName != "SuperAdmin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa người dùng!");
            }

            // Không cho phép xóa chính mình
            if (userId == SessionManager.UserId)
            {
                throw new InvalidOperationException("Không thể xóa chính mình!");
            }

            _userRepo.Delete(userId);
            _userRepo.Save();
        }
    }
}