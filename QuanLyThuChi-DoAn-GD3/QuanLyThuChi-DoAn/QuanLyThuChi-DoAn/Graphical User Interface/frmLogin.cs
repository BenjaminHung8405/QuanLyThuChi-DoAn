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
            var context = new AppDbContext();
            _userService = new UserService(context);
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
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
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
