// ============================================
// DAL/ChefDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Chef DAL - Data access layer for chefs
    /// </summary>
    public class ChefDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public ChefDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all chefs with filters
        /// </summary>
        public List<Chef> GetChefs(int companyID, int? branchID = null, bool? isFeatured = null, int limit = 100)
        {
            try
            {
                List<Chef> chefs = new List<Chef>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IsFeatured", isFeatured),
                    dbHelper.CreateParameter("@Limit", limit)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetChefs", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    chefs.Add(MapDataRowToChef(row));
                }

                return chefs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting chefs: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new chef
        /// </summary>
        public int InsertChef(Chef chef)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", chef.CompanyID),
                    dbHelper.CreateParameter("@BranchID", chef.BranchID),
                    dbHelper.CreateParameter("@ChefName", chef.ChefName),
                    dbHelper.CreateParameter("@Designation", chef.Designation),
                    dbHelper.CreateParameter("@Biography", chef.Biography),
                    dbHelper.CreateParameter("@ImagePath", chef.ImagePath),
                    dbHelper.CreateParameter("@SocialFacebook", chef.SocialFacebook),
                    dbHelper.CreateParameter("@SocialTwitter", chef.SocialTwitter),
                    dbHelper.CreateParameter("@SocialInstagram", chef.SocialInstagram),
                    dbHelper.CreateParameter("@SocialLinkedIn", chef.SocialLinkedIn),
                    dbHelper.CreateParameter("@DisplayOrder", chef.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", chef.IsFeatured),
                    dbHelper.CreateParameter("@CreatedBy", chef.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertChef", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting chef: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing chef
        /// </summary>
        public bool UpdateChef(Chef chef)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ChefID", chef.ChefID),
                    dbHelper.CreateParameter("@ChefName", chef.ChefName),
                    dbHelper.CreateParameter("@Designation", chef.Designation),
                    dbHelper.CreateParameter("@Biography", chef.Biography),
                    dbHelper.CreateParameter("@ImagePath", chef.ImagePath),
                    dbHelper.CreateParameter("@SocialFacebook", chef.SocialFacebook),
                    dbHelper.CreateParameter("@SocialTwitter", chef.SocialTwitter),
                    dbHelper.CreateParameter("@SocialInstagram", chef.SocialInstagram),
                    dbHelper.CreateParameter("@SocialLinkedIn", chef.SocialLinkedIn),
                    dbHelper.CreateParameter("@DisplayOrder", chef.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", chef.IsFeatured),
                    dbHelper.CreateParameter("@UpdatedBy", chef.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateChef", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating chef: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a chef
        /// </summary>
        public bool DeleteChef(int chefID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ChefID", chefID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteChef", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting chef: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Chef object
        /// </summary>
        private Chef MapDataRowToChef(DataRow row)
        {
            return new Chef
            {
                ChefID = Convert.ToInt32(row["ChefID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                ChefName = row["ChefName"].ToString(),
                Designation = row["Designation"]?.ToString(),
                Biography = row["Biography"]?.ToString(),
                ImagePath = row["ImagePath"]?.ToString(),
                SocialFacebook = row["SocialFacebook"]?.ToString(),
                SocialTwitter = row["SocialTwitter"]?.ToString(),
                SocialInstagram = row["SocialInstagram"]?.ToString(),
                SocialLinkedIn = row["SocialLinkedIn"]?.ToString(),
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        #endregion
    }
}