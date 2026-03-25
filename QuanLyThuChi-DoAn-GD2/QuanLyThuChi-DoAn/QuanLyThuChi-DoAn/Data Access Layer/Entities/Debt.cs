using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Debts")]
    public class Debt
    {
        [Key]
        public long DebtId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public int PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }

        [Required]
        [StringLength(20)]
        public string DebtType { get; set; } // 'RECEIVABLE' hoặc 'PAYABLE'

        [Column(TypeName = "decimal(18, 0)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal PaidAmount { get; set; } = 0;

        public DateTime? DueDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "PENDING";

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
