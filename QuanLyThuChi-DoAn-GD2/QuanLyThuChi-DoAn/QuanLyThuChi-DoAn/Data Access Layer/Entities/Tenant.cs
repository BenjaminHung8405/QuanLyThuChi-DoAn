using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuChi_DoAn
{
    [Table("Tenants")]
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }

        [Required]
        [StringLength(50)]
        public string TenantCode { get; set; }

        [Required]
        [StringLength(255)]
        public string TenantName { get; set; }

        [StringLength(50)]
        public string TaxCode { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}