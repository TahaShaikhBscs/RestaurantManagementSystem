// ============================================
// Models/Coupon.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Coupon Model - Represents discount coupons
    /// </summary>
    public class Coupon
    {
        public int CouponID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } // Percentage, Fixed
        public decimal DiscountAmount { get; set; }
        public decimal MinimumOrder { get; set; }
        public decimal MaximumDiscount { get; set; }
        public bool IsUnlimited { get; set; }
        public int TotalUses { get; set; }
        public int MaxUses { get; set; }
        public int UsedCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string ApplicableTo { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsValid
        {
            get
            {
                if (!IsActive || !Status) return false;
                if (StartDate.HasValue && StartDate.Value > DateTime.Now) return false;
                if (EndDate.HasValue && EndDate.Value < DateTime.Now) return false;
                if (!IsUnlimited && UsedCount >= MaxUses) return false;
                return true;
            }
        }
    }
}