// ============================================
// Models/Offer.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Offer Model - Represents a promotional offer on the website
    /// </summary>
    public class Offer
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the offer
        /// </summary>
        public int OfferID { get; set; }

        /// <summary>
        /// Company ID this offer belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this offer belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Title of the offer
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the offer
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Offer code for redemption
        /// </summary>
        public string OfferCode { get; set; }

        /// <summary>
        /// Type of discount (Percentage, Fixed)
        /// </summary>
        public string DiscountType { get; set; }

        /// <summary>
        /// Value of the discount
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// Minimum order amount to apply the offer
        /// </summary>
        public decimal MinOrderAmount { get; set; }

        /// <summary>
        /// Maximum discount amount
        /// </summary>
        public decimal MaxDiscount { get; set; }

        /// <summary>
        /// Path to the offer image
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Start date of the offer
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date of the offer
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Whether the offer is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Whether the offer is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Usage limit for the offer
        /// </summary>
        public int UsageLimit { get; set; }

        /// <summary>
        /// Number of times the offer has been used
        /// </summary>
        public int UsedCount { get; set; }

        /// <summary>
        /// Terms and conditions of the offer
        /// </summary>
        public string TermsAndConditions { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Status of the offer
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the offer was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the offer was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the offer
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the offer
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the offer image URL
        /// </summary>
        public string ImageUrl
        {
            get { return string.IsNullOrEmpty(ImagePath) ? "/Assets/Images/default-offer.jpg" : ImagePath; }
        }

        /// <summary>
        /// Gets the formatted discount text
        /// </summary>
        public string DiscountText
        {
            get
            {
                if (DiscountType == "Percentage")
                    return $"{DiscountValue}% OFF";
                else
                    return $"${DiscountValue:F2} OFF";
            }
        }

        /// <summary>
        /// Gets the short description (first 100 characters)
        /// </summary>
        public string ShortDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                    return string.Empty;

                if (Description.Length <= 100)
                    return Description;

                return Description.Substring(0, 100) + "...";
            }
        }

        /// <summary>
        /// Whether the offer is valid (active and within date range)
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (!IsActive || !Status)
                    return false;

                if (StartDate.HasValue && StartDate.Value > DateTime.Now)
                    return false;

                if (EndDate.HasValue && EndDate.Value < DateTime.Now)
                    return false;

                return true;
            }
        }

        #endregion
    }
}   