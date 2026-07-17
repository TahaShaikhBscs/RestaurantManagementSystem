// ============================================
// Models/Settings.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// System Settings Model - Manages application settings
    /// </summary>
    public class SystemSetting
    {
        public int SettingID { get; set; }
        public int CompanyID { get; set; }
        public string Category { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string DataType { get; set; } // String, Int, Decimal, Boolean, Date, JSON
        public string Description { get; set; }
        public bool IsEditable { get; set; }
        public bool IsSystem { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Tax Rate Model
    /// </summary>
    public class TaxRate
    {
        public int TaxRateID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string TaxName { get; set; }
        public decimal TaxRateA { get; set; }
        public string TaxType { get; set; } // Percentage, Fixed
        public string ApplyTo { get; set; } // All, Food, Beverage, Service
        public bool IsDefault { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Discount Coupon Model
    /// </summary>
    public class DiscountCoupon
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
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}