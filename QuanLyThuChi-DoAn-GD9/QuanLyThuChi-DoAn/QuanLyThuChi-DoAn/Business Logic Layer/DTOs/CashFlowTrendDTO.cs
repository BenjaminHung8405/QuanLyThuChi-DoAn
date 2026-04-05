namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class CashFlowTrendDTO
    {
        public DateTime Date { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetCashFlow => TotalIncome - TotalExpense;
    }
}
