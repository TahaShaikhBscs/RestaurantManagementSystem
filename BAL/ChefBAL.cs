// ============================================
// BAL/ChefBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Chef BAL - Business logic layer for chefs
    /// </summary>
    public class ChefBAL
    {
        #region Private Fields

        private readonly ChefDAL chefDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public ChefBAL()
        {
            chefDAL = new ChefDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all chefs with filters
        /// </summary>
        public List<Chef> GetChefs(int companyID, int? branchID = null, bool? isFeatured = null, int limit = 100)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            return chefDAL.GetChefs(companyID, branchID, isFeatured, limit);
        }

        /// <summary>
        /// Gets featured chefs
        /// </summary>
        public List<Chef> GetFeaturedChefs(int companyID, int? branchID = null, int limit = 10)
        {
            return GetChefs(companyID, branchID, true, limit);
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a chef
        /// </summary>
        public int SaveChef(Chef chef)
        {
            ValidateChef(chef);

            int result;

            if (chef.ChefID > 0)
            {
                bool updated = chefDAL.UpdateChef(chef);
                result = chef.ChefID;
                if (updated)
                {
                    auditLogger.LogUpdate("Chefs", "Chefs", chef.ChefID,
                        $"Chef: {chef.ChefName}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = chefDAL.InsertChef(chef);
                if (result > 0)
                {
                    auditLogger.LogInsert("Chefs", "Chefs", result,
                        $"Chef: {chef.ChefName}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a chef
        /// </summary>
        public bool DeleteChef(int chefID, int updatedBy)
        {
            if (chefID <= 0)
                throw new ArgumentException("Invalid chef ID.");

            bool deleted = chefDAL.DeleteChef(chefID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("Chefs", "Chefs", chefID,
                    $"ChefID: {chefID}");
            }

            return deleted;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates a chef
        /// </summary>
        private void ValidateChef(Chef chef)
        {
            if (chef == null)
                throw new ArgumentNullException(nameof(chef));

            if (chef.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(chef.ChefName))
                throw new ArgumentException("Chef name is required.");

            if (chef.ChefName.Length > 200)
                throw new ArgumentException("Chef name cannot exceed 200 characters.");

            if (chef.Designation != null && chef.Designation.Length > 100)
                throw new ArgumentException("Designation cannot exceed 100 characters.");

            if (chef.SocialFacebook != null && chef.SocialFacebook.Length > 200)
                throw new ArgumentException("Facebook URL cannot exceed 200 characters.");

            if (chef.SocialTwitter != null && chef.SocialTwitter.Length > 200)
                throw new ArgumentException("Twitter URL cannot exceed 200 characters.");

            if (chef.SocialInstagram != null && chef.SocialInstagram.Length > 200)
                throw new ArgumentException("Instagram URL cannot exceed 200 characters.");

            if (chef.SocialLinkedIn != null && chef.SocialLinkedIn.Length > 200)
                throw new ArgumentException("LinkedIn URL cannot exceed 200 characters.");
        }

        #endregion
    }
}   