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
            pnlCardNetIncome = new Panel();
            lblOutputVAT = new Label();
            lblNetIncome = new Label();
            lblTitleNetIncome = new Label();
            pnlNetIncomeAccent = new Panel();
            pnlCardNetExpense = new Panel();
            lblInputVAT = new Label();
            lblNetExpense = new Label();
            lblTitleNetExpense = new Label();
            pnlNetExpenseAccent = new Panel();
            pnlCardBusinessPerformance = new Panel();
            tlpBusinessMetrics = new TableLayoutPanel();
            lblEstimatedTax = new Label();
            lblEstimatedTaxCaption = new Label();
            lblNetProfit = new Label();
            lblNetProfitCaption = new Label();
            lblTitleBusinessPerformance = new Label();
            pnlBusinessPerformanceAccent = new Panel();
            pnlCharts = new Panel();
            pnlChartCard = new Panel();
            pnlExpenseHost = new Panel();
            lblChartSubtitle = new Label();
            lblChartTitle = new Label();
            pnlFilter.SuspendLayout();
            tlpFilter.SuspendLayout();
            tlpKpi.SuspendLayout();
            pnlCardNetIncome.SuspendLayout();
            pnlCardNetExpense.SuspendLayout();
            pnlCardBusinessPerformance.SuspendLayout();
            tlpBusinessMetrics.SuspendLayout();
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
            tlpKpi.ColumnCount = 3;
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tlpKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tlpKpi.Controls.Add(pnlCardNetIncome, 0, 0);
            tlpKpi.Controls.Add(pnlCardNetExpense, 1, 0);
            tlpKpi.Controls.Add(pnlCardBusinessPerformance, 2, 0);
            tlpKpi.Dock = DockStyle.Top;
            tlpKpi.Location = new Point(20, 100);
            tlpKpi.Name = "tlpKpi";
            tlpKpi.RowCount = 1;
            tlpKpi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpKpi.Size = new Size(1140, 148);
            tlpKpi.TabIndex = 1;
            // 
            // pnlCardNetIncome
            // 
            pnlCardNetIncome.BackColor = Color.White;
            pnlCardNetIncome.BorderStyle = BorderStyle.FixedSingle;
            pnlCardNetIncome.Controls.Add(lblOutputVAT);
            pnlCardNetIncome.Controls.Add(lblNetIncome);
            pnlCardNetIncome.Controls.Add(lblTitleNetIncome);
            pnlCardNetIncome.Controls.Add(pnlNetIncomeAccent);
            pnlCardNetIncome.Dock = DockStyle.Fill;
            pnlCardNetIncome.Location = new Point(8, 10);
            pnlCardNetIncome.Margin = new Padding(8, 10, 8, 10);
            pnlCardNetIncome.Name = "pnlCardNetIncome";
            pnlCardNetIncome.Size = new Size(364, 128);
            pnlCardNetIncome.TabIndex = 0;
            // 
            // lblOutputVAT
            // 
            lblOutputVAT.Dock = DockStyle.Bottom;
            lblOutputVAT.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblOutputVAT.ForeColor = Color.Gray;
            lblOutputVAT.Location = new Point(0, 100);
            lblOutputVAT.Name = "lblOutputVAT";
            lblOutputVAT.Padding = new Padding(16, 0, 16, 8);
            lblOutputVAT.Size = new Size(362, 26);
            lblOutputVAT.TabIndex = 3;
            lblOutputVAT.Text = "+ Thuế GTGT đầu ra: 0 VNĐ";
            lblOutputVAT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetIncome
            // 
            lblNetIncome.AutoSize = false;
            lblNetIncome.Dock = DockStyle.Fill;
            lblNetIncome.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetIncome.ForeColor = Color.SeaGreen;
            lblNetIncome.Location = new Point(0, 35);
            lblNetIncome.Name = "lblNetIncome";
            lblNetIncome.Padding = new Padding(16, 0, 16, 0);
            lblNetIncome.Size = new Size(362, 91);
            lblNetIncome.TabIndex = 2;
            lblNetIncome.Text = "0 VNĐ";
            lblNetIncome.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTitleNetIncome
            // 
            lblTitleNetIncome.Dock = DockStyle.Top;
            lblTitleNetIncome.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitleNetIncome.ForeColor = Color.DimGray;
            lblTitleNetIncome.Location = new Point(0, 5);
            lblTitleNetIncome.Name = "lblTitleNetIncome";
            lblTitleNetIncome.Padding = new Padding(16, 8, 16, 0);
            lblTitleNetIncome.Size = new Size(362, 30);
            lblTitleNetIncome.TabIndex = 1;
            lblTitleNetIncome.Text = "DOANH THU THUẦN";
            lblTitleNetIncome.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlNetIncomeAccent
            // 
            pnlNetIncomeAccent.BackColor = Color.SeaGreen;
            pnlNetIncomeAccent.Dock = DockStyle.Top;
            pnlNetIncomeAccent.Location = new Point(0, 0);
            pnlNetIncomeAccent.Name = "pnlNetIncomeAccent";
            pnlNetIncomeAccent.Size = new Size(362, 5);
            pnlNetIncomeAccent.TabIndex = 0;
            // 
            // pnlCardNetExpense
            // 
            pnlCardNetExpense.BackColor = Color.White;
            pnlCardNetExpense.BorderStyle = BorderStyle.FixedSingle;
            pnlCardNetExpense.Controls.Add(lblInputVAT);
            pnlCardNetExpense.Controls.Add(lblNetExpense);
            pnlCardNetExpense.Controls.Add(lblTitleNetExpense);
            pnlCardNetExpense.Controls.Add(pnlNetExpenseAccent);
            pnlCardNetExpense.Dock = DockStyle.Fill;
            pnlCardNetExpense.Location = new Point(388, 10);
            pnlCardNetExpense.Margin = new Padding(8, 10, 8, 10);
            pnlCardNetExpense.Name = "pnlCardNetExpense";
            pnlCardNetExpense.Size = new Size(364, 128);
            pnlCardNetExpense.TabIndex = 1;
            // 
            // lblInputVAT
            // 
            lblInputVAT.Dock = DockStyle.Bottom;
            lblInputVAT.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblInputVAT.ForeColor = Color.Gray;
            lblInputVAT.Location = new Point(0, 100);
            lblInputVAT.Name = "lblInputVAT";
            lblInputVAT.Padding = new Padding(16, 0, 16, 8);
            lblInputVAT.Size = new Size(362, 26);
            lblInputVAT.TabIndex = 3;
            lblInputVAT.Text = "+ Thuế GTGT đầu vào: 0 VNĐ";
            lblInputVAT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetExpense
            // 
            lblNetExpense.AutoSize = false;
            lblNetExpense.Dock = DockStyle.Fill;
            lblNetExpense.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetExpense.ForeColor = Color.IndianRed;
            lblNetExpense.Location = new Point(0, 35);
            lblNetExpense.Name = "lblNetExpense";
            lblNetExpense.Padding = new Padding(16, 0, 16, 0);
            lblNetExpense.Size = new Size(362, 91);
            lblNetExpense.TabIndex = 2;
            lblNetExpense.Text = "0 VNĐ";
            lblNetExpense.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTitleNetExpense
            // 
            lblTitleNetExpense.Dock = DockStyle.Top;
            lblTitleNetExpense.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitleNetExpense.ForeColor = Color.DimGray;
            lblTitleNetExpense.Location = new Point(0, 5);
            lblTitleNetExpense.Name = "lblTitleNetExpense";
            lblTitleNetExpense.Padding = new Padding(16, 8, 16, 0);
            lblTitleNetExpense.Size = new Size(362, 30);
            lblTitleNetExpense.TabIndex = 1;
            lblTitleNetExpense.Text = "CHI PHÍ THUẦN";
            lblTitleNetExpense.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlNetExpenseAccent
            // 
            pnlNetExpenseAccent.BackColor = Color.IndianRed;
            pnlNetExpenseAccent.Dock = DockStyle.Top;
            pnlNetExpenseAccent.Location = new Point(0, 0);
            pnlNetExpenseAccent.Name = "pnlNetExpenseAccent";
            pnlNetExpenseAccent.Size = new Size(362, 5);
            pnlNetExpenseAccent.TabIndex = 0;
            // 
            // pnlCardBusinessPerformance
            // 
            pnlCardBusinessPerformance.BackColor = Color.White;
            pnlCardBusinessPerformance.BorderStyle = BorderStyle.FixedSingle;
            pnlCardBusinessPerformance.Controls.Add(tlpBusinessMetrics);
            pnlCardBusinessPerformance.Controls.Add(lblTitleBusinessPerformance);
            pnlCardBusinessPerformance.Controls.Add(pnlBusinessPerformanceAccent);
            pnlCardBusinessPerformance.Dock = DockStyle.Fill;
            pnlCardBusinessPerformance.Location = new Point(768, 10);
            pnlCardBusinessPerformance.Margin = new Padding(8, 10, 8, 10);
            pnlCardBusinessPerformance.Name = "pnlCardBusinessPerformance";
            pnlCardBusinessPerformance.Size = new Size(364, 128);
            pnlCardBusinessPerformance.TabIndex = 2;
            // 
            // tlpBusinessMetrics
            // 
            tlpBusinessMetrics.ColumnCount = 2;
            tlpBusinessMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            tlpBusinessMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tlpBusinessMetrics.Controls.Add(lblEstimatedTax, 1, 1);
            tlpBusinessMetrics.Controls.Add(lblEstimatedTaxCaption, 0, 1);
            tlpBusinessMetrics.Controls.Add(lblNetProfit, 1, 0);
            tlpBusinessMetrics.Controls.Add(lblNetProfitCaption, 0, 0);
            tlpBusinessMetrics.Dock = DockStyle.Fill;
            tlpBusinessMetrics.Location = new Point(0, 35);
            tlpBusinessMetrics.Name = "tlpBusinessMetrics";
            tlpBusinessMetrics.Padding = new Padding(16, 6, 16, 10);
            tlpBusinessMetrics.RowCount = 2;
            tlpBusinessMetrics.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpBusinessMetrics.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpBusinessMetrics.Size = new Size(362, 91);
            tlpBusinessMetrics.TabIndex = 2;
            // 
            // lblEstimatedTax
            // 
            lblEstimatedTax.Dock = DockStyle.Fill;
            lblEstimatedTax.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstimatedTax.ForeColor = Color.SteelBlue;
            lblEstimatedTax.Location = new Point(209, 43);
            lblEstimatedTax.Name = "lblEstimatedTax";
            lblEstimatedTax.Size = new Size(134, 38);
            lblEstimatedTax.TabIndex = 3;
            lblEstimatedTax.Text = "0 VNĐ";
            lblEstimatedTax.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEstimatedTaxCaption
            // 
            lblEstimatedTaxCaption.Dock = DockStyle.Fill;
            lblEstimatedTaxCaption.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEstimatedTaxCaption.ForeColor = Color.DimGray;
            lblEstimatedTaxCaption.Location = new Point(19, 43);
            lblEstimatedTaxCaption.Name = "lblEstimatedTaxCaption";
            lblEstimatedTaxCaption.Size = new Size(184, 38);
            lblEstimatedTaxCaption.TabIndex = 2;
            lblEstimatedTaxCaption.Text = "Thuế GTGT Phải Nộp:";
            lblEstimatedTaxCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetProfit
            // 
            lblNetProfit.Dock = DockStyle.Fill;
            lblNetProfit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetProfit.ForeColor = Color.SteelBlue;
            lblNetProfit.Location = new Point(209, 6);
            lblNetProfit.Name = "lblNetProfit";
            lblNetProfit.Size = new Size(134, 37);
            lblNetProfit.TabIndex = 1;
            lblNetProfit.Text = "0 VNĐ";
            lblNetProfit.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetProfitCaption
            // 
            lblNetProfitCaption.Dock = DockStyle.Fill;
            lblNetProfitCaption.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNetProfitCaption.ForeColor = Color.DimGray;
            lblNetProfitCaption.Location = new Point(19, 6);
            lblNetProfitCaption.Name = "lblNetProfitCaption";
            lblNetProfitCaption.Size = new Size(184, 37);
            lblNetProfitCaption.TabIndex = 0;
            lblNetProfitCaption.Text = "Lợi Nhuận Gộp:";
            lblNetProfitCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTitleBusinessPerformance
            // 
            lblTitleBusinessPerformance.Dock = DockStyle.Top;
            lblTitleBusinessPerformance.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitleBusinessPerformance.ForeColor = Color.DimGray;
            lblTitleBusinessPerformance.Location = new Point(0, 5);
            lblTitleBusinessPerformance.Name = "lblTitleBusinessPerformance";
            lblTitleBusinessPerformance.Padding = new Padding(16, 8, 16, 0);
            lblTitleBusinessPerformance.Size = new Size(362, 30);
            lblTitleBusinessPerformance.TabIndex = 1;
            lblTitleBusinessPerformance.Text = "LỢI NHUẬN & THUẾ";
            lblTitleBusinessPerformance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlBusinessPerformanceAccent
            // 
            pnlBusinessPerformanceAccent.BackColor = Color.SteelBlue;
            pnlBusinessPerformanceAccent.Dock = DockStyle.Top;
            pnlBusinessPerformanceAccent.Location = new Point(0, 0);
            pnlBusinessPerformanceAccent.Name = "pnlBusinessPerformanceAccent";
            pnlBusinessPerformanceAccent.Size = new Size(362, 5);
            pnlBusinessPerformanceAccent.TabIndex = 0;
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
            pnlCardNetIncome.ResumeLayout(false);
            pnlCardNetExpense.ResumeLayout(false);
            pnlCardBusinessPerformance.ResumeLayout(false);
            tlpBusinessMetrics.ResumeLayout(false);
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
        private Panel pnlCardNetIncome;
        private Panel pnlNetIncomeAccent;
        private Label lblTitleNetIncome;
        private Label lblNetIncome;
        private Label lblOutputVAT;
        private Panel pnlCardNetExpense;
        private Panel pnlNetExpenseAccent;
        private Label lblTitleNetExpense;
        private Label lblNetExpense;
        private Label lblInputVAT;
        private Panel pnlCardBusinessPerformance;
        private Panel pnlBusinessPerformanceAccent;
        private Label lblTitleBusinessPerformance;
        private TableLayoutPanel tlpBusinessMetrics;
        private Label lblNetProfitCaption;
        private Label lblNetProfit;
        private Label lblEstimatedTaxCaption;
        private Label lblEstimatedTax;
        private Panel pnlCharts;
        private Panel pnlChartCard;
        private Label lblChartTitle;
        private Panel pnlExpenseHost;
        private Label lblChartSubtitle;
    }
}
