
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Customer Model - Represents a restaurant customer
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CustomerType { get; set; } // Regular, VIP
        public int LoyaltyPoints { get; set; }
        public string MembershipNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public decimal TotalPurchases { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
