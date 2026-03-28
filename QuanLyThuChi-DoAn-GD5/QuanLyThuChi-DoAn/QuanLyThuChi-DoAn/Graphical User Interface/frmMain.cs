using QuanLyThuChi_DoAn.BLL.Common;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin dưới StatusStrip
            // Nếu StatusStrip không có Label cụ thể trong Designer, thêm kiểm tra null
            try
            {
                var lblUserStatus = statusStrip1.Items.Count > 0 ? statusStrip1.Items[0] as ToolStripStatusLabel : null;
                if (lblUserStatus != null)
                {
                    lblUserStatus.Text = $"Người dùng: {SessionManager.FullName} | Chi nhánh: {SessionManager.BranchId ?? 0}";
                    lblUserStatus.ForeColor = Color.Green; // fallback cho ColorLib.Primary
                }
            }
            catch
            {
                // Nếu không có StatusLabel, bỏ qua
            }

            // 2. Thực hiện phân quyền Menu
            ApplyAuthorization();

            // Mặc định hiện màn hình Dashboard (nếu có)
            // ShowUserControl(new ucDashboard());
        }

        // Hàm dùng chung để nạp các màn hình con vào vùng chính
        private void ShowUserControl(UserControl uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

        /// <summary>
        /// Phân quyền hiển thị menu dựa trên role của người dùng
        /// </summary>
        private void ApplyAuthorization()
        {
            string role = SessionManager.RoleName;

            // 🔍 Debug: In ra role hiện tại để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Current Role: {role}");

            // Mặc định: Ẩn tất cả các menu quản lý cao cấp
            mnuManageUsers.Visible = false;
            mnuBranchConfig.Visible = false;
            mnuCashFunds.Visible = false;
            mnuDebtSummary.Visible = true; // Báo cáo mặc định cho tất cả

            // Quyền STAFF (Hạn chế tối đa - chỉ xem báo cáo cá nhân)
            if (role == "Staff")
            {
                mnuManageUsers.Visible = false;
                mnuBranchConfig.Visible = false;
                mnuCashFunds.Visible = false;
                mnuDebtSummary.Visible = false; // Chỉ xem báo cáo cá nhân
            }

            // Quyền MANAGER (Quản lý chi nhánh)
            else if (role == "BranchManager" || role == "Manager")
            {
                mnuManageUsers.Visible = true;   // Quản lý nhân viên trong chi nhánh
                mnuBranchConfig.Visible = true;  // Cấu hình chi nhánh của mình
                mnuCashFunds.Visible = true;
                mnuDebtSummary.Visible = true;
            }

            // Quyền SUPERADMIN (Toàn quyền)
            else if (role == "SuperAdmin")
            {
                // Hiển thị tất cả menu items
                mnuManageUsers.Visible = true;
                mnuBranchConfig.Visible = true;
                mnuCashFunds.Visible = true;
                mnuDebtSummary.Visible = true;
            }
        }

        // Sự kiện khi bấm vào menu Đối tác
        private void menuPartner_Click(object sender, EventArgs e)
        {
            ShowUserControl(new ucPartner());
        }

        private void mnuTransactionCategory_Click(object sender, EventArgs e)
        {
            try
            {
                // Nạp UserControl Quản lý Danh mục Thu/Chi vào khung chính
                ShowUserControl(new ucTransactionCategory());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Danh mục: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                // Nạp UserControl Quản lý Giao dịch (Thu/Chi) vào khung chính
                ShowUserControl(new ucTransaction());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Giao dịch: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            // 1. Xác nhận đăng xuất
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return; // Người dùng nhấn No, không đăng xuất
            }

            try
            {
                // 2. Xóa sạch dữ liệu trong SessionManager
                SessionManager.Logout();

                // 3. Đóng Form hiện tại và hiện lại Form Login
                this.Hide();

                // 4. Khởi tạo và hiển thị Form Login mới
                frmLogin login = new frmLogin();
                login.Show();

                // 5. Giải phóng bộ nhớ của Main Form
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình đăng xuất: {ex.Message}", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
