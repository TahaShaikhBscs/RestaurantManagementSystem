
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Stock Model - Represents inventory items
    /// </summary>
    public class Stock
    {
        public int StockID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost { get; set; }
        public int MinStockLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public string Location { get; set; }
        public string Barcode { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}