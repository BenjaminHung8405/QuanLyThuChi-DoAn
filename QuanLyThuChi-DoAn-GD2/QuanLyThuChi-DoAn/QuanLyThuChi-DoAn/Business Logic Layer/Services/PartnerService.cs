using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class PartnerService
    {
        private readonly BaseRepository<Partner> _partnerRepo;

        public PartnerService(AppDbContext context)
        {
            _partnerRepo = new BaseRepository<Partner>(context);
        }

        // Lọc đối tác theo loại (CUSTOMER/SUPPLIER) và Tenant
        public List<Partner> GetPartnersByType(string type)
        {
            return _partnerRepo.Find(p => p.TenantId == SessionManager.TenantId && p.Type == type)
                               .ToList();
        }

        public void DeletePartner(int id)
        {
            var partner = _partnerRepo.GetById(id);
            // Logic: Nếu đối tác đã có giao dịch hoặc nợ, không cho xóa thẳng (Soft delete hoặc báo lỗi)
            if (partner != null && partner.InitialDebt == 0)
            {
                _partnerRepo.Delete(id);
                _partnerRepo.Save();
            }
        }
    }
}