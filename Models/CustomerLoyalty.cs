// ============================================
// Models/CustomerLoyalty.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Customer Loyalty Model - Manages customer loyalty points
    /// </summary>
    public class CustomerLoyalty
    {
        public int LoyaltyID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int CustomerID { get; set; }
        public int Points { get; set; }
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string MembershipLevel { get; set; } // Bronze, Silver, Gold, Platinum
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// Loyalty Transaction Model
    /// </summary>
    public class LoyaltyTransaction
    {
        public int LoyaltyTransactionID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int CustomerID { get; set; }
        public string TransactionType { get; set; } // Earn, Redeem, Adjust
        public int Points { get; set; }
        public string ReferenceType { get; set; } // Order, Payment, etc.
        public int? ReferenceID { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
    }
}