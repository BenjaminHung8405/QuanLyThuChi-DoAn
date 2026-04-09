using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class PartnerService
    {
        private readonly BaseRepository<Partner> _partnerRepo;

        public PartnerService(AppDbContext context)
        {
            _partnerRepo = new BaseRepository<Partner>(context);
        }

        private static int ResolveTenantScope(int requestedTenantId = 0)
        {
            if (SessionManager.IsSuperAdmin)
            {
                if (requestedTenantId > 0)
                    return requestedTenantId;

                if (SessionManager.CurrentTenantId.HasValue && SessionManager.CurrentTenantId.Value > 0)
                    return SessionManager.CurrentTenantId.Value;

                throw new InvalidOperationException("SuperAdmin cần chọn Tenant để thao tác dữ liệu.");
            }

            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
                throw new InvalidOperationException("Không có tenant ngữ cảnh. Vui lòng đăng nhập lại.");

            return SessionManager.CurrentTenantId.Value;
        }

        private static void EnsureCanManagePartnerMasterData()
        {
            if (!SessionManager.IsSuperAdmin && !SessionManager.IsTenantAdmin)
                throw new UnauthorizedAccessException("Bạn không có quyền cấu hình danh mục đối tác.");
        }

        private static int? ResolveBranchScopeForRead()
        {
            if (SessionManager.IsBranchManager || SessionManager.IsStaff)
            {
                if (!SessionManager.CurrentBranchId.HasValue || SessionManager.CurrentBranchId.Value <= 0)
                    throw new InvalidOperationException("Không có chi nhánh ngữ cảnh. Vui lòng đăng nhập lại.");

                return SessionManager.CurrentBranchId.Value;
            }

            if (SessionManager.CurrentBranchId.HasValue && SessionManager.CurrentBranchId.Value > 0)
                return SessionManager.CurrentBranchId.Value;

            return null;
        }

        private static int ResolveBranchScopeForWrite(int requestedBranchId = 0)
        {
            if (requestedBranchId > 0)
            {
                if ((SessionManager.IsBranchManager || SessionManager.IsStaff)
                    && SessionManager.CurrentBranchId.HasValue
                    && SessionManager.CurrentBranchId.Value > 0
                    && requestedBranchId != SessionManager.CurrentBranchId.Value)
                {
                    throw new UnauthorizedAccessException("Bạn không có quyền thao tác dữ liệu ngoài chi nhánh hiện tại.");
                }

                return requestedBranchId;
            }

            if (SessionManager.CurrentBranchId.HasValue && SessionManager.CurrentBranchId.Value > 0)
                return SessionManager.CurrentBranchId.Value;

            throw new InvalidOperationException("Vui lòng chọn chi nhánh trước khi thao tác dữ liệu.");
        }

        // Lấy tất cả đối tác theo Tenant (chỉ đối tác đang hoạt động)
        public List<Partner> GetByTenant(int tenantId)
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();
            return _partnerRepo.Find(p => p.TenantId == scopedTenantId
                                        && p.IsActive
                                        && (!scopedBranchId.HasValue || p.BranchId == scopedBranchId.Value))
                               .OrderByDescending(p => p.PartnerId)
                               .ToList();
        }

        /// <summary>
        /// Lấy đối tác theo TenantId, có thể lọc theo loại
        /// </summary>
        public List<Partner> GetPartnersByTenant(int tenantId, string type = null)
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();
            var query = _partnerRepo.Find(p => p.TenantId == scopedTenantId
                                            && p.IsActive
                                            && (!scopedBranchId.HasValue || p.BranchId == scopedBranchId.Value));
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
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();
            var query = _partnerRepo.Find(p => p.TenantId == scopedTenantId
                                            && p.IsActive
                                            && (!scopedBranchId.HasValue || p.BranchId == scopedBranchId.Value));

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
            int scopedTenantId = ResolveTenantScope();
            int? scopedBranchId = ResolveBranchScopeForRead();
            return _partnerRepo.Find(p => p.TenantId == scopedTenantId
                                       && p.Type == type
                                       && p.IsActive
                                       && (!scopedBranchId.HasValue || p.BranchId == scopedBranchId.Value))
                               .ToList();
        }

        /// <summary>
        /// Kiểm tra trùng lặp tên đối tác trong cùng 1 công ty
        /// </summary>
        public bool IsExisted(string name, int tenantId, int branchId)
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int scopedBranchId = ResolveBranchScopeForWrite(branchId);
            string normalizedName = (name ?? string.Empty).Trim();
            return _partnerRepo.Find(p => p.PartnerName == normalizedName
                                       && p.TenantId == scopedTenantId
                                       && p.BranchId == scopedBranchId
                                       && p.IsActive).Any();
        }

        /// <summary>
        /// Tạo đối tác mới
        /// </summary>
        public void CreatePartner(Partner partner)
        {
            EnsureCanManagePartnerMasterData();

            if (partner == null)
                throw new ArgumentNullException(nameof(partner));

            int scopedTenantId = ResolveTenantScope(partner.TenantId);
            int scopedBranchId = ResolveBranchScopeForWrite(partner.BranchId);
            if (IsExisted(partner.PartnerName, scopedTenantId, scopedBranchId))
            {
                throw new InvalidOperationException($"Đối tác tên '{partner.PartnerName}' đã tồn tại!");
            }

            partner.TenantId = scopedTenantId;
            partner.BranchId = scopedBranchId;
            partner.IsActive = true;
            _partnerRepo.Add(partner);
            _partnerRepo.Save();
        }

        /// <summary>
        /// Cập nhật thông tin đối tác
        /// </summary>
        public void UpdatePartner(Partner partner)
        {
            EnsureCanManagePartnerMasterData();

            if (partner == null)
                throw new ArgumentNullException(nameof(partner));

            int scopedTenantId = ResolveTenantScope(partner.TenantId);
            int scopedBranchId = ResolveBranchScopeForWrite(partner.BranchId);
            int? readBranchScope = ResolveBranchScopeForRead();
            var existing = _partnerRepo.GetById(partner.PartnerId);
            if (existing == null || existing.TenantId != scopedTenantId || (readBranchScope.HasValue && existing.BranchId != readBranchScope.Value))
                throw new KeyNotFoundException("Không tìm thấy đối tác trong phạm vi tenant hiện tại.");

            existing.PartnerName = partner.PartnerName;
            existing.Phone = partner.Phone;
            existing.Address = partner.Address;
            existing.Type = partner.Type;
            existing.InitialDebt = partner.InitialDebt;
            existing.IsActive = partner.IsActive;
            existing.TenantId = scopedTenantId;
            existing.BranchId = scopedBranchId;

            _partnerRepo.Update(existing);
            _partnerRepo.Save();
        }

        /// <summary>
        /// Xóa đối tác (Soft Delete)
        /// 🔒 Chỉ đặt IsActive = false để bảo toàn lịch sử giao dịch/công nợ
        /// </summary>
        public void DeletePartner(int id)
        {
            EnsureCanManagePartnerMasterData();

            int scopedTenantId = ResolveTenantScope();
            int? readBranchScope = ResolveBranchScopeForRead();

            var partner = _partnerRepo.GetById(id);
            if (partner == null || partner.TenantId != scopedTenantId || (readBranchScope.HasValue && partner.BranchId != readBranchScope.Value))
            {
                throw new InvalidOperationException("Không tìm thấy đối tác cần xóa!");
            }

            partner.IsActive = false;
            _partnerRepo.Update(partner);
            _partnerRepo.Save();
        }
    }
}