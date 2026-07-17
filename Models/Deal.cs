// ============================================
// Models/Deal.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Deal Model - Represents a deal/combo/meal in the restaurant
    /// </summary>
    public class Deal
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the deal
        /// </summary>
        public int DealID { get; set; }

        /// <summary>
        /// Company ID this deal belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this deal belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Name of the deal
        /// </summary>
        public string DealName { get; set; }

        /// <summary>
        /// Description of the deal
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of the deal
        /// </summary>
        public decimal DealPrice { get; set; }

        /// <summary>
        /// Image path for the deal
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Start date of the deal
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date of the deal
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Status of the deal (Active/Inactive)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the deal was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the deal was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the deal
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the deal
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Number of items in the deal
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Original price of all items combined
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Discount amount
        /// </summary>
        public decimal Discount
        {
            get { return OriginalPrice - DealPrice; }
        }

        /// <summary>
        /// Discount percentage
        /// </summary>
        public decimal DiscountPercentage
        {
            get
            {
                if (OriginalPrice <= 0) return 0;
                return (Discount / OriginalPrice) * 100;
            }
        }

        /// <summary>
        /// Status text for display
        /// </summary>
        public string StatusText
        {
            get { return Status ? "Active" : "Inactive"; }
        }

        /// <summary>
        /// Whether the deal is currently valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (!Status) return false;
                if (StartDate.HasValue && StartDate.Value > DateTime.Now) return false;
                if (EndDate.HasValue && EndDate.Value < DateTime.Now) return false;
                return true;
            }
        }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// List of items in the deal
        /// </summary>
        public List<DealItem> DealItems { get; set; }

        #endregion
    }
}