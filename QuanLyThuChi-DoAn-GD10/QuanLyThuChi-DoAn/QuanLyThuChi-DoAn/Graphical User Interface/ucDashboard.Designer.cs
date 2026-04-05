namespace QuanLyThuChi_DoAn
{
    partial class ucDashboard
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
            pnlFilter = new Panel();
            tlpFilter = new TableLayoutPanel();
            lblFromDate = new Label();
            dtpFromDate = new DateTimePicker();
            lblToDate = new Label();
            dtpToDate = new DateTimePicker();
            pnlFilterSpacer = new Panel();
            btnThongKe = new Button();
            tlpKpi = new TableLayoutPanel();
            pnlCardIncome = new Panel();
            lblTotalIncome = new Label();
            lblTitleIncome = new Label();
            pnlCardExpense = new Panel();
            lblTotalExpense = new Label();
            lblTitleExpense = new Label();
            pnlCardBalance = new Panel();
            lblBalance = new Label();
            lblTitleBalance = new Label();
            pnlCardDebt = new Panel();
            lblTotalPayable = new Label();
            lblTotalReceivable = new Label();
            lblTitleDebt = new Label();
            pnlCharts = new Panel();
            pnlChartCard = new Panel();
            pnlExpenseHost = new Panel();
            lblChartSubtitle = new Label();
            lblChartTitle = new Label();
            pnlFilter.SuspendLayout();
            tlpFilter.SuspendLayout();
            tlpKpi.SuspendLayout();
            pnlCardIncome.SuspendLayout();
            pnlCardExpense.SuspendLayout();
            pnlCardBalance.SuspendLayout();
            pnlCardDebt.SuspendLayout();
            pnlCharts.SuspendLayout();
            pnlChartCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.White;
            pnlFilter.BorderStyle = BorderStyle.FixedSingle;
            pnlFilter.Controls.Add(tlpFilter);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Location = new Point(20, 20);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Padding = new Padding(16, 14, 16, 14);
            pnlFilter.Size = new Size(1140, 80);
            pnlFilter.TabIndex = 0;
            // 
            // tlpFilter
            // 
            tlpFilter.ColumnCount = 6;
            tlpFilter.ColumnStyles.Add(new ColumnStyle());
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            tlpFilter.ColumnStyles.Add(new ColumnStyle());
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
            tlpFilter.Controls.Add(lblFromDate, 0, 0);
            tlpFilter.Controls.Add(dtpFromDate, 1, 0);
            tlpFilter.Controls.Add(lblToDate, 2, 0);
            tlpFilter.Controls.Add(dtpToDate, 3, 0);
            tlpFilter.Controls.Add(pnlFilterSpacer, 4, 0);
            tlpFilter.Controls.Add(btnThongKe, 5, 0);
            tlpFilter.Dock = DockStyle.Fill;
            tlpFilter.Location = new Point(16, 14);
            tlpFilter.Name = "tlpFilter";
            tlpFilter.RowCount = 1;
            tlpFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFilter.Size = new Size(1106, 50);
            tlpFilter.TabIndex = 0;
            // 
            // lblFromDate
            // 
            lblFromDate.Anchor = AnchorStyles.Left;
            lblFromDate.AutoSize = true;
            lblFromDate.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFromDate.Location = new Point(3, 15);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(68, 19);
            lblFromDate.TabIndex = 0;
            lblFromDate.Text = "Từ ngày:";
            // 
            // dtpFromDate
            // 
            dtpFromDate.Anchor = AnchorStyles.Left;
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpFromDate.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.Location = new Point(77, 11);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(178, 26);
            dtpFromDate.TabIndex = 1;
            // 
            // lblToDate
            // 
            lblToDate.Anchor = AnchorStyles.Left;
            lblToDate.AutoSize = true;
            lblToDate.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblToDate.Location = new Point(267, 15);
            lblToDate.Name = "lblToDate";
            lblToDate.Padding = new Padding(8, 0, 0, 0);
            lblToDate.Size = new Size(76, 19);
            lblToDate.TabIndex = 2;
            lblToDate.Text = "Đến ngày:";
            // 
            // dtpToDate
            // 
            dtpToDate.Anchor = AnchorStyles.Left;
            dtpToDate.CustomFormat = "dd/MM/yyyy";
            dtpToDate.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Location = new Point(349, 11);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(178, 26);
            dtpToDate.TabIndex = 3;
            // 
            // pnlFilterSpacer
            // 
            pnlFilterSpacer.Dock = DockStyle.Fill;
            pnlFilterSpacer.Location = new Point(539, 3);
            pnlFilterSpacer.Name = "pnlFilterSpacer";
            pnlFilterSpacer.Size = new Size(388, 44);
            pnlFilterSpacer.TabIndex = 4;
            // 
            // btnThongKe
            // 
            btnThongKe.BackColor = Color.FromArgb(33, 120, 210);
            btnThongKe.Cursor = Cursors.Hand;
            btnThongKe.Dock = DockStyle.Fill;
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.FlatAppearance.MouseDownBackColor = Color.FromArgb(22, 95, 168);
            btnThongKe.FlatAppearance.MouseOverBackColor = Color.FromArgb(44, 136, 230);
            btnThongKe.FlatStyle = FlatStyle.Flat;
            btnThongKe.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThongKe.ForeColor = Color.White;
            btnThongKe.Location = new Point(933, 3);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Size = new Size(170, 44);
            btnThongKe.TabIndex = 5;
            btnThongKe.Text = "Thống kê";
            btnThongKe.UseVisualStyleBackColor = false;
            // 
            // tlpKpi
            // 
            tlpKpi.BackColor = Color.Transparent;
            tlpKpi.ColumnCount = 4;
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpKpi.Controls.Add(pnlCardIncome, 0, 0);
            tlpKpi.Controls.Add(pnlCardExpense, 1, 0);
            tlpKpi.Controls.Add(pnlCardBalance, 2, 0);
            tlpKpi.Controls.Add(pnlCardDebt, 3, 0);
            tlpKpi.Dock = DockStyle.Top;
            tlpKpi.Location = new Point(20, 100);
            tlpKpi.Name = "tlpKpi";
            tlpKpi.RowCount = 1;
            tlpKpi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpKpi.Size = new Size(1140, 148);
            tlpKpi.TabIndex = 1;
            // 
            // pnlCardIncome
            // 
            pnlCardIncome.BackColor = Color.FromArgb(237, 250, 241);
            pnlCardIncome.BorderStyle = BorderStyle.FixedSingle;
            pnlCardIncome.Controls.Add(lblTotalIncome);
            pnlCardIncome.Controls.Add(lblTitleIncome);
            pnlCardIncome.Dock = DockStyle.Fill;
            pnlCardIncome.Location = new Point(6, 10);
            pnlCardIncome.Margin = new Padding(6, 10, 6, 10);
            pnlCardIncome.Name = "pnlCardIncome";
            pnlCardIncome.Padding = new Padding(16, 14, 16, 14);
            pnlCardIncome.Size = new Size(273, 128);
            pnlCardIncome.TabIndex = 0;
            // 
            // lblTotalIncome
            // 
            lblTotalIncome.Dock = DockStyle.Fill;
            lblTotalIncome.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalIncome.ForeColor = Color.SeaGreen;
            lblTotalIncome.Location = new Point(16, 35);
            lblTotalIncome.Name = "lblTotalIncome";
            lblTotalIncome.Size = new Size(239, 77);
            lblTotalIncome.TabIndex = 1;
            lblTotalIncome.Text = "0 VNĐ";
            lblTotalIncome.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTitleIncome
            // 
            lblTitleIncome.Dock = DockStyle.Top;
            lblTitleIncome.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitleIncome.ForeColor = Color.FromArgb(80, 80, 80);
            lblTitleIncome.Location = new Point(16, 14);
            lblTitleIncome.Name = "lblTitleIncome";
            lblTitleIncome.Size = new Size(239, 21);
            lblTitleIncome.TabIndex = 0;
            lblTitleIncome.Text = "TỔNG THU";
            // 
            // pnlCardExpense
            // 
            pnlCardExpense.BackColor = Color.FromArgb(254, 242, 242);
            pnlCardExpense.BorderStyle = BorderStyle.FixedSingle;
            pnlCardExpense.Controls.Add(lblTotalExpense);
            pnlCardExpense.Controls.Add(lblTitleExpense);
            pnlCardExpense.Dock = DockStyle.Fill;
            pnlCardExpense.Location = new Point(291, 10);
            pnlCardExpense.Margin = new Padding(6, 10, 6, 10);
            pnlCardExpense.Name = "pnlCardExpense";
            pnlCardExpense.Padding = new Padding(16, 14, 16, 14);
            pnlCardExpense.Size = new Size(273, 128);
            pnlCardExpense.TabIndex = 1;
            // 
            // lblTotalExpense
            // 
            lblTotalExpense.Dock = DockStyle.Fill;
            lblTotalExpense.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalExpense.ForeColor = Color.IndianRed;
            lblTotalExpense.Location = new Point(16, 35);
            lblTotalExpense.Name = "lblTotalExpense";
            lblTotalExpense.Size = new Size(239, 77);
            lblTotalExpense.TabIndex = 1;
            lblTotalExpense.Text = "0 VNĐ";
            lblTotalExpense.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTitleExpense
            // 
            lblTitleExpense.Dock = DockStyle.Top;
            lblTitleExpense.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitleExpense.ForeColor = Color.FromArgb(80, 80, 80);
            lblTitleExpense.Location = new Point(16, 14);
            lblTitleExpense.Name = "lblTitleExpense";
            lblTitleExpense.Size = new Size(239, 21);
            lblTitleExpense.TabIndex = 0;
            lblTitleExpense.Text = "TỔNG CHI";
            // 
            // pnlCardBalance
            // 
            pnlCardBalance.BackColor = Color.FromArgb(239, 246, 255);
            pnlCardBalance.BorderStyle = BorderStyle.FixedSingle;
            pnlCardBalance.Controls.Add(lblBalance);
            pnlCardBalance.Controls.Add(lblTitleBalance);
            pnlCardBalance.Dock = DockStyle.Fill;
            pnlCardBalance.Location = new Point(576, 10);
            pnlCardBalance.Margin = new Padding(6, 10, 6, 10);
            pnlCardBalance.Name = "pnlCardBalance";
            pnlCardBalance.Padding = new Padding(16, 14, 16, 14);
            pnlCardBalance.Size = new Size(273, 128);
            pnlCardBalance.TabIndex = 2;
            // 
            // lblBalance
            // 
            lblBalance.Dock = DockStyle.Fill;
            lblBalance.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalance.ForeColor = Color.RoyalBlue;
            lblBalance.Location = new Point(16, 35);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(239, 77);
            lblBalance.TabIndex = 1;
            lblBalance.Text = "0 VNĐ";
            lblBalance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTitleBalance
            // 
            lblTitleBalance.Dock = DockStyle.Top;
            lblTitleBalance.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitleBalance.ForeColor = Color.FromArgb(80, 80, 80);
            lblTitleBalance.Location = new Point(16, 14);
            lblTitleBalance.Name = "lblTitleBalance";
            lblTitleBalance.Size = new Size(239, 21);
            lblTitleBalance.TabIndex = 0;
            lblTitleBalance.Text = "TỒN QUỸ HIỆN TẠI";
            // 
            // pnlCardDebt
            // 
            pnlCardDebt.BackColor = Color.FromArgb(255, 249, 238);
            pnlCardDebt.BorderStyle = BorderStyle.FixedSingle;
            pnlCardDebt.Controls.Add(lblTotalPayable);
            pnlCardDebt.Controls.Add(lblTotalReceivable);
            pnlCardDebt.Controls.Add(lblTitleDebt);
            pnlCardDebt.Dock = DockStyle.Fill;
            pnlCardDebt.Location = new Point(861, 10);
            pnlCardDebt.Margin = new Padding(6, 10, 6, 10);
            pnlCardDebt.Name = "pnlCardDebt";
            pnlCardDebt.Padding = new Padding(16, 14, 16, 14);
            pnlCardDebt.Size = new Size(273, 128);
            pnlCardDebt.TabIndex = 3;
            // 
            // lblTotalPayable
            // 
            lblTotalPayable.Dock = DockStyle.Top;
            lblTotalPayable.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalPayable.ForeColor = Color.IndianRed;
            lblTotalPayable.Location = new Point(16, 70);
            lblTotalPayable.Name = "lblTotalPayable";
            lblTotalPayable.Size = new Size(239, 28);
            lblTotalPayable.TabIndex = 2;
            lblTotalPayable.Text = "Phải trả: 0 VNĐ";
            // 
            // lblTotalReceivable
            // 
            lblTotalReceivable.Dock = DockStyle.Top;
            lblTotalReceivable.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalReceivable.ForeColor = Color.SeaGreen;
            lblTotalReceivable.Location = new Point(16, 42);
            lblTotalReceivable.Name = "lblTotalReceivable";
            lblTotalReceivable.Size = new Size(239, 28);
            lblTotalReceivable.TabIndex = 1;
            lblTotalReceivable.Text = "Phải thu: 0 VNĐ";
            // 
            // lblTitleDebt
            // 
            lblTitleDebt.Dock = DockStyle.Top;
            lblTitleDebt.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitleDebt.ForeColor = Color.FromArgb(80, 80, 80);
            lblTitleDebt.Location = new Point(16, 14);
            lblTitleDebt.Name = "lblTitleDebt";
            lblTitleDebt.Size = new Size(239, 28);
            lblTitleDebt.TabIndex = 0;
            lblTitleDebt.Text = "CÔNG NỢ";
            // 
            // pnlCharts
            // 
            pnlCharts.Controls.Add(pnlChartCard);
            pnlCharts.Dock = DockStyle.Fill;
            pnlCharts.Location = new Point(20, 248);
            pnlCharts.Name = "pnlCharts";
            pnlCharts.Padding = new Padding(0, 12, 0, 0);
            pnlCharts.Size = new Size(1140, 462);
            pnlCharts.TabIndex = 2;
            // 
            // pnlChartCard
            // 
            pnlChartCard.BackColor = Color.White;
            pnlChartCard.BorderStyle = BorderStyle.FixedSingle;
            pnlChartCard.Controls.Add(pnlExpenseHost);
            pnlChartCard.Controls.Add(lblChartSubtitle);
            pnlChartCard.Controls.Add(lblChartTitle);
            pnlChartCard.Dock = DockStyle.Fill;
            pnlChartCard.Location = new Point(0, 12);
            pnlChartCard.Name = "pnlChartCard";
            pnlChartCard.Padding = new Padding(18);
            pnlChartCard.Size = new Size(1140, 450);
            pnlChartCard.TabIndex = 0;
            // 
            // pnlExpenseHost
            // 
            pnlExpenseHost.BackColor = Color.White;
            pnlExpenseHost.Dock = DockStyle.Fill;
            pnlExpenseHost.Location = new Point(18, 76);
            pnlExpenseHost.Name = "pnlExpenseHost";
            pnlExpenseHost.Padding = new Padding(4);
            pnlExpenseHost.Size = new Size(1102, 354);
            pnlExpenseHost.TabIndex = 2;
            // 
            // lblChartSubtitle
            // 
            lblChartSubtitle.Dock = DockStyle.Top;
            lblChartSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblChartSubtitle.ForeColor = Color.FromArgb(120, 120, 120);
            lblChartSubtitle.Location = new Point(18, 49);
            lblChartSubtitle.Name = "lblChartSubtitle";
            lblChartSubtitle.Size = new Size(1102, 27);
            lblChartSubtitle.TabIndex = 1;
            lblChartSubtitle.Text = "Top danh mục chi trong khoảng thời gian đã chọn";
            // 
            // lblChartTitle
            // 
            lblChartTitle.Dock = DockStyle.Top;
            lblChartTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblChartTitle.ForeColor = Color.FromArgb(40, 40, 40);
            lblChartTitle.Location = new Point(18, 18);
            lblChartTitle.Name = "lblChartTitle";
            lblChartTitle.Padding = new Padding(0, 0, 0, 10);
            lblChartTitle.Size = new Size(1102, 31);
            lblChartTitle.TabIndex = 0;
            lblChartTitle.Text = "Phân tích cơ cấu chi tiêu";
            // 
            // ucDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlCharts);
            Controls.Add(tlpKpi);
            Controls.Add(pnlFilter);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucDashboard";
            Padding = new Padding(20);
            Size = new Size(1180, 730);
            Load += ucDashboard_Load;
            pnlFilter.ResumeLayout(false);
            tlpFilter.ResumeLayout(false);
            tlpFilter.PerformLayout();
            tlpKpi.ResumeLayout(false);
            pnlCardIncome.ResumeLayout(false);
            pnlCardExpense.ResumeLayout(false);
            pnlCardBalance.ResumeLayout(false);
            pnlCardDebt.ResumeLayout(false);
            pnlCharts.ResumeLayout(false);
            pnlChartCard.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlFilter;
        private TableLayoutPanel tlpFilter;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Panel pnlFilterSpacer;
        private Button btnThongKe;
        private TableLayoutPanel tlpKpi;
        private Panel pnlCardIncome;
        private Label lblTitleIncome;
        private Label lblTotalIncome;
        private Panel pnlCardExpense;
        private Label lblTitleExpense;
        private Label lblTotalExpense;
        private Panel pnlCardBalance;
        private Label lblTitleBalance;
        private Label lblBalance;
        private Panel pnlCardDebt;
        private Label lblTitleDebt;
        private Label lblTotalReceivable;
        private Label lblTotalPayable;
        private Panel pnlCharts;
        private Panel pnlChartCard;
        private Label lblChartTitle;
        private Panel pnlExpenseHost;
        private Label lblChartSubtitle;
    }
}
