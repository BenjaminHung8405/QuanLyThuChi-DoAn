using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class UserService
    {
        private readonly BaseRepository<User> _userRepo;

        public UserService(AppDbContext context)
        {
            _userRepo = new BaseRepository<User>(context);
        }

        /// <summary>
        /// Xác thực đăng nhập và nạp dữ liệu vào phiên làm việc
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            // 1. Truy vấn người dùng từ DB (Chỉ lấy User đang hoạt động)
            var user = _userRepo.Find(u => u.Username == username && u.IsActive == true)
                                .FirstOrDefault();

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
            }

            return isValid;
        }

        /// <summary>
        /// Tạo người dùng mới với mật khẩu được mã hóa
        /// </summary>
        public void CreateUser(User newUser, string plainPassword)
        {
            // Thực hiện băm mật khẩu trước khi lưu xuống Database
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            _userRepo.Add(newUser);
            _userRepo.Save();
        }
    }
}