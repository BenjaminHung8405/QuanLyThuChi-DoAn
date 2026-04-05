using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.DTOs;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardOverviewDTO> GetOverviewAsync(int tenantId, int? branchId, DateTime fromDate, DateTime toDate)
        {
            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1).AddTicks(-1);

            IQueryable<Transaction> transQuery = _context.Transactions
                .AsNoTracking()
                .Where(t => t.TenantId == tenantId
                            && t.IsActive == true
                            && t.TransDate >= startDate
                            && t.TransDate <= endDate
                            && t.Status == "COMPLETED");

            if (branchId.HasValue && branchId.Value > 0)
            {
                transQuery = transQuery.Where(t => t.BranchId == branchId.Value);
            }

            decimal totalIn = await transQuery
                .Where(t => t.TransType == "IN")
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            decimal totalOut = await transQuery
                .Where(t => t.TransType == "OUT")
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            IQueryable<Debt> debtQuery = _context.Debts
                .AsNoTracking()
                .Where(d => d.TenantId == tenantId
                            && d.IsActive == true
                            && d.Status != "PAID");

            if (branchId.HasValue && branchId.Value > 0)
            {
                debtQuery = debtQuery.Where(d => d.BranchId == branchId.Value);
            }

            decimal totalReceivable = await debtQuery
                .Where(d => d.DebtType == "RECEIVABLE")
                .SumAsync(d => (decimal?)(d.TotalAmount - d.PaidAmount)) ?? 0;

            decimal totalPayable = await debtQuery
                .Where(d => d.DebtType == "PAYABLE")
                .SumAsync(d => (decimal?)(d.TotalAmount - d.PaidAmount)) ?? 0;

            return new DashboardOverviewDTO
            {
                TotalIncome = totalIn,
                TotalExpense = totalOut,
                TotalReceivable = totalReceivable,
                TotalPayable = totalPayable
            };
        }

        public async Task<List<CategoryStatisticDTO>> GetExpenseByCategoryAsync(int tenantId, int? branchId, DateTime fromDate, DateTime toDate)
        {
            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1).AddTicks(-1);

            IQueryable<Transaction> query = _context.Transactions
                .AsNoTracking()
                .Where(t => t.TenantId == tenantId
                            && t.TransType == "OUT"
                            && t.IsActive == true
                            && t.Status == "COMPLETED"
                            && t.TransDate >= startDate
                            && t.TransDate <= endDate);

            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(t => t.BranchId == branchId.Value);
            }

            List<CategoryStatisticDTO> stats = await query
                .GroupBy(t => t.Category != null ? t.Category.CategoryName : "Khac")
                .Select(g => new CategoryStatisticDTO
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();

            return stats;
        }

        public async Task<List<CashFlowTrendDTO>> GetCashFlowTrendAsync(int tenantId, int? branchId, DateTime fromDate, DateTime toDate)
        {
            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1).AddTicks(-1);

            IQueryable<Transaction> query = _context.Transactions
                .AsNoTracking()
                .Where(t => t.TenantId == tenantId
                            && t.IsActive == true
                            && t.Status == "COMPLETED"
                            && t.TransDate >= startDate
                            && t.TransDate <= endDate);

            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(t => t.BranchId == branchId.Value);
            }

            List<CashFlowTrendDTO> trendList = await query
                .GroupBy(t => t.TransDate.Date)
                .Select(g => new CashFlowTrendDTO
                {
                    Date = g.Key,
                    TotalIncome = g.Sum(x => x.TransType == "IN" ? x.Amount : 0),
                    TotalExpense = g.Sum(x => x.TransType == "OUT" ? x.Amount : 0)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return trendList;
        }

        public async Task<List<CashbookDetailDTO>> GetCashbookDetailsAsync(int tenantId, int? branchId, DateTime fromDate, DateTime toDate)
        {
            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1).AddTicks(-1);

            IQueryable<Transaction> pastQuery = _context.Transactions
                .AsNoTracking()
                .Where(t => t.TenantId == tenantId
                            && t.IsActive == true
                            && t.Status == "COMPLETED"
                            && t.TransDate < startDate);

            if (branchId.HasValue && branchId.Value > 0)
            {
                pastQuery = pastQuery.Where(t => t.BranchId == branchId.Value);
            }

            decimal pastIncome = await pastQuery
                .Where(t => t.TransType == "IN")
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            decimal pastExpense = await pastQuery
                .Where(t => t.TransType == "OUT")
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            decimal currentBalance = pastIncome - pastExpense;

            IQueryable<Transaction> periodQuery = _context.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => t.TenantId == tenantId
                            && t.IsActive == true
                            && t.Status == "COMPLETED"
                            && t.TransDate >= startDate
                            && t.TransDate <= endDate);

            if (branchId.HasValue && branchId.Value > 0)
            {
                periodQuery = periodQuery.Where(t => t.BranchId == branchId.Value);
            }

            List<Transaction> transactions = await periodQuery
                .OrderBy(t => t.TransDate)
                .ThenBy(t => t.TransId)
                .ToListAsync();

            var resultList = new List<CashbookDetailDTO>(transactions.Count + 1)
            {
                new CashbookDetailDTO
                {
                    TransactionDate = startDate,
                    TransactionType = "-",
                    CategoryName = "SỐ DƯ ĐẦU KỲ",
                    Notes = string.Empty,
                    IncomeAmount = 0,
                    ExpenseAmount = 0,
                    RunningBalance = currentBalance,
                    CreatorName = string.Empty
                }
            };

            foreach (Transaction transaction in transactions)
            {
                bool isIncome = string.Equals(transaction.TransType, "IN", StringComparison.OrdinalIgnoreCase);

                decimal income = isIncome
                    ? transaction.Amount
                    : 0;
                decimal expense = !isIncome
                    ? transaction.Amount
                    : 0;

                currentBalance += income - expense;

                resultList.Add(new CashbookDetailDTO
                {
                    TransactionDate = transaction.TransDate,
                    TransactionType = isIncome ? "Thu" : "Chi",
                    CategoryName = transaction.Category != null ? transaction.Category.CategoryName ?? "Khác" : "Khác",
                    Notes = transaction.Description ?? string.Empty,
                    IncomeAmount = income,
                    ExpenseAmount = expense,
                    RunningBalance = currentBalance,
                    CreatorName = transaction.User != null ? transaction.User.FullName ?? string.Empty : string.Empty
                });
            }

            return resultList;
        }

        public async Task<List<CashbookSummaryDTO>> GetCashbookSummaryAsync(int tenantId, int? branchId, DateTime fromDate, DateTime toDate)
        {
            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1).AddTicks(-1);

            IQueryable<Transaction> periodQuery = _context.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .Where(t => t.TenantId == tenantId
                            && t.IsActive == true
                            && t.Status == "COMPLETED"
                            && t.TransDate >= startDate
                            && t.TransDate <= endDate);

            if (branchId.HasValue && branchId.Value > 0)
            {
                periodQuery = periodQuery.Where(t => t.BranchId == branchId.Value);
            }

            return await periodQuery
                .GroupBy(t => new
                {
                    t.TransType,
                    CategoryName = t.Category != null ? t.Category.CategoryName : "Khác"
                })
                .Select(g => new CashbookSummaryDTO
                {
                    TransactionType = g.Key.TransType == "IN" ? "Thu" : "Chi",
                    CategoryName = g.Key.CategoryName ?? "Khác",
                    TotalTransactions = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.TransactionType)
                .ThenByDescending(x => x.TotalAmount)
                .ToListAsync();
        }

        public async Task<List<BranchReconciliationDTO>> GetChainReconciliationAsync(int tenantId, DateTime fromDate, DateTime toDate)
        {
            // 1. Chuẩn hóa biên độ thời gian (Chống trượt giao dịch ngày cuối)
            DateTime startDate = fromDate.Date;
            DateTime endDate = toDate.Date.AddDays(1).AddTicks(-1);

            // 2. KHỞI TẠO QUERY VỚI AS-NO-TRACKING (Tối ưu RAM cực mạnh)
            // Include Branch để lấy được tên BranchName
            var query = _context.Transactions
                .AsNoTracking()
                .Include(t => t.Branch)
                .Where(t => t.TenantId == tenantId &&
                            t.IsActive == true &&
                            t.TransDate >= startDate &&
                            t.TransDate <= endDate &&
                            t.Status == "COMPLETED");

            // 3. THỰC THI NHÓM VÀ TÍNH TOÁN DƯỚI TẦNG SQL SERVER
            var stats = await query
                .GroupBy(t => t.Branch.BranchName)
                .Select(g => new BranchReconciliationDTO
                {
                    // g.Key chính là BranchName. Dùng toán tử ?? để đề phòng tên chi nhánh bị Null trong DB
                    BranchName = g.Key ?? "Chi nhánh không xác định",

                    // Tính tổng thu/chi
                    TotalIncome = g.Sum(x => x.TransType == "IN" ? x.Amount : 0),
                    TotalExpense = g.Sum(x => x.TransType == "OUT" ? x.Amount : 0),

                    // Đếm tổng số phiếu Thu/Chi của chi nhánh đó
                    TransactionCount = g.Count()
                })
                .OrderByDescending(x => x.TotalIncome - x.TotalExpense)
                .ToListAsync();

            return stats;
        }
    }
}