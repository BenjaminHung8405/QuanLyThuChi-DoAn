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
                mnuDashboard.Click += mnuDashboard_Click;
                mnuCashLedger.Click += mnuCashLedger_Click;
                mnuReconciliation.Click += mnuReconciliation_Click;
                mnuManageUsers.Click += mnuManageUsers_Click;
                mnuBranchConfig.Click += mnuBranchConfig_Click;
                mnuTaxes.Click += mnuTaxes_Click;
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

            // 3. Điều hướng về trang chủ dựa trên vai trò người dùng
            NavigateToHomeByRole();
        }

        // Hàm dùng chung để nạp các màn hình con vào vùng chính
        private void ShowUserControl(UserControl uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

        /// <summary>
        /// Điều hướng tự động đến trang chủ phù hợp dựa trên vai trò của người dùng.
        /// 
        /// 🧠 UX Logic:
        /// - Admin/TenantAdmin: Mặc định mở Dashboard để phân tích, ra quyết định.
        /// - Staff/BranchManager: Mặc định mở màn hình giao dịch để nhập liệu nhanh.
        /// </summary>
        private void NavigateToHomeByRole()
        {
            try
            {
                // Quản lý (Admin / TenantAdmin) → Dashboard
                if (SessionManager.IsSuperAdmin || SessionManager.IsTenantAdmin)
                {
                    ShowUserControl(new ucDashboard());
                }
                // Nhân viên (Staff / BranchManager) → Giao dịch (Transaction)
                else if (SessionManager.IsStaff || SessionManager.IsBranchManager)
                {
                    ShowUserControl(new ucTransaction());
                }
                // Fallback (nếu có role mới thêm vào sau)
                else
                {
                    ShowUserControl(new ucTransaction());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi điều hướng trang chủ: {ex.Message}");
                // Không hiển thị MessageBox ở đây vì đây là trang khởi động
            }
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
            mnuTaxes.Visible = SessionManager.IsSuperAdmin || SessionManager.IsTenantAdmin;
            mnuDashboard.Visible = SessionManager.CanViewSummaryReports;
            mnuReconciliation.Visible = SessionManager.CanViewSummaryReports;
            mnuInternalTransfer.Visible = SessionManager.CanTransferInterBranch;
            mnuDebtManagement.Visible = SessionManager.CanApproveDebt;

            string currentRoleCode = (SessionManager.CurrentRoleCode ?? string.Empty).Trim().ToUpperInvariant();
            if (currentRoleCode == "STAFF")
            {
                mnuReports.Visible = false;
                mnuManageUsers.Visible = false;
            }
            else if (currentRoleCode == "BRANCHMANAGER")
            {
                mnuReconciliation.Visible = false;
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

        private void mnuDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucDashboard());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Tổng quan: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuCashLedger_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucCashbookReport());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Sổ quỹ chi tiết: {ex.Message}", "Lỗi hệ thống",
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

        private void mnuTaxes_Click(object? sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucTaxManagement());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Thuế suất: {ex.Message}", "Lỗi hệ thống",
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

        private void mnuReconciliation_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucReconciliation());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Đối soát chuỗi: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuManageUsers_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUserControl(new ucUserManagement());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Quản lý người dùng: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuBranchConfig_Click(object sender, EventArgs e)
        {
            try
            {
                var branchManagement = new ucBranchManagement();
                branchManagement.BranchChanged += BranchManagement_BranchChanged;
                ShowUserControl(branchManagement);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở màn hình Quản lý chi nhánh: {ex.Message}", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BranchManagement_BranchChanged(object? sender, ucBranchManagement.BranchChangedEventArgs e)
        {
            try
            {
                await RefreshBranchComboAfterBranchChangedAsync(e.TenantId, e.BranchId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể đồng bộ danh sách chi nhánh: {ex.Message}", "Lỗi hệ thống",
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

            if (SessionManager.IsSuperAdmin || SessionManager.IsTenantAdmin)
            {
                branches.Insert(0, new Branch
                {
                    BranchId = 0,
                    BranchName = "--- Tất cả chi nhánh ---",
                    TenantId = tenantId,
                    IsActive = true
                });
            }

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
                    if (SessionManager.IsSuperAdmin || SessionManager.IsTenantAdmin)
                    {
                        cbBranchs.ComboBox.SelectedValue = 0;
                    }
                    else
                    {
                        cbBranchs.SelectedIndex = 0;
                    }
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

        private async Task RefreshBranchComboAfterBranchChangedAsync(int tenantId, int? preferredBranchId)
        {
            if (!SessionManager.CurrentTenantId.HasValue)
            {
                return;
            }

            if (SessionManager.CurrentTenantId.Value != tenantId)
            {
                return;
            }

            if (SessionManager.IsStaff)
            {
                return;
            }

            if (SessionManager.FixedBranchId.HasValue && SessionManager.FixedBranchId.Value > 0)
            {
                return;
            }

            _isInitializing = true;
            cbBranchs.SelectedIndexChanged -= cbBranchs_SelectedIndexChanged;
            try
            {
                var branches = await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    var service = new BranchService(context);
                    return service.GetBranchesByTenant(tenantId);
                });

                if (SessionManager.IsSuperAdmin || SessionManager.IsTenantAdmin)
                {
                    branches.Insert(0, new Branch
                    {
                        BranchId = 0,
                        BranchName = "--- Tất cả chi nhánh ---",
                        TenantId = tenantId,
                        IsActive = true
                    });
                }

                cbBranchs.ComboBox.DataSource = null;
                cbBranchs.ComboBox.Items.Clear();
                cbBranchs.ComboBox.DataSource = branches;
                cbBranchs.ComboBox.DisplayMember = "BranchName";
                cbBranchs.ComboBox.ValueMember = "BranchId";

                if (branches.Count == 0)
                {
                    cbBranchs.ComboBox.SelectedIndex = -1;
                    SessionManager.CurrentBranchId = null;
                    SessionManager.BranchId = null;
                    SessionManager.BranchName = string.Empty;
                    UpdateUserStatusLabel();
                    return;
                }

                int targetBranchId;
                if (preferredBranchId.HasValue && branches.Any(b => b.BranchId == preferredBranchId.Value))
                {
                    targetBranchId = preferredBranchId.Value;
                }
                else if (SessionManager.CurrentBranchId.HasValue && branches.Any(b => b.BranchId == SessionManager.CurrentBranchId.Value))
                {
                    targetBranchId = SessionManager.CurrentBranchId.Value;
                }
                else if ((SessionManager.IsSuperAdmin || SessionManager.IsTenantAdmin) && branches.Any(b => b.BranchId == 0))
                {
                    targetBranchId = 0;
                }
                else
                {
                    targetBranchId = branches[0].BranchId;
                }

                cbBranchs.ComboBox.SelectedValue = targetBranchId;
                SessionManager.CurrentBranchId = targetBranchId;
                SessionManager.BranchId = targetBranchId;
                SetBranchNameFromComboBox();
                UpdateUserStatusLabel();
            }
            finally
            {
                cbBranchs.SelectedIndexChanged += cbBranchs_SelectedIndexChanged;
                _isInitializing = false;
            }
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
                    ucTrans.ReloadContextAndData();
                }
                else if (current is ucCashFund ucFund)
                {
                    ucFund.LoadData();
                }
                else if (current is ucDebt ucDebtView)
                {
                    await ucDebtView.LoadDebtDataAsync();
                }
                else if (current is ucDashboard ucDashboardView)
                {
                    await ucDashboardView.LoadReportDataAsync();
                }
                else if (current is ucCashbookReport ucCashbookView)
                {
                    await ucCashbookView.ReloadDataAsync();
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

        private void mnuChangePassword_Click(object sender, EventArgs e)
        {

        }

        private void mnuCashLedger_Click_1(object sender, EventArgs e)
        {

        }
    }
}
