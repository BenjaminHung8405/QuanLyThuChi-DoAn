using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.DTOs;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// CategoryId hệ thống dành cho phiếu Chuyển quỹ nội bộ (quỹ nguồn - phía OUT).
        /// Phải khớp với <c>TransactionService.InternalTransferOutCategoryId</c>.
        /// </summary>
        private const int InternalTransferOutCategoryId = 98;

        /// <summary>
        /// CategoryId hệ thống dành cho phiếu Nhận chuyển quỹ nội bộ (quỹ đích - phía IN).
        /// Phải khớp với <c>TransactionService.InternalTransferInCategoryId</c>.
        /// </summary>
        private const int InternalTransferInCategoryId = 99;

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

            // Loại trừ các phiếu Chuyển quỹ nội bộ ra khỏi P&L.
            // Chúng là dịch chuyển tiền giữa các quỹ, KHÔNG phải doanh thu hay chi phí thực tế.
            IQueryable<Transaction> incomeQuery = transQuery
                .Where(t => t.TransType == "IN"
                         && t.CategoryId != InternalTransferInCategoryId);
            decimal netIncome = await incomeQuery.SumAsync(t => (decimal?)t.SubTotal) ?? 0;
            decimal outputVat = await incomeQuery.SumAsync(t => (decimal?)t.TaxAmount) ?? 0;
            decimal grossIncome = await incomeQuery.SumAsync(t => (decimal?)t.Amount) ?? 0;

            IQueryable<Transaction> expenseQuery = transQuery
                .Where(t => t.TransType == "OUT"
                         && t.CategoryId != InternalTransferOutCategoryId);
            decimal netExpense = await expenseQuery.SumAsync(t => (decimal?)t.SubTotal) ?? 0;
            decimal inputVat = await expenseQuery.SumAsync(t => (decimal?)t.TaxAmount) ?? 0;
            decimal grossExpense = await expenseQuery.SumAsync(t => (decimal?)t.Amount) ?? 0;

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
                GrossIncome = grossIncome,
                NetIncome = netIncome,
                OutputVAT = outputVat,
                GrossExpense = grossExpense,
                NetExpense = netExpense,
                InputVAT = inputVat,
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
                            && t.TransDate <= endDate
                            // Loại trừ phiếu Chuyển quỹ nội bộ khỏi biểu đồ cơ cấu chi tiêu
                            && t.CategoryId != InternalTransferOutCategoryId);

            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(t => t.BranchId == branchId.Value);
            }

            List<CategoryStatisticDTO> stats = await query
                .GroupBy(t => t.CategoryNameSnapshot == null || t.CategoryNameSnapshot == string.Empty
                    ? "Khac"
                    : t.CategoryNameSnapshot)
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
                    // Loại trừ phiếu chuyển quỹ nội bộ khỏi biểu đồ xu hướng Thu/Chi
                    TotalIncome = g.Sum(x => x.TransType == "IN" && x.CategoryId != InternalTransferInCategoryId ? x.Amount : 0),
                    TotalExpense = g.Sum(x => x.TransType == "OUT" && x.CategoryId != InternalTransferOutCategoryId ? x.Amount : 0)
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
                    CategoryName = string.IsNullOrWhiteSpace(transaction.CategoryNameSnapshot) ? "Khác" : transaction.CategoryNameSnapshot,
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
                    CategoryName = t.CategoryNameSnapshot == null || t.CategoryNameSnapshot == string.Empty
                        ? "Khác"
                        : t.CategoryNameSnapshot
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

                    // Tính tổng thu/chi thực tế - loại trừ phiếu Chuyển quỹ nội bộ (không phải P&L)
                    TotalIncome = g.Sum(x => x.TransType == "IN" && x.CategoryId != InternalTransferInCategoryId ? x.Amount : 0),
                    TotalExpense = g.Sum(x => x.TransType == "OUT" && x.CategoryId != InternalTransferOutCategoryId ? x.Amount : 0),

                    // Đếm tổng số phiếu Thu/Chi của chi nhánh đó (bao gồm cả phiếu chuyển quỹ)
                    TransactionCount = g.Count()
                })
                .OrderByDescending(x => x.TotalIncome - x.TotalExpense)
                .ToListAsync();

            return stats;
        }
    }
}