using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn
{
    public partial class frmLogin : Form
    {
        private readonly UserService _userService;

        public frmLogin()
        {
            InitializeComponent();
            // Khởi tạo Service để sử dụng
            try
            {
                var context = new AppDbContext();
                // Test database connection safely
                try
                {
                    if (!context.Database.CanConnect())
                    {
                        MessageBox.Show(
                            "Không thể kết nối đến cơ sở dữ liệu.\nVui lòng kiểm tra:\n1. SQL Server đang chạy?\n2. Tên server: .\\SQLEXPRESS\n3. Database CashFlowDB tồn tại?",
                            "Lỗi kết nối DB",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
                catch (Exception dbEx)
                {
                    MessageBox.Show(
                        $"Lỗi kiểm tra kết nối: {dbEx.Message}\n\nKiểm tra:\n1. SQL Server có chạy không?\n2. Connection string đúng không?",
                        "Lỗi DB Connection",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                _userService = new UserService(context);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khởi tạo ứng dụng:\n{ex.GetType().Name}: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            AcceptButton = btnLogin; // Nhấn Enter sẽ kích hoạt btnLogin
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 1. Kiểm tra nhập liệu cơ bản
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Check if UserService is properly initialized
                if (_userService == null)
                {
                    MessageBox.Show(
                        "Lỗi: UserService chưa được khởi tạo.\nVui lòng khởi động lại ứng dụng.",
                        "Lỗi hệ thống",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop
                    );
                    return;
                }

                // 2. Gọi tầng BLL để xác thực
                bool isSuccess = _userService.Authenticate(username, password);

                if (isSuccess)
                {
                    // 3. Lưu "Remember Me" nếu người dùng chọn
                    SaveRememberMe(username);

                    // 4. Đăng nhập thành công
                    this.Hide();
                    frmMain main = new frmMain();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (NullReferenceException nex)
            {
                MessageBox.Show(
                    $"Lỗi null reference: {nex.Message}\n\nNhật ký:" + Environment.NewLine + GetDetailedErrorInfo(nex),
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi hệ thống: {ex.GetType().Name}" + Environment.NewLine + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
            }
        }

        private string GetDetailedErrorInfo(Exception ex)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Type: {ex.GetType().FullName}");
            sb.AppendLine($"Message: {ex.Message}");
            if (ex.InnerException != null)
                sb.AppendLine($"Inner: {ex.InnerException.Message}");
            return sb.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isRemembered)
            {
                txtUsername.Text = Properties.Settings.Default.userSaved;
                chkRememberMe.Checked = true;
                txtPassword.Focus(); // Con trỏ chuột nhảy vào ô mật khẩu luôn
            }
        }

        /// <summary>
        /// Lưu thông tin "Remember Me" vào Settings
        /// </summary>
        private void SaveRememberMe(string username)
        {
            if (chkRememberMe.Checked)
            {
                Properties.Settings.Default.userSaved = username;
                Properties.Settings.Default.isRemembered = true;
            }
            else
            {
                Properties.Settings.Default.userSaved = "";
                Properties.Settings.Default.isRemembered = false;
            }

            // Quan trọng: Phải Save để ghi vào file cấu hình
            Properties.Settings.Default.Save();
        }
    }
}
