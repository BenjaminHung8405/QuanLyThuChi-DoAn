using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách giao dịch (Có lọc theo ngày và từ khóa)
        public List<Transaction> GetTransactions(int tenantId, DateTime fromDate, DateTime toDate, string keyword = "")
        {
            var query = _context.Transactions
                .Include(t => t.Category) // Kéo theo dữ liệu bảng Category
                .Include(t => t.Partner)  // Kéo theo dữ liệu bảng Partner
                .Where(t => t.TenantId == tenantId)  // ✅ Filter by tenant ID
                .Where(t => t.TransDate.Date >= fromDate.Date && t.TransDate.Date <= toDate.Date)
                .Where(t => t.Status != "DELETED") // ✅ Exclude soft-deleted transactions
                .Where(t => t.IsActive == true);   // ✅ Only show active transactions

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string keywordNormalized = keyword.ToLower(); // Nên dùng hàm RemoveVietnameseAccents của bạn ở UI
                query = query.Where(t =>
                    (t.Description != null && t.Description.ToLower().Contains(keywordNormalized)) ||
                    (t.Category != null && t.Category.CategoryName.ToLower().Contains(keywordNormalized)) ||
                    (t.Partner != null && t.Partner.PartnerName.ToLower().Contains(keywordNormalized))
                );
            }

            return query.OrderByDescending(t => t.TransDate).ToList(); // Mới nhất xếp trên cùng
        }

        // 2. Thống kê tổng Thu / Chi
        public (decimal TotalIn, decimal TotalOut) GetSummary(int tenantId, DateTime fromDate, DateTime toDate)
        {
            var transactions = _context.Transactions
                .Where(t => t.TenantId == tenantId)
                .Where(t => t.TransDate.Date >= fromDate.Date && t.TransDate.Date <= toDate.Date)
                .ToList();

            decimal totalIn = transactions.Where(t => t.TransType == "IN").Sum(t => t.Amount);
            decimal totalOut = transactions.Where(t => t.TransType == "OUT").Sum(t => t.Amount);

            return (totalIn, totalOut);
        }

        // 3. Thêm mới giao dịch
        public void CreateTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        // 4. Cập nhật giao dịch
        public void UpdateTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        // 5. Xóa mềm giao dịch
        public void DeleteTransaction(long transactionId, int tenantId)
        {
            var transaction = _context.Transactions
                .FirstOrDefault(t => t.TransId == transactionId && t.TenantId == tenantId);

            if (transaction == null)
                throw new KeyNotFoundException($"Không tìm thấy giao dịch ID {transactionId}");

            transaction.Status = "DELETED";
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }
    }
}
