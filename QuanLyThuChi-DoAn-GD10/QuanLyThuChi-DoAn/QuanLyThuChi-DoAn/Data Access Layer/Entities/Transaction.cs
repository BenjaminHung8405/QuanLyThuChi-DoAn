using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public long TransId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public int FundId { get; set; }
        [ForeignKey("FundId")]
        public virtual CashFund CashFund { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual TransactionCategory Category { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryNameSnapshot { get; set; } = string.Empty;

        public int? PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }

        [StringLength(255)]
        public string? PartnerNameSnapshot { get; set; }

        public long? DebtId { get; set; }
        [ForeignKey("DebtId")]
        public virtual Debt Debt { get; set; }

        // Branch association - assigned from SessionManager when creating transactions
        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public DateTime TransDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18, 0)")]
        public decimal SubTotal { get; set; }

        public int? TaxId { get; set; }
        [ForeignKey("TaxId")]
        public virtual Tax Tax { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(10)]
        public string TransType { get; set; } // 'IN' hoặc 'OUT'

        [StringLength(50)]
        public string RefNo { get; set; }

        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "COMPLETED";

        public long? TransferRefId { get; set; }
        [ForeignKey("TransferRefId")]
        public virtual Transaction TransferRefTransaction { get; set; }



        public bool IsActive { get; set; } = true;
    }
}