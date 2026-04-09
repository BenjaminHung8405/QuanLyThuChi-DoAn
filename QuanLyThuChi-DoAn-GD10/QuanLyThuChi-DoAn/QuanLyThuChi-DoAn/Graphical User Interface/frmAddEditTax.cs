using System;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class frmAddEditTax : Form
    {
        private readonly TaxService _taxService;
        private readonly AppDbContext _context;
        private int _taxId;
        private bool _isSaving;

        public int SavedTaxId { get; private set; }

        public frmAddEditTax()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _taxService = new TaxService(_context);
            _taxId = 0;

            WireEvents();
            ConfigureAddModeUi();
        }

        public frmAddEditTax(Tax existingTax) : this()
        {
            if (existingTax == null)
                throw new ArgumentNullException(nameof(existingTax));

            _taxId = existingTax.TaxId;
            txtName.Text = (existingTax.TaxName ?? string.Empty).Trim();
            numRate.Value = ClampTaxRate(existingTax.Rate);

            ConfigureEditModeUi();
        }

        private void WireEvents()
        {
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            FormClosed += frmAddEditTax_FormClosed;
        }

        private static decimal ClampTaxRate(decimal rate)
        {
            if (rate < 0m) return 0m;
            if (rate > 100m) return 100m;
            return decimal.Round(rate, 2, MidpointRounding.AwayFromZero);
        }

        private void ConfigureAddModeUi()
        {
            Text = "Thêm Mới Thuế Suất";
            lblHeaderTitle.Text = "THÊM THUẾ SUẤT MỚI";
            lblHeaderSubtitle.Text = "Tạo cấu hình thuế suất mới cho tenant";
            btnSave.Text = "💾 Thêm Mới";
            txtName.Focus();
        }

        private void ConfigureEditModeUi()
        {
            Text = "Cập Nhật Thuế Suất";
            lblHeaderTitle.Text = "CẬP NHẬT THUẾ SUẤT";
            lblHeaderSubtitle.Text = "Điều chỉnh tên và mức thuế suất hiện có";
            btnSave.Text = "💾 Cập Nhật";
        }

        private void frmAddEditTax_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _context.Dispose();
        }

        private bool ValidateInput(out string taxName)
        {
            taxName = txtName.Text.Trim();
            txtName.Text = taxName;

            if (string.IsNullOrWhiteSpace(taxName))
            {
                MessageBox.Show("Vui lòng nhập tên loại thuế (VD: VAT 10%)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (taxName.Length > 100)
            {
                MessageBox.Show("Tên loại thuế không được vượt quá 100 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            return true;
        }

        private void SetSavingState(bool isSaving)
        {
            _isSaving = isSaving;

            btnSave.Enabled = !isSaving;
            btnCancel.Enabled = !isSaving;
            txtName.Enabled = !isSaving;
            numRate.Enabled = !isSaving;
            UseWaitCursor = isSaving;
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!ValidateInput(out string taxName))
            {
                return;
            }

            try
            {
                SetSavingState(true);

                var taxObj = new Tax
                {
                    TaxId = _taxId,
                    TaxName = taxName,
                    Rate = numRate.Value,
                    IsActive = true
                };

                bool isSuccess;
                if (_taxId == 0)
                {
                    isSuccess = await _taxService.AddTaxAsync(taxObj);
                }
                else
                {
                    isSuccess = await _taxService.UpdateTaxAsync(taxObj);
                }

                if (isSuccess)
                {
                    SavedTaxId = taxObj.TaxId;
                    MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                MessageBox.Show("Không thể lưu dữ liệu thuế suất. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    SetSavingState(false);
                }
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
