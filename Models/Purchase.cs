// ============================================
// Models/Purchase.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Purchase Order Model - Represents a purchase order to suppliers
    /// </summary>
    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int SupplierID { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentStatus { get; set; } // Pending, Partial, Paid
        public string OrderStatus { get; set; } // Draft, Sent, Received, Cancelled
        public string Notes { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
        public string CreatedByUserName { get; set; }
        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    }

    /// <summary>
    /// Purchase Order Item Model - Represents items in a purchase order
    /// </summary>
    public class PurchaseOrderItem
    {
        public int PurchaseOrderItemID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int StockID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public string Notes { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string Category { get; set; }
    }
}