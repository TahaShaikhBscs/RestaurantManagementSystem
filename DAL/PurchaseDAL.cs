// ============================================
// DAL/PurchaseDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Purchase DAL - Data access layer for purchase order operations
    /// Handles all database operations for purchase orders and items
    /// </summary>
    public class PurchaseDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public PurchaseDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new purchase order into the database
        /// </summary>
        public int InsertPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", purchaseOrder.CompanyID),
                    dbHelper.CreateParameter("@BranchID", purchaseOrder.BranchID),
                    dbHelper.CreateParameter("@SupplierID", purchaseOrder.SupplierID),
                    dbHelper.CreateParameter("@PurchaseOrderNumber", purchaseOrder.PurchaseOrderNumber),
                    dbHelper.CreateParameter("@OrderDate", purchaseOrder.OrderDate),
                    dbHelper.CreateParameter("@ExpectedDeliveryDate", purchaseOrder.ExpectedDeliveryDate),
                    dbHelper.CreateParameter("@ActualDeliveryDate", purchaseOrder.ActualDeliveryDate),
                    dbHelper.CreateParameter("@SubTotal", purchaseOrder.SubTotal),
                    dbHelper.CreateParameter("@Tax", purchaseOrder.Tax),
                    dbHelper.CreateParameter("@Discount", purchaseOrder.Discount),
                    dbHelper.CreateParameter("@TotalAmount", purchaseOrder.TotalAmount),
                    dbHelper.CreateParameter("@PaidAmount", purchaseOrder.PaidAmount),
                    dbHelper.CreateParameter("@PaymentStatus", purchaseOrder.PaymentStatus),
                    dbHelper.CreateParameter("@OrderStatus", purchaseOrder.OrderStatus),
                    dbHelper.CreateParameter("@Notes", purchaseOrder.Notes),
                    dbHelper.CreateParameter("@CreatedBy", purchaseOrder.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertPurchaseOrder", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting purchase order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Inserts a purchase order item
        /// </summary>
        public int InsertPurchaseOrderItem(PurchaseOrderItem item)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@PurchaseOrderID", item.PurchaseOrderID),
                    dbHelper.CreateParameter("@StockID", item.StockID),
                    dbHelper.CreateParameter("@Quantity", item.Quantity),
                    dbHelper.CreateParameter("@UnitPrice", item.UnitPrice),
                    dbHelper.CreateParameter("@TotalPrice", item.TotalPrice),
                    dbHelper.CreateParameter("@ReceivedQuantity", item.ReceivedQuantity),
                    dbHelper.CreateParameter("@Notes", item.Notes),
                    dbHelper.CreateParameter("@CreatedBy", item.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertPurchaseOrderItem", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting purchase order item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates a purchase order
        /// </summary>
        public bool UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@PurchaseOrderID", purchaseOrder.PurchaseOrderID),
                    dbHelper.CreateParameter("@SupplierID", purchaseOrder.SupplierID),
                    dbHelper.CreateParameter("@ExpectedDeliveryDate", purchaseOrder.ExpectedDeliveryDate),
                    dbHelper.CreateParameter("@ActualDeliveryDate", purchaseOrder.ActualDeliveryDate),
                    dbHelper.CreateParameter("@SubTotal", purchaseOrder.SubTotal),
                    dbHelper.CreateParameter("@Tax", purchaseOrder.Tax),
                    dbHelper.CreateParameter("@Discount", purchaseOrder.Discount),
                    dbHelper.CreateParameter("@TotalAmount", purchaseOrder.TotalAmount),
                    dbHelper.CreateParameter("@PaidAmount", purchaseOrder.PaidAmount),
                    dbHelper.CreateParameter("@PaymentStatus", purchaseOrder.PaymentStatus),
                    dbHelper.CreateParameter("@OrderStatus", purchaseOrder.OrderStatus),
                    dbHelper.CreateParameter("@Notes", purchaseOrder.Notes),
                    dbHelper.CreateParameter("@UpdatedBy", purchaseOrder.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdatePurchaseOrder", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating purchase order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a purchase order by ID with all details
        /// </summary>
        public DataSet GetPurchaseOrderById(int purchaseOrderID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@PurchaseOrderID", purchaseOrderID)
                };

                using (SqlConnection connection = dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand("sp_GetPurchaseOrderById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters.ToArray());

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
                throw new Exception($"Error getting purchase order by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all purchase orders for a branch
        /// </summary>
        public List<PurchaseOrder> GetPurchaseOrdersByBranch(int branchID, string status = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                List<PurchaseOrder> orders = new List<PurchaseOrder>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Status", status),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetPurchaseOrdersByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    orders.Add(MapDataRowToPurchaseOrder(row));
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting purchase orders by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates purchase order status
        /// </summary>
        public bool UpdatePurchaseOrderStatus(int purchaseOrderID, string status, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@PurchaseOrderID", purchaseOrderID),
                    dbHelper.CreateParameter("@OrderStatus", status),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdatePurchaseOrderStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating purchase order status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to a PurchaseOrder object
        /// </summary>
        private PurchaseOrder MapDataRowToPurchaseOrder(DataRow row)
        {
            return new PurchaseOrder
            {
                PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                SupplierID = Convert.ToInt32(row["SupplierID"]),
                PurchaseOrderNumber = row["PurchaseOrderNumber"].ToString(),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                ExpectedDeliveryDate = row["ExpectedDeliveryDate"] != DBNull.Value ? Convert.ToDateTime(row["ExpectedDeliveryDate"]) : (DateTime?)null,
                ActualDeliveryDate = row["ActualDeliveryDate"] != DBNull.Value ? Convert.ToDateTime(row["ActualDeliveryDate"]) : (DateTime?)null,
                SubTotal = Convert.ToDecimal(row["SubTotal"]),
                Tax = Convert.ToDecimal(row["Tax"]),
                Discount = Convert.ToDecimal(row["Discount"]),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                PaidAmount = Convert.ToDecimal(row["PaidAmount"]),
                PaymentStatus = row["PaymentStatus"].ToString(),
                OrderStatus = row["OrderStatus"].ToString(),
                Notes = row["Notes"]?.ToString(),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                SupplierName = row.Table.Columns.Contains("SupplierName") ? row["SupplierName"]?.ToString() : null
            };
        }
    }
}