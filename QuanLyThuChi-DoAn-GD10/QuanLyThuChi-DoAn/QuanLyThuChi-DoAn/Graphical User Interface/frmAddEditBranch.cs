using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.BLL.Services;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    public partial class frmAddEditBranch : Form
    {
        private readonly AppDbContext _context;
        private readonly BranchService _branchService;
        private readonly int? _branchId;
        private readonly bool _isEditMode;

        private Branch? _editingBranch;
        private bool _isSaving;
        private bool _isFormReady;
        private bool _allowCloseWhenSaving;

        public int? SavedBranchId { get; private set; }

        public frmAddEditBranch() : this(null)
        {
        }

        public frmAddEditBranch(int branchId) : this((int?)branchId)
        {
        }

        private frmAddEditBranch(int? branchId)
        {
            InitializeComponent();

            _branchId = branchId;
            _isEditMode = branchId.HasValue && branchId.Value > 0;

            _context = new AppDbContext();
            _branchService = new BranchService(_context);

            ConfigureModeTexts();
            WireEvents();

            txtBranchName.PlaceholderText = "Ví dụ: Chi nhánh Quận 1";
            txtAddress.PlaceholderText = "Ví dụ: 123 Lê Lợi, Quận 1, TP.HCM";
            txtPhone.PlaceholderText = "Ví dụ: 0901234567";

            btnSave.Enabled = false;
        }

        private void WireEvents()
        {
            Load += frmAddEditBranch_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            txtBranchName.TextChanged += (_, __) => UpdateSaveButtonState();
            txtAddress.TextChanged += (_, __) => UpdateSaveButtonState();
            txtPhone.TextChanged += (_, __) => UpdateSaveButtonState();
            FormClosing += frmAddEditBranch_FormClosing;
            FormClosed += frmAddEditBranch_FormClosed;
        }

        private void ConfigureModeTexts()
        {
            if (_isEditMode)
            {
                Text = "Cập nhật Chi nhánh";
                lblHeaderTitle.Text = "CẬP NHẬT CHI NHÁNH";
                lblHeaderSubtitle.Text = "Điều chỉnh thông tin chi nhánh hiện có";
                btnSave.Text = "💾 Cập nhật";
            }
            else
            {
                Text = "Thêm Chi nhánh mới";
                lblHeaderTitle.Text = "THÊM CHI NHÁNH MỚI";
                lblHeaderSubtitle.Text = "Thiết lập thông tin chi nhánh trong hệ thống";
                btnSave.Text = "💾 Lưu thông tin";
            }
        }

        private async void frmAddEditBranch_Load(object? sender, EventArgs e)
        {
            UseWaitCursor = true;
            try
            {
                if (_isEditMode)
                {
                    await LoadBranchForEditAsync();
                }

                _isFormReady = true;
                UpdateSaveButtonState();
                txtBranchName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
            }
            finally
            {
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                }
            }
        }

        private void frmAddEditBranch_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isSaving && !_allowCloseWhenSaving)
            {
                e.Cancel = true;
            }
        }

        private void frmAddEditBranch_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _context.Dispose();
        }

        private int GetTenantIdOrThrow()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                throw new InvalidOperationException("Chưa chọn Tenant ngữ cảnh. Vui lòng chọn Tenant trước khi thao tác chi nhánh.");
            }

            return SessionManager.CurrentTenantId.Value;
        }

        private static string NormalizeWhitespace(string? value)
        {
            return string.Join(" ", (value ?? string.Empty)
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }

        private async Task LoadBranchForEditAsync()
        {
            if (!_branchId.HasValue || _branchId.Value <= 0)
            {
                throw new InvalidOperationException("Không xác định được chi nhánh cần cập nhật.");
            }

            int tenantId = GetTenantIdOrThrow();
            Branch branch = await _branchService.GetByIdAsync(_branchId.Value);
            if (branch == null)
            {
                throw new InvalidOperationException("Không tìm thấy chi nhánh cần cập nhật.");
            }

            if (branch.TenantId != tenantId)
            {
                throw new InvalidOperationException("Chi nhánh này không thuộc Tenant ngữ cảnh hiện tại.");
            }

            _editingBranch = branch;
            txtBranchName.Text = branch.BranchName ?? string.Empty;
            txtAddress.Text = branch.Address ?? string.Empty;
            txtPhone.Text = branch.Phone ?? string.Empty;
        }

        private bool ValidateInput(out string branchName, out string? address, out string? phone)
        {
            branchName = NormalizeWhitespace(txtBranchName.Text);
            address = NormalizeWhitespace(txtAddress.Text);
            phone = txtPhone.Text.Trim();

            txtBranchName.Text = branchName;
            txtAddress.Text = address;
            txtPhone.Text = phone;

            if (string.IsNullOrWhiteSpace(branchName))
            {
                MessageBox.Show("Vui lòng nhập tên chi nhánh.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBranchName.Focus();
                return false;
            }

            if (branchName.Length > 100)
            {
                MessageBox.Show("Tên chi nhánh không được vượt quá 100 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBranchName.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(address) && address.Length > 200)
            {
                MessageBox.Show("Địa chỉ không được vượt quá 200 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(phone))
            {
                if (phone.Length > 20)
                {
                    MessageBox.Show("Số điện thoại không được vượt quá 20 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return false;
                }

                bool isPhoneValid = phone.All(ch => char.IsDigit(ch) || ch == ' ' || ch == '+' || ch == '-' || ch == '(' || ch == ')' || ch == '.');
                if (!isPhoneValid)
                {
                    MessageBox.Show("Số điện thoại chỉ được chứa số và các ký tự + - ( ) .", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return false;
                }
            }

            address = string.IsNullOrWhiteSpace(address) ? null : address;
            phone = string.IsNullOrWhiteSpace(phone) ? null : phone;
            return true;
        }

        private void UpdateSaveButtonState()
        {
            if (!_isFormReady || _isSaving)
            {
                btnSave.Enabled = false;
                return;
            }

            btnSave.Enabled = !string.IsNullOrWhiteSpace(txtBranchName.Text);
        }

        private void SetSavingState(bool isSaving)
        {
            _isSaving = isSaving;
            if (!isSaving)
            {
                _allowCloseWhenSaving = false;
            }

            btnSave.Enabled = !isSaving;
            btnCancel.Enabled = !isSaving;
            txtBranchName.Enabled = !isSaving;
            txtAddress.Enabled = !isSaving;
            txtPhone.Enabled = !isSaving;
            UseWaitCursor = isSaving;

            if (!isSaving)
            {
                UpdateSaveButtonState();
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!ValidateInput(out string branchName, out string? address, out string? phone))
            {
                return;
            }

            try
            {
                SetSavingState(true);
                int tenantId = GetTenantIdOrThrow();

                bool isSuccess;
                if (_isEditMode)
                {
                    if (_editingBranch == null)
                    {
                        throw new InvalidOperationException("Không tìm thấy dữ liệu chi nhánh để cập nhật.");
                    }

                    _editingBranch.TenantId = tenantId;
                    _editingBranch.BranchName = branchName;
                    _editingBranch.Address = address ?? string.Empty;
                    _editingBranch.Phone = phone ?? string.Empty;

                    isSuccess = await _branchService.UpdateAsync(_editingBranch);
                    if (isSuccess)
                    {
                        SavedBranchId = _editingBranch.BranchId;
                    }
                }
                else
                {
                    var newBranch = new Branch
                    {
                        TenantId = tenantId,
                        BranchName = branchName,
                        Address = address ?? string.Empty,
                        Phone = phone ?? string.Empty,
                        IsActive = true
                    };

                    isSuccess = await _branchService.AddAsync(newBranch);
                    if (isSuccess)
                    {
                        SavedBranchId = newBranch.BranchId;
                    }
                }

                if (isSuccess)
                {
                    string successMessage = _isEditMode
                        ? "Cập nhật chi nhánh thành công!"
                        : "Thêm chi nhánh mới thành công!";

                    MessageBox.Show(successMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    _allowCloseWhenSaving = true;
                    Close();
                    return;
                }

                MessageBox.Show("Không thể lưu dữ liệu chi nhánh. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xử lý chi nhánh", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
