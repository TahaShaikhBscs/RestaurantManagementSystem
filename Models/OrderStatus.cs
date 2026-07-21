// ============================================
// Models/OrderStatus.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Order Status Model - Represents order status lookup values
    /// </summary>
    public class OrderStatus
    {
        public int OrderStatusID { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public string ColorCode { get; set; }
        public bool IsFinal { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}