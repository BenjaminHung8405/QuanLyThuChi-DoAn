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
                UpdateUserStatusLabel();
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
                if (SessionManager.TenantId.HasValue)
                {
                    LoadBranchesIntoComboBox(SessionManager.TenantId.Value);
                }
                else
                {
                    cbBranchs.ComboBox.DataSource = null;
                    cbBranchs.ComboBox.Items.Clear();
                }
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
            // Bắt đầu câu truy vấn cơ bản (chỉ lấy những Tenant còn hoạt động)
            var query = _context.Tenants.Where(t => t.IsActive == true);

            // KIỂM TRA QUYỀN TRUY CẬP (AUTHORIZATION)
            if (SessionManager.CurrentTenantId.HasValue)
            {
                // TENANT MANAGER HOẶC NHÂN VIÊN (có TenantId cụ thể)
                int myTenantId = SessionManager.CurrentTenantId.Value;

                // Ép truy vấn chỉ lấy đúng 1 Tenant của họ
                query = query.Where(t => t.TenantId == myTenantId);

                // Khóa ComboBox, không cho thay đổi sang tenant khác
                cbTenants.Enabled = false;
            }
            else
            {
                // SUPERADMIN (CurrentTenantId == null): cho phép chọn tenant
                cbTenants.Enabled = true;
            }

            var tenantList = query.ToList();

            // Ngắt sự kiện để tránh vòng lặp khi gán DataSource
            cbTenants.SelectedIndexChanged -= cbTenants_SelectedIndexChanged;

            cbTenants.ComboBox.DataSource = tenantList;
            cbTenants.ComboBox.DisplayMember = "TenantName";
            cbTenants.ComboBox.ValueMember = "TenantId";

            cbTenants.SelectedIndexChanged += cbTenants_SelectedIndexChanged;

            // Tự động chọn dòng đầu tiên (nếu có)
            if (tenantList.Count > 0)
            {
                cbTenants.SelectedIndex = 0;

                // Cập nhật Session với Tenant hiện tại
                SessionManager.TenantId = (int?)cbTenants.ComboBox.SelectedValue;
                SessionManager.CurrentTenantId = SessionManager.TenantId;

                // Gọi hàm load Branches tương ứng với Tenant vừa chọn
                if (SessionManager.TenantId.HasValue)
                {
                    LoadBranchesIntoComboBox(SessionManager.TenantId.Value);
                }
                else
                {
                    // Nếu không có tenant, dọn sạch branch
                    cbBranchs.ComboBox.DataSource = null;
                    cbBranchs.ComboBox.Items.Clear();
                    SessionManager.BranchId = null;
                    SessionManager.BranchName = string.Empty;
                }
            }
            else
            {
                // Nếu không có tenant, dọn sạch branch
                cbBranchs.ComboBox.DataSource = null;
                cbBranchs.ComboBox.Items.Clear();
                SessionManager.TenantId = null;
                SessionManager.BranchId = null;
                SessionManager.BranchName = string.Empty;
            }

            UpdateUserStatusLabel();
        }

        private void LoadBranchesIntoComboBox(int tenantId)
        {
            var branches = _branchService.GetBranchesByTenant(tenantId);

            cbBranchs.ComboBox.DataSource = null; // Đảm bảo xóa state cũ trước
            cbBranchs.ComboBox.Items.Clear();

            cbBranchs.ComboBox.DataSource = branches;
            cbBranchs.ComboBox.DisplayMember = "BranchName";
            cbBranchs.ComboBox.ValueMember = "BranchId";

            if (branches.Count > 0)
            {
                cbBranchs.SelectedIndex = 0;
                SessionManager.BranchId = (int)cbBranchs.ComboBox.SelectedValue;
                SetBranchNameFromComboBox();
            }
            else
            {
                cbBranchs.SelectedIndex = -1;
                SessionManager.BranchId = null;
                SessionManager.BranchName = string.Empty;
            }

            UpdateUserStatusLabel();
        }

        private async void cbTenants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTenants.ComboBox.SelectedValue == null
                || !int.TryParse(cbTenants.ComboBox.SelectedValue.ToString(), out int newTenantId))
            {
                return;
            }

            // --- 1. Session + UI lock + loading ---
            SessionManager.CurrentTenantId = newTenantId;

            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.Visible = true;

            cbTenants.Enabled = false;
            cbBranchs.Enabled = false;

            // Nếu có thêm text/status label
            // lblStatus.Text = "Đang tải chi nhánh...";

            try
            {
                // --- 2. Async load branch list từ service/data layer ---
                // (Giả sử bạn có async method: GetBranchesByTenantAsync)
                var branches = await Task.Run(() => _branchService.GetBranchesByTenant(newTenantId));

                // --- 3. Reload branch ComboBox safe ---
                cbBranchs.SelectedIndexChanged -= cbBranchs_SelectedIndexChanged;

                cbBranchs.ComboBox.DataSource = branches;
                cbBranchs.ComboBox.DisplayMember = "BranchName";
                cbBranchs.ComboBox.ValueMember = "BranchId";

                if (branches.Any())
                {
                    cbBranchs.ComboBox.SelectedIndex = 0;
                    int selectedBranchId = (int)cbBranchs.ComboBox.SelectedValue;
                    SessionManager.CurrentBranchId = selectedBranchId;
                    SessionManager.BranchId = selectedBranchId; // fallback existing code path
                }
                else
                {
                    cbBranchs.ComboBox.SelectedIndex = -1;
                    SessionManager.CurrentBranchId = 0;
                    SessionManager.BranchId = null;
                }

                // Về UI status text
                // lblStatus.Text = branches.Any() ? "Tải chi nhánh xong" : "Không có chi nhánh";
            }
            catch (Exception ex)
            {
                // Ở block catch chỉ show lỗi, không throw tiếp
                MessageBox.Show($"Lỗi load chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // --- 4. always restore UI state + event hook ---
                cbBranchs.SelectedIndexChanged -= cbBranchs_SelectedIndexChanged;
                cbBranchs.SelectedIndexChanged += cbBranchs_SelectedIndexChanged;

                cbTenants.Enabled = true;
                cbBranchs.Enabled = true;

                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar1.Visible = false;

                // sau cùng, refresh data hiện tại theo branch mới
                RefreshCurrentView();
            }
        }

        private void cbBranchs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBranchs.ComboBox.SelectedValue != null && int.TryParse(cbBranchs.ComboBox.SelectedValue.ToString(), out int newBranchId))
            {
                SessionManager.BranchId = newBranchId;
                SessionManager.CurrentBranchId = newBranchId;
                SetBranchNameFromComboBox();
                RefreshCurrentView();
                UpdateUserStatusLabel();
            }
        }

        public void SetLoadingState(bool isLoading)
        {
            toolStripProgressBar1.Style = isLoading ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            toolStripProgressBar1.Visible = isLoading;
        }

        private void RefreshCurrentView()
        {
            if (pnlContent.Controls.Count == 0) return;

            var current = pnlContent.Controls[0];

            if (current is ucTransaction ucTrans)
            {
                ucTrans.LoadData();
            }
            else if (current is ucCashFund ucFund)
            {
                ucFund.LoadData();
            }
        }

        private void SetBranchNameFromComboBox()
        {
            if (cbBranchs.ComboBox.SelectedItem != null)
            {
                var selectedBranch = cbBranchs.ComboBox.SelectedItem;
                var nameProperty = selectedBranch.GetType().GetProperty("BranchName");

                if (nameProperty != null)
                {
                    SessionManager.BranchName = nameProperty.GetValue(selectedBranch)?.ToString() ?? string.Empty;
                }
                else
                {
                    SessionManager.BranchName = cbBranchs.ComboBox.Text;
                }
            }
            else
            {
                SessionManager.BranchName = string.Empty;
            }
        }

        private void UpdateUserStatusLabel()
        {
            var lblUserStatus = statusStrip1.Items.Count > 0 ? statusStrip1.Items[0] as ToolStripStatusLabel : null;
            if (lblUserStatus == null) return;

            string branchDisplay;

            if (!string.IsNullOrWhiteSpace(SessionManager.BranchName))
            {
                branchDisplay = SessionManager.BranchName;
            }
            else if (cbBranchs.ComboBox.SelectedItem != null)
            {
                branchDisplay = cbBranchs.ComboBox.Text;
            }
            else if (SessionManager.BranchId.HasValue)
            {
                branchDisplay = SessionManager.BranchId.Value.ToString();
            }
            else
            {
                branchDisplay = "Tất cả";
            }

            lblUserStatus.Text = $"Người dùng: {SessionManager.FullName} | Chi nhánh: {branchDisplay}";
            lblUserStatus.ForeColor = Color.Green; // fallback cho ColorLib.Primary
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
