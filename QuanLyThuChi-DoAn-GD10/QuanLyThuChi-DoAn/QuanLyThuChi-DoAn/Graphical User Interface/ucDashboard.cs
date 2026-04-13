using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.DTOs;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using DashboardSummaryDTO = QuanLyThuChi_DoAn.BLL.DTOs.DashboardOverviewDTO;

namespace QuanLyThuChi_DoAn
{
    public partial class ucDashboard : UserControl
    {
        private readonly ReportService _reportService;
        private readonly ListView _lvExpenseBreakdown;

        public ucDashboard()
        {
            InitializeComponent();
            _reportService = new ReportService(new AppDbContext());
            btnThongKe.Click += btnThongKe_Click;
            _lvExpenseBreakdown = BuildExpenseBreakdownListView();
            pnlExpenseHost.Controls.Add(_lvExpenseBreakdown);
            pnlExpenseHost.Resize += (_, _) => ResizeExpenseColumns();
            ResizeExpenseColumns();
        }

        private static ListView BuildExpenseBreakdownListView()
        {
            ListView listView = new ListView
            {
                Name = "lvExpenseBreakdown",
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                HideSelection = false,
                MultiSelect = false,
                View = View.Details,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };

            listView.Columns.Add("Danh mục", 420, HorizontalAlignment.Left);
            listView.Columns.Add("Số tiền", 240, HorizontalAlignment.Right);
            listView.Columns.Add("Tỷ trọng", 140, HorizontalAlignment.Right);
            return listView;
        }

        private void ResizeExpenseColumns()
        {
            if (_lvExpenseBreakdown.Columns.Count < 3)
            {
                return;
            }

            int totalWidth = _lvExpenseBreakdown.ClientSize.Width;
            if (totalWidth <= 0)
            {
                return;
            }

            int categoryWidth = (int)(totalWidth * 0.55);
            int amountWidth = (int)(totalWidth * 0.25);
            int ratioWidth = Math.Max(90, totalWidth - categoryWidth - amountWidth - 2);

            _lvExpenseBreakdown.Columns[0].Width = Math.Max(180, categoryWidth);
            _lvExpenseBreakdown.Columns[1].Width = Math.Max(140, amountWidth);
            _lvExpenseBreakdown.Columns[2].Width = ratioWidth;
        }

        private async void ucDashboard_Load(object sender, EventArgs e)
        {
            await InitializeDashboardAsync();
        }

        private async Task InitializeDashboardAsync()
        {
            DateTime now = DateTime.Now;
            dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpToDate.Value = now;

            await LoadReportDataAsync();
        }

        public async Task LoadReportDataAsync()
        {
            try
            {
                SetLoadingState(true);

                if (!TryGetReportParameters(out int tenantId, out int? branchId, out DateTime fromDate, out DateTime toDate))
                {
                    return;
                }

                DashboardSummaryDTO summary = await _reportService.GetOverviewAsync(tenantId, branchId, fromDate, toDate);
                List<CategoryStatisticDTO> categoryStats = await _reportService.GetExpenseByCategoryAsync(tenantId, branchId, fromDate, toDate);

                UpdateDashboardCards(summary);
                BindPieChart(categoryStats);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private bool TryGetReportParameters(out int tenantId, out int? branchId, out DateTime fromDate, out DateTime toDate)
        {
            tenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId ?? 0;
            branchId = SessionManager.CurrentBranchId;
            fromDate = dtpFromDate.Value.Date;
            toDate = dtpToDate.Value.Date;

            if (tenantId <= 0)
            {
                MessageBox.Show("Chưa xác định Tenant hiện tại. Vui lòng chọn tenant/đăng nhập lại.", "Thiếu ngữ cảnh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (fromDate > toDate)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Sai khoảng thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (branchId.HasValue && branchId.Value <= 0)
            {
                branchId = null;
            }

            return true;
        }

        private void UpdateDashboardCards(DashboardSummaryDTO summary)
        {
            // 1. CAP NHAT CARD DOANH THU
            lblNetIncome.Text = summary.NetIncome.ToString("N0") + " đ";
            lblOutputVAT.Text = $"+ Thuế GTGT thu hộ (Đầu ra): {summary.OutputVAT:N0} đ";

            // 2. CAP NHAT CARD CHI PHI
            lblNetExpense.Text = summary.NetExpense.ToString("N0") + " đ";
            lblInputVAT.Text = $"+ Thuế GTGT được khấu trừ (Đầu vào): {summary.InputVAT:N0} đ";

            // 3. CAP NHAT CARD LOI NHUAN & THUE
            lblNetProfit.Text = summary.NetProfit.ToString("N0") + " đ";
            lblEstimatedTax.Text = summary.EstimatedTaxPayable.ToString("N0") + " đ";

            // UX NANG CAO: DOI MAU THEO SO LIEU THUC TE
            if (summary.NetProfit < 0)
            {
                lblNetProfit.ForeColor = Color.IndianRed;
            }
            else
            {
                lblNetProfit.ForeColor = Color.SeaGreen;
            }

            if (summary.EstimatedTaxPayable > 0)
            {
                lblEstimatedTax.ForeColor = Color.IndianRed;
            }
            else
            {
                lblEstimatedTax.ForeColor = Color.SeaGreen;
            }
        }

        private void BindPieChart(List<CategoryStatisticDTO> stats)
        {
            _lvExpenseBreakdown.BeginUpdate();
            _lvExpenseBreakdown.Items.Clear();

            List<CategoryStatisticDTO> positiveStats = stats
                .Where(x => x.TotalAmount > 0)
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            decimal totalAmount = positiveStats.Sum(x => x.TotalAmount);

            if (positiveStats.Count == 0)
            {
                ListViewItem emptyItem = new ListViewItem("Chưa có phát sinh chi tiêu");
                emptyItem.SubItems.Add("0 đ");
                emptyItem.SubItems.Add("0.00%");
                _lvExpenseBreakdown.Items.Add(emptyItem);
                ResizeExpenseColumns();
                _lvExpenseBreakdown.EndUpdate();
                return;
            }

            foreach (CategoryStatisticDTO stat in positiveStats)
            {
                decimal ratio = totalAmount > 0 ? (stat.TotalAmount / totalAmount) * 100m : 0m;
                ListViewItem row = new ListViewItem(stat.CategoryName);
                row.SubItems.Add($"{stat.TotalAmount:N0} đ");
                row.SubItems.Add($"{ratio:N2}%");
                _lvExpenseBreakdown.Items.Add(row);
            }

            ResizeExpenseColumns();
            _lvExpenseBreakdown.EndUpdate();
        }

        private void SetLoadingState(bool isLoading)
        {
            btnThongKe.Enabled = !isLoading;
            btnThongKe.Text = isLoading ? "Đang tải dữ liệu..." : "Thống kê";
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private async void btnThongKe_Click(object? sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }
    }
}
