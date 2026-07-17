// ============================================
// DAL/CategoryDAL.cs - Complete Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Category DAL - Data access layer for category operations
    /// Handles all database operations for categories
    /// </summary>
    public class CategoryDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public CategoryDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Inserts a new category into the database
        /// </summary>
        public int InsertCategory(Category category)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", category.CompanyID),
                    dbHelper.CreateParameter("@BranchID", category.BranchID),
                    dbHelper.CreateParameter("@CategoryName", category.CategoryName),
                    dbHelper.CreateParameter("@Description", category.Description),
                    dbHelper.CreateParameter("@ParentCategoryID", category.ParentCategoryID),
                    dbHelper.CreateParameter("@DisplayOrder", category.DisplayOrder),
                    dbHelper.CreateParameter("@Icon", category.Icon),
                    dbHelper.CreateParameter("@Image", category.Image),
                    dbHelper.CreateParameter("@Status", category.Status),
                    dbHelper.CreateParameter("@CreatedBy", category.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertCategory", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting category: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing category
        /// </summary>
        public bool UpdateCategory(Category category)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CategoryID", category.CategoryID),
                    dbHelper.CreateParameter("@CategoryName", category.CategoryName),
                    dbHelper.CreateParameter("@Description", category.Description),
                    dbHelper.CreateParameter("@ParentCategoryID", category.ParentCategoryID),
                    dbHelper.CreateParameter("@DisplayOrder", category.DisplayOrder),
                    dbHelper.CreateParameter("@Icon", category.Icon),
                    dbHelper.CreateParameter("@Image", category.Image),
                    dbHelper.CreateParameter("@Status", category.Status),
                    dbHelper.CreateParameter("@UpdatedBy", category.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateCategory", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating category: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a category (marks as deleted)
        /// </summary>
        public bool DeleteCategory(int categoryID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CategoryID", categoryID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteCategory", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting category: {ex.Message}", ex);
            }
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a category by ID
        /// </summary>
        public Category GetCategoryById(int categoryID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CategoryID", categoryID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCategoryById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToCategory(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting category by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all categories for a branch
        /// </summary>
        public List<Category> GetCategoriesByBranch(int branchID, bool includeInactive = false)
        {
            try
            {
                List<Category> categories = new List<Category>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCategoriesByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    categories.Add(MapDataRowToCategory(row));
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting categories by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all categories for a company
        /// </summary>
        public List<Category> GetCategoriesByCompany(int companyID, bool includeInactive = false)
        {
            try
            {
                List<Category> categories = new List<Category>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCategoriesByCompany", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    categories.Add(MapDataRowToCategory(row));
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting categories by company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets root categories (categories with no parent)
        /// </summary>
        public List<Category> GetRootCategories(int branchID, bool includeInactive = false)
        {
            try
            {
                List<Category> categories = new List<Category>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetRootCategories", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    categories.Add(MapDataRowToCategory(row));
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting root categories: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches categories by name
        /// </summary>
        public List<Category> SearchCategories(int branchID, string searchTerm)
        {
            try
            {
                List<Category> categories = new List<Category>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchCategories", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    categories.Add(MapDataRowToCategory(row));
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching categories: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a category name already exists in a branch
        /// </summary>
        public bool CategoryNameExists(int branchID, string categoryName, int? excludeCategoryID = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@CategoryName", categoryName),
                    dbHelper.CreateParameter("@ExcludeCategoryID", excludeCategoryID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCategoryByName", parameters);
                return dataTable.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking category name: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks for circular parent reference
        /// </summary>
        public bool IsCircularParentReference(int categoryID, int parentCategoryID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CategoryID", categoryID),
                    dbHelper.CreateParameter("@ParentCategoryID", parentCategoryID)
                };

                object result = dbHelper.ExecuteScalar("sp_CheckCircularParent", parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking circular parent: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Category object
        /// </summary>
        private Category MapDataRowToCategory(DataRow row)
        {
            return new Category
            {
                CategoryID = row["CategoryID"] != DBNull.Value ? Convert.ToInt32(row["CategoryID"]) : 0,
                CompanyID = row["CompanyID"] != DBNull.Value ? Convert.ToInt32(row["CompanyID"]) : 0,
                BranchID = row["BranchID"] != DBNull.Value ? Convert.ToInt32(row["BranchID"]) : 0,

                CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : string.Empty,
                Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : string.Empty,

                ParentCategoryID = row["ParentCategoryID"] != DBNull.Value
                    ? Convert.ToInt32(row["ParentCategoryID"])
                    : (int?)null,

                DisplayOrder = row["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(row["DisplayOrder"]) : 0,

                Icon = row["Icon"] != DBNull.Value ? row["Icon"].ToString() : string.Empty,
                Image = row["Image"] != DBNull.Value ? row["Image"].ToString() : string.Empty,

                Status = row["Status"] != DBNull.Value && Convert.ToBoolean(row["Status"]),

                CreatedDate = row["CreatedDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["CreatedDate"])
                    : DateTime.MinValue,

                UpdatedDate = row["UpdatedDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["UpdatedDate"])
                    : DateTime.MinValue,

                CreatedBy = row["CreatedBy"] != DBNull.Value ? Convert.ToInt32(row["CreatedBy"]) : 0,
                UpdatedBy = row["UpdatedBy"] != DBNull.Value ? Convert.ToInt32(row["UpdatedBy"]) : 0,

                IsDeleted = row["IsDeleted"] != DBNull.Value && Convert.ToBoolean(row["IsDeleted"]),

                ParentCategoryName = row.Table.Columns.Contains("ParentCategoryName") &&
                                     row["ParentCategoryName"] != DBNull.Value
                                        ? row["ParentCategoryName"].ToString()
                                        : string.Empty
            };
        }

        #endregion
    }
}