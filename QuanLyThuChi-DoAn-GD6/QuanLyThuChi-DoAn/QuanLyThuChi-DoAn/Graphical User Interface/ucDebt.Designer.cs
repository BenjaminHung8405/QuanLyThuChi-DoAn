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
            flowFilters = new FlowLayoutPanel();
            cboDebtType = new ComboBox();
            cboStatus = new ComboBox();
            txtSearch = new TextBox();
            btnFilter = new FontAwesome.Sharp.IconButton();
            btnPayDebt = new FontAwesome.Sharp.IconButton();
            pnlStats = new Panel();
            lblTotalReceivable = new Label();
            lblTotalPayable = new Label();
            tableLayout = new TableLayoutPanel();
            pnlGrid = new Panel();
            dgvDebts = new DataGridView();
            colDebtId = new DataGridViewTextBoxColumn();
            colPartner = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colPaid = new DataGridViewTextBoxColumn();
            colRemaining = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colRawDebtType = new DataGridViewTextBoxColumn();
            pnlTop.SuspendLayout();
            flowFilters.SuspendLayout();
            pnlStats.SuspendLayout();
            tableLayout.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDebts).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.WhiteSmoke;
            pnlTop.Controls.Add(flowFilters);
            pnlTop.Controls.Add(pnlStats);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(16, 16);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(12);
            pnlTop.Size = new Size(1136, 100);
            pnlTop.TabIndex = 1;
            // 
            // flowFilters
            // 
            flowFilters.AutoSize = true;
            flowFilters.Controls.Add(cboDebtType);
            flowFilters.Controls.Add(cboStatus);
            flowFilters.Controls.Add(txtSearch);
            flowFilters.Controls.Add(btnFilter);
            flowFilters.Controls.Add(btnPayDebt);
            flowFilters.Dock = DockStyle.Left;
            flowFilters.Location = new Point(12, 12);
            flowFilters.Margin = new Padding(0);
            flowFilters.Name = "flowFilters";
            flowFilters.Size = new Size(824, 76);
            flowFilters.TabIndex = 0;
            flowFilters.WrapContents = false;
            // 
            // cboDebtType
            // 
            cboDebtType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDebtType.Location = new Point(6, 6);
            cboDebtType.Margin = new Padding(6);
            cboDebtType.Name = "cboDebtType";
            cboDebtType.Size = new Size(160, 23);
            cboDebtType.TabIndex = 0;
            // 
            // cboStatus
            // 
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Location = new Point(178, 6);
            cboStatus.Margin = new Padding(6);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(160, 23);
            cboStatus.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(350, 6);
            txtSearch.Margin = new Padding(6);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo tên đối tác...";
            txtSearch.Size = new Size(220, 23);
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
            btnFilter.Location = new Point(582, 6);
            btnFilter.Margin = new Padding(6);
            btnFilter.Name = "btnFilter";
            btnFilter.Padding = new Padding(8, 0, 8, 0);
            btnFilter.Size = new Size(92, 36);
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
            btnPayDebt.Location = new Point(686, 6);
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
            // pnlStats
            // 
            pnlStats.Controls.Add(lblTotalReceivable);
            pnlStats.Controls.Add(lblTotalPayable);
            pnlStats.Dock = DockStyle.Right;
            pnlStats.Location = new Point(839, 12);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(6);
            pnlStats.Size = new Size(285, 76);
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
            tableLayout.Location = new Point(16, 116);
            tableLayout.Name = "tableLayout";
            tableLayout.RowCount = 1;
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayout.Size = new Size(1136, 505);
            tableLayout.TabIndex = 0;
            // 
            // pnlGrid
            // 
            pnlGrid.Controls.Add(dgvDebts);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(3, 3);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(12);
            pnlGrid.Size = new Size(1130, 499);
            pnlGrid.TabIndex = 0;
            // 
            // dgvDebts
            // 
            dgvDebts.AllowUserToAddRows = false;
            dgvDebts.AllowUserToDeleteRows = false;
            dgvDebts.AutoGenerateColumns = false;
            dgvDebts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDebts.BackgroundColor = Color.White;
            dgvDebts.BorderStyle = BorderStyle.None;
            dgvDebts.Columns.AddRange(new DataGridViewColumn[] { colDebtId, colPartner, colType, colTotal, colPaid, colRemaining, colStatus, colRawDebtType });
            dgvDebts.Dock = DockStyle.Fill;
            dgvDebts.Location = new Point(12, 12);
            dgvDebts.Name = "dgvDebts";
            dgvDebts.MultiSelect = false;
            dgvDebts.ReadOnly = true;
            dgvDebts.RowHeadersVisible = false;
            dgvDebts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDebts.Size = new Size(1106, 475);
            dgvDebts.TabIndex = 0;
            dgvDebts.CellContentClick += dgvDebts_CellContentClick;
            dgvDebts.SelectionChanged += dgvDebts_SelectionChanged;
            // 
            // colDebtId
            // 
            colDebtId.DataPropertyName = "DebtId";
            colDebtId.HeaderText = "DebtId";
            colDebtId.Name = "colDebtId";
            colDebtId.ReadOnly = true;
            colDebtId.Visible = false;
            // 
            // colPartner
            // 
            colPartner.DataPropertyName = "PartnerName";
            colPartner.HeaderText = "Đối tác";
            colPartner.Name = "colPartner";
            colPartner.ReadOnly = true;
            // 
            // colType
            // 
            colType.DataPropertyName = "DebtType";
            colType.HeaderText = "Loại nợ";
            colType.Name = "colType";
            colType.ReadOnly = true;
            // 
            // colTotal
            // 
            colTotal.DataPropertyName = "TotalAmount";
            colTotal.HeaderText = "Tổng nợ";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            // 
            // colPaid
            // 
            colPaid.DataPropertyName = "PaidAmount";
            colPaid.HeaderText = "Đã trả";
            colPaid.Name = "colPaid";
            colPaid.ReadOnly = true;
            // 
            // colRemaining
            // 
            colRemaining.DataPropertyName = "Remaining";
            colRemaining.HeaderText = "Còn nợ";
            colRemaining.Name = "colRemaining";
            colRemaining.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Trạng thái";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // colRawDebtType
            // 
            colRawDebtType.DataPropertyName = "RawDebtType";
            colRawDebtType.HeaderText = "RawDebtType";
            colRawDebtType.Name = "colRawDebtType";
            colRawDebtType.ReadOnly = true;
            colRawDebtType.Visible = false;
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
            pnlTop.PerformLayout();
            flowFilters.ResumeLayout(false);
            flowFilters.PerformLayout();
            pnlStats.ResumeLayout(false);
            tableLayout.ResumeLayout(false);
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDebts).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlTop;
        private FlowLayoutPanel flowFilters;
        private ComboBox cboDebtType;
        private ComboBox cboStatus;
        private TextBox txtSearch;
        private FontAwesome.Sharp.IconButton btnFilter;
        private FontAwesome.Sharp.IconButton btnPayDebt;
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
    }
}
