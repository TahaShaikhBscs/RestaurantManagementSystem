// ============================================
// Models/Company.cs - Updated
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Company Model - Represents a company in the system
    /// </summary>
    public class Company
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the company
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Address of the company
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Phone number of the company
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email address of the company
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// National Tax Number
        /// </summary>
        public string NTN { get; set; }

        /// <summary>
        /// Goods and Services Tax number
        /// </summary>
        public string GST { get; set; }

        /// <summary>
        /// Logo path
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Currency code (e.g., USD, PKR)
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Currency symbol (e.g., $, ₨)
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Time zone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Status of the company (Active/Inactive)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the company was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the company was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the company
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the company
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Status text for display
        /// </summary>
        public string StatusText
        {
            get { return Status ? "Active" : "Inactive"; }
        }

        /// <summary>
        /// Gets the status badge class
        /// </summary>
        public string StatusBadgeClass
        {
            get { return Status ? "success" : "danger"; }
        }

        #endregion
    }
}