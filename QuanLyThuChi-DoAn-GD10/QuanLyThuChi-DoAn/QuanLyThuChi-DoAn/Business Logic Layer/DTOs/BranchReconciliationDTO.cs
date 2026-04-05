using System;

namespace QuanLyThuChi_DoAn.BLL.DTOs
{
    public class BranchReconciliationDTO
    {
        /// <summary>
        /// Tên chi nhánh
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Tổng tiền thu vào (Doanh thu)
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// Tổng tiền chi ra (Chi phí)
        /// </summary>
        public decimal TotalExpense { get; set; }

        /// <summary>
        /// Tổng số lượng giao dịch (Thu + Chi) đã thực hiện
        /// Giúp Giám đốc nhận biết chi nhánh nào có nhiều hóa đơn lẻ tẻ
        /// </summary>
        public int TransactionCount { get; set; }

        /// <summary>
        /// Lợi nhuận ròng (Net Revenue) = TotalIncome - TotalExpense
        /// Computed Property để UI tự động tính toán
        /// </summary>
        public decimal NetRevenue => TotalIncome - TotalExpense;

        /// <summary>
        /// Trạng thái hiệu quả: "Lãi" nếu NetRevenue >= 0, ngược lại "Lỗ"
        /// Sử dụng để hiển thị trực quan trên báo cáo Excel
        /// </summary>
        public string PerformanceStatus => NetRevenue >= 0 ? "Lãi" : "Lỗ";
    }
}
