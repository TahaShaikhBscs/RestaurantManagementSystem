// ============================================
// DAL/TableDAL.cs
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
    /// Table DAL - Data access layer for restaurant table operations
    /// Handles all database operations for restaurant tables
    /// </summary>
    public class TableDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public TableDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new table into the database
        /// </summary>
        public int InsertTable(RestaurantTable table)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", table.CompanyID),
                    dbHelper.CreateParameter("@BranchID", table.BranchID),
                    dbHelper.CreateParameter("@TableNumber", table.TableNumber),
                    dbHelper.CreateParameter("@Floor", table.Floor),
                    dbHelper.CreateParameter("@Capacity", table.Capacity),
                    dbHelper.CreateParameter("@Status", table.Status),
                    dbHelper.CreateParameter("@IsActive", table.IsActive),
                    dbHelper.CreateParameter("@QRCode", table.QRCode),
                    dbHelper.CreateParameter("@CreatedBy", table.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertRestaurantTable", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting table: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing table
        /// </summary>
        public bool UpdateTable(RestaurantTable table)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TableID", table.TableID),
                    dbHelper.CreateParameter("@TableNumber", table.TableNumber),
                    dbHelper.CreateParameter("@Floor", table.Floor),
                    dbHelper.CreateParameter("@Capacity", table.Capacity),
                    dbHelper.CreateParameter("@Status", table.Status),
                    dbHelper.CreateParameter("@IsActive", table.IsActive),
                    dbHelper.CreateParameter("@QRCode", table.QRCode),
                    dbHelper.CreateParameter("@UpdatedBy", table.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateRestaurantTable", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating table: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a table
        /// </summary>
        public bool DeleteTable(int tableID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TableID", tableID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteRestaurantTable", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting table: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a table by ID
        /// </summary>
        public RestaurantTable GetTableById(int tableID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TableID", tableID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetRestaurantTableById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToTable(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting table by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all tables for a branch
        /// </summary>
        public List<RestaurantTable> GetTablesByBranch(int branchID, bool includeInactive = false)
        {
            try
            {
                List<RestaurantTable> tables = new List<RestaurantTable>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetTablesByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    tables.Add(MapDataRowToTable(row));
                }

                return tables;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting tables by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets available tables for a branch
        /// </summary>
        public List<RestaurantTable> GetAvailableTables(int branchID, int? capacity = null)
        {
            try
            {
                List<RestaurantTable> tables = new List<RestaurantTable>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Capacity", capacity)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAvailableTables", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    tables.Add(MapDataRowToTable(row));
                }

                return tables;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting available tables: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates table status
        /// </summary>
        public bool UpdateTableStatus(int tableID, string status, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TableID", tableID),
                    dbHelper.CreateParameter("@Status", status),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateTableStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating table status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches tables
        /// </summary>
        public List<RestaurantTable> SearchTables(int branchID, string searchTerm)
        {
            try
            {
                List<RestaurantTable> tables = new List<RestaurantTable>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchTables", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    tables.Add(MapDataRowToTable(row));
                }

                return tables;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching tables: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to a RestaurantTable object
        /// </summary>
        private RestaurantTable MapDataRowToTable(DataRow row)
        {
            return new RestaurantTable
            {
                TableID = row.GetInt("TableID"),
                CompanyID = row.GetInt("CompanyID"),
                BranchID = row.GetInt("BranchID"),

                TableNumber = row.GetString("TableNumber"),
                Floor = row.GetString("Floor"),

                Capacity = row.GetInt("Capacity"),

                Status = row.GetString("Status"),
                IsActive = row.GetBool("IsActive"),

                QRCode = row.GetString("QRCode"),

                _Status = row.GetBool("_Status"),

                CreatedDate = row.GetDateTime("CreatedDate"),
                UpdatedDate = row.GetDateTime("UpdatedDate"),

                CreatedBy = row.GetInt("CreatedBy"),
                UpdatedBy = row.GetInt("UpdatedBy"),

                IsDeleted = row.GetBool("IsDeleted")
            };
        }
    }
}