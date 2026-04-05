using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("CashFunds")]
    public class CashFund
    {
        [Key]
        public int FundId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [Required]
        [StringLength(100)]
        public string FundName { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal Balance { get; set; } = 0;

        // Soft-delete flag: true = active, false = hidden/deleted
        public bool IsActive { get; set; } = true;
    }
}