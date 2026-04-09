using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class TaxService
    {
        private readonly AppDbContext _context;

        public TaxService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tax>> GetActiveTaxesAsync()
        {
            if (!SessionManager.CurrentTenantId.HasValue || SessionManager.CurrentTenantId.Value <= 0)
            {
                return new List<Tax>();
            }

            int currentTenantId = SessionManager.CurrentTenantId.Value;

            return await _context.Taxes
                .AsNoTracking()
                .Where(t => t.TenantId == currentTenantId && t.IsActive)
                .OrderBy(t => t.Rate)
                .ThenBy(t => t.TaxName)
                .ToListAsync();
        }
    }
}
