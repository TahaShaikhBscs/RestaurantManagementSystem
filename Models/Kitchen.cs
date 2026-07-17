// ============================================
// Models/Kitchen.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Kitchen Model - Represents kitchen display and kitchen order management
    /// </summary>
    public class Kitchen
    {
        public int KitchenDisplayID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string DisplayName { get; set; }
        public string DisplayType { get; set; } // Standard, Quick, Pizza, etc.
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
    }

    /// <summary>
    /// Kitchen Order Model - Represents individual kitchen order items
    /// </summary>
    public class KitchenOrder
    {
        public int KitchenOrderID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int OrderItemID { get; set; }
        public int? KitchenDisplayID { get; set; }
        public string Status { get; set; } // Pending, Cooking, Ready, Served, Cancelled
        public int Priority { get; set; } // 1=Normal, 2=High, 3=Urgent
        public int? PreparationTime { get; set; } // Actual preparation time in minutes
        public DateTime? StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public string Notes { get; set; }
        public bool _Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties from joins
        public string OrderNumber { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Instructions { get; set; }
        public int EstimatedTime { get; set; }
        public string TableNumber { get; set; }
        public string PriorityStatus { get; set; } // Normal, Urgent, Delayed
        public string DisplayName { get; set; }
    }
}