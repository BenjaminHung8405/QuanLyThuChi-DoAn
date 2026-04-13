using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.DTOs;

namespace QuanLyThuChi_DoAn
{
    public partial class ucUserManagement : UserControl
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly BindingSource _bindingSource;
        private readonly Font _activeStatusFont;
        private readonly Font _inactiveStatusFont;

        private List<UserDTO> _userList = new List<UserDTO>();
        private bool _isBusy;

        public ucUserManagement()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _userService = new UserService(_context);
            _bindingSource = new BindingSource();
            _activeStatusFont = new Font(dgvUsers.Font, FontStyle.Bold);
            _inactiveStatusFont = new Font(dgvUsers.Font, FontStyle.Bold | FontStyle.Italic);

            dgvUsers.DataSource = _bindingSource;
            ConfigureGrid();
            WireEvents();
            UpdateToggleButtonState();

            Disposed += OnDisposed;
        }

        private void WireEvents()
        {
            Load += ucUserManagement_Load;
            btnSearch.Click += (_, __) => ApplySearchFilter();
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            btnAddUser.Click += btnAddUser_Click;
            btnEditUser.Click += btnEditUser_Click;
            btnToggleStatus.Click += async (_, __) => await ToggleStatusAsync();
            dgvUsers.SelectionChanged += (_, __) => UpdateToggleButtonState();
            dgvUsers.CellDoubleClick += dgvUsers_CellDoubleClick;
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            dgvUsers.DataError += dgvUsers_DataError;
        }

        private async void btnAddUser_Click(object? sender, EventArgs e)
        {
            await OpenAddUserDialogAsync();
        }

        private async void btnEditUser_Click(object? sender, EventArgs e)
        {
            await OpenEditUserDialogAsync();
        }

        private async void dgvUsers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isBusy)
            {
                return;
            }

            if (e.ColumnIndex >= 0 && dgvUsers.Columns[e.ColumnIndex].Name == "IsActive")
            {
                return;
            }

            await OpenEditUserDialogAsync();
        }

        private void dgvUsers_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            // Handle DataGridView formatting errors silently
            e.Cancel = false;
            e.ThrowException = false;
        }

        private async void ucUserManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            try
            {
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDisposed(object? sender, EventArgs e)
        {
            _activeStatusFont.Dispose();
            _inactiveStatusFont.Dispose();
            _context.Dispose();
        }

        private void ConfigureGrid()
        {
            dgvUsers.AutoGenerateColumns = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.MultiSelect = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
        }

        private int GetTenantIdOrThrow()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                throw new InvalidOperationException("Không xác định được Tenant hiện tại. Vui lòng chọn Tenant trước.");
            }

            return SessionManager.CurrentTenantId.Value;
        }

        private async Task LoadUsersAsync(int? preferredUserId = null)
        {
            int tenantId = GetTenantIdOrThrow();

            SetBusyState(true);
            try
            {
                _userList = await _userService.GetUsersByTenantAsync(tenantId);
                ApplySearchFilter(preferredUserId);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task OpenAddUserDialogAsync()
        {
            if (_isBusy)
            {
                return;
            }

            using var addUserForm = new frmAddUser();
            DialogResult dialogResult = FindForm() is Form owner
                ? addUserForm.ShowDialog(owner)
                : addUserForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                await LoadUsersAsync();
            }
        }

        private async Task OpenEditUserDialogAsync()
        {
            if (_isBusy)
            {
                return;
            }

            UserDTO? selectedUser = GetSelectedUser();
            if (selectedUser == null || selectedUser.UserId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để chỉnh sửa.", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.UserId == SessionManager.CurrentUserId)
            {
                MessageBox.Show("Bạn không thể tự chỉnh sửa thông tin hoặc phân quyền của chính mình ở màn hình này.", "Từ chối quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var editUserForm = new frmEditUser(selectedUser.UserId);
            DialogResult dialogResult = FindForm() is Form owner
                ? editUserForm.ShowDialog(owner)
                : editUserForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                await LoadUsersAsync(selectedUser.UserId);
            }
        }

        private async Task ToggleStatusAsync()
        {
            if (_isBusy)
            {
                return;
            }

            UserDTO? selectedUser = GetSelectedUser();
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trong danh sách.", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.UserId == SessionManager.CurrentUserId)
            {
                MessageBox.Show("Bạn không thể tự khóa hoặc mở khóa tài khoản của chính mình.", "Từ chối quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string action = selectedUser.IsActive ? "khóa" : "mở khóa";
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn {action} tài khoản '{selectedUser.Username}' không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                bool isSuccess = await _userService.ToggleUserStatusAsync(selectedUser.UserId);
                if (isSuccess)
                {
                    await LoadUsersAsync(selectedUser.UserId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private UserDTO? GetSelectedUser()
        {
            return dgvUsers.CurrentRow?.DataBoundItem as UserDTO ?? _bindingSource.Current as UserDTO;
        }

        private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplySearchFilter();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (txtSearch.TextLength == 0)
                {
                    return;
                }

                e.SuppressKeyPress = true;
                txtSearch.Clear();
                ApplySearchFilter();
                return;
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }

            ApplySearchFilter();
        }

        private void ApplySearchFilter(int? preferredUserId = null)
        {
            int? selectedUserId = preferredUserId ?? GetSelectedUser()?.UserId;
            IEnumerable<UserDTO> filteredUsers = _userList;

            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredUsers = _userList.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.FullName) && u.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(u.Username) && u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }

            _bindingSource.DataSource = filteredUsers.ToList();
            FormatGrid();
            SelectRowByUserIdOrFallback(selectedUserId);
            UpdateToggleButtonState();
        }

        private void SelectRowByUserIdOrFallback(int? userId)
        {
            if (dgvUsers.Rows.Count == 0)
            {
                return;
            }

            DataGridViewRow? targetRow = null;
            if (userId.HasValue)
            {
                foreach (DataGridViewRow row in dgvUsers.Rows)
                {
                    if (row.DataBoundItem is UserDTO rowUser && rowUser.UserId == userId.Value)
                    {
                        targetRow = row;
                        break;
                    }
                }
            }

            targetRow ??= dgvUsers.Rows[0];

            // CurrentCell cannot point to a hidden column (e.g. UserId), so choose the first visible cell.
            DataGridViewCell? firstVisibleCell = null;
            foreach (DataGridViewCell cell in targetRow.Cells)
            {
                if (cell.OwningColumn != null && cell.OwningColumn.Visible)
                {
                    firstVisibleCell = cell;
                    break;
                }
            }

            dgvUsers.ClearSelection();
            targetRow.Selected = true;
            if (firstVisibleCell != null)
            {
                dgvUsers.CurrentCell = firstVisibleCell;
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            btnSearch.Enabled = !isBusy;
            btnAddUser.Enabled = !isBusy;
            txtSearch.Enabled = !isBusy;
            UseWaitCursor = isBusy;
            UpdateToggleButtonState();
        }

        private void UpdateToggleButtonState()
        {
            UserDTO? selectedUser = GetSelectedUser();
            if (selectedUser == null)
            {
                btnEditUser.Enabled = false;
                btnToggleStatus.Enabled = false;
                btnToggleStatus.Text = "🔒 Khóa / Mở Khóa";
                btnToggleStatus.BackColor = Color.Goldenrod;
                return;
            }

            btnEditUser.Enabled = !_isBusy;
            btnToggleStatus.Enabled = !_isBusy;

            btnToggleStatus.Text = selectedUser.IsActive ? "🔒 Khóa tài khoản" : "🔓 Mở khóa tài khoản";
            btnToggleStatus.BackColor = selectedUser.IsActive ? Color.Firebrick : Color.SeaGreen;
        }

        private void FormatGrid()
        {
            if (dgvUsers.Columns.Count == 0)
            {
                return;
            }

            EnsureStatusTextColumn();

            // 1. Ẩn các cột không cần thiết (như ID)
            if (dgvUsers.Columns["UserId"] is DataGridViewColumn userIdColumn)
            {
                userIdColumn.Visible = false;
            }

            if (dgvUsers.Columns["TenantId"] is DataGridViewColumn tenantIdColumn)
            {
                tenantIdColumn.Visible = false;
            }

            if (dgvUsers.Columns["RoleId"] is DataGridViewColumn roleIdColumn)
            {
                roleIdColumn.Visible = false;
            }

            if (dgvUsers.Columns["BranchId"] is DataGridViewColumn branchIdColumn)
            {
                branchIdColumn.Visible = false;
            }

            // 2. Đổi tên cột Tiếng Việt và căn chỉnh độ rộng
            if (dgvUsers.Columns["FullName"] is DataGridViewColumn fullNameColumn)
            {
                fullNameColumn.HeaderText = "Họ và Tên";
                fullNameColumn.FillWeight = 150;
            }

            if (dgvUsers.Columns["Username"] is DataGridViewColumn usernameColumn)
            {
                usernameColumn.HeaderText = "Tên Đăng Nhập";
            }

            if (dgvUsers.Columns["RoleName"] is DataGridViewColumn roleNameColumn)
            {
                roleNameColumn.HeaderText = "Chức Vụ";
            }

            if (dgvUsers.Columns["BranchName"] is DataGridViewColumn branchNameColumn)
            {
                branchNameColumn.HeaderText = "Chi Nhánh";
            }

            // 3. Format cột Ngày tạo
            if (dgvUsers.Columns["CreatedDate"] is DataGridViewColumn createdDateColumn)
            {
                createdDateColumn.HeaderText = "Ngày Tạo";
                createdDateColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            // 4. (Nâng cao) Định dạng cột Trạng Thái (IsActive)
            if (dgvUsers.Columns["IsActive"] is DataGridViewColumn isActiveColumn)
            {
                isActiveColumn.HeaderText = "Trạng Thái";
                isActiveColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                isActiveColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                isActiveColumn.FillWeight = 90;
            }
        }

        private void EnsureStatusTextColumn()
        {
            if (dgvUsers.Columns["IsActive"] is not DataGridViewCheckBoxColumn checkBoxColumn)
            {
                return;
            }

            int insertionIndex = checkBoxColumn.Index;
            int displayIndex = checkBoxColumn.DisplayIndex;

            dgvUsers.Columns.Remove(checkBoxColumn);

            var statusTextColumn = new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                DataPropertyName = nameof(UserDTO.IsActive),
                HeaderText = "Trạng Thái",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dgvUsers.Columns.Insert(insertionIndex, statusTextColumn);
            statusTextColumn.DisplayIndex = displayIndex;
        }

        private void dgvUsers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvUsers.Columns[e.ColumnIndex].Name != "IsActive" || e.Value is not bool isActive)
            {
                return;
            }

            try
            {
                if (isActive)
                {
                    e.Value = "● Đang hoạt động";
                    e.CellStyle.ForeColor = Color.SeaGreen;
                    e.CellStyle.Font = _activeStatusFont;
                    dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    e.Value = "● Đã khóa";
                    e.CellStyle.ForeColor = Color.IndianRed;
                    e.CellStyle.Font = _inactiveStatusFont;
                    dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                }

                e.FormattingApplied = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Formatting error: {ex.Message}");
                e.FormattingApplied = false;
            }
        }
    }
}
