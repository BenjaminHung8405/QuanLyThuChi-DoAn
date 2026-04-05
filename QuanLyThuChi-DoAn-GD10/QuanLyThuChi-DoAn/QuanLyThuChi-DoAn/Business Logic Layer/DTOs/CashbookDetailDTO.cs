namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class CashbookDetailDTO
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public decimal IncomeAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal RunningBalance { get; set; }
        public string CreatorName { get; set; } = string.Empty;
    }
}
