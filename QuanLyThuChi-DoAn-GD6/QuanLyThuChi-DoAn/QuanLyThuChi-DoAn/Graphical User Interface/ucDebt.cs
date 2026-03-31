using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyThuChi_DoAn.BLL.Services;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class ucDebt : UserControl
    {
        private DebtService _debtService = new DebtService();

        public ucDebt()
        {
            InitializeComponent();

            // default filter selections
            if (cboDebtType.Items.Count > 0) cboDebtType.SelectedIndex = 0;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;

            // wire events
            btnFilter.Click += async (s, e) => await LoadDebtDataAsync();
            btnPayDebt.Click += btnPayDebt_Click;
            dgvDebts.CellFormatting += dgvDebts_CellFormatting;

            // initial load
            _ = LoadDebtDataAsync();
        }

        private void dgvDebts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDebts.Rows.Count <= e.RowIndex) return;

            var row = dgvDebts.Rows[e.RowIndex];
            var data = row.DataBoundItem as dynamic;
            if (data == null) return;

            string rawType = null;
            try
            {
                // Prefer RawDebtType (internal), fallback to DebtType
                rawType = data.RawDebtType ?? data.DebtType;
            }
            catch { }

            if (string.IsNullOrWhiteSpace(rawType)) return;

            if (string.Equals(rawType.ToString(), "RECEIVABLE", StringComparison.OrdinalIgnoreCase))
                row.DefaultCellStyle.ForeColor = Color.FromArgb(25, 118, 210); // blue
            else
                row.DefaultCellStyle.ForeColor = Color.FromArgb(211, 47, 47); // red
        }

        private async System.Threading.Tasks.Task LoadDebtDataAsync()
        {
            var main = this.FindForm() as frmMain;
            try
            {
                main?.SetLoadingState(true);

                // run heavy fetch on background thread
                var type = cboDebtType.SelectedItem?.ToString();
                var status = cboStatus.SelectedItem?.ToString();
                int? partnerId = null;
                string search = txtSearch.Text?.Trim();

                var data = await System.Threading.Tasks.Task.Run(() => _debtService.GetDebts(type, status, partnerId));

                // Filter safely by partner name if search provided
                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => (d.Partner?.PartnerName ?? string.Empty).IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                }

                // prepare display list
                var displayData = data.Select(d => new
                {
                    d.DebtId,
                    PartnerName = d.Partner?.PartnerName ?? string.Empty,
                    DebtTypeName = string.Equals(d.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase) ? "Khách nợ (Phải thu)" : "Mình nợ (Phải trả)",
                    d.TotalAmount,
                    d.PaidAmount,
                    Remaining = d.TotalAmount - d.PaidAmount,
                    d.DueDate,
                    d.Status,
                    d.DebtType
                }).ToList();

                if (dgvDebts.InvokeRequired)
                {
                    dgvDebts.Invoke(new Action(() => dgvDebts.DataSource = displayData));
                }
                else
                {
                    dgvDebts.DataSource = displayData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load khoản nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                main?.SetLoadingState(false);
            }
        }

        private void btnPayDebt_Click(object sender, EventArgs e)
        {
            if (dgvDebts.CurrentRow == null) return;

            try
            {
                var cells = dgvDebts.CurrentRow.Cells;

                // try read DebtId and Remaining from cells (designer columns)
                if (cells["DebtId"] == null || cells["Remaining"] == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin khoản nợ.");
                    return;
                }

                var debtIdObj = cells["DebtId"].Value;
                var remainingObj = cells["Remaining"].Value;

                if (debtIdObj == null)
                {
                    MessageBox.Show("ID khoản nợ không hợp lệ.");
                    return;
                }

                long debtId = Convert.ToInt64(debtIdObj);
                decimal remaining = 0;
                if (remainingObj != null)
                {
                    Decimal.TryParse(remainingObj.ToString(), out remaining);
                }

                if (remaining <= 0)
                {
                    MessageBox.Show("Khoản nợ này đã tất toán!");
                    return;
                }

                using (var frm = new frmDebtPayment(debtId))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDebtData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form trả nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Synchronous loader used by events that expect a non-async refresh (e.g. after dialog)
        private void LoadDebtData()
        {
            try
            {
                string selectedType = cboDebtType.SelectedValue?.ToString() ?? cboDebtType.SelectedItem?.ToString();
                string selectedStatus = cboStatus.SelectedValue?.ToString() ?? cboStatus.SelectedItem?.ToString();

                var rawData = _debtService.GetDebts(selectedType, selectedStatus, null);

                // compute remaining for each and totals
                var prepared = rawData.Select(d => new
                {
                    d.DebtId,
                    PartnerName = d.Partner?.PartnerName ?? string.Empty,
                    DebtType = d.DebtType,
                    DebtTypeName = string.Equals(d.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase) ? "Phải thu" : "Phải trả",
                    TotalAmount = d.TotalAmount,
                    PaidAmount = d.PaidAmount,
                    Remaining = d.TotalAmount - d.PaidAmount,
                    DueDate = d.DueDate?.ToString("dd/MM/yyyy"),
                    d.Status
                }).ToList();

                var totalReceivable = prepared.Where(x => string.Equals(x.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase)).Sum(x => x.Remaining);
                var totalPayable = prepared.Where(x => string.Equals(x.DebtType, "PAYABLE", StringComparison.OrdinalIgnoreCase)).Sum(x => x.Remaining);

                Action uiUpdate = () =>
                {
                    lblTotalReceivable.Text = $"Khách nợ: {totalReceivable:N0} đ";
                    lblTotalPayable.Text = $"Nợ NCC: {totalPayable:N0} đ";
                    dgvDebts.DataSource = prepared.Select(x => new
                    {
                        x.DebtId,
                        PartnerName = x.PartnerName,
                        DebtType = x.DebtTypeName,
                        TotalAmount = x.TotalAmount,
                        PaidAmount = x.PaidAmount,
                        Remaining = x.Remaining,
                        DueDate = x.DueDate,
                        Status = x.Status,
                        RawDebtType = x.DebtType
                    }).ToList();
                };

                if (InvokeRequired)
                    Invoke(uiUpdate);
                else
                    uiUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load khoản nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
