using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;
using Microsoft.EntityFrameworkCore;

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
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            // 1. Truy vấn người dùng từ DB (Include Role để lấy RoleName)
            var user = _context.Users
                               .Include(u => u.Role)
                               .FirstOrDefault(u => u.Username == username && u.IsActive == true);

            if (user == null) return false;

            // 2. So khớp mật khẩu nhập vào với chuỗi Hash lưu trong DB
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (isValid)
            {
                // 3. Nếu khớp, thiết lập thông tin vào SessionManager để sử dụng toàn app
                SessionManager.UserId = user.UserId;
                SessionManager.Username = user.Username;
                SessionManager.FullName = user.FullName;
                SessionManager.TenantId = user.TenantId;
                SessionManager.BranchId = user.BranchId;
                SessionManager.RoleId = user.RoleId;
                SessionManager.RoleName = user.Role?.RoleName ?? "Unknown"; // 🔧 FIX: Set RoleName từ Role entity
            }

            return isValid;
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
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            newUser.TenantId = SessionManager.TenantId; // Ép buộc theo Tenant hiện tại

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

            user.TenantId = SessionManager.TenantId;
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