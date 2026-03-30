using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    public partial class ucTransaction : UserControl
    {
        private readonly TransactionService _transactionService;
        private readonly CashFundService _cashFundService;
        private bool _isAddMode = false;
        private object _selectedTransaction = null;

        public ucTransaction()
        {
            InitializeComponent();
            var context = new AppDbContext();
            _transactionService = new TransactionService(context);
            _cashFundService = new CashFundService(context);
        }

        private void ucTransaction_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Set default date range: First day to last day of current month
                DateTime today = DateTime.Today;
                dtpFromDate.Value = new DateTime(today.Year, today.Month, 1); // ✅ FROM = first day
                dtpToDate.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)); // ✅ TO = last day

                dtpTransactionDate.Value = today;
                radIn.Checked = true; // Default to Income

                // 2. Setup DataGridView columns for Transaction model
                SetupDataGridView();

                // 3. Attach event handlers
                AttachEventHandlers();

                // 4. Initialize form state
                ResetForm();
                SetInputFieldsEnabled(false);

                // 4.1. Load dropdown master data including Funds 
                LoadMasterData();

                // 5. Refresh data
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Attach event handlers for UI controls
        /// </summary>
        private void AttachEventHandlers()
        {
            // Dynamic category filtering
            radIn.CheckedChanged += (s, e) => OnTransactionTypeChanged();
            radOut.CheckedChanged += (s, e) => OnTransactionTypeChanged();

            // Filter and refresh buttons
            btnFilter.Click += (s, e) => RefreshDataGrid(applyDateFilter: true);
            btnRefresh.Click += (s, e) =>
            {
                ResetForm();
                SetInputFieldsEnabled(false);
                RefreshDataGrid(applyDateFilter: false);
            };

            // CRUD buttons
            btnNew.Click += (s, e) => OnBtnNewClicked();
            btnSave.Click += (s, e) => OnBtnSaveClicked();
            btnCancel.Click += (s, e) => OnBtnCancelClicked();
            btnDelete.Click += (s, e) => OnBtnDeleteClicked();

            // Grid selection
            dgvTransactions.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    OnGridRowSelected(e.RowIndex);
                }
            };

            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;
        }

        /// <summary>
        /// Handle transaction type change - filter categories accordingly
        /// </summary>
        private void OnTransactionTypeChanged()
        {
            // TODO: Filter cboCategory based on radIn.Checked value
            // Load categories where Type = (radIn.Checked ? "IN" : "OUT")
            cboCategory.SelectedIndex = -1;
        }

        /// <summary>
        /// Configure DataGridView columns for Transaction entities
        /// </summary>
        private void SetupDataGridView()
        {
            dgvTransactions.AutoGenerateColumns = false;
            dgvTransactions.Columns.Clear();

            var colTransId = new DataGridViewTextBoxColumn
            {
                Name = "colTransId",
                DataPropertyName = "TransId",
                HeaderText = "TransId",
                Visible = false
            };

            var colTransDate = new DataGridViewTextBoxColumn
            {
                Name = "colTransDate",
                DataPropertyName = "TransDate",
                HeaderText = "Ngày",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 100
            };

            var colTransType = new DataGridViewTextBoxColumn
            {
                Name = "colTransType",
                DataPropertyName = "TransType",
                HeaderText = "Loại",
                Width = 80
            };

            var colAmount = new DataGridViewTextBoxColumn
            {
                Name = "colAmount",
                DataPropertyName = "Amount",
                HeaderText = "Số tiền",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 120
            };

            var colDescription = new DataGridViewTextBoxColumn
            {
                Name = "colDescription",
                DataPropertyName = "Description",
                HeaderText = "Nội dung",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            var colRefNo = new DataGridViewTextBoxColumn
            {
                Name = "colRefNo",
                DataPropertyName = "RefNo",
                HeaderText = "Mã Ref",
                Width = 120
            };

            dgvTransactions.Columns.AddRange(new DataGridViewColumn[]
            {
                colTransId,
                colTransDate,
                colTransType,
                colAmount,
                colDescription,
                colRefNo
            });

            dgvTransactions.CellFormatting -= DgvTransactions_CellFormatting;
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;
        }

        /// <summary>
        /// Refresh the transaction data grid
        /// Uses safe date ranges (2000-2099) instead of DateTime.MinValue/MaxValue to avoid SQL provider exceptions
        /// </summary>
        public void RefreshDataGrid(bool applyDateFilter = false)
        {
            try
            {
                int tenantId = 4;
                var keyword = txtSearch.Text.Trim();

                // Use safe date ranges (2000-2099) instead of DateTime.MinValue/MaxValue
                // This avoids SQL provider exceptions with year 0001 or 9999
                var fromDate = applyDateFilter ? dtpFromDate.Value : new DateTime(2000, 1, 1); // ✅ FIXED: Use dtpFromDate for FROM
                var toDate = applyDateFilter ? dtpToDate.Value : new DateTime(2099, 12, 31);   // ✅ FIXED: Use dtpToDate for TO

                var transactions = _transactionService.GetTransactions(
                    tenantId,
                    fromDate,
                    toDate,
                    keyword
                );

                dgvTransactions.DataSource = null;
                dgvTransactions.DataSource = transactions;

                decimal totalIn = transactions.Where(t => t.TransType == "IN").Sum(t => t.Amount);
                decimal totalOut = transactions.Where(t => t.TransType == "OUT").Sum(t => t.Amount);
                decimal balance = totalIn - totalOut;

                lblTotalIn.Text = $"Tổng Thu: {totalIn:N0} đ";
                lblTotalOut.Text = $"Tổng Chi: {totalOut:N0} đ";
                lblBalance.Text = $"Tồn Quỹ: {balance:N0} đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Format grid cells (color, currency format, etc.)
        /// Updated for new column names: TransType, Description, Amount
        /// </summary>
        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string colName = dgvTransactions.Columns[e.ColumnIndex].Name;

            // Color code transaction type and map to Thu/Chi
            if (colName == "colTransType")
            {
                string type = e.Value.ToString();
                e.Value = type == "IN" ? "Thu" : "Chi";
                e.CellStyle.ForeColor = type == "IN" ? Color.MediumSeaGreen : Color.Crimson;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            // Format currency
            else if (colName == "colAmount")
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal amount))
                {
                    e.Value = amount.ToString("N0");
                }
            }
        }

        /// <summary>
        /// Handle grid row selection (auto-edit mode)
        /// Updated for new schema: TransDate, TransType, Description, etc.
        /// </summary>
        private void OnGridRowSelected(int rowIndex)
        {
            try
            {
                _selectedTransaction = dgvTransactions.Rows[rowIndex].DataBoundItem;

                // TODO: Populate form fields from selected transaction
                // Get values using new column names:
                // - TransDate (instead of TransactionDate)
                // - TransType (instead of Type) - determines radIn/radOut
                // - Description (instead of Note)
                // - Amount, CategoryId, PartnerId, etc.

                SetInputFieldsEnabled(true);
                _isAddMode = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handle "New" button click
        /// </summary>
        private void OnBtnNewClicked()
        {
            ResetForm();
            SetInputFieldsEnabled(true);
            _isAddMode = true;
            _selectedTransaction = null;
            txtAmount.Focus();
        }

        /// <summary>
        /// Handle "Save" button click
        /// Updated for new schema with TransType, Description, RefNo, CreatedBy, Status, FundId
        /// </summary>
        private void OnBtnSaveClicked()
        {
            // Gọi hàm kiểm tra trước khi làm bất cứ việc gì khác
            if (!ValidateInput()) return;

            try
            {
                string typeValue = radIn.Checked ? "IN" : "OUT";
                int categoryId = (int)cboCategory.SelectedValue;
                int? partnerId = (cboPartner.SelectedValue == null || (int)cboPartner.SelectedValue == 0) ? (int?)null : (int)cboPartner.SelectedValue;

                if (_isAddMode)
                {
                    var newTrans = new Transaction
                    {
                        // TỰ ĐỘNG GÁN TỪ PHIÊN ĐĂNG NHẬP
                        TenantId = SessionManager.TenantId,   // Ví dụ: 4
                        BranchId = SessionManager.BranchId.Value,   // Ví dụ: 4 (Chi nhánh Long Xuyên)
                        CreatedBy = SessionManager.UserId,    // Ví dụ: 2 (Admin)

                        // DỮ LIỆU TỪ FORM
                        FundId = (int)cboFund.SelectedValue,
                        CategoryId = categoryId,
                        PartnerId = partnerId,
                        TransDate = dtpTransactionDate.Value,
                        Amount = decimal.Parse(txtAmount.Text.Trim()),
                        Description = txtNote.Text.Trim(),
                        TransType = typeValue,
                        RefNo = $"TR-{DateTime.Now:yyyyMMddHHmmss}",
                        Status = "COMPLETED",
                        IsActive = true // Luôn bật IsActive khi tạo mới
                    };

                    _transactionService.CreateTransaction(newTrans);
                    MessageBox.Show("Lập phiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_selectedTransaction != null)
                {
                    var existing = (Transaction)_selectedTransaction;
                    // Cập nhật các trường cho phép sửa
                    existing.Amount = decimal.Parse(txtAmount.Text.Trim());
                    existing.Description = txtNote.Text.Trim();
                    existing.CategoryId = categoryId;
                    existing.PartnerId = partnerId;
                    existing.TransDate = dtpTransactionDate.Value;
                    existing.TransType = typeValue;

                    _transactionService.UpdateTransaction(existing);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ResetForm();
                SetInputFieldsEnabled(false);
                RefreshDataGrid(); // Gọi hàm này để Grid cập nhật ngay lập tức
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handle "Cancel" button click
        /// </summary>
        private void OnBtnCancelClicked()
        {
            ResetForm();
            SetInputFieldsEnabled(false);
            _isAddMode = false;
        }

        /// <summary>
        /// Handle "Delete" button click (soft delete via Status field)
        /// </summary>
        private void OnBtnDeleteClicked()
        {
            if (_selectedTransaction == null) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa giao dịch này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // TODO: Call _transactionService.DeleteTransaction(_selectedTransaction.TransId);
                    // Note: TransId is the new column name (was TransactionId)
                    ResetForm();
                    SetInputFieldsEnabled(false);
                    RefreshDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Reset all form input fields
        /// </summary>
        private void ResetForm()
        {
            txtAmount.Text = "";
            txtNote.Text = "";
            dtpTransactionDate.Value = DateTime.Today;
            radIn.Checked = true;
            if (cboPartner.Items.Count > 0) cboPartner.SelectedIndex = 0;
            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = -1;
            _selectedTransaction = null;
        }

        /// <summary>
        /// Enable or disable input fields based on mode
        /// </summary>
        private void SetInputFieldsEnabled(bool enabled)
        {
            radIn.Enabled = enabled;
            radOut.Enabled = enabled;
            txtAmount.Enabled = enabled;
            dtpTransactionDate.Enabled = enabled;
            cboCategory.Enabled = enabled;
            cboPartner.Enabled = enabled;
            txtNote.Enabled = enabled;

            btnSave.Enabled = enabled;
            btnDelete.Enabled = _selectedTransaction != null && enabled;
            btnSave.Text = enabled ? (_isAddMode ? "Lập Phiếu" : "Cập nhật") : "Lưu";
        }

        /// <summary>
        /// Validate user input before saving a transaction
        /// </summary>
        private bool ValidateInput()
        {
            // 1. Kiểm tra số tiền (Amount)
            if (!decimal.TryParse(txtAmount.Text.Trim(), out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ. Vui lòng nhập số dương lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            // 2. Kiểm tra Danh mục (Category)
            if (cboCategory.SelectedValue == null || (int)cboCategory.SelectedValue <= 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục Thu hoặc Chi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategory.Focus();
                return false;
            }

            // Kiểm tra Quỹ tiền
            if (cboFund.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Quỹ tiền (Tiền mặt/Ngân hàng) cho giao dịch này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboFund.Focus();
                return false;
            }

            // 3. Kiểm tra Ngày tháng (TransDate)
            // Không cho phép lưu giao dịch quá xa trong tương lai (ví dụ > 1 năm)
            if (dtpTransactionDate.Value > DateTime.Now.AddYears(1))
            {
                MessageBox.Show("Ngày giao dịch không hợp lệ (vượt quá 1 năm tới)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 4. Kiểm tra Chi nhánh (BranchId) - Rất quan trọng cho Task của bạn
            // Giả sử SessionManager đã lưu BranchId khi user đăng nhập vào Chi nhánh
            if (SessionManager.BranchId == null || SessionManager.BranchId <= 0)
            {
                MessageBox.Show("Không xác định được chi nhánh đang làm việc. Vui lòng đăng nhập lại!", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update summary labels with totals
        /// Uses TransType field (IN/OUT) instead of Type
        /// </summary>
        private void UpdateSummaryLabels()
        {
            // TODO: Calculate totals from current grid data using TransType field
            // decimal totalIn = transactions.Where(t => t.TransType == "IN").Sum(t => t.Amount);
            // decimal totalOut = transactions.Where(t => t.TransType == "OUT").Sum(t => t.Amount);
            // decimal balance = totalIn - totalOut;
            // lblTotalIn.Text = $"Tổng Thu: {totalIn:N0} đ";
            // lblTotalOut.Text = $"Tổng Chi: {totalOut:N0} đ";
            // lblBalance.Text = $"Tồn Quỹ: {balance:N0} đ";
        }

        private void LoadMasterData()
        {
            // 1. Load categories
            // (Giữ nguyên logic load category hiện có của bạn)

            // 2. Load partners
            // (Giữ nguyên logic load partner hiện có của bạn)

            // 3. Load Cash Funds theo branch + role
            var funds = _cashFundService.GetFundsByBranch(
                SessionManager.TenantId,
                SessionManager.BranchId ?? 0,
                SessionManager.RoleId
            );

            cboFund.DataSource = funds;
            cboFund.DisplayMember = "FundName";
            cboFund.ValueMember = "FundId";

            if (funds != null && funds.Count > 0)
            {
                cboFund.SelectedIndex = 0;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }
    }
}
