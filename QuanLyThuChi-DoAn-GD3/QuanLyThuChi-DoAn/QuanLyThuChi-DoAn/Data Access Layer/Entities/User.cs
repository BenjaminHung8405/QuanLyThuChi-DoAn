using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}