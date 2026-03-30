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

        // Lấy tất cả đối tác theo Tenant (chỉ đối tác đang hoạt động)
        public List<Partner> GetByTenant(int tenantId)
        {
            return _partnerRepo.Find(p => p.TenantId == tenantId && p.IsActive).ToList();
        }

        /// <summary>
        /// Lấy đối tác theo TenantId, có thể lọc theo loại
        /// </summary>
        public List<Partner> GetPartnersByTenant(int tenantId, string type = null)
        {
            var query = _partnerRepo.Find(p => p.TenantId == tenantId && p.IsActive);
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(p => p.Type == type);
            }
            return query.OrderByDescending(p => p.PartnerId).ToList();
        }

        /// <summary>
        /// Lấy danh sách đối tác theo Tenant với hỗ trợ tìm kiếm theo từ khóa
        /// 🔒 Chỉ lấy đối tác đang hoạt động (IsActive = true)
        /// </summary>
        /// <param name="tenantId">ID của công ty</param>
        /// <param name="keyword">Từ khóa tìm kiếm (Tên hoặc SĐT) - Nếu rỗng sẽ lấy tất cả</param>
        /// <returns>Danh sách đối tác sắp xếp theo ID giảm dần</returns>
        public List<Partner> GetPartners(int tenantId, string keyword = "")
        {
            var query = _partnerRepo.Find(p => p.TenantId == tenantId && p.IsActive);

            // Nếu người dùng có gõ tìm kiếm
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower().Trim();
                query = query.Where(p => p.PartnerName.ToLower().Contains(keyword)
                                      || (!string.IsNullOrEmpty(p.Phone) && p.Phone.Contains(keyword)));
            }

            return query.OrderByDescending(p => p.PartnerId).ToList();
        }

        // Lọc đối tác theo loại (CUSTOMER/SUPPLIER) và Tenant (chỉ đối tác đang hoạt động)
        public List<Partner> GetPartnersByType(string type)
        {
            if (!SessionManager.TenantId.HasValue)
                return new List<Partner>();

            return _partnerRepo.Find(p => p.TenantId == SessionManager.TenantId.Value && p.Type == type && p.IsActive)
                               .ToList();
        }

        /// <summary>
        /// Kiểm tra trùng lặp tên đối tác trong cùng 1 công ty
        /// </summary>
        public bool IsExisted(string name, int tenantId)
        {
            return _partnerRepo.Find(p => p.PartnerName == name && p.TenantId == tenantId).Any();
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

            // ✅ Business Logic: Kiểm tra trùng lặp tên đối tác
            if (!SessionManager.TenantId.HasValue)
                throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

            if (IsExisted(partner.PartnerName, SessionManager.TenantId.Value))
            {
                throw new InvalidOperationException($"Đối tác tên '{partner.PartnerName}' đã tồn tại!");
            }

            partner.TenantId = SessionManager.TenantId.Value; // Ép buộc theo Tenant hiện tại
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

            if (!SessionManager.TenantId.HasValue)
                throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

            partner.TenantId = SessionManager.TenantId.Value;
            _partnerRepo.Update(partner);
            _partnerRepo.Save();
        }

        /// <summary>
        /// Xóa đối tác (Hard Delete nếu không có giao dịch, Soft Delete nếu có giao dịch)
        /// 🔒 Soft Delete: Chỉ đặt IsActive = false để bảo toàn lịch sử công nợ
        /// </summary>
        public void DeletePartner(int id)
        {
            // 🔐 Deep Security: Chỉ SuperAdmin mới được xóa đối tác
            if (SessionManager.RoleName != "SuperAdmin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa đối tác!");
            }

            var partner = _partnerRepo.GetById(id);
            if (partner == null)
            {
                throw new InvalidOperationException("Không tìm thấy đối tác cần xóa!");
            }

            // ✅ Logic: Kiểm tra xem đối tác có giao dịch hay nợ phát sinh không
            // Nếu InitialDebt == 0 và không có giao dịch → Hard Delete
            // Nếu InitialDebt > 0 hoặc có giao dịch → Soft Delete (IsActive = false)

            if (partner.InitialDebt == 0)
            {
                // 🗑️ Hard Delete - Xóa hoàn toàn (không có dữ liệu liên kết)
                _partnerRepo.Delete(id);
                _partnerRepo.Save();
            }
            else
            {
                // 🔒 Soft Delete - Chỉ đặt IsActive = false (bảo toàn lịch sử công nợ)
                // Phương pháp này giúp:
                // 1. Bảo toàn lịch sử công nợ (vẫn có dữ liệu trong Transactions, Debts)
                // 2. Không ảnh hưởng đến báo cáo lịch sử
                // 3. Ngăn không cho chọn đối tác này khi nhập phiếu mới
                partner.IsActive = false;
                _partnerRepo.Update(partner);
                _partnerRepo.Save();
            }
        }
    }
}