using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Graphical_User_Interface;

namespace QuanLyThuChi_DoAn
{
    public partial class ucTransaction : UserControl
    {
        private readonly TransactionService _transactionService;
        private readonly CashFundService _cashFundService;
        private readonly CategoryService _categoryService;
        private readonly PartnerService _partnerService;
        private readonly TaxService _taxService;
        private bool _isAddMode = false;
        private object _selectedTransaction = null;

        public ucTransaction()
        {
            InitializeComponent();
            var context = new AppDbContext();
            _transactionService = new TransactionService(context);
            _cashFundService = new CashFundService(context);
            _categoryService = new CategoryService(context);
            _partnerService = new PartnerService(context);
            _taxService = new TaxService(context);
        }

        private async void ucTransaction_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Set default date range: First day to last day of current month
                DateTime today = DateTime.Today;
                dtpFromDate.Value = new DateTime(today.Year, today.Month, 1); // ✅ FROM = first day
                dtpToDate.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)); // ✅ TO = last day

                dtpTransactionDate.Value = DateTime.Now;
                dtpTransactionDate.Enabled = false;
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

                // 4.1.1. Load taxes into cbTax from database
                await LoadTaxesAsync();

                // 4.2. Load filter dropdowns (category + partner) with "Tất cả" item
                LoadFilterControls();

                // 5. Refresh data
                RefreshDataGrid();

                // 6. Apply action-based permissions
                ApplyPermissionRules();
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

            // Auto-calculate tax and total
            txtSubTotal.ValueChanged += txtSubTotal_ValueChanged;
            cbTax.SelectedIndexChanged += cbTax_SelectedIndexChanged;

            // Filter and refresh buttons
            btnRefresh.Click += (s, e) =>
            {
                ResetForm();
                if (cboFilterCategory.Items.Count > 0) cboFilterCategory.SelectedIndex = 0;
                if (cboFilterPartner.Items.Count > 0) cboFilterPartner.SelectedIndex = 0;
                SetInputFieldsEnabled(false);
                RefreshDataGrid(applyDateFilter: false);
            };

            // CRUD buttons
            btnNew.Click += (s, e) => OnBtnNewClicked();
            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => OnBtnCancelClicked();
            btnDelete.Click += btnDelete_Click;

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
            string type = radIn.Checked ? "IN" : "OUT";

            // Lọc category theo loại giao dịch (IN/OUT)
            var categories = _categoryService.GetCategoriesForCurrentSession(type);
            cboCategory.DataSource = categories;
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryId";
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
        public void RefreshDataGrid(bool applyDateFilter = false, int selectedCategoryId = 0, int selectedPartnerId = 0)
        {
            try
            {
                var keyword = txtSearch.Text.Trim();

                // Use safe date ranges (2000-2099) instead of DateTime.MinValue/MaxValue
                // This avoids SQL provider exceptions with year 0001 or 9999
                var fromDate = applyDateFilter ? dtpFromDate.Value : new DateTime(2000, 1, 1); // ✅ FIXED: Use dtpFromDate for FROM
                var toDate = applyDateFilter ? dtpToDate.Value : new DateTime(2099, 12, 31);   // ✅ FIXED: Use dtpToDate for TO

                var transactions = _transactionService.GetTransactionsForCurrentSession(
                    fromDate,
                    toDate,
                    keyword
                );

                // Áp dụng filter danh mục/đối tác
                int filterCategoryId = selectedCategoryId;
                int filterPartnerId = selectedPartnerId;

                if (filterCategoryId == 0 && cboFilterCategory.SelectedValue != null && int.TryParse(cboFilterCategory.SelectedValue.ToString(), out var selectedCatId))
                {
                    filterCategoryId = selectedCatId;
                }

                if (filterPartnerId == 0 && cboFilterPartner.SelectedValue != null && int.TryParse(cboFilterPartner.SelectedValue.ToString(), out var selectedPartnerId2))
                {
                    filterPartnerId = selectedPartnerId2;
                }

                if (filterCategoryId > 0)
                {
                    transactions = transactions.Where(t => t.CategoryId == filterCategoryId).ToList();
                }

                if (filterPartnerId > 0)
                {
                    transactions = transactions.Where(t => t.PartnerId == filterPartnerId).ToList();
                }

                dgvTransactions.DataSource = null;
                dgvTransactions.DataSource = transactions;

                decimal totalIn = transactions.Where(t => t.TransType == "IN").Sum(t => t.Amount);
                decimal totalOut = transactions.Where(t => t.TransType == "OUT").Sum(t => t.Amount);
                decimal balance = totalIn - totalOut;

                lblTotalIn.Text = $"Tổng Thu: {totalIn:N0} đ";
                lblTotalOut.Text = $"Tổng Chi: {totalOut:N0} đ";

                IEnumerable<CashFund> cashFunds;
                if (SessionManager.CurrentTenantId.HasValue && SessionManager.CurrentBranchId.HasValue)
                {
                    cashFunds = _cashFundService.GetFundsByBranch(SessionManager.CurrentTenantId.Value, SessionManager.CurrentBranchId.Value, SessionManager.RoleId);
                }
                else
                {
                    cashFunds = _cashFundService.GetVisibleFunds(SessionManager.RoleId);
                }

                var realBalance = cashFunds.Sum(f => f.Balance);
                lblBalance.Text = $"Số dư quỹ hiện tại: {realBalance:N0} đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadData()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LoadData));
                return;
            }

            RefreshDataGrid();
        }

        public void ReloadContextAndData()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ReloadContextAndData));
                return;
            }

            LoadMasterData();
            LoadFilterControls();
            RefreshDataGrid();
        }

        public void LoadData(int categoryId = 0, int partnerId = 0)
        {
            // 1. Base query
            var query = _transactionService.GetTransactionsQueryForCurrentSession();

            // 2. Filter date-range
            var fromDate = dtpFromDate.Value.Date;
            var toDate = dtpToDate.Value.Date;
            query = query.Where(t => t.TransDate.Date >= fromDate && t.TransDate.Date <= toDate);

            // 3. Filter UI
            if (categoryId > 0)
            {
                query = query.Where(t => t.CategoryId == categoryId);
            }

            if (partnerId > 0)
            {
                query = query.Where(t => t.PartnerId == partnerId);
            }

            // 4. Execute
            var result = query.OrderByDescending(t => t.TransDate).ToList();

            this.Invoke(new Action(() =>
            {
                if (result.Count == 0)
                {
                    dgvTransactions.DataSource = null;
                    MessageBox.Show("Không tìm thấy giao dịch phù hợp với điều kiện lọc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dgvTransactions.DataSource = result;
                }

                var totalIn = result.Where(t => t.TransType == "IN").Sum(t => t.Amount);
                var totalOut = result.Where(t => t.TransType == "OUT").Sum(t => t.Amount);
                var balance = totalIn - totalOut;

                lblTotalIn.Text = $"Tổng Thu: {totalIn:N0} đ";
                lblTotalOut.Text = $"Tổng Chi: {totalOut:N0} đ";

                // Cập nhật số dư theo branch đang chọn ở frmMain
                IEnumerable<CashFund> cashFunds;
                if (SessionManager.CurrentTenantId.HasValue && SessionManager.CurrentBranchId.HasValue)
                {
                    cashFunds = _cashFundService.GetFundsByBranch(SessionManager.CurrentTenantId.Value, SessionManager.CurrentBranchId.Value, SessionManager.RoleId);
                }
                else
                {
                    // SuperAdmin hoặc không có branch, hiển thị dựa trên quyền
                    cashFunds = _cashFundService.GetVisibleFunds(SessionManager.RoleId);
                }

                var realBalance = cashFunds.Sum(f => f.Balance);
                lblBalance.Text = $"Số dư quỹ hiện tại: {realBalance:N0} đ";
            }));
        }

        private Task LoadDataAsync()
        {
            RefreshDataGrid();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Format grid cells (color, currency format, etc.)
        /// Updated for new column names: TransType, Description, Amount
        /// </summary>
        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvTransactions.Columns[e.ColumnIndex].Name;

            // Color code transaction type and map to Thu/Chi
            if (colName == "colTransType")
            {
                var row = dgvTransactions.Rows[e.RowIndex];
                var transaction = row.DataBoundItem as Transaction;
                if (transaction == null) return;

                string type = transaction.TransType?.Trim().ToUpperInvariant();
                var isIn = type == "IN";

                e.Value = isIn ? "Thu" : "Chi";
                e.CellStyle.ForeColor = isIn ? Color.MediumSeaGreen : Color.Crimson;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                e.FormattingApplied = true;
                return;
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
                var selected = dgvTransactions.Rows[rowIndex].DataBoundItem as Transaction;
                if (selected == null) return;

                _selectedTransaction = selected;

                // Populate form fields from selected transaction
                dtpTransactionDate.Value = selected.TransDate;
                decimal subTotalValue = selected.SubTotal > 0 ? selected.SubTotal : selected.Amount;
                txtSubTotal.Value = Math.Min(txtSubTotal.Maximum, Math.Max(txtSubTotal.Minimum, subTotalValue));
                txtNote.Text = selected.Description;

                if (selected.TaxId.HasValue && selected.TaxId.Value > 0)
                {
                    cbTax.SelectedValue = selected.TaxId.Value;
                }
                else if (cbTax.Items.Count > 0)
                {
                    cbTax.SelectedIndex = 0;
                }

                UpdateTaxAndTotalPreview();

                string tx = selected.TransType?.Trim().ToUpperInvariant();
                radIn.Checked = tx == "IN";
                radOut.Checked = tx == "OUT";

                // Load categories theo proc nếu chưa được set
                OnTransactionTypeChanged();
                if (selected.CategoryId > 0)
                {
                    cboCategory.SelectedValue = selected.CategoryId;
                }

                if (selected.PartnerId.HasValue)
                {
                    cboPartner.SelectedValue = selected.PartnerId.Value;
                }
                else if (cboPartner.Items.Count > 0)
                {
                    cboPartner.SelectedIndex = 0;
                }

                if (selected.FundId > 0)
                {
                    cboFund.SelectedValue = selected.FundId;
                }

                _isAddMode = false;
                SetInputFieldsEnabled(true);

                if (SessionManager.RoleName == "Staff")
                {
                    bool isOwner = selected.CreatedBy == SessionManager.UserId;
                    bool within24h = selected.TransDate >= DateTime.Now.AddHours(-24);
                    btnSave.Enabled = isOwner && within24h;
                    btnDelete.Enabled = false;
                }

                ApplyPermissionRules();
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
            txtSubTotal.Focus();
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            await SaveTransactionAsync();
        }

        /// <summary>
        /// Handle "Save" button click
        /// Updated for new schema with TransType, Description, RefNo, CreatedBy, Status, FundId
        /// </summary>
        private async Task SaveTransactionAsync()
        {
            // Gọi hàm kiểm tra trước khi làm bất cứ việc gì khác
            if (!ValidateInput()) return;

            if (SessionManager.RoleName == "Staff" && !_isAddMode)
            {
                MessageBox.Show("Bạn không có quyền sửa phiếu sau 24h hoặc của người khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSave.Enabled = false;

                if (!SessionManager.CurrentTenantId.HasValue)
                {
                    MessageBox.Show("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!SessionManager.BranchId.HasValue)
                {
                    MessageBox.Show("Không có chi nhánh ngữ cảnh. Vui lòng chọn chi nhánh.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string typeValue = radIn.Checked ? "IN" : "OUT";
                int categoryId = (int)cboCategory.SelectedValue;
                int? partnerId = (cboPartner.SelectedValue == null || (int)cboPartner.SelectedValue == 0) ? (int?)null : (int)cboPartner.SelectedValue;
                int? taxId = GetSelectedTaxId();

                decimal subTotal = txtSubTotal.Value;
                if (subTotal <= 0)
                {
                    MessageBox.Show("Số tiền trước thuế không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSubTotal.Focus();
                    return;
                }

                bool isSuccess = false;

                if (_isAddMode)
                {
                    var newTrans = new Transaction
                    {
                        // TỰ ĐỘNG GÁN TỪ PHIÊN ĐĂNG NHẬP
                        TenantId = SessionManager.CurrentTenantId.Value,
                        BranchId = SessionManager.BranchId.Value,   // Ví dụ: 4 (Chi nhánh Long Xuyên)
                        CreatedBy = SessionManager.UserId,    // Ví dụ: 2 (Admin)

                        // DỮ LIỆU TỪ FORM
                        FundId = (int)cboFund.SelectedValue,
                        CategoryId = categoryId,
                        PartnerId = partnerId,
                        TransDate = DateTime.Now,
                        SubTotal = subTotal,
                        TaxId = taxId,
                        Amount = subTotal,
                        Description = txtNote.Text.Trim(),
                        TransType = typeValue,
                        RefNo = $"TR-{DateTime.Now:yyyyMMddHHmmss}",
                        Status = "COMPLETED",
                        IsActive = true // Luôn bật IsActive khi tạo mới
                    };

                    isSuccess = await _transactionService.AddTransactionAsync(newTrans);
                }
                else if (_selectedTransaction != null)
                {
                    var existing = (Transaction)_selectedTransaction;
                    // Cập nhật các trường cho phép sửa
                    existing.SubTotal = subTotal;
                    existing.TaxId = taxId;
                    existing.Amount = subTotal;
                    existing.Description = txtNote.Text.Trim();
                    existing.CategoryId = categoryId;
                    existing.PartnerId = partnerId;
                    existing.TransType = typeValue;

                    _transactionService.UpdateTransaction(existing);
                    isSuccess = true;
                }

                if (!isSuccess)
                {
                    MessageBox.Show("Không thể lưu phiếu. Vui lòng thử lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(_isAddMode ? "Lập phiếu thành công!" : "Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetForm();
                SetInputFieldsEnabled(false);
                RefreshDataGrid(); // Gọi hàm này để Grid cập nhật ngay lập tức
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Chỉ mở lại nút khi form đang ở trạng thái nhập/sửa.
                if (txtSubTotal.Enabled)
                {
                    btnSave.Enabled = true;
                }
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

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn phiếu nào chưa
            if (dgvTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu giao dịch để hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy ID của phiếu đang chọn
            var selectedRow = dgvTransactions.SelectedRows[0];
            long transId = 0;

            if (selectedRow.DataBoundItem is Transaction selectedTransaction)
            {
                transId = selectedTransaction.TransId;
            }
            else
            {
                if (dgvTransactions.Columns.Contains("TransactionId")
                    && selectedRow.Cells["TransactionId"].Value != null
                    && long.TryParse(selectedRow.Cells["TransactionId"].Value.ToString(), out var idFromTransactionId))
                {
                    transId = idFromTransactionId;
                }
                else if (dgvTransactions.Columns.Contains("colTransId")
                    && selectedRow.Cells["colTransId"].Value != null
                    && long.TryParse(selectedRow.Cells["colTransId"].Value.ToString(), out var idFromColTransId))
                {
                    transId = idFromColTransId;
                }
            }

            if (transId <= 0)
            {
                MessageBox.Show("Không xác định được mã giao dịch để hủy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // (Tùy chọn) Kiểm tra xem phiếu này đã bị hủy chưa
            if (selectedRow.DataBoundItem is Transaction selectedTx)
            {
                if (!selectedTx.IsActive)
                {
                    MessageBox.Show("Phiếu này đã bị hủy từ trước, không thể hủy thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (dgvTransactions.Columns.Contains("IsActive") && selectedRow.Cells["IsActive"].Value != null)
            {
                bool isActive = Convert.ToBoolean(selectedRow.Cells["IsActive"].Value);
                if (!isActive)
                {
                    MessageBox.Show("Phiếu này đã bị hủy từ trước, không thể hủy thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 3. Cảnh báo xác nhận lần 1
            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn HỦY tờ phiếu này không?\nHành động này sẽ được lưu vào Nhật ký hệ thống (Audit Log).",
                "Xác nhận Hủy Phiếu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                // 4. Mở Form bắt buộc nhập lý do
                using (var frmReason = new frmInputReason())
                {
                    if (frmReason.ShowDialog() == DialogResult.OK)
                    {
                        bool previousDeleteEnabled = btnDelete.Enabled;
                        try
                        {
                            btnDelete.Enabled = false;

                            // Lấy lý do Kế toán vừa gõ
                            string reason = frmReason.CancelReason;

                            // 5. Gọi Service "2 trong 1" (Vừa Xóa mềm, vừa Ghi Log)
                            bool isSuccess = await _transactionService.CancelTransactionAsync(transId, reason);

                            if (isSuccess)
                            {
                                MessageBox.Show("Đã hủy phiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // 6. Load lại lưới dữ liệu
                                await LoadDataAsync();
                            }
                            else
                            {
                                MessageBox.Show("Không thể hủy phiếu. Vui lòng thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi hủy phiếu: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            btnDelete.Enabled = previousDeleteEnabled;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reset all form input fields
        /// </summary>
        private void ResetForm()
        {
            txtSubTotal.Value = 0;
            txtTaxAmount.Text = "0";
            txtTotalAmount.Text = "0";
            txtNote.Text = "";
            dtpTransactionDate.Value = DateTime.Now;
            radIn.Checked = true;
            if (cboPartner.Items.Count > 0) cboPartner.SelectedIndex = 0;
            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = -1;
            if (cbTax.Items.Count > 0) cbTax.SelectedIndex = 0;
            _selectedTransaction = null;

            UpdateTaxAndTotalPreview();

            ApplyPermissionRules();
        }

        /// <summary>
        /// Enable or disable input fields based on mode
        /// </summary>
        private void SetInputFieldsEnabled(bool enabled)
        {
            radIn.Enabled = enabled;
            radOut.Enabled = enabled;
            txtSubTotal.Enabled = enabled;
            cbTax.Enabled = enabled;
            dtpTransactionDate.Enabled = false;
            cboCategory.Enabled = enabled;
            cboPartner.Enabled = enabled;
            txtNote.Enabled = enabled;

            bool canEdit = enabled;
            if (SessionManager.RoleName == "Staff")
            {
                // Nhân viên chỉ cho thêm mới, không sửa nếu không thoả điều kiện
                if (!_isAddMode)
                {
                    canEdit = false;
                }
            }

            btnSave.Enabled = canEdit;
            btnDelete.Enabled = _selectedTransaction != null && enabled && SessionManager.RoleName != "Staff";
            btnSave.Text = enabled ? (_isAddMode ? "Lập Phiếu" : "Cập nhật") : "Lưu";

            btnDelete.Text = "Hủy Phiếu";
        }

        /// <summary>
        /// Validate user input before saving a transaction
        /// </summary>
        private bool ValidateInput()
        {
            // 1. Kiểm tra số tiền (Amount)
            decimal amount = txtSubTotal.Value;
            if (amount <= 0)
            {
                MessageBox.Show("Số tiền trước thuế không hợp lệ. Vui lòng nhập số dương lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubTotal.Focus();
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

            // 3. Kiểm tra Chi nhánh (BranchId) - Rất quan trọng cho Task của bạn
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

        public void LoadMasterData()
        {
            try
            {
                if (!SessionManager.CurrentTenantId.HasValue
                    || !SessionManager.CurrentBranchId.HasValue
                    || SessionManager.CurrentBranchId.Value <= 0)
                {
                    cboFund.DataSource = null;
                    cboFund.Items.Clear();
                    return;
                }

                int currentTenantId = SessionManager.CurrentTenantId.Value;

                // 1. Load Danh mục (Category) theo loại Thu/Chi hiện tại
                OnTransactionTypeChanged();

                // 2. Load Đối tác (Partner)
                var partners = _partnerService.GetPartnersByTenant(currentTenantId);
                cboPartner.DataSource = partners;
                cboPartner.DisplayMember = "PartnerName";
                cboPartner.ValueMember = "PartnerId";
                cboPartner.SelectedIndex = -1; // giữ trạng thái chưa chọn

                // 3. Load Cash Funds theo context branch hiện tại (chi nhánh đang chọn)
                int currentBranchId = SessionManager.CurrentBranchId.Value;
                var funds = _cashFundService.GetFundsByBranchId(SessionManager.CurrentTenantId.Value, currentBranchId);

                cboFund.DataSource = funds;
                cboFund.DisplayMember = "FundName";
                cboFund.ValueMember = "FundId";

                if (funds != null && funds.Count > 0)
                {
                    cboFund.SelectedIndex = 0;
                }
                else
                {
                    cboFund.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu Master: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTaxesAsync()
        {
            try
            {
                var taxes = await _taxService.GetActiveTaxesAsync();
                taxes.Insert(0, new Tax
                {
                    TaxId = 0,
                    TaxName = "Không áp thuế",
                    Rate = 0,
                    IsActive = true,
                    TenantId = SessionManager.CurrentTenantId ?? 0
                });

                cbTax.DataSource = taxes;
                cbTax.DisplayMember = nameof(Tax.TaxName);
                cbTax.ValueMember = nameof(Tax.TaxId);
                cbTax.SelectedIndex = 0;

                UpdateTaxAndTotalPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh mục Thuế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetSelectedTaxId()
        {
            if (cbTax.SelectedValue == null)
            {
                return null;
            }

            if (int.TryParse(cbTax.SelectedValue.ToString(), out int selectedTaxId) && selectedTaxId > 0)
            {
                return selectedTaxId;
            }

            return null;
        }

        private decimal GetSelectedTaxRate()
        {
            if (cbTax.SelectedItem is Tax selectedTax)
            {
                return selectedTax.Rate;
            }

            return 0;
        }

        // Sự kiện khi Kế toán thay đổi số tiền trước thuế
        private void txtSubTotal_ValueChanged(object? sender, EventArgs e)
        {
            CalculateTotal();
        }

        // Sự kiện khi thay đổi thuế suất
        private void cbTax_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CalculateTotal();
        }

        // HÀM TÍNH TOÁN TRUNG TÂM
        private void CalculateTotal()
        {
            decimal subTotal = txtSubTotal.Value;
            if (subTotal <= 0)
            {
                txtTaxAmount.Text = "0";
                txtTotalAmount.Text = "0";
                return;
            }

            decimal taxRate = GetSelectedTaxRate();
            decimal taxAmount = Math.Round(subTotal * (taxRate / 100m), 0, MidpointRounding.AwayFromZero);
            decimal totalAmount = subTotal + taxAmount;

            txtTaxAmount.Text = taxAmount.ToString("N0");
            txtTotalAmount.Text = totalAmount.ToString("N0");
        }

        private void UpdateTaxAndTotalPreview()
        {
            CalculateTotal();
        }

        private void ApplyPermissionRules()
        {
            string role = SessionManager.RoleName;

            if (role == "Staff")
            {
                // Staff chỉ được thêm mới
                btnDelete.Enabled = false;
                if (!_isAddMode)
                {
                    btnSave.Enabled = false;
                    SetInputFieldsEnabled(false);
                }
            }
            else if (role == "Admin")
            {
                btnDelete.Visible = true;
            }
            else
            {
                // BranchManager, SuperAdmin...
                btnDelete.Visible = true;
            }
        }

        private void LoadFilterControls()
        {
            try
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                {
                    return;
                }

                int currentTenantId = SessionManager.CurrentTenantId.Value;

                // Category filter (include "Tất cả")
                var categories = _categoryService.GetCategoriesForCurrentSession();
                categories.Insert(0, new TransactionCategory
                {
                    CategoryId = 0,
                    CategoryName = "--- Tất cả danh mục ---"
                });

                cboFilterCategory.DataSource = categories;
                cboFilterCategory.DisplayMember = "CategoryName";
                cboFilterCategory.ValueMember = "CategoryId";
                cboFilterCategory.SelectedIndex = 0;

                // Partner filter (include "Tất cả")
                var partners = _partnerService.GetPartnersByTenant(currentTenantId);
                partners.Insert(0, new Partner
                {
                    PartnerId = 0,
                    PartnerName = "--- Tất cả đối tác ---"
                });

                cboFilterPartner.DataSource = partners;
                cboFilterPartner.DisplayMember = "PartnerName";
                cboFilterPartner.ValueMember = "PartnerId";
                cboFilterPartner.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu lọc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            int selectedCategoryId = 0;
            int selectedPartnerId = 0;

            if (cboFilterCategory.SelectedValue != null && int.TryParse(cboFilterCategory.SelectedValue.ToString(), out var catId))
            {
                selectedCategoryId = catId;
            }

            if (cboFilterPartner.SelectedValue != null && int.TryParse(cboFilterPartner.SelectedValue.ToString(), out var partnerId))
            {
                selectedPartnerId = partnerId;
            }

            var mainForm = this.FindForm() as frmMain;
            if (mainForm != null)
            {
                mainForm.SetLoadingState(true);
            }

            btnFilter.Enabled = false;
            try
            {
                await Task.Run(() => LoadData(selectedCategoryId, selectedPartnerId));
            }
            finally
            {
                if (mainForm != null)
                {
                    mainForm.SetLoadingState(false);
                }
                btnFilter.Enabled = true;
            }
        }
    }
}
