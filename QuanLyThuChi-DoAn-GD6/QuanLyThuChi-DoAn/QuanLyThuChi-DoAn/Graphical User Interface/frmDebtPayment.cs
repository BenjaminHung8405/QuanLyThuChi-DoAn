using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using QuanLyThuChi_DoAn.BLL.Services;
using System.Text;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class frmDebtPayment : Form
    {
        private readonly long _debtId;
        private readonly DebtService _debtService = new DebtService();
        private decimal _remaining = 0m;

        public frmDebtPayment()
        {
            InitializeComponent();
        }

        public frmDebtPayment(long debtId) : this()
        {
            _debtId = debtId;
            LoadDebtInfo();
            // populate fund options using CashFundService
            try
            {
                var fundService = new CashFundService(new Data_Access_Layer.AppDbContext());
                var funds = fundService.GetVisibleFunds(QuanLyThuChi_DoAn.BLL.Common.SessionManager.RoleId);
                cboFund.DataSource = funds;
                cboFund.DisplayMember = "FundName";
                cboFund.ValueMember = "FundId";
                if (cboFund.Items.Count > 0) cboFund.SelectedIndex = 0;
            }
            catch
            {
                // fallback: leave cboFund empty
            }
        }

        private void LoadDebtInfo()
        {
            try
            {
                // Prefer direct GetDebtById for efficiency
                var debt = _debtService.GetDebtById(_debtId);
                if (debt == null)
                {
                    MessageBox.Show("Không tìm thấy khoản nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }

                lblPartnerName.Text = debt.Partner?.PartnerName ?? string.Empty;
                txtTotalAmount.Text = (debt.TotalAmount).ToString("N0");
                txtPaidAmount.Text = (debt.PaidAmount).ToString("N0");
                _remaining = debt.TotalAmount - debt.PaidAmount;
                txtRemainingAmount.Text = _remaining.ToString("N0");

                // set header color based on type
                if (string.Equals(debt.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase))
                {
                    pnlHeader.BackColor = Color.FromArgb(25, 118, 210); // blue
                }
                else
                {
                    pnlHeader.BackColor = Color.FromArgb(211, 47, 47); // red
                }
                // configure input defaults
                if (_remaining < 0) _remaining = 0;
                nudAmount.Maximum = _remaining;
                nudAmount.Value = 0;
                dtpPaymentDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nạp thông tin khoản nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void nudAmount_ValueChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            decimal remaining = 0;
            Decimal.TryParse(txtRemainingAmount.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out remaining);
            var value = nudAmount.Value;
            if (value > remaining)
            {
                // show immediate warning
                nudAmount.ForeColor = Color.Red;
                errorProvider.SetError(nudAmount, "Không thể trả quá số nợ hiện có!");
            }
            else
            {
                nudAmount.ForeColor = Color.Black;
                errorProvider.SetError(nudAmount, string.Empty);
            }
        }

        private void linkPayAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            decimal remaining = 0;
            Decimal.TryParse(txtRemainingAmount.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out remaining);
            if (remaining <= 0) return;
            if (remaining > nudAmount.Maximum) nudAmount.Maximum = remaining;
            nudAmount.Value = remaining;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            try
            {
                decimal amount = nudAmount.Value;
                decimal remaining = 0;
                Decimal.TryParse(txtRemainingAmount.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out remaining);
                // Validation
                if (cboFund.SelectedItem == null || cboFund.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Quỹ/Tài khoản thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (amount <= 0)
                {
                    MessageBox.Show("Số tiền thanh toán không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (amount > _remaining)
                {
                    MessageBox.Show($"Số tiền trả ({amount:N0}) không được lớn hơn dư nợ ({_remaining:N0})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm
                var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xác nhận thanh toán {amount:N0} đ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                // Call BLL
                int selectedFundId;
                try
                {
                    selectedFundId = Convert.ToInt32(cboFund.SelectedValue);
                }
                catch
                {
                    MessageBox.Show("Quỹ chọn không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string note = txtNote.Text?.Trim() ?? string.Empty;

                bool isSuccess = _debtService.PayDebt(_debtId, selectedFundId, amount, note);
                if (isSuccess)
                {
                    MessageBox.Show("Thanh toán công nợ thành công!", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
