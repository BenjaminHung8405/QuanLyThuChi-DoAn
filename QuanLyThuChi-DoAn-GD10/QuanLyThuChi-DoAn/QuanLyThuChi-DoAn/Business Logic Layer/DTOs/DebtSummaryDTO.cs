namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class DebtSummaryDTO
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; } = string.Empty;
        public string DebtType { get; set; } = string.Empty;
        public int TotalVouchers { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingDebt => TotalAmount - TotalPaid;
    }
}
