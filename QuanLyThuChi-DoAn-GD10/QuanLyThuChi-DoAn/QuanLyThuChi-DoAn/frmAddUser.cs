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
    public partial class frmAddUser : Form
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly BranchService _branchService;
        private bool _isSaving;

        private sealed class RoleOption
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public bool RequiresBranch { get; set; }
        }

        public frmAddUser()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _userService = new UserService(_context);
            _branchService = new BranchService(_context);

            Load += frmAddUser_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
            FormClosed += frmAddUser_FormClosed;
        }

        private void frmAddUser_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _context.Dispose();
        }

        private async void frmAddUser_Load(object? sender, EventArgs e)
        {
            try
            {
                await LoadComboBoxDataAsync();
                txtFullName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
            }
        }

        private int GetTenantIdOrThrow()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                throw new InvalidOperationException("Không xác định được Tenant hiện tại. Vui lòng đăng nhập lại.");
            }

            return SessionManager.CurrentTenantId.Value;
        }

        private static bool IsBranchScopedRole(string? roleName)
        {
            return string.Equals(roleName, "BranchManager", StringComparison.OrdinalIgnoreCase)
                || string.Equals(roleName, "Staff", StringComparison.OrdinalIgnoreCase);
        }

        private static string MapRoleDisplayName(string? roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return "N/A";
            }

            if (string.Equals(roleName, "TenantAdmin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return "Giám đốc";
            }

            if (string.Equals(roleName, "BranchManager", StringComparison.OrdinalIgnoreCase))
            {
                return "Quản lý chi nhánh";
            }

            if (string.Equals(roleName, "Staff", StringComparison.OrdinalIgnoreCase))
            {
                return "Nhân viên thu ngân";
            }

            if (string.Equals(roleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return "Quản trị hệ thống";
            }

            return roleName;
        }

        private async Task LoadComboBoxDataAsync()
        {
            int tenantId = GetTenantIdOrThrow();

            var branches = await _branchService.GetBranchesByTenantAsync(tenantId);
            cbBranch.DataSource = branches;
            cbBranch.DisplayMember = nameof(Branch.BranchName);
            cbBranch.ValueMember = nameof(Branch.BranchId);
            cbBranch.SelectedIndex = branches.Count > 0 ? 0 : -1;

            var roles = await _userService.GetAssignableRolesByTenantAsync(tenantId);
            var roleOptions = roles
                .Select(r => new RoleOption
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName ?? string.Empty,
                    DisplayName = MapRoleDisplayName(r.RoleName),
                    RequiresBranch = IsBranchScopedRole(r.RoleName)
                })
                .ToList();

            cbRole.DataSource = roleOptions;
            cbRole.DisplayMember = nameof(RoleOption.DisplayName);
            cbRole.ValueMember = nameof(RoleOption.RoleId);
            cbRole.SelectedIndex = roleOptions.Count > 0 ? 0 : -1;

            UpdateBranchStateByRole();

            if (roleOptions.Count == 0)
            {
                MessageBox.Show("Không có vai trò hợp lệ để gán cho người dùng mới.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            cbBranch.Enabled = requiresBranch;
            lblBranch.Text = requiresBranch ? "Chi nhánh (*)" : "Chi nhánh";

            if (!requiresBranch)
            {
                cbBranch.SelectedIndex = -1;
            }
            else if (cbBranch.SelectedIndex < 0 && cbBranch.Items.Count > 0)
            {
                cbBranch.SelectedIndex = 0;
            }
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
            out string username,
            out string rawPassword,
            out int? branchId,
            out RoleOption? selectedRole)
        {
            fullName = txtFullName.Text.Trim();
            username = txtUsername.Text.Trim();
            rawPassword = txtPassword.Text;
            selectedRole = cbRole.SelectedItem as RoleOption;
            branchId = null;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(rawPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, Tên đăng nhập và Mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (selectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ cho tài khoản.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void SetSavingState(bool isSaving)
        {
            _isSaving = isSaving;
            btnSave.Enabled = !isSaving;
            btnCancel.Enabled = !isSaving;
            txtFullName.Enabled = !isSaving;
            txtUsername.Enabled = !isSaving;
            txtPassword.Enabled = !isSaving;
            cbBranch.Enabled = !isSaving && ((cbRole.SelectedItem as RoleOption)?.RequiresBranch ?? true);
            cbRole.Enabled = !isSaving;
            UseWaitCursor = isSaving;
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!ValidateInput(out string fullName, out string username, out string rawPassword, out int? branchId, out RoleOption? selectedRole) ||
                selectedRole == null)
            {
                return;
            }

            try
            {
                SetSavingState(true);

                var newUser = new User
                {
                    TenantId = GetTenantIdOrThrow(),
                    FullName = fullName,
                    Username = username,
                    BranchId = branchId,
                    RoleId = selectedRole.RoleId
                };

                bool isSuccess = await _userService.CreateUserAsync(newUser, rawPassword);
                if (isSuccess)
                {
                    MessageBox.Show("Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                MessageBox.Show("Tạo tài khoản thất bại, vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tạo tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
