// ============================================
// Models/PaymentMethod.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Payment Method Model - Represents payment method lookup values
    /// </summary>
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; }
        public string MethodCode { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}