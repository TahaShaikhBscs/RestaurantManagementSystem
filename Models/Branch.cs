// ============================================
// Models/Branch.cs - Updated
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Branch Model - Represents a branch of a company
    /// </summary>
    public class Branch
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the branch
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Company ID this branch belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Name of the branch
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Address of the branch
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Phone number of the branch
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email address of the branch
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Whether the branch has a kitchen
        /// </summary>
        public bool Kitchen { get; set; }

        /// <summary>
        /// Whether the branch has tables
        /// </summary>
        public bool HasTables { get; set; }

        /// <summary>
        /// Whether the branch has a cash counter
        /// </summary>
        public bool HasCashCounter { get; set; }

        /// <summary>
        /// Whether the branch has a warehouse
        /// </summary>
        public bool HasWarehouse { get; set; }

        /// <summary>
        /// Status of the branch (Active/Inactive)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the branch was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the branch was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the branch
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the branch
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Name of the company (from join)
        /// </summary>
        public string CompanyName { get; set; }

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

        /// <summary>
        /// Gets the features summary
        /// </summary>
        public string FeaturesSummary
        {
            get
            {
                string features = "";
                if (Kitchen) features += "Kitchen, ";
                if (HasTables) features += "Tables, ";
                if (HasCashCounter) features += "Cash, ";
                if (HasWarehouse) features += "Warehouse";
                return features.TrimEnd(' ', ',');
            }
        }

        #endregion
    }
}