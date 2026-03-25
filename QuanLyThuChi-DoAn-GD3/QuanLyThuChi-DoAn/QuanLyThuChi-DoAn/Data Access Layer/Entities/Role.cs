using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}