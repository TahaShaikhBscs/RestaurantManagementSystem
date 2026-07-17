// ============================================
// Models/Category.cs - Complete Updated
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Category Model - Represents a product category in the restaurant menu
    /// </summary>
    public class Category
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the category
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Company ID this category belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this category belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Name of the category (e.g., Appetizers, Main Course)
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Description of the category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Parent category ID for sub-categories (null for root categories)
        /// </summary>
        public int? ParentCategoryID { get; set; }

        /// <summary>
        /// Display order for sorting (lower numbers appear first)
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Icon name or path for the category
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Image path for the category
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Status of the category (Active/Inactive)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the category was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the category was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the category
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the category
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Name of the parent category (for display purposes)
        /// </summary>
        public string ParentCategoryName { get; set; }

        /// <summary>
        /// Indicates if this is a root category (no parent)
        /// </summary>
        public bool IsRootCategory
        {
            get { return !ParentCategoryID.HasValue; }
        }

        /// <summary>
        /// Status text for display
        /// </summary>
        public string StatusText
        {
            get { return Status ? "Active" : "Inactive"; }
        }

        #endregion
    }
}