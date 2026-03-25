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

        /// <summary>
        /// Tạo đối tác mới
        /// </summary>
        public void CreatePartner(Partner partner)
        {
            // 🔐 Deep Security: Kiểm tra quyền tạo đối tác
            // Chỉ SuperAdmin hoặc BranchManager mới có quyền
            if (SessionManager.RoleName != "SuperAdmin" && SessionManager.RoleName != "BranchManager")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền tạo đối tác!");
            }

            partner.TenantId = SessionManager.TenantId; // Ép buộc theo Tenant hiện tại
            _partnerRepo.Add(partner);
            _partnerRepo.Save();
        }

        /// <summary>
        /// Cập nhật thông tin đối tác
        /// </summary>
        public void UpdatePartner(Partner partner)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin hoặc BranchManager mới được sửa
            if (SessionManager.RoleName != "SuperAdmin" && SessionManager.RoleName != "BranchManager")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa đối tác!");
            }

            partner.TenantId = SessionManager.TenantId;
            _partnerRepo.Update(partner);
            _partnerRepo.Save();
        }

        /// <summary>
        /// Xóa đối tác (Soft Delete hoặc Hard Delete)
        /// </summary>
        public void DeletePartner(int id)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được xóa đối tác
            if (SessionManager.RoleName != "SuperAdmin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa đối tác!");
            }

            var partner = _partnerRepo.GetById(id);
            // Logic: Nếu đối tác đã có giao dịch hoặc nợ, không cho xóa thẳng (Soft delete hoặc báo lỗi)
            if (partner != null && partner.InitialDebt == 0)
            {
                _partnerRepo.Delete(id);
                _partnerRepo.Save();
            }
            else if (partner != null)
            {
                throw new InvalidOperationException("Không thể xóa đối tác đã có giao dịch hoặc nợ!");
            }
        }
    }
}