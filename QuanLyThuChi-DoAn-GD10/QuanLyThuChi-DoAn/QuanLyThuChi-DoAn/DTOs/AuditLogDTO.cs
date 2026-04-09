using System;

namespace QuanLyThuChi_DoAn.DTOs
{
    public class AuditLogDTO
    {
        public long LogId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActorDisplay { get; set; } = string.Empty;
        public string ActionTypeDisplay { get; set; } = string.Empty;
        public string ActionCode { get; set; } = string.Empty;
        public string ModuleDisplay { get; set; } = string.Empty;
        public string ReferenceCode { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class AuditLogUserOptionDTO
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}