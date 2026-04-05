using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class DebtService
    {
        private readonly AppDbContext _context;

        public DebtService()
        {
            _context = new AppDbContext();
        }

        public DebtService(AppDbContext context)
        {
            _context = context ?? new AppDbContext();
        }

        // Lấy danh sách khoản nợ theo lọc đơn giản
        public List<Debt> GetDebts(string type = null, string status = null, int? partnerId = null)
        {
            var query = _context.Debts
                .AsNoTracking()
                .Include(d => d.Partner)
                .AsQueryable();

            if (SessionManager.IsSuperAdmin)
            {
                if (SessionManager.CurrentTenantId.HasValue)
                {
                    int selectedTenantId = SessionManager.CurrentTenantId.Value;
                    query = query.Where(d => d.TenantId == selectedTenantId);
                }

                // Nếu BranchId > 0 (chi nhánh cụ thể) -> Lọc theo chi nhánh
                // Nếu BranchId == 0 (Tất cả chi nhánh) -> Bỏ qua điều kiện, query lấy toàn bộ tenant
                if (SessionManager.CurrentBranchId.HasValue && SessionManager.CurrentBranchId.Value > 0)
                {
                    int selectedBranchId = SessionManager.CurrentBranchId.Value;
                    query = query.Where(d => d.BranchId == selectedBranchId);
                }
            }
            else
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    return new List<Debt>();

                int tenantId = SessionManager.CurrentTenantId.Value;
                query = query.Where(d => d.TenantId == tenantId);

                if (SessionManager.IsBranchManager || SessionManager.IsStaff)
                {
                    if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                        return new List<Debt>();

                    int currentBranchId = SessionManager.CurrentBranchId.Value;
                    query = query.Where(d => d.BranchId == currentBranchId);
                }
            }

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(d => d.DebtType == type);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(d => d.Status == status);

            if (partnerId.HasValue)
                query = query.Where(d => d.PartnerId == partnerId.Value);

            return query.OrderByDescending(d => d.DebtId).ToList();
        }

        /// <summary>
        /// Lấy danh sách công nợ bất đồng bộ theo Tenant và Branch
        /// Nếu branchId == 0, sẽ lấy toàn bộ công nợ của Tenant (chế độ "Tất cả chi nhánh")
        /// Nếu branchId > 0, sẽ lọc theo chi nhánh cụ thể
        /// </summary>
        public async Task<List<Debt>> GetDebtsAsync(int tenantId, int? branchId)
        {
            var query = _context.Debts
                .AsNoTracking()
                .Where(d => d.TenantId == tenantId && d.IsActive)
                .AsQueryable();

            // Nếu BranchId > 0 (chi nhánh cụ thể) -> Lọc theo chi nhánh
            // Nếu BranchId == 0 hoặc null (Tất cả chi nhánh) -> Bỏ qua điều kiện, query lấy toàn bộ tenant
            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(d => d.BranchId == branchId.Value);
            }

            return await query
                .Include(d => d.Partner)
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
        }

        // Lấy khoản nợ theo ID (bao gồm Partner)
        public Debt GetDebtById(long debtId)
        {
            var query = _context.Debts.AsNoTracking().Include(d => d.Partner).Where(d => d.DebtId == debtId);

            if (SessionManager.IsSuperAdmin)
            {
                if (SessionManager.CurrentTenantId.HasValue)
                {
                    int selectedTenantId = SessionManager.CurrentTenantId.Value;
                    query = query.Where(d => d.TenantId == selectedTenantId);
                }

                if (SessionManager.CurrentBranchId.HasValue && SessionManager.CurrentBranchId.Value > 0)
                {
                    int selectedBranchId = SessionManager.CurrentBranchId.Value;
                    query = query.Where(d => d.BranchId == selectedBranchId);
                }

                return query.FirstOrDefault();
            }

            if (!SessionManager.CurrentTenantId.HasValue)
                return null;

            int tenantId = SessionManager.CurrentTenantId.Value;
            query = query.Where(d => d.TenantId == tenantId);

            if (SessionManager.IsBranchManager || SessionManager.IsStaff)
            {
                if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                    return null;

                int currentBranchId = SessionManager.CurrentBranchId.Value;
                query = query.Where(d => d.BranchId == currentBranchId);
            }

            return query.FirstOrDefault();
        }

        public bool AddDebt(int partnerId, string debtType, decimal totalAmount, DateTime? dueDate, string notes)
        {
            try
            {
                var newDebt = new Debt
                {
                    TenantId = SessionManager.CurrentTenantId ?? 0,
                    BranchId = SessionManager.CurrentBranchId ?? 0,
                    PartnerId = partnerId,
                    DebtType = string.IsNullOrWhiteSpace(debtType) ? "PAYABLE" : debtType.ToUpperInvariant(),
                    TotalAmount = totalAmount,
                    PaidAmount = 0,
                    DueDate = dueDate,
                    Status = "NEW",
                    Notes = notes ?? string.Empty,
                    CreatedDate = DateTime.Now
                };

                _context.Debts.Add(newDebt);
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm công nợ: " + ex.Message);
            }
        }

        public async Task<bool> ApproveDebtAsync(long debtId)
        {
            try
            {
                if (!SessionManager.CanApproveDebt)
                    throw new UnauthorizedAccessException("Bạn không có quyền duyệt công nợ.");

                IQueryable<Debt> query = _context.Debts;

                if (SessionManager.IsSuperAdmin)
                {
                    if (SessionManager.CurrentTenantId.HasValue && SessionManager.CurrentTenantId.Value > 0)
                    {
                        int selectedTenantId = SessionManager.CurrentTenantId.Value;
                        query = query.Where(d => d.TenantId == selectedTenantId);
                    }

                    if (SessionManager.CurrentBranchId.HasValue && SessionManager.CurrentBranchId.Value > 0)
                    {
                        int selectedBranchId = SessionManager.CurrentBranchId.Value;
                        query = query.Where(d => d.BranchId == selectedBranchId);
                    }
                }
                else
                {
                    if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
                        throw new InvalidOperationException("Không có Tenant trong ngữ cảnh.");

                    int tenantId = SessionManager.CurrentTenantId.Value;
                    query = query.Where(d => d.TenantId == tenantId);

                    if (SessionManager.IsBranchManager || SessionManager.IsStaff)
                    {
                        if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                            throw new InvalidOperationException("Không có Chi nhánh trong ngữ cảnh.");

                        int currentBranchId = SessionManager.CurrentBranchId.Value;
                        query = query.Where(d => d.BranchId == currentBranchId);
                    }
                }

                var debt = await query.FirstOrDefaultAsync(d => d.DebtId == debtId && d.IsActive);
                if (debt == null) return false;

                // Chỉ cho phép duyệt nếu đang ở trạng thái NEW
                if (!string.Equals(debt.Status, "NEW", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Chỉ có thể duyệt khoản nợ ở trạng thái MỚI TẠO (NEW).");

                debt.Status = "PENDING";

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi duyệt công nợ: " + ex.Message);
            }
        }

        /// <summary>
        /// Thực hiện trả nợ: tạo Transaction, cập nhật số dư Quỹ và cập nhật PaidAmount trong bảng Debts.
        /// Sử dụng IDbContextTransaction để đảm bảo atomic.
        /// </summary>
        public bool PayDebt(long debtId, int fundId, decimal amount, string note)
        {
            if (!SessionManager.CanApproveDebt)
                throw new UnauthorizedAccessException("Bạn không có quyền duyệt thanh toán công nợ.");

            using (var tx = _context.Database.BeginTransaction())
            {
                try
                {
                    Debt debt;
                    if (SessionManager.IsSuperAdmin)
                    {
                        debt = _context.Debts.FirstOrDefault(d => d.DebtId == debtId);
                    }
                    else
                    {
                        if (!SessionManager.CurrentTenantId.HasValue)
                            throw new InvalidOperationException("Không có Tenant trong ngữ cảnh.");

                        int currentTenantId = SessionManager.CurrentTenantId.Value;
                        var debtQuery = _context.Debts.Where(d => d.DebtId == debtId && d.TenantId == currentTenantId);

                        if (SessionManager.IsBranchManager || SessionManager.IsStaff)
                        {
                            if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                                throw new InvalidOperationException("Không có Chi nhánh trong ngữ cảnh.");

                            int currentBranchId = SessionManager.CurrentBranchId.Value;
                            debtQuery = debtQuery.Where(d => d.BranchId == currentBranchId);
                        }

                        debt = debtQuery.FirstOrDefault();
                    }

                    if (debt == null)
                        throw new KeyNotFoundException("Không tìm thấy khoản nợ.");

                    int tenantId = debt.TenantId;

                    decimal remaining = debt.TotalAmount - debt.PaidAmount;
                    if (amount <= 0) throw new ArgumentException("Số tiền phải lớn hơn 0.");
                    if (amount > remaining) throw new InvalidOperationException("Không thể trả quá số nợ hiện có!");

                    var fundQuery = _context.CashFunds.Where(f => f.FundId == fundId && f.TenantId == tenantId && f.IsActive);

                    if ((SessionManager.IsBranchManager || SessionManager.IsStaff) && SessionManager.CurrentBranchId.HasValue)
                    {
                        int currentBranchId = SessionManager.CurrentBranchId.Value;
                        fundQuery = fundQuery.Where(f => f.BranchId == currentBranchId);
                    }

                    var fund = fundQuery.FirstOrDefault();
                    if (fund == null) throw new KeyNotFoundException("Quỹ thanh toán không hợp lệ hoặc không hoạt động.");

                    // Determine transaction direction
                    string transType = string.Equals(debt.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase) ? "IN" : "OUT";

                    // For OUT, ensure fund has enough balance
                    if (transType == "OUT" && fund.Balance < amount)
                        throw new InvalidOperationException("Quỹ không đủ số dư để thực hiện thanh toán.");

                    // Get or determine branch
                    int branchId = SessionManager.CurrentBranchId ?? 0;
                    if (branchId <= 0)
                    {
                        branchId = debt.BranchId;
                    }

                    if (branchId <= 0)
                    {
                        var branch = _context.Branches.FirstOrDefault(b => b.TenantId == tenantId && b.IsActive);
                        if (branch == null)
                            throw new InvalidOperationException("Không tìm thấy chi nhánh hoạt động. Vui lòng cấu hình chi nhánh trước.");
                        branchId = branch.BranchId;
                    }

                    if ((SessionManager.IsBranchManager || SessionManager.IsStaff) && SessionManager.CurrentBranchId.HasValue && branchId != SessionManager.CurrentBranchId.Value)
                        throw new UnauthorizedAccessException("Bạn chỉ có quyền thao tác công nợ trong chi nhánh hiện tại.");

                    // Choose a default category for this transType
                    var category = _context.TransactionCategories
                        .Where(c => c.TenantId == tenantId && c.BranchId == branchId && c.Type == transType && c.IsActive)
                        .OrderBy(c => c.CategoryId)
                        .FirstOrDefault();
                    if (category == null)
                        throw new InvalidOperationException("Không tìm thấy danh mục giao dịch phù hợp. Vui lòng tạo Category trước.");

                    // Verify the branch belongs to this tenant
                    var selectedBranch = _context.Branches.FirstOrDefault(b => b.BranchId == branchId && b.TenantId == tenantId);
                    if (selectedBranch == null)
                        throw new InvalidOperationException("Chi nhánh không hợp lệ cho Tenant hiện tại.");

                    // Create Transaction
                    var transaction = new Transaction
                    {
                        TenantId = tenantId,
                        FundId = fundId,
                        CategoryId = category.CategoryId,
                        PartnerId = debt.PartnerId,
                        DebtId = debt.DebtId,
                        BranchId = branchId,
                        TransDate = DateTime.Now,
                        Amount = amount,
                        Description = note,
                        TransType = transType,
                        RefNo = string.Empty, // Not null - use empty string instead
                        CreatedBy = SessionManager.CurrentUserId > 0 ? SessionManager.CurrentUserId : 1, // Default to system user if not set
                        Status = "COMPLETED",
                        IsActive = true
                    };

                    // Update fund balance and debt paid amount
                    if (transType == "IN")
                        fund.Balance += amount;
                    else
                        fund.Balance -= amount;

                    debt.PaidAmount += amount;
                    if (debt.PaidAmount >= debt.TotalAmount)
                        debt.Status = "PAID";
                    else if (debt.PaidAmount > 0)
                        debt.Status = "PARTIALLY_PAID";

                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
