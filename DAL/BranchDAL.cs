// ============================================
// DAL/BranchDAL.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Branch DAL - Data access layer for branch operations
    /// </summary>
    public class BranchDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public BranchDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Inserts a new branch into the database
        /// </summary>
        public int InsertBranch(Branch branch)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", branch.CompanyID),
                    dbHelper.CreateParameter("@BranchName", branch.BranchName),
                    dbHelper.CreateParameter("@Address", branch.Address),
                    dbHelper.CreateParameter("@Phone", branch.Phone),
                    dbHelper.CreateParameter("@Email", branch.Email),
                    dbHelper.CreateParameter("@Kitchen", branch.Kitchen),
                    dbHelper.CreateParameter("@HasTables", branch.HasTables),
                    dbHelper.CreateParameter("@HasCashCounter", branch.HasCashCounter),
                    dbHelper.CreateParameter("@HasWarehouse", branch.HasWarehouse),
                    dbHelper.CreateParameter("@Status", branch.Status),
                    dbHelper.CreateParameter("@CreatedBy", branch.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertBranch", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing branch
        /// </summary>
        public bool UpdateBranch(Branch branch)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branch.BranchID),
                    dbHelper.CreateParameter("@BranchName", branch.BranchName),
                    dbHelper.CreateParameter("@Address", branch.Address),
                    dbHelper.CreateParameter("@Phone", branch.Phone),
                    dbHelper.CreateParameter("@Email", branch.Email),
                    dbHelper.CreateParameter("@Kitchen", branch.Kitchen),
                    dbHelper.CreateParameter("@HasTables", branch.HasTables),
                    dbHelper.CreateParameter("@HasCashCounter", branch.HasCashCounter),
                    dbHelper.CreateParameter("@HasWarehouse", branch.HasWarehouse),
                    dbHelper.CreateParameter("@Status", branch.Status),
                    dbHelper.CreateParameter("@UpdatedBy", branch.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateBranch", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a branch
        /// </summary>
        public bool DeleteBranch(int branchID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteBranch", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting branch: {ex.Message}", ex);
            }
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a branch by ID
        /// </summary>
        public Branch GetBranchById(int branchID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetBranchById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToBranch(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting branch by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all branches for a company
        /// </summary>
        public List<Branch> GetBranchesByCompany(int companyID)
        {
            try
            {
                List<Branch> branches = new List<Branch>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetBranchesByCompany", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    branches.Add(MapDataRowToBranch(row));
                }

                return branches;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting branches by company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all branches
        /// </summary>
        public List<Branch> GetAllBranches()
        {
            try
            {
                List<Branch> branches = new List<Branch>();
                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAllBranches");

                foreach (DataRow row in dataTable.Rows)
                {
                    branches.Add(MapDataRowToBranch(row));
                }

                return branches;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all branches: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches branches
        /// </summary>
        public List<Branch> SearchBranches(string searchTerm, int? companyID = null)
        {
            try
            {
                List<Branch> branches = new List<Branch>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SearchTerm", searchTerm),
                    dbHelper.CreateParameter("@CompanyID", companyID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchBranches", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    branches.Add(MapDataRowToBranch(row));
                }

                return branches;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching branches: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Branch object
        /// </summary>
        private Branch MapDataRowToBranch(DataRow row)
        {
            return new Branch
            {
                BranchID = row.GetInt("BranchID"),
                CompanyID = row.GetInt("CompanyID"),

                BranchName = row.GetString("BranchName"),
                Address = row.GetString("Address"),
                Phone = row.GetString("Phone"),
                Email = row.GetString("Email"),

                Kitchen = row.GetBool("Kitchen"),
                HasTables = row.GetBool("HasTables"),
                HasCashCounter = row.GetBool("HasCashCounter"),
                HasWarehouse = row.GetBool("HasWarehouse"),

                Status = row.GetBool("Status"),

                CreatedDate = row.GetDateTime("CreatedDate"),
                UpdatedDate = row.GetDateTime("UpdatedDate"),

                CreatedBy = row.GetInt("CreatedBy"),
                UpdatedBy = row.GetInt("UpdatedBy"),

                IsDeleted = row.GetBool("IsDeleted"),

                CompanyName = row.GetString("CompanyName")
            };
        }
        #endregion
    }
}