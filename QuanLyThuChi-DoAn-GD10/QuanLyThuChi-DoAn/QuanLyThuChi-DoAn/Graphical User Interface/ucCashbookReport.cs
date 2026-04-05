using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.DTOs;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Helpers;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class ucCashbookReport : UserControl
    {
        private readonly ReportService _reportService;
        private List<CashbookDetailDTO> _detailData = new();
        private List<CashbookSummaryDTO> _summaryData = new();

        public ucCashbookReport()
        {
            InitializeComponent();
            _reportService = new ReportService(new AppDbContext());
            btnExportExcel.Enabled = false;
        }

        private async void ucCashbookReport_Load(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpToDate.Value = now;
            await LoadDataAsync();
        }

        private async void btnXemBaoCao_Click(object? sender, EventArgs e)
        {
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Sai khoảng thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadDataAsync();
        }

        public async Task ReloadDataAsync()
        {
            await LoadDataAsync();
        }

        private bool TryGetFilterParameters(out int tenantId, out int? branchId, out DateTime fromDate, out DateTime toDate)
        {
            tenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId ?? 0;
            branchId = SessionManager.CurrentBranchId ?? SessionManager.BranchId;

            if (branchId.HasValue && branchId.Value <= 0)
            {
                branchId = null;
            }

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

            return true;
        }

        private async Task LoadDataAsync()
        {
            if (!TryGetFilterParameters(out int tenantId, out int? branchId, out DateTime fromDate, out DateTime toDate))
            {
                dgvReport.DataSource = null;
                dgvReport.Columns.Clear();
                btnExportExcel.Enabled = false;
                return;
            }

            try
            {
                SetLoadingState(true);

                dgvReport.DataSource = null;
                dgvReport.Columns.Clear();

                if (radChiTiet.Checked)
                {
                    _detailData = await _reportService.GetCashbookDetailsAsync(tenantId, branchId, fromDate, toDate);
                    _summaryData = new List<CashbookSummaryDTO>();
                    dgvReport.DataSource = _detailData;
                    FormatDetailGrid();
                    btnExportExcel.Enabled = _detailData.Count > 0;
                }
                else
                {
                    _summaryData = await _reportService.GetCashbookSummaryAsync(tenantId, branchId, fromDate, toDate);
                    _detailData = new List<CashbookDetailDTO>();
                    dgvReport.DataSource = _summaryData;
                    FormatSummaryGrid();
                    btnExportExcel.Enabled = _summaryData.Count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo dòng tiền: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void FormatDetailGrid()
        {
            if (dgvReport.Columns["TransactionDate"] is DataGridViewColumn transDateColumn)
            {
                transDateColumn.HeaderText = "Ngày Giao Dịch";
                transDateColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                transDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                transDateColumn.FillWeight = 90;
            }

            if (dgvReport.Columns["TransactionType"] is DataGridViewColumn transTypeColumn)
            {
                transTypeColumn.HeaderText = "Loại Phiếu";
                transTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                transTypeColumn.FillWeight = 80;
            }

            if (dgvReport.Columns["CategoryName"] is DataGridViewColumn categoryColumn)
            {
                categoryColumn.HeaderText = "Danh Mục";
                categoryColumn.FillWeight = 180;
            }

            if (dgvReport.Columns["Notes"] is DataGridViewColumn notesColumn)
            {
                notesColumn.HeaderText = "Diễn Giải";
                notesColumn.FillWeight = 220;
            }

            if (dgvReport.Columns["CreatorName"] is DataGridViewColumn creatorColumn)
            {
                creatorColumn.HeaderText = "Người Lập";
                creatorColumn.FillWeight = 130;
            }

            string[] moneyCols = { "IncomeAmount", "ExpenseAmount", "RunningBalance" };
            string[] headers = { "Số Tiền Thu", "Số Tiền Chi", "TỒN QUỸ" };

            for (int i = 0; i < moneyCols.Length; i++)
            {
                if (dgvReport.Columns[moneyCols[i]] is DataGridViewColumn col)
                {
                    col.HeaderText = headers[i];
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            if (dgvReport.Columns["RunningBalance"] is DataGridViewColumn runningBalanceColumn)
            {
                runningBalanceColumn.DefaultCellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
                runningBalanceColumn.DefaultCellStyle.ForeColor = Color.Blue;
            }
        }

        private void FormatSummaryGrid()
        {
            if (dgvReport.Columns["TransactionType"] is DataGridViewColumn transTypeColumn)
            {
                transTypeColumn.HeaderText = "Loại (Thu/Chi)";
                transTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                transTypeColumn.FillWeight = 90;
            }

            if (dgvReport.Columns["CategoryName"] is DataGridViewColumn categoryColumn)
            {
                categoryColumn.HeaderText = "Danh Mục";
                categoryColumn.FillWeight = 200;
            }

            if (dgvReport.Columns["TotalTransactions"] is DataGridViewColumn countColumn)
            {
                countColumn.HeaderText = "Số Lượng Phiếu";
                countColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                countColumn.FillWeight = 120;
            }

            if (dgvReport.Columns["TotalAmount"] is DataGridViewColumn amountColumn)
            {
                amountColumn.HeaderText = "Tổng tiền";
                amountColumn.DefaultCellStyle.Format = "N0";
                amountColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                amountColumn.FillWeight = 130;
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnXemBaoCao.Enabled = !isLoading;
            btnXemBaoCao.Text = isLoading ? "Đang xử lý..." : "📊 Xem Báo Cáo";
            btnExportExcel.Enabled = !isLoading && (radChiTiet.Checked ? _detailData.Count > 0 : _summaryData.Count > 0);
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void btnExportExcel_Click(object? sender, EventArgs e)
        {
            string title = $"BÁO CÁO DÒNG TIỀN TỪ {dtpFromDate.Value:dd/MM/yyyy} ĐẾN {dtpToDate.Value:dd/MM/yyyy}";
            string fileName = $"BaoCaoDongTien_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            if (radChiTiet.Checked && _detailData.Count > 0)
            {
                ExcelHelper.ExportListToExcel(_detailData, "SoQuy", fileName, title);
                return;
            }

            if (radTongHop.Checked && _summaryData.Count > 0)
            {
                ExcelHelper.ExportListToExcel(_summaryData, "TongHop", fileName, title);
                return;
            }

            MessageBox.Show("Chưa có dữ liệu để xuất Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
