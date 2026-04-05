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

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class ucBranchManagement : UserControl
    {
        public sealed class BranchChangedEventArgs : EventArgs
        {
            public BranchChangedEventArgs(int tenantId, int? branchId, string action)
            {
                TenantId = tenantId;
                BranchId = branchId;
                Action = action;
            }

            public int TenantId { get; }
            public int? BranchId { get; }
            public string Action { get; }
        }

        private readonly AppDbContext _context;
        private readonly BranchService _branchService;
        private readonly BindingSource _bindingSource;
        private readonly Font _activeStatusFont;
        private readonly Font _inactiveStatusFont;

        private List<Branch> _branchList = new List<Branch>();
        private bool _isBusy;

        public event EventHandler<BranchChangedEventArgs>? BranchChanged;

        public ucBranchManagement()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _branchService = new BranchService(_context);
            _bindingSource = new BindingSource();
            _activeStatusFont = new Font(dgvBranches.Font, FontStyle.Bold);
            _inactiveStatusFont = new Font(dgvBranches.Font, FontStyle.Bold | FontStyle.Italic);

            dgvBranches.DataSource = _bindingSource;

            ConfigureGrid();
            WireEvents();
            UpdateActionButtonsState();

            Disposed += OnDisposed;
        }

        private void WireEvents()
        {
            btnSearch.Click += (_, __) => ApplySearchFilter();
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnAddBranch.Click += btnAddBranch_Click;
            btnEditBranch.Click += btnEditBranch_Click;
            btnToggleStatus.Click += btnToggleStatus_Click;

            dgvBranches.SelectionChanged += (_, __) => UpdateActionButtonsState();
            dgvBranches.CellDoubleClick += dgvBranches_CellDoubleClick;
            dgvBranches.DataError += dgvBranches_DataError;
        }

        private void ConfigureGrid()
        {
            dgvBranches.AutoGenerateColumns = true;
            dgvBranches.AllowUserToAddRows = false;
            dgvBranches.AllowUserToDeleteRows = false;
            dgvBranches.ReadOnly = true;
            dgvBranches.MultiSelect = false;
            dgvBranches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBranches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBranches.BackgroundColor = Color.White;
            dgvBranches.BorderStyle = BorderStyle.None;
        }

        private async void ucBranchManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            try
            {
                await LoadBranchesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAddBranch_Click(object? sender, EventArgs e)
        {
            await OpenAddBranchDialogAsync();
        }

        private async void btnEditBranch_Click(object? sender, EventArgs e)
        {
            await OpenEditBranchDialogAsync();
        }

        private async void btnToggleStatus_Click(object? sender, EventArgs e)
        {
            await ToggleStatusAsync();
        }

        private async void dgvBranches_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isBusy)
            {
                return;
            }

            if (e.ColumnIndex >= 0 && dgvBranches.Columns[e.ColumnIndex].Name == "IsActive")
            {
                return;
            }

            await OpenEditBranchDialogAsync();
        }

        private void dgvBranches_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            e.ThrowException = false;
        }

        private void OnDisposed(object? sender, EventArgs e)
        {
            _activeStatusFont.Dispose();
            _inactiveStatusFont.Dispose();
            _context.Dispose();
        }

        private int GetTenantIdOrThrow()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                throw new InvalidOperationException("Chưa chọn Tenant ngữ cảnh. Vui lòng chọn Tenant trước.");
            }

            return SessionManager.CurrentTenantId.Value;
        }

        private async Task LoadBranchesAsync(int? preferredBranchId = null)
        {
            int tenantId = GetTenantIdOrThrow();

            SetBusyState(true);
            try
            {
                _branchList = await _branchService.GetByTenantAsync(tenantId);
                ApplySearchFilter(preferredBranchId);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task OpenAddBranchDialogAsync()
        {
            if (_isBusy)
            {
                return;
            }

            try
            {
                GetTenantIdOrThrow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thiếu ngữ cảnh Tenant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var addForm = new frmAddEditBranch();
            DialogResult dialogResult = FindForm() is Form owner
                ? addForm.ShowDialog(owner)
                : addForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Clear();
                }

                int tenantId = GetTenantIdOrThrow();
                int? branchId = addForm.SavedBranchId;

                await LoadBranchesAsync(branchId);
                NotifyBranchChanged(tenantId, branchId, "added");
            }
        }

        private async Task OpenEditBranchDialogAsync()
        {
            if (_isBusy)
            {
                return;
            }

            Branch? selectedBranch = GetSelectedBranch();
            if (selectedBranch == null || selectedBranch.BranchId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một chi nhánh để chỉnh sửa.", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var editForm = new frmAddEditBranch(selectedBranch.BranchId);
            DialogResult dialogResult = FindForm() is Form owner
                ? editForm.ShowDialog(owner)
                : editForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Clear();
                }

                int tenantId = GetTenantIdOrThrow();
                int? branchId = editForm.SavedBranchId ?? selectedBranch.BranchId;

                await LoadBranchesAsync(branchId);
                NotifyBranchChanged(tenantId, branchId, "updated");
            }
        }

        private async Task ToggleStatusAsync()
        {
            if (_isBusy)
            {
                return;
            }

            Branch? selectedBranch = GetSelectedBranch();
            if (selectedBranch == null)
            {
                MessageBox.Show("Vui lòng chọn một chi nhánh trong danh sách.", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string action = selectedBranch.IsActive ? "khóa" : "mở khóa";
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn {action} chi nhánh '{selectedBranch.BranchName}' không?",
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
                bool isSuccess = await _branchService.ToggleStatusAsync(selectedBranch.BranchId);
                if (isSuccess)
                {
                    await LoadBranchesAsync(selectedBranch.BranchId);
                    NotifyBranchChanged(selectedBranch.TenantId, selectedBranch.BranchId, "status-changed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật trạng thái chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private Branch? GetSelectedBranch()
        {
            return dgvBranches.CurrentRow?.DataBoundItem as Branch ?? _bindingSource.Current as Branch;
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

        private void ApplySearchFilter(int? preferredBranchId = null)
        {
            int? selectedBranchId = preferredBranchId ?? GetSelectedBranch()?.BranchId;
            IEnumerable<Branch> filteredBranches = _branchList;

            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredBranches = _branchList.Where(b =>
                    (!string.IsNullOrWhiteSpace(b.BranchName) && b.BranchName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(b.Address) && b.Address.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(b.Phone) && b.Phone.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }

            _bindingSource.DataSource = filteredBranches.ToList();
            FormatGrid();
            SelectRowByBranchIdOrFallback(selectedBranchId);
            UpdateActionButtonsState();
        }

        private void SelectRowByBranchIdOrFallback(int? branchId)
        {
            if (dgvBranches.Rows.Count == 0)
            {
                return;
            }

            DataGridViewRow? targetRow = null;
            if (branchId.HasValue)
            {
                foreach (DataGridViewRow row in dgvBranches.Rows)
                {
                    if (row.DataBoundItem is Branch rowBranch && rowBranch.BranchId == branchId.Value)
                    {
                        targetRow = row;
                        break;
                    }
                }
            }

            targetRow ??= dgvBranches.Rows[0];

            DataGridViewCell? firstVisibleCell = null;
            foreach (DataGridViewCell cell in targetRow.Cells)
            {
                if (cell.OwningColumn != null && cell.OwningColumn.Visible)
                {
                    firstVisibleCell = cell;
                    break;
                }
            }

            dgvBranches.ClearSelection();
            targetRow.Selected = true;
            if (firstVisibleCell != null)
            {
                dgvBranches.CurrentCell = firstVisibleCell;
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            btnSearch.Enabled = !isBusy;
            btnAddBranch.Enabled = !isBusy;
            txtSearch.Enabled = !isBusy;
            UseWaitCursor = isBusy;
            UpdateActionButtonsState();
        }

        private void UpdateActionButtonsState()
        {
            Branch? selectedBranch = GetSelectedBranch();
            if (selectedBranch == null)
            {
                btnEditBranch.Enabled = false;
                btnToggleStatus.Enabled = false;
                btnToggleStatus.Text = "🔒 Khóa / Mở";
                btnToggleStatus.BackColor = Color.IndianRed;
                return;
            }

            btnEditBranch.Enabled = !_isBusy;
            btnToggleStatus.Enabled = !_isBusy;

            btnToggleStatus.Text = selectedBranch.IsActive ? "🔒 Khóa chi nhánh" : "🔓 Mở chi nhánh";
            btnToggleStatus.BackColor = selectedBranch.IsActive ? Color.IndianRed : Color.SeaGreen;
        }

        private void NotifyBranchChanged(int tenantId, int? branchId, string action)
        {
            BranchChanged?.Invoke(this, new BranchChangedEventArgs(tenantId, branchId, action));
        }

        private void dgvBranches_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvBranches.Columns.Count == 0)
            {
                return;
            }

            EnsureStatusTextColumn();

            if (dgvBranches.Columns["BranchId"] is DataGridViewColumn branchIdColumn)
            {
                branchIdColumn.Visible = false;
            }

            if (dgvBranches.Columns["TenantId"] is DataGridViewColumn tenantIdColumn)
            {
                tenantIdColumn.Visible = false;
            }

            if (dgvBranches.Columns["Tenant"] is DataGridViewColumn tenantColumn)
            {
                tenantColumn.Visible = false;
            }

            if (dgvBranches.Columns["Users"] is DataGridViewColumn usersColumn)
            {
                usersColumn.Visible = false;
            }

            if (dgvBranches.Columns["Transactions"] is DataGridViewColumn transactionsColumn)
            {
                transactionsColumn.Visible = false;
            }

            if (dgvBranches.Columns["BranchName"] is DataGridViewColumn branchNameColumn)
            {
                branchNameColumn.HeaderText = "Tên Chi Nhánh";
                branchNameColumn.FillWeight = 150;
            }

            if (dgvBranches.Columns["Address"] is DataGridViewColumn addressColumn)
            {
                addressColumn.HeaderText = "Địa Chỉ";
                addressColumn.FillWeight = 200;
            }

            if (dgvBranches.Columns["Phone"] is DataGridViewColumn phoneColumn)
            {
                phoneColumn.HeaderText = "Điện Thoại";
            }

            if (dgvBranches.Columns["CreatedDate"] is DataGridViewColumn createdDateColumn)
            {
                createdDateColumn.HeaderText = "Ngày Tạo";
                createdDateColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvBranches.Columns["IsActive"] is DataGridViewColumn isActiveColumn)
            {
                isActiveColumn.HeaderText = "Trạng Thái";
                isActiveColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                isActiveColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                isActiveColumn.FillWeight = 90;
            }
        }

        private void EnsureStatusTextColumn()
        {
            if (dgvBranches.Columns["IsActive"] is not DataGridViewCheckBoxColumn checkBoxColumn)
            {
                return;
            }

            int insertionIndex = checkBoxColumn.Index;
            int displayIndex = checkBoxColumn.DisplayIndex;

            dgvBranches.Columns.Remove(checkBoxColumn);

            var statusTextColumn = new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                DataPropertyName = "IsActive",
                HeaderText = "Trạng Thái",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dgvBranches.Columns.Insert(insertionIndex, statusTextColumn);
            statusTextColumn.DisplayIndex = displayIndex;
        }

        private void dgvBranches_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvBranches.Columns[e.ColumnIndex].Name != "IsActive" || e.Value is not bool isActive)
            {
                return;
            }

            if (isActive)
            {
                e.Value = "Đang hoạt động";
                e.CellStyle.ForeColor = Color.SeaGreen;
                e.CellStyle.Font = _activeStatusFont;
                dgvBranches.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            else
            {
                e.Value = "Tạm ngưng";
                e.CellStyle.ForeColor = Color.IndianRed;
                e.CellStyle.Font = _inactiveStatusFont;
                dgvBranches.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            }

            e.FormattingApplied = true;
        }
    }
}
