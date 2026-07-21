// ============================================
// Models/POS/POSPayment.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// POS Payment Model
    /// </summary>
    public class POSPayment
    {
        public int PaymentID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsRefund { get; set; }
        public decimal RefundAmount { get; set; }
        public DateTime? RefundDate { get; set; }
        public int? RefundBy { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Display Properties
        public string PaymentMethodName { get; set; }
        public string PaymentMethodIcon { get; set; }
    }
}