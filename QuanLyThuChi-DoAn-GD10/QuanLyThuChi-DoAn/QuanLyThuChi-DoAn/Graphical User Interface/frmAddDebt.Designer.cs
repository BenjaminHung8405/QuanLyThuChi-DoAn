namespace QuanLyThuChi_DoAn
{
    partial class frmAddDebt
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
            lblPartner = new Label();
            cboPartner = new ComboBox();
            lblDebtType = new Label();
            cboDebtType = new ComboBox();
            lblTotalAmount = new Label();
            txtTotalAmount = new NumericUpDown();
            lblDueDate = new Label();
            dtpDueDate = new DateTimePicker();
            lblNotes = new Label();
            txtNotes = new TextBox();
            pnlHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlMain.SuspendLayout();
            tableInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtTotalAmount).BeginInit();
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
            lblHeaderSubtitle.Size = new Size(265, 15);
            lblHeaderSubtitle.TabIndex = 1;
            lblHeaderSubtitle.Text = "Ghi nhận nợ phát sinh không kèm dòng tiền mặt";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(18, 10);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(219, 25);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "THÊM KHOẢN NỢ MỚI";
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
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(463, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(122, 36);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Hủy bỏ";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(56, 142, 60);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(327, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(122, 36);
            btnSave.TabIndex = 5;
            btnSave.Text = "Lưu công nợ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
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
            tableInput.Controls.Add(lblPartner, 0, 0);
            tableInput.Controls.Add(cboPartner, 1, 0);
            tableInput.Controls.Add(lblDebtType, 0, 1);
            tableInput.Controls.Add(cboDebtType, 1, 1);
            tableInput.Controls.Add(lblTotalAmount, 0, 2);
            tableInput.Controls.Add(txtTotalAmount, 1, 2);
            tableInput.Controls.Add(lblDueDate, 0, 3);
            tableInput.Controls.Add(dtpDueDate, 1, 3);
            tableInput.Controls.Add(lblNotes, 0, 4);
            tableInput.Controls.Add(txtNotes, 1, 4);
            tableInput.Dock = DockStyle.Top;
            tableInput.Location = new Point(20, 20);
            tableInput.Name = "tableInput";
            tableInput.RowCount = 5;
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tableInput.Size = new Size(560, 394);
            tableInput.TabIndex = 0;
            // 
            // lblPartner
            // 
            lblPartner.Anchor = AnchorStyles.Left;
            lblPartner.AutoSize = true;
            lblPartner.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPartner.Location = new Point(3, 23);
            lblPartner.Name = "lblPartner";
            lblPartner.Size = new Size(70, 19);
            lblPartner.TabIndex = 0;
            lblPartner.Text = "Đối tác (*)";
            // 
            // cboPartner
            // 
            cboPartner.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboPartner.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPartner.FormattingEnabled = true;
            cboPartner.Location = new Point(148, 21);
            cboPartner.Name = "cboPartner";
            cboPartner.Size = new Size(409, 23);
            cboPartner.TabIndex = 0;
            // 
            // lblDebtType
            // 
            lblDebtType.Anchor = AnchorStyles.Left;
            lblDebtType.AutoSize = true;
            lblDebtType.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDebtType.Location = new Point(3, 89);
            lblDebtType.Name = "lblDebtType";
            lblDebtType.Size = new Size(106, 19);
            lblDebtType.TabIndex = 2;
            lblDebtType.Text = "Loại công nợ (*)";
            // 
            // cboDebtType
            // 
            cboDebtType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboDebtType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDebtType.FormattingEnabled = true;
            cboDebtType.Location = new Point(148, 87);
            cboDebtType.Name = "cboDebtType";
            cboDebtType.Size = new Size(409, 23);
            cboDebtType.TabIndex = 1;
            cboDebtType.SelectedIndexChanged += cboDebtType_SelectedIndexChanged;
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.Anchor = AnchorStyles.Left;
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalAmount.Location = new Point(3, 155);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(89, 19);
            lblTotalAmount.TabIndex = 4;
            lblTotalAmount.Text = "Số tiền nợ (*)";
            // 
            // txtTotalAmount
            // 
            txtTotalAmount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTotalAmount.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTotalAmount.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            txtTotalAmount.Location = new Point(148, 150);
            txtTotalAmount.Maximum = decimal.MaxValue;
            txtTotalAmount.Name = "txtTotalAmount";
            txtTotalAmount.Size = new Size(409, 29);
            txtTotalAmount.TabIndex = 2;
            txtTotalAmount.TextAlign = HorizontalAlignment.Right;
            txtTotalAmount.ThousandsSeparator = true;
            // 
            // lblDueDate
            // 
            lblDueDate.Anchor = AnchorStyles.Left;
            lblDueDate.AutoSize = true;
            lblDueDate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDueDate.Location = new Point(3, 221);
            lblDueDate.Name = "lblDueDate";
            lblDueDate.Size = new Size(106, 19);
            lblDueDate.TabIndex = 6;
            lblDueDate.Text = "Hạn thanh toán";
            // 
            // dtpDueDate
            // 
            dtpDueDate.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtpDueDate.CustomFormat = "dd/MM/yyyy";
            dtpDueDate.Format = DateTimePickerFormat.Custom;
            dtpDueDate.Location = new Point(148, 219);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.ShowCheckBox = true;
            dtpDueDate.Size = new Size(409, 23);
            dtpDueDate.TabIndex = 3;
            // 
            // lblNotes
            // 
            lblNotes.Anchor = AnchorStyles.Left;
            lblNotes.AutoSize = true;
            lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNotes.Location = new Point(3, 319);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(94, 19);
            lblNotes.TabIndex = 8;
            lblNotes.Text = "Ghi chú/Lý do";
            // 
            // txtNotes
            // 
            txtNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNotes.Location = new Point(148, 285);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(409, 88);
            txtNotes.TabIndex = 4;
            // 
            // frmAddDebt
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
            Name = "frmAddDebt";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ghi nhận công nợ";
            Load += frmAddDebt_Load;
            Shown += frmAddDebt_Shown;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFooter.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            tableInput.ResumeLayout(false);
            tableInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtTotalAmount).EndInit();
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
        private Label lblPartner;
        private ComboBox cboPartner;
        private Label lblDebtType;
        private ComboBox cboDebtType;
        private Label lblTotalAmount;
        private NumericUpDown txtTotalAmount;
        private Label lblDueDate;
        private DateTimePicker dtpDueDate;
        private Label lblNotes;
        private TextBox txtNotes;
    }
}
