// ============================================
// Models/POS/OrderModifier.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// Order Modifier Model
    /// </summary>
    public class OrderModifier
    {
        public int OrderModifierID { get; set; }
        public int OrderItemID { get; set; }
        public int ModifierID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Display Properties
        public string ModifierName { get; set; }
        public string ModifierGroupName { get; set; }
    }
}