namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class frmAddEditBranch
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
            lblBranchName = new Label();
            txtBranchName = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
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
            pnlHeader.Size = new Size(620, 74);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderSubtitle
            // 
            lblHeaderSubtitle.AutoSize = true;
            lblHeaderSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHeaderSubtitle.ForeColor = Color.WhiteSmoke;
            lblHeaderSubtitle.Location = new Point(20, 43);
            lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            lblHeaderSubtitle.Size = new Size(233, 15);
            lblHeaderSubtitle.TabIndex = 1;
            lblHeaderSubtitle.Text = "Thiết lập thông tin chi nhánh trong hệ thống";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(18, 10);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(223, 25);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "THÊM CHI NHÁNH MỚI";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.WhiteSmoke;
            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 341);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(16, 10, 16, 10);
            pnlFooter.Size = new Size(620, 60);
            pnlFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(99, 99, 99);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(483, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(122, 36);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "❌ Hủy bỏ";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(56, 142, 60);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(323, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 36);
            btnSave.TabIndex = 3;
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
            pnlMain.Size = new Size(620, 267);
            pnlMain.TabIndex = 1;
            // 
            // tableInput
            // 
            tableInput.ColumnCount = 2;
            tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableInput.Controls.Add(lblBranchName, 0, 0);
            tableInput.Controls.Add(txtBranchName, 1, 0);
            tableInput.Controls.Add(lblAddress, 0, 1);
            tableInput.Controls.Add(txtAddress, 1, 1);
            tableInput.Controls.Add(lblPhone, 0, 2);
            tableInput.Controls.Add(txtPhone, 1, 2);
            tableInput.Dock = DockStyle.Top;
            tableInput.Location = new Point(20, 20);
            tableInput.Name = "tableInput";
            tableInput.RowCount = 3;
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.Size = new Size(580, 228);
            tableInput.TabIndex = 0;
            // 
            // lblBranchName
            // 
            lblBranchName.Anchor = AnchorStyles.Left;
            lblBranchName.AutoSize = true;
            lblBranchName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBranchName.Location = new Point(3, 23);
            lblBranchName.Name = "lblBranchName";
            lblBranchName.Size = new Size(118, 19);
            lblBranchName.TabIndex = 0;
            lblBranchName.Text = "Tên chi nhánh (*)";
            // 
            // txtBranchName
            // 
            txtBranchName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtBranchName.Location = new Point(148, 21);
            txtBranchName.MaxLength = 100;
            txtBranchName.Name = "txtBranchName";
            txtBranchName.Size = new Size(429, 23);
            txtBranchName.TabIndex = 0;
            // 
            // lblAddress
            // 
            lblAddress.Anchor = AnchorStyles.Left;
            lblAddress.AutoSize = true;
            lblAddress.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAddress.Location = new Point(3, 103);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(57, 19);
            lblAddress.TabIndex = 2;
            lblAddress.Text = "Địa chỉ";
            // 
            // txtAddress
            // 
            txtAddress.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtAddress.Location = new Point(148, 69);
            txtAddress.MaxLength = 200;
            txtAddress.Multiline = true;
            txtAddress.Name = "txtAddress";
            txtAddress.ScrollBars = ScrollBars.Vertical;
            txtAddress.Size = new Size(429, 90);
            txtAddress.TabIndex = 1;
            // 
            // lblPhone
            // 
            lblPhone.Anchor = AnchorStyles.Left;
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPhone.Location = new Point(3, 185);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(76, 19);
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Điện thoại";
            // 
            // txtPhone
            // 
            txtPhone.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPhone.Location = new Point(148, 183);
            txtPhone.MaxLength = 20;
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(429, 23);
            txtPhone.TabIndex = 2;
            // 
            // frmAddEditBranch
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(620, 401);
            Controls.Add(pnlMain);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddEditBranch";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thêm/Sửa Chi nhánh";
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
        private Label lblBranchName;
        private TextBox txtBranchName;
        private Label lblAddress;
        private TextBox txtAddress;
        private Label lblPhone;
        private TextBox txtPhone;
    }
}