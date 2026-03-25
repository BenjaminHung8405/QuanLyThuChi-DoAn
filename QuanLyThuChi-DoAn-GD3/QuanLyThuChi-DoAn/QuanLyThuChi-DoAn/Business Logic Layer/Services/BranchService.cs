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

        /// <summary>
        /// Tạo chi nhánh mới
        /// </summary>
        public void CreateBranch(Branch branch)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được tạo chi nhánh
            if (SessionManager.RoleName != "SuperAdmin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo chi nhánh!");
            }

            branch.TenantId = SessionManager.TenantId; // Ép buộc theo Tenant hiện tại
            _branchRepo.Add(branch);
            _branchRepo.Save();
        }

        /// <summary>
        /// Cập nhật thông tin chi nhánh
        /// </summary>
        public void UpdateBranch(Branch branch)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin hoặc BranchManager của chi nhánh tương ứng mới được sửa
            if (SessionManager.RoleName == "SuperAdmin")
            {
                // SuperAdmin có quyền sửa bất kỳ chi nhánh nào
            }
            else if (SessionManager.RoleName == "BranchManager" && branch.BranchId != SessionManager.BranchId)
            {
                throw new UnauthorizedAccessException("Bạn chỉ có quyền cấu hình chi nhánh của mình!");
            }
            else
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa chi nhánh!");
            }

            branch.TenantId = SessionManager.TenantId;
            _branchRepo.Update(branch);
            _branchRepo.Save();
        }

        /// <summary>
        /// Xóa chi nhánh (Deactivate)
        /// </summary>
        public void DeleteBranch(int branchId)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được xóa chi nhánh
            if (SessionManager.RoleName != "SuperAdmin")
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