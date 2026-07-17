// ============================================
// Models/OrderTimeline.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Order Timeline Model - Tracks order status changes
    /// </summary>
    public class OrderTimeline
    {
        public int OrderTimelineID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public string Status { get; set; }
        public string PreviousStatus { get; set; }
        public string Notes { get; set; }
        public DateTime TimelineDate { get; set; }
        public bool _Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string CreatedByUserName { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
    }
}