using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class ucDebt : UserControl
    {
        private readonly DebtService _debtService = new DebtService();
        private bool _isLoadingData;
        private bool _isOpeningPayment;

        private sealed class DebtGridItem
        {
            public long DebtId { get; set; }
            public string PartnerName { get; set; }
            public string DebtType { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal Remaining { get; set; }
            public string Status { get; set; }
            public string RawStatus { get; set; }
            public string RawDebtType { get; set; }
        }

        public ucDebt()
        {
            InitializeComponent();
            InitializeComboboxes();
            ConfigureGrid();

            if (cboDebtType.Items.Count > 0) cboDebtType.SelectedIndex = 0;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;

            btnFilter.Click += async (s, e) => await LoadDebtDataAsync();
            txtSearch.KeyDown += txtSearch_KeyDown;
            cboDebtType.SelectedIndexChanged += filterControl_Changed;
            cboStatus.SelectedIndexChanged += filterControl_Changed;
            dgvDebts.CellFormatting += dgvDebts_CellFormatting;
            dgvDebts.CellDoubleClick += dgvDebts_CellDoubleClick;
            btnApproveDebt.Click += btnApproveDebt_Click;

            btnPayDebt.Visible = SessionManager.CanApproveDebt;
            btnApproveDebt.Visible = SessionManager.CanApproveDebt;

            TogglePayButtonState();
            _ = LoadDebtDataAsync();
        }

        private void ConfigureGrid()
        {
            EnsureGridColumns();
            dgvDebts.MultiSelect = false;
            dgvDebts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 247, 255);
            dgvDebts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 33, 33);

            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPaid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colRemaining.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            colTotal.DefaultCellStyle.Format = "N0";
            colPaid.DefaultCellStyle.Format = "N0";
            colRemaining.DefaultCellStyle.Format = "N0";
        }

        private void EnsureGridColumns()
        {
            if (dgvDebts.Columns.Count > 0 && colTotal != null && colPaid != null && colRemaining != null)
            {
                return;
            }

            dgvDebts.AutoGenerateColumns = false;
            dgvDebts.Columns.Clear();

            colDebtId ??= new DataGridViewTextBoxColumn
            {
                Name = "colDebtId",
                DataPropertyName = "DebtId",
                HeaderText = "ID",
                Visible = false
            };

            colPartner ??= new DataGridViewTextBoxColumn
            {
                Name = "colPartner",
                DataPropertyName = "PartnerName",
                HeaderText = "Đối tác"
            };

            colType ??= new DataGridViewTextBoxColumn
            {
                Name = "colType",
                DataPropertyName = "DebtType",
                HeaderText = "Loại công nợ"
            };

            colTotal ??= new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                DataPropertyName = "TotalAmount",
                HeaderText = "Tổng nợ"
            };

            colPaid ??= new DataGridViewTextBoxColumn
            {
                Name = "colPaid",
                DataPropertyName = "PaidAmount",
                HeaderText = "Đã trả"
            };

            colRemaining ??= new DataGridViewTextBoxColumn
            {
                Name = "colRemaining",
                DataPropertyName = "Remaining",
                HeaderText = "Còn lại"
            };

            colStatus ??= new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái"
            };

            colRawDebtType ??= new DataGridViewTextBoxColumn
            {
                Name = "colRawDebtType",
                DataPropertyName = "RawDebtType",
                HeaderText = "RawDebtType",
                Visible = false
            };

            dgvDebts.Columns.AddRange(new DataGridViewColumn[]
            {
                colDebtId,
                colPartner,
                colType,
                colTotal,
                colPaid,
                colRemaining,
                colStatus,
                colRawDebtType
            });
        }

        private void InitializeComboboxes()
        {
            // Populate Debt Type
            cboDebtType.Items.Clear();
            cboDebtType.Items.Add("Tất cả loại");
            cboDebtType.Items.Add("Khách nợ (Phải thu)");
            cboDebtType.Items.Add("Nợ NCC (Phải trả)");
            cboDebtType.SelectedIndex = 0;

            // Populate Status
            cboStatus.Items.Clear();
            cboStatus.Items.Add("Tất cả trạng thái");
            cboStatus.Items.Add("Mới tạo (chờ duyệt)");
            cboStatus.Items.Add("Chưa thanh toán");
            cboStatus.Items.Add("Thanh toán một phần");
            cboStatus.Items.Add("Đã thanh toán");
            cboStatus.SelectedIndex = 0;
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            await LoadDebtDataAsync();
        }

        private async void filterControl_Changed(object sender, EventArgs e)
        {
            if (_isLoadingData || !IsHandleCreated) return;
            await LoadDebtDataAsync();
        }

        private async void dgvDebts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            await OpenPaymentFormAsync();
        }

        private void dgvDebts_SelectionChanged(object sender, EventArgs e)
        {
            TogglePayButtonState();
        }

        private void dgvDebts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDebts.Rows.Count <= e.RowIndex) return;

            var row = dgvDebts.Rows[e.RowIndex];
            string rawType = null;

            if (row.DataBoundItem is DebtGridItem item)
            {
                rawType = item.RawDebtType;
            }
            else if (row.DataBoundItem is DataRowView drv && drv.Row.Table.Columns.Contains("RawDebtType"))
            {
                rawType = drv["RawDebtType"]?.ToString();
            }

            if (!string.IsNullOrWhiteSpace(rawType))
            {
                row.DefaultCellStyle.ForeColor = string.Equals(rawType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase)
                    ? Color.FromArgb(25, 118, 210)
                    : Color.FromArgb(211, 47, 47);
            }

            if (e.Value != null && dgvDebts.Columns[e.ColumnIndex].DataPropertyName == "Status")
            {
                var status = e.Value.ToString();
                if (status == "Mới tạo")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(30, 136, 229);
                }
                else if (status == "Đã thanh toán")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(46, 125, 50);
                }
                else if (status == "Thanh toán một phần")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(245, 124, 0);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(97, 97, 97);
                }
            }
        }

        public async Task LoadDebtDataAsync(long? preferredDebtId = null)
        {
            if (_isLoadingData) return;

            ApplyBranchContextActionState();

            var main = this.FindForm() as frmMain;
            try
            {
                _isLoadingData = true;
                SetFilterState(true);
                main?.SetLoadingState(true);

                long? selectedDebtId = preferredDebtId ?? GetCurrentDebtId();
                string type = GetSelectedDebtTypeFilter();
                string status = GetSelectedStatusFilter();
                string search = txtSearch.Text?.Trim();

                int tenantId = SessionManager.CurrentTenantId ?? 0;
                int? branchId = SessionManager.CurrentBranchId;

                var allDebts = await _debtService.GetDebtsAsync(tenantId, branchId);

                var data = allDebts
                    .Where(d => (type == null || d.DebtType == type) &&
                                (status == null || d.Status == status))
                    .ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => (d.Partner?.PartnerName ?? string.Empty).IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                }

                var displayData = data.Select(d => new DebtGridItem
                {
                    DebtId = d.DebtId,
                    PartnerName = d.Partner?.PartnerName ?? string.Empty,
                    DebtType = string.Equals(d.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase) ? "Khách nợ (Phải thu)" : "Mình nợ (Phải trả)",
                    TotalAmount = d.TotalAmount,
                    PaidAmount = d.PaidAmount,
                    Remaining = d.TotalAmount - d.PaidAmount,
                    Status = ToStatusDisplay(d.Status),
                    RawStatus = d.Status,
                    RawDebtType = d.DebtType
                }).ToList();

                BindGrid(displayData, selectedDebtId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load khoản nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                main?.SetLoadingState(false);
                _isLoadingData = false;
                SetFilterState(false);
                ApplyBranchContextActionState();
                TogglePayButtonState();
            }
        }

        private void BindGrid(List<DebtGridItem> displayData, long? selectedDebtId)
        {
            dgvDebts.DataSource = null;
            dgvDebts.DataSource = displayData;

            decimal totalReceivable = displayData
                .Where(x => string.Equals(x.RawDebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase))
                .Sum(x => x.Remaining);
            decimal totalPayable = displayData
                .Where(x => string.Equals(x.RawDebtType, "PAYABLE", StringComparison.OrdinalIgnoreCase))
                .Sum(x => x.Remaining);

            lblTotalReceivable.Text = $"Khách nợ: {totalReceivable:N0} đ";
            lblTotalPayable.Text = $"Nợ NCC: {totalPayable:N0} đ";

            if (dgvDebts.Rows.Count == 0)
            {
                dgvDebts.ClearSelection();
                return;
            }

            bool restored = false;
            if (selectedDebtId.HasValue)
            {
                foreach (DataGridViewRow row in dgvDebts.Rows)
                {
                    if (row.Cells[colDebtId.Index].Value == null) continue;
                    if (long.TryParse(row.Cells[colDebtId.Index].Value.ToString(), out long id) && id == selectedDebtId.Value)
                    {
                        row.Selected = true;
                        dgvDebts.CurrentCell = row.Cells[colPartner.Index];
                        restored = true;
                        break;
                    }
                }
            }

            if (!restored)
            {
                dgvDebts.Rows[0].Selected = true;
                dgvDebts.CurrentCell = dgvDebts.Rows[0].Cells[colPartner.Index];
            }
        }

        private string GetSelectedDebtTypeFilter()
        {
            var typeDisplay = cboDebtType.SelectedItem?.ToString();
            if (typeDisplay == "Khách nợ (Phải thu)") return "RECEIVABLE";
            if (typeDisplay == "Nợ NCC (Phải trả)") return "PAYABLE";
            return null;
        }

        private string GetSelectedStatusFilter()
        {
            var statusDisplay = cboStatus.SelectedItem?.ToString();
            if (statusDisplay == "Mới tạo (chờ duyệt)") return "NEW";
            if (statusDisplay == "Chưa thanh toán") return "PENDING";
            if (statusDisplay == "Thanh toán một phần") return "PARTIALLY_PAID";
            if (statusDisplay == "Đã thanh toán") return "PAID";
            return null;
        }

        private static string ToStatusDisplay(string rawStatus)
        {
            if (string.Equals(rawStatus, "NEW", StringComparison.OrdinalIgnoreCase)) return "Mới tạo";
            if (string.Equals(rawStatus, "PENDING", StringComparison.OrdinalIgnoreCase)) return "Chưa thanh toán";
            if (string.Equals(rawStatus, "PARTIALLY_PAID", StringComparison.OrdinalIgnoreCase)) return "Thanh toán một phần";
            if (string.Equals(rawStatus, "PAID", StringComparison.OrdinalIgnoreCase)) return "Đã thanh toán";
            return string.IsNullOrWhiteSpace(rawStatus) ? "Không xác định" : rawStatus;
        }

        private void SetFilterState(bool isLoading)
        {
            btnFilter.Enabled = !isLoading;
            cboDebtType.Enabled = !isLoading;
            cboStatus.Enabled = !isLoading;
            txtSearch.Enabled = !isLoading;
            btnFilter.Text = isLoading ? "Đang tải..." : "Lọc";
        }

        private long? GetCurrentDebtId()
        {
            if (dgvDebts.CurrentRow == null) return null;

            if (dgvDebts.CurrentRow.DataBoundItem is DebtGridItem item)
            {
                return item.DebtId;
            }

            if (dgvDebts.CurrentRow.DataBoundItem is DataRowView drv
                && drv.Row.Table.Columns.Contains("DebtId")
                && drv["DebtId"] != DBNull.Value
                && long.TryParse(drv["DebtId"].ToString(), out long rowDebtId))
            {
                return rowDebtId;
            }

            var cellValue = dgvDebts.CurrentRow.Cells[colDebtId.Index].Value;
            if (cellValue == null) return null;

            if (long.TryParse(cellValue.ToString(), out long debtId))
            {
                return debtId;
            }

            return null;
        }

        private decimal GetCurrentRemainingAmount()
        {
            if (dgvDebts.CurrentRow == null) return 0m;

            if (dgvDebts.CurrentRow.DataBoundItem is DebtGridItem item)
            {
                return item.Remaining;
            }

            if (dgvDebts.CurrentRow.DataBoundItem is DataRowView drv
                && drv.Row.Table.Columns.Contains("Remaining")
                && drv["Remaining"] != DBNull.Value)
            {
                decimal.TryParse(drv["Remaining"].ToString(), out decimal rowRemaining);
                return rowRemaining;
            }

            var cellValue = dgvDebts.CurrentRow.Cells[colRemaining.Index].Value;
            if (cellValue == null) return 0m;
            decimal.TryParse(cellValue.ToString(), out decimal remaining);
            return remaining;
        }

        private string GetCurrentDebtStatus()
        {
            if (dgvDebts.CurrentRow == null) return string.Empty;

            if (dgvDebts.CurrentRow.DataBoundItem is DebtGridItem item)
            {
                if (!string.IsNullOrWhiteSpace(item.RawStatus))
                    return NormalizeDebtStatus(item.RawStatus);

                return NormalizeDebtStatus(item.Status);
            }

            if (dgvDebts.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("RawStatus") && drv["RawStatus"] != DBNull.Value)
                    return NormalizeDebtStatus(drv["RawStatus"]?.ToString());

                if (drv.Row.Table.Columns.Contains("Status") && drv["Status"] != DBNull.Value)
                    return NormalizeDebtStatus(drv["Status"]?.ToString());
            }

            var cellValue = dgvDebts.CurrentRow.Cells[colStatus.Index].Value;
            return NormalizeDebtStatus(cellValue?.ToString());
        }

        private static string NormalizeDebtStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return string.Empty;

            var trimmed = status.Trim();
            if (string.Equals(trimmed, "Chưa thanh toán", StringComparison.OrdinalIgnoreCase)) return "PENDING";
            if (string.Equals(trimmed, "Thanh toán một phần", StringComparison.OrdinalIgnoreCase)) return "PARTIALLY_PAID";
            if (string.Equals(trimmed, "Đã thanh toán", StringComparison.OrdinalIgnoreCase)) return "PAID";
            if (string.Equals(trimmed, "Mới tạo", StringComparison.OrdinalIgnoreCase)) return "NEW";

            return trimmed.ToUpperInvariant();
        }

        private static bool IsPayableStatus(string status)
        {
            return string.Equals(status, "PENDING", StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, "PARTIAL", StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, "PARTIALLY_PAID", StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, "PARTIAL_PAID", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsViewAllMode()
        {
            return !SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0;
        }

        private void ApplyBranchContextActionState()
        {
            bool isViewAllMode = IsViewAllMode();

            btnAddNewDebt.Enabled = !isViewAllMode;
            btnAddNewDebt.BackColor = isViewAllMode ? Color.LightGray : Color.FromArgb(76, 175, 80);

            if (isViewAllMode)
            {
                btnPayDebt.Enabled = false;
                btnPayDebt.BackColor = Color.LightGray;
                btnApproveDebt.Enabled = false;
                btnApproveDebt.BackColor = Color.LightGray;
            }
            else
            {
                btnPayDebt.BackColor = Color.FromArgb(76, 175, 80);
                btnApproveDebt.BackColor = Color.FromArgb(255, 152, 0);
            }
        }

        private void TogglePayButtonState()
        {
            if (IsViewAllMode())
            {
                btnPayDebt.Enabled = false;
                btnApproveDebt.Enabled = false;
                return;
            }

            btnApproveDebt.Visible = SessionManager.CanApproveDebt;

            if (_isLoadingData || _isOpeningPayment)
            {
                btnPayDebt.Enabled = false;
                btnApproveDebt.Enabled = false;
                return;
            }

            long? currentDebtId = GetCurrentDebtId();
            if (!currentDebtId.HasValue)
            {
                btnPayDebt.Enabled = false;
                btnApproveDebt.Enabled = false;
                return;
            }

            string currentStatus = GetCurrentDebtStatus();
            decimal remainingAmount = GetCurrentRemainingAmount();

            btnApproveDebt.Enabled = SessionManager.CanApproveDebt
                && string.Equals(currentStatus, "NEW", StringComparison.OrdinalIgnoreCase);

            if (!SessionManager.CanApproveDebt)
            {
                btnPayDebt.Enabled = false;
            }
            else
            {
                bool isValidToPay = remainingAmount > 0 && IsPayableStatus(currentStatus);
                btnPayDebt.Enabled = isValidToPay;
            }
        }

        private async Task OpenPaymentFormAsync()
        {
            if (!SessionManager.CanApproveDebt)
            {
                MessageBox.Show("Bạn không có quyền duyệt/thanh toán công nợ.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsViewAllMode())
            {
                MessageBox.Show("Vui lòng chọn chi nhánh cụ thể trước khi thanh toán công nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long? debtId = GetCurrentDebtId();
            if (!debtId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn khoản nợ cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal remaining = GetCurrentRemainingAmount();
            if (remaining <= 0)
            {
                MessageBox.Show("Khoản nợ này đã tất toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string currentStatus = GetCurrentDebtStatus();
            if (!IsPayableStatus(currentStatus))
            {
                MessageBox.Show("Khoản nợ chưa ở trạng thái cho phép thanh toán. Vui lòng duyệt công nợ trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _isOpeningPayment = true;
                TogglePayButtonState();

                using (var frm = new frmDebtPayment(debtId.Value))
                {
                    if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        await LoadDebtDataAsync(debtId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form trả nợ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isOpeningPayment = false;
                TogglePayButtonState();
            }
        }

        private async void btnPayDebt_Click(object sender, EventArgs e)
        {
            await OpenPaymentFormAsync();
        }

        private async void btnApproveDebt_Click(object sender, EventArgs e)
        {
            long? debtId = GetCurrentDebtId();
            if (!debtId.HasValue) return;

            if (!SessionManager.CanApproveDebt)
            {
                MessageBox.Show("Bạn không có quyền duyệt công nợ.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsViewAllMode())
            {
                MessageBox.Show("Vui lòng chọn chi nhánh cụ thể trước khi duyệt công nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string currentStatus = GetCurrentDebtStatus();
            if (!string.Equals(currentStatus, "NEW", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ có thể duyệt khoản nợ ở trạng thái MỚI TẠO (NEW).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                "Xác nhận khoản nợ này là hợp lệ và cho phép xuất quỹ thanh toán?",
                "Xác nhận duyệt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                btnApproveDebt.Enabled = false;

                bool success = await _debtService.ApproveDebtAsync(debtId.Value);
                if (success)
                {
                    MessageBox.Show("Đã duyệt thành công! Kế toán đã có thể thanh toán khoản nợ này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDebtDataAsync(debtId);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khoản nợ để duyệt hoặc khoản nợ không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                TogglePayButtonState();
            }
        }

        private void dgvDebts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void btnAddNewDebt_Click(object sender, EventArgs e)
        {
            if (IsViewAllMode())
            {
                MessageBox.Show("Vui lòng chọn chi nhánh cụ thể trước khi thêm công nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var frm = new frmAddDebt())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await LoadDebtDataAsync();
                }
            }
        }
    }
}
