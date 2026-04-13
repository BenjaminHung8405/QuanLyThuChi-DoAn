namespace QuanLyThuChi_DoAn
{
    partial class ucTransactionCategory
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
            label1 = new Label();
            btnRefresh = new FontAwesome.Sharp.IconButton();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            dgvCategories = new DataGridView();
            panel3 = new Panel();
            cboFilterType = new ComboBox();
            txtSearch = new TextBox();
            panel4 = new Panel();
            gbDetail = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnDelete = new FontAwesome.Sharp.IconButton();
            btnSave = new FontAwesome.Sharp.IconButton();
            btnCancel = new FontAwesome.Sharp.IconButton();
            btnNew = new FontAwesome.Sharp.IconButton();
            panel7 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel6 = new Panel();
            cboType = new ComboBox();
            label5 = new Label();
            txtCategoryName = new TextBox();
            label2 = new Label();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            gbDetail.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 8);
            label1.Name = "label1";
            label1.Size = new Size(291, 25);
            label1.TabIndex = 0;
            label1.Text = "QUẢN LÝ DANH MỤC THU/CHI";
            // 
            // btnRefresh
            // 
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            btnRefresh.IconColor = Color.Black;
            btnRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRefresh.IconSize = 32;
            btnRefresh.Location = new Point(1102, 8);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(50, 44);
            btnRefresh.TabIndex = 1;
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(16, 8, 16, 8);
            panel1.Size = new Size(1168, 60);
            panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 60);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1168, 637);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.Controls.Add(dgvCategories);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(16);
            panel2.Size = new Size(694, 631);
            panel2.TabIndex = 0;
            // 
            // dgvCategories
            // 
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.Size = new Size(662, 560);
            dgvCategories.TabIndex = 1;
            dgvCategories.Dock = DockStyle.Fill;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.MultiSelect = false;
            dgvCategories.ReadOnly = true;
            dgvCategories.Location = new Point(16, 55);
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.None;
            // 
            // panel3
            // 
            panel3.Controls.Add(cboFilterType);
            panel3.Controls.Add(txtSearch);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(16, 16);
            panel3.Name = "panel3";
            panel3.Size = new Size(662, 39);
            panel3.TabIndex = 0;
            // 
            // cboFilterType
            // 
            cboFilterType.Dock = DockStyle.Right;
            cboFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilterType.FormattingEnabled = true;
            cboFilterType.Items.AddRange(new object[] { "Tất cả", "Khoản Thu", "Khoản Chi" });
            cboFilterType.Location = new Point(438, 0);
            cboFilterType.Name = "cboFilterType";
            cboFilterType.Size = new Size(224, 23);
            cboFilterType.TabIndex = 5;
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Dock = DockStyle.Left;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Margin = new Padding(3, 3, 24, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm...";
            txtSearch.Size = new Size(411, 23);
            txtSearch.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(gbDetail);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(703, 3);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(16);
            panel4.Size = new Size(462, 631);
            panel4.TabIndex = 1;
            // 
            // gbDetail
            // 
            gbDetail.Controls.Add(tableLayoutPanel3);
            gbDetail.Controls.Add(panel7);
            gbDetail.Controls.Add(tableLayoutPanel2);
            gbDetail.Controls.Add(txtCategoryName);
            gbDetail.Controls.Add(label2);
            gbDetail.Dock = DockStyle.Fill;
            gbDetail.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbDetail.Location = new Point(16, 16);
            gbDetail.Name = "gbDetail";
            gbDetail.Padding = new Padding(16);
            gbDetail.Size = new Size(430, 599);
            gbDetail.TabIndex = 2;
            gbDetail.TabStop = false;
            gbDetail.Text = "Thông tin chi tiết";
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
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(16, 210);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(398, 100);
            tableLayoutPanel3.TabIndex = 10;
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
            btnDelete.Location = new Point(202, 53);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(8, 0, 0, 0);
            btnDelete.Size = new Size(193, 44);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
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
            btnSave.Size = new Size(193, 44);
            btnSave.TabIndex = 2;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
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
            btnCancel.Location = new Point(202, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(8, 0, 0, 0);
            btnCancel.Size = new Size(193, 44);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Bỏ qua";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
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
            btnNew.Size = new Size(193, 44);
            btnNew.TabIndex = 0;
            btnNew.Text = "Thêm mới";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += BtnNew_Click;
            // 
            // panel7
            // 
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(16, 186);
            panel7.Name = "panel7";
            panel7.Size = new Size(398, 24);
            panel7.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel6, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(16, 96);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(398, 90);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // panel6
            // 
            panel6.Controls.Add(cboType);
            panel6.Controls.Add(label5);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(3, 3);
            panel6.Name = "panel6";
            panel6.Padding = new Padding(8);
            panel6.Size = new Size(193, 84);
            panel6.TabIndex = 2;
            // 
            // cboType
            // 
            cboType.Dock = DockStyle.Fill;
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.FormattingEnabled = true;
            cboType.Items.AddRange(new object[] { "Thu", "Chi" });
            cboType.Location = new Point(8, 45);
            cboType.Name = "cboType";
            cboType.Size = new Size(177, 29);
            cboType.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Location = new Point(8, 8);
            label5.Name = "label5";
            label5.Padding = new Padding(0, 8, 0, 8);
            label5.Size = new Size(101, 37);
            label5.TabIndex = 5;
            label5.Text = "Loại Thu/Chi";
            // 
            // txtCategoryName
            // 
            txtCategoryName.BorderStyle = BorderStyle.FixedSingle;
            txtCategoryName.Dock = DockStyle.Top;
            txtCategoryName.Location = new Point(16, 67);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(398, 29);
            txtCategoryName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(16, 38);
            label2.Name = "label2";
            label2.Padding = new Padding(0, 0, 0, 8);
            label2.Size = new Size(131, 29);
            label2.TabIndex = 1;
            label2.Text = "Tên danh mục (*)";
            // 
            // ucTransactionCategory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel1);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucTransactionCategory";
            Size = new Size(1168, 697);
            Load += ucTransactionCategory_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            gbDetail.ResumeLayout(false);
            gbDetail.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private FontAwesome.Sharp.IconButton btnRefresh;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private DataGridView dgvCategories;
        private ComboBox cboFilterType;
        private TextBox txtSearch;
        private Panel panel3;
        private Panel panel4;
        private GroupBox gbDetail;
        private TableLayoutPanel tableLayoutPanel3;
        private FontAwesome.Sharp.IconButton btnDelete;
        private FontAwesome.Sharp.IconButton btnSave;
        private FontAwesome.Sharp.IconButton btnCancel;
        private FontAwesome.Sharp.IconButton btnNew;
        private TextBox txtCategoryName;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel7;
        private Panel panel6;
        private ComboBox cboType;
        private Label label5;
    }
}
