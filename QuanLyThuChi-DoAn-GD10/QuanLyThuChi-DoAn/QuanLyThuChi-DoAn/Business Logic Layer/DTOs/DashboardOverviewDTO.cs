namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class DashboardOverviewDTO
    {
        // Revenue section
        public decimal GrossIncome { get; set; }
        public decimal NetIncome { get; set; }
        public decimal OutputVAT { get; set; }

        // Expense section
        public decimal GrossExpense { get; set; }
        public decimal NetExpense { get; set; }
        public decimal InputVAT { get; set; }

        // Management metrics
        public decimal NetProfit => NetIncome - NetExpense;
        public decimal EstimatedTaxPayable => OutputVAT - InputVAT;

        // Backward-compatible aliases for existing dashboard/service bindings
        public decimal TotalIncome
        {
            get => GrossIncome;
            set => GrossIncome = value;
        }

        public decimal TotalExpense
        {
            get => GrossExpense;
            set => GrossExpense = value;
        }

        public decimal CurrentBalance => GrossIncome - GrossExpense;
        public decimal TotalReceivable { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal NetWorth => CurrentBalance + TotalReceivable - TotalPayable;
    }
}
