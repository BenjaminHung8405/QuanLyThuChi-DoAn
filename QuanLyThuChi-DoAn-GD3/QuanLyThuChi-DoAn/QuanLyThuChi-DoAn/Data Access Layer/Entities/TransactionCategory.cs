using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("TransactionCategories")]
    public class TransactionCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(10)]
        public string Type { get; set; } // 'IN' hoặc 'OUT'
    }
}