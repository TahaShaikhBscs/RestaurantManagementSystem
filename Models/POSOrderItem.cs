// ============================================
// Models/POS/POSOrderItem.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// POS Order Item Model
    /// </summary>
    public class POSOrderItem
    {
        #region Properties

        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int MenuItemID { get; set; }
        public int? DealID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Instructions { get; set; }
        public int KitchenStatusID { get; set; }
        public int? PreparationTime { get; set; }
        public bool IsVoid { get; set; }
        public string VoidReason { get; set; }
        public bool IsPrinted { get; set; }
        public int PrintCount { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string DealName { get; set; }
        public string KitchenStatusName { get; set; }
        public string KitchenStatusColor { get; set; }

        #endregion

        #region Navigation Properties

        public List<OrderModifier> Modifiers { get; set; }

        #endregion
    }
}