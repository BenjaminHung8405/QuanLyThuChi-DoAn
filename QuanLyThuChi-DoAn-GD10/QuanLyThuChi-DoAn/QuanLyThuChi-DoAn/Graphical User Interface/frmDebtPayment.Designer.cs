namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class frmDebtPayment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.grpDebtInfo = new System.Windows.Forms.GroupBox();
            this.txtRemainingAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPaidAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.linkPayAll = new System.Windows.Forms.LinkLabel();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.dtpPaymentDate = new System.Windows.Forms.DateTimePicker();
            this.cboFund = new System.Windows.Forms.ComboBox();
            this.nudAmount = new System.Windows.Forms.NumericUpDown();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblFund = new System.Windows.Forms.Label();
            this.lblAmountToPay = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlHeader.SuspendLayout();
            this.grpDebtInfo.SuspendLayout();
            this.grpInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).BeginInit();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // frmDebtPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 550);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "frmDebtPayment";
            this.Text = "Thanh toán công nợ";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 90;
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(25, 118, 210);
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(12);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Controls.Add(this.lblPartnerName);
            this.Controls.Add(this.pnlHeader);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(16, 12);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(260, 25);
            this.lblHeader.Text = "THANH TOÁN CÔNG NỢ";
            // 
            // lblPartnerName
            // 
            this.lblPartnerName.AutoSize = true;
            this.lblPartnerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPartnerName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPartnerName.Location = new System.Drawing.Point(18, 46);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(100, 15);
            this.lblPartnerName.Text = "[Tên đối tác]";
            // 
            // grpDebtInfo
            // 
            this.grpDebtInfo.Location = new System.Drawing.Point(12, 104);
            this.grpDebtInfo.Name = "grpDebtInfo";
            this.grpDebtInfo.Size = new System.Drawing.Size(426, 120);
            this.grpDebtInfo.Text = "Thông tin khoản nợ";
            this.grpDebtInfo.BackColor = System.Drawing.SystemColors.Control;
            // 
            // txtRemainingAmount
            // 
            this.txtRemainingAmount.Location = new System.Drawing.Point(140, 78);
            this.txtRemainingAmount.Name = "txtRemainingAmount";
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.BackColor = System.Drawing.Color.LightGray;
            this.txtRemainingAmount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtRemainingAmount.Size = new System.Drawing.Size(260, 27);
            this.txtRemainingAmount.TabIndex = 5;
            this.txtRemainingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 15);
            this.label3.Text = "Còn lại (Dư nợ):";
            // 
            // txtPaidAmount
            // 
            this.txtPaidAmount.Location = new System.Drawing.Point(140, 45);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.ReadOnly = true;
            this.txtPaidAmount.BackColor = System.Drawing.Color.LightGray;
            this.txtPaidAmount.Size = new System.Drawing.Size(260, 23);
            this.txtPaidAmount.TabIndex = 3;
            this.txtPaidAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.Text = "Đã thanh toán:";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(140, 16);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.BackColor = System.Drawing.Color.LightGray;
            this.txtTotalAmount.Size = new System.Drawing.Size(260, 23);
            this.txtTotalAmount.TabIndex = 1;
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.Text = "Tổng nợ:";
            // 
            // add controls to grpDebtInfo
            // 
            this.grpDebtInfo.Controls.Add(this.txtRemainingAmount);
            this.grpDebtInfo.Controls.Add(this.label3);
            this.grpDebtInfo.Controls.Add(this.txtPaidAmount);
            this.grpDebtInfo.Controls.Add(this.label2);
            this.grpDebtInfo.Controls.Add(this.txtTotalAmount);
            this.grpDebtInfo.Controls.Add(this.label1);
            this.Controls.Add(this.grpDebtInfo);
            // 
            // grpInput
            // 
            this.grpInput.Location = new System.Drawing.Point(12, 236);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(426, 220);
            this.grpInput.Text = "Nhập thông tin thanh toán";
            // 
            // linkPayAll
            // 
            this.linkPayAll.AutoSize = true;
            this.linkPayAll.Location = new System.Drawing.Point(360, 30);
            this.linkPayAll.Name = "linkPayAll";
            this.linkPayAll.Size = new System.Drawing.Size(40, 15);
            this.linkPayAll.TabIndex = 1;
            this.linkPayAll.TabStop = true;
            this.linkPayAll.Text = "Trả hết";
            this.linkPayAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPayAll_LinkClicked);
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(140, 108);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(260, 96);
            this.txtNote.TabIndex = 4;
            // 
            // dtpPaymentDate
            // 
            this.dtpPaymentDate.Location = new System.Drawing.Point(140, 78);
            this.dtpPaymentDate.Name = "dtpPaymentDate";
            this.dtpPaymentDate.Size = new System.Drawing.Size(260, 23);
            this.dtpPaymentDate.TabIndex = 3;
            // 
            // cboFund
            // 
            this.cboFund.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFund.Location = new System.Drawing.Point(140, 46);
            this.cboFund.Name = "cboFund";
            this.cboFund.Size = new System.Drawing.Size(200, 23);
            this.cboFund.TabIndex = 2;
            // 
            // nudAmount
            // 
            this.nudAmount.Location = new System.Drawing.Point(140, 18);
            this.nudAmount.Name = "nudAmount";
            this.nudAmount.Size = new System.Drawing.Size(200, 23);
            this.nudAmount.TabIndex = 0;
            this.nudAmount.Maximum = new decimal(new int[] {
            -1539607552,
            465,
            0,
            0});
            this.nudAmount.DecimalPlaces = 0;
            this.nudAmount.ThousandsSeparator = true;
            this.nudAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.nudAmount.ValueChanged += new System.EventHandler(this.nudAmount_ValueChanged);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(18, 110);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(36, 15);
            this.lblNote.Text = "Ghi chú:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(18, 82);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(54, 15);
            this.lblDate.Text = "Ngày trả:";
            // 
            // lblFund
            // 
            this.lblFund.AutoSize = true;
            this.lblFund.Location = new System.Drawing.Point(18, 50);
            this.lblFund.Name = "lblFund";
            this.lblFund.Size = new System.Drawing.Size(40, 15);
            this.lblFund.Text = "Quỹ:";
            // 
            // lblAmountToPay
            // 
            this.lblAmountToPay.AutoSize = true;
            this.lblAmountToPay.Location = new System.Drawing.Point(18, 22);
            this.lblAmountToPay.Name = "lblAmountToPay";
            this.lblAmountToPay.Size = new System.Drawing.Size(72, 15);
            this.lblAmountToPay.Text = "Số tiền trả:";
            // 
            // add controls to grpInput
            // 
            this.grpInput.Controls.Add(this.linkPayAll);
            this.grpInput.Controls.Add(this.txtNote);
            this.grpInput.Controls.Add(this.dtpPaymentDate);
            this.grpInput.Controls.Add(this.cboFund);
            this.grpInput.Controls.Add(this.nudAmount);
            this.grpInput.Controls.Add(this.lblNote);
            this.grpInput.Controls.Add(this.lblDate);
            this.grpInput.Controls.Add(this.lblFund);
            this.grpInput.Controls.Add(this.lblAmountToPay);
            this.Controls.Add(this.grpInput);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 64;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(12);
            this.pnlFooter.Controls.Add(this.btnConfirm);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pnlFooter);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(238, 15);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 34);
            this.btnConfirm.Text = "Xác nhận";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Location = new System.Drawing.Point(338, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 34);
            this.btnCancel.Text = "Hủy bỏ";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // finalize
            // 
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpDebtInfo.ResumeLayout(false);
            this.grpDebtInfo.PerformLayout();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.GroupBox grpDebtInfo;
        private System.Windows.Forms.TextBox txtRemainingAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.LinkLabel linkPayAll;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.DateTimePicker dtpPaymentDate;
        private System.Windows.Forms.ComboBox cboFund;
        private System.Windows.Forms.NumericUpDown nudAmount;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblFund;
        private System.Windows.Forms.Label lblAmountToPay;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}