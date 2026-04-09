using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.DTOs;
using QuanLyThuChi_DoAn.Helpers;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class ucAuditLogViewer : UserControl
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        private readonly BindingSource _bindingSource;
        private readonly Font _actionDangerFont;
        private readonly Font _actionNormalFont;

        private List<AuditLogDTO> _currentLogs = new List<AuditLogDTO>();
        private bool _isBusy;

        public ucAuditLogViewer()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _auditLogService = new AuditLogService(_context);
            _bindingSource = new BindingSource();
            _actionDangerFont = new Font(dgvLogs.Font, FontStyle.Bold);
            _actionNormalFont = new Font(dgvLogs.Font, FontStyle.Regular);

            dgvLogs.DataSource = _bindingSource;

            ConfigureGrid();
            InitializeFilterDefaults();
            WireEvents();

            Disposed += OnDisposed;
        }

        private void WireEvents()
        {
            btnFilter.Click += btnFilter_Click;
            btnExport.Click += btnExport_Click;
            dgvLogs.CellFormatting += dgvLogs_CellFormatting;
            dgvLogs.DataBindingComplete += dgvLogs_DataBindingComplete;
        }

        private void ConfigureGrid()
        {
            dgvLogs.AutoGenerateColumns = true;
            dgvLogs.ReadOnly = true;
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.MultiSelect = false;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogs.BackgroundColor = Color.White;
            dgvLogs.BorderStyle = BorderStyle.None;
        }

        private void InitializeFilterDefaults()
        {
            DateTime today = DateTime.Now.Date;
            dtpFromDate.Value = today.AddDays(-30);
            dtpToDate.Value = today;

            List<ActionFilterOption> actionItems = new List<ActionFilterOption>
            {
                new ActionFilterOption("ALL", "Tất cả"),
                new ActionFilterOption("CREATE", "Thêm mới"),
                new ActionFilterOption("UPDATE", "Cập nhật"),
                new ActionFilterOption("VOID_TRANSACTION", "Hủy phiếu"),
                new ActionFilterOption("DELETE", "Xóa")
            };

            cboActionType.DataSource = actionItems;
            cboActionType.DisplayMember = nameof(ActionFilterOption.DisplayName);
            cboActionType.ValueMember = nameof(ActionFilterOption.ActionCode);
            cboActionType.SelectedValue = "ALL";
        }

        private async void ucAuditLogViewer_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            if (!SessionManager.IsSuperAdmin && !SessionManager.IsTenantAdmin)
            {
                MessageBox.Show(
                    "Bạn không có quyền truy cập màn hình Nhật ký hệ thống.",
                    "Từ chối truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Enabled = false;
                return;
            }

            try
            {
                await LoadUserFilterAsync();
                await LoadLogsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu nhật ký hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadUserFilterAsync()
        {
            List<AuditLogUserOptionDTO> users = await _auditLogService.GetFilterUsersAsync();
            cboUser.DataSource = users;
            cboUser.DisplayMember = nameof(AuditLogUserOptionDTO.DisplayName);
            cboUser.ValueMember = nameof(AuditLogUserOptionDTO.UserId);
            cboUser.SelectedValue = 0;
        }

        private async void btnFilter_Click(object? sender, EventArgs e)
        {
            await LoadLogsAsync();
        }

        private async Task LoadLogsAsync()
        {
            if (_isBusy)
            {
                return;
            }

            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Sai khoảng thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetBusyState(true);

                string actionCode = cboActionType.SelectedValue?.ToString() ?? "ALL";
                int selectedUserId = cboUser.SelectedValue is int value ? value : 0;

                _currentLogs = await _auditLogService.GetLogsAsync(
                    dtpFromDate.Value,
                    dtpToDate.Value,
                    actionCode,
                    selectedUserId > 0 ? selectedUserId : null);

                _bindingSource.DataSource = _currentLogs;
                FormatGridColumns();
                btnExport.Enabled = _currentLogs.Count > 0;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc nhật ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void btnExport_Click(object? sender, EventArgs e)
        {
            if (_currentLogs.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu để xuất Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<AuditLogExportRow> exportRows = _currentLogs.Select(log => new AuditLogExportRow
            {
                ThoiGian = log.ActionDate.ToString("dd/MM/yyyy HH:mm:ss"),
                NguoiThucHien = log.ActorDisplay,
                HanhDong = log.ActionTypeDisplay,
                PhanHe = log.ModuleDisplay,
                MaThamChieu = log.ReferenceCode,
                ChiTiet = log.Details
            }).ToList();

            string fileName = $"AuditLogs_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            string title = $"NHAT KY HE THONG TU {dtpFromDate.Value:dd/MM/yyyy} DEN {dtpToDate.Value:dd/MM/yyyy}";
            ExcelHelper.ExportListToExcel(exportRows, "AuditLogs", fileName, title);
        }

        private void FormatGridColumns()
        {
            if (dgvLogs.Columns.Count == 0)
            {
                return;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.LogId)] is DataGridViewColumn logIdColumn)
            {
                logIdColumn.Visible = false;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.ActionCode)] is DataGridViewColumn actionCodeColumn)
            {
                actionCodeColumn.Visible = false;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.ActionDate)] is DataGridViewColumn actionDateColumn)
            {
                actionDateColumn.HeaderText = "Thời gian";
                actionDateColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                actionDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                actionDateColumn.FillWeight = 125;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.ActorDisplay)] is DataGridViewColumn actorColumn)
            {
                actorColumn.HeaderText = "Người thực hiện";
                actorColumn.FillWeight = 170;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.ActionTypeDisplay)] is DataGridViewColumn actionTypeColumn)
            {
                actionTypeColumn.HeaderText = "Hành động";
                actionTypeColumn.FillWeight = 130;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.ModuleDisplay)] is DataGridViewColumn moduleColumn)
            {
                moduleColumn.HeaderText = "Phân hệ";
                moduleColumn.FillWeight = 110;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.ReferenceCode)] is DataGridViewColumn referenceColumn)
            {
                referenceColumn.HeaderText = "Mã tham chiếu";
                referenceColumn.FillWeight = 140;
            }

            if (dgvLogs.Columns[nameof(AuditLogDTO.Details)] is DataGridViewColumn detailsColumn)
            {
                detailsColumn.HeaderText = "Chi tiết / Lý do";
                detailsColumn.FillWeight = 325;
                detailsColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            dgvLogs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        }

        private void dgvLogs_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (!(dgvLogs.Columns[e.ColumnIndex].Name == nameof(AuditLogDTO.ActionTypeDisplay)))
            {
                return;
            }

            if (!(dgvLogs.Rows[e.RowIndex].DataBoundItem is AuditLogDTO dto))
            {
                return;
            }

            string actionCode = dto.ActionCode;
            if (actionCode == "VOID_TRANSACTION" || actionCode == "DELETE")
            {
                e.CellStyle.ForeColor = Color.Firebrick;
                e.CellStyle.Font = _actionDangerFont;
                return;
            }

            if (actionCode == "CREATE")
            {
                e.CellStyle.ForeColor = Color.SeaGreen;
                e.CellStyle.Font = _actionNormalFont;
                return;
            }

            if (actionCode == "UPDATE")
            {
                e.CellStyle.ForeColor = Color.SteelBlue;
                e.CellStyle.Font = _actionNormalFont;
                return;
            }

            e.CellStyle.ForeColor = Color.Black;
            e.CellStyle.Font = _actionNormalFont;
        }

        private void dgvLogs_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatGridColumns();
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            btnFilter.Enabled = !isBusy;
            btnExport.Enabled = !isBusy && _currentLogs.Count > 0;
            btnFilter.Text = isBusy ? "Đang tải..." : "🔍 Lọc dữ liệu";
            Cursor = isBusy ? Cursors.WaitCursor : Cursors.Default;
        }

        private void OnDisposed(object? sender, EventArgs e)
        {
            _actionDangerFont.Dispose();
            _actionNormalFont.Dispose();
            _context.Dispose();
        }

        private sealed class ActionFilterOption
        {
            public ActionFilterOption(string actionCode, string displayName)
            {
                ActionCode = actionCode;
                DisplayName = displayName;
            }

            public string ActionCode { get; }
            public string DisplayName { get; }
        }

        private sealed class AuditLogExportRow
        {
            public string ThoiGian { get; set; } = string.Empty;
            public string NguoiThucHien { get; set; } = string.Empty;
            public string HanhDong { get; set; } = string.Empty;
            public string PhanHe { get; set; } = string.Empty;
            public string MaThamChieu { get; set; } = string.Empty;
            public string ChiTiet { get; set; } = string.Empty;
        }
    }
}
