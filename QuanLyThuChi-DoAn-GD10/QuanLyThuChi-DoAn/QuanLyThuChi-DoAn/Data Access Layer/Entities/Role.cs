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

        [Required]
        [StringLength(50)]
        public string RoleCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public int PriorityLevel { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}