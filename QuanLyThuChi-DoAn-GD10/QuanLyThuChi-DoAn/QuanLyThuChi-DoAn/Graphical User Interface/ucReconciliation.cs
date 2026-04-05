using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.DTOs;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Helpers;

namespace QuanLyThuChi_DoAn
{
    public partial class ucReconciliation : UserControl
    {
        private readonly ReportService _reportService;
        private List<BranchReconciliationDTO> _currentData = new();

        public ucReconciliation()
        {
            InitializeComponent();
            _reportService = new ReportService(new AppDbContext());

            btnXemBaoCao.Click += btnXemBaoCao_Click;
            btnExportExcel.Click += btnExportExcel_Click;
            btnExportExcel.Enabled = false;
        }

        private async void ucReconciliation_Load(object? sender, EventArgs e)
        {
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            DateTime now = DateTime.Now;
            dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpToDate.Value = now.Date;

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                SetLoadingState(true);

                if (!TryGetFilterParameters(out int tenantId, out DateTime fromDate, out DateTime toDate))
                {
                    _currentData = new List<BranchReconciliationDTO>();
                    BindGrid();
                    return;
                }

                _currentData = await _reportService.GetChainReconciliationAsync(tenantId, fromDate, toDate);
                BindGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo đối soát: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private bool TryGetFilterParameters(out int tenantId, out DateTime fromDate, out DateTime toDate)
        {
            tenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId ?? 0;
            fromDate = dtpFromDate.Value.Date;
            toDate = dtpToDate.Value.Date;

            if (!SessionManager.CanViewSummaryReports)
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo đối soát toàn chuỗi.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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

        private void BindGrid()
        {
            dgvReconciliation.DataSource = null;
            dgvReconciliation.DataSource = _currentData;
            FormatGrid();
            btnExportExcel.Enabled = _currentData.Count > 0;
        }

        private void FormatGrid()
        {
            if (dgvReconciliation.Columns.Count == 0)
            {
                return;
            }

            if (dgvReconciliation.Columns["PerformanceStatus"] != null)
            {
                dgvReconciliation.Columns["PerformanceStatus"].Visible = false;
            }

            if (dgvReconciliation.Columns["BranchName"] != null)
            {
                dgvReconciliation.Columns["BranchName"].HeaderText = "Chi Nhánh";
                dgvReconciliation.Columns["BranchName"].FillWeight = 150;
            }

            if (dgvReconciliation.Columns["TransactionCount"] != null)
            {
                dgvReconciliation.Columns["TransactionCount"].HeaderText = "Số Giao Dịch";
                dgvReconciliation.Columns["TransactionCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            string[] moneyColumns = { "TotalIncome", "TotalExpense", "NetRevenue" };
            string[] headers = { "Tổng Thu", "Tổng Chi", "Lợi Nhuận Ròng" };

            for (int i = 0; i < moneyColumns.Length; i++)
            {
                DataGridViewColumn? col = dgvReconciliation.Columns[moneyColumns[i]];
                if (col != null)
                {
                    col.HeaderText = headers[i];
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

        }

        private void SetLoadingState(bool isLoading)
        {
            btnXemBaoCao.Enabled = !isLoading;
            btnXemBaoCao.Text = isLoading ? "Đang tải..." : "📊 Xem Báo Cáo";
            btnExportExcel.Enabled = !isLoading && _currentData.Count > 0;
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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

        private void btnExportExcel_Click(object? sender, EventArgs e)
        {
            if (_currentData.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu để xuất Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string fileName = $"DoiSoatChuoi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            string title = $"BÁO CÁO ĐỐI SOÁT CHUỖI TỪ {dtpFromDate.Value:dd/MM/yyyy} ĐẾN {dtpToDate.Value:dd/MM/yyyy}";

            ExcelHelper.ExportListToExcel(_currentData, "Doi Soat", fileName, title);
        }
    }
}
