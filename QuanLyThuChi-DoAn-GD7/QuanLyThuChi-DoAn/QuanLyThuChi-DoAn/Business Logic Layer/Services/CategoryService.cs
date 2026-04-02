using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.Data_Access_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class CategoryService
    {
        private readonly BaseRepository<TransactionCategory> _catRepo;

        public CategoryService(AppDbContext context)
        {
            _catRepo = new BaseRepository<TransactionCategory>(context);
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

        private static void EnsureCanManageCategoryMasterData()
        {
            if (!SessionManager.IsSuperAdmin && !SessionManager.IsTenantAdmin)
                throw new UnauthorizedAccessException("Bạn không có quyền cấu hình danh mục Thu/Chi.");
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

        public List<TransactionCategory> GetCategories(string type) // "IN" hoặc "OUT"
        {
            int scopedTenantId = ResolveTenantScope();
            int? scopedBranchId = ResolveBranchScopeForRead();
            return _catRepo.Find(c => c.TenantId == scopedTenantId
                                   && c.IsActive
                                   && c.Type == type
                                   && (!scopedBranchId.HasValue || c.BranchId == scopedBranchId.Value))
                           .OrderBy(c => c.CategoryName).ToList();
        }

        /// <summary>
        /// Lấy danh sách loại thu chi theo TenantId và loại (nếu có)
        /// </summary>
        public List<TransactionCategory> GetCategoriesByTenant(int tenantId, string type = null)
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();
            var query = _catRepo.Find(c => c.TenantId == scopedTenantId
                                        && c.IsActive
                                        && (!scopedBranchId.HasValue || c.BranchId == scopedBranchId.Value));
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(c => c.Type == type);
            }
            return query.OrderBy(c => c.CategoryName).ToList();
        }

        public List<TransactionCategory> GetCategoriesForCurrentSession(string type = null)
        {
            int scopedTenantId = ResolveTenantScope();
            int? scopedBranchId = ResolveBranchScopeForRead();
            var query = _catRepo.Find(c => c.TenantId == scopedTenantId
                                        && c.IsActive
                                        && (!scopedBranchId.HasValue || c.BranchId == scopedBranchId.Value));
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(c => c.Type == type);
            }
            return query.OrderBy(c => c.CategoryName).ToList();
        }

        /// <summary>
        /// Tạo loại thu chi mới
        /// </summary>
        public void CreateCategory(TransactionCategory category)
        {
            EnsureCanManageCategoryMasterData();

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            category.TenantId = ResolveTenantScope(category.TenantId);
            category.BranchId = ResolveBranchScopeForWrite(category.BranchId);
            category.IsActive = true;
            _catRepo.Add(category);
            _catRepo.Save();
        }

        /// <summary>
        /// Cập nhật loại thu chi
        /// </summary>
        public void UpdateCategory(TransactionCategory category)
        {
            EnsureCanManageCategoryMasterData();

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            int scopedTenantId = ResolveTenantScope(category.TenantId);
            int scopedBranchId = ResolveBranchScopeForWrite(category.BranchId);
            int? readBranchScope = ResolveBranchScopeForRead();
            var existing = _catRepo.GetById(category.CategoryId);
            if (existing == null || existing.TenantId != scopedTenantId || (readBranchScope.HasValue && existing.BranchId != readBranchScope.Value))
                throw new KeyNotFoundException("Không tìm thấy danh mục trong phạm vi tenant hiện tại.");

            existing.CategoryName = category.CategoryName;
            existing.Type = category.Type;
            existing.IsActive = category.IsActive;
            existing.TenantId = scopedTenantId;
            existing.BranchId = scopedBranchId;

            _catRepo.Update(existing);
            _catRepo.Save();
        }

        /// <summary>
        /// Xóa loại thu chi
        /// </summary>
        public void DeleteCategory(int categoryId)
        {
            EnsureCanManageCategoryMasterData();

            int scopedTenantId = ResolveTenantScope();
            int? readBranchScope = ResolveBranchScopeForRead();
            var existing = _catRepo.GetById(categoryId);
            if (existing == null || existing.TenantId != scopedTenantId || (readBranchScope.HasValue && existing.BranchId != readBranchScope.Value))
                throw new KeyNotFoundException("Không tìm thấy danh mục trong phạm vi tenant hiện tại.");

            existing.IsActive = false;
            _catRepo.Update(existing);
            _catRepo.Save();
        }
    }
}