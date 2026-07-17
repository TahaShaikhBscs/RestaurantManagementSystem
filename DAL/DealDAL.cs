// ============================================
// DAL/DealDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Deal DAL - Data access layer for deal operations
    /// Handles all database operations for deals and deal items
    /// </summary>
    public class DealDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public DealDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Deal CRUD Operations

        /// <summary>
        /// Inserts a new deal into the database
        /// </summary>
        public int InsertDeal(Deal deal)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", deal.CompanyID),
                    dbHelper.CreateParameter("@BranchID", deal.BranchID),
                    dbHelper.CreateParameter("@DealName", deal.DealName),
                    dbHelper.CreateParameter("@Description", deal.Description),
                    dbHelper.CreateParameter("@DealPrice", deal.DealPrice),
                    dbHelper.CreateParameter("@Image", deal.Image),
                    dbHelper.CreateParameter("@StartDate", deal.StartDate),
                    dbHelper.CreateParameter("@EndDate", deal.EndDate),
                    dbHelper.CreateParameter("@Status", deal.Status),
                    dbHelper.CreateParameter("@CreatedBy", deal.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertDeal", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting deal: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing deal
        /// </summary>
        public bool UpdateDeal(Deal deal)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealID", deal.DealID),
                    dbHelper.CreateParameter("@DealName", deal.DealName),
                    dbHelper.CreateParameter("@Description", deal.Description),
                    dbHelper.CreateParameter("@DealPrice", deal.DealPrice),
                    dbHelper.CreateParameter("@Image", deal.Image),
                    dbHelper.CreateParameter("@StartDate", deal.StartDate),
                    dbHelper.CreateParameter("@EndDate", deal.EndDate),
                    dbHelper.CreateParameter("@Status", deal.Status),
                    dbHelper.CreateParameter("@UpdatedBy", deal.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateDeal", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating deal: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a deal (marks as deleted)
        /// </summary>
        public bool DeleteDeal(int dealID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealID", dealID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteDeal", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting deal: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a deal by ID
        /// </summary>
        public Deal GetDealById(int dealID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealID", dealID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetDealByID", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToDeal(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting deal by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all deals for a branch
        /// </summary>
        public List<Deal> GetAllDeals(int branchID, bool includeInactive = false)
        {
            try
            {
                List<Deal> deals = new List<Deal>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAllDeals", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    deals.Add(MapDataRowToDeal(row));
                }

                return deals;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all deals: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches deals by name or description
        /// </summary>
        public List<Deal> SearchDeals(int branchID, string searchTerm)
        {
            try
            {
                List<Deal> deals = new List<Deal>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchDeals", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    deals.Add(MapDataRowToDeal(row));
                }

                return deals;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching deals: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a deal name already exists
        /// </summary>
        public bool DealNameExists(int branchID, string dealName, int? excludeDealID = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DealName", dealName),
                    dbHelper.CreateParameter("@ExcludeDealID", excludeDealID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetDealByName", parameters);
                return dataTable.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking deal name: {ex.Message}", ex);
            }
        }

        #endregion

        #region Deal Item CRUD Operations

        /// <summary>
        /// Inserts a deal item
        /// </summary>
        public int InsertDealItem(DealItem item)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealID", item.DealID),
                    dbHelper.CreateParameter("@MenuItemID", item.MenuItemID),
                    dbHelper.CreateParameter("@Quantity", item.Quantity),
                    dbHelper.CreateParameter("@CreatedBy", item.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertDealItem", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting deal item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates a deal item
        /// </summary>
        public bool UpdateDealItem(DealItem item)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealItemID", item.DealItemID),
                    dbHelper.CreateParameter("@Quantity", item.Quantity),
                    dbHelper.CreateParameter("@UpdatedBy", item.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateDealItem", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating deal item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a deal item
        /// </summary>
        public bool DeleteDealItem(int dealItemID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealItemID", dealItemID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteDealItem", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting deal item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all items for a deal
        /// </summary>
        public List<DealItem> GetDealItems(int dealID)
        {
            try
            {
                List<DealItem> items = new List<DealItem>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@DealID", dealID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetDealItems", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    items.Add(MapDataRowToDealItem(row));
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting deal items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a complete deal with all items
        /// </summary>
        public DataSet GetDealWithItems(int dealID)
        {
            try
            {
                using (SqlConnection connection = dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand("sp_GetDealWithItems", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(dbHelper.CreateParameter("@DealID", dealID));

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting deal with items: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Deal object
        /// </summary>
        private Deal MapDataRowToDeal(DataRow row)
        {
            return new Deal
            {
                DealID = Convert.ToInt32(row["DealID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                DealName = row["DealName"].ToString(),
                Description = row["Description"]?.ToString(),
                DealPrice = Convert.ToDecimal(row["DealPrice"]),
                Image = row["Image"]?.ToString(),
                StartDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : (DateTime?)null,
                EndDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : (DateTime?)null,
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                ItemCount = row.Table.Columns.Contains("ItemCount") ? Convert.ToInt32(row["ItemCount"]) : 0,
                OriginalPrice = row.Table.Columns.Contains("OriginalPrice") ? Convert.ToDecimal(row["OriginalPrice"]) : 0
            };
        }

        /// <summary>
        /// Maps a DataRow to a DealItem object
        /// </summary>
        private DealItem MapDataRowToDealItem(DataRow row)
        {
            return new DealItem
            {
                DealItemID = Convert.ToInt32(row["DealItemID"]),
                DealID = Convert.ToInt32(row["DealID"]),
                MenuItemID = Convert.ToInt32(row["MenuItemID"]),
                Quantity = Convert.ToInt32(row["Quantity"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                ItemName = row["ItemName"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                Description = row["Description"]?.ToString(),
                Image = row["Image"]?.ToString(),
                CategoryID = Convert.ToInt32(row["CategoryID"]),
                CategoryName = row["CategoryName"].ToString()
            };
        }

        #endregion
    }
}