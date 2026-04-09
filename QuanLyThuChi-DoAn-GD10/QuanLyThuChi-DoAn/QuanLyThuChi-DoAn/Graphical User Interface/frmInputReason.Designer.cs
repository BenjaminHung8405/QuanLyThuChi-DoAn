namespace QuanLyThuChi_DoAn.Graphical_User_Interface
{
    partial class frmInputReason
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
            lblPrompt = new Label();
            txtReason = new TextBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new Point(20, 20);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(173, 15);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "Vui lòng nhập lý do hủy phiếu:";
            // 
            // txtReason
            // 
            txtReason.Location = new Point(20, 44);
            txtReason.Multiline = true;
            txtReason.Name = "txtReason";
            txtReason.ScrollBars = ScrollBars.Vertical;
            txtReason.Size = new Size(310, 92);
            txtReason.TabIndex = 1;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(156, 150);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(84, 30);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Xác nhận";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(246, 150);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(84, 30);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Hủy bỏ";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmInputReason
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(350, 200);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(txtReason);
            Controls.Add(lblPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmInputReason";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nhập lý do hủy";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPrompt;
        private TextBox txtReason;
        private Button btnConfirm;
        private Button btnCancel;
    }
}