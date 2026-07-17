// ============================================
// Models/FAQ.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// FAQ Model - Represents a frequently asked question on the website
    /// </summary>
    public class FAQ
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the FAQ
        /// </summary>
        public int FAQID { get; set; }

        /// <summary>
        /// Company ID this FAQ belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this FAQ belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Question text
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Answer text
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Category of the FAQ (e.g., General, Ordering, Delivery)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether this FAQ is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Status of the FAQ
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the FAQ was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the FAQ was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the FAQ
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the FAQ
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion
    }
}