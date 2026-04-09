using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using System.Text.Json;

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
            if (!SessionManager.CurrentTenantId.HasValue)
            {
                return new List<Transaction>();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;
            int currentBranchId = SessionManager.CurrentBranchId ?? 0;

            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Partner)
                .Where(t => t.TenantId == currentTenantId)
                .Where(t => t.TransDate.Date >= fromDate.Date && t.TransDate.Date <= toDate.Date)
                .Where(t => t.Status != "DELETED")
                .Where(t => t.IsActive == true);

            if (currentBranchId > 0)
            {
                query = query.Where(t => t.BranchId == currentBranchId);
            }
            else if (SessionManager.IsBranchManager || SessionManager.IsStaff)
            {
                // Role theo chi nhánh bắt buộc phải có branch cụ thể.
                return new List<Transaction>();
            }

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
            if (!SessionManager.CurrentTenantId.HasValue)
            {
                return Enumerable.Empty<Transaction>().AsQueryable();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;
            int currentBranchId = SessionManager.CurrentBranchId ?? 0;

            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Partner)
                .Where(t => t.IsActive == true && t.Status == "COMPLETED")
                .Where(t => t.TenantId == currentTenantId);

            if (currentBranchId > 0)
            {
                query = query.Where(t => t.BranchId == currentBranchId);
            }
            else if (SessionManager.IsBranchManager || SessionManager.IsStaff)
            {
                // Role theo chi nhánh bắt buộc phải có branch cụ thể.
                return Enumerable.Empty<Transaction>().AsQueryable();
            }

            return query;
        }

        /// <summary>
        /// Lấy danh sách mức thuế còn hiệu lực theo tenant đang đăng nhập.
        /// </summary>
        public List<Tax> GetTaxesForCurrentSession()
        {
            if (!SessionManager.CurrentTenantId.HasValue)
            {
                return new List<Tax>();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;

            return _context.Taxes
                .AsNoTracking()
                .Where(t => t.TenantId == currentTenantId)
                .Where(t => t.IsActive)
                .OrderBy(t => t.Rate)
                .ThenBy(t => t.TaxName)
                .ToList();
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

        // 3. Thêm mới giao dịch (async) và tự động tính thuế/tổng tiền
        public async Task<bool> AddTransactionAsync(Transaction trans)
        {
            if (trans == null)
                throw new ArgumentNullException(nameof(trans));

            ApplySessionScope(trans);
            await ApplyTaxCalculationAsync(trans);

            if (trans.Amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            if (trans.TenantId <= 0)
                throw new ArgumentException("TenantId không hợp lệ.");

            if (trans.BranchId <= 0)
                throw new ArgumentException("BranchId không hợp lệ.");

            var branch = await _context.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BranchId == trans.BranchId
                                       && b.TenantId == trans.TenantId
                                       && b.IsActive);
            if (branch == null)
                throw new InvalidOperationException("Chi nhánh của giao dịch không hợp lệ.");

            var fund = await _context.CashFunds.FirstOrDefaultAsync(f => f.FundId == trans.FundId
                                                                       && f.TenantId == trans.TenantId
                                                                       && f.BranchId == trans.BranchId
                                                                       && f.IsActive);
            if (fund == null)
                throw new Exception("Không tìm thấy Quỹ tiền hợp lệ! Vui lòng kiểm tra lại.");

            var category = await _context.TransactionCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == trans.CategoryId
                                       && c.TenantId == trans.TenantId
                                       && c.BranchId == trans.BranchId
                                       && c.IsActive);
            if (category == null)
                throw new InvalidOperationException("Danh mục giao dịch không hợp lệ cho chi nhánh hiện tại.");

            if (trans.PartnerId.HasValue)
            {
                var partner = await _context.Partners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PartnerId == trans.PartnerId.Value
                                           && p.TenantId == trans.TenantId
                                           && p.BranchId == trans.BranchId
                                           && p.IsActive);
                if (partner == null)
                    throw new InvalidOperationException("Đối tác không hợp lệ cho chi nhánh hiện tại.");
            }

            string transType = trans.TransType?.Trim().ToUpperInvariant() ?? string.Empty;
            if (transType == "IN")
            {
                fund.Balance += trans.Amount;
            }
            else if (transType == "OUT")
            {
                if (fund.Balance < trans.Amount)
                    throw new Exception($"Quỹ '{fund.FundName}' không đủ số dư để thực hiện khoản chi này!");

                fund.Balance -= trans.Amount;
            }
            else
            {
                throw new Exception("Loại giao dịch không hợp lệ. Vui lòng chọn IN hoặc OUT.");
            }

            trans.TransType = transType;

            _context.Transactions.Add(trans);
            return await _context.SaveChangesAsync() > 0;
        }

        // 4. Thêm mới giao dịch và tự động cập nhật số dư Quỹ
        public void CreateTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            ApplySessionScope(transaction);
            ApplyTaxCalculation(transaction);

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

        // 5. Cập nhật giao dịch
        public void UpdateTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

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

            transaction.TenantId = existing.TenantId;
            transaction.BranchId = existing.BranchId;
            ApplyTaxCalculation(transaction);

            if (transaction.Amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        // 6. Xóa mềm giao dịch
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

        /// <summary>
        /// Hủy giao dịch và ghi AuditLog trong cùng một SaveChangesAsync.
        /// Nếu một trong hai thao tác thất bại, EF Core sẽ rollback toàn bộ.
        /// </summary>
        public async Task<bool> CancelTransactionAsync(long transactionId, string cancelReason)
        {
            if (transactionId <= 0)
                throw new ArgumentException("Mã giao dịch không hợp lệ.", nameof(transactionId));

            string reason = cancelReason?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Vui lòng nhập lý do hủy giao dịch.", nameof(cancelReason));

            var query = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.TransId == transactionId);

            if (!SessionManager.IsSuperAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    throw new UnauthorizedAccessException("Không có tenant ngữ cảnh.");

                int currentTenantId = SessionManager.CurrentTenantId.Value;
                query = query.Where(t => t.TenantId == currentTenantId);

                if (SessionManager.IsBranchManager || SessionManager.IsStaff)
                {
                    if (!SessionManager.CurrentBranchId.HasValue)
                        throw new UnauthorizedAccessException("Không có chi nhánh ngữ cảnh.");

                    int currentBranchId = SessionManager.CurrentBranchId.Value;
                    query = query.Where(t => t.BranchId == currentBranchId);
                }
            }

            var transaction = await query.FirstOrDefaultAsync();
            if (transaction == null)
                throw new KeyNotFoundException($"Không tìm thấy giao dịch ID {transactionId} trong phạm vi được phép.");

            if (!transaction.IsActive
                || string.Equals(transaction.Status, "CANCELLED", StringComparison.OrdinalIgnoreCase)
                || string.Equals(transaction.Status, "DELETED", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Giao dịch này đã bị hủy từ trước.");
            }

            // Snapshot tối thiểu để tránh lỗi circular reference khi serialize entity đầy đủ.
            var oldData = new
            {
                transaction.TransId,
                transaction.TenantId,
                transaction.BranchId,
                transaction.FundId,
                transaction.CategoryId,
                CategoryName = transaction.Category?.CategoryName ?? "Khác",
                transaction.PartnerId,
                transaction.TransDate,
                transaction.SubTotal,
                transaction.TaxAmount,
                transaction.Amount,
                transaction.TransType,
                transaction.Description,
                transaction.RefNo,
                transaction.Status,
                transaction.IsActive
            };

            transaction.IsActive = false;
            transaction.Status = "CANCELLED";

            var auditLog = new AuditLog
            {
                TenantId = transaction.TenantId,
                UserId = SessionManager.CurrentUserId > 0 ? SessionManager.CurrentUserId : null,
                UserName = string.IsNullOrWhiteSpace(SessionManager.Username) ? "system" : SessionManager.Username,
                ActionType = "VOID_TRANSACTION",
                TableName = "Transactions",
                RecordId = transaction.TransId.ToString(),
                OldValues = JsonSerializer.Serialize(oldData),
                NewValues = JsonSerializer.Serialize(new
                {
                    IsActive = false,
                    Status = transaction.Status,
                    Reason = reason
                }),
                ActionDate = DateTime.Now
            };

            _context.AuditLogs.Add(auditLog);

            return await _context.SaveChangesAsync() > 0;
        }

        private void ApplyTaxCalculation(Transaction transaction)
        {
            NormalizeSubTotal(transaction);

            if (transaction.TaxId.HasValue && transaction.TaxId.Value > 0)
            {
                var tax = _context.Taxes
                    .AsNoTracking()
                    .FirstOrDefault(t => t.TaxId == transaction.TaxId.Value
                                      && t.TenantId == transaction.TenantId
                                      && t.IsActive);

                if (tax == null)
                    throw new InvalidOperationException("Mức thuế không hợp lệ cho tenant hiện tại.");

                ApplyTaxAmounts(transaction, tax.Rate);
                return;
            }

            transaction.TaxAmount = 0;
            transaction.Amount = transaction.SubTotal;
        }

        private async Task ApplyTaxCalculationAsync(Transaction transaction)
        {
            NormalizeSubTotal(transaction);

            if (transaction.TaxId.HasValue && transaction.TaxId.Value > 0)
            {
                var tax = await _context.Taxes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TaxId == transaction.TaxId.Value
                                           && t.TenantId == transaction.TenantId
                                           && t.IsActive);

                if (tax == null)
                    throw new InvalidOperationException("Mức thuế không hợp lệ cho tenant hiện tại.");

                ApplyTaxAmounts(transaction, tax.Rate);
                return;
            }

            transaction.TaxAmount = 0;
            transaction.Amount = transaction.SubTotal;
        }

        private static void NormalizeSubTotal(Transaction transaction)
        {
            if (transaction.SubTotal <= 0 && transaction.Amount > 0)
            {
                // Backward-compatible: nếu UI cũ chỉ gửi Amount thì xem như SubTotal.
                transaction.SubTotal = transaction.Amount;
            }

            if (transaction.SubTotal <= 0)
                throw new ArgumentException("Tiền trước thuế phải lớn hơn 0.");
        }

        private static void ApplyTaxAmounts(Transaction transaction, decimal rate)
        {
            transaction.TaxAmount = Math.Round(
                transaction.SubTotal * (rate / 100m),
                0,
                MidpointRounding.AwayFromZero);

            transaction.Amount = transaction.SubTotal + transaction.TaxAmount;
        }

        private static void ApplySessionScope(Transaction transaction)
        {
            if (SessionManager.CurrentTenantId.HasValue)
            {
                int sessionTenantId = SessionManager.CurrentTenantId.Value;

                if (transaction.TenantId <= 0)
                {
                    transaction.TenantId = sessionTenantId;
                }
                else if (!SessionManager.IsSuperAdmin && transaction.TenantId != sessionTenantId)
                {
                    throw new UnauthorizedAccessException("Bạn không có quyền tạo giao dịch ngoài tenant hiện tại.");
                }
            }

            if (SessionManager.CurrentBranchId.HasValue)
            {
                int sessionBranchId = SessionManager.CurrentBranchId.Value;

                if (transaction.BranchId <= 0)
                {
                    transaction.BranchId = sessionBranchId;
                }
                else if ((SessionManager.IsBranchManager || SessionManager.IsStaff)
                         && transaction.BranchId != sessionBranchId)
                {
                    throw new UnauthorizedAccessException("Bạn không có quyền tạo giao dịch ngoài chi nhánh hiện tại.");
                }
            }
        }
    }
}
