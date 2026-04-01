using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class BranchService
    {
        private readonly BaseRepository<Branch> _branchRepo;

        public BranchService(AppDbContext context)
        {
            _branchRepo = new BaseRepository<Branch>(context);
        }

        // Lấy danh sách chi nhánh của Tenant hiện tại
        public List<Branch> GetActiveBranches()
        {
            return _branchRepo.Find(b => b.TenantId == SessionManager.TenantId && b.IsActive)
                              .OrderBy(b => b.BranchName).ToList();
        }

        // Lấy danh sách chi nhánh theo tenant id rõ ràng
        public List<Branch> GetBranchesByTenant(int tenantId)
        {
            return _branchRepo.Find(b => b.TenantId == tenantId && b.IsActive)
                              .OrderBy(b => b.BranchName).ToList();
        }

        // Lấy tất cả chi nhánh (SuperAdmin)
        public List<Branch> GetAllActiveBranches()
        {
            return _branchRepo.Find(b => b.IsActive)
                              .OrderBy(b => b.BranchName).ToList();
        }

        /// <summary>
        /// Tạo chi nhánh mới
        /// </summary>
        public void CreateBranch(Branch branch)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo chi nhánh!");
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

            _branchRepo.Add(branch);
            _branchRepo.Save();
        }

        /// <summary>
        /// Cập nhật thông tin chi nhánh
        /// </summary>
        public void UpdateBranch(Branch branch)
        {
            if (!SessionManager.CanManageBranches)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa chi nhánh!");
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
        /// Xóa chi nhánh (Deactivate)
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
                _branchRepo.Delete(branchId);
                _branchRepo.Save();
            }
        }
    }
}