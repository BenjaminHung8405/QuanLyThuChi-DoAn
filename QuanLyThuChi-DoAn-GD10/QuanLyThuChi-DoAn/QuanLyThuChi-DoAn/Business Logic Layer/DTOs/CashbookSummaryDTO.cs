namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class CashbookSummaryDTO
    {
        public string TransactionType { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
