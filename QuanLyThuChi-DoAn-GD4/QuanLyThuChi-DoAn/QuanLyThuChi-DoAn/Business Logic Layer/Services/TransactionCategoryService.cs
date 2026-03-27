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

        /// <summary>
        /// Get all active categories for a tenant, optionally filtered by keyword
        /// </summary>
        /// <param name="tenantId">Tenant ID for multi-tenancy isolation</param>
        /// <param name="keyword">Optional keyword to search by CategoryName (will be normalized)</param>
        /// <returns>List of TransactionCategory ordered by Type and CategoryName</returns>
        public List<TransactionCategory> GetCategories(int tenantId, string keyword = "")
        {
            // ✅ Multi-tenancy: Filter by TenantId and IsActive
            var query = _context.TransactionCategories.Where(c => c.TenantId == tenantId && c.IsActive == true);

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
            return _context.TransactionCategories
                .FirstOrDefault(c => c.CategoryId == categoryId && c.TenantId == tenantId);
        }

        /// <summary>
        /// Create new transaction category with validation
        /// </summary>
        /// <param name="category">TransactionCategory object to create</param>
        public void CreateCategory(TransactionCategory category)
        {
            // ✅ Validation
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name cannot be empty");

            // ✅ Check for duplicate category name within same tenant and type
            var exists = _context.TransactionCategories
                .Any(c => c.TenantId == category.TenantId
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
            // ✅ Validation
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name cannot be empty");

            // ✅ Check for duplicate category name (excluding current record)
            var exists = _context.TransactionCategories
                .Any(c => c.TenantId == category.TenantId
                    && c.Type == category.Type
                    && c.CategoryName.ToLower() == category.CategoryName.ToLower()
                    && c.CategoryId != category.CategoryId
                    && c.IsActive);

            if (exists)
                throw new InvalidOperationException($"Category '{category.CategoryName}' already exists for this type");

            _context.TransactionCategories.Update(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete transaction category (Soft Delete using IsActive flag)
        /// </summary>
        public void DeleteCategory(int categoryId, int tenantId)
        {
            var category = _context.TransactionCategories
                .FirstOrDefault(c => c.CategoryId == categoryId && c.TenantId == tenantId);

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
            var query = _context.TransactionCategories
                .Where(c => c.TenantId == tenantId
                    && c.Type == type
                    && c.CategoryName.ToLower() == categoryName.ToLower()
                    && c.IsActive);

            if (excludeCategoryId.HasValue)
                query = query.Where(c => c.CategoryId != excludeCategoryId.Value);

            return query.Any();
        }
    }
}
