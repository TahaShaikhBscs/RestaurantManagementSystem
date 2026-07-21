// ============================================
// Models/OrderCoupon.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Order Coupon Model - Tracks coupons applied to orders
    /// </summary>
    public class OrderCoupon
    {
        public int OrderCouponID { get; set; }
        public int OrderID { get; set; }
        public int CouponID { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime AppliedDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Display Properties
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public string DiscountType { get; set; }
        public decimal CouponDiscountAmount { get; set; }
    }
}