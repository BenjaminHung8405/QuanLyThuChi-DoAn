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
        private bool _isAddMode = false;
        private CashFund _selectedFund = null;

        public ucCashFund()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _cashFundService = new CashFundService(_context);

            // Hook load event
            this.Load += UcCashFund_Load;

            // Initial state
            ResetForm();
            LoadFunds();
        }

        private void UcCashFund_Load(object sender, EventArgs e)
        {
            ApplyAuthorization();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ResetForm()
        {
            txtFundName.Text = "";
            txtAccountNumber.Text = "";
            txtBalance.Text = "";
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
            SetInputFieldsEnabled(false);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _isAddMode = true;
            _selectedFund = null;
            txtFundName.Text = "";
            txtAccountNumber.Text = "";
            txtBalance.Text = "";
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

            if (!decimal.TryParse(txtBalance.Text.Trim(), out decimal initialBalance))
            {
                if (_isAddMode)
                {
                    MessageBox.Show("Số dư ban đầu không hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBalance.Focus();
                    return;
                }
            }

            if (_isAddMode)
            {
                var newFund = new CashFund
                {
                    FundName = txtFundName.Text.Trim(),
                    AccountNumber = txtAccountNumber.Text.Trim(),
                    Balance = initialBalance,
                    IsActive = true,
                    BranchId = SessionManager.BranchId ?? 0,
                    TenantId = SessionManager.TenantId
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
            LoadFunds();
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
            txtBalance.Text = row.Balance.ToString("N0");

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

        private void dgvCashFunds_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCashFunds.Columns[e.ColumnIndex].Name != "Balance")
                return;

            if (e.Value == null || e.Value == DBNull.Value)
                return;

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
            if (SessionManager.RoleId == 3) // Staff
            {
                btnNew.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnDelete.Visible = false;

                txtFundName.Enabled = false;
                txtAccountNumber.Enabled = false;
                txtBalance.Enabled = false;
            }
            else if (SessionManager.RoleId == 2) // Admin
            {
                btnNew.Visible = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDelete.Visible = true;

                txtFundName.Enabled = true;
                txtAccountNumber.Enabled = true;
                // Only enable balance on add mode, but default admin can see
                txtBalance.Enabled = _isAddMode;
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                var auditResults = _cashFundService.AuditBranchFunds(SessionManager.TenantId, SessionManager.BranchId ?? 0);
                var errors = auditResults.Where(r => !r.IsValid).ToList();

                if (errors.Count == 0)
                {
                    MessageBox.Show("Tuyệt vời! Dữ liệu Sổ quỹ và Lịch sử giao dịch hoàn toàn khớp nhau.",
                                    "Kết quả Đối soát", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string errorMsg = "CẢNH BÁO: Phát hiện lệch dữ liệu tại các quỹ sau:\n\n";

                    foreach (var err in errors)
                    {
                        errorMsg += $"- Quỹ: {err.FundName}\n";
                        errorMsg += $"  + Số dư hệ thống ghi nhận: {err.CurrentBalance:N0} đ\n";
                        errorMsg += $"  + Lịch sử giao dịch tính ra: {err.CalculatedBalance:N0} đ\n";
                        errorMsg += $"  => LỆCH: {err.Difference:N0} đ\n\n";
                    }

                    errorMsg += "Vui lòng kiểm tra lại lịch sử giao dịch hoặc sử dụng chức năng Đồng bộ.";

                    MessageBox.Show(errorMsg, "Lỗi Toàn vẹn Dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình đối soát: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            // 1. CHỐT CHẶN BẢO MẬT: Chỉ Admin (RoleId = 2) mới được dùng
            if (SessionManager.RoleId != 2)
            {
                MessageBox.Show("Từ chối truy cập! Chỉ Quản trị viên hệ thống (Admin) mới có quyền thực hiện Đồng bộ số liệu.",
                                "Cảnh báo Bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kiểm tra xem người dùng đã chọn Quỹ nào trên DataGridView chưa
            if (dgvCashFunds.CurrentRow == null || dgvCashFunds.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn một Quỹ tiền trên danh sách để thực hiện đồng bộ!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedFundId = Convert.ToInt32(dgvCashFunds.CurrentRow.Cells["FundId"].Value);
            string fundName = dgvCashFunds.CurrentRow.Cells["FundName"].Value?.ToString();

            // 3. CẢNH BÁO XÁC NHẬN
            DialogResult dialogResult = MessageBox.Show(
                $"CẢNH BÁO NGHIỆP VỤ:\n\nBạn đang yêu cầu Đồng bộ lại số dư cho: [{fundName}].\n" +
                "Hệ thống sẽ lấy tổng lịch sử giao dịch và GHI ĐÈ lên số dư hiện tại của quỹ này.\n\n" +
                "Hành động này không thể hoàn tác. Bạn có chắc chắn muốn tiếp tục?",
                "Xác nhận Đồng bộ Sổ Quỹ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    _cashFundService.SyncFundBalance(selectedFundId, SessionManager.UserId);

                    MessageBox.Show($"Đồng bộ thành công! Số dư của [{fundName}] đã được cập nhật chuẩn xác.",
                                    "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadFunds();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi trong quá trình đồng bộ: {ex.Message}",
                                    "Lỗi Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
