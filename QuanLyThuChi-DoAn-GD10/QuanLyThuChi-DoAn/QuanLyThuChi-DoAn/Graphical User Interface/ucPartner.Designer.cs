namespace QuanLyThuChi_DoAn
{
    partial class ucPartner
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
            panel1 = new Panel();
            btnRefresh = new FontAwesome.Sharp.IconButton();
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            dgvPartners = new DataGridView();
            PartnerName = new DataGridViewTextBoxColumn();
            Phone = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            InitialDebt = new DataGridViewTextBoxColumn();
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
            txtInitialDebt = new NumericUpDown();
            label6 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel6 = new Panel();
            cboType = new ComboBox();
            label5 = new Label();
            panel5 = new Panel();
            txtPhone = new TextBox();
            label4 = new Label();
            txtAddress = new TextBox();
            label3 = new Label();
            txtPartnerName = new TextBox();
            label2 = new Label();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPartners).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            gbDetail.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtInitialDebt).BeginInit();
            SuspendLayout();
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
            panel1.TabIndex = 0;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 8);
            label1.Name = "label1";
            label1.Size = new Size(173, 25);
            label1.TabIndex = 0;
            label1.Text = "QUẢN LÝ ĐỐI TÁC";
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
            tableLayoutPanel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(dgvPartners);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(16);
            panel2.Size = new Size(694, 631);
            panel2.TabIndex = 0;
            // 
            // dgvPartners
            // 
            dgvPartners.AllowUserToAddRows = false;
            dgvPartners.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPartners.BackgroundColor = Color.White;
            dgvPartners.BorderStyle = BorderStyle.None;
            dgvPartners.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPartners.Columns.AddRange(new DataGridViewColumn[] { PartnerName, Phone, Type, InitialDebt });
            dgvPartners.Dock = DockStyle.Fill;
            dgvPartners.Location = new Point(16, 55);
            dgvPartners.MultiSelect = false;
            dgvPartners.Name = "dgvPartners";
            dgvPartners.ReadOnly = true;
            dgvPartners.RowHeadersVisible = false;
            dgvPartners.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPartners.Size = new Size(662, 560);
            dgvPartners.TabIndex = 1;
            // 
            // PartnerName
            // 
            PartnerName.DataPropertyName = "PartnerName";
            PartnerName.FillWeight = 150F;
            PartnerName.HeaderText = "Tên Đối Tác";
            PartnerName.Name = "PartnerName";
            PartnerName.ReadOnly = true;
            // 
            // Phone
            // 
            Phone.DataPropertyName = "Phone";
            Phone.HeaderText = "Số Điện Thoại";
            Phone.Name = "Phone";
            Phone.ReadOnly = true;
            // 
            // Type
            // 
            Type.DataPropertyName = "Type";
            Type.FillWeight = 80F;
            Type.HeaderText = "Loại";
            Type.Name = "Type";
            Type.ReadOnly = true;
            // 
            // InitialDebt
            // 
            InitialDebt.DataPropertyName = "InitialDebt";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            InitialDebt.DefaultCellStyle = dataGridViewCellStyle1;
            InitialDebt.HeaderText = "Nợ Đầu Kỳ";
            InitialDebt.Name = "InitialDebt";
            InitialDebt.ReadOnly = true;
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
            cboFilterType.FormattingEnabled = true;
            cboFilterType.Items.AddRange(new object[] { "Tất cả", "Khách hàng", "Nhà cung cấp" });
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
            txtSearch.PlaceholderText = "Tìm kiếm nhanh (Tên/SĐT)";
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
            gbDetail.Controls.Add(txtInitialDebt);
            gbDetail.Controls.Add(label6);
            gbDetail.Controls.Add(tableLayoutPanel2);
            gbDetail.Controls.Add(txtAddress);
            gbDetail.Controls.Add(label3);
            gbDetail.Controls.Add(txtPartnerName);
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
            tableLayoutPanel3.Location = new Point(16, 342);
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
            panel7.Location = new Point(16, 318);
            panel7.Name = "panel7";
            panel7.Size = new Size(398, 24);
            panel7.TabIndex = 9;
            // 
            // txtInitialDebt
            // 
            txtInitialDebt.BorderStyle = BorderStyle.FixedSingle;
            txtInitialDebt.Dock = DockStyle.Top;
            txtInitialDebt.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            txtInitialDebt.Location = new Point(16, 289);
            txtInitialDebt.Maximum = decimal.MaxValue;
            txtInitialDebt.Name = "txtInitialDebt";
            txtInitialDebt.Size = new Size(398, 29);
            txtInitialDebt.TabIndex = 7;
            txtInitialDebt.TextAlign = HorizontalAlignment.Right;
            txtInitialDebt.ThousandsSeparator = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Location = new Point(16, 252);
            label6.Name = "label6";
            label6.Padding = new Padding(0, 8, 0, 8);
            label6.Size = new Size(61, 37);
            label6.TabIndex = 6;
            label6.Text = "Số tiền";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel6, 1, 0);
            tableLayoutPanel2.Controls.Add(panel5, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(16, 162);
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
            panel6.Location = new Point(202, 3);
            panel6.Name = "panel6";
            panel6.Padding = new Padding(8);
            panel6.Size = new Size(193, 84);
            panel6.TabIndex = 1;
            // 
            // cboType
            // 
            cboType.Dock = DockStyle.Fill;
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.FormattingEnabled = true;
            cboType.Items.AddRange(new object[] { "Khách hàng", "Nhà cung cấp" });
            cboType.Location = new Point(8, 45);
            cboType.Name = "cboType";
            cboType.Size = new Size(177, 29);
            cboType.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Location = new Point(8, 8);
            label5.Name = "label5";
            label5.Padding = new Padding(0, 8, 0, 8);
            label5.Size = new Size(0, 37);
            label5.TabIndex = 5;
            // 
            // panel5
            // 
            panel5.Controls.Add(txtPhone);
            panel5.Controls.Add(label4);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(3, 3);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(8);
            panel5.Size = new Size(193, 84);
            panel5.TabIndex = 0;
            // 
            // txtPhone
            // 
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.Dock = DockStyle.Fill;
            txtPhone.Location = new Point(8, 45);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(177, 29);
            txtPhone.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(8, 8);
            label4.Name = "label4";
            label4.Padding = new Padding(0, 8, 0, 8);
            label4.Size = new Size(106, 37);
            label4.TabIndex = 5;
            label4.Text = "Số điện thoại";
            // 
            // txtAddress
            // 
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            txtAddress.Dock = DockStyle.Top;
            txtAddress.Location = new Point(16, 133);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(398, 29);
            txtAddress.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(16, 96);
            label3.Name = "label3";
            label3.Padding = new Padding(0, 8, 0, 8);
            label3.Size = new Size(58, 37);
            label3.TabIndex = 3;
            label3.Text = "Địa chỉ";
            // 
            // txtPartnerName
            // 
            txtPartnerName.BorderStyle = BorderStyle.FixedSingle;
            txtPartnerName.Dock = DockStyle.Top;
            txtPartnerName.Location = new Point(16, 67);
            txtPartnerName.Name = "txtPartnerName";
            txtPartnerName.Size = new Size(398, 29);
            txtPartnerName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(16, 38);
            label2.Name = "label2";
            label2.Padding = new Padding(0, 0, 0, 8);
            label2.Size = new Size(208, 29);
            label2.TabIndex = 1;
            label2.Text = "Khách hàng / Nhà cung cấp";
            // 
            // ucPartner
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel1);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucPartner";
            Size = new Size(1168, 697);
            Load += ucPartner_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPartners).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            gbDetail.ResumeLayout(false);
            gbDetail.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtInitialDebt).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private DataGridView dgvPartners;
        private Panel panel3;
        private ComboBox cboFilterType;
        private TextBox txtSearch;
        private Panel panel4;
        private GroupBox gbDetail;
        private Label label2;
        private TextBox txtAddress;
        private Label label3;
        private TextBox txtPartnerName;
        private NumericUpDown txtInitialDebt;
        private Label label6;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel6;
        private ComboBox cboType;
        private Label label5;
        private Panel panel5;
        private TextBox txtPhone;
        private Label label4;
        private TableLayoutPanel tableLayoutPanel3;
        private FontAwesome.Sharp.IconButton btnDelete;
        private FontAwesome.Sharp.IconButton btnSave;
        private FontAwesome.Sharp.IconButton btnCancel;
        private FontAwesome.Sharp.IconButton btnNew;
        private Panel panel7;
        private DataGridViewTextBoxColumn PartnerName;
        private DataGridViewTextBoxColumn Phone;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn InitialDebt;
        private FontAwesome.Sharp.IconButton btnRefresh;
    }
}
