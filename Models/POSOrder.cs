// ============================================
// Models/POS/POSOrder.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// POS Order Model - Complete order with all details
    /// </summary>
    public class POSOrder
    {
        #region Properties

        public int OrderID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int? TableID { get; set; }
        public int? CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public string OrderType { get; set; }
        public int OrderStatusID { get; set; }
        public string PaymentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public decimal TipAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public int? EstimatedTime { get; set; }
        public int? ActualTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string SpecialInstructions { get; set; }
        public string OrderSource { get; set; }
        public bool IsVoid { get; set; }
        public string VoidReason { get; set; }
        public DateTime? VoidDate { get; set; }
        public int? VoidBy { get; set; }
        public bool IsHeld { get; set; }
        public DateTime? HeldDate { get; set; }
        public int? HeldBy { get; set; }
        public bool IsKitchenSent { get; set; }
        public DateTime? KitchenSentDate { get; set; }
        public bool IsPrinted { get; set; }
        public int PrintCount { get; set; }
        public int? ShiftID { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string TableNumber { get; set; }
        public string OrderStatusName { get; set; }
        public string OrderStatusColor { get; set; }
        public string CreatedByUserName { get; set; }

        #endregion

        #region Navigation Properties

        public List<POSOrderItem> OrderItems { get; set; }
        public List<POSPayment> Payments { get; set; }
        public List<OrderCoupon> Coupons { get; set; }

        #endregion
    }
}