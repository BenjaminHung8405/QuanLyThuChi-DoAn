namespace QuanLyThuChi_DoAn
{
    partial class ucCashFund
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.DataGridView dgvCashFunds;

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
            topPanel = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            btnRefresh = new FontAwesome.Sharp.IconButton();
            lblTitle = new Label();
            panel2 = new Panel();
            btnSyncSelected = new FontAwesome.Sharp.IconButton();
            lblTotalBalance = new Label();
            lblTotalBalanceLabel = new Label();
            dgvCashFunds = new DataGridView();
            colCheck = new DataGridViewCheckBoxColumn();
            colFundId = new DataGridViewTextBoxColumn();
            colFundName = new DataGridViewTextBoxColumn();
            colBranchName = new DataGridViewTextBoxColumn();
            colSysBalance = new DataGridViewTextBoxColumn();
            colActualBalance = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            pnlInput = new Panel();
            pnlButtons = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnDelete = new FontAwesome.Sharp.IconButton();
            btnSave = new FontAwesome.Sharp.IconButton();
            btnCancel = new FontAwesome.Sharp.IconButton();
            btnNew = new FontAwesome.Sharp.IconButton();
            gbCashFundDetail = new GroupBox();
            txtBalance = new NumericUpDown();
            label3 = new Label();
            txtAccountNumber = new TextBox();
            label2 = new Label();
            cboBranch = new ComboBox();
            lblBranch = new Label();
            txtFundName = new TextBox();
            label1 = new Label();
            topPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCashFunds).BeginInit();
            pnlInput.SuspendLayout();
            pnlButtons.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            gbCashFundDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtBalance).BeginInit();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.White;
            topPanel.Controls.Add(tableLayoutPanel1);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(16, 16);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1136, 100);
            topPanel.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1136, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1130, 44);
            panel1.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            btnRefresh.IconColor = Color.FromArgb(46, 125, 50);
            btnRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRefresh.IconSize = 20;
            btnRefresh.Location = new Point(1068, 0);
            btnRefresh.Margin = new Padding(2, 0, 4, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(62, 44);
            btnRefresh.TabIndex = 8;
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(217, 25);
            lblTitle.TabIndex = 7;
            lblTitle.Text = "DANH SÁCH QUỸ TIỀN";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnSyncSelected);
            panel2.Controls.Add(lblTotalBalance);
            panel2.Controls.Add(lblTotalBalanceLabel);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(1130, 44);
            panel2.TabIndex = 1;
            // 
            // btnSyncSelected
            // 
            btnSyncSelected.Cursor = Cursors.Hand;
            btnSyncSelected.Dock = DockStyle.Right;
            btnSyncSelected.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSyncSelected.IconChar = FontAwesome.Sharp.IconChar.SyncAlt;
            btnSyncSelected.IconColor = Color.FromArgb(46, 125, 50);
            btnSyncSelected.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnSyncSelected.IconSize = 20;
            btnSyncSelected.ImageAlign = ContentAlignment.MiddleLeft;
            btnSyncSelected.Location = new Point(941, 0);
            btnSyncSelected.Margin = new Padding(2, 0, 4, 0);
            btnSyncSelected.Name = "btnSyncSelected";
            btnSyncSelected.Padding = new Padding(8, 0, 0, 0);
            btnSyncSelected.Size = new Size(189, 44);
            btnSyncSelected.TabIndex = 11;
            btnSyncSelected.Text = "Đồng bộ quỹ đã chọn";
            btnSyncSelected.UseVisualStyleBackColor = true;
            btnSyncSelected.Click += btnSyncSelected_Click;
            // 
            // lblTotalBalance
            // 
            lblTotalBalance.AutoSize = true;
            lblTotalBalance.Dock = DockStyle.Left;
            lblTotalBalance.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalBalance.ForeColor = Color.FromArgb(51, 51, 51);
            lblTotalBalance.Location = new Point(112, 0);
            lblTotalBalance.Name = "lblTotalBalance";
            lblTotalBalance.Size = new Size(23, 25);
            lblTotalBalance.TabIndex = 9;
            lblTotalBalance.Text = "0";
            // 
            // lblTotalBalanceLabel
            // 
            lblTotalBalanceLabel.AutoSize = true;
            lblTotalBalanceLabel.Dock = DockStyle.Left;
            lblTotalBalanceLabel.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalBalanceLabel.ForeColor = Color.FromArgb(51, 51, 51);
            lblTotalBalanceLabel.Location = new Point(0, 0);
            lblTotalBalanceLabel.Name = "lblTotalBalanceLabel";
            lblTotalBalanceLabel.Size = new Size(112, 25);
            lblTotalBalanceLabel.TabIndex = 7;
            lblTotalBalanceLabel.Text = "Tổng số dư:";
            // 
            // dgvCashFunds
            // 
            dgvCashFunds.AllowUserToAddRows = false;
            dgvCashFunds.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 248, 255);
            dgvCashFunds.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCashFunds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCashFunds.BackgroundColor = Color.White;
            dgvCashFunds.Columns.AddRange(new DataGridViewColumn[] { colCheck, colFundId, colFundName, colBranchName, colSysBalance, colActualBalance, colStatus });
            dgvCashFunds.Dock = DockStyle.Left;
            dgvCashFunds.Location = new Point(16, 116);
            dgvCashFunds.Margin = new Padding(0);
            dgvCashFunds.MultiSelect = false;
            dgvCashFunds.Name = "dgvCashFunds";
            dgvCashFunds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCashFunds.Size = new Size(779, 505);
            dgvCashFunds.TabIndex = 0;
            dgvCashFunds.CellClick += dgvCashFunds_CellClick;
            dgvCashFunds.CellFormatting += dgvCashFunds_CellFormatting;
            // 
            // colCheck
            // 
            colCheck.HeaderText = "Chọn";
            colCheck.Name = "colCheck";
            // 
            // colFundId
            // 
            colFundId.HeaderText = "ID";
            colFundId.Name = "colFundId";
            colFundId.ReadOnly = true;
            colFundId.Visible = false;
            // 
            // colFundName
            // 
            colFundName.HeaderText = "Tên Quỹ";
            colFundName.Name = "colFundName";
            colFundName.ReadOnly = true;
            // 
            // colBranchName
            // 
            colBranchName.HeaderText = "Chi nhánh";
            colBranchName.Name = "colBranchName";
            colBranchName.ReadOnly = true;
            // 
            // colSysBalance
            // 
            colSysBalance.HeaderText = "Dư trên sổ";
            colSysBalance.Name = "colSysBalance";
            colSysBalance.ReadOnly = true;
            // 
            // colActualBalance
            // 
            colActualBalance.HeaderText = "Dư thực tế";
            colActualBalance.Name = "colActualBalance";
            colActualBalance.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Trạng thái";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // pnlInput
            // 
            pnlInput.AutoScroll = true;
            pnlInput.Controls.Add(pnlButtons);
            pnlInput.Controls.Add(gbCashFundDetail);
            pnlInput.Dock = DockStyle.Right;
            pnlInput.Location = new Point(795, 116);
            pnlInput.Margin = new Padding(0);
            pnlInput.Name = "pnlInput";
            pnlInput.Padding = new Padding(16);
            pnlInput.Size = new Size(357, 505);
            pnlInput.TabIndex = 4;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(tableLayoutPanel3);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(16, 390);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(325, 99);
            pnlButtons.TabIndex = 15;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(btnDelete, 1, 1);
            tableLayoutPanel3.Controls.Add(btnSave, 0, 1);
            tableLayoutPanel3.Controls.Add(btnCancel, 1, 0);
            tableLayoutPanel3.Controls.Add(btnNew, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Bottom;
            tableLayoutPanel3.Location = new Point(0, -1);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(325, 100);
            tableLayoutPanel3.TabIndex = 11;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(198, 40, 40);
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.White;
            btnDelete.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            btnDelete.IconColor = Color.White;
            btnDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnDelete.IconSize = 32;
            btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
            btnDelete.Location = new Point(165, 53);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(8, 0, 0, 0);
            btnDelete.Size = new Size(157, 44);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(46, 125, 50);
            btnSave.Dock = DockStyle.Fill;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.IconChar = FontAwesome.Sharp.IconChar.Save;
            btnSave.IconColor = Color.White;
            btnSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnSave.IconSize = 32;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(3, 53);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(8, 0, 0, 0);
            btnSave.Size = new Size(156, 44);
            btnSave.TabIndex = 2;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.IconChar = FontAwesome.Sharp.IconChar.None;
            btnCancel.IconColor = Color.Black;
            btnCancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCancel.IconSize = 32;
            btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancel.Location = new Point(165, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(8, 0, 0, 0);
            btnCancel.Size = new Size(157, 44);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Bỏ qua";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnNew
            // 
            btnNew.Dock = DockStyle.Fill;
            btnNew.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNew.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            btnNew.IconColor = Color.Black;
            btnNew.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnNew.IconSize = 32;
            btnNew.ImageAlign = ContentAlignment.MiddleLeft;
            btnNew.Location = new Point(3, 3);
            btnNew.Name = "btnNew";
            btnNew.Padding = new Padding(8, 0, 0, 0);
            btnNew.Size = new Size(156, 44);
            btnNew.TabIndex = 0;
            btnNew.Text = "Thêm mới";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // gbCashFundDetail
            // 
            gbCashFundDetail.Controls.Add(txtBalance);
            gbCashFundDetail.Controls.Add(label3);
            gbCashFundDetail.Controls.Add(txtAccountNumber);
            gbCashFundDetail.Controls.Add(label2);
            gbCashFundDetail.Controls.Add(cboBranch);
            gbCashFundDetail.Controls.Add(lblBranch);
            gbCashFundDetail.Controls.Add(txtFundName);
            gbCashFundDetail.Controls.Add(label1);
            gbCashFundDetail.Dock = DockStyle.Top;
            gbCashFundDetail.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbCashFundDetail.Location = new Point(16, 16);
            gbCashFundDetail.Name = "gbCashFundDetail";
            gbCashFundDetail.Padding = new Padding(16);
            gbCashFundDetail.Size = new Size(325, 362);
            gbCashFundDetail.TabIndex = 2;
            gbCashFundDetail.TabStop = false;
            gbCashFundDetail.Text = "Thông tin Quỹ Tiền";
            // 
            // txtBalance
            // 
            txtBalance.BorderStyle = BorderStyle.FixedSingle;
            txtBalance.Dock = DockStyle.Top;
            txtBalance.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            txtBalance.Location = new Point(16, 257);
            txtBalance.Maximum = decimal.MaxValue;
            txtBalance.Name = "txtBalance";
            txtBalance.Size = new Size(293, 29);
            txtBalance.TabIndex = 6;
            txtBalance.TextAlign = HorizontalAlignment.Right;
            txtBalance.ThousandsSeparator = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(16, 220);
            label3.Name = "label3";
            label3.Padding = new Padding(0, 8, 0, 8);
            label3.Size = new Size(53, 37);
            label3.TabIndex = 5;
            label3.Text = "Số dư";
            // 
            // txtAccountNumber
            // 
            txtAccountNumber.BorderStyle = BorderStyle.FixedSingle;
            txtAccountNumber.Dock = DockStyle.Top;
            txtAccountNumber.Location = new Point(16, 191);
            txtAccountNumber.Name = "txtAccountNumber";
            txtAccountNumber.Size = new Size(293, 29);
            txtAccountNumber.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(16, 154);
            label2.Name = "label2";
            label2.Padding = new Padding(0, 8, 0, 8);
            label2.Size = new Size(99, 37);
            label2.TabIndex = 3;
            label2.Text = "Số tài khoản";
            // 
            // cboBranch
            // 
            cboBranch.Dock = DockStyle.Top;
            cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBranch.Location = new Point(16, 125);
            cboBranch.Name = "cboBranch";
            cboBranch.Size = new Size(293, 29);
            cboBranch.TabIndex = 3;
            // 
            // lblBranch
            // 
            lblBranch.AutoSize = true;
            lblBranch.Dock = DockStyle.Top;
            lblBranch.Location = new Point(16, 96);
            lblBranch.Name = "lblBranch";
            lblBranch.Padding = new Padding(0, 0, 0, 8);
            lblBranch.Size = new Size(81, 29);
            lblBranch.TabIndex = 2;
            lblBranch.Text = "Chi nhánh";
            // 
            // txtFundName
            // 
            txtFundName.BorderStyle = BorderStyle.FixedSingle;
            txtFundName.Dock = DockStyle.Top;
            txtFundName.Location = new Point(16, 67);
            txtFundName.Name = "txtFundName";
            txtFundName.Size = new Size(293, 29);
            txtFundName.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(16, 38);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 0, 0, 8);
            label1.Size = new Size(66, 29);
            label1.TabIndex = 1;
            label1.Text = "Tên quỹ";
            // 
            // ucCashFund
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlInput);
            Controls.Add(dgvCashFunds);
            Controls.Add(topPanel);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucCashFund";
            Padding = new Padding(16);
            Size = new Size(1168, 637);
            topPanel.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCashFunds).EndInit();
            pnlInput.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            gbCashFundDetail.ResumeLayout(false);
            gbCashFundDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtBalance).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label lblTitle;
        private Panel panel2;
        private Label lblTotalBalance;
        private Label lblTotalBalanceLabel;
        private FontAwesome.Sharp.IconButton btnRefresh;
        private Panel pnlInput;
        private Panel pnlButtons;
        private TableLayoutPanel tableLayoutPanel3;
        private FontAwesome.Sharp.IconButton btnDelete;
        private FontAwesome.Sharp.IconButton btnSave;
        private FontAwesome.Sharp.IconButton btnCancel;
        private FontAwesome.Sharp.IconButton btnNew;
        private GroupBox gbCashFundDetail;
        private NumericUpDown txtBalance;
        private Label label3;
        private TextBox txtAccountNumber;
        private Label label2;
        private ComboBox cboBranch;
        private Label lblBranch;
        private TextBox txtFundName;
        private Label label1;
        private DataGridViewCheckBoxColumn colCheck;
        private DataGridViewTextBoxColumn colFundId;
        private DataGridViewTextBoxColumn colFundName;
        private DataGridViewTextBoxColumn colBranchName;
        private DataGridViewTextBoxColumn colSysBalance;
        private DataGridViewTextBoxColumn colActualBalance;
        private DataGridViewTextBoxColumn colStatus;
        private FontAwesome.Sharp.IconButton btnSyncSelected;
    }
}