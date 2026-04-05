namespace QuanLyThuChi_DoAn
{
    partial class frmAddUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblHeaderSubtitle = new Label();
            lblHeaderTitle = new Label();
            pnlFooter = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            pnlMain = new Panel();
            tableInput = new TableLayoutPanel();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblBranch = new Label();
            cbBranch = new ComboBox();
            lblRole = new Label();
            cbRole = new ComboBox();
            pnlHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlMain.SuspendLayout();
            tableInput.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.SteelBlue;
            pnlHeader.Controls.Add(lblHeaderSubtitle);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(18, 10, 18, 10);
            pnlHeader.Size = new Size(600, 74);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderSubtitle
            // 
            lblHeaderSubtitle.AutoSize = true;
            lblHeaderSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHeaderSubtitle.ForeColor = Color.WhiteSmoke;
            lblHeaderSubtitle.Location = new Point(20, 43);
            lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            lblHeaderSubtitle.Size = new Size(211, 15);
            lblHeaderSubtitle.TabIndex = 1;
            lblHeaderSubtitle.Text = "Thiết lập thông tin tài khoản nhân viên";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(18, 10);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(222, 25);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "THÊM NHÂN VIÊN MỚI";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.WhiteSmoke;
            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 501);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(16, 10, 16, 10);
            pnlFooter.Size = new Size(600, 60);
            pnlFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(99, 99, 99);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(463, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(122, 36);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "❌ Hủy bỏ";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(56, 142, 60);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(275, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(174, 36);
            btnSave.TabIndex = 5;
            btnSave.Text = "💾 Lưu thông tin";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(tableInput);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 74);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(20);
            pnlMain.Size = new Size(600, 427);
            pnlMain.TabIndex = 1;
            // 
            // tableInput
            // 
            tableInput.ColumnCount = 2;
            tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableInput.Controls.Add(lblFullName, 0, 0);
            tableInput.Controls.Add(txtFullName, 1, 0);
            tableInput.Controls.Add(lblUsername, 0, 1);
            tableInput.Controls.Add(txtUsername, 1, 1);
            tableInput.Controls.Add(lblPassword, 0, 2);
            tableInput.Controls.Add(txtPassword, 1, 2);
            tableInput.Controls.Add(lblBranch, 0, 3);
            tableInput.Controls.Add(cbBranch, 1, 3);
            tableInput.Controls.Add(lblRole, 0, 4);
            tableInput.Controls.Add(cbRole, 1, 4);
            tableInput.Dock = DockStyle.Top;
            tableInput.Location = new Point(20, 20);
            tableInput.Name = "tableInput";
            tableInput.RowCount = 5;
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.Size = new Size(560, 330);
            tableInput.TabIndex = 0;
            // 
            // lblFullName
            // 
            lblFullName.Anchor = AnchorStyles.Left;
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFullName.Location = new Point(3, 23);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(88, 19);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Họ và Tên (*)";
            // 
            // txtFullName
            // 
            txtFullName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtFullName.Location = new Point(148, 21);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(409, 23);
            txtFullName.TabIndex = 0;
            // 
            // lblUsername
            // 
            lblUsername.Anchor = AnchorStyles.Left;
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.Location = new Point(3, 89);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(118, 19);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Tên đăng nhập (*)";
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtUsername.Location = new Point(148, 87);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(409, 23);
            txtUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = AnchorStyles.Left;
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.Location = new Point(3, 155);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(86, 19);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu (*)";
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.Location = new Point(148, 153);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(409, 23);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblBranch
            // 
            lblBranch.Anchor = AnchorStyles.Left;
            lblBranch.AutoSize = true;
            lblBranch.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBranch.Location = new Point(3, 221);
            lblBranch.Name = "lblBranch";
            lblBranch.Size = new Size(90, 19);
            lblBranch.TabIndex = 6;
            lblBranch.Text = "Chi nhánh (*)";
            // 
            // cbBranch
            // 
            cbBranch.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBranch.FormattingEnabled = true;
            cbBranch.Location = new Point(148, 219);
            cbBranch.Name = "cbBranch";
            cbBranch.Size = new Size(409, 23);
            cbBranch.TabIndex = 3;
            // 
            // lblRole
            // 
            lblRole.Anchor = AnchorStyles.Left;
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRole.Location = new Point(3, 287);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(77, 19);
            lblRole.TabIndex = 8;
            lblRole.Text = "Chức vụ (*)";
            // 
            // cbRole
            // 
            cbRole.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRole.FormattingEnabled = true;
            cbRole.Location = new Point(148, 285);
            cbRole.Name = "cbRole";
            cbRole.Size = new Size(409, 23);
            cbRole.TabIndex = 4;
            // 
            // frmAddUser
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(600, 561);
            Controls.Add(pnlMain);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUser";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thêm Nhân Viên Mới";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFooter.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            tableInput.ResumeLayout(false);
            tableInput.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblHeaderSubtitle;
        private Label lblHeaderTitle;
        private Panel pnlFooter;
        private Button btnCancel;
        private Button btnSave;
        private Panel pnlMain;
        private TableLayoutPanel tableInput;
        private Label lblFullName;
        private TextBox txtFullName;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblBranch;
        private ComboBox cbBranch;
        private Label lblRole;
        private ComboBox cbRole;
    }
}