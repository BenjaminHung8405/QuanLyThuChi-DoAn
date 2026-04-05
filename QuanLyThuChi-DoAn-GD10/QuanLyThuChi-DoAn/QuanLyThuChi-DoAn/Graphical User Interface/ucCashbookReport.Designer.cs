namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class ucCashbookReport
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
            grpViewMode = new GroupBox();
            radTongHop = new RadioButton();
            radChiTiet = new RadioButton();
            btnXemBaoCao = new Button();
            btnExportExcel = new Button();
            pnlMain = new Panel();
            dgvReport = new DataGridView();
            pnlHeader.SuspendLayout();
            tlpHeader.SuspendLayout();
            grpViewMode.SuspendLayout();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
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
            pnlHeader.Padding = new Padding(16, 10, 16, 10);
            pnlHeader.Size = new Size(1168, 98);
            pnlHeader.TabIndex = 0;
            // 
            // tlpHeader
            // 
            tlpHeader.ColumnCount = 8;
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 154F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpHeader.Controls.Add(lblTitle, 0, 0);
            tlpHeader.Controls.Add(lblFromDate, 0, 1);
            tlpHeader.Controls.Add(dtpFromDate, 1, 1);
            tlpHeader.Controls.Add(lblToDate, 2, 1);
            tlpHeader.Controls.Add(dtpToDate, 3, 1);
            tlpHeader.Controls.Add(grpViewMode, 4, 1);
            tlpHeader.Controls.Add(btnXemBaoCao, 6, 1);
            tlpHeader.Controls.Add(btnExportExcel, 7, 1);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(16, 10);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.RowCount = 2;
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpHeader.Size = new Size(1134, 76);
            tlpHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Left;
            lblTitle.AutoSize = true;
            tlpHeader.SetColumnSpan(lblTitle, 8);
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(31, 63, 94);
            lblTitle.Location = new Point(3, 2);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(267, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BÁO CÁO DÒNG TIỀN";
            // 
            // lblFromDate
            // 
            lblFromDate.Anchor = AnchorStyles.Left;
            lblFromDate.AutoSize = true;
            lblFromDate.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFromDate.Location = new Point(3, 48);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(68, 19);
            lblFromDate.TabIndex = 1;
            lblFromDate.Text = "Từ ngày:";
            // 
            // dtpFromDate
            // 
            dtpFromDate.Anchor = AnchorStyles.Left;
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpFromDate.Location = new Point(80, 46);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(140, 23);
            dtpFromDate.TabIndex = 2;
            // 
            // lblToDate
            // 
            lblToDate.Anchor = AnchorStyles.Left;
            lblToDate.AutoSize = true;
            lblToDate.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblToDate.Location = new Point(228, 48);
            lblToDate.Name = "lblToDate";
            lblToDate.Size = new Size(73, 19);
            lblToDate.TabIndex = 3;
            lblToDate.Text = "Đến ngày:";
            // 
            // dtpToDate
            // 
            dtpToDate.Anchor = AnchorStyles.Left;
            dtpToDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Location = new Point(307, 46);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(140, 23);
            dtpToDate.TabIndex = 4;
            // 
            // grpViewMode
            // 
            grpViewMode.Controls.Add(radTongHop);
            grpViewMode.Controls.Add(radChiTiet);
            grpViewMode.Dock = DockStyle.Fill;
            grpViewMode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            grpViewMode.Location = new Point(455, 37);
            grpViewMode.Margin = new Padding(0, 3, 0, 0);
            grpViewMode.Name = "grpViewMode";
            grpViewMode.Size = new Size(300, 39);
            grpViewMode.TabIndex = 5;
            grpViewMode.TabStop = false;
            grpViewMode.Text = "Chế độ xem:";
            // 
            // radTongHop
            // 
            radTongHop.AutoSize = true;
            radTongHop.Location = new Point(132, 16);
            radTongHop.Name = "radTongHop";
            radTongHop.Size = new Size(146, 19);
            radTongHop.TabIndex = 1;
            radTongHop.Text = "Tổng hợp theo danh mục";
            radTongHop.UseVisualStyleBackColor = true;
            // 
            // radChiTiet
            // 
            radChiTiet.AutoSize = true;
            radChiTiet.Checked = true;
            radChiTiet.Location = new Point(12, 16);
            radChiTiet.Name = "radChiTiet";
            radChiTiet.Size = new Size(109, 19);
            radChiTiet.TabIndex = 0;
            radChiTiet.TabStop = true;
            radChiTiet.Text = "Sổ quỹ chi tiết";
            radChiTiet.UseVisualStyleBackColor = true;
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
            btnXemBaoCao.Location = new Point(823, 40);
            btnXemBaoCao.Margin = new Padding(6, 6, 8, 6);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(140, 30);
            btnXemBaoCao.TabIndex = 6;
            btnXemBaoCao.Text = "📊 Xem Báo Cáo";
            btnXemBaoCao.UseVisualStyleBackColor = false;
            btnXemBaoCao.Click += btnXemBaoCao_Click;
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
            btnExportExcel.Location = new Point(980, 40);
            btnExportExcel.Margin = new Padding(9, 6, 3, 6);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(151, 30);
            btnExportExcel.TabIndex = 7;
            btnExportExcel.Text = "📥 Xuất Excel";
            btnExportExcel.UseVisualStyleBackColor = false;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(dgvReport);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 98);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(10);
            pnlMain.Size = new Size(1168, 539);
            pnlMain.TabIndex = 1;
            // 
            // dgvReport
            // 
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(233, 242, 250);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(46, 67, 86);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(233, 242, 250);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(46, 67, 86);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvReport.ColumnHeadersHeight = 42;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(214, 231, 248);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(35, 35, 35);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvReport.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(248, 251, 255);
            dgvReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReport.Dock = DockStyle.Fill;
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.GridColor = Color.FromArgb(220, 228, 236);
            dgvReport.Location = new Point(10, 10);
            dgvReport.MultiSelect = false;
            dgvReport.Name = "dgvReport";
            dgvReport.ReadOnly = true;
            dgvReport.RowHeadersVisible = false;
            dgvReport.RowTemplate.Height = 34;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.Size = new Size(1148, 519);
            dgvReport.TabIndex = 0;
            // 
            // ucCashbookReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucCashbookReport";
            Size = new Size(1168, 637);
            Load += ucCashbookReport_Load;
            pnlHeader.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            tlpHeader.PerformLayout();
            grpViewMode.ResumeLayout(false);
            grpViewMode.PerformLayout();
            pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
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
        private GroupBox grpViewMode;
        private RadioButton radTongHop;
        private RadioButton radChiTiet;
        private Button btnXemBaoCao;
        private Button btnExportExcel;
        private Panel pnlMain;
        private DataGridView dgvReport;
    }
}
