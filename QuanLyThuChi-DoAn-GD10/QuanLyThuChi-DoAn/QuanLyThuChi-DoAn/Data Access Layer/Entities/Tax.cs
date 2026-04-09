using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Taxes")]
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [Required]
        [StringLength(100)]
        public string TaxName { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Rate { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
