
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Payment Model - Represents a payment transaction
    /// </summary>
    public class Payment
    {
        public int PaymentID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public string PaymentMethod { get; set; } // Cash, Card, Online
        public decimal Amount { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentStatus { get; set; } // Pending, Completed, Failed
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}