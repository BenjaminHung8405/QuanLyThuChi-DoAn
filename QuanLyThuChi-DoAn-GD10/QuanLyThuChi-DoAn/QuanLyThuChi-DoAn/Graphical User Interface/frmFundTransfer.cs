using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn
{
    public partial class frmFundTransfer : Form
    {
        private readonly AppDbContext _context;
        private readonly CashFundService _cashFundService;
        private readonly CategoryService _categoryService;

        public frmFundTransfer()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _cashFundService = new CashFundService(_context);
            _categoryService = new CategoryService(_context);
            this.Load += FrmFundTransfer_Load;
        }

        private void FrmFundTransfer_Load(object sender, EventArgs e)
        {
            LoadFundComboBoxes();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void LoadFundComboBoxes()
        {
            try
            {
                if (!SessionManager.CurrentTenantId.HasValue || !SessionManager.CurrentBranchId.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn Tenant và Branch trước khi thực hiện chuyển quỹ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int tenantId = SessionManager.CurrentTenantId.Value;
                int currentBranchId = SessionManager.CurrentBranchId.Value;
                int roleId = SessionManager.RoleId;

                var sourceFunds = _cashFundService.GetFundsByBranch(tenantId, currentBranchId, roleId);
                cbSourceFund.DataSource = sourceFunds;
                cbSourceFund.DisplayMember = "FundName";
                cbSourceFund.ValueMember = "FundId";

                if (sourceFunds.Any())
                {
                    cbSourceFund.SelectedIndex = 0;
                }
                else
                {
                    cbSourceFund.SelectedIndex = -1;
                }

                var destinationFunds = _cashFundService.GetFundsByTenant(tenantId)
                    .Where(f => f.BranchId != currentBranchId || f.IsActive)
                    .ToList();

                cbDestFund.DataSource = destinationFunds;
                cbDestFund.DisplayMember = "FundName";
                cbDestFund.ValueMember = "FundId";
                cbDestFund.SelectedIndex = -1;

                cbSourceFund.SelectedIndexChanged += CbSourceFund_SelectedIndexChanged;
                cbDestFund.SelectedIndexChanged += CbDestFund_SelectedIndexChanged;

                UpdateBalanceLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu quỹ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbSourceFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SessionManager.CurrentTenantId.HasValue)
                return;

            if (cbSourceFund.SelectedValue == null || !int.TryParse(cbSourceFund.SelectedValue.ToString(), out int selectedSourceId))
            {
                lblBalanceSourceFund.Text = "0 đ";
                return;
            }

            var source = _cashFundService.GetFundsByBranch(SessionManager.CurrentTenantId.Value, SessionManager.CurrentBranchId.Value, SessionManager.RoleId)
                .FirstOrDefault(f => f.FundId == selectedSourceId);

            lblBalanceSourceFund.Text = source != null ? $"{source.Balance:N0} đ" : "0 đ";

            int sourceTenantId = source?.TenantId ?? SessionManager.CurrentTenantId.Value;

            var destinationFunds = _cashFundService.GetFundsByTenant(sourceTenantId)
                .Where(f => f.FundId != selectedSourceId)
                .ToList();

            cbDestFund.DataSource = destinationFunds;
            cbDestFund.DisplayMember = "FundName";
            cbDestFund.ValueMember = "FundId";
            cbDestFund.SelectedIndex = -1;
        }

        private void CbDestFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDestFund.SelectedValue == null || !int.TryParse(cbDestFund.SelectedValue.ToString(), out int selectedDestId))
            {
                lblBalanceDestFund.Text = "0 đ";
                return;
            }

            var dest = _cashFundService.GetFundsByTenant(SessionManager.CurrentTenantId.Value)
                .FirstOrDefault(f => f.FundId == selectedDestId);

            lblBalanceDestFund.Text = dest != null ? $"{dest.Balance:N0} đ" : "0 đ";
        }

        private void UpdateBalanceLabels()
        {
            if (cbSourceFund.SelectedValue != null && int.TryParse(cbSourceFund.SelectedValue.ToString(), out int sourceId))
            {
                var source = _cashFundService.GetFundsByBranch(SessionManager.CurrentTenantId.Value, SessionManager.CurrentBranchId.Value, SessionManager.RoleId)
                    .FirstOrDefault(f => f.FundId == sourceId);
                lblBalanceSourceFund.Text = source != null ? $"{source.Balance:N0} đ" : "0 đ";
            }
            else
            {
                lblBalanceSourceFund.Text = "0 đ";
            }

            if (cbDestFund.SelectedValue != null && int.TryParse(cbDestFund.SelectedValue.ToString(), out int destId))
            {
                var dest = _cashFundService.GetFundsByTenant(SessionManager.CurrentTenantId.Value)
                    .FirstOrDefault(f => f.FundId == destId);
                lblBalanceDestFund.Text = dest != null ? $"{dest.Balance:N0} đ" : "0 đ";
            }
            else
            {
                lblBalanceDestFund.Text = "0 đ";
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            // 1. Validate basic UI inputs
            if (cbSourceFund.SelectedItem == null || cbDestFund.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn cả Quỹ nguồn và Quỹ đích.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(cbSourceFund.SelectedValue?.ToString(), out int sourceFundId) ||
                !int.TryParse(cbDestFund.SelectedValue?.ToString(), out int destFundId))
            {
                MessageBox.Show("Quỹ chưa hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sourceFundId == destFundId)
            {
                MessageBox.Show("Quỹ nguồn và quỹ đích không được trùng nhau.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal amount = txtAmount.Value;
            if (amount <= 0)
            {
                MessageBox.Show("Số tiền phải là số dương lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Load quỹ từ DB
            var sourceFund = _context.CashFunds.FirstOrDefault(f => f.FundId == sourceFundId);
            var destFund = _context.CashFunds.FirstOrDefault(f => f.FundId == destFundId);

            if (sourceFund == null)
            {
                MessageBox.Show("Không tìm thấy quỹ nguồn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (destFund == null)
            {
                MessageBox.Show("Không tìm thấy quỹ đích.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sourceFund.Balance < amount)
            {
                MessageBox.Show("Quỹ nguồn không đủ số dư.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using var transaction = _context.Database.BeginTransaction();

                var now = DateTime.Now;
                var description = txtNotes.Text.Trim();

                var transOut = new Transaction
                {
                    TenantId = SessionManager.CurrentTenantId ?? 0,
                    BranchId = (SessionManager.CurrentBranchId.HasValue && SessionManager.CurrentBranchId.Value > 0)
                        ? SessionManager.CurrentBranchId.Value
                        : sourceFund.BranchId,
                    FundId = sourceFund.FundId,
                    CategoryId = 98, // Dùng danh mục Rút tiền (OUT)
                    TransType = "OUT",
                    Amount = amount,
                    TransDate = now,
                    Description = description,
                    RefNo = $"TRF-{now:yyyyMMddHHmmss}-{sourceFundId}-{destFundId}",
                    CreatedBy = SessionManager.CurrentUserId,
                    Status = "COMPLETED",
                    IsActive = true
                };

                var destFundFromDb = _context.CashFunds.FirstOrDefault(f => f.FundId == destFundId);

                if (destFundFromDb == null)
                {
                    MessageBox.Show("Không tìm thấy quỹ đích.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var transIn = new Transaction
                {
                    TenantId = SessionManager.CurrentTenantId ?? 0,
                    BranchId = destFundFromDb.BranchId,
                    FundId = destFundFromDb.FundId,
                    CategoryId = 99, // Dùng danh mục Nhận tiền (IN)
                    TransType = "IN",
                    Amount = amount,
                    TransDate = now,
                    Description = description,
                    RefNo = $"TRF-{now:yyyyMMddHHmmss}-{sourceFundId}-{destFundId}",
                    CreatedBy = SessionManager.CurrentUserId,
                    Status = "COMPLETED",
                    IsActive = true,
                    TransferRefTransaction = transOut
                };

                sourceFund.Balance -= amount;
                destFundFromDb.Balance += amount;

                _context.Transactions.AddRange(new[] { transOut, transIn });
                _context.CashFunds.Update(sourceFund);
                _context.CashFunds.Update(destFundFromDb);

                _context.SaveChanges();
                transaction.Commit();

                MessageBox.Show("Chuyển quỹ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cbSourceFund.SelectedIndex = -1;
                cbDestFund.SelectedIndex = -1;
                txtAmount.Value = 0;
                txtNotes.Text = "";

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n" + ex.InnerException.Message;
                }
                MessageBox.Show($"Chuyển quỹ thất bại: {message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    _context.Database.RollbackTransaction();
                }
                catch
                {
                }
            }
        }
    }


}
