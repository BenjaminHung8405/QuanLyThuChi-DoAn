using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
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

        /// <summary>
        /// Lấy giao dịch theo branch/tenant đang chọn trong session
        /// </summary>
        public List<Transaction> GetTransactionsForCurrentSession(DateTime fromDate, DateTime toDate, string keyword = "")
        {
            if (!SessionManager.CurrentTenantId.HasValue || !SessionManager.CurrentBranchId.HasValue)
            {
                return new List<Transaction>();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;
            int currentBranchId = SessionManager.CurrentBranchId.Value;

            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Partner)
                .Where(t => t.TenantId == currentTenantId)
                .Where(t => t.BranchId == currentBranchId)
                .Where(t => t.TransDate.Date >= fromDate.Date && t.TransDate.Date <= toDate.Date)
                .Where(t => t.Status != "DELETED")
                .Where(t => t.IsActive == true);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string keywordNormalized = keyword.ToLower();
                query = query.Where(t =>
                    (t.Description != null && t.Description.ToLower().Contains(keywordNormalized)) ||
                    (t.Category != null && t.Category.CategoryName.ToLower().Contains(keywordNormalized)) ||
                    (t.Partner != null && t.Partner.PartnerName.ToLower().Contains(keywordNormalized))
                );
            }

            return query.OrderByDescending(t => t.TransDate).ToList();
        }

        /// <summary>
        /// Lấy câu truy vấn Transaction cho session hiện tại với phân quyền tenant/branch
        /// </summary>
        public IQueryable<Transaction> GetTransactionsQueryForCurrentSession()
        {
            if (!SessionManager.CurrentTenantId.HasValue || !SessionManager.CurrentBranchId.HasValue)
            {
                return Enumerable.Empty<Transaction>().AsQueryable();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;
            int currentBranchId = SessionManager.CurrentBranchId.Value;

            return _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Partner)
                .Where(t => t.IsActive == true && t.Status == "COMPLETED")
                .Where(t => t.TenantId == currentTenantId)
                .Where(t => t.BranchId == currentBranchId);
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

        // 3. Thêm mới giao dịch và tự động cập nhật số dư Quỹ
        public void CreateTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            if (transaction.TenantId <= 0)
                throw new ArgumentException("TenantId không hợp lệ.");

            if (transaction.BranchId <= 0)
                throw new ArgumentException("BranchId không hợp lệ.");

            var branch = _context.Branches.FirstOrDefault(b => b.BranchId == transaction.BranchId && b.TenantId == transaction.TenantId && b.IsActive);
            if (branch == null)
                throw new InvalidOperationException("Chi nhánh của giao dịch không hợp lệ.");

            // Bước 1: Tìm quỹ tiền mà giao dịch này đang trỏ tới
            var fund = _context.CashFunds.FirstOrDefault(f => f.FundId == transaction.FundId
                                                            && f.TenantId == transaction.TenantId
                                                            && f.BranchId == transaction.BranchId
                                                            && f.IsActive);
            if (fund == null)
            {
                throw new Exception("Không tìm thấy Quỹ tiền hợp lệ! Vui lòng kiểm tra lại.");
            }

            var category = _context.TransactionCategories.FirstOrDefault(c => c.CategoryId == transaction.CategoryId
                                                                           && c.TenantId == transaction.TenantId
                                                                           && c.BranchId == transaction.BranchId
                                                                           && c.IsActive);
            if (category == null)
                throw new InvalidOperationException("Danh mục giao dịch không hợp lệ cho chi nhánh hiện tại.");

            if (transaction.PartnerId.HasValue)
            {
                var partner = _context.Partners.FirstOrDefault(p => p.PartnerId == transaction.PartnerId.Value
                                                                  && p.TenantId == transaction.TenantId
                                                                  && p.BranchId == transaction.BranchId
                                                                  && p.IsActive);
                if (partner == null)
                    throw new InvalidOperationException("Đối tác không hợp lệ cho chi nhánh hiện tại.");
            }

            // Bước 2: Cập nhật số dư dựa trên loại giao dịch
            if (transaction.TransType == "IN")
            {
                fund.Balance += transaction.Amount;
            }
            else if (transaction.TransType == "OUT")
            {
                if (fund.Balance < transaction.Amount)
                {
                    throw new Exception($"Quỹ '{fund.FundName}' không đủ số dư để thực hiện khoản chi này!");
                }

                fund.Balance -= transaction.Amount;
            }
            else
            {
                throw new Exception("Loại giao dịch không hợp lệ. Vui lòng chọn IN hoặc OUT.");
            }

            // Bước 3: Đưa giao dịch mới vào DB
            _context.Transactions.Add(transaction);

            // Bước 4: Lưu toàn bộ thay đổi (Transactions + CashFunds)
            _context.SaveChanges();
        }

        // 4. Cập nhật giao dịch
        public void UpdateTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            var existing = _context.Transactions.FirstOrDefault(t => t.TransId == transaction.TransId);
            if (existing == null)
                throw new KeyNotFoundException("Không tìm thấy giao dịch cần cập nhật.");

            if (!SessionManager.IsSuperAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue || existing.TenantId != SessionManager.CurrentTenantId.Value)
                    throw new UnauthorizedAccessException("Bạn không có quyền sửa giao dịch ngoài tenant hiện tại.");

                if ((SessionManager.IsBranchManager || SessionManager.IsStaff)
                    && SessionManager.CurrentBranchId.HasValue
                    && existing.BranchId != SessionManager.CurrentBranchId.Value)
                {
                    throw new UnauthorizedAccessException("Bạn không có quyền sửa giao dịch ngoài chi nhánh hiện tại.");
                }
            }

            var category = _context.TransactionCategories.FirstOrDefault(c => c.CategoryId == transaction.CategoryId
                                                                           && c.TenantId == existing.TenantId
                                                                           && c.BranchId == existing.BranchId
                                                                           && c.IsActive);
            if (category == null)
                throw new InvalidOperationException("Danh mục giao dịch không hợp lệ cho chi nhánh hiện tại.");

            if (transaction.PartnerId.HasValue)
            {
                var partner = _context.Partners.FirstOrDefault(p => p.PartnerId == transaction.PartnerId.Value
                                                                  && p.TenantId == existing.TenantId
                                                                  && p.BranchId == existing.BranchId
                                                                  && p.IsActive);
                if (partner == null)
                    throw new InvalidOperationException("Đối tác không hợp lệ cho chi nhánh hiện tại.");
            }

            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        // 5. Xóa mềm giao dịch
        public void DeleteTransaction(long transactionId, int tenantId)
        {
            int scopedTenantId = tenantId;
            if (!SessionManager.IsSuperAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    throw new UnauthorizedAccessException("Không có tenant ngữ cảnh.");

                scopedTenantId = SessionManager.CurrentTenantId.Value;
            }

            var transaction = _context.Transactions
                .FirstOrDefault(t => t.TransId == transactionId && t.TenantId == scopedTenantId);

            if (transaction == null)
                throw new KeyNotFoundException($"Không tìm thấy giao dịch ID {transactionId}");

            if (!SessionManager.IsSuperAdmin
                && (SessionManager.IsBranchManager || SessionManager.IsStaff)
                && SessionManager.CurrentBranchId.HasValue
                && transaction.BranchId != SessionManager.CurrentBranchId.Value)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa giao dịch ngoài chi nhánh hiện tại.");
            }

            transaction.Status = "DELETED";
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }
    }
}
