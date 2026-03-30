using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    public partial class frmMain : Form
    {
        private readonly AppDbContext _context;
        private readonly TenantService _tenantService;
        private readonly BranchService _branchService;

        public frmMain()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _tenantService = new TenantService(_context);
            _branchService = new BranchService(_context);
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
                    var branchDisplay = string.IsNullOrWhiteSpace(SessionManager.BranchName)
                        ? (SessionManager.BranchId.HasValue ? SessionManager.BranchId.ToString() : "Tất cả")
                        : SessionManager.BranchName;

                    lblUserStatus.Text = $"Người dùng: {SessionManager.FullName} | Chi nhánh: {branchDisplay}";
                    lblUserStatus.ForeColor = Color.Green; // fallback cho ColorLib.Primary
                }
            }
            catch
            {
                // Nếu không có StatusLabel, bỏ qua
            }

            // 2. Thực hiện phân quyền Menu
            ApplyAuthorization();

            // 2.1 Setup combo box ngữ cảnh (Tenant / Branch)
            SetupContextComboBoxes();

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
            mnuCashFunds.Visible = true;
            mnuDebtSummary.Visible = true; // Báo cáo mặc định cho tất cả

            // Quyền STAFF (Hạn chế tối đa - chỉ xem báo cáo cá nhân)
            if (role == "Staff")
            {
                mnuManageUsers.Visible = false;
                mnuBranchConfig.Visible = false;
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

        private void mnuCashFunds_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucCashFund());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Quỹ tiền: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupContextComboBoxes()
        {
            // Gỡ sự kiện để tránh lỗi vòng lặp khi gán DataSource
            cbTenants.SelectedIndexChanged -= cbTenants_SelectedIndexChanged;
            cbBranchs.SelectedIndexChanged -= cbBranchs_SelectedIndexChanged;

            string roleName = SessionManager.RoleName;

            if (roleName == "Staff")
            {
                cbTenants.Visible = false;
                cbBranchs.Visible = false;
            }
            else if (roleName == "BranchManager")
            {
                cbTenants.Visible = false;
                cbBranchs.Visible = true;
                LoadBranchesIntoComboBox(SessionManager.TenantId);
            }
            else if (roleName == "SuperAdmin" || roleName == "Admin")
            {
                cbTenants.Visible = true;
                cbBranchs.Visible = true;
                LoadTenantsIntoComboBox();
            }
            else
            {
                // Fallback: mặc định dạng SuperAdmin cho an toàn nếu role không xác định
                cbTenants.Visible = true;
                cbBranchs.Visible = true;
                LoadTenantsIntoComboBox();
            }

            cbTenants.SelectedIndexChanged += cbTenants_SelectedIndexChanged;
            cbBranchs.SelectedIndexChanged += cbBranchs_SelectedIndexChanged;

            // Mặc định chọn Tenant[0] và Branch[0] nếu chưa chọn
            if (cbTenants.ComboBox.Items.Count > 0 && cbTenants.ComboBox.SelectedIndex < 0)
            {
                cbTenants.ComboBox.SelectedIndex = 0;
                SessionManager.TenantId = (int)cbTenants.ComboBox.SelectedValue;
            }

            if (cbBranchs.ComboBox.Items.Count > 0 && cbBranchs.ComboBox.SelectedIndex < 0)
            {
                cbBranchs.ComboBox.SelectedIndex = 0;
                SessionManager.BranchId = (int)cbBranchs.ComboBox.SelectedValue;
            }
        }

        private void LoadTenantsIntoComboBox()
        {
            var tenants = _tenantService.GetAllTenants();

            cbTenants.ComboBox.DataSource = tenants;
            cbTenants.ComboBox.DisplayMember = "TenantName";
            cbTenants.ComboBox.ValueMember = "TenantId";

            if (tenants.Count > 0)
            {
                if (SessionManager.TenantId == 0)
                {
                    cbTenants.SelectedIndex = 0;
                    SessionManager.TenantId = (int)cbTenants.ComboBox.SelectedValue;
                }
                else
                {
                    cbTenants.ComboBox.SelectedValue = SessionManager.TenantId;
                }

                LoadBranchesIntoComboBox(SessionManager.TenantId);
            }
        }

        private void LoadBranchesIntoComboBox(int tenantId)
        {
            var branches = _branchService.GetBranchesByTenant(tenantId);

            cbBranchs.ComboBox.DataSource = branches;
            cbBranchs.ComboBox.DisplayMember = "BranchName";
            cbBranchs.ComboBox.ValueMember = "BranchId";

            if (branches.Count > 0)
            {
                cbBranchs.SelectedIndex = 0;
                SessionManager.BranchId = (int)cbBranchs.ComboBox.SelectedValue;
            }
            else
            {
                cbBranchs.SelectedIndex = -1;
            }
        }

        private void cbTenants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTenants.ComboBox.SelectedValue != null && int.TryParse(cbTenants.ComboBox.SelectedValue.ToString(), out int newTenantId))
            {
                SessionManager.TenantId = newTenantId;
                LoadBranchesIntoComboBox(newTenantId);
                RefreshCurrentView();
            }
        }

        private void cbBranchs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBranchs.ComboBox.SelectedValue != null && int.TryParse(cbBranchs.ComboBox.SelectedValue.ToString(), out int newBranchId))
            {
                SessionManager.BranchId = newBranchId;
                RefreshCurrentView();
            }
        }

        private void RefreshCurrentView()
        {
            if (pnlContent.Controls.Count == 0) return;

            var current = pnlContent.Controls[0];

            if (current is ucTransaction ucTrans)
            {
                ucTrans.RefreshDataGrid();
            }
            else if (current is ucCashFund ucFund)
            {
                ucFund.LoadFunds();
            }
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
