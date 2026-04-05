namespace QuanLyThuChi_DoAn
{
    partial class ucReconciliation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            tlpHeader = new TableLayoutPanel();
            lblTitle = new Label();
            lblFromDate = new Label();
            dtpFromDate = new DateTimePicker();
            lblToDate = new Label();
            dtpToDate = new DateTimePicker();
            pnlHeaderSpacer = new Panel();
            btnXemBaoCao = new Button();
            btnExportExcel = new Button();
            pnlMain = new Panel();
            dgvReconciliation = new DataGridView();
            pnlHeader.SuspendLayout();
            tlpHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReconciliation).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.WhiteSmoke;
            pnlHeader.BorderStyle = BorderStyle.FixedSingle;
            pnlHeader.Controls.Add(tlpHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(16, 12, 16, 12);
            pnlHeader.Size = new Size(1168, 80);
            pnlHeader.TabIndex = 0;
            // 
            // tlpHeader
            // 
            tlpHeader.ColumnCount = 9;
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 154F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 162F));
            tlpHeader.Controls.Add(lblTitle, 0, 0);
            tlpHeader.Controls.Add(lblFromDate, 2, 0);
            tlpHeader.Controls.Add(dtpFromDate, 3, 0);
            tlpHeader.Controls.Add(lblToDate, 4, 0);
            tlpHeader.Controls.Add(dtpToDate, 5, 0);
            tlpHeader.Controls.Add(pnlHeaderSpacer, 6, 0);
            tlpHeader.Controls.Add(btnXemBaoCao, 7, 0);
            tlpHeader.Controls.Add(btnExportExcel, 8, 0);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(16, 12);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.RowCount = 1;
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpHeader.Size = new Size(1134, 54);
            tlpHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Left;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(31, 63, 94);
            lblTitle.Location = new Point(3, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(312, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ĐỐI SOÁT CHUỖI CHI NHÁNH";
            // 
            // lblFromDate
            // 
            lblFromDate.Anchor = AnchorStyles.Left;
            lblFromDate.AutoSize = true;
            lblFromDate.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFromDate.Location = new Point(349, 17);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(68, 19);
            lblFromDate.TabIndex = 1;
            lblFromDate.Text = "Từ ngày:";
            // 
            // dtpFromDate
            // 
            dtpFromDate.Anchor = AnchorStyles.Left;
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpFromDate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.Location = new Point(423, 14);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(142, 25);
            dtpFromDate.TabIndex = 2;
            // 
            // lblToDate
            // 
            lblToDate.Anchor = AnchorStyles.Left;
            lblToDate.AutoSize = true;
            lblToDate.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblToDate.Location = new Point(571, 17);
            lblToDate.Name = "lblToDate";
            lblToDate.Size = new Size(73, 19);
            lblToDate.TabIndex = 3;
            lblToDate.Text = "Đến ngày:";
            // 
            // dtpToDate
            // 
            dtpToDate.Anchor = AnchorStyles.Left;
            dtpToDate.CustomFormat = "dd/MM/yyyy";
            dtpToDate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Location = new Point(650, 14);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(142, 25);
            dtpToDate.TabIndex = 4;
            // 
            // pnlHeaderSpacer
            // 
            pnlHeaderSpacer.Dock = DockStyle.Fill;
            pnlHeaderSpacer.Location = new Point(798, 3);
            pnlHeaderSpacer.Name = "pnlHeaderSpacer";
            pnlHeaderSpacer.Size = new Size(17, 48);
            pnlHeaderSpacer.TabIndex = 5;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.BackColor = Color.SteelBlue;
            btnXemBaoCao.Cursor = Cursors.Hand;
            btnXemBaoCao.Dock = DockStyle.Fill;
            btnXemBaoCao.FlatAppearance.BorderSize = 0;
            btnXemBaoCao.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 101, 145);
            btnXemBaoCao.FlatAppearance.MouseOverBackColor = Color.FromArgb(84, 142, 190);
            btnXemBaoCao.FlatStyle = FlatStyle.Flat;
            btnXemBaoCao.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXemBaoCao.ForeColor = Color.White;
            btnXemBaoCao.Location = new Point(821, 6);
            btnXemBaoCao.Margin = new Padding(3, 6, 8, 6);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(143, 42);
            btnXemBaoCao.TabIndex = 6;
            btnXemBaoCao.Text = "📊 Xem Báo Cáo";
            btnXemBaoCao.UseVisualStyleBackColor = false;
            // 
            // btnExportExcel
            // 
            btnExportExcel.BackColor = Color.SeaGreen;
            btnExportExcel.Cursor = Cursors.Hand;
            btnExportExcel.Dock = DockStyle.Fill;
            btnExportExcel.FlatAppearance.BorderSize = 0;
            btnExportExcel.FlatAppearance.MouseDownBackColor = Color.FromArgb(32, 111, 70);
            btnExportExcel.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 156, 104);
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExportExcel.ForeColor = Color.White;
            btnExportExcel.Location = new Point(975, 6);
            btnExportExcel.Margin = new Padding(3, 6, 3, 6);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(156, 42);
            btnExportExcel.TabIndex = 7;
            btnExportExcel.Text = "📥 Xuất Excel";
            btnExportExcel.UseVisualStyleBackColor = false;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(dgvReconciliation);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 80);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(10);
            pnlMain.Size = new Size(1168, 617);
            pnlMain.TabIndex = 1;
            // 
            // dgvReconciliation
            // 
            dgvReconciliation.AllowUserToAddRows = false;
            dgvReconciliation.AllowUserToDeleteRows = false;
            dgvReconciliation.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(233, 242, 250);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(46, 67, 86);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(233, 242, 250);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(46, 67, 86);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvReconciliation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvReconciliation.ColumnHeadersHeight = 42;
            dgvReconciliation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReconciliation.EnableHeadersVisualStyles = false;
            dgvReconciliation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(214, 231, 248);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(35, 35, 35);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvReconciliation.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(248, 251, 255);
            dgvReconciliation.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvReconciliation.BackgroundColor = Color.White;
            dgvReconciliation.BorderStyle = BorderStyle.None;
            dgvReconciliation.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReconciliation.Dock = DockStyle.Fill;
            dgvReconciliation.GridColor = Color.FromArgb(220, 228, 236);
            dgvReconciliation.Location = new Point(10, 10);
            dgvReconciliation.MultiSelect = false;
            dgvReconciliation.Name = "dgvReconciliation";
            dgvReconciliation.ReadOnly = true;
            dgvReconciliation.RowHeadersVisible = false;
            dgvReconciliation.RowTemplate.Height = 34;
            dgvReconciliation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReconciliation.Size = new Size(1148, 597);
            dgvReconciliation.TabIndex = 0;
            // 
            // ucReconciliation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucReconciliation";
            Size = new Size(1168, 697);
            Load += ucReconciliation_Load;
            pnlHeader.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            tlpHeader.PerformLayout();
            pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReconciliation).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private TableLayoutPanel tlpHeader;
        private Label lblTitle;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Panel pnlHeaderSpacer;
        private Button btnXemBaoCao;
        private Button btnExportExcel;
        private Panel pnlMain;
        private DataGridView dgvReconciliation;
    }
}
