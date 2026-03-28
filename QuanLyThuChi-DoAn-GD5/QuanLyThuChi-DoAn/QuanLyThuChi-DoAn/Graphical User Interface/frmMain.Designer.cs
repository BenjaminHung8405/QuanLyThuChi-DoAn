namespace QuanLyThuChi_DoAn
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            mnuSystem = new ToolStripMenuItem();
            mnuChangePassword = new ToolStripMenuItem();
            mnuManageUsers = new ToolStripMenuItem();
            mnuBranchConfig = new ToolStripMenuItem();
            mnuLogout = new ToolStripMenuItem();
            mnuCatalog = new ToolStripMenuItem();
            mnuPartners = new ToolStripMenuItem();
            mnuCashFunds = new ToolStripMenuItem();
            mnuTransactionCategory = new ToolStripMenuItem();
            mnuOperations = new ToolStripMenuItem();
            mnuTransaction = new ToolStripMenuItem();
            mnuDebtManagement = new ToolStripMenuItem();
            mnuInternalTransfer = new ToolStripMenuItem();
            mnuReports = new ToolStripMenuItem();
            mnuCashLedger = new ToolStripMenuItem();
            mnuReportByPeriod = new ToolStripMenuItem();
            mnuDebtSummary = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            statusStrip1 = new StatusStrip();
            lblUserStatus = new ToolStripStatusLabel();
            pnlContent = new Panel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.White;
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { mnuSystem, mnuCatalog, mnuOperations, mnuReports });
            menuStrip1.Location = new Point(16, 16);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            menuStrip1.Size = new Size(1168, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // mnuSystem
            // 
            mnuSystem.DropDownItems.AddRange(new ToolStripItem[] { mnuChangePassword, mnuManageUsers, mnuBranchConfig, mnuLogout });
            mnuSystem.Name = "mnuSystem";
            mnuSystem.ShortcutKeys = Keys.Alt | Keys.S;
            mnuSystem.Size = new Size(69, 20);
            mnuSystem.Text = "Hệ thống";
            // 
            // mnuChangePassword
            // 
            mnuChangePassword.Name = "mnuChangePassword";
            mnuChangePassword.Size = new Size(180, 22);
            mnuChangePassword.Text = "Đổi mật khẩu";
            // 
            // mnuManageUsers
            // 
            mnuManageUsers.Name = "mnuManageUsers";
            mnuManageUsers.Size = new Size(180, 22);
            mnuManageUsers.Text = "Quản lý người dùng";
            // 
            // mnuBranchConfig
            // 
            mnuBranchConfig.Name = "mnuBranchConfig";
            mnuBranchConfig.Size = new Size(180, 22);
            mnuBranchConfig.Text = "Cấu hình chi nhánh";
            // 
            // mnuLogout
            // 
            mnuLogout.Name = "mnuLogout";
            mnuLogout.Size = new Size(180, 22);
            mnuLogout.Text = "Đăng xuất / Thoát";
            mnuLogout.Click += menuLogout_Click;
            // 
            // mnuCatalog
            // 
            mnuCatalog.DropDownItems.AddRange(new ToolStripItem[] { mnuPartners, mnuCashFunds, mnuTransactionCategory });
            mnuCatalog.Name = "mnuCatalog";
            mnuCatalog.ShortcutKeys = Keys.Alt | Keys.D;
            mnuCatalog.Size = new Size(74, 20);
            mnuCatalog.Text = "Danh mục";
            // 
            // mnuPartners
            // 
            mnuPartners.Name = "mnuPartners";
            mnuPartners.Size = new Size(240, 22);
            mnuPartners.Text = "Đối tác (Khách hàng/NCC)";
            mnuPartners.Click += menuPartner_Click;
            // 
            // mnuCashFunds
            // 
            mnuCashFunds.Name = "mnuCashFunds";
            mnuCashFunds.Size = new Size(240, 22);
            mnuCashFunds.Text = "Quỹ tiền (Tiền mặt/Ngân hàng)";
            // 
            // mnuTransactionCategory
            // 
            mnuTransactionCategory.Name = "mnuTransactionCategory";
            mnuTransactionCategory.Size = new Size(240, 22);
            mnuTransactionCategory.Text = "Loại thu chi";
            mnuTransactionCategory.Click += mnuTransactionCategory_Click;
            // 
            // mnuOperations
            // 
            mnuOperations.DropDownItems.AddRange(new ToolStripItem[] { mnuTransaction, mnuDebtManagement, mnuInternalTransfer });
            mnuOperations.Name = "mnuOperations";
            mnuOperations.ShortcutKeys = Keys.Alt | Keys.N;
            mnuOperations.Size = new Size(74, 20);
            mnuOperations.Text = "Nghiệp vụ";
            // 
            // mnuTransaction
            // 
            mnuTransaction.Name = "mnuTransaction";
            mnuTransaction.Size = new Size(175, 22);
            mnuTransaction.Text = "Sổ Giao Dịch";
            mnuTransaction.Click += mnuTransaction_Click;
            // 
            // mnuDebtManagement
            // 
            mnuDebtManagement.Name = "mnuDebtManagement";
            mnuDebtManagement.Size = new Size(175, 22);
            mnuDebtManagement.Text = "Quản lý Công nợ";
            // 
            // mnuInternalTransfer
            // 
            mnuInternalTransfer.Name = "mnuInternalTransfer";
            mnuInternalTransfer.Size = new Size(175, 22);
            mnuInternalTransfer.Text = "Chuyển tiền nội bộ";
            // 
            // mnuReports
            // 
            mnuReports.DropDownItems.AddRange(new ToolStripItem[] { mnuCashLedger, mnuReportByPeriod, mnuDebtSummary });
            mnuReports.Name = "mnuReports";
            mnuReports.ShortcutKeys = Keys.Alt | Keys.B;
            mnuReports.Size = new Size(61, 20);
            mnuReports.Text = "Báo cáo";
            // 
            // mnuCashLedger
            // 
            mnuCashLedger.Name = "mnuCashLedger";
            mnuCashLedger.Size = new Size(198, 22);
            mnuCashLedger.Text = "Sổ quỹ chi tiết";
            // 
            // mnuReportByPeriod
            // 
            mnuReportByPeriod.Name = "mnuReportByPeriod";
            mnuReportByPeriod.Size = new Size(198, 22);
            mnuReportByPeriod.Text = "Báo cáo thu chi theo kỳ";
            // 
            // mnuDebtSummary
            // 
            mnuDebtSummary.Name = "mnuDebtSummary";
            mnuDebtSummary.Size = new Size(198, 22);
            mnuDebtSummary.Text = "Tổng hợp công nợ";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.White;
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Location = new Point(16, 40);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0, 0, 2, 0);
            toolStrip1.RenderMode = ToolStripRenderMode.Professional;
            toolStrip1.Size = new Size(1168, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblUserStatus });
            statusStrip1.Location = new Point(16, 702);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.RenderMode = ToolStripRenderMode.Professional;
            statusStrip1.Size = new Size(1168, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblUserStatus
            // 
            lblUserStatus.Name = "lblUserStatus";
            lblUserStatus.Size = new Size(118, 17);
            lblUserStatus.Text = "toolStripStatusLabel1";
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.White;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(16, 65);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1168, 637);
            pnlContent.TabIndex = 3;
            pnlContent.Paint += pnlContent_Paint;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 740);
            Controls.Add(pnlContent);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            ForeColor = Color.FromArgb(51, 51, 51);
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuStrip1;
            Name = "frmMain";
            Padding = new Padding(16);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += frmMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStrip toolStrip1;
        private StatusStrip statusStrip1;
        private Panel pnlContent;
        private ToolStripMenuItem mnuSystem;
        private ToolStripMenuItem mnuChangePassword;
        private ToolStripMenuItem mnuManageUsers;
        private ToolStripMenuItem mnuBranchConfig;
        private ToolStripMenuItem mnuLogout;
        private ToolStripMenuItem mnuCatalog;
        private ToolStripMenuItem mnuPartners;
        private ToolStripMenuItem mnuCashFunds;
        private ToolStripMenuItem mnuTransactionCategory;
        private ToolStripMenuItem mnuOperations;
        private ToolStripMenuItem mnuTransaction;
        private ToolStripMenuItem mnuDebtManagement;
        private ToolStripMenuItem mnuInternalTransfer;
        private ToolStripMenuItem mnuReports;
        private ToolStripMenuItem mnuCashLedger;
        private ToolStripMenuItem mnuReportByPeriod;
        private ToolStripMenuItem mnuDebtSummary;
        private ToolStripStatusLabel lblUserStatus;
    }
}
