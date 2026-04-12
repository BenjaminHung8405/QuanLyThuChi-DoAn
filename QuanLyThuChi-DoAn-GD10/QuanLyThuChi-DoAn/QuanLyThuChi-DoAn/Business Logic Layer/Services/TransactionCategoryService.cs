using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    /// <summary>
    /// Service for managing Transaction Categories (Chi tiêu / Thu nhập)
    /// Handles CRUD operations with multi-tenancy support
    /// </summary>
    public class TransactionCategoryService
    {
        private readonly AppDbContext _context;

        public TransactionCategoryService(AppDbContext context)
        {
            _context = context;
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
            if (!SessionManager.IsTenantAdmin && !SessionManager.IsBranchManager)
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

        /// <summary>
        /// Get all active categories for a tenant, optionally filtered by keyword
        /// </summary>
        /// <param name="tenantId">Tenant ID for multi-tenancy isolation</param>
        /// <param name="keyword">Optional keyword to search by CategoryName (will be normalized)</param>
        /// <returns>List of TransactionCategory ordered by Type and CategoryName</returns>
        public List<TransactionCategory> GetCategories(int tenantId, string keyword = "")
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();

            // ✅ Multi-tenancy: Filter by TenantId and IsActive
            var query = _context.TransactionCategories
                .Where(c => c.TenantId == scopedTenantId && c.IsActive == true)
                .Where(c => !scopedBranchId.HasValue || c.BranchId == scopedBranchId.Value);

            // ✅ Optional keyword search with Vietnamese accent normalization
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string keywordNormalized = TextUtility.RemoveVietnameseAccents(keyword);
                query = query.Where(c =>
                    TextUtility.RemoveVietnameseAccents(c.CategoryName).Contains(keywordNormalized)
                );
            }

            return query
                .OrderBy(c => c.Type)
                .ThenBy(c => c.CategoryName)
                .ToList();
        }

        /// <summary>
        /// Get category by ID with tenant validation
        /// </summary>
        public TransactionCategory GetCategoryById(int categoryId, int tenantId)
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();
            return _context.TransactionCategories
                .FirstOrDefault(c => c.CategoryId == categoryId
                                  && c.TenantId == scopedTenantId
                                  && c.IsActive
                                  && (!scopedBranchId.HasValue || c.BranchId == scopedBranchId.Value));
        }

        /// <summary>
        /// Create new transaction category with validation
        /// </summary>
        /// <param name="category">TransactionCategory object to create</param>
        public void CreateCategory(TransactionCategory category)
        {
            EnsureCanManageCategoryMasterData();

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // ✅ Validation
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name cannot be empty");

            int scopedTenantId = ResolveTenantScope(category.TenantId);
            int scopedBranchId = ResolveBranchScopeForWrite(category.BranchId);
            category.TenantId = scopedTenantId;
            category.BranchId = scopedBranchId;

            // ✅ Check for duplicate category name within same tenant and type
            var exists = _context.TransactionCategories
                .Any(c => c.TenantId == scopedTenantId
                    && c.BranchId == scopedBranchId
                    && c.Type == category.Type
                    && c.CategoryName.ToLower() == category.CategoryName.ToLower()
                    && c.IsActive);

            if (exists)
                throw new InvalidOperationException($"Category '{category.CategoryName}' already exists for this type");

            // ✅ Set default IsActive = true for new categories
            category.IsActive = true;

            _context.TransactionCategories.Add(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Update existing transaction category
        /// </summary>
        public void UpdateCategory(TransactionCategory category)
        {
            EnsureCanManageCategoryMasterData();

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // ✅ Validation
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name cannot be empty");

            int scopedTenantId = ResolveTenantScope(category.TenantId);
            int scopedBranchId = ResolveBranchScopeForWrite(category.BranchId);
            int? readBranchScope = ResolveBranchScopeForRead();
            var existingCategory = _context.TransactionCategories
                .FirstOrDefault(c => c.CategoryId == category.CategoryId
                                  && c.TenantId == scopedTenantId
                                  && c.IsActive
                                  && (!readBranchScope.HasValue || c.BranchId == readBranchScope.Value));

            if (existingCategory == null)
                throw new KeyNotFoundException("Không tìm thấy danh mục trong phạm vi tenant hiện tại.");

            // ✅ Check for duplicate category name (excluding current record)
            var exists = _context.TransactionCategories
                .Any(c => c.TenantId == scopedTenantId
                    && c.BranchId == scopedBranchId
                    && c.Type == category.Type
                    && c.CategoryName.ToLower() == category.CategoryName.ToLower()
                    && c.CategoryId != category.CategoryId
                    && c.IsActive);

            if (exists)
                throw new InvalidOperationException($"Category '{category.CategoryName}' already exists for this type");

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Type = category.Type;
            existingCategory.TenantId = scopedTenantId;
            existingCategory.BranchId = scopedBranchId;

            _context.TransactionCategories.Update(existingCategory);
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete transaction category (Soft Delete using IsActive flag)
        /// </summary>
        public void DeleteCategory(int categoryId, int tenantId)
        {
            EnsureCanManageCategoryMasterData();

            int scopedTenantId = ResolveTenantScope(tenantId);
            int? readBranchScope = ResolveBranchScopeForRead();
            var category = _context.TransactionCategories
                .FirstOrDefault(c => c.CategoryId == categoryId
                                  && c.TenantId == scopedTenantId
                                  && c.IsActive
                                  && (!readBranchScope.HasValue || c.BranchId == readBranchScope.Value));

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            // ✅ Soft Delete: Set IsActive = false instead of hard delete
            category.IsActive = false;
            _context.TransactionCategories.Update(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Check if category name already exists for tenant and type
        /// </summary>
        public bool IsCategoryNameExists(string categoryName, string type, int tenantId, int? excludeCategoryId = null)
        {
            int scopedTenantId = ResolveTenantScope(tenantId);
            int? scopedBranchId = ResolveBranchScopeForRead();
            var query = _context.TransactionCategories
                .Where(c => c.TenantId == scopedTenantId
                    && (!scopedBranchId.HasValue || c.BranchId == scopedBranchId.Value)
                    && c.Type == type
                    && c.CategoryName.ToLower() == categoryName.ToLower()
                    && c.IsActive);

            if (excludeCategoryId.HasValue)
                query = query.Where(c => c.CategoryId != excludeCategoryId.Value);

            return query.Any();
        }
    }
}
