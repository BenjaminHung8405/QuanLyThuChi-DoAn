using System;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class frmInputReason : Form
    {
        // Biến public để form cha (Lịch sử) lấy được lý do sau khi form này tắt
        public string CancelReason { get; private set; } = string.Empty;

        public frmInputReason()
        {
            InitializeComponent();
            txtReason.Focus();
        }

        private void btnConfirm_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Vui lòng không để trống lý do hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtReason.Text.Trim().Length < 10)
            {
                MessageBox.Show("Lý do hủy quá ngắn, vui lòng mô tả chi tiết hơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CancelReason = txtReason.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}