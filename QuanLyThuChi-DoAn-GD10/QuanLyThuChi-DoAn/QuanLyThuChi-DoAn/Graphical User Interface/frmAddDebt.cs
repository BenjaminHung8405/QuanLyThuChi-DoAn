using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    public partial class frmAddDebt : Form
    {
        private readonly AppDbContext _context;
        private readonly PartnerService _partnerService;
        private readonly DebtService _debtService;

        private sealed class DebtTypeOption
        {
            public string Display { get; set; }
            public string Value { get; set; }
        }

        public frmAddDebt()
        {
            InitializeComponent();

            _context = new AppDbContext();
            _partnerService = new PartnerService(_context);
            _debtService = new DebtService(_context);

            LoadPartners();
            InitDebtTypeComboBox();
            InitDebtCreatedDateField();
        }

        private void frmAddDebt_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyHeaderStyleByDebtType();
                dtpDueDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitDebtCreatedDateField()
        {
            // Ngày lập công nợ được hệ thống tự động gán theo ngày hiện tại.
            lblDueDate.Text = "Ngày lập công nợ (*)";
            dtpDueDate.ShowCheckBox = false;
            dtpDueDate.Enabled = false;
            dtpDueDate.Value = DateTime.Now;
        }

        private void frmAddDebt_Shown(object sender, EventArgs e)
        {
            cboPartner.Focus();
        }

        private void InitDebtTypeComboBox()
        {
            var debtTypes = new[]
            {
                new DebtTypeOption { Display = "Khách nợ mình (Phải thu)", Value = "RECEIVABLE" },
                new DebtTypeOption { Display = "Mình nợ đối tác (Phải trả)", Value = "PAYABLE" }
            };

            cboDebtType.DataSource = debtTypes;
            cboDebtType.DisplayMember = nameof(DebtTypeOption.Display);
            cboDebtType.ValueMember = nameof(DebtTypeOption.Value);
            cboDebtType.SelectedIndex = 0;
        }

        private void LoadPartners()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
                throw new InvalidOperationException("Không có tenant trong phiên đăng nhập.");

            int tenantId = SessionManager.CurrentTenantId.Value;
            var partners = _partnerService.GetPartnersByTenant(tenantId);
            // Nếu muốn xài GetAllActive không theo Tenant có thể triển khai thêm trong PartnerService

            cboPartner.DataSource = partners;
            cboPartner.DisplayMember = "PartnerName";
            cboPartner.ValueMember = "PartnerId";

            cboPartner.SelectedIndex = partners.Any() ? 0 : -1;
        }

        private void cboDebtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyHeaderStyleByDebtType();
        }

        private void ApplyHeaderStyleByDebtType()
        {
            string debtType = cboDebtType.SelectedValue?.ToString();
            if (string.Equals(debtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase))
            {
                pnlHeader.BackColor = Color.SteelBlue;
            }
            else if (string.Equals(debtType, "PAYABLE", StringComparison.OrdinalIgnoreCase))
            {
                pnlHeader.BackColor = Color.IndianRed;
            }
            else
            {
                pnlHeader.BackColor = Color.SteelBlue;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Validate dữ liệu
            if (cboPartner.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Đối tác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalAmount = txtTotalAmount.Value;
            if (totalAmount <= 0)
            {
                MessageBox.Show("Số tiền nợ phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy dữ liệu
            int partnerId = (int)cboPartner.SelectedValue;
            string debtType = (cboDebtType.SelectedItem as DebtTypeOption)?.Value ?? "RECEIVABLE";
            DateTime? dueDate = DateTime.Now.Date;
            string notes = txtNotes.Text.Trim();

            // 3. Gọi Service lưu vào DB
            try
            {
                if (_debtService.AddDebt(partnerId, debtType, totalAmount, dueDate, notes))
                {
                    MessageBox.Show("Thêm công nợ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Thêm công nợ thất bại, vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput(out int partnerId, out string debtType, out decimal totalAmount)
        {
            partnerId = 0;
            debtType = null;
            totalAmount = 0;

            if (cboPartner.SelectedValue == null || !int.TryParse(cboPartner.SelectedValue.ToString(), out partnerId) || partnerId <= 0)
            {
                MessageBox.Show("Vui lòng chọn đối tác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPartner.Focus();
                return false;
            }

            debtType = cboDebtType.SelectedValue?.ToString();
            if (string.IsNullOrWhiteSpace(debtType))
            {
                MessageBox.Show("Vui lòng chọn loại công nợ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDebtType.Focus();
                return false;
            }

            totalAmount = txtTotalAmount.Value;
            if (totalAmount <= 0)
            {
                MessageBox.Show("Số tiền nợ không hợp lệ. Vui lòng nhập số dương lớn hơn 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTotalAmount.Focus();
                return false;
            }

            return true;
        }
    }
}
