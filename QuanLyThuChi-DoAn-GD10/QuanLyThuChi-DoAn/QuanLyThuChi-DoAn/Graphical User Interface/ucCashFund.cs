using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.BLL.Services;

namespace QuanLyThuChi_DoAn
{
    public partial class ucCashFund : UserControl
    {
        private readonly AppDbContext _context;
        private readonly CashFundService _cashFundService;
        private readonly BranchService _branchService;
        private bool _isAddMode = false;
        private CashFund _selectedFund = null;

        public ucCashFund()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _cashFundService = new CashFundService(_context);
            _branchService = new BranchService(_context);

            // Hook load event
            this.Load += UcCashFund_Load;

            // Connect missing button event handler
            btnSyncSelected.Click += btnSyncSelected_Click;

            dgvCashFunds.CellValueChanged += dgvCashFunds_CellValueChanged;
            dgvCashFunds.CurrentCellDirtyStateChanged += dgvCashFunds_CurrentCellDirtyStateChanged;

            // Initial state
            ResetForm();
            LoadData();
        }

        private void UcCashFund_Load(object sender, EventArgs e)
        {
            ApplyAuthorization();
            LoadData();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ResetForm()
        {
            txtFundName.Text = "";
            txtAccountNumber.Text = "";
            txtBalance.Value = 0;
            _selectedFund = null;
            _isAddMode = false;
            SetInputFieldsEnabled(false);
        }

        private void SetInputFieldsEnabled(bool enabled)
        {
            txtFundName.Enabled = enabled;
            txtAccountNumber.Enabled = enabled;
            txtBalance.Enabled = enabled && _isAddMode;

            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            btnDelete.Enabled = _selectedFund != null;
        }

        public void LoadFunds()
        {
            var funds = _context.CashFunds
                .Where(f => f.IsActive)
                .ToList();

            dgvCashFunds.DataSource = null;
            dgvCashFunds.DataSource = funds;

            lblTotalBalance.Text = funds.Sum(f => f.Balance).ToString("N0");

            ResetForm();
            SetInputFieldsEnabled(false); btnSyncSelected.Enabled = false;
        }

        public void LoadData()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LoadData));
                return;
            }

            // 1. Load danh sách chi nhánh cho admin và chọn mặc định
            LoadBranches();

            // 2. Xóa dữ liệu cũ trên Grid
            dgvCashFunds.Rows.Clear();

            // 2. Lấy danh sách quỹ theo vai trò / quyền truy cập
            var funds = _cashFundService.GetVisibleFunds(SessionManager.RoleId);

            // 3. Đổ từng dòng vào Grid và kiểm tra trạng thái
            foreach (var fund in funds)
            {
                decimal actualBalance = _cashFundService.CalculateActualBalance(fund.FundId);
                bool isMatched = (fund.Balance == actualBalance);
                string statusText = isMatched ? "✔️ Khớp" : "⚠️ Lệch dữ liệu";

                int rowIndex = dgvCashFunds.Rows.Add(false, fund.FundId, fund.FundName, fund.Branch?.BranchName ?? "", fund.Balance, actualBalance, statusText);

                if (!isMatched)
                {
                    dgvCashFunds.Rows[rowIndex].Cells["colStatus"].Style.ForeColor = Color.Red;
                    dgvCashFunds.Rows[rowIndex].Cells["colStatus"].Style.Font = new Font(dgvCashFunds.Font, FontStyle.Bold);
                }
                else
                {
                    dgvCashFunds.Rows[rowIndex].Cells["colStatus"].Style.ForeColor = Color.Green;
                }
            }

            // Cập nhật tổng số dư trên sổ
            lblTotalBalance.Text = funds.Sum(f => f.Balance).ToString("N0");

            ResetForm();
            SetInputFieldsEnabled(false);
            btnSyncSelected.Enabled = false;
        }

        private void LoadBranches()
        {
            List<Branch> branches;

            if (SessionManager.RoleId == 1)
            {
                // SuperAdmin: xem tất cả chi nhánh
                branches = _branchService.GetAllActiveBranches();
            }
            else if (SessionManager.RoleId == 2)
            {
                // TenantManager: xem chi nhánh trong tenant
                if (!SessionManager.CurrentTenantId.HasValue)
                {
                    branches = new List<Branch>();
                }
                else
                {
                    branches = _branchService.GetBranchesByTenant(SessionManager.CurrentTenantId.Value);
                }
            }
            else if (SessionManager.RoleId == 3)
            {
                // BranchManager: chỉ chi nhánh hiện tại
                if (!SessionManager.CurrentBranchId.HasValue || !SessionManager.CurrentTenantId.HasValue)
                {
                    branches = new List<Branch>();
                }
                else
                {
                    var currentBranchId = SessionManager.CurrentBranchId.Value;
                    branches = _branchService.GetBranchesByTenant(SessionManager.CurrentTenantId.Value)
                        .Where(b => b.BranchId == currentBranchId)
                        .ToList();
                }
            }
            else
            {
                branches = new List<Branch>();
            }

            cboBranch.DataSource = branches;
            cboBranch.DisplayMember = "BranchName";
            cboBranch.ValueMember = "BranchId";

            if (SessionManager.RoleId == 3 && branches.Any())
            {
                cboBranch.SelectedValue = SessionManager.CurrentBranchId;
            }
            else
            {
                cboBranch.SelectedIndex = -1;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (SessionManager.RoleId != 1 && SessionManager.RoleId != 2)
            {
                MessageBox.Show("Bạn không có quyền thêm quỹ mới.", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isAddMode = true;
            _selectedFund = null;
            txtFundName.Text = "";
            txtAccountNumber.Text = "";
            txtBalance.Value = 0;
            SetInputFieldsEnabled(true);
            txtFundName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFundName.Text))
            {
                MessageBox.Show("Tên quỹ không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFundName.Focus();
                return;
            }

            decimal initialBalance = txtBalance.Value;

            if (_isAddMode)
            {
                if (SessionManager.RoleId != 1 && SessionManager.RoleId != 2)
                {
                    MessageBox.Show("Bạn không có quyền lưu quỹ mới.", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboBranch.SelectedValue == null || (int)cboBranch.SelectedValue <= 0)
                {
                    MessageBox.Show("Vui lòng chọn Chi nhánh cho quỹ mới.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedBranchId = (int)cboBranch.SelectedValue;
                var branchAllowed = ((List<Branch>)cboBranch.DataSource)?.Any(b => b.BranchId == selectedBranchId) == true;
                if (!branchAllowed)
                {
                    MessageBox.Show("Chi nhánh được chọn không thuộc quyền của bạn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!SessionManager.TenantId.HasValue)
                {
                    MessageBox.Show("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newFund = new CashFund
                {
                    FundName = txtFundName.Text.Trim(),
                    AccountNumber = txtAccountNumber.Text.Trim(),
                    Balance = initialBalance,
                    IsActive = true,
                    BranchId = selectedBranchId,
                    TenantId = SessionManager.TenantId.Value
                };

                _context.CashFunds.Add(newFund);
                _context.SaveChanges();
                MessageBox.Show("Thêm quỹ thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (_selectedFund == null)
                {
                    MessageBox.Show("Không có quỹ được chọn để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var existing = _context.CashFunds.Find(_selectedFund.FundId);
                if (existing != null)
                {
                    existing.FundName = txtFundName.Text.Trim();
                    existing.AccountNumber = txtAccountNumber.Text.Trim();
                    // KHÔNG SỬA SỐ DƯ KHI CẬP NHẬT
                    _context.SaveChanges();
                    MessageBox.Show("Cập nhật quỹ thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadFunds();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetInputFieldsEnabled(false);
            _isAddMode = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedFund == null)
            {
                MessageBox.Show("Vui lòng chọn quỹ để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fundInDb = _context.CashFunds.Find(_selectedFund.FundId);
            if (fundInDb == null)
            {
                MessageBox.Show("Quỹ không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (fundInDb.Balance > 0)
            {
                MessageBox.Show("Không thể xóa quỹ đang còn tiền. Vui lòng chuyển hết tiền sang quỹ khác hoặc tạo phiếu chi trước khi xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            fundInDb.IsActive = false;
            _context.SaveChanges();

            MessageBox.Show("Quỹ đã được ẩn (xóa mềm).", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadFunds();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvCashFunds_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvCashFunds.Rows[e.RowIndex].DataBoundItem as CashFund;
            if (row == null) return;

            _selectedFund = row;
            _isAddMode = false;

            txtFundName.Text = row.FundName;
            txtAccountNumber.Text = row.AccountNumber;
            txtBalance.Value = Math.Min(txtBalance.Maximum, Math.Max(txtBalance.Minimum, row.Balance));

            // Enable sync/audit only when there is a selected fund
            btnSyncSelected.Enabled = true;

            // Nếu user là staff, chỉ xem không được sửa
            if (SessionManager.RoleId == 3)
            {
                txtFundName.Enabled = false;
                txtAccountNumber.Enabled = false;
                txtBalance.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
            }
            else
            {
                SetInputFieldsEnabled(true);
                txtBalance.Enabled = false; // Không cho sửa số dư khi edit
                btnSave.Text = "Cập nhật";
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            if (_selectedFund == null)
            {
                MessageBox.Show("Vui lòng chọn một quỹ để đối soát.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _cashFundService.AuditFund(_selectedFund.FundId);

            if (result.IsValid)
            {
                MessageBox.Show($"Quỹ [{result.FundName}] đã đồng bộ chính xác. Số dư hiện tại: {result.CurrentBalance:N0} đ.", "Đối soát hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Cảnh báo: Quỹ [{result.FundName}] không đồng bộ.\n" +
                                $"Số dư hiện tại: {result.CurrentBalance:N0} đ\n" +
                                $"Tính theo giao dịch: {result.CalculatedBalance:N0} đ\n" +
                                $"Chênh lệch: {result.Difference:N0} đ", "Lỗi đối soát", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvCashFunds_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCashFunds.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvCashFunds.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvCashFunds_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvCashFunds.Columns[e.ColumnIndex].Name == "colCheck")
            {
                UpdateSyncButtonState();
            }
        }

        private void UpdateSyncButtonState()
        {
            bool anyChecked = dgvCashFunds.Rows.Cast<DataGridViewRow>()
                .Any(r => !r.IsNewRow && r.Cells["colCheck"].Value != null && Convert.ToBoolean(r.Cells["colCheck"].Value));

            btnSyncSelected.Enabled = anyChecked && SessionManager.RoleId == 2;
        }

        private void dgvCashFunds_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var columnName = dgvCashFunds.Columns[e.ColumnIndex].Name;
            if (columnName != "colSysBalance" && columnName != "colActualBalance" && columnName != "colStatus")
                return;

            if (e.Value == null || e.Value == DBNull.Value)
                return;

            if (columnName == "colStatus")
            {
                // colStatus đã được điền sẵn "✔️ Khớp" hoặc "⚠️ Lệch dữ liệu" trong LoadData()
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.Font = new Font(dgvCashFunds.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = e.Value.ToString().Contains("Khớp") ? Color.Green : Color.Red;
                }
                return;
            }

            if (!decimal.TryParse(Convert.ToString(e.Value), out decimal balance))
                return;

            e.CellStyle.Format = "N0";
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            e.Value = string.Format("{0:#,0} đ", balance);

            if (balance < 500_000m)
            {
                e.CellStyle.ForeColor = Color.Crimson;
                e.CellStyle.Font = new Font(dgvCashFunds.Font, FontStyle.Bold);
            }
            else if (balance > 0m)
            {
                e.CellStyle.ForeColor = Color.Navy;
                e.CellStyle.Font = new Font(dgvCashFunds.Font, FontStyle.Regular);
            }
            else
            {
                e.CellStyle.ForeColor = dgvCashFunds.DefaultCellStyle.ForeColor;
                e.CellStyle.Font = dgvCashFunds.DefaultCellStyle.Font;
            }
        }

        private void ApplyAuthorization()
        {
            if (SessionManager.RoleId == 1) // SuperAdmin
            {
                btnNew.Visible = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDelete.Visible = true;

                txtFundName.Enabled = true;
                txtAccountNumber.Enabled = true;
                cboBranch.Enabled = true;
                txtBalance.Enabled = _isAddMode;
            }
            else if (SessionManager.RoleId == 2) // TenantManager
            {
                btnNew.Visible = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDelete.Visible = true;

                txtFundName.Enabled = true;
                txtAccountNumber.Enabled = true;
                cboBranch.Enabled = true;
                txtBalance.Enabled = _isAddMode;
            }
            else // BranchManager / Staff
            {
                btnNew.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnDelete.Visible = false;

                txtFundName.Enabled = false;
                txtAccountNumber.Enabled = false;
                txtBalance.Enabled = false;
                cboBranch.Enabled = false;
            }
        }

        private void btnSyncSelected_Click(object sender, EventArgs e)
        {
            if (SessionManager.RoleId != 2)
            {
                MessageBox.Show("Từ chối truy cập! Chỉ Quản trị viên hệ thống (Admin) mới có quyền thực hiện Đồng bộ số liệu cho quỹ đã chọn.",
                                "Cảnh báo Bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int syncCount = 0;

            foreach (DataGridViewRow row in dgvCashFunds.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["colCheck"].Value != null && Convert.ToBoolean(row.Cells["colCheck"].Value);
                if (!isChecked) continue;

                int fundId = Convert.ToInt32(row.Cells["colFundId"].Value);
                decimal actualBal = Convert.ToDecimal(row.Cells["colActualBalance"].Value);

                var fundToUpdate = _context.CashFunds.Find(fundId);
                if (fundToUpdate != null)
                {
                    fundToUpdate.Balance = actualBal;
                    syncCount++;
                }
            }

            if (syncCount > 0)
            {
                try
                {
                    _context.SaveChanges();
                    MessageBox.Show($"Đã đồng bộ thành công {syncCount} quỹ tiền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi đồng bộ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng tick chọn ít nhất 1 quỹ cần đồng bộ!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
