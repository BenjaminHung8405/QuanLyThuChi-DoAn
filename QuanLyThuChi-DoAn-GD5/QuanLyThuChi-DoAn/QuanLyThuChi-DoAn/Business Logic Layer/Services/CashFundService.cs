using System;
using System.Collections.Generic;
using System.Linq;
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
            var query = _context.CashFunds
                .Where(f => f.TenantId == tenantId && f.BranchId == branchId && f.IsActive);

            if (roleId == 3)
            {
                query = query.Where(f => f.FundName != null && f.FundName.Contains("Tiền mặt"));
            }
            else if (roleId == 2)
            {
                // Admin: lấy tất cả quỹ đang hoạt động
            }
            else
            {
                // Các vai trò khác không được truy cập danh sách quỹ theo yêu cầu RBAC cụ thể
                query = query.Where(f => false);
            }

            return query.ToList();
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
                .Where(t => t.FundId == fundId && t.TransType == "IN" && t.IsActive == true)
                .Sum(t => t.Amount);

            decimal totalOut = _context.Transactions
                .Where(t => t.FundId == fundId && t.TransType == "OUT" && t.IsActive == true)
                .Sum(t => t.Amount);

            // 3. Cập nhật số dư mới
            decimal calculatedBalance = totalIn - totalOut;
            fund.Balance = calculatedBalance;

            // TODO: Optionally log actionUserId to SystemLogs here.

            // 4. Lưu thay đổi vào Database
            _context.SaveChanges();
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
                    .Where(t => t.FundId == fund.FundId && t.TransType == "IN" && t.IsActive == true)
                    .Sum(t => t.Amount);

                decimal totalOut = _context.Transactions
                    .Where(t => t.FundId == fund.FundId && t.TransType == "OUT" && t.IsActive == true)
                    .Sum(t => t.Amount);

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
