using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;
using QuanLyThuChi_DoAn.DTOs;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class UserService
    {
        private readonly BaseRepository<User> _userRepo;
        private readonly AppDbContext _context;

        private const string SuperAdminCode = "SUPERADMIN";
        private const string TenantAdminCode = "TENANTADMIN";
        private const string BranchManagerCode = "BRANCHMANAGER";
        private const string StaffCode = "STAFF";

        public UserService(AppDbContext context)
        {
            _userRepo = new BaseRepository<User>(context);
            _context = context;
        }

        private static void EnsureCanManageUsers()
        {
            if (!SessionManager.IsSuperAdmin && !SessionManager.IsTenantAdmin)
            {
                throw new UnauthorizedAccessException("Chỉ SuperAdmin hoặc Giám đốc mới có quyền quản lý người dùng.");
            }
        }

        private static string NormalizeRoleCode(string? roleCode)
        {
            return (roleCode ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static bool IsBranchScopedRole(string? roleCode)
        {
            string normalizedCode = NormalizeRoleCode(roleCode);
            return normalizedCode == BranchManagerCode || normalizedCode == StaffCode;
        }

        private static string MapRoleDisplayName(string? roleCode, string? roleName)
        {
            string normalizedCode = NormalizeRoleCode(roleCode);
            if (normalizedCode == SuperAdminCode)
            {
                return "Quản trị hệ thống";
            }

            if (normalizedCode == TenantAdminCode)
            {
                return "Giám đốc";
            }

            if (normalizedCode == BranchManagerCode)
            {
                return "Quản lý chi nhánh";
            }

            if (normalizedCode == StaffCode)
            {
                return "Nhân viên";
            }

            if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return "Giám đốc";
            }

            return string.IsNullOrWhiteSpace(roleName) ? "N/A" : roleName;
        }

        private static int ResolveTenantScope(int? requestedTenantId)
        {
            if (SessionManager.IsSuperAdmin)
            {
                int superAdminTenantId = requestedTenantId ?? SessionManager.CurrentTenantId ?? 0;
                if (superAdminTenantId <= 0)
                {
                    throw new InvalidOperationException("Vui lòng chọn Tenant hợp lệ trước khi quản lý người dùng.");
                }

                return superAdminTenantId;
            }

            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                throw new InvalidOperationException("Không xác định được Tenant hiện tại. Vui lòng đăng nhập lại.");
            }

            if (requestedTenantId.HasValue && requestedTenantId.Value > 0 && requestedTenantId.Value != SessionManager.CurrentTenantId.Value)
            {
                throw new UnauthorizedAccessException("Bạn chỉ được quản lý tài khoản thuộc công ty của mình.");
            }

            return SessionManager.CurrentTenantId.Value;
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
                            r.RoleCode,
                            r.PriorityLevel,
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
                            int roleCodeOrdinal = reader.GetOrdinal("RoleCode");
                            int priorityLevelOrdinal = reader.GetOrdinal("PriorityLevel");
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
                            string roleCode = reader.IsDBNull(roleCodeOrdinal) ? string.Empty : reader.GetString(roleCodeOrdinal);
                            int priorityLevel = reader.IsDBNull(priorityLevelOrdinal) ? -1 : reader.GetInt32(priorityLevelOrdinal);
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
                                    SessionManager.CurrentRoleCode = roleCode ?? string.Empty;
                                    SessionManager.RoleName = roleName ?? "Unknown";

                                    // Warn if DB has inconsistent roleId/roleName
                                    if ((string.Equals(roleCode, SuperAdminCode, StringComparison.OrdinalIgnoreCase)
                                         || string.Equals(roleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                                         && !SessionManager.IsSuperAdmin)
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

        // 1. LẤY DANH SÁCH NGƯỜI DÙNG THEO TENANT
        public async Task<List<UserDTO>> GetUsersByTenantAsync(int tenantId)
        {
            EnsureCanManageUsers();

            int effectiveTenantId = ResolveTenantScope(tenantId);

            var users = await _context.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Branch)
                .Where(u => u.TenantId == effectiveTenantId)
                .OrderBy(u => u.FullName)
                .ThenBy(u => u.Username)
                .Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    RoleCode = u.Role != null ? u.Role.RoleCode : null,
                    RoleName = u.Role != null ? u.Role.RoleName : null,
                    BranchName = u.Branch != null ? u.Branch.BranchName : null,
                    u.IsActive
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                FullName = u.FullName ?? string.Empty,
                Username = u.Username ?? string.Empty,
                RoleName = MapRoleDisplayName(u.RoleCode, u.RoleName),
                BranchName = string.IsNullOrWhiteSpace(u.BranchName) ? "Toàn hệ thống" : u.BranchName,
                IsActive = u.IsActive,
                CreatedDate = DateTime.Now
            }).ToList();
        }

        public async Task<List<Role>> GetAssignableRolesByTenantAsync(int tenantId)
        {
            EnsureCanManageUsers();

            _ = ResolveTenantScope(tenantId);

            int fallbackPriority = SessionManager.IsSuperAdmin ? int.MaxValue : -1;
            int currentRolePriority = await _context.Roles
                .AsNoTracking()
                .Where(r => r.RoleId == SessionManager.RoleId)
                .Select(r => (int?)r.PriorityLevel)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false) ?? fallbackPriority;

            var query = _context.Roles
                .AsNoTracking()
                .Where(r => r.IsActive && r.PriorityLevel < currentRolePriority);

            return await query
                .OrderByDescending(r => r.PriorityLevel)
                .ThenBy(r => r.RoleName)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Branch>> GetActiveBranchesByTenantAsync(int tenantId)
        {
            EnsureCanManageUsers();

            int effectiveTenantId = ResolveTenantScope(tenantId);

            return await _context.Branches
                .AsNoTracking()
                .Where(b => b.TenantId == effectiveTenantId && b.IsActive)
                .OrderBy(b => b.BranchName)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<User?> GetUserByIdAsync(int userId, int tenantId)
        {
            EnsureCanManageUsers();

            if (userId <= 0)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ.", nameof(userId));
            }

            int effectiveTenantId = ResolveTenantScope(tenantId);

            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId && u.TenantId == effectiveTenantId)
                .ConfigureAwait(false);
        }

        public async Task<bool> UpdateUserAsync(User user, string? rawPassword = null)
        {
            EnsureCanManageUsers();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.UserId <= 0)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException("Họ tên không được để trống.");
            }

            if (user.RoleId <= 0)
            {
                throw new ArgumentException("Vai trò không hợp lệ.");
            }

            var existingUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == user.UserId)
                .ConfigureAwait(false);

            if (existingUser == null)
            {
                throw new InvalidOperationException("Không tìm thấy người dùng.");
            }

            int effectiveTenantId = ResolveTenantScope(existingUser.TenantId);
            if (existingUser.TenantId != effectiveTenantId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật tài khoản của Tenant khác.");
            }

            var targetRole = existingUser.Role
                ?? await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.RoleId == existingUser.RoleId)
                    .ConfigureAwait(false);

            if (targetRole == null)
            {
                throw new InvalidOperationException("Vai trò hiện tại của người dùng không hợp lệ.");
            }

            var desiredRole = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleId == user.RoleId && r.IsActive)
                .ConfigureAwait(false);

            if (desiredRole == null)
            {
                throw new InvalidOperationException("Vai trò cập nhật không hợp lệ hoặc đã ngừng hoạt động.");
            }

            int fallbackPriority = SessionManager.IsSuperAdmin ? int.MaxValue : -1;
            int editorPriority = await _context.Roles
                .AsNoTracking()
                .Where(r => r.RoleId == SessionManager.RoleId)
                .Select(r => (int?)r.PriorityLevel)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false) ?? fallbackPriority;

            bool isUpdatingSelf = existingUser.UserId == SessionManager.CurrentUserId;
            if (!isUpdatingSelf && targetRole.PriorityLevel >= editorPriority)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa tài khoản có cấp bậc ngang hoặc cao hơn bạn.");
            }

            if (desiredRole.PriorityLevel >= editorPriority)
            {
                throw new UnauthorizedAccessException("Bạn không thể gán một vai trò có quyền ngang hoặc cao hơn quyền hiện tại.");
            }

            bool requiresBranch = IsBranchScopedRole(desiredRole.RoleCode);
            int? newBranchId = user.BranchId;

            if (requiresBranch && (!newBranchId.HasValue || newBranchId.Value <= 0))
            {
                throw new InvalidOperationException("Vai trò này bắt buộc phải gắn với một chi nhánh.");
            }

            if (newBranchId.HasValue)
            {
                bool isBranchValid = await _context.Branches
                    .AsNoTracking()
                    .AnyAsync(b => b.BranchId == newBranchId.Value && b.TenantId == effectiveTenantId && b.IsActive)
                    .ConfigureAwait(false);

                if (!isBranchValid)
                {
                    throw new InvalidOperationException("Chi nhánh không hợp lệ hoặc không thuộc Tenant hiện tại.");
                }
            }

            if (!requiresBranch)
            {
                newBranchId = null;
            }

            string normalizedFullName = user.FullName.Trim();
            bool shouldUpdatePassword = !string.IsNullOrWhiteSpace(rawPassword);
            bool hasChanges = !string.Equals(existingUser.FullName, normalizedFullName, StringComparison.Ordinal)
                              || existingUser.RoleId != desiredRole.RoleId
                              || existingUser.BranchId != newBranchId
                              || shouldUpdatePassword;

            if (!hasChanges)
            {
                return true;
            }

            existingUser.TenantId = effectiveTenantId;
            existingUser.FullName = normalizedFullName;
            existingUser.RoleId = desiredRole.RoleId;
            existingUser.BranchId = newBranchId;

            if (shouldUpdatePassword)
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword!);
            }

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        // 2. TẠO TÀI KHOẢN MỚI
        public async Task<bool> CreateUserAsync(User newUser, string rawPassword)
        {
            EnsureCanManageUsers();

            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser));
            }

            if (string.IsNullOrWhiteSpace(newUser.FullName))
            {
                throw new ArgumentException("Họ tên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(newUser.Username))
            {
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(rawPassword))
            {
                throw new ArgumentException("Mật khẩu không được để trống.");
            }

            int effectiveTenantId = ResolveTenantScope(newUser.TenantId);

            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleId == newUser.RoleId && r.IsActive)
                .ConfigureAwait(false);

            if (role == null)
            {
                throw new InvalidOperationException("Vai trò không hợp lệ hoặc đã ngừng hoạt động.");
            }

            int fallbackPriority = SessionManager.IsSuperAdmin ? int.MaxValue : -1;
            int creatorPriority = await _context.Roles
                .AsNoTracking()
                .Where(r => r.RoleId == SessionManager.RoleId)
                .Select(r => (int?)r.PriorityLevel)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false) ?? fallbackPriority;

            if (role.PriorityLevel >= creatorPriority)
            {
                throw new UnauthorizedAccessException("Bạn không thể tạo tài khoản có quyền ngang hoặc cao hơn quyền hiện tại.");
            }

            bool requiresBranch = IsBranchScopedRole(role.RoleCode);
            if (requiresBranch && !newUser.BranchId.HasValue)
            {
                throw new InvalidOperationException("Vai trò này bắt buộc phải gắn với một chi nhánh.");
            }

            if (newUser.BranchId.HasValue)
            {
                bool isBranchValid = await _context.Branches
                    .AsNoTracking()
                    .AnyAsync(b => b.BranchId == newUser.BranchId.Value && b.TenantId == effectiveTenantId && b.IsActive)
                    .ConfigureAwait(false);

                if (!isBranchValid)
                {
                    throw new InvalidOperationException("Chi nhánh không hợp lệ hoặc không thuộc Tenant hiện tại.");
                }
            }

            bool isExist = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.TenantId == effectiveTenantId && u.Username == newUser.Username)
                .ConfigureAwait(false);

            if (isExist)
            {
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại trong công ty này.");
            }

            newUser.TenantId = effectiveTenantId;
            newUser.FullName = newUser.FullName.Trim();
            newUser.Username = newUser.Username.Trim();
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);
            newUser.IsActive = true;

            if (!requiresBranch)
            {
                newUser.BranchId = null;
            }

            _context.Users.Add(newUser);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        // 3. KHÓA/MỞ KHÓA TÀI KHOẢN (SOFT DELETE)
        public async Task<bool> ToggleUserStatusAsync(int userId)
        {
            EnsureCanManageUsers();

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId)
                .ConfigureAwait(false);

            if (user == null)
            {
                throw new InvalidOperationException("Không tìm thấy người dùng.");
            }

            int effectiveTenantId = ResolveTenantScope(user.TenantId);
            if (user.TenantId != effectiveTenantId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thay đổi tài khoản của Tenant khác.");
            }

            if (user.UserId == SessionManager.CurrentUserId)
            {
                throw new InvalidOperationException("Không thể tự khóa tài khoản đang đăng nhập.");
            }

            if (!SessionManager.IsSuperAdmin
                && user.Role != null
                && (string.Equals(user.Role.RoleCode, SuperAdminCode, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(user.Role.RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase)))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thay đổi trạng thái của SuperAdmin.");
            }

            user.IsActive = !user.IsActive;
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        /// <summary>
        /// Tạo người dùng mới với mật khẩu được mã hóa
        /// </summary>
        public void CreateUser(User newUser, string plainPassword)
        {
            CreateUserAsync(newUser, plainPassword).GetAwaiter().GetResult();
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
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                throw new InvalidOperationException("Không tìm thấy người dùng!");
            }

            if (userId == SessionManager.UserId)
            {
                throw new InvalidOperationException("Không thể tự khóa chính mình!");
            }

            int effectiveTenantId = ResolveTenantScope(user.TenantId);
            if (user.TenantId != effectiveTenantId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền khóa tài khoản ở Tenant khác.");
            }

            user.IsActive = false;
            _context.SaveChanges();
        }
    }
}