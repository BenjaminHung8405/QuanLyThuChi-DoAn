using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("RolePermissions")]
    public class RolePermission
    {
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [StringLength(50)]
        public string PermissionCode { get; set; }
        [ForeignKey("PermissionCode")]
        public virtual Permission Permission { get; set; }
    }
}