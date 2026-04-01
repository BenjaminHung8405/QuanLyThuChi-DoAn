namespace QuanLyThuChi_DoAn
{
    partial class ucTransaction
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
            pnlGrid = new Panel();
            dgvTransactions = new DataGridView();
            colDate = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colPartner = new DataGridViewTextBoxColumn();
            colNote = new DataGridViewTextBoxColumn();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlInput = new Panel();
            pnlButtons = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnDelete = new FontAwesome.Sharp.IconButton();
            btnSave = new FontAwesome.Sharp.IconButton();
            btnCancel = new FontAwesome.Sharp.IconButton();
            btnNew = new FontAwesome.Sharp.IconButton();
            gbTransactionDetail = new GroupBox();
            txtNote = new TextBox();
            lblNote = new Label();
            cboFund = new ComboBox();
            label2 = new Label();
            cboPartner = new ComboBox();
            lblPartner = new Label();
            cboCategory = new ComboBox();
            lblCategory = new Label();
            dtpTransactionDate = new DateTimePicker();
            lblDate = new Label();
            txtAmount = new TextBox();
            lblAmount = new Label();
            pnlRadio = new Panel();
            radOut = new RadioButton();
            radIn = new RadioButton();
            lblType = new Label();
            lblTotalIn = new Label();
            lblTotalOut = new Label();
            pnlHeader = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            panel1 = new Panel();
            label1 = new Label();
            btnRefresh = new FontAwesome.Sharp.IconButton();
            btnFilter = new FontAwesome.Sharp.IconButton();
            txtSearch = new TextBox();
            dtpFromDate = new DateTimePicker();
            dtpToDate = new DateTimePicker();
            pnlTotal = new Panel();
            label4 = new Label();
            cboFilterCategory = new ComboBox();
            label3 = new Label();
            cboFilterPartner = new ComboBox();
            lblBalance = new Label();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            pnlInput.SuspendLayout();
            pnlButtons.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            gbTransactionDetail.SuspendLayout();
            pnlRadio.SuspendLayout();
            pnlHeader.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel1.SuspendLayout();
            pnlTotal.SuspendLayout();
            SuspendLayout();
            // 
            // pnlGrid
            // 
            pnlGrid.Controls.Add(dgvTransactions);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(3, 3);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(16);
            pnlGrid.Size = new Size(754, 565);
            pnlGrid.TabIndex = 0;
            // 
            // dgvTransactions
            // 
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.BackgroundColor = Color.White;
            dgvTransactions.BorderStyle = BorderStyle.None;
            dgvTransactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTransactions.Columns.AddRange(new DataGridViewColumn[] { colDate, colType, colAmount, colCategory, colPartner, colNote });
            dgvTransactions.Dock = DockStyle.Fill;
            dgvTransactions.Location = new Point(16, 16);
            dgvTransactions.MultiSelect = false;
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.Size = new Size(722, 533);
            dgvTransactions.TabIndex = 0;
            // 
            // colDate
            // 
            colDate.DataPropertyName = "TransactionDate";
            colDate.HeaderText = "Ngày";
            colDate.MinimumWidth = 8;
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
            // 
            // colType
            // 
            colType.DataPropertyName = "Type";
            colType.HeaderText = "Loại";
            colType.MinimumWidth = 8;
            colType.Name = "colType";
            colType.ReadOnly = true;
            colType.Width = 80;
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "Amount";
            colAmount.HeaderText = "Số tiền";
            colAmount.MinimumWidth = 8;
            colAmount.Name = "colAmount";
            colAmount.ReadOnly = true;
            colAmount.Width = 120;
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "Category.CategoryName";
            colCategory.HeaderText = "Danh mục";
            colCategory.MinimumWidth = 8;
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            colCategory.Width = 150;
            // 
            // colPartner
            // 
            colPartner.DataPropertyName = "Partner.PartnerName";
            colPartner.HeaderText = "Đối tác";
            colPartner.MinimumWidth = 8;
            colPartner.Name = "colPartner";
            colPartner.ReadOnly = true;
            colPartner.Width = 150;
            // 
            // colNote
            // 
            colNote.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNote.DataPropertyName = "Note";
            colNote.HeaderText = "Ghi chú";
            colNote.MinimumWidth = 8;
            colNote.Name = "colNote";
            colNote.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.1541061F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.84589F));
            tableLayoutPanel1.Controls.Add(pnlGrid, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlInput, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 126);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1168, 571);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // pnlInput
            // 
            pnlInput.AutoScroll = true;
            pnlInput.Controls.Add(pnlButtons);
            pnlInput.Controls.Add(gbTransactionDetail);
            pnlInput.Dock = DockStyle.Fill;
            pnlInput.Location = new Point(763, 3);
            pnlInput.Name = "pnlInput";
            pnlInput.Padding = new Padding(16);
            pnlInput.Size = new Size(402, 565);
            pnlInput.TabIndex = 1;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(tableLayoutPanel3);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(16, 463);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(353, 99);
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
            tableLayoutPanel3.Size = new Size(353, 100);
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
            btnDelete.Location = new Point(179, 53);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(8, 0, 0, 0);
            btnDelete.Size = new Size(171, 44);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
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
            btnSave.Size = new Size(170, 44);
            btnSave.TabIndex = 2;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
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
            btnCancel.Location = new Point(179, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(8, 0, 0, 0);
            btnCancel.Size = new Size(171, 44);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Bỏ qua";
            btnCancel.UseVisualStyleBackColor = true;
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
            btnNew.Size = new Size(170, 44);
            btnNew.TabIndex = 0;
            btnNew.Text = "Thêm mới";
            btnNew.UseVisualStyleBackColor = true;
            // 
            // gbTransactionDetail
            // 
            gbTransactionDetail.Controls.Add(txtNote);
            gbTransactionDetail.Controls.Add(lblNote);
            gbTransactionDetail.Controls.Add(cboFund);
            gbTransactionDetail.Controls.Add(label2);
            gbTransactionDetail.Controls.Add(cboPartner);
            gbTransactionDetail.Controls.Add(lblPartner);
            gbTransactionDetail.Controls.Add(cboCategory);
            gbTransactionDetail.Controls.Add(lblCategory);
            gbTransactionDetail.Controls.Add(dtpTransactionDate);
            gbTransactionDetail.Controls.Add(lblDate);
            gbTransactionDetail.Controls.Add(txtAmount);
            gbTransactionDetail.Controls.Add(lblAmount);
            gbTransactionDetail.Controls.Add(pnlRadio);
            gbTransactionDetail.Controls.Add(lblType);
            gbTransactionDetail.Dock = DockStyle.Top;
            gbTransactionDetail.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbTransactionDetail.Location = new Point(16, 16);
            gbTransactionDetail.Name = "gbTransactionDetail";
            gbTransactionDetail.Padding = new Padding(16);
            gbTransactionDetail.Size = new Size(353, 463);
            gbTransactionDetail.TabIndex = 2;
            gbTransactionDetail.TabStop = false;
            gbTransactionDetail.Text = "Thông tin Giao dịch";
            // 
            // txtNote
            // 
            txtNote.BorderStyle = BorderStyle.FixedSingle;
            txtNote.Dock = DockStyle.Top;
            txtNote.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNote.Location = new Point(16, 409);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(321, 60);
            txtNote.TabIndex = 15;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.Dock = DockStyle.Top;
            lblNote.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNote.Location = new Point(16, 380);
            lblNote.Name = "lblNote";
            lblNote.Padding = new Padding(0, 5, 0, 5);
            lblNote.Size = new Size(58, 29);
            lblNote.TabIndex = 14;
            lblNote.Text = "Ghi chú";
            // 
            // cboFund
            // 
            cboFund.Dock = DockStyle.Top;
            cboFund.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFund.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboFund.FormattingEnabled = true;
            cboFund.Location = new Point(16, 352);
            cboFund.Name = "cboFund";
            cboFund.Size = new Size(321, 28);
            cboFund.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(16, 323);
            label2.Name = "label2";
            label2.Padding = new Padding(0, 5, 0, 5);
            label2.Size = new Size(129, 29);
            label2.TabIndex = 12;
            label2.Text = "Quỹ tiền/Tài khoản";
            // 
            // cboPartner
            // 
            cboPartner.Dock = DockStyle.Top;
            cboPartner.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPartner.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboPartner.FormattingEnabled = true;
            cboPartner.Location = new Point(16, 295);
            cboPartner.Name = "cboPartner";
            cboPartner.Size = new Size(321, 28);
            cboPartner.TabIndex = 9;
            // 
            // lblPartner
            // 
            lblPartner.AutoSize = true;
            lblPartner.Dock = DockStyle.Top;
            lblPartner.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPartner.Location = new Point(16, 266);
            lblPartner.Name = "lblPartner";
            lblPartner.Padding = new Padding(0, 5, 0, 5);
            lblPartner.Size = new Size(54, 29);
            lblPartner.TabIndex = 8;
            lblPartner.Text = "Đối tác";
            // 
            // cboCategory
            // 
            cboCategory.Dock = DockStyle.Top;
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboCategory.FormattingEnabled = true;
            cboCategory.Location = new Point(16, 238);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(321, 28);
            cboCategory.TabIndex = 7;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Dock = DockStyle.Top;
            lblCategory.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategory.Location = new Point(16, 209);
            lblCategory.Name = "lblCategory";
            lblCategory.Padding = new Padding(0, 5, 0, 5);
            lblCategory.Size = new Size(93, 29);
            lblCategory.TabIndex = 6;
            lblCategory.Text = "Danh mục (*)";
            // 
            // dtpTransactionDate
            // 
            dtpTransactionDate.CustomFormat = "dd/MM/yyyy";
            dtpTransactionDate.Dock = DockStyle.Top;
            dtpTransactionDate.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTransactionDate.Format = DateTimePickerFormat.Custom;
            dtpTransactionDate.Location = new Point(16, 182);
            dtpTransactionDate.Name = "dtpTransactionDate";
            dtpTransactionDate.Size = new Size(321, 27);
            dtpTransactionDate.TabIndex = 5;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Dock = DockStyle.Top;
            lblDate.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDate.Location = new Point(16, 153);
            lblDate.Name = "lblDate";
            lblDate.Padding = new Padding(0, 5, 0, 5);
            lblDate.Size = new Size(124, 29);
            lblDate.TabIndex = 4;
            lblDate.Text = "Ngày lập phiếu (*)";
            // 
            // txtAmount
            // 
            txtAmount.BorderStyle = BorderStyle.FixedSingle;
            txtAmount.Dock = DockStyle.Top;
            txtAmount.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAmount.Location = new Point(16, 126);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(321, 27);
            txtAmount.TabIndex = 3;
            txtAmount.TextAlign = HorizontalAlignment.Right;
            // 
            // lblAmount
            // 
            lblAmount.AutoSize = true;
            lblAmount.Dock = DockStyle.Top;
            lblAmount.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAmount.Location = new Point(16, 97);
            lblAmount.Name = "lblAmount";
            lblAmount.Padding = new Padding(0, 5, 0, 5);
            lblAmount.Size = new Size(135, 29);
            lblAmount.TabIndex = 2;
            lblAmount.Text = "Số tiền giao dịch (*)";
            // 
            // pnlRadio
            // 
            pnlRadio.Controls.Add(radOut);
            pnlRadio.Controls.Add(radIn);
            pnlRadio.Dock = DockStyle.Top;
            pnlRadio.Location = new Point(16, 62);
            pnlRadio.Name = "pnlRadio";
            pnlRadio.Size = new Size(321, 35);
            pnlRadio.TabIndex = 1;
            // 
            // radOut
            // 
            radOut.AutoSize = true;
            radOut.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            radOut.Location = new Point(160, 6);
            radOut.Name = "radOut";
            radOut.Size = new Size(94, 24);
            radOut.TabIndex = 1;
            radOut.Text = "Khoản Chi";
            radOut.UseVisualStyleBackColor = true;
            // 
            // radIn
            // 
            radIn.AutoSize = true;
            radIn.Checked = true;
            radIn.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            radIn.Location = new Point(8, 6);
            radIn.Name = "radIn";
            radIn.Size = new Size(97, 24);
            radIn.TabIndex = 0;
            radIn.TabStop = true;
            radIn.Text = "Khoản Thu";
            radIn.UseVisualStyleBackColor = true;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Dock = DockStyle.Top;
            lblType.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblType.Location = new Point(16, 38);
            lblType.Name = "lblType";
            lblType.Padding = new Padding(0, 0, 0, 5);
            lblType.Size = new Size(97, 24);
            lblType.TabIndex = 0;
            lblType.Text = "Loại giao dịch";
            // 
            // lblTotalIn
            // 
            lblTotalIn.AutoSize = true;
            lblTotalIn.Dock = DockStyle.Left;
            lblTotalIn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalIn.ForeColor = Color.FromArgb(46, 125, 50);
            lblTotalIn.Location = new Point(0, 0);
            lblTotalIn.Name = "lblTotalIn";
            lblTotalIn.Size = new Size(113, 21);
            lblTotalIn.TabIndex = 0;
            lblTotalIn.Text = "Tổng Thu: 0 đ";
            // 
            // lblTotalOut
            // 
            lblTotalOut.AutoSize = true;
            lblTotalOut.Dock = DockStyle.Left;
            lblTotalOut.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalOut.ForeColor = Color.FromArgb(198, 40, 40);
            lblTotalOut.Location = new Point(113, 0);
            lblTotalOut.Name = "lblTotalOut";
            lblTotalOut.Padding = new Padding(30, 0, 0, 0);
            lblTotalOut.Size = new Size(139, 21);
            lblTotalOut.TabIndex = 1;
            lblTotalOut.Text = "Tổng Chi: 0 đ";
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(tableLayoutPanel4);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(16, 8, 16, 8);
            pnlHeader.Size = new Size(1168, 126);
            pnlHeader.TabIndex = 5;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(lblBalance, 0, 2);
            tableLayoutPanel4.Controls.Add(panel1, 0, 0);
            tableLayoutPanel4.Controls.Add(pnlTotal, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(16, 8);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.Size = new Size(1136, 110);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnFilter);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(dtpFromDate);
            panel1.Controls.Add(dtpToDate);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 2, 8, 2);
            panel1.Size = new Size(1130, 30);
            panel1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 2);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 0, 16, 0);
            label1.Size = new Size(214, 25);
            label1.TabIndex = 2;
            label1.Text = "QUẢN LÝ GIAO DỊCH";
            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            btnRefresh.IconColor = Color.FromArgb(46, 125, 50);
            btnRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRefresh.IconSize = 20;
            btnRefresh.Location = new Point(556, 2);
            btnRefresh.Margin = new Padding(2, 0, 4, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(38, 26);
            btnRefresh.TabIndex = 3;
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Cursor = Cursors.Hand;
            btnFilter.Dock = DockStyle.Right;
            btnFilter.IconChar = FontAwesome.Sharp.IconChar.Filter;
            btnFilter.IconColor = Color.FromArgb(25, 103, 210);
            btnFilter.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnFilter.IconSize = 20;
            btnFilter.Location = new Point(594, 2);
            btnFilter.Margin = new Padding(2, 0, 2, 0);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(38, 26);
            btnFilter.TabIndex = 4;
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Dock = DockStyle.Right;
            txtSearch.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(632, 2);
            txtSearch.Margin = new Padding(4, 0, 8, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm...";
            txtSearch.Size = new Size(270, 25);
            txtSearch.TabIndex = 5;
            // 
            // dtpFromDate
            // 
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpFromDate.Dock = DockStyle.Right;
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.Location = new Point(902, 2);
            dtpFromDate.Margin = new Padding(4, 0, 4, 0);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(110, 23);
            dtpFromDate.TabIndex = 6;
            // 
            // dtpToDate
            // 
            dtpToDate.CustomFormat = "dd/MM/yyyy";
            dtpToDate.Dock = DockStyle.Right;
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Location = new Point(1012, 2);
            dtpToDate.Margin = new Padding(4, 0, 4, 0);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(110, 23);
            dtpToDate.TabIndex = 7;
            // 
            // pnlTotal
            // 
            pnlTotal.Controls.Add(label4);
            pnlTotal.Controls.Add(cboFilterCategory);
            pnlTotal.Controls.Add(label3);
            pnlTotal.Controls.Add(cboFilterPartner);
            pnlTotal.Controls.Add(lblTotalOut);
            pnlTotal.Controls.Add(lblTotalIn);
            pnlTotal.Dock = DockStyle.Fill;
            pnlTotal.Location = new Point(3, 39);
            pnlTotal.Name = "pnlTotal";
            pnlTotal.Size = new Size(1130, 30);
            pnlTotal.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Right;
            label4.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(541, 0);
            label4.Name = "label4";
            label4.Padding = new Padding(0, 5, 0, 5);
            label4.Size = new Size(76, 29);
            label4.TabIndex = 13;
            label4.Text = "Danh mục:";
            // 
            // cboFilterCategory
            // 
            cboFilterCategory.Dock = DockStyle.Right;
            cboFilterCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilterCategory.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboFilterCategory.FormattingEnabled = true;
            cboFilterCategory.Location = new Point(617, 0);
            cboFilterCategory.Name = "cboFilterCategory";
            cboFilterCategory.Size = new Size(228, 28);
            cboFilterCategory.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Right;
            label3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(845, 0);
            label3.Name = "label3";
            label3.Padding = new Padding(0, 5, 0, 5);
            label3.Size = new Size(57, 29);
            label3.TabIndex = 11;
            label3.Text = "Đối tác:";
            // 
            // cboFilterPartner
            // 
            cboFilterPartner.Dock = DockStyle.Right;
            cboFilterPartner.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilterPartner.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboFilterPartner.FormattingEnabled = true;
            cboFilterPartner.Location = new Point(902, 0);
            cboFilterPartner.Name = "cboFilterPartner";
            cboFilterPartner.Size = new Size(228, 28);
            cboFilterPartner.TabIndex = 10;
            // 
            // lblBalance
            // 
            lblBalance.AutoSize = true;
            lblBalance.Dock = DockStyle.Left;
            lblBalance.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalance.ForeColor = Color.FromArgb(25, 103, 210);
            lblBalance.Location = new Point(3, 72);
            lblBalance.Name = "lblBalance";
            lblBalance.Padding = new Padding(30, 0, 0, 0);
            lblBalance.Size = new Size(239, 38);
            lblBalance.TabIndex = 5;
            lblBalance.Text = "Số dư quỹ hiện tại: ... VNĐ";
            // 
            // ucTransaction
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Controls.Add(pnlHeader);
            ForeColor = Color.FromArgb(51, 51, 51);
            Name = "ucTransaction";
            Size = new Size(1168, 697);
            Load += ucTransaction_Load;
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            pnlInput.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            gbTransactionDetail.ResumeLayout(false);
            gbTransactionDetail.PerformLayout();
            pnlRadio.ResumeLayout(false);
            pnlRadio.PerformLayout();
            pnlHeader.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            pnlTotal.ResumeLayout(false);
            pnlTotal.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TextBox txtSearch;
        private Panel pnlGrid;
        private DataGridView dgvTransactions;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colPartner;
        private DataGridViewTextBoxColumn colNote;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel pnlInput;
        private Panel pnlHeader;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel1;
        private FontAwesome.Sharp.IconButton btnRefresh;
        private Label label1;
        private FontAwesome.Sharp.IconButton btnFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Panel pnlTotal;
        private Label lblTotalIn;
        private Label lblTotalOut;
        private GroupBox gbTransactionDetail;
        private ComboBox cboPartner;
        private Label lblPartner;
        private ComboBox cboCategory;
        private Label lblCategory;
        private DateTimePicker dtpTransactionDate;
        private Label lblDate;
        private TextBox txtAmount;
        private Label lblAmount;
        private Panel pnlRadio;
        private RadioButton radOut;
        private RadioButton radIn;
        private Label lblType;
        private Panel pnlButtons;
        private TableLayoutPanel tableLayoutPanel3;
        private FontAwesome.Sharp.IconButton btnDelete;
        private FontAwesome.Sharp.IconButton btnSave;
        private FontAwesome.Sharp.IconButton btnCancel;
        private FontAwesome.Sharp.IconButton btnNew;
        private TextBox txtNote;
        private Label lblNote;
        private ComboBox cboFund;
        private Label label2;
        private Label label4;
        private ComboBox cboFilterCategory;
        private Label label3;
        private ComboBox cboFilterPartner;
        private Label lblBalance;
    }
}
