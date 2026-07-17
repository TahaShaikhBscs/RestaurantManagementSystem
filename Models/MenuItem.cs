
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Menu Item Model - Represents a product in the restaurant menu
    /// </summary>
    public class MenuItem
    {
        public int MenuItemID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int CategoryID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public string Image { get; set; }
        public int PreparationTime { get; set; }
        public bool IsKitchen { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int MinStockAlert { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string CategoryName { get; set; }
        public string BranchName { get; set; }
    }
}
