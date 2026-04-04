namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class DashboardOverviewDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal CurrentBalance => TotalIncome - TotalExpense;
        public decimal TotalReceivable { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal NetWorth => CurrentBalance + TotalReceivable - TotalPayable;
    }
}
