namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class ucBranchManagement
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
            pnlHeader = new Panel();
            tlpHeader = new TableLayoutPanel();
            lblTitle = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnAddBranch = new Button();
            btnEditBranch = new Button();
            btnToggleStatus = new Button();
            pnlMain = new Panel();
            dgvBranches = new DataGridView();
            pnlHeader.SuspendLayout();
            tlpHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBranches).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.WhiteSmoke;
            pnlHeader.Controls.Add(tlpHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(10);
            pnlHeader.Size = new Size(1168, 80);
            pnlHeader.TabIndex = 0;
            // 
            // tlpHeader
            // 
            tlpHeader.ColumnCount = 6;
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.Controls.Add(lblTitle, 0, 0);
            tlpHeader.Controls.Add(txtSearch, 1, 0);
            tlpHeader.Controls.Add(btnSearch, 2, 0);
            tlpHeader.Controls.Add(btnAddBranch, 3, 0);
            tlpHeader.Controls.Add(btnEditBranch, 4, 0);
            tlpHeader.Controls.Add(btnToggleStatus, 5, 0);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(10, 10);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.RowCount = 1;
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpHeader.Size = new Size(1148, 60);
            tlpHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(12, 51, 82);
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(294, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ CHI NHÁNH";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Location = new Point(303, 18);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Nhập tên chi nhánh...";
            txtSearch.Size = new Size(282, 23);
            txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(33, 150, 243);
            btnSearch.Dock = DockStyle.Fill;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(591, 12);
            btnSearch.Margin = new Padding(3, 12, 3, 12);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(114, 36);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "🔍 Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnAddBranch
            // 
            btnAddBranch.BackColor = Color.SeaGreen;
            btnAddBranch.Dock = DockStyle.Fill;
            btnAddBranch.FlatAppearance.BorderSize = 0;
            btnAddBranch.FlatStyle = FlatStyle.Flat;
            btnAddBranch.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddBranch.ForeColor = Color.White;
            btnAddBranch.Location = new Point(711, 12);
            btnAddBranch.Margin = new Padding(3, 12, 3, 12);
            btnAddBranch.Name = "btnAddBranch";
            btnAddBranch.Size = new Size(144, 36);
            btnAddBranch.TabIndex = 3;
            btnAddBranch.Text = "➕ Thêm Mới";
            btnAddBranch.UseVisualStyleBackColor = false;
            // 
            // btnEditBranch
            // 
            btnEditBranch.BackColor = Color.SteelBlue;
            btnEditBranch.Dock = DockStyle.Fill;
            btnEditBranch.FlatAppearance.BorderSize = 0;
            btnEditBranch.FlatStyle = FlatStyle.Flat;
            btnEditBranch.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditBranch.ForeColor = Color.White;
            btnEditBranch.Location = new Point(861, 12);
            btnEditBranch.Margin = new Padding(3, 12, 3, 12);
            btnEditBranch.Name = "btnEditBranch";
            btnEditBranch.Size = new Size(134, 36);
            btnEditBranch.TabIndex = 4;
            btnEditBranch.Text = "✏️ Cập Nhật";
            btnEditBranch.UseVisualStyleBackColor = false;
            // 
            // btnToggleStatus
            // 
            btnToggleStatus.BackColor = Color.IndianRed;
            btnToggleStatus.Dock = DockStyle.Fill;
            btnToggleStatus.FlatAppearance.BorderSize = 0;
            btnToggleStatus.FlatStyle = FlatStyle.Flat;
            btnToggleStatus.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnToggleStatus.ForeColor = Color.White;
            btnToggleStatus.Location = new Point(1001, 12);
            btnToggleStatus.Margin = new Padding(3, 12, 3, 12);
            btnToggleStatus.Name = "btnToggleStatus";
            btnToggleStatus.Size = new Size(144, 36);
            btnToggleStatus.TabIndex = 5;
            btnToggleStatus.Text = "🔒 Khóa / Mở";
            btnToggleStatus.UseVisualStyleBackColor = false;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(dgvBranches);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 80);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(10);
            pnlMain.Size = new Size(1168, 617);
            pnlMain.TabIndex = 1;
            // 
            // dgvBranches
            // 
            dgvBranches.AllowUserToAddRows = false;
            dgvBranches.AllowUserToDeleteRows = false;
            dgvBranches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBranches.BackgroundColor = Color.White;
            dgvBranches.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 239, 241);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvBranches.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvBranches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvBranches.DefaultCellStyle = dataGridViewCellStyle2;
            dgvBranches.Dock = DockStyle.Fill;
            dgvBranches.EnableHeadersVisualStyles = false;
            dgvBranches.Location = new Point(10, 10);
            dgvBranches.MultiSelect = false;
            dgvBranches.Name = "dgvBranches";
            dgvBranches.ReadOnly = true;
            dgvBranches.RowHeadersWidth = 51;
            dgvBranches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBranches.Size = new Size(1148, 597);
            dgvBranches.TabIndex = 0;
            dgvBranches.CellFormatting += dgvBranches_CellFormatting;
            dgvBranches.DataBindingComplete += dgvBranches_DataBindingComplete;
            // 
            // ucBranchManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucBranchManagement";
            Size = new Size(1168, 697);
            Load += ucBranchManagement_Load;
            pnlHeader.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            tlpHeader.PerformLayout();
            pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBranches).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private TableLayoutPanel tlpHeader;
        private Label lblTitle;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnAddBranch;
        private Button btnEditBranch;
        private Button btnToggleStatus;
        private Panel pnlMain;
        private DataGridView dgvBranches;
    }
}
