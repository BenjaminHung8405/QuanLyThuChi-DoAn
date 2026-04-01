using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class frmDebtPayment : Form
    {
        private readonly long _debtId;
        private readonly DebtService _debtService = new DebtService();
        private decimal _remaining = 0m;
        private bool _isSubmitting;

        public frmDebtPayment()
        {
            InitializeComponent();
            InitializeFormBehavior();
        }

        public frmDebtPayment(long debtId) : this()
        {
            _debtId = debtId;
            InitializeData();
        }

        private void InitializeFormBehavior()
        {
            AcceptButton = btnConfirm;
            CancelButton = btnCancel;
            dtpPaymentDate.Enabled = false;
            lblDate.Text = "Ngày ghi nhận:";
            cboFund.SelectedIndexChanged += cboFund_SelectedIndexChanged;
            UpdateConfirmState();
        }

        private void InitializeData()
        {
            if (!SessionManager.CanApproveDebt)
            {
                MessageBox.Show("Bạn không có quyền duyệt/thanh toán công nợ.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            bool loaded = LoadDebtInfo();
            if (!loaded) return;

            LoadFundOptions();
            dtpPaymentDate.Value = DateTime.Now;
            nudAmount.Focus();
            UpdateConfirmState();
        }

        private void LoadFundOptions()
        {
            try
            {
                var fundService = new CashFundService(new Data_Access_Layer.AppDbContext());
                var funds = fundService.GetVisibleFunds(QuanLyThuChi_DoAn.BLL.Common.SessionManager.RoleId);

                cboFund.DataSource = null;
                cboFund.DataSource = funds;
                cboFund.DisplayMember = "FundName";
                cboFund.ValueMember = "FundId";

                if (cboFund.Items.Count > 0)
                {
                    cboFund.SelectedIndex = 0;
                    errorProvider.SetError(cboFund, string.Empty);
                }
                else
                {
                    errorProvider.SetError(cboFund, "Không có quỹ khả dụng để thanh toán.");
                }
            }
            catch (Exception ex)
            {
                cboFund.DataSource = null;
                errorProvider.SetError(cboFund, "Không tải được danh sách quỹ.");
                MessageBox.Show($"Không thể tải danh sách quỹ: {ex.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool LoadDebtInfo()
        {
            try
            {
                var debt = _debtService.GetDebtById(_debtId);
                if (debt == null)
                {
                    MessageBox.Show("Không tìm thấy khoản nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return false;
                }

                bool isReceivable = string.Equals(debt.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase);
                lblPartnerName.Text = debt.Partner?.PartnerName ?? string.Empty;
                txtTotalAmount.Text = debt.TotalAmount.ToString("N0");
                txtPaidAmount.Text = debt.PaidAmount.ToString("N0");
                _remaining = Math.Max(0m, debt.TotalAmount - debt.PaidAmount);
                txtRemainingAmount.Text = _remaining.ToString("N0");

                if (isReceivable)
                {
                    lblHeader.Text = "THU CÔNG NỢ KHÁCH HÀNG";
                    pnlHeader.BackColor = Color.FromArgb(25, 118, 210);
                }
                else
                {
                    lblHeader.Text = "THANH TOÁN CÔNG NỢ";
                    pnlHeader.BackColor = Color.FromArgb(211, 47, 47);
                }

                nudAmount.Maximum = _remaining;
                nudAmount.Value = 0;
                nudAmount.Enabled = _remaining > 0;
                linkPayAll.Enabled = _remaining > 0;

                if (_remaining <= 0)
                {
                    errorProvider.SetError(nudAmount, "Khoản nợ này đã tất toán.");
                }
                else
                {
                    errorProvider.SetError(nudAmount, string.Empty);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nạp thông tin khoản nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return false;
            }
        }

        private void nudAmount_ValueChanged(object sender, EventArgs e)
        {
            errorProvider.SetError(nudAmount, string.Empty);
            decimal value = nudAmount.Value;

            if (value > _remaining)
            {
                nudAmount.ForeColor = Color.Red;
                errorProvider.SetError(nudAmount, "Không thể trả quá số nợ hiện có!");
            }
            else if (value <= 0 && _remaining > 0)
            {
                nudAmount.ForeColor = Color.Black;
                errorProvider.SetError(nudAmount, "Vui lòng nhập số tiền lớn hơn 0.");
            }
            else
            {
                nudAmount.ForeColor = Color.Black;
                errorProvider.SetError(nudAmount, string.Empty);
            }

            UpdateConfirmState();
        }

        private void linkPayAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_remaining <= 0) return;

            nudAmount.Value = _remaining;
            UpdateConfirmState();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isSubmitting) return;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cboFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFund.SelectedItem == null || cboFund.SelectedValue == null)
            {
                errorProvider.SetError(cboFund, "Vui lòng chọn quỹ thanh toán.");
            }
            else
            {
                errorProvider.SetError(cboFund, string.Empty);
            }

            UpdateConfirmState();
        }

        private bool TryGetSelectedFundId(out int selectedFundId)
        {
            selectedFundId = 0;

            if (cboFund.SelectedItem == null || cboFund.SelectedValue == null)
            {
                return false;
            }

            try
            {
                selectedFundId = Convert.ToInt32(cboFund.SelectedValue);
                return selectedFundId > 0;
            }
            catch
            {
                return false;
            }
        }

        private void SetSubmittingState(bool isSubmitting)
        {
            _isSubmitting = isSubmitting;
            Cursor = isSubmitting ? Cursors.WaitCursor : Cursors.Default;
            btnCancel.Enabled = !isSubmitting;
            btnConfirm.Text = isSubmitting ? "Đang xử lý..." : "Xác nhận";
            UpdateConfirmState();
        }

        private void UpdateConfirmState()
        {
            bool hasRemaining = _remaining > 0;
            bool amountValid = nudAmount.Value > 0 && nudAmount.Value <= _remaining;
            bool fundValid = cboFund.SelectedItem != null && cboFund.SelectedValue != null;

            nudAmount.Enabled = hasRemaining && !_isSubmitting;
            linkPayAll.Enabled = hasRemaining && !_isSubmitting;
            cboFund.Enabled = !_isSubmitting && cboFund.Items.Count > 0;
            btnConfirm.Enabled = !_isSubmitting && hasRemaining && amountValid && fundValid;
        }

        private static string BuildErrorMessage(Exception ex)
        {
            if (ex == null) return "Không xác định lỗi.";

            var messages = new List<string>();
            var current = ex;

            while (current != null)
            {
                if (!string.IsNullOrWhiteSpace(current.Message) && !messages.Contains(current.Message))
                {
                    messages.Add(current.Message);
                }

                current = current.InnerException;
            }

            if (messages.Count <= 1)
            {
                return messages.Count == 0 ? "Không xác định lỗi." : messages[0];
            }

            return string.Join(Environment.NewLine, messages.Select((m, i) => $"{i + 1}. {m}"));
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_isSubmitting) return;

            if (!SessionManager.CanApproveDebt)
            {
                MessageBox.Show("Bạn không có quyền duyệt/thanh toán công nợ.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            errorProvider.Clear();
            decimal amount = nudAmount.Value;

            if (!TryGetSelectedFundId(out int selectedFundId))
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

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xác nhận thanh toán {amount:N0} đ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            string note = txtNote.Text?.Trim() ?? string.Empty;

            try
            {
                SetSubmittingState(true);
                bool isSuccess = await Task.Run(() =>
                {
                    var paymentService = new DebtService();
                    return paymentService.PayDebt(_debtId, selectedFundId, amount, note);
                });

                if (isSuccess)
                {
                    MessageBox.Show("Thanh toán công nợ thành công!", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Thanh toán chưa thành công. Vui lòng thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận thanh toán:\n{BuildErrorMessage(ex)}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetSubmittingState(false);
            }
        }
    }
}
