using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Helpers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn
{
    public partial class frmFundTransfer : Form
    {
        private readonly AppDbContext _context;
        private readonly CashFundService _cashFundService;
        private readonly TransactionService _transactionService;
        private bool _isFormattingAmountInput;
        private bool _isFormattingBankFeeInput;

        public frmFundTransfer()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _cashFundService = new CashFundService(_context);
            _transactionService = new TransactionService(_context);
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            txtAmount.TextChanged += txtAmount_TextChanged;
            txtBankFee.TextChanged += txtBankFee_TextChanged;
        }

        private void FrmFundTransfer_Load(object? sender, EventArgs e)
        {
            LoadFundComboBoxes();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1.Enabled = false;
            txtAmount.Value = 0;
            txtBankFee.Value = 0;
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

                cbSourceFund.SelectedIndexChanged -= CbSourceFund_SelectedIndexChanged;
                cbDestFund.SelectedIndexChanged -= CbDestFund_SelectedIndexChanged;

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

                int selectedSourceFundId = sourceFunds.FirstOrDefault()?.FundId ?? 0;
                BindDestinationFundsBySource(selectedSourceFundId);

                cbSourceFund.SelectedIndexChanged += CbSourceFund_SelectedIndexChanged;
                cbDestFund.SelectedIndexChanged += CbDestFund_SelectedIndexChanged;

                UpdateBalanceLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu quỹ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbSourceFund_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!SessionManager.CurrentTenantId.HasValue || !SessionManager.CurrentBranchId.HasValue)
                return;

            if (cbSourceFund.SelectedValue == null || !int.TryParse(cbSourceFund.SelectedValue.ToString(), out int selectedSourceId))
            {
                lblBalanceSourceFund.Text = FormatMoney(0);
                return;
            }

            int tenantId = SessionManager.CurrentTenantId.Value;
            int branchId = SessionManager.CurrentBranchId.Value;

            var source = _cashFundService.GetFundsByBranch(tenantId, branchId, SessionManager.RoleId)
                .FirstOrDefault(f => f.FundId == selectedSourceId);

            lblBalanceSourceFund.Text = source != null ? FormatMoney(source.Balance) : FormatMoney(0);

            BindDestinationFundsBySource(selectedSourceId);
        }

        private void CbDestFund_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!SessionManager.CurrentTenantId.HasValue)
                return;

            if (cbDestFund.SelectedValue == null || !int.TryParse(cbDestFund.SelectedValue.ToString(), out int selectedDestId))
            {
                lblBalanceDestFund.Text = FormatMoney(0);
                return;
            }

            var dest = _cashFundService.GetFundsByTenant(SessionManager.CurrentTenantId.Value)
                .FirstOrDefault(f => f.FundId == selectedDestId);

            lblBalanceDestFund.Text = dest != null ? FormatMoney(dest.Balance) : FormatMoney(0);
        }

        private void UpdateBalanceLabels()
        {
            if (SessionManager.CurrentTenantId.HasValue
                && SessionManager.CurrentBranchId.HasValue
                && cbSourceFund.SelectedValue != null
                && int.TryParse(cbSourceFund.SelectedValue.ToString(), out int sourceId))
            {
                var source = _cashFundService.GetFundsByBranch(SessionManager.CurrentTenantId.Value, SessionManager.CurrentBranchId.Value, SessionManager.RoleId)
                    .FirstOrDefault(f => f.FundId == sourceId);
                lblBalanceSourceFund.Text = source != null ? FormatMoney(source.Balance) : FormatMoney(0);
            }
            else
            {
                lblBalanceSourceFund.Text = FormatMoney(0);
            }

            if (SessionManager.CurrentTenantId.HasValue
                && cbDestFund.SelectedValue != null
                && int.TryParse(cbDestFund.SelectedValue.ToString(), out int destId))
            {
                var dest = _cashFundService.GetFundsByTenant(SessionManager.CurrentTenantId.Value)
                    .FirstOrDefault(f => f.FundId == destId);
                lblBalanceDestFund.Text = dest != null ? FormatMoney(dest.Balance) : FormatMoney(0);
            }
            else
            {
                lblBalanceDestFund.Text = FormatMoney(0);
            }
        }

        private void BindDestinationFundsBySource(int selectedSourceFundId)
        {
            if (!SessionManager.CurrentTenantId.HasValue)
            {
                cbDestFund.DataSource = null;
                return;
            }

            int tenantId = SessionManager.CurrentTenantId.Value;
            var destinationFunds = _cashFundService.GetFundsByTenant(tenantId)
                .Where(f => f.FundId != selectedSourceFundId)
                .ToList();

            cbDestFund.DataSource = destinationFunds;
            cbDestFund.DisplayMember = "FundName";
            cbDestFund.ValueMember = "FundId";
            cbDestFund.SelectedIndex = -1;
        }

        private async void btnTransfer_Click(object? sender, EventArgs e)
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

            decimal bankFee = txtBankFee.Value;
            if (bankFee < 0)
            {
                MessageBox.Show("Phí giao dịch không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnTransfer.Enabled = false;

                InternalTransferResult result = await _transactionService.CreateInternalTransferAsync(
                    sourceFundId,
                    destFundId,
                    amount,
                    bankFee,
                    txtNotes.Text.Trim());

                MessageBox.Show(
                    "Đã tạo lệnh chuyển quỹ thành công theo mô hình 2 bước.\n\n"
                    + $"- Phiếu CHI quỹ nguồn: COMPLETED\n"
                    + $"- Phiếu THU quỹ đích: PENDING (chờ chi nhánh đích xác nhận)\n"
                    + $"- Số tiền chuyển: {FormatMoney(result.Amount)}\n"
                    + $"- Phí giao dịch: {FormatMoney(result.BankFee)}\n"
                    + $"- Mã tham chiếu: {result.TransferRefNo}",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

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
            }
            finally
            {
                btnTransfer.Enabled = true;
            }
        }

        private void txtAmount_TextChanged(object? sender, EventArgs e)
        {
            ApplyLiveThousandsSeparator(txtAmount, ref _isFormattingAmountInput);
        }

        private void txtBankFee_TextChanged(object? sender, EventArgs e)
        {
            ApplyLiveThousandsSeparator(txtBankFee, ref _isFormattingBankFeeInput);
        }

        private void ApplyLiveThousandsSeparator(NumericUpDown control, ref bool isFormatting)
        {
            if (isFormatting || !control.Focused)
            {
                return;
            }

            try
            {
                isFormatting = true;

                string digitsOnly = new string(control.Text.Where(char.IsDigit).ToArray());
                if (digitsOnly.Length == 0)
                {
                    if (control.Value != 0)
                    {
                        control.Value = 0;
                    }

                    return;
                }

                if (!decimal.TryParse(digitsOnly, NumberStyles.None, CultureInfo.InvariantCulture, out decimal parsedValue))
                {
                    return;
                }

                parsedValue = Math.Min(control.Maximum, Math.Max(control.Minimum, parsedValue));
                if (control.Value != parsedValue)
                {
                    control.Value = parsedValue;
                }

                control.Select(control.Text.Length, 0);
            }
            finally
            {
                isFormatting = false;
            }
        }

        private static string FormatMoney(decimal value)
        {
            return value.ToString("N0", AppCulture.GetConfiguredCulture()) + " đ";
        }
    }


}
