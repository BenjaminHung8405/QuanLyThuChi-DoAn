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
            if (!SessionManager.CurrentTenantId.HasValue)
                return new List<Debt>();

            int tenantId = SessionManager.CurrentTenantId.Value;

            var query = _context.Debts
                .Include(d => d.Partner)
                .Where(d => d.TenantId == tenantId);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(d => d.DebtType == type);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(d => d.Status == status);

            if (partnerId.HasValue)
                query = query.Where(d => d.PartnerId == partnerId.Value);

            return query.OrderByDescending(d => d.DebtId).ToList();
        }

        // Lấy khoản nợ theo ID (bao gồm Partner)
        public Debt GetDebtById(long debtId)
        {
            if (!SessionManager.CurrentTenantId.HasValue)
                return null;

            int tenantId = SessionManager.CurrentTenantId.Value;
            return _context.Debts.Include(d => d.Partner)
                        .FirstOrDefault(d => d.DebtId == debtId && d.TenantId == tenantId);
        }

        /// <summary>
        /// Thực hiện trả nợ: tạo Transaction, cập nhật số dư Quỹ và cập nhật PaidAmount trong bảng Debts.
        /// Sử dụng IDbContextTransaction để đảm bảo atomic.
        /// </summary>
        public bool PayDebt(long debtId, int fundId, decimal amount, string note)
        {
            if (!SessionManager.CurrentTenantId.HasValue)
                throw new InvalidOperationException("Không có Tenant trong ngữ cảnh.");

            int tenantId = SessionManager.CurrentTenantId.Value;

            using (var tx = _context.Database.BeginTransaction())
            {
                try
                {
                    var debt = _context.Debts.FirstOrDefault(d => d.DebtId == debtId && d.TenantId == tenantId);
                    if (debt == null)
                        throw new KeyNotFoundException("Không tìm thấy khoản nợ.");

                    decimal remaining = debt.TotalAmount - debt.PaidAmount;
                    if (amount <= 0) throw new ArgumentException("Số tiền phải lớn hơn 0.");
                    if (amount > remaining) throw new InvalidOperationException("Không thể trả quá số nợ hiện có!");

                    var fund = _context.CashFunds.FirstOrDefault(f => f.FundId == fundId && f.TenantId == tenantId && f.IsActive);
                    if (fund == null) throw new KeyNotFoundException("Quỹ thanh toán không hợp lệ.");

                    // Determine transaction direction
                    string transType = string.Equals(debt.DebtType, "RECEIVABLE", StringComparison.OrdinalIgnoreCase) ? "IN" : "OUT";

                    // For OUT, ensure fund has enough balance
                    if (transType == "OUT" && fund.Balance < amount)
                        throw new InvalidOperationException("Quỹ không đủ số dư để thực hiện thanh toán.");

                    // Choose a default category for this transType
                    var category = _context.TransactionCategories.FirstOrDefault(c => c.TenantId == tenantId && c.Type == transType && c.IsActive);
                    if (category == null)
                        throw new InvalidOperationException("Không tìm thấy danh mục giao dịch phù hợp. Vui lòng tạo Category trước.");

                    // Create Transaction
                    var transaction = new Transaction
                    {
                        TenantId = tenantId,
                        FundId = fundId,
                        CategoryId = category.CategoryId,
                        PartnerId = debt.PartnerId,
                        DebtId = debt.DebtId,
                        BranchId = SessionManager.CurrentBranchIdValue,
                        TransDate = DateTime.Now,
                        Amount = amount,
                        Description = note,
                        TransType = transType,
                        RefNo = null,
                        CreatedBy = SessionManager.CurrentUserId,
                        Status = "COMPLETED",
                        IsActive = true
                    };

                    // Update fund balance and debt paid amount
                    if (transType == "IN")
                        fund.Balance += amount;
                    else
                        fund.Balance -= amount;

                    debt.PaidAmount += amount;
                    if (debt.PaidAmount > debt.TotalAmount) debt.PaidAmount = debt.TotalAmount;

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
