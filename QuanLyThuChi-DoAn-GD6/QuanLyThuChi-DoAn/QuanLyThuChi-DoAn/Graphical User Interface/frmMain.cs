using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Graphical_User_Interface;

namespace QuanLyThuChi_DoAn
{
    public partial class frmMain : Form
    {
        private readonly AppDbContext _context;
        private readonly TenantService _tenantService;
        private readonly BranchService _branchService;
        private bool _isInitializing = false; // Flag to guard SelectedIndexChanged during data-binding

        public frmMain()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _tenantService = new TenantService(_context);
            _branchService = new BranchService(_context);
            // Wire debt management menu to load the ucDebt user control
            try
            {
                mnuDebtManagement.Click += mnuDebtManagement_Click;
            }
            catch
            {
                // If the menu item isn't present in this build, ignore silently
            }
        }

        private async void frmMain_Load(object sender, EventArgs e)
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
            await SetupContextComboBoxes();

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
            System.Diagnostics.Debug.WriteLine($"Current Role: {SessionManager.RoleName} ({SessionManager.RoleId})");

            mnuManageUsers.Visible = SessionManager.CanManageUsers;
            mnuBranchConfig.Visible = SessionManager.CanManageBranches;
            mnuCashFunds.Visible = true;
            mnuDebtSummary.Visible = SessionManager.CanViewSummaryReports;
            mnuInternalTransfer.Visible = SessionManager.CanTransferInterBranch;
            mnuDebtManagement.Visible = SessionManager.CanApproveDebt;
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

        private void mnuInternalTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                using var transferForm = new frmFundTransfer();
                transferForm.StartPosition = FormStartPosition.CenterParent;

                var dialogResult = transferForm.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    // Nếu chuyển quỹ thành công, làm mới giao diện hiện tại (transactions và quỹ)
                    RefreshCurrentView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Chuyển quỹ nội bộ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void mnuDebtManagement_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucDebt());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Khoản nợ: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SetupContextComboBoxes()
        {
            _isInitializing = true;
            try
            {
                cbTenants.SelectedIndexChanged -= cbTenants_SelectedIndexChanged;
                cbBranchs.SelectedIndexChanged -= cbBranchs_SelectedIndexChanged;

                if (SessionManager.IsStaff)
                {
                    cbTenants.Visible = false;
                    cbBranchs.Visible = false;
                }
                else if (SessionManager.IsBranchManager)
                {
                    cbTenants.Visible = false;
                    cbBranchs.Visible = true;

                    if (SessionManager.CurrentTenantId.HasValue)
                    {
                        LoadBranchesIntoComboBox(SessionManager.CurrentTenantId.Value);

                        if (SessionManager.FixedBranchId.HasValue)
                        {
                            cbBranchs.ComboBox.SelectedValue = SessionManager.FixedBranchId.Value;
                            SessionManager.CurrentBranchId = SessionManager.FixedBranchId.Value;
                            SessionManager.BranchId = SessionManager.FixedBranchId.Value;
                            SetBranchNameFromComboBox();
                        }
                    }
                    else
                    {
                        cbBranchs.ComboBox.DataSource = null;
                        cbBranchs.ComboBox.Items.Clear();
                    }

                    cbBranchs.Enabled = false;
                }
                else if (SessionManager.IsTenantAdmin)
                {
                    cbTenants.Visible = false;
                    cbBranchs.Visible = true;

                    if (!SessionManager.CurrentTenantId.HasValue)
                    {
                        throw new InvalidOperationException("Tài khoản TenantAdmin chưa có Tenant hợp lệ.");
                    }

                    LoadBranchesIntoComboBox(SessionManager.CurrentTenantId.Value);
                    cbBranchs.Enabled = true;
                }
                else
                {
                    cbTenants.Visible = true;
                    cbBranchs.Visible = true;
                    await LoadTenantsIntoComboBox();
                    cbTenants.Enabled = SessionManager.CanChangeTenantContext;
                    cbBranchs.Enabled = SessionManager.CanChangeBranchContext;
                }

                cbTenants.SelectedIndexChanged += cbTenants_SelectedIndexChanged;
                cbBranchs.SelectedIndexChanged += cbBranchs_SelectedIndexChanged;
            }
            finally
            {
                _isInitializing = false;
            }
        }

        private async Task LoadTenantsIntoComboBox()
        {
            _isInitializing = true; // Bật cờ chặn: đang nạp dữ liệu
            try
            {
                // Lấy danh sách tenants từ service trong một background thread
                var tenants = await Task.Run(() => _tenantService.GetActiveTenants());

                // Nếu user có FixedTenant thì chỉ hiển thị tenant đó
                int? effectiveTenant = SessionManager.FixedTenantId ?? SessionManager.TenantId;
                if (effectiveTenant.HasValue && effectiveTenant.Value > 0)
                {
                    tenants = tenants.Where(t => t.TenantId == effectiveTenant.Value).ToList();
                    cbTenants.Enabled = false; // khóa cứng UI
                }
                else
                {
                    cbTenants.Enabled = SessionManager.IsSuperAdmin;
                }

                // Gán DataSource an toàn
                cbTenants.SelectedIndexChanged -= cbTenants_SelectedIndexChanged;
                cbTenants.ComboBox.DataSource = tenants;
                cbTenants.ComboBox.DisplayMember = "TenantName";
                cbTenants.ComboBox.ValueMember = "TenantId";

                // CỰC KỲ QUAN TRỌNG: Gán lại đúng ID từ Session sau khi nạp xong danh sách
                if (SessionManager.TenantId.HasValue)
                {
                    cbTenants.ComboBox.SelectedValue = SessionManager.TenantId.Value;
                }
                else if (tenants.Count > 0)
                {
                    cbTenants.ComboBox.SelectedIndex = 0;
                    SessionManager.TenantId = (int?)cbTenants.ComboBox.SelectedValue;
                }

                // Load branches tương ứng
                if (SessionManager.TenantId.HasValue)
                {
                    LoadBranchesIntoComboBox(SessionManager.TenantId.Value);
                }

                UpdateUserStatusLabel();
            }
            finally
            {
                cbTenants.SelectedIndexChanged += cbTenants_SelectedIndexChanged;
                _isInitializing = false; // Mở khóa
            }
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
                if (SessionManager.FixedBranchId.HasValue)
                {
                    cbBranchs.ComboBox.SelectedValue = SessionManager.FixedBranchId.Value;
                }
                else if (SessionManager.CurrentBranchId.HasValue)
                {
                    cbBranchs.ComboBox.SelectedValue = SessionManager.CurrentBranchId.Value;
                }
                else
                {
                    cbBranchs.SelectedIndex = 0;
                }

                if (cbBranchs.ComboBox.SelectedValue is int selectedBranchId)
                {
                    SessionManager.BranchId = selectedBranchId;
                    SessionManager.CurrentBranchId = selectedBranchId;
                }

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
            if (_isInitializing) return;

            // Only SuperAdmin may change tenant at runtime
            if (!SessionManager.IsSuperAdmin)
            {
                // Revert selection to current tenant to prevent client-side tampering
                _isInitializing = true;
                try
                {
                    if (SessionManager.CurrentTenantId.HasValue)
                        cbTenants.ComboBox.SelectedValue = SessionManager.CurrentTenantId.Value;
                }
                finally { _isInitializing = false; }
                return;
            }

            if (!(cbTenants.ComboBox.SelectedValue is int newTenantId))
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
                SetLoadingState(true);

                // --- 2. Async load branch list từ service/data layer ---
                // Create a fresh AppDbContext inside the background task so we don't use
                // the UI-thread DbContext across threads (DbContext is NOT thread-safe).
                var branches = await Task.Run(() =>
                {
                    using var bgContext = new AppDbContext();
                    var bgBranchService = new BranchService(bgContext);
                    return bgBranchService.GetBranchesByTenant(newTenantId);
                });

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

                // Re-enable controls according to role-based rules
                cbTenants.Enabled = SessionManager.CanChangeTenantContext;
                cbBranchs.Enabled = SessionManager.CanChangeBranchContext && !SessionManager.FixedBranchId.HasValue;

                SetLoadingState(false);

                // sau cùng, refresh data hiện tại theo branch mới
                RefreshCurrentView();
            }
        }

        private void cbBranchs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            if (cbBranchs.ComboBox.SelectedValue is int newBranchId)
            {
                // If the user has a FixedBranchId, prevent changing it from UI
                if (SessionManager.FixedBranchId.HasValue && SessionManager.FixedBranchId.Value > 0 && SessionManager.FixedBranchId.Value != newBranchId)
                {
                    _isInitializing = true;
                    try
                    {
                        if (SessionManager.CurrentBranchId > 0)
                            cbBranchs.ComboBox.SelectedValue = SessionManager.CurrentBranchId;
                    }
                    finally { _isInitializing = false; }
                    return;
                }

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
            panelLoadingOverlay.Visible = isLoading;
            if (isLoading)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Start a determinate progress for long-running UI population tasks
        public void StartProgress(int maximum)
        {
            if (maximum <= 0) maximum = 1;
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar1.Maximum = maximum;
            toolStripProgressBar1.Step = 1;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Visible = true;
            panelLoadingOverlay.Visible = true;
            this.Cursor = Cursors.WaitCursor;
        }

        // Increment the determinate progress by step (default 1)
        public void IncrementProgress(int step = 1)
        {
            try
            {
                if (!toolStripProgressBar1.Visible) return;
                int newVal = Math.Min(toolStripProgressBar1.Maximum, toolStripProgressBar1.Value + step);
                toolStripProgressBar1.Value = newVal;
            }
            catch { }
        }

        // Stop and reset determinate progress
        public void StopProgress()
        {
            toolStripProgressBar1.Visible = false;
            panelLoadingOverlay.Visible = false;
            try { toolStripProgressBar1.Value = 0; } catch { }
            this.Cursor = Cursors.Default;
        }

        private async void RefreshCurrentView()
        {
            if (pnlContent.Controls.Count == 0) return;

            var current = pnlContent.Controls[0];

            try
            {
                SetLoadingState(true);

                if (current is ucTransaction ucTrans)
                {
                    ucTrans.LoadData();
                }
                else if (current is ucCashFund ucFund)
                {
                    ucFund.LoadData();
                }
                else
                {
                    // Extend: other UserControls with LoadData() can be added here
                }
            }
            finally
            {
                SetLoadingState(false);
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
