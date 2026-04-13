namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class ucDebt
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
            pnlTop = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            flowFilters = new FlowLayoutPanel();
            cboDebtType = new ComboBox();
            cboStatus = new ComboBox();
            txtSearch = new TextBox();
            btnFilter = new FontAwesome.Sharp.IconButton();
            btnPayDebt = new FontAwesome.Sharp.IconButton();
            chkToggleView = new CheckBox();
            panel1 = new Panel();
            btnResetView = new FontAwesome.Sharp.IconButton();
            btnApproveDebt = new FontAwesome.Sharp.IconButton();
            btnAddNewDebt = new FontAwesome.Sharp.IconButton();
            pnlStats = new Panel();
            lblTotalReceivable = new Label();
            lblTotalPayable = new Label();
            tableLayout = new TableLayoutPanel();
            pnlGrid = new Panel();
            dgvDebts = new DataGridView();
            pnlTop.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowFilters.SuspendLayout();
            panel1.SuspendLayout();
            pnlStats.SuspendLayout();
            tableLayout.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDebts).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.WhiteSmoke;
            pnlTop.Controls.Add(tableLayoutPanel1);
            pnlTop.Controls.Add(pnlStats);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(16, 16);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(12);
            pnlTop.Size = new Size(1136, 121);
            pnlTop.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(flowFilters, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 53.4090919F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 46.5909081F));
            tableLayoutPanel1.Size = new Size(827, 97);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // flowFilters
            // 
            flowFilters.AutoSize = true;
            flowFilters.Controls.Add(cboDebtType);
            flowFilters.Controls.Add(cboStatus);
            flowFilters.Controls.Add(txtSearch);
            flowFilters.Controls.Add(btnFilter);
            flowFilters.Controls.Add(btnPayDebt);
            flowFilters.Controls.Add(chkToggleView);
            flowFilters.Dock = DockStyle.Fill;
            flowFilters.Location = new Point(0, 0);
            flowFilters.Margin = new Padding(0);
            flowFilters.Name = "flowFilters";
            flowFilters.Size = new Size(827, 51);
            flowFilters.TabIndex = 1;
            flowFilters.WrapContents = false;
            // 
            // cboDebtType
            // 
            cboDebtType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDebtType.Location = new Point(6, 6);
            cboDebtType.Margin = new Padding(6);
            cboDebtType.Name = "cboDebtType";
            cboDebtType.Size = new Size(140, 23);
            cboDebtType.TabIndex = 0;
            // 
            // cboStatus
            // 
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Location = new Point(158, 6);
            cboStatus.Margin = new Padding(6);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(140, 23);
            cboStatus.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(310, 6);
            txtSearch.Margin = new Padding(6);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo tên đối tác...";
            txtSearch.Size = new Size(130, 23);
            txtSearch.TabIndex = 2;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = Color.FromArgb(33, 150, 243);
            btnFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFilter.ForeColor = Color.White;
            btnFilter.IconChar = FontAwesome.Sharp.IconChar.Filter;
            btnFilter.IconColor = Color.White;
            btnFilter.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnFilter.IconSize = 24;
            btnFilter.ImageAlign = ContentAlignment.MiddleLeft;
            btnFilter.Location = new Point(452, 6);
            btnFilter.Margin = new Padding(6);
            btnFilter.Name = "btnFilter";
            btnFilter.Padding = new Padding(8, 0, 8, 0);
            btnFilter.Size = new Size(88, 36);
            btnFilter.TabIndex = 3;
            btnFilter.Text = "Lọc";
            btnFilter.UseVisualStyleBackColor = false;
            // 
            // btnPayDebt
            // 
            btnPayDebt.BackColor = Color.FromArgb(76, 175, 80);
            btnPayDebt.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPayDebt.ForeColor = Color.White;
            btnPayDebt.IconChar = FontAwesome.Sharp.IconChar.MoneyCheckDollar;
            btnPayDebt.IconColor = Color.White;
            btnPayDebt.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnPayDebt.IconSize = 24;
            btnPayDebt.ImageAlign = ContentAlignment.MiddleLeft;
            btnPayDebt.Location = new Point(552, 6);
            btnPayDebt.Margin = new Padding(6);
            btnPayDebt.Name = "btnPayDebt";
            btnPayDebt.Padding = new Padding(8, 0, 8, 0);
            btnPayDebt.Size = new Size(132, 36);
            btnPayDebt.TabIndex = 4;
            btnPayDebt.Text = "Thanh toán";
            btnPayDebt.TextAlign = ContentAlignment.MiddleRight;
            btnPayDebt.UseVisualStyleBackColor = false;
            btnPayDebt.Click += btnPayDebt_Click;
            // 
            // chkToggleView
            // 
            chkToggleView.Appearance = Appearance.Button;
            chkToggleView.BackColor = Color.White;
            chkToggleView.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            chkToggleView.FlatStyle = FlatStyle.Flat;
            chkToggleView.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkToggleView.ForeColor = Color.FromArgb(33, 33, 33);
            chkToggleView.Location = new Point(696, 6);
            chkToggleView.Margin = new Padding(6);
            chkToggleView.Name = "chkToggleView";
            chkToggleView.Size = new Size(125, 36);
            chkToggleView.TabIndex = 5;
            chkToggleView.Text = "Góc nhìn: Từng phiếu";
            chkToggleView.TextAlign = ContentAlignment.MiddleCenter;
            chkToggleView.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnResetView);
            panel1.Controls.Add(btnApproveDebt);
            panel1.Controls.Add(btnAddNewDebt);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 54);
            panel1.Name = "panel1";
            panel1.Size = new Size(821, 40);
            panel1.TabIndex = 2;
            // 
            // btnResetView
            // 
            btnResetView.BackColor = Color.FromArgb(96, 125, 139);
            btnResetView.Dock = DockStyle.Left;
            btnResetView.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResetView.ForeColor = Color.White;
            btnResetView.IconChar = FontAwesome.Sharp.IconChar.ArrowRotateLeft;
            btnResetView.IconColor = Color.White;
            btnResetView.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnResetView.IconSize = 20;
            btnResetView.ImageAlign = ContentAlignment.MiddleLeft;
            btnResetView.Location = new Point(286, 0);
            btnResetView.Margin = new Padding(6);
            btnResetView.Name = "btnResetView";
            btnResetView.Padding = new Padding(8, 0, 8, 0);
            btnResetView.Size = new Size(136, 40);
            btnResetView.TabIndex = 8;
            btnResetView.Text = "Xem tất cả";
            btnResetView.TextAlign = ContentAlignment.MiddleRight;
            btnResetView.UseVisualStyleBackColor = false;
            btnResetView.Visible = false;
            // 
            // btnApproveDebt
            // 
            btnApproveDebt.BackColor = Color.FromArgb(255, 152, 0);
            btnApproveDebt.Dock = DockStyle.Left;
            btnApproveDebt.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnApproveDebt.ForeColor = Color.White;
            btnApproveDebt.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            btnApproveDebt.IconColor = Color.White;
            btnApproveDebt.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnApproveDebt.IconSize = 24;
            btnApproveDebt.ImageAlign = ContentAlignment.MiddleLeft;
            btnApproveDebt.Location = new Point(160, 0);
            btnApproveDebt.Margin = new Padding(6);
            btnApproveDebt.Name = "btnApproveDebt";
            btnApproveDebt.Padding = new Padding(8, 0, 8, 0);
            btnApproveDebt.Size = new Size(126, 40);
            btnApproveDebt.TabIndex = 7;
            btnApproveDebt.Text = "✓ Duyệt nợ";
            btnApproveDebt.TextAlign = ContentAlignment.MiddleRight;
            btnApproveDebt.UseVisualStyleBackColor = false;
            // 
            // btnAddNewDebt
            // 
            btnAddNewDebt.BackColor = Color.FromArgb(76, 175, 80);
            btnAddNewDebt.Dock = DockStyle.Left;
            btnAddNewDebt.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddNewDebt.ForeColor = Color.White;
            btnAddNewDebt.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            btnAddNewDebt.IconColor = Color.White;
            btnAddNewDebt.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnAddNewDebt.IconSize = 24;
            btnAddNewDebt.ImageAlign = ContentAlignment.MiddleLeft;
            btnAddNewDebt.Location = new Point(0, 0);
            btnAddNewDebt.Margin = new Padding(6);
            btnAddNewDebt.Name = "btnAddNewDebt";
            btnAddNewDebt.Padding = new Padding(8, 0, 8, 0);
            btnAddNewDebt.Size = new Size(160, 40);
            btnAddNewDebt.TabIndex = 6;
            btnAddNewDebt.Text = "Thêm công nợ";
            btnAddNewDebt.TextAlign = ContentAlignment.MiddleRight;
            btnAddNewDebt.UseVisualStyleBackColor = false;
            btnAddNewDebt.Click += btnAddNewDebt_Click;
            // 
            // pnlStats
            // 
            pnlStats.Controls.Add(lblTotalReceivable);
            pnlStats.Controls.Add(lblTotalPayable);
            pnlStats.Dock = DockStyle.Right;
            pnlStats.Location = new Point(839, 12);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(6);
            pnlStats.Size = new Size(285, 97);
            pnlStats.TabIndex = 1;
            // 
            // lblTotalReceivable
            // 
            lblTotalReceivable.Dock = DockStyle.Top;
            lblTotalReceivable.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalReceivable.ForeColor = Color.FromArgb(25, 118, 210);
            lblTotalReceivable.Location = new Point(6, 42);
            lblTotalReceivable.Name = "lblTotalReceivable";
            lblTotalReceivable.Size = new Size(273, 36);
            lblTotalReceivable.TabIndex = 0;
            lblTotalReceivable.Text = "Khách nợ: 0";
            lblTotalReceivable.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalPayable
            // 
            lblTotalPayable.Dock = DockStyle.Top;
            lblTotalPayable.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalPayable.ForeColor = Color.FromArgb(211, 47, 47);
            lblTotalPayable.Location = new Point(6, 6);
            lblTotalPayable.Name = "lblTotalPayable";
            lblTotalPayable.Size = new Size(273, 36);
            lblTotalPayable.TabIndex = 1;
            lblTotalPayable.Text = "Nợ NCC: 0";
            lblTotalPayable.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayout
            // 
            tableLayout.ColumnCount = 1;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayout.Controls.Add(pnlGrid, 0, 0);
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.Location = new Point(16, 137);
            tableLayout.Name = "tableLayout";
            tableLayout.RowCount = 1;
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayout.Size = new Size(1136, 484);
            tableLayout.TabIndex = 0;
            // 
            // pnlGrid
            // 
            pnlGrid.Controls.Add(dgvDebts);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(3, 3);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(12);
            pnlGrid.Size = new Size(1130, 478);
            pnlGrid.TabIndex = 0;
            // 
            // dgvDebts
            // 
            dgvDebts.AllowUserToAddRows = false;
            dgvDebts.AllowUserToDeleteRows = false;
            dgvDebts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDebts.BackgroundColor = Color.White;
            dgvDebts.BorderStyle = BorderStyle.None;
            dgvDebts.Dock = DockStyle.Fill;
            dgvDebts.Location = new Point(12, 12);
            dgvDebts.MultiSelect = false;
            dgvDebts.Name = "dgvDebts";
            dgvDebts.ReadOnly = true;
            dgvDebts.RowHeadersVisible = false;
            dgvDebts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDebts.Size = new Size(1106, 454);
            dgvDebts.TabIndex = 0;
            dgvDebts.CellContentClick += dgvDebts_CellContentClick;
            dgvDebts.SelectionChanged += dgvDebts_SelectionChanged;
            // 
            // ucDebt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayout);
            Controls.Add(pnlTop);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucDebt";
            Padding = new Padding(16);
            Size = new Size(1168, 637);
            pnlTop.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowFilters.ResumeLayout(false);
            flowFilters.PerformLayout();
            panel1.ResumeLayout(false);
            pnlStats.ResumeLayout(false);
            tableLayout.ResumeLayout(false);
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDebts).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlTop;
        private Panel pnlStats;
        private Label lblTotalReceivable;
        private Label lblTotalPayable;
        private TableLayoutPanel tableLayout;
        private Panel pnlGrid;
        private DataGridView dgvDebts;
        private DataGridViewTextBoxColumn colDebtId;
        private DataGridViewTextBoxColumn colPartner;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colPaid;
        private DataGridViewTextBoxColumn colRemaining;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colRawDebtType;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowFilters;
        private ComboBox cboDebtType;
        private ComboBox cboStatus;
        private TextBox txtSearch;
        private FontAwesome.Sharp.IconButton btnFilter;
        private FontAwesome.Sharp.IconButton btnPayDebt;
        private CheckBox chkToggleView;
        private Panel panel1;
        private FontAwesome.Sharp.IconButton btnResetView;
        private FontAwesome.Sharp.IconButton btnApproveDebt;
        private FontAwesome.Sharp.IconButton btnAddNewDebt;
    }
}
