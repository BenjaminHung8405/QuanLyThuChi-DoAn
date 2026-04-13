using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class CashFundService
    {
        private readonly AppDbContext _context;

        public CashFundService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<CashFund> GetFundsByBranch(int tenantId, int branchId, int roleId)
        {
            bool isAdminScope = roleId == (int)UserRole.SuperAdmin || roleId == (int)UserRole.TenantAdmin;
            bool isBranchScope = roleId == (int)UserRole.BranchManager || roleId == (int)UserRole.Staff;

            if (!isAdminScope && !isBranchScope)
            {
                return new List<CashFund>();
            }

            var query = _context.CashFunds
                .Include(f => f.Branch)
                .Where(f => f.TenantId == tenantId && f.IsActive);

            if (branchId > 0)
            {
                query = query.Where(f => f.BranchId == branchId);
            }
            else if (!isAdminScope)
            {
                // BranchManager/Staff không được phép xem "tất cả chi nhánh".
                return new List<CashFund>();
            }

            return query
                .OrderBy(f => f.FundName)
                .ToList();
        }

        public List<CashFund> GetFundsForCurrentSession(int roleId)
        {
            if (!SessionManager.CurrentTenantId.HasValue || !SessionManager.CurrentBranchId.HasValue)
            {
                return new List<CashFund>();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;
            int currentBranchId = SessionManager.CurrentBranchId.Value;
            return GetFundsByBranch(currentTenantId, currentBranchId, roleId);
        }

        public List<CashFund> GetFundsByTenant(int tenantId)
        {
            return _context.CashFunds
                .Include(f => f.Branch)
                .Where(f => f.TenantId == tenantId && f.IsActive)
                .OrderBy(f => f.FundName)
                .ToList();
        }

        public List<CashFund> GetFundsByBranchId(int tenantId, int branchId)
        {
            return _context.CashFunds
                .Include(f => f.Branch)
                .Where(f => f.TenantId == tenantId && f.BranchId == branchId && f.IsActive)
                .OrderBy(f => f.FundName)
                .ToList();
        }

        public List<CashFund> GetVisibleFunds(int roleId)
        {
            var query = _context.CashFunds
                .Include(f => f.Branch)
                .Where(f => f.IsActive);

            if (roleId == (int)UserRole.SuperAdmin)
            {
                // SuperAdmin: xem tất cả tenants
                return query.OrderBy(f => f.FundName).ToList();
            }

            if (!SessionManager.CurrentTenantId.HasValue)
            {
                return new List<CashFund>();
            }

            int tenantId = SessionManager.CurrentTenantId.Value;

            if (roleId == (int)UserRole.TenantAdmin)
            {
                // TenantManager: xem tất cả quỹ trong tenant hiện tại
                return query
                    .Where(f => f.TenantId == tenantId)
                    .OrderBy(f => f.FundName)
                    .ToList();
            }

            if (roleId == (int)UserRole.BranchManager || roleId == (int)UserRole.Staff)
            {
                // BranchManager/Staff: chỉ xem quỹ trong chi nhánh hiện tại
                if (!SessionManager.CurrentBranchId.HasValue)
                    return new List<CashFund>();

                int branchId = SessionManager.CurrentBranchId.Value;
                if (branchId <= 0)
                    return new List<CashFund>();

                return query
                    .Where(f => f.TenantId == tenantId && f.BranchId == branchId)
                    .OrderBy(f => f.FundName)
                    .ToList();
            }

            // Trường hợp role khác
            return new List<CashFund>();
        }

        public CashFund CreateFund(CashFund fund)
        {
            if (fund == null)
            {
                throw new ArgumentNullException(nameof(fund));
            }

            fund.IsActive = true;
            _context.CashFunds.Add(fund);
            _context.SaveChanges();
            return fund;
        }

        public void UpdateFund(CashFund fund)
        {
            if (fund == null)
            {
                throw new ArgumentNullException(nameof(fund));
            }

            var existing = _context.CashFunds.FirstOrDefault(f => f.FundId == fund.FundId);
            if (existing == null)
            {
                throw new InvalidOperationException("Không tìm thấy quỹ cần cập nhật!");
            }

            existing.FundName = fund.FundName;
            existing.AccountNumber = fund.AccountNumber;
            // Balance remains unchanged per business rule

            _context.SaveChanges();
        }

        public void DeleteFund(int fundId)
        {
            var existing = _context.CashFunds.FirstOrDefault(f => f.FundId == fundId);
            if (existing == null)
            {
                throw new InvalidOperationException("Không tìm thấy quỹ cần xóa!");
            }

            if (existing.Balance > 0)
            {
                throw new InvalidOperationException("Không thể xóa quỹ đang còn số dư!");
            }

            existing.IsActive = false;
            _context.SaveChanges();
        }

        public void UpdateBalance(int fundId, decimal amountChange)
        {
            var existing = _context.CashFunds.FirstOrDefault(f => f.FundId == fundId);
            if (existing == null)
            {
                throw new InvalidOperationException("Không tìm thấy quỹ cần cập nhật số dư!");
            }

            existing.Balance += amountChange;
            _context.SaveChanges();
        }

        public void SyncFundBalance(int fundId, int actionUserId)
        {
            // 1. Tìm Quỹ cần đồng bộ
            var fund = _context.CashFunds.Find(fundId);
            if (fund == null)
            {
                throw new Exception("Không tìm thấy dữ liệu Quỹ tiền hợp lệ trong hệ thống!");
            }

            // 2. Tính lại tổng tiền Thu (IN) và Chi (OUT) từ bảng Giao dịch
            decimal totalIn = _context.Transactions
                .Where(t => t.FundId == fundId
                         && t.TransType == "IN"
                         && t.IsActive == true
                         && t.Status == "COMPLETED")
                .Sum(t => (decimal?)t.Amount) ?? 0;

            decimal totalOut = _context.Transactions
                .Where(t => t.FundId == fundId
                         && t.TransType == "OUT"
                         && t.IsActive == true
                         && t.Status == "COMPLETED")
                .Sum(t => (decimal?)t.Amount) ?? 0;

            // 3. Cập nhật số dư mới
            decimal calculatedBalance = totalIn - totalOut;
            fund.Balance = calculatedBalance;

            // TODO: Optionally log actionUserId to SystemLogs here.

            // 4. Lưu thay đổi vào Database
            _context.SaveChanges();
        }

        public decimal CalculateActualBalance(int fundId)
        {
            var transactions = _context.Transactions
                .Where(t => t.FundId == fundId &&
                            t.Status == "COMPLETED" &&
                            t.IsActive == true)
                .ToList();

            decimal totalIn = transactions.Where(t => t.TransType == "IN").Sum(t => t.Amount);
            decimal totalOut = transactions.Where(t => t.TransType == "OUT").Sum(t => t.Amount);

            // Nếu sau này có InitialBalance thì thêm vào công thức:
            // var fund = _context.CashFunds.Find(fundId); return (fund?.InitialBalance ?? 0) + totalIn - totalOut;
            return totalIn - totalOut;
        }

        public FundAuditResult AuditFund(int fundId)
        {
            var fund = _context.CashFunds.FirstOrDefault(f => f.FundId == fundId && f.IsActive == true);
            if (fund == null)
            {
                throw new KeyNotFoundException("Không tìm thấy quỹ cần kiểm soát.");
            }

            decimal totalIn = _context.Transactions
                .Where(t => t.FundId == fundId
                         && t.TransType == "IN"
                         && t.IsActive == true
                         && t.Status == "COMPLETED")
                .Sum(t => (decimal?)t.Amount) ?? 0;

            decimal totalOut = _context.Transactions
                .Where(t => t.FundId == fundId
                         && t.TransType == "OUT"
                         && t.IsActive == true
                         && t.Status == "COMPLETED")
                .Sum(t => (decimal?)t.Amount) ?? 0;

            decimal calculated = totalIn - totalOut;

            return new FundAuditResult
            {
                FundId = fund.FundId,
                FundName = fund.FundName,
                CurrentBalance = fund.Balance,
                CalculatedBalance = calculated,
                Difference = fund.Balance - calculated
            };
        }

        public List<FundAuditResult> AuditBranchFunds(int tenantId, int branchId)
        {
            var auditResults = new List<FundAuditResult>();

            // Lấy tất cả các quỹ đang hoạt động của chi nhánh
            var funds = _context.CashFunds
                .Where(f => f.TenantId == tenantId && f.BranchId == branchId && f.IsActive == true)
                .ToList();

            foreach (var fund in funds)
            {
                decimal totalIn = _context.Transactions
                    .Where(t => t.FundId == fund.FundId
                             && t.TransType == "IN"
                             && t.IsActive == true
                             && t.Status == "COMPLETED")
                    .Sum(t => (decimal?)t.Amount) ?? 0;

                decimal totalOut = _context.Transactions
                    .Where(t => t.FundId == fund.FundId
                             && t.TransType == "OUT"
                             && t.IsActive == true
                             && t.Status == "COMPLETED")
                    .Sum(t => (decimal?)t.Amount) ?? 0;

                decimal calculated = totalIn - totalOut;

                auditResults.Add(new FundAuditResult
                {
                    FundId = fund.FundId,
                    FundName = fund.FundName,
                    CurrentBalance = fund.Balance,
                    CalculatedBalance = calculated,
                    Difference = fund.Balance - calculated
                });
            }

            return auditResults;
        }
    }

    public class FundAuditResult
    {
        public int FundId { get; set; }
        public string FundName { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CalculatedBalance { get; set; }
        public decimal Difference { get; set; }
        public bool IsValid => Difference == 0;
    }
}
