using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.DTOs;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    public partial class ucTransactionCategory : UserControl
    {
        private readonly TransactionCategoryService _categoryService;
        private bool _isAddMode = false;
        private TransactionCategory _selectedCategory = null;

        public ucTransactionCategory()
        {
            InitializeComponent();
            var context = new AppDbContext();
            _categoryService = new TransactionCategoryService(context);
            SetupDataGridView();
            HookupEvents();
        }

        private void HookupEvents()
        {
            // ✅ Instant Search - Hook TextChanged events
            txtSearch.TextChanged += (s, e) => RefreshDataGrid();
            cboFilterType.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // ✅ DataGrid Interaction - Hook CellClick to populate textboxes
            dgvCategories.CellClick += DgvCategories_CellClick;
            dgvCategories.CellFormatting += DgvCategories_CellFormatting;
            dgvCategories.DataError += DgvCategories_DataError;

            // Button events
            btnNew.Click += BtnNew_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;
        }

        private void SetupDataGridView()
        {
            dgvCategories.AutoGenerateColumns = false;
            dgvCategories.Columns.Clear();
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            var colCategoryName = new DataGridViewTextBoxColumn
            {
                Name = "CategoryName",
                DataPropertyName = "CategoryName",
                HeaderText = "Tên Danh Mục",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 200
            };

            var colType = new DataGridViewTextBoxColumn
            {
                Name = "Type",
                DataPropertyName = "Type",
                HeaderText = "Phân Loại",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colTransactionCount = new DataGridViewTextBoxColumn
            {
                Name = "TransactionCount",
                DataPropertyName = "TransactionCount",
                HeaderText = "Số Giao Dịch",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }
            };

            var colTotalAmount = new DataGridViewTextBoxColumn
            {
                Name = "TotalAmount",
                DataPropertyName = "TotalAmount",
                HeaderText = "Tổng Tiền",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }
            };

            var colIsActive = new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                DataPropertyName = "IsActive",
                HeaderText = "Trạng Thái",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            dgvCategories.Columns.AddRange(new DataGridViewColumn[]
            {
                colCategoryName,
                colType,
                colTransactionCount,
                colTotalAmount,
                colIsActive
            });
        }

        private void RefreshDataGrid()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                string filterType = cboFilterType.SelectedItem?.ToString() ?? "Tất cả";

                if (!SessionManager.TenantId.HasValue)
                {
                    dgvCategories.DataSource = new List<TransactionCategoryDisplayDTO>();
                    return;
                }

                int currentTenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId.Value;
                
                // ✅ Using enriched statistics method from Service
                List<TransactionCategoryDisplayDTO> categories = _categoryService.GetCategoryStats(currentTenantId, keyword);

                // ✅ Filter by Type (Tất cả/Khoản Thu/Khoản Chi)
                if (filterType != "Tất cả")
                {
                    string typeValue = filterType == "Khoản Thu" ? "IN" : "OUT";
                    categories = categories.Where(c => c.Type == typeValue).ToList();
                }

                dgvCategories.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🎨 ĐIỂM NHẤN UX: Định dạng màu sắc Thu (Xanh) - Chi (Đỏ) và Trạng thái
        private void DgvCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCategories.Rows.Count) return;

            string colName = dgvCategories.Columns[e.ColumnIndex].Name;
            var category = dgvCategories.Rows[e.RowIndex].DataBoundItem as TransactionCategoryDisplayDTO;
            if (category == null) return;

            if (colName == "Type")
            {
                if (category.Type == "IN")
                {
                    e.Value = "➕ Khoản Thu";
                    e.CellStyle.ForeColor = Color.MediumSeaGreen;
                    e.CellStyle.Font = new Font(dgvCategories.Font, FontStyle.Bold);
                }
                else if (category.Type == "OUT")
                {
                    e.Value = "➖ Khoản Chi";
                    e.CellStyle.ForeColor = Color.Crimson;
                    e.CellStyle.Font = new Font(dgvCategories.Font, FontStyle.Bold);
                }
                e.FormattingApplied = true;
            }
            else if (colName == "IsActive")
            {
                // ✅ Avoid FormatException: Just set the value and color, let the grid handle display
                e.Value = category.IsActive ? "🎯 Đang hoạt động" : "💤 Ngừng hoạt động";
                e.CellStyle.ForeColor = category.IsActive ? Color.SeaGreen : Color.Gray;
                e.FormattingApplied = true;
            }
            else if (colName == "TotalAmount")
            {
                // ✅ Don't manually ToString("N0") here because the column already has Format = "N0"
                // Just change the color based on category type
                if (category.TotalAmount > 0)
                {
                    e.CellStyle.ForeColor = category.Type == "OUT" ? Color.Crimson : Color.MediumSeaGreen;
                }
            }
            else if (colName == "TransactionCount")
            {
                if (category.TransactionCount == 0)
                {
                    e.CellStyle.ForeColor = Color.Silver;
                }
            }
        }

        private void DgvCategories_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Silently handle data errors (usually formatting issues during rapid scroll)
            e.ThrowException = false;
        }

        // ✅ Auto-Edit on Select: Enable input fields and set to edit mode
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvCategories.Rows[e.RowIndex];
            var dto = row.DataBoundItem as TransactionCategoryDisplayDTO;
            if (dto == null) return;

            // ✅ Map DTO back to Entity for the service operations
            _selectedCategory = new TransactionCategory
            {
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                Type = dto.Type,
                IsActive = dto.IsActive
            };

            // Populate textboxes with selected data
            txtCategoryName.Text = _selectedCategory.CategoryName ?? "";
            cboType.SelectedItem = _selectedCategory.Type == "IN" ? "Thu" : "Chi";

            // ✅ CHANGE: Enable input fields for edit (Auto-Edit on Select)
            SetInputFieldsEnabled(true);
            _isAddMode = false; // Mark as Edit mode (not Add mode)
            txtCategoryName.Focus();
        }

        // ✅ btnNew_Click - Reset form, enable inputs, set focus
        private void BtnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetInputFieldsEnabled(true);
            _isAddMode = true;
            _selectedCategory = null;
            txtCategoryName.Focus();
        }

        // ✅ btnCancel_Click - Reset to initial locked state
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetInputFieldsEnabled(false);
            _isAddMode = false;
            RefreshDataGrid();
        }

        // ✅ btnSave_Click - Validation & Multi-tenant Context
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Ngăn việc nút bị click nhiều lần liên tiếp (Double-click/Enter bounce)
            if (!btnSave.Enabled) return;

            // ✅ Validate CategoryName is not empty
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return;
            }

            try
            {
                string successMessage = "";

                // 1. CHUẨN HÓA DỮ LIỆU ĐẦU VÀO (Dùng chung cho cả Add và Edit)
                string typeValue = "IN"; // Mặc định
                string selectedType = cboType.SelectedItem?.ToString();

                if (selectedType == "Thu" || selectedType == "IN") typeValue = "IN";
                else if (selectedType == "Chi" || selectedType == "OUT") typeValue = "OUT";

                // 2. XỬ LÝ LƯU HOẶC CẬP NHẬT
                if (_isAddMode)
                {
                    if (!SessionManager.TenantId.HasValue)
                    {
                        MessageBox.Show("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                    {
                        MessageBox.Show("Vui lòng chọn chi nhánh trước khi thêm danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var newCategory = new TransactionCategory
                    {
                        CategoryName = txtCategoryName.Text.Trim(),
                        Type = typeValue, // Dùng biến đã chuẩn hóa
                        TenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId.Value,
                        BranchId = SessionManager.CurrentBranchId.Value
                    };

                    _categoryService.CreateCategory(newCategory);
                    successMessage = "Thêm danh mục thành công!";
                }
                else if (_selectedCategory != null)
                {
                    if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                    {
                        MessageBox.Show("Vui lòng chọn chi nhánh trước khi cập nhật danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _selectedCategory.CategoryName = txtCategoryName.Text.Trim();
                    _selectedCategory.Type = typeValue; // Dùng biến đã chuẩn hóa
                    _selectedCategory.TenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId ?? 0;
                    _selectedCategory.BranchId = SessionManager.CurrentBranchId.Value;

                    _categoryService.UpdateCategory(_selectedCategory);
                    successMessage = "Cập nhật danh mục thành công!";
                }

                // 3. RESET VÀ REFRESH UI
                ResetForm();
                SetInputFieldsEnabled(false);
                _isAddMode = false;
                RefreshDataGrid();

                // 4. HIỂN THỊ THÔNG BÁO
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

        // ✅ btnDelete_Click - Confirmation & Soft Delete Logic
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Ask for confirmation before delete
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa danh mục '{_selectedCategory.CategoryName}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (!SessionManager.TenantId.HasValue)
                {
                    MessageBox.Show("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int currentTenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId.Value;
                _categoryService.DeleteCategory(_selectedCategory.CategoryId, currentTenantId);
                MessageBox.Show("Xóa danh mục thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtCategoryName.Text = "";
            txtSearch.Text = "";
            cboType.SelectedIndex = 0;
            cboFilterType.SelectedIndex = 0;
            _selectedCategory = null;
        }

        // Helper: Toggle input fields enabled/disabled based on mode
        private void SetInputFieldsEnabled(bool enabled)
        {
            txtCategoryName.Enabled = enabled;
            cboType.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnDelete.Enabled = _selectedCategory != null;

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

        private void ucTransactionCategory_Load(object sender, EventArgs e)
        {
            // ✅ Items already added in Designer, just set selected index
            if (cboFilterType.Items.Count == 0)
            {
                cboFilterType.Items.Add("Tất cả");
                cboFilterType.Items.Add("Khoản Thu");
                cboFilterType.Items.Add("Khoản Chi");
            }
            cboFilterType.SelectedIndex = 0;

            if (cboType.Items.Count == 0)
            {
                cboType.Items.Add("IN");
                cboType.Items.Add("OUT");
            }
            cboType.SelectedIndex = 0;

            // ✅ Load initial data with tenant context
            RefreshDataGrid();
        }
    }
}
