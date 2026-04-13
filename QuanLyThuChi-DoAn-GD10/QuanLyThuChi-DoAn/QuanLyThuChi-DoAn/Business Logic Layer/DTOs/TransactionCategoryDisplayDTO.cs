using System;

namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class TransactionCategoryDisplayDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Type { get; set; } = "IN"; // 'IN' hoặc 'OUT'
        public bool IsActive { get; set; } = true;
        
        // Statistics
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
