// ============================================
// Models/Role.cs - Updated
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Role Model - Represents a user role
    /// </summary>
    public class Role
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the role
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Name of the role
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Description of the role
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Status of the role (Active/Inactive)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the role was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the role was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the role
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the role
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