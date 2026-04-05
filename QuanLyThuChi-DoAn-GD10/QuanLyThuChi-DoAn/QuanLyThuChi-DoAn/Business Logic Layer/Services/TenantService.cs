using System.Collections.Generic;
using System.Linq;
using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class TenantService
    {
        private readonly AppDbContext _context;

        public TenantService(AppDbContext context)
        {
            _context = context;
        }

        public List<Tenant> GetActiveTenants()
        {
            return _context.Tenants.Where(t => t.IsActive).ToList();
        }

        public List<Tenant> GetAllTenants()
        {
            return _context.Tenants.ToList();
        }

        public Tenant GetTenantById(int tenantId)
        {
            return _context.Tenants.FirstOrDefault(t => t.TenantId == tenantId && t.IsActive);
        }
    }
}
