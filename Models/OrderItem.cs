
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Order Item Model - Represents individual items in an order
    /// </summary>
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int MenuItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Instructions { get; set; }
        public string KitchenStatus { get; set; } // Pending, Cooking, Ready, Served
        public int? PreparationTime { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string ItemName { get; set; }
        public string Barcode { get; set; }
    }
}