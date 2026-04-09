using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class ucTaxManagement : UserControl
    {
        private readonly AppDbContext _context;
        private readonly TaxService _taxService;
        private readonly BindingSource _bindingSource;
        private readonly Font _activeStatusFont;
        private readonly Font _inactiveStatusFont;

        private List<Tax> _taxList = new List<Tax>();
        private bool _isBusy;

        public ucTaxManagement()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _taxService = new TaxService(_context);
            _bindingSource = new BindingSource();
            _activeStatusFont = new Font(dgvTaxes.Font, FontStyle.Bold);
            _inactiveStatusFont = new Font(dgvTaxes.Font, FontStyle.Bold | FontStyle.Italic);

            dgvTaxes.DataSource = _bindingSource;

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

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnToggleStatus.Click += btnToggleStatus_Click;

            dgvTaxes.SelectionChanged += (_, __) => UpdateActionButtonsState();
            dgvTaxes.CellDoubleClick += dgvTaxes_CellDoubleClick;
            dgvTaxes.DataError += dgvTaxes_DataError;
        }

        private void ConfigureGrid()
        {
            dgvTaxes.AutoGenerateColumns = true;
            dgvTaxes.AllowUserToAddRows = false;
            dgvTaxes.AllowUserToDeleteRows = false;
            dgvTaxes.ReadOnly = true;
            dgvTaxes.MultiSelect = false;
            dgvTaxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaxes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTaxes.BackgroundColor = Color.White;
            dgvTaxes.BorderStyle = BorderStyle.None;
        }

        private async void ucTaxManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            try
            {
                await LoadTaxesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu thuế suất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTaxesAsync(int? preferredTaxId = null)
        {
            SetBusyState(true);
            try
            {
                _taxList = await _taxService.GetAllTaxesAsync();
                ApplySearchFilter(preferredTaxId);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async void btnToggleStatus_Click(object? sender, EventArgs e)
        {
            await ToggleStatusAsync();
        }

        private async void btnAdd_Click(object? sender, EventArgs e)
        {
            await OpenAddDialogAsync();
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            await OpenEditDialogAsync();
        }

        private async void dgvTaxes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isBusy)
            {
                return;
            }

            if (e.ColumnIndex >= 0 && dgvTaxes.Columns[e.ColumnIndex].Name == "IsActive")
            {
                return;
            }

            await OpenEditDialogAsync();
        }

        private void dgvTaxes_DataError(object? sender, DataGridViewDataErrorEventArgs e)
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

        private async Task ToggleStatusAsync()
        {
            if (_isBusy)
            {
                return;
            }

            Tax? selectedTax = GetSelectedTax();
            if (selectedTax == null)
            {
                MessageBox.Show("Vui lòng chọn một loại thuế trong danh sách.", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string action = selectedTax.IsActive ? "khóa" : "mở khóa";
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn {action} loại thuế '{selectedTax.TaxName}' ({selectedTax.Rate:0.##}%) không?",
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
                bool isSuccess = await _taxService.ToggleTaxStatusAsync(selectedTax.TaxId);
                if (isSuccess)
                {
                    await LoadTaxesAsync(selectedTax.TaxId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật trạng thái thuế suất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task OpenAddDialogAsync()
        {
            if (_isBusy)
            {
                return;
            }

            try
            {
                using (var frm = new frmAddEditTax())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int? preferredTaxId = frm.SavedTaxId > 0 ? frm.SavedTaxId : null;
                        await LoadTaxesAsync(preferredTaxId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở form thêm thuế suất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task OpenEditDialogAsync()
        {
            if (_isBusy)
            {
                return;
            }

            Tax? selectedTax = GetSelectedTax();
            if (selectedTax == null)
            {
                MessageBox.Show("Vui lòng chọn một loại thuế để cập nhật.", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var frm = new frmAddEditTax(selectedTax))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int? preferredTaxId = frm.SavedTaxId > 0 ? frm.SavedTaxId : selectedTax.TaxId;
                        await LoadTaxesAsync(preferredTaxId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở form cập nhật thuế suất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Tax? GetSelectedTax()
        {
            return dgvTaxes.CurrentRow?.DataBoundItem as Tax ?? _bindingSource.Current as Tax;
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

        private void ApplySearchFilter(int? preferredTaxId = null)
        {
            int? selectedTaxId = preferredTaxId ?? GetSelectedTax()?.TaxId;
            IEnumerable<Tax> filteredTaxes = _taxList;

            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string normalizedKeyword = keyword.Replace('%', ' ').Trim();
                string invariantKeyword = normalizedKeyword.Replace(',', '.');

                filteredTaxes = _taxList.Where(t =>
                    (!string.IsNullOrWhiteSpace(t.TaxName) && t.TaxName.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase)) ||
                    t.Rate.ToString("0.##", CultureInfo.InvariantCulture).Contains(invariantKeyword, StringComparison.OrdinalIgnoreCase) ||
                    t.Rate.ToString("0.##", CultureInfo.CurrentCulture).Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase));
            }

            _bindingSource.DataSource = filteredTaxes.ToList();
            FormatGrid();
            SelectRowByTaxIdOrFallback(selectedTaxId);
            UpdateActionButtonsState();
        }

        private void SelectRowByTaxIdOrFallback(int? taxId)
        {
            if (dgvTaxes.Rows.Count == 0)
            {
                return;
            }

            DataGridViewRow? targetRow = null;
            if (taxId.HasValue)
            {
                foreach (DataGridViewRow row in dgvTaxes.Rows)
                {
                    if (row.DataBoundItem is Tax rowTax && rowTax.TaxId == taxId.Value)
                    {
                        targetRow = row;
                        break;
                    }
                }
            }

            targetRow ??= dgvTaxes.Rows[0];

            DataGridViewCell? firstVisibleCell = null;
            foreach (DataGridViewCell cell in targetRow.Cells)
            {
                if (cell.OwningColumn != null && cell.OwningColumn.Visible)
                {
                    firstVisibleCell = cell;
                    break;
                }
            }

            dgvTaxes.ClearSelection();
            targetRow.Selected = true;
            if (firstVisibleCell != null)
            {
                dgvTaxes.CurrentCell = firstVisibleCell;
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            btnSearch.Enabled = !isBusy;
            btnAdd.Enabled = !isBusy;
            txtSearch.Enabled = !isBusy;
            UseWaitCursor = isBusy;
            UpdateActionButtonsState();
        }

        private void UpdateActionButtonsState()
        {
            Tax? selectedTax = GetSelectedTax();
            if (selectedTax == null)
            {
                btnEdit.Enabled = false;
                btnToggleStatus.Enabled = false;
                btnToggleStatus.Text = "🔒 Khóa / Mở";
                btnToggleStatus.BackColor = Color.IndianRed;
                return;
            }

            btnEdit.Enabled = !_isBusy;
            btnToggleStatus.Enabled = !_isBusy;

            btnToggleStatus.Text = selectedTax.IsActive ? "🔒 Khóa thuế suất" : "🔓 Mở thuế suất";
            btnToggleStatus.BackColor = selectedTax.IsActive ? Color.IndianRed : Color.SeaGreen;
        }

        private void dgvTaxes_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvTaxes.Columns.Count == 0)
            {
                return;
            }

            EnsureStatusTextColumn();

            if (dgvTaxes.Columns["TaxId"] is DataGridViewColumn taxIdColumn)
            {
                taxIdColumn.Visible = false;
            }

            if (dgvTaxes.Columns["TenantId"] is DataGridViewColumn tenantIdColumn)
            {
                tenantIdColumn.Visible = false;
            }

            if (dgvTaxes.Columns["Tenant"] is DataGridViewColumn tenantColumn)
            {
                tenantColumn.Visible = false;
            }

            if (dgvTaxes.Columns["Transactions"] is DataGridViewColumn transactionsColumn)
            {
                transactionsColumn.Visible = false;
            }

            if (dgvTaxes.Columns["TaxName"] is DataGridViewColumn taxNameColumn)
            {
                taxNameColumn.HeaderText = "Tên Loại Thuế";
                taxNameColumn.FillWeight = 200;
            }

            if (dgvTaxes.Columns["Rate"] is DataGridViewColumn rateColumn)
            {
                rateColumn.HeaderText = "Mức Thuế Suất (%)";
                rateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                rateColumn.DefaultCellStyle.Format = "0.##\\%";
                rateColumn.FillWeight = 120;
            }

            if (dgvTaxes.Columns["IsActive"] is DataGridViewColumn isActiveColumn)
            {
                isActiveColumn.HeaderText = "Trạng Thái";
                isActiveColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                isActiveColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                isActiveColumn.FillWeight = 130;
            }
        }

        private void EnsureStatusTextColumn()
        {
            if (dgvTaxes.Columns["IsActive"] is not DataGridViewCheckBoxColumn checkBoxColumn)
            {
                return;
            }

            int insertionIndex = checkBoxColumn.Index;
            int displayIndex = checkBoxColumn.DisplayIndex;

            dgvTaxes.Columns.Remove(checkBoxColumn);

            var statusTextColumn = new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                DataPropertyName = "IsActive",
                HeaderText = "Trạng Thái",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dgvTaxes.Columns.Insert(insertionIndex, statusTextColumn);
            statusTextColumn.DisplayIndex = displayIndex;
        }

        private void dgvTaxes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvTaxes.Columns[e.ColumnIndex].Name != "IsActive" || e.Value is not bool isActive)
            {
                return;
            }

            if (isActive)
            {
                e.Value = "Đang áp dụng";
                e.CellStyle.ForeColor = Color.SeaGreen;
                e.CellStyle.Font = _activeStatusFont;
                dgvTaxes.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            else
            {
                e.Value = "Ngừng áp dụng";
                e.CellStyle.ForeColor = Color.IndianRed;
                e.CellStyle.Font = _inactiveStatusFont;
                dgvTaxes.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }

            e.FormattingApplied = true;
        }
    }
}
