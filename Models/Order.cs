
using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Order Model - Represents a customer order
    /// </summary>
    public class Order
    {
        public int OrderID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int? TableID { get; set; }
        public int? CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public string OrderType { get; set; } // Dine In, Take Away, Delivery
        public string OrderStatus { get; set; } // Pending, Cooking, Ready, Completed, Cancelled
        public string PaymentStatus { get; set; } // Pending, Partial, Completed
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string SpecialInstructions { get; set; }
        public string OrderSource { get; set; }
        public bool IsVoid { get; set; }
        public string VoidReason { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string TableNumber { get; set; }
        public string CreatedByUserName { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
