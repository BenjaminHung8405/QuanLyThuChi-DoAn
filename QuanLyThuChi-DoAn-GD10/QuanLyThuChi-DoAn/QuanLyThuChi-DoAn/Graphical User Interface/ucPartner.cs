using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    public partial class ucPartner : UserControl
    {
        private readonly PartnerService _partnerService;
        private bool _isAddMode = false;
        private Partner _selectedPartner = null;

        public ucPartner()
        {
            InitializeComponent();
            var context = new AppDbContext();
            _partnerService = new PartnerService(context);
            HookupEvents();
        }

        private void HookupEvents()
        {
            // ✅ Criterion 2: Instant Search - Hook TextChanged events
            txtSearch.TextChanged += (s, e) => RefreshDataGrid();
            cboFilterType.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // ✅ Criterion 3: DataGrid Interaction - Hook CellClick to populate textboxes
            dgvPartners.CellClick += DgvPartners_CellClick;
            dgvPartners.CellFormatting += DgvPartners_CellFormatting;

            // Button events
            btnNew.Click += BtnNew_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;
        }

        private void RefreshDataGrid()
        {
            try
            {
                // ✅ Criterion 5: CRITICAL - Pass SessionManager.TenantId to filter by tenant
                string keyword = txtSearch.Text.Trim();
                string filterType = cboFilterType.SelectedItem?.ToString() ?? "Tất cả";

                // ✅ FIX: Get ALL partners WITHOUT keyword filter from Service
                // (Service may not normalize same as UI, so we filter here with TextUtility)
                if (!SessionManager.TenantId.HasValue)
                {
                    dgvPartners.DataSource = new List<Partner>();
                    return;
                }

                int currentTenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId.Value;
                List<Partner> partners = _partnerService.GetPartners(currentTenantId, "");

                // ✅ Criterion 2: Filter by keyword with proper normalization (Tiếng Việt)
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string keywordNormalized = TextUtility.RemoveVietnameseAccents(keyword);

                    partners = partners.Where(p =>
                    {
                        string name = TextUtility.RemoveVietnameseAccents(p.PartnerName ?? string.Empty);
                        string phone = TextUtility.RemoveVietnameseAccents(p.Phone ?? string.Empty);
                        return name.Contains(keywordNormalized) || phone.Contains(keywordNormalized);
                    }).ToList();
                }

                // Filter by Type (Tất cả/Khách hàng/Nhà cung cấp)
                if (filterType != "Tất cả")
                {
                    // Map Vietnamese to English values
                    string typeValue = filterType == "Khách hàng" ? "CUSTOMER" : "SUPPLIER";
                    partners = partners.Where(p => p.Type == typeValue).ToList();
                }

                dgvPartners.DataSource = partners;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Criterion 3: CellFormatting - Color code by Type
        private void DgvPartners_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPartners.Columns[e.ColumnIndex].DataPropertyName == "Type" && e.Value != null)
            {
                if (e.Value.ToString() == "CUSTOMER")
                {
                    e.CellStyle.ForeColor = System.Drawing.Color.Blue;
                    e.CellStyle.Font = new System.Drawing.Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                }
                else if (e.Value.ToString() == "SUPPLIER")
                {
                    e.CellStyle.ForeColor = System.Drawing.Color.Orange;
                    e.CellStyle.Font = new System.Drawing.Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                }
            }

            // ✅ Criterion 3: Format InitialDebt as currency with thousands separator
            if (dgvPartners.Columns[e.ColumnIndex].DataPropertyName == "InitialDebt" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal amount))
                {
                    e.Value = amount.ToString("N0"); // Format as "1.000.000"
                    e.FormattingApplied = true;
                }
            }
        }

        // ✅ Criterion 3: CellClick - Map selected row to textboxes (Auto-Edit on Select)
        private void DgvPartners_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPartners.Rows[e.RowIndex];
            _selectedPartner = (Partner)row.DataBoundItem;

            // Populate textboxes with selected data
            txtPartnerName.Text = _selectedPartner.PartnerName ?? "";
            txtPhone.Text = _selectedPartner.Phone ?? "";
            txtAddress.Text = _selectedPartner.Address ?? "";
            cboType.SelectedItem = MapDbValueToDisplayType(_selectedPartner.Type);
            txtInitialDebt.Value = Math.Min(txtInitialDebt.Maximum, Math.Max(txtInitialDebt.Minimum, _selectedPartner.InitialDebt));

            // ✅ CHANGE: Enable input fields for edit (Auto-Edit on Select)
            SetInputFieldsEnabled(true);
            _isAddMode = false; // Mark as Edit mode (not Add mode)
            txtPartnerName.Focus();
        }

        // ✅ Criterion 1: btnNew_Click - Reset form, enable inputs, set focus
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetInputFieldsEnabled(true);
            _isAddMode = true;
            _selectedPartner = null;
            txtPartnerName.Focus();
        }

        // ✅ Criterion 1: btnCancel_Click - Reset to initial locked state
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetInputFieldsEnabled(false);
            _isAddMode = false;
            RefreshDataGrid();
        }

        // ✅ Criterion 4: btnSave_Click - Validation & Multi-tenant Context
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Ngăn việc nút bị click nhiều lần liên tiếp (Double-click/Enter bounce)
            if (!btnSave.Enabled) return;

            // ✅ Criterion 4: Validate PartnerName is not empty
            if (string.IsNullOrWhiteSpace(txtPartnerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đối tác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPartnerName.Focus();
                return;
            }

            // ✅ Criterion 4: NumericUpDown already constrains initial debt to numeric input
            decimal initialDebt = txtInitialDebt.Value;

            try
            {
                string successMessage = "";

                if (_isAddMode)
                {
                    // ✅ Criterion 5: CRITICAL - Assign TenantId from SessionManager
                    if (!SessionManager.TenantId.HasValue)
                    {
                        MessageBox.Show("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                    {
                        MessageBox.Show("Vui lòng chọn chi nhánh trước khi thêm đối tác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var newPartner = new Partner
                    {
                        PartnerName = txtPartnerName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        Type = MapDisplayTypeToDbValue(cboType.SelectedItem?.ToString()),
                        InitialDebt = initialDebt,
                        TenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId.Value,
                        BranchId = SessionManager.CurrentBranchId.Value
                    };

                    _partnerService.CreatePartner(newPartner);
                    successMessage = "Thêm đối tác thành công!";
                }
                else if (_selectedPartner != null)
                {
                    // ✅ Edit mode (Auto-Edit on Select)
                    if (!SessionManager.TenantId.HasValue)
                    {
                        MessageBox.Show("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                    {
                        MessageBox.Show("Vui lòng chọn chi nhánh trước khi cập nhật đối tác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _selectedPartner.PartnerName = txtPartnerName.Text.Trim();
                    _selectedPartner.Phone = txtPhone.Text.Trim();
                    _selectedPartner.Address = txtAddress.Text.Trim();
                    _selectedPartner.Type = MapDisplayTypeToDbValue(cboType.SelectedItem?.ToString());
                    _selectedPartner.InitialDebt = initialDebt;
                    _selectedPartner.TenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId.Value;
                    _selectedPartner.BranchId = SessionManager.CurrentBranchId.Value;

                    _partnerService.UpdatePartner(_selectedPartner);
                    successMessage = "Cập nhật đối tác thành công!";
                }

                // 1. CHUẨN UX: Reset giao diện và khóa nút TRƯỚC KHI hiển thị MessageBox 
                // để tránh tình trạng phím Enter nảy (bounce) click nhầm lần nữa
                ResetForm();
                SetInputFieldsEnabled(false);
                _isAddMode = false;
                RefreshDataGrid();

                // 2. Hiển thị thông báo SAU CÙNG
                if (!string.IsNullOrEmpty(successMessage))
                {
                    MessageBox.Show(successMessage, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Criterion 6: btnDelete_Click - Confirmation & Delete Logic
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPartner == null)
            {
                MessageBox.Show("Vui lòng chọn đối tác cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Criterion 6: Ask for confirmation before delete
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa đối tác '{_selectedPartner.PartnerName}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                _partnerService.DeletePartner(_selectedPartner.PartnerId);
                MessageBox.Show("Xóa đối tác thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                SetInputFieldsEnabled(false);
                _isAddMode = false;
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Refresh Button - Manual refresh data
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
                SetInputFieldsEnabled(false);
                _isAddMode = false;
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper: Reset all input fields
        private void ResetForm()
        {
            txtPartnerName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            cboType.SelectedIndex = 0;
            txtInitialDebt.Value = 0;
            txtSearch.Text = "";
            cboFilterType.SelectedIndex = 0;
            _selectedPartner = null;
        }

        // Helper: Toggle input fields enabled/disabled based on mode
        private void SetInputFieldsEnabled(bool enabled)
        {
            txtPartnerName.Enabled = enabled;
            txtPhone.Enabled = enabled;
            txtAddress.Enabled = enabled;
            cboType.Enabled = enabled;
            txtInitialDebt.Enabled = enabled;
            btnSave.Enabled = enabled;

            // ✅ Maintain white text color regardless of enabled state
            btnSave.ForeColor = Color.White;

            // ✅ Update button text based on mode
            if (enabled)
            {
                if (_isAddMode)
                    btnSave.Text = "Thêm";
                else
                    btnSave.Text = "Cập nhật"; // ✅ Auto-Edit on Select: Show "Cập nhật" when editing
            }
            else
            {
                btnSave.Text = "Lưu"; // Default text when disabled
            }
        }

        private void ucPartner_Load(object sender, EventArgs e)
        {
            // ✅ Items already added in Designer, just set selected index
            cboFilterType.SelectedIndex = 0;
            cboType.SelectedIndex = 0;

            // ✅ Load initial data with tenant context
            RefreshDataGrid();
        }

        private string MapDisplayTypeToDbValue(string displayType)
        {
            return displayType switch
            {
                "Khách hàng" => "CUSTOMER",
                "Nhà cung cấp" => "SUPPLIER",
                "CUSTOMER" => "CUSTOMER",
                "SUPPLIER" => "SUPPLIER",
                _ => "CUSTOMER"
            };
        }

        private string MapDbValueToDisplayType(string dbType)
        {
            return dbType switch
            {
                "CUSTOMER" => "Khách hàng",
                "SUPPLIER" => "Nhà cung cấp",
                _ => "Khách hàng"
            };
        }
    }
}
