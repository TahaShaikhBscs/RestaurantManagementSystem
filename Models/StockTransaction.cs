// ============================================
// Models/StockTransaction.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Stock Transaction Model - Tracks all inventory movements
    /// </summary>
    public class StockTransaction
    {
        public int StockTransactionID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int StockID { get; set; }
        public string TransactionType { get; set; } // StockIn, StockOut, Adjustment, Return, Waste
        public string ReferenceType { get; set; } // Purchase, Order, Adjustment, etc.
        public int? ReferenceID { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PreviousQuantity { get; set; }
        public decimal? NewQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Notes { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string BranchName { get; set; }
        public string CreatedByUserName { get; set; }
    }

    /// <summary>
    /// Stock Adjustment Model - Represents stock adjustments
    /// </summary>
    public class StockAdjustment
    {
        public int StockAdjustmentID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string AdjustmentNumber { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string AdjustmentType { get; set; } // Add, Remove, Correct
        public string Reason { get; set; }
        public decimal TotalAdjustment { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string Notes { get; set; }
        public bool _Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string ApprovedByName { get; set; }
        public List<StockAdjustmentItem> AdjustmentItems { get; set; }
    }

    /// <summary>
    /// Stock Adjustment Item Model
    /// </summary>
    public class StockAdjustmentItem
    {
        public int StockAdjustmentItemID { get; set; }
        public int StockAdjustmentID { get; set; }
        public int StockID { get; set; }
        public decimal Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
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
    }
}