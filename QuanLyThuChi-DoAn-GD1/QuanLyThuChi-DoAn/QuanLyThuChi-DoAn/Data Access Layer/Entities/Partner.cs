using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Partners")]
    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [Required]
        [StringLength(255)]
        public string PartnerName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Type { get; set; } // 'CUSTOMER' hoặc 'SUPPLIER'

        [Column(TypeName = "decimal(18, 0)")]
        public decimal InitialDebt { get; set; } = 0;
    }
}