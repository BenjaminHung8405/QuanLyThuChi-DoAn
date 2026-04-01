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

namespace QuanLyThuChi_DoAn
{
    public partial class ucTransaction : UserControl
    {
        private readonly TransactionService _transactionService;
        private readonly CashFundService _cashFundService;
        private readonly CategoryService _categoryService;
        private readonly PartnerService _partnerService;
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
                txtAmount.Text = selected.Amount.ToString("N0");
                txtNote.Text = selected.Description;

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

            if (SessionManager.RoleName == "Staff" && !_isAddMode)
            {
                MessageBox.Show("Bạn không có quyền sửa phiếu sau 24h hoặc của người khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
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

            if (SessionManager.RoleName == "Staff")
            {
                MessageBox.Show("Bạn không có quyền xóa giao dịch.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa giao dịch này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var tx = (Transaction)_selectedTransaction;
                    if (SessionManager.RoleName == "Admin")
                    {
                        // Xóa vĩnh viễn
                        _transactionService.DeleteTransaction(tx.TransId, tx.TenantId);
                    }
                    else
                    {
                        // Soft delete
                        _transactionService.DeleteTransaction(tx.TransId, tx.TenantId);
                    }

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

            ApplyPermissionRules();
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

            // Admin có quyền xóa vĩnh viễn mới hiển thị btnDelete (gắn chức năng riêng khi cần)
            if (SessionManager.RoleName != "Admin")
            {
                btnDelete.Text = "Xóa";
            }
            else
            {
                btnDelete.Text = "Xóa vĩnh viễn";
            }
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

        public void LoadMasterData()
        {
            try
            {
                if (!SessionManager.CurrentTenantId.HasValue || !SessionManager.CurrentBranchId.HasValue)
                {
                    // Không có ngữ cảnh tenant/branch đầy đủ
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu Master: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
