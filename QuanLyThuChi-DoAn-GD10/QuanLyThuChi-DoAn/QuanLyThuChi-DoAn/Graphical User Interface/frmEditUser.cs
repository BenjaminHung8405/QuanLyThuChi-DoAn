using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    public partial class frmEditUser : Form
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly BranchService _branchService;
        private bool _isSaving;
        private bool _isFormReady;
        private bool _allowCloseWhenSaving;
        private int _userId;

        private sealed class RoleOption
        {
            public int RoleId { get; set; }
            public string RoleCode { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public bool RequiresBranch { get; set; }
        }

        public frmEditUser(int userId)
        {
            InitializeComponent();

            _userId = userId;
            _context = new AppDbContext();
            _userService = new UserService(_context);
            _branchService = new BranchService(_context);

            Load += frmEditUser_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
            cbBranch.SelectedIndexChanged += (_, __) => UpdateSaveButtonState();
            txtFullName.TextChanged += (_, __) => UpdateSaveButtonState();
            txtPassword.TextChanged += (_, __) => UpdateSaveButtonState();
            txtFullName.PlaceholderText = "Ví dụ: Nguyễn Văn A";
            txtPassword.PlaceholderText = "Để trống để giữ nguyên mật khẩu hiện tại";
            FormClosing += frmEditUser_FormClosing;
            FormClosed += frmEditUser_FormClosed;

            btnSave.Enabled = false;
        }

        private void frmEditUser_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isSaving && !_allowCloseWhenSaving)
            {
                e.Cancel = true;
            }
        }

        private void frmEditUser_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _context.Dispose();
        }

        private async void frmEditUser_Load(object? sender, EventArgs e)
        {
            UseWaitCursor = true;
            try
            {
                await LoadComboBoxDataAsync();
                bool userLoaded = await LoadUserDataAsync();
                _isFormReady = userLoaded;
                UpdateSaveButtonState();

                if (userLoaded)
                {
                    txtFullName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
            }
            finally
            {
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                }
            }
        }

        private async Task<bool> LoadUserDataAsync()
        {
            try
            {
                int tenantId = GetTenantIdOrThrow();
                var user = await _userService.GetUserByIdAsync(_userId, tenantId);

                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                    return false;
                }

                txtFullName.Text = user.FullName ?? string.Empty;
                txtUsername.Text = user.Username ?? string.Empty;
                txtUsername.ReadOnly = true;

                if (user.BranchId.HasValue)
                {
                    cbBranch.SelectedValue = user.BranchId.Value;
                }

                if (user.RoleId > 0)
                {
                    cbRole.SelectedValue = user.RoleId;
                }

                lblPassword.Text = "Mật khẩu mới (để trống nếu không thay đổi)";
                txtPassword.PlaceholderText = "Để trống để giữ nguyên mật khẩu hiện tại";
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
        }

        private static string NormalizeWhitespace(string? value)
        {
            return string.Join(" ", (value ?? string.Empty)
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }

        private int GetTenantIdOrThrow()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                throw new InvalidOperationException("Không xác định được Tenant hiện tại. Vui lòng đăng nhập lại.");
            }

            return SessionManager.CurrentTenantId.Value;
        }

        private static string NormalizeRoleCode(string? roleCode)
        {
            return (roleCode ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static bool IsBranchScopedRole(string? roleCode)
        {
            string normalizedCode = NormalizeRoleCode(roleCode);
            return normalizedCode == "BRANCHMANAGER" || normalizedCode == "STAFF";
        }

        private static string MapRoleDisplayName(string? roleCode, string? roleName)
        {
            string normalizedCode = NormalizeRoleCode(roleCode);
            if (normalizedCode == "SUPERADMIN")
            {
                return "Quản trị hệ thống";
            }

            if (normalizedCode == "TENANTADMIN")
            {
                return "Giám đốc";
            }

            if (normalizedCode == "BRANCHMANAGER")
            {
                return "Quản lý chi nhánh";
            }

            if (normalizedCode == "STAFF")
            {
                return "Nhân viên thu ngân";
            }

            return string.IsNullOrWhiteSpace(roleName) ? "N/A" : roleName;
        }

        private async Task LoadComboBoxDataAsync()
        {
            int tenantId = GetTenantIdOrThrow();

            var branches = await _branchService.GetBranchesByTenantAsync(tenantId);
            cbBranch.DataSource = branches;
            cbBranch.DisplayMember = nameof(Branch.BranchName);
            cbBranch.ValueMember = nameof(Branch.BranchId);
            
            if (SessionManager.IsBranchManager && SessionManager.CurrentBranchId.HasValue)
            {
                cbBranch.SelectedValue = SessionManager.CurrentBranchId.Value;
                cbBranch.Enabled = false;
            }
            else
            {
                cbBranch.SelectedIndex = branches.Count > 0 ? 0 : -1;
            }

            int currentPriority = SessionManager.CurrentPriorityLevel;
            var roles = await _userService.GetAssignableRolesByTenantAsync(tenantId);
            var roleOptions = roles
                .Where(r => r.PriorityLevel < currentPriority)
                .Select(r => new RoleOption
                {
                    RoleId = r.RoleId,
                    RoleCode = r.RoleCode ?? string.Empty,
                    DisplayName = MapRoleDisplayName(r.RoleCode, r.RoleName),
                    RequiresBranch = IsBranchScopedRole(r.RoleCode)
                })
                .ToList();

            cbRole.DataSource = roleOptions;
            cbRole.DisplayMember = nameof(RoleOption.DisplayName);
            cbRole.ValueMember = nameof(RoleOption.RoleId);
            cbRole.SelectedIndex = roleOptions.Count > 0 ? 0 : -1;

            UpdateBranchStateByRole();

            if (roleOptions.Count == 0)
            {
                MessageBox.Show("Không có vai trò hợp lệ để gán cho người dùng.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSave.Enabled = false;
            }
        }

        private void cbRole_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateBranchStateByRole();
        }

        private void UpdateBranchStateByRole()
        {
            var selectedRole = cbRole.SelectedItem as RoleOption;
            bool requiresBranch = selectedRole?.RequiresBranch ?? true;

            cbBranch.Enabled = requiresBranch && !SessionManager.IsBranchManager;
            lblBranch.Text = requiresBranch ? "Chi nhánh (*)" : "Chi nhánh";

            if (!requiresBranch)
            {
                cbBranch.SelectedIndex = -1;
            }
            else if (cbBranch.SelectedIndex < 0 && cbBranch.Items.Count > 0)
            {
                cbBranch.SelectedIndex = 0;
            }

            UpdateSaveButtonState();
        }

        private static bool TryGetSelectedInt(ComboBox comboBox, out int selectedValue)
        {
            selectedValue = 0;

            if (comboBox.SelectedValue is int intValue)
            {
                selectedValue = intValue;
                return true;
            }

            return int.TryParse(comboBox.SelectedValue?.ToString(), out selectedValue);
        }

        private bool ValidateInput(
            out string fullName,
            out string? newPassword,
            out int? branchId,
            out RoleOption? selectedRole)
        {
            fullName = NormalizeWhitespace(txtFullName.Text);
            newPassword = string.IsNullOrWhiteSpace(txtPassword.Text) ? null : txtPassword.Text;
            selectedRole = cbRole.SelectedItem as RoleOption;
            branchId = null;

            txtFullName.Text = fullName;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Vui lòng nhập Họ tên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(newPassword) && newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (selectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbRole.Focus();
                return false;
            }

            if (selectedRole.RequiresBranch)
            {
                if (!TryGetSelectedInt(cbBranch, out int selectedBranchId) || selectedBranchId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbBranch.Focus();
                    return false;
                }

                branchId = selectedBranchId;
            }

            return true;
        }

        private void UpdateSaveButtonState()
        {
            if (!_isFormReady || _isSaving)
            {
                btnSave.Enabled = false;
                return;
            }

            bool hasFullName = !string.IsNullOrWhiteSpace(txtFullName.Text);
            RoleOption? selectedRole = cbRole.SelectedItem as RoleOption;
            bool hasRole = selectedRole != null;
            bool requiresBranch = selectedRole?.RequiresBranch ?? true;
            bool hasBranch = !requiresBranch || (TryGetSelectedInt(cbBranch, out int branchId) && branchId > 0);

            btnSave.Enabled = hasFullName && hasRole && hasBranch;
        }

        private void SetSavingState(bool isSaving)
        {
            _isSaving = isSaving;
            if (!isSaving)
            {
                _allowCloseWhenSaving = false;
            }

            btnSave.Enabled = !isSaving;
            btnCancel.Enabled = !isSaving;
            txtFullName.Enabled = !isSaving;
            txtUsername.Enabled = !isSaving;
            txtPassword.Enabled = !isSaving;
            cbBranch.Enabled = !isSaving && ((cbRole.SelectedItem as RoleOption)?.RequiresBranch ?? true);
            cbRole.Enabled = !isSaving;
            UseWaitCursor = isSaving;

            if (!isSaving)
            {
                UpdateSaveButtonState();
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!ValidateInput(out string fullName, out string? newPassword, out int? branchId, out RoleOption? selectedRole) ||
                selectedRole == null)
            {
                return;
            }

            try
            {
                SetSavingState(true);

                var user = new User
                {
                    UserId = _userId,
                    TenantId = GetTenantIdOrThrow(),
                    FullName = fullName,
                    BranchId = branchId,
                    RoleId = selectedRole.RoleId
                };

                bool isSuccess = await _userService.UpdateUserAsync(user, newPassword);
                if (isSuccess)
                {
                    MessageBox.Show("Cập nhật thông tin người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    _allowCloseWhenSaving = true;
                    Close();
                    return;
                }

                MessageBox.Show("Cập nhật thông tin thất bại, vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật người dùng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    SetSavingState(false);
                }
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
