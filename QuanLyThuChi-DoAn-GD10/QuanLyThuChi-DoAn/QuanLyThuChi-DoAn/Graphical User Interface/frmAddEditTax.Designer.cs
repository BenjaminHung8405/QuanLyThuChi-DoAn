namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class frmAddEditTax
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
            lblTaxName = new Label();
            txtName = new TextBox();
            lblRate = new Label();
            numRate = new NumericUpDown();
            pnlHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlMain.SuspendLayout();
            tableInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRate).BeginInit();
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
            lblHeaderSubtitle.Size = new Size(257, 15);
            lblHeaderSubtitle.TabIndex = 1;
            lblHeaderSubtitle.Text = "Thiết lập tên loại thuế và mức phần trăm áp dụng";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(18, 10);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(237, 25);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "THÊM THUẾ SUẤT MỚI";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.WhiteSmoke;
            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 225);
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
            btnCancel.Text = "❌ Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(56, 142, 60);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(349, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(124, 36);
            btnSave.TabIndex = 3;
            btnSave.Text = "💾 Lưu";
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
            pnlMain.Size = new Size(620, 151);
            pnlMain.TabIndex = 1;
            // 
            // tableInput
            // 
            tableInput.ColumnCount = 2;
            tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableInput.Controls.Add(lblTaxName, 0, 0);
            tableInput.Controls.Add(txtName, 1, 0);
            tableInput.Controls.Add(lblRate, 0, 1);
            tableInput.Controls.Add(numRate, 1, 1);
            tableInput.Dock = DockStyle.Top;
            tableInput.Location = new Point(20, 20);
            tableInput.Name = "tableInput";
            tableInput.RowCount = 2;
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tableInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tableInput.Size = new Size(580, 112);
            tableInput.TabIndex = 0;
            // 
            // lblTaxName
            // 
            lblTaxName.Anchor = AnchorStyles.Left;
            lblTaxName.AutoSize = true;
            lblTaxName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTaxName.Location = new Point(3, 18);
            lblTaxName.Name = "lblTaxName";
            lblTaxName.Size = new Size(120, 19);
            lblTaxName.TabIndex = 0;
            lblTaxName.Text = "Tên loại thuế (*)";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(163, 16);
            txtName.MaxLength = 100;
            txtName.Name = "txtName";
            txtName.Size = new Size(414, 23);
            txtName.TabIndex = 0;
            // 
            // lblRate
            // 
            lblRate.Anchor = AnchorStyles.Left;
            lblRate.AutoSize = true;
            lblRate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRate.Location = new Point(3, 74);
            lblRate.Name = "lblRate";
            lblRate.Size = new Size(102, 19);
            lblRate.TabIndex = 2;
            lblRate.Text = "Mức suất (%)";
            // 
            // numRate
            // 
            numRate.DecimalPlaces = 2;
            numRate.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            numRate.Location = new Point(163, 72);
            numRate.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numRate.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numRate.Name = "numRate";
            numRate.Size = new Size(180, 23);
            numRate.TabIndex = 1;
            // 
            // frmAddEditTax
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(620, 285);
            Controls.Add(pnlMain);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddEditTax";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thêm/Sửa Thuế suất";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFooter.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            tableInput.ResumeLayout(false);
            tableInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRate).EndInit();
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
        private Label lblTaxName;
        private TextBox txtName;
        private Label lblRate;
        private NumericUpDown numRate;
    }
}