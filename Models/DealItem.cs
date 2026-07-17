// ============================================
// Models/DealItem.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Deal Item Model - Represents a menu item in a deal
    /// </summary>
    public class DealItem
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the deal item
        /// </summary>
        public int DealItemID { get; set; }

        /// <summary>
        /// Deal ID this item belongs to
        /// </summary>
        public int DealID { get; set; }

        /// <summary>
        /// Menu item ID
        /// </summary>
        public int MenuItemID { get; set; }

        /// <summary>
        /// Quantity of this item in the deal
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Status of the deal item
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the deal item was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the deal item was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the deal item
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the deal item
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties (from joins)

        /// <summary>
        /// Name of the menu item
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Price of the menu item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Description of the menu item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Image of the menu item
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Category ID of the menu item
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Category name of the menu item
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Total price for this item (Quantity * Price)
        /// </summary>
        public decimal TotalPrice
        {
            get { return Quantity * Price; }
        }

        #endregion
    }
}