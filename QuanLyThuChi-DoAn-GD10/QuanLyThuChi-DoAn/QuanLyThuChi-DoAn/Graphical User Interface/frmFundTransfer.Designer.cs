namespace QuanLyThuChi_DoAn
{
    partial class frmFundTransfer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label2 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel3 = new Panel();
            lblBalanceDestFund = new Label();
            label9 = new Label();
            cbDestFund = new ComboBox();
            label3 = new Label();
            panel2 = new Panel();
            lblBalanceSourceFund = new Label();
            label7 = new Label();
            cbSourceFund = new ComboBox();
            label1 = new Label();
            panel4 = new Panel();
            txtNotes = new TextBox();
            label6 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label5 = new Label();
            txtAmount = new TextBox();
            label4 = new Label();
            panel5 = new Panel();
            btnTransfer = new FontAwesome.Sharp.IconButton();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(16, 16);
            panel1.Name = "panel1";
            panel1.Size = new Size(752, 60);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Padding = new Padding(0, 0, 0, 16);
            label2.Size = new Size(203, 46);
            label2.TabIndex = 1;
            label2.Text = "Chuyển Quỹ nội bộ";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(panel3, 2, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 1, 0);
            tableLayoutPanel1.Controls.Add(panel5, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(16, 76);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 62.260128F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 37.739872F));
            tableLayoutPanel1.Size = new Size(752, 469);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblBalanceDestFund);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(cbDestFund);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(503, 3);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(16);
            panel3.Size = new Size(246, 286);
            panel3.TabIndex = 2;
            // 
            // lblBalanceDestFund
            // 
            lblBalanceDestFund.AutoSize = true;
            lblBalanceDestFund.Dock = DockStyle.Top;
            lblBalanceDestFund.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalanceDestFund.Location = new Point(16, 129);
            lblBalanceDestFund.Name = "lblBalanceDestFund";
            lblBalanceDestFund.Padding = new Padding(0, 0, 0, 16);
            lblBalanceDestFund.Size = new Size(101, 37);
            lblBalanceDestFund.TabIndex = 5;
            lblBalanceDestFund.Text = "10.000.000 đ";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Top;
            label9.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(16, 76);
            label9.Name = "label9";
            label9.Padding = new Padding(0, 16, 0, 16);
            label9.Size = new Size(57, 53);
            label9.TabIndex = 4;
            label9.Text = "Số dư:";
            // 
            // cbDestFund
            // 
            cbDestFund.Dock = DockStyle.Top;
            cbDestFund.FormattingEnabled = true;
            cbDestFund.Location = new Point(16, 53);
            cbDestFund.Name = "cbDestFund";
            cbDestFund.Size = new Size(214, 23);
            cbDestFund.TabIndex = 1;
            cbDestFund.SelectedIndexChanged += CbDestFund_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(16, 16);
            label3.Name = "label3";
            label3.Padding = new Padding(0, 0, 0, 16);
            label3.Size = new Size(78, 37);
            label3.TabIndex = 0;
            label3.Text = "Quỹ đích";
            // 
            // panel2
            // 
            panel2.Controls.Add(lblBalanceSourceFund);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(cbSourceFund);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(16);
            panel2.Size = new Size(244, 286);
            panel2.TabIndex = 0;
            // 
            // lblBalanceSourceFund
            // 
            lblBalanceSourceFund.AutoSize = true;
            lblBalanceSourceFund.Dock = DockStyle.Top;
            lblBalanceSourceFund.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalanceSourceFund.Location = new Point(16, 129);
            lblBalanceSourceFund.Name = "lblBalanceSourceFund";
            lblBalanceSourceFund.Padding = new Padding(0, 0, 0, 16);
            lblBalanceSourceFund.Size = new Size(101, 37);
            lblBalanceSourceFund.TabIndex = 3;
            lblBalanceSourceFund.Text = "10.000.000 đ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Top;
            label7.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(16, 76);
            label7.Name = "label7";
            label7.Padding = new Padding(0, 16, 0, 16);
            label7.Size = new Size(57, 53);
            label7.TabIndex = 2;
            label7.Text = "Số dư:";
            // 
            // cbSourceFund
            // 
            cbSourceFund.Dock = DockStyle.Top;
            cbSourceFund.FormattingEnabled = true;
            cbSourceFund.Location = new Point(16, 53);
            cbSourceFund.Name = "cbSourceFund";
            cbSourceFund.Size = new Size(212, 23);
            cbSourceFund.TabIndex = 1;
            cbSourceFund.SelectedIndexChanged += CbSourceFund_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 16);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 0, 0, 16);
            label1.Size = new Size(95, 37);
            label1.TabIndex = 0;
            label1.Text = "Quỹ nguồn";
            // 
            // panel4
            // 
            panel4.Controls.Add(txtNotes);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(dateTimePicker1);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(txtAmount);
            panel4.Controls.Add(label4);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(253, 3);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(16);
            panel4.Size = new Size(244, 286);
            panel4.TabIndex = 3;
            // 
            // txtNotes
            // 
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            txtNotes.Dock = DockStyle.Top;
            txtNotes.Location = new Point(16, 205);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(212, 62);
            txtNotes.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(16, 152);
            label6.Name = "label6";
            label6.Padding = new Padding(0, 16, 0, 16);
            label6.Size = new Size(82, 53);
            label6.TabIndex = 10;
            label6.Text = "Nội dung";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker1.Dock = DockStyle.Top;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(16, 129);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(212, 23);
            dateTimePicker1.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(16, 76);
            label5.Name = "label5";
            label5.Padding = new Padding(0, 16, 0, 16);
            label5.Size = new Size(111, 53);
            label5.TabIndex = 8;
            label5.Text = "Ngày chuyển";
            // 
            // txtAmount
            // 
            txtAmount.BorderStyle = BorderStyle.FixedSingle;
            txtAmount.Dock = DockStyle.Top;
            txtAmount.Location = new Point(16, 53);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(212, 23);
            txtAmount.TabIndex = 7;
            txtAmount.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(16, 16);
            label4.Name = "label4";
            label4.Padding = new Padding(0, 0, 0, 16);
            label4.Size = new Size(123, 37);
            label4.TabIndex = 1;
            label4.Text = "Số tiền chuyển";
            // 
            // panel5
            // 
            panel5.Controls.Add(btnTransfer);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(253, 295);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(16);
            panel5.Size = new Size(244, 171);
            panel5.TabIndex = 4;
            // 
            // btnTransfer
            // 
            btnTransfer.BackColor = Color.FromArgb(46, 125, 50);
            btnTransfer.Dock = DockStyle.Top;
            btnTransfer.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTransfer.ForeColor = Color.White;
            btnTransfer.IconChar = FontAwesome.Sharp.IconChar.None;
            btnTransfer.IconColor = Color.Black;
            btnTransfer.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnTransfer.Location = new Point(16, 16);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(212, 69);
            btnTransfer.TabIndex = 5;
            btnTransfer.Text = "Thực hiện chuyển quỹ";
            btnTransfer.UseVisualStyleBackColor = false;
            btnTransfer.Click += btnTransfer_Click;
            // 
            // frmFundTransfer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(784, 561);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel1);
            ForeColor = Color.FromArgb(51, 51, 51);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmFundTransfer";
            Padding = new Padding(16);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chuyển Quỹ nội bộ";
            Load += FrmFundTransfer_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel3;
        private ComboBox cbDestFund;
        private Label label3;
        private Panel panel2;
        private ComboBox cbSourceFund;
        private Label label1;
        private Panel panel4;
        private TextBox textBox2;
        private Label label4;
        private Panel panel5;
        private FontAwesome.Sharp.IconButton btnTransfer;
        private TextBox txtNotes;
        private Label label6;
        private DateTimePicker dateTimePicker1;
        private Label label5;
        private TextBox txtAmount;
        private Label lblBalanceSourceFund;
        private Label label7;
        private Label lblBalanceDestFund;
        private Label label9;
    }
}