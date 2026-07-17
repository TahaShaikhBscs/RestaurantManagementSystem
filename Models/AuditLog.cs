// ============================================
// Models/AuditLog.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Audit Log Model - Tracks all system activities
    /// </summary>
    public class AuditLog
    {
        public int AuditLogID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int UserID { get; set; }
        public string ActionType { get; set; } // Insert, Update, Delete, Login, Logout, etc.
        public string Module { get; set; }
        public string TableName { get; set; }
        public int? RecordID { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
    }
}