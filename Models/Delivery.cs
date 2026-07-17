// ============================================
// Models/Delivery.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Delivery Order Model - Manages delivery orders
    /// </summary>
    public class DeliveryOrder
    {
        public int DeliveryOrderID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public string DeliveryPerson { get; set; }
        public string DeliveryPersonPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryInstructions { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public decimal DeliveryFee { get; set; }
        public string DeliveryStatus { get; set; } // Pending, OnTheWay, Delivered, Failed
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string BranchName { get; set; }
    }
}