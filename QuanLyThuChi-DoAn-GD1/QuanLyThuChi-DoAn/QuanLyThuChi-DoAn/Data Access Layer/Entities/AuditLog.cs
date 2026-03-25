using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public long LogId { get; set; }

        public int TenantId { get; set; }
        public int? UserId { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(20)]
        public string ActionType { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(50)]
        public string RecordId { get; set; }

        public string OldValues { get; set; }
        public string NewValues { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}
