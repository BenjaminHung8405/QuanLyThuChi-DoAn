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
    }
}