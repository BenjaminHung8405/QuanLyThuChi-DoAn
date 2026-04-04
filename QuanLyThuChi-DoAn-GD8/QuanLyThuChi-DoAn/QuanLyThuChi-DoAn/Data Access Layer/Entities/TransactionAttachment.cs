using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuChi_DoAn
{
    [Table("TransactionAttachments")]
    public class TransactionAttachment
    {
        [Key]
        public long AttachmentId { get; set; }

        public long TransId { get; set; }
        [ForeignKey("TransId")]
        public virtual Transaction Transaction { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;
    }
}