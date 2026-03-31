using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        [StringLength(50)]
        public string PermissionCode { get; set; }

        [StringLength(100)]
        public string PermissionName { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}