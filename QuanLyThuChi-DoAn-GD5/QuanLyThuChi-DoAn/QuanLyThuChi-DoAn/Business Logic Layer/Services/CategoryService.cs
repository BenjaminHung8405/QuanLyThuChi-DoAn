using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class CategoryService
    {
        private readonly BaseRepository<TransactionCategory> _catRepo;

        public CategoryService(AppDbContext context)
        {
            _catRepo = new BaseRepository<TransactionCategory>(context);
        }

        public List<TransactionCategory> GetCategories(string type) // "IN" hoặc "OUT"
        {
            return _catRepo.Find(c => c.TenantId == SessionManager.TenantId && c.Type == type)
                           .OrderBy(c => c.CategoryName).ToList();
        }

        /// <summary>
        /// Tạo loại thu chi mới
        /// </summary>
        public void CreateCategory(TransactionCategory category)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin hoặc BranchManager mới được tạo danh mục
            if (SessionManager.RoleName != "SuperAdmin" && SessionManager.RoleName != "BranchManager")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo loại thu chi!");
            }

            category.TenantId = SessionManager.TenantId; // Ép buộc theo Tenant hiện tại
            _catRepo.Add(category);
            _catRepo.Save();
        }

        /// <summary>
        /// Cập nhật loại thu chi
        /// </summary>
        public void UpdateCategory(TransactionCategory category)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin hoặc BranchManager mới được sửa
            if (SessionManager.RoleName != "SuperAdmin" && SessionManager.RoleName != "BranchManager")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa loại thu chi!");
            }

            category.TenantId = SessionManager.TenantId;
            _catRepo.Update(category);
            _catRepo.Save();
        }

        /// <summary>
        /// Xóa loại thu chi
        /// </summary>
        public void DeleteCategory(int categoryId)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được xóa danh mục
            if (SessionManager.RoleName != "SuperAdmin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa loại thu chi!");
            }

            _catRepo.Delete(categoryId);
            _catRepo.Save();
        }
    }
}