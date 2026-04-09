using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class BranchService
    {
        private readonly BaseRepository<Branch> _branchRepo;
        private readonly AppDbContext _context;

        public BranchService(AppDbContext context)
        {
            _branchRepo = new BaseRepository<Branch>(context);
            _context = context;
        }

        // ============= SYNCHRONOUS METHODS =============

        /// <summary>
        /// Lấy danh sách chi nhánh HOẠT ĐỘNG của Tenant hiện tại
        /// </summary>
        public List<Branch> GetActiveBranches()
        {
            return _branchRepo.Find(b => b.TenantId == SessionManager.TenantId && b.IsActive)
                              .OrderByDescending(b => b.CreatedDate).ToList();
        }

        /// <summary>
        /// Lấy danh sách chi nhánh HOẠT ĐỘNG theo tenant id rõ ràng
        /// </summary>
        public List<Branch> GetBranchesByTenant(int tenantId)
        {
            return _branchRepo.Find(b => b.TenantId == tenantId && b.IsActive)
                              .OrderByDescending(b => b.CreatedDate).ToList();
        }

        /// <summary>
        /// Tương thích ngược cho các màn hình cũ đang gọi API này.
        /// </summary>
        public async Task<List<Branch>> GetBranchesByTenantAsync(int tenantId)
        {
            return await GetActiveBranchesByTenantAsync(tenantId).ConfigureAwait(false);
        }

        /// <summary>
        /// Lấy tất cả chi nhánh (HOẠT ĐỘNG + KHÓA) của Tenant
        /// </summary>
        public List<Branch> GetAllBranchesByTenant(int tenantId)
        {
            return _branchRepo.Find(b => b.TenantId == tenantId)
                              .OrderByDescending(b => b.CreatedDate).ToList();
        }

        /// <summary>
        /// Lấy tất cả chi nhánh HOẠT ĐỘNG (SuperAdmin)
        /// </summary>
        public List<Branch> GetAllActiveBranches()
        {
            return _branchRepo.Find(b => b.IsActive)
                              .OrderByDescending(b => b.CreatedDate).ToList();
        }

        /// <summary>
        /// Tạo chi nhánh mới (SYNC)
        /// </summary>
        public void CreateBranch(Branch branch)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo chi nhánh!");
            }

            // Kiểm tra trùng tên trong Tenant
            var existsDuplicate = _context.Branches.Any(b =>
                b.TenantId == branch.TenantId &&
                b.BranchName == branch.BranchName);

            if (existsDuplicate)
            {
                throw new InvalidOperationException("Tên chi nhánh này đã tồn tại trong hệ thống!");
            }

            if (SessionManager.IsTenantAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

                branch.TenantId = SessionManager.CurrentTenantId.Value;
            }
            else if (branch.TenantId <= 0)
            {
                throw new InvalidOperationException("SuperAdmin cần chỉ định Tenant hợp lệ khi tạo chi nhánh.");
            }

            branch.CreatedDate = DateTime.Now;
            _branchRepo.Add(branch);
            _branchRepo.Save();
        }

        /// <summary>
        /// Cập nhật thông tin chi nhánh (SYNC)
        /// </summary>
        public void UpdateBranch(Branch branch)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa chi nhánh!");
            }

            // Kiểm tra trùng tên (ngoại trừ chính nó)
            var existsDuplicate = _context.Branches.Any(b =>
                b.TenantId == branch.TenantId &&
                b.BranchName == branch.BranchName &&
                b.BranchId != branch.BranchId);

            if (existsDuplicate)
            {
                throw new InvalidOperationException("Tên chi nhánh này đã tồn tại trong hệ thống!");
            }

            if (SessionManager.IsTenantAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

                branch.TenantId = SessionManager.CurrentTenantId.Value;
            }

            _branchRepo.Update(branch);
            _branchRepo.Save();
        }

        /// <summary>
        /// Xóa chi nhánh (Deactivate) (SYNC)
        /// </summary>
        public void DeleteBranch(int branchId)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa chi nhánh!");
            }

            var branch = _branchRepo.GetById(branchId);
            if (branch != null)
            {
                branch.IsActive = false;
                _branchRepo.Update(branch);
                _branchRepo.Save();
            }
        }

        // ============= ASYNCHRONOUS METHODS =============

        /// <summary>
        /// Lấy danh sách chi nhánh theo Tenant (bao gồm cả chi nhánh đã khóa nếu cần)
        /// </summary>
        public async Task<List<Branch>> GetByTenantAsync(int tenantId)
        {
            return await _context.Branches
                .AsNoTracking()
                .Where(b => b.TenantId == tenantId)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Lấy danh sách chi nhánh HOẠT ĐỘNG của Tenant
        /// </summary>
        public async Task<List<Branch>> GetActiveBranchesByTenantAsync(int tenantId)
        {
            return await _context.Branches
                .AsNoTracking()
                .Where(b => b.TenantId == tenantId && b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Thêm mới chi nhánh ASYNC
        /// Kiểm tra trùng tên trong cùng Tenant
        /// </summary>
        public async Task<bool> AddAsync(Branch branch)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo chi nhánh!");
            }

            // Kiểm tra trùng tên trong Tenant
            var exists = await _context.Branches.AnyAsync(b =>
                b.TenantId == branch.TenantId &&
                b.BranchName == branch.BranchName)
                .ConfigureAwait(false);

            if (exists)
            {
                throw new InvalidOperationException("Tên chi nhánh này đã tồn tại trong hệ thống!");
            }

            if (SessionManager.IsTenantAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

                branch.TenantId = SessionManager.CurrentTenantId.Value;
            }
            else if (branch.TenantId <= 0)
            {
                throw new InvalidOperationException("SuperAdmin cần chỉ định Tenant hợp lệ khi tạo chi nhánh.");
            }

            branch.CreatedDate = DateTime.Now;
            _context.Branches.Add(branch);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        /// <summary>
        /// Khóa/Mở khóa chi nhánh
        /// </summary>
        public async Task<bool> ToggleStatusAsync(int branchId)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thay đổi trạng thái chi nhánh!");
            }

            var branch = await _context.Branches.FindAsync(branchId).ConfigureAwait(false);
            if (branch == null)
            {
                return false;
            }

            branch.IsActive = !branch.IsActive;
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        /// <summary>
        /// Cập nhật thông tin chi nhánh ASYNC
        /// </summary>
        public async Task<bool> UpdateAsync(Branch branch)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa chi nhánh!");
            }

            // Kiểm tra trùng tên (ngoại trừ chính nó)
            var existsDuplicate = await _context.Branches.AnyAsync(b =>
                b.TenantId == branch.TenantId &&
                b.BranchName == branch.BranchName &&
                b.BranchId != branch.BranchId)
                .ConfigureAwait(false);

            if (existsDuplicate)
            {
                throw new InvalidOperationException("Tên chi nhánh này đã tồn tại trong hệ thống!");
            }

            if (SessionManager.IsTenantAdmin)
            {
                if (!SessionManager.CurrentTenantId.HasValue)
                    throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

                branch.TenantId = SessionManager.CurrentTenantId.Value;
            }

            _context.Branches.Update(branch);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        /// <summary>
        /// Lấy chi nhánh theo ID
        /// </summary>
        public async Task<Branch> GetByIdAsync(int branchId)
        {
            return await _context.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BranchId == branchId)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Xóa chi nhánh ASYNC (Deactivate)
        /// </summary>
        public async Task<bool> DeleteAsync(int branchId)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa chi nhánh!");
            }

            var branch = await _context.Branches.FindAsync(branchId).ConfigureAwait(false);
            if (branch == null)
            {
                return false;
            }

            branch.IsActive = false;
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}