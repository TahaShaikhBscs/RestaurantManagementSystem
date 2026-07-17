// ============================================
// DAL/StockDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Stock DAL - Data access layer for stock and inventory operations
    /// Handles all database operations for stock items and transactions
    /// </summary>
    public class StockDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public StockDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new stock item into the database
        /// </summary>
        public int InsertStock(Stock stock)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", stock.CompanyID),
                    dbHelper.CreateParameter("@BranchID", stock.BranchID),
                    dbHelper.CreateParameter("@ProductName", stock.ProductName),
                    dbHelper.CreateParameter("@Category", stock.Category),
                    dbHelper.CreateParameter("@Quantity", stock.Quantity),
                    dbHelper.CreateParameter("@UnitPrice", stock.UnitPrice),
                    dbHelper.CreateParameter("@TotalCost", stock.TotalCost),
                    dbHelper.CreateParameter("@MinStockLevel", stock.MinStockLevel),
                    dbHelper.CreateParameter("@MaxStockLevel", stock.MaxStockLevel),
                    dbHelper.CreateParameter("@Location", stock.Location),
                    dbHelper.CreateParameter("@Barcode", stock.Barcode),
                    dbHelper.CreateParameter("@Status", stock.Status),
                    dbHelper.CreateParameter("@CreatedBy", stock.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertStock", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting stock: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing stock item
        /// </summary>
        public bool UpdateStock(Stock stock)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@StockID", stock.StockID),
                    dbHelper.CreateParameter("@ProductName", stock.ProductName),
                    dbHelper.CreateParameter("@Category", stock.Category),
                    dbHelper.CreateParameter("@Quantity", stock.Quantity),
                    dbHelper.CreateParameter("@UnitPrice", stock.UnitPrice),
                    dbHelper.CreateParameter("@TotalCost", stock.TotalCost),
                    dbHelper.CreateParameter("@MinStockLevel", stock.MinStockLevel),
                    dbHelper.CreateParameter("@MaxStockLevel", stock.MaxStockLevel),
                    dbHelper.CreateParameter("@Location", stock.Location),
                    dbHelper.CreateParameter("@Barcode", stock.Barcode),
                    dbHelper.CreateParameter("@Status", stock.Status),
                    dbHelper.CreateParameter("@UpdatedBy", stock.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateStock", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating stock: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a stock item
        /// </summary>
        public bool DeleteStock(int stockID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@StockID", stockID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteStock", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting stock: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a stock item by ID
        /// </summary>
        public Stock GetStockById(int stockID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@StockID", stockID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetStockById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToStock(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting stock by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all stock items for a branch
        /// </summary>
        public List<Stock> GetStockByBranch(int branchID, string category = null, bool includeInactive = false)
        {
            try
            {
                List<Stock> stocks = new List<Stock>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Category", category),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetStockByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    stocks.Add(MapDataRowToStock(row));
                }

                return stocks;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting stock by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches stock items by name or barcode
        /// </summary>
        public List<Stock> SearchStock(int branchID, string searchTerm)
        {
            try
            {
                List<Stock> stocks = new List<Stock>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchStock", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    stocks.Add(MapDataRowToStock(row));
                }

                return stocks;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching stock: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates stock quantity
        /// </summary>
        public bool UpdateStockQuantity(int stockID, decimal quantity, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@StockID", stockID),
                    dbHelper.CreateParameter("@Quantity", quantity),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateStockQuantity", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating stock quantity: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets low stock items
        /// </summary>
        public List<Stock> GetLowStockItems(int branchID)
        {
            try
            {
                List<Stock> stocks = new List<Stock>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetLowStockItems", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    stocks.Add(MapDataRowToStock(row));
                }

                return stocks;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting low stock items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Inserts a stock transaction
        /// </summary>
        public int InsertStockTransaction(StockTransaction transaction)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", transaction.CompanyID),
                    dbHelper.CreateParameter("@BranchID", transaction.BranchID),
                    dbHelper.CreateParameter("@StockID", transaction.StockID),
                    dbHelper.CreateParameter("@TransactionType", transaction.TransactionType),
                    dbHelper.CreateParameter("@ReferenceType", transaction.ReferenceType),
                    dbHelper.CreateParameter("@ReferenceID", transaction.ReferenceID),
                    dbHelper.CreateParameter("@Quantity", transaction.Quantity),
                    dbHelper.CreateParameter("@PreviousQuantity", transaction.PreviousQuantity),
                    dbHelper.CreateParameter("@NewQuantity", transaction.NewQuantity),
                    dbHelper.CreateParameter("@UnitPrice", transaction.UnitPrice),
                    dbHelper.CreateParameter("@TotalAmount", transaction.TotalAmount),
                    dbHelper.CreateParameter("@Notes", transaction.Notes),
                    dbHelper.CreateParameter("@TransactionDate", transaction.TransactionDate),
                    dbHelper.CreateParameter("@CreatedBy", transaction.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertStockTransaction", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting stock transaction: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets stock transactions for a stock item
        /// </summary>
        public List<StockTransaction> GetStockTransactions(int stockID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                List<StockTransaction> transactions = new List<StockTransaction>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@StockID", stockID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetStockTransactions", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    transactions.Add(MapDataRowToStockTransaction(row));
                }

                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting stock transactions: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to a Stock object
        /// </summary>
        private Stock MapDataRowToStock(DataRow row)
        {
            return new Stock
            {
                StockID = Convert.ToInt32(row["StockID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                ProductName = row["ProductName"].ToString(),
                Category = row["Category"]?.ToString(),
                Quantity = Convert.ToDecimal(row["Quantity"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                TotalCost = Convert.ToDecimal(row["TotalCost"]),
                MinStockLevel = Convert.ToInt32(row["MinStockLevel"]),
                MaxStockLevel = Convert.ToInt32(row["MaxStockLevel"]),
                Location = row["Location"]?.ToString(),
                Barcode = row["Barcode"]?.ToString(),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
        }

        /// <summary>
        /// Updates stock from order (automatic inventory deduction)
        /// </summary>
        /// <param name="orderID">Order ID</param>
        /// <param name="updatedBy">User ID performing the update</param>
        /// <returns>True if successful</returns>
        public bool UpdateStockFromOrder(int orderID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@OrderID", orderID),
            dbHelper.CreateParameter("@UpdatedBy", updatedBy)
        };

                object result = dbHelper.ExecuteScalar("sp_UpdateStockFromOrder", parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating stock from order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets inventory report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="category">Category filter</param>
        /// <returns>DataTable with inventory data</returns>
        public DataTable GetInventoryReport(int branchID, string category = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@Category", category)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetInventoryReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting inventory report: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Maps a DataRow to a StockTransaction object
        /// </summary>
        private StockTransaction MapDataRowToStockTransaction(DataRow row)
        {
            return new StockTransaction
            {
                StockTransactionID = Convert.ToInt32(row["StockTransactionID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                StockID = Convert.ToInt32(row["StockID"]),
                TransactionType = row["TransactionType"].ToString(),
                ReferenceType = row["ReferenceType"]?.ToString(),
                ReferenceID = row["ReferenceID"] != DBNull.Value ? Convert.ToInt32(row["ReferenceID"]) : (int?)null,
                Quantity = Convert.ToDecimal(row["Quantity"]),
                PreviousQuantity = row["PreviousQuantity"] != DBNull.Value ? Convert.ToDecimal(row["PreviousQuantity"]) : (decimal?)null,
                NewQuantity = row["NewQuantity"] != DBNull.Value ? Convert.ToDecimal(row["NewQuantity"]) : (decimal?)null,
                UnitPrice = row["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(row["UnitPrice"]) : (decimal?)null,
                TotalAmount = row["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(row["TotalAmount"]) : (decimal?)null,
                Notes = row["Notes"]?.ToString(),
                TransactionDate = Convert.ToDateTime(row["TransactionDate"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
        }
    }
}