namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class ucAuditLogViewer
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
            flpFilters = new FlowLayoutPanel();
            lblFromDate = new Label();
            dtpFromDate = new DateTimePicker();
            lblToDate = new Label();
            dtpToDate = new DateTimePicker();
            lblUser = new Label();
            cboUser = new ComboBox();
            lblActionType = new Label();
            cboActionType = new ComboBox();
            btnFilter = new Button();
            btnExport = new Button();
            lblTitle = new Label();
            pnlMain = new Panel();
            dgvLogs = new DataGridView();
            pnlHeader.SuspendLayout();
            flpFilters.SuspendLayout();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.WhiteSmoke;
            pnlHeader.Controls.Add(flpFilters);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(12);
            pnlHeader.Size = new Size(1168, 110);
            pnlHeader.TabIndex = 0;
            // 
            // flpFilters
            // 
            flpFilters.AutoScroll = true;
            flpFilters.Controls.Add(lblFromDate);
            flpFilters.Controls.Add(dtpFromDate);
            flpFilters.Controls.Add(lblToDate);
            flpFilters.Controls.Add(dtpToDate);
            flpFilters.Controls.Add(lblUser);
            flpFilters.Controls.Add(cboUser);
            flpFilters.Controls.Add(lblActionType);
            flpFilters.Controls.Add(cboActionType);
            flpFilters.Controls.Add(btnFilter);
            flpFilters.Controls.Add(btnExport);
            flpFilters.Dock = DockStyle.Fill;
            flpFilters.Location = new Point(12, 54);
            flpFilters.Name = "flpFilters";
            flpFilters.Padding = new Padding(0, 4, 0, 0);
            flpFilters.Size = new Size(1144, 44);
            flpFilters.TabIndex = 1;
            flpFilters.WrapContents = false;
            // 
            // lblFromDate
            // 
            lblFromDate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblFromDate.Location = new Point(3, 4);
            lblFromDate.Margin = new Padding(3, 0, 6, 0);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(64, 30);
            lblFromDate.TabIndex = 0;
            lblFromDate.Text = "Từ ngày";
            lblFromDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpFromDate
            // 
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpFromDate.Font = new Font("Segoe UI", 9.5F);
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.Location = new Point(73, 7);
            dtpFromDate.Margin = new Padding(0, 3, 12, 3);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(130, 24);
            dtpFromDate.TabIndex = 1;
            // 
            // lblToDate
            // 
            lblToDate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblToDate.Location = new Point(215, 4);
            lblToDate.Margin = new Padding(0, 0, 6, 0);
            lblToDate.Name = "lblToDate";
            lblToDate.Size = new Size(72, 30);
            lblToDate.TabIndex = 2;
            lblToDate.Text = "Đến ngày";
            lblToDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpToDate
            // 
            dtpToDate.CustomFormat = "dd/MM/yyyy";
            dtpToDate.Font = new Font("Segoe UI", 9.5F);
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Location = new Point(293, 7);
            dtpToDate.Margin = new Padding(0, 3, 12, 3);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(130, 24);
            dtpToDate.TabIndex = 3;
            // 
            // lblUser
            // 
            lblUser.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblUser.Location = new Point(435, 4);
            lblUser.Margin = new Padding(0, 0, 6, 0);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(90, 30);
            lblUser.TabIndex = 4;
            lblUser.Text = "Nhân viên";
            lblUser.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboUser
            // 
            cboUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUser.Font = new Font("Segoe UI", 9.5F);
            cboUser.FormattingEnabled = true;
            cboUser.Location = new Point(531, 7);
            cboUser.Margin = new Padding(0, 3, 12, 3);
            cboUser.Name = "cboUser";
            cboUser.Size = new Size(220, 25);
            cboUser.TabIndex = 5;
            // 
            // lblActionType
            // 
            lblActionType.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblActionType.Location = new Point(763, 4);
            lblActionType.Margin = new Padding(0, 0, 6, 0);
            lblActionType.Name = "lblActionType";
            lblActionType.Size = new Size(82, 30);
            lblActionType.TabIndex = 6;
            lblActionType.Text = "Hành động";
            lblActionType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboActionType
            // 
            cboActionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboActionType.Font = new Font("Segoe UI", 9.5F);
            cboActionType.FormattingEnabled = true;
            cboActionType.Location = new Point(851, 7);
            cboActionType.Margin = new Padding(0, 3, 12, 3);
            cboActionType.Name = "cboActionType";
            cboActionType.Size = new Size(140, 25);
            cboActionType.TabIndex = 7;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = Color.FromArgb(33, 150, 243);
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.FlatStyle = FlatStyle.Flat;
            btnFilter.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnFilter.ForeColor = Color.White;
            btnFilter.Location = new Point(1003, 6);
            btnFilter.Margin = new Padding(0, 2, 8, 2);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(120, 28);
            btnFilter.TabIndex = 8;
            btnFilter.Text = "🔍 Lọc dữ liệu";
            btnFilter.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.SeaGreen;
            btnExport.Enabled = false;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(1131, 6);
            btnExport.Margin = new Padding(0, 2, 0, 2);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(122, 28);
            btnExport.TabIndex = 9;
            btnExport.Text = "📥 Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(12, 51, 82);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1144, 42);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "NHẬT KÝ HỆ THỐNG (AUDIT LOGS)";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(dgvLogs);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 110);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(10);
            pnlMain.Size = new Size(1168, 587);
            pnlMain.TabIndex = 1;
            // 
            // dgvLogs
            // 
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogs.BackgroundColor = Color.White;
            dgvLogs.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 239, 241);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvLogs.DefaultCellStyle = dataGridViewCellStyle2;
            dgvLogs.Dock = DockStyle.Fill;
            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.Location = new Point(10, 10);
            dgvLogs.MultiSelect = false;
            dgvLogs.Name = "dgvLogs";
            dgvLogs.ReadOnly = true;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.RowHeadersWidth = 51;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.Size = new Size(1148, 567);
            dgvLogs.TabIndex = 0;
            // 
            // ucAuditLogViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            Name = "ucAuditLogViewer";
            Size = new Size(1168, 697);
            Load += ucAuditLogViewer_Load;
            pnlHeader.ResumeLayout(false);
            flpFilters.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private FlowLayoutPanel flpFilters;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Label lblUser;
        private ComboBox cboUser;
        private Label lblActionType;
        private ComboBox cboActionType;
        private Button btnFilter;
        private Button btnExport;
        private Label lblTitle;
        private Panel pnlMain;
        private DataGridView dgvLogs;
    }
}
