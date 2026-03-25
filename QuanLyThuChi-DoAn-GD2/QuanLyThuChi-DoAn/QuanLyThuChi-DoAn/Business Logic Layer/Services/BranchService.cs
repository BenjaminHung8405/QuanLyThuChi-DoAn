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

        public void SaveBranch(Branch branch)
        {
            branch.TenantId = SessionManager.TenantId; // Ép buộc theo Tenant hiện tại
            if (branch.BranchId == 0) _branchRepo.Add(branch);
            else _branchRepo.Update(branch);
            _branchRepo.Save();
        }
    }
}