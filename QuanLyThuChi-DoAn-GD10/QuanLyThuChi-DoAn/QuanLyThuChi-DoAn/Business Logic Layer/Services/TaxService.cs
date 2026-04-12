using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class TaxService
    {
        private readonly AppDbContext _context;

        public TaxService(AppDbContext context)
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

        private static void EnsureCanManageTaxMasterData()
        {
            if (!SessionManager.IsTenantAdmin && !SessionManager.IsBranchManager)
                throw new UnauthorizedAccessException("Bạn không có quyền cấu hình danh mục thuế.");
        }

        // 1. Lấy tất cả danh mục thuế (Cả khóa và mở - Dành cho màn hình Quản lý)
        public async Task<List<Tax>> GetAllTaxesAsync()
        {
            EnsureCanManageTaxMasterData();
            int scopedTenantId = ResolveTenantScope();

            return await _context.Taxes
                .AsNoTracking()
                .Where(t => t.TenantId == scopedTenantId)
                .OrderBy(t => t.Rate)
                .ThenBy(t => t.TaxName)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        // 2. Lấy các loại thuế đang hoạt động (Dành cho ComboBox ở màn hình Nhập phiếu)
        public async Task<List<Tax>> GetActiveTaxesAsync()
        {
            int scopedTenantId = ResolveTenantScope();

            return await _context.Taxes
                .AsNoTracking()
                .Where(t => t.TenantId == scopedTenantId && t.IsActive)
                .OrderBy(t => t.Rate)
                .ThenBy(t => t.TaxName)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        // 3. Thêm mới
        public async Task<bool> AddTaxAsync(Tax tax)
        {
            EnsureCanManageTaxMasterData();

            if (tax == null)
                throw new ArgumentNullException(nameof(tax));

            string normalizedTaxName = (tax.TaxName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedTaxName))
                throw new InvalidOperationException("Tên loại thuế không được để trống.");

            int scopedTenantId = ResolveTenantScope(tax.TenantId);

            bool exists = await _context.Taxes
                .AsNoTracking()
                .AnyAsync(t => t.TenantId == scopedTenantId
                            && t.TaxName.ToLower() == normalizedTaxName.ToLower())
                .ConfigureAwait(false);

            if (exists)
                throw new InvalidOperationException("Tên loại thuế này đã tồn tại!");

            tax.TenantId = scopedTenantId;
            tax.TaxName = normalizedTaxName;
            tax.IsActive = true;

            _context.Taxes.Add(tax);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        // 4. Cập nhật (Sửa)
        public async Task<bool> UpdateTaxAsync(Tax tax)
        {
            EnsureCanManageTaxMasterData();

            if (tax == null)
                throw new ArgumentNullException(nameof(tax));

            string normalizedTaxName = (tax.TaxName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedTaxName))
                throw new InvalidOperationException("Tên loại thuế không được để trống.");

            int scopedTenantId = ResolveTenantScope(tax.TenantId);

            var existing = await _context.Taxes
                .FirstOrDefaultAsync(t => t.TaxId == tax.TaxId && t.TenantId == scopedTenantId)
                .ConfigureAwait(false);

            if (existing == null)
                throw new KeyNotFoundException("Không tìm thấy dữ liệu!");

            bool existsDuplicate = await _context.Taxes
                .AsNoTracking()
                .AnyAsync(t => t.TenantId == scopedTenantId
                            && t.TaxId != tax.TaxId
                            && t.TaxName.ToLower() == normalizedTaxName.ToLower())
                .ConfigureAwait(false);

            if (existsDuplicate)
                throw new InvalidOperationException("Tên loại thuế này đã tồn tại!");

            existing.TaxName = normalizedTaxName;
            existing.Rate = tax.Rate;

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        // 5. Khóa / Mở khóa (Soft Delete)
        public async Task<bool> ToggleTaxStatusAsync(int taxId)
        {
            EnsureCanManageTaxMasterData();

            int scopedTenantId = ResolveTenantScope();

            var tax = await _context.Taxes
                .FirstOrDefaultAsync(t => t.TaxId == taxId && t.TenantId == scopedTenantId)
                .ConfigureAwait(false);

            if (tax == null)
                throw new KeyNotFoundException("Không tìm thấy dữ liệu!");

            tax.IsActive = !tax.IsActive;
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}
