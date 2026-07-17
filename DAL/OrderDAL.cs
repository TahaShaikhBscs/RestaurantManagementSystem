// ============================================
// DAL/OrderDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Order DAL - Data access layer for order operations
    /// Handles all database operations for orders and order items
    /// </summary>
    public class OrderDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public OrderDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new order into the database
        /// </summary>
        public int InsertOrder(Order order)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", order.CompanyID),
                    dbHelper.CreateParameter("@BranchID", order.BranchID),
                    dbHelper.CreateParameter("@TableID", order.TableID),
                    dbHelper.CreateParameter("@CustomerID", order.CustomerID),
                    dbHelper.CreateParameter("@OrderNumber", order.OrderNumber),
                    dbHelper.CreateParameter("@OrderType", order.OrderType),
                    dbHelper.CreateParameter("@OrderStatus", order.OrderStatus),
                    dbHelper.CreateParameter("@PaymentStatus", order.PaymentStatus),
                    dbHelper.CreateParameter("@SubTotal", order.SubTotal),
                    dbHelper.CreateParameter("@Tax", order.Tax),
                    dbHelper.CreateParameter("@Discount", order.Discount),
                    dbHelper.CreateParameter("@ServiceCharge", order.ServiceCharge),
                    dbHelper.CreateParameter("@TotalAmount", order.TotalAmount),
                    dbHelper.CreateParameter("@PaidAmount", order.PaidAmount),
                    dbHelper.CreateParameter("@ChangeAmount", order.ChangeAmount),
                    dbHelper.CreateParameter("@DeliveryDate", order.DeliveryDate),
                    dbHelper.CreateParameter("@DeliveryAddress", order.DeliveryAddress),
                    dbHelper.CreateParameter("@SpecialInstructions", order.SpecialInstructions),
                    dbHelper.CreateParameter("@OrderSource", order.OrderSource),
                    dbHelper.CreateParameter("@CreatedBy", order.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertOrder", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Inserts an order item
        /// </summary>
        public int InsertOrderItem(OrderItem orderItem)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderItem.OrderID),
                    dbHelper.CreateParameter("@MenuItemID", orderItem.MenuItemID),
                    dbHelper.CreateParameter("@Quantity", orderItem.Quantity),
                    dbHelper.CreateParameter("@UnitPrice", orderItem.UnitPrice),
                    dbHelper.CreateParameter("@Discount", orderItem.Discount),
                    dbHelper.CreateParameter("@TotalPrice", orderItem.TotalPrice),
                    dbHelper.CreateParameter("@Instructions", orderItem.Instructions),
                    dbHelper.CreateParameter("@KitchenStatus", orderItem.KitchenStatus),
                    dbHelper.CreateParameter("@PreparationTime", orderItem.PreparationTime),
                    dbHelper.CreateParameter("@CreatedBy", orderItem.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertOrderItem", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting order item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets complete order details by ID
        /// </summary>
        public DataSet GetOrderById(int orderID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID)
                };

                // This stored procedure returns multiple result sets
                using (SqlConnection connection = dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand("sp_GetOrderById", connection))
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
                throw new Exception($"Error getting order by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates order status
        /// </summary>
        public bool UpdateOrderStatus(int orderID, string status, string notes, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@OrderStatus", status),
                    dbHelper.CreateParameter("@Notes", notes),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateOrderStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating order status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates kitchen status for an order item
        /// </summary>
        public bool UpdateKitchenStatus(int orderItemID, string status, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderItemID", orderItemID),
                    dbHelper.CreateParameter("@KitchenStatus", status),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateKitchenStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating kitchen status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets orders by branch with filters
        /// </summary>
        public List<Order> GetOrdersByBranch(int branchID, string orderStatus = null, DateTime? dateFrom = null, DateTime? dateTo = null, string orderType = null)
        {
            try
            {
                List<Order> orders = new List<Order>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@OrderStatus", orderStatus),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@OrderType", orderType)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetOrdersByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    orders.Add(MapDataRowToOrder(row));
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting orders by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches orders
        /// </summary>
        public List<Order> SearchOrders(int branchID, string searchTerm)
        {
            try
            {
                List<Order> orders = new List<Order>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchOrders", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    orders.Add(MapDataRowToOrder(row));
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching orders: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        public bool CancelOrder(int orderID, string reason, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@Reason", reason),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_CancelOrder", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error cancelling order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets kitchen orders
        /// </summary>
        public List<KitchenOrder> GetKitchenOrders(int branchID, string status = null)
        {
            try
            {
                List<KitchenOrder> kitchenOrders = new List<KitchenOrder>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Status", status)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetKitchenOrders", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    kitchenOrders.Add(MapDataRowToKitchenOrder(row));
                }

                return kitchenOrders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting kitchen orders: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to an Order object
        /// </summary>
        private Order MapDataRowToOrder(DataRow row)
        {
            return new Order
            {
                OrderID = Convert.ToInt32(row["OrderID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                TableID = row["TableID"] != DBNull.Value ? Convert.ToInt32(row["TableID"]) : (int?)null,
                CustomerID = row["CustomerID"] != DBNull.Value ? Convert.ToInt32(row["CustomerID"]) : (int?)null,
                OrderNumber = row["OrderNumber"].ToString(),
                OrderType = row["OrderType"].ToString(),
                OrderStatus = row["OrderStatus"].ToString(),
                PaymentStatus = row["PaymentStatus"].ToString(),
                SubTotal = Convert.ToDecimal(row["SubTotal"]),
                Tax = Convert.ToDecimal(row["Tax"]),
                Discount = Convert.ToDecimal(row["Discount"]),
                ServiceCharge = Convert.ToDecimal(row["ServiceCharge"]),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                PaidAmount = Convert.ToDecimal(row["PaidAmount"]),
                ChangeAmount = Convert.ToDecimal(row["ChangeAmount"]),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                DeliveryDate = row["DeliveryDate"] != DBNull.Value ? Convert.ToDateTime(row["DeliveryDate"]) : (DateTime?)null,
                DeliveryAddress = row["DeliveryAddress"]?.ToString(),
                SpecialInstructions = row["SpecialInstructions"]?.ToString(),
                OrderSource = row["OrderSource"]?.ToString(),
                IsVoid = Convert.ToBoolean(row["IsVoid"]),
                VoidReason = row["VoidReason"]?.ToString(),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                CustomerName = row.Table.Columns.Contains("CustomerName") ? row["CustomerName"]?.ToString() : null,
                CustomerPhone = row.Table.Columns.Contains("CustomerPhone") ? row["CustomerPhone"]?.ToString() : null,
                TableNumber = row.Table.Columns.Contains("TableNumber") ? row["TableNumber"]?.ToString() : null,
                CreatedByUserName = row.Table.Columns.Contains("CreatedByUserName") ? row["CreatedByUserName"]?.ToString() : null
            };
        }

        /// <summary>
        /// Gets orders by customer ID
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>List of orders</returns>
        public List<Order> GetOrdersByCustomer(int customerID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                List<Order> orders = new List<Order>();
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@CustomerID", customerID),
            dbHelper.CreateParameter("@DateFrom", dateFrom),
            dbHelper.CreateParameter("@DateTo", dateTo)
        };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetOrdersByCustomer", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    orders.Add(MapDataRowToOrder(row));
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting orders by customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets daily sales for dashboard chart
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="days">Number of days</param>
        /// <returns>DataTable with daily sales</returns>
        public DataTable GetDailySales(int branchID, int days = 7)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@Days", days)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetDailySales", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting daily sales: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets weekly sales for dashboard chart
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="weeks">Number of weeks</param>
        /// <returns>DataTable with weekly sales</returns>
        public DataTable GetWeeklySales(int branchID, int weeks = 4)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@Weeks", weeks)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetWeeklySales", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting weekly sales: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Maps a DataRow to a KitchenOrder object
        /// </summary>
        private KitchenOrder MapDataRowToKitchenOrder(DataRow row)
        {
            return new KitchenOrder
            {
                KitchenOrderID = Convert.ToInt32(row["KitchenOrderID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                OrderItemID = Convert.ToInt32(row["OrderItemID"]),
                KitchenDisplayID = row["KitchenDisplayID"] != DBNull.Value ? Convert.ToInt32(row["KitchenDisplayID"]) : (int?)null,
                Status = row["Status"].ToString(),
                Priority = Convert.ToInt32(row["Priority"]),
                PreparationTime = row["PreparationTime"] != DBNull.Value ? Convert.ToInt32(row["PreparationTime"]) : (int?)null,
                StartTime = row["StartTime"] != DBNull.Value ? Convert.ToDateTime(row["StartTime"]) : (DateTime?)null,
                CompleteTime = row["CompleteTime"] != DBNull.Value ? Convert.ToDateTime(row["CompleteTime"]) : (DateTime?)null,
                Notes = row["Notes"]?.ToString(),
                _Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                OrderNumber = row.Table.Columns.Contains("OrderNumber") ? row["OrderNumber"]?.ToString() : null,
                ItemName = row.Table.Columns.Contains("ItemName") ? row["ItemName"]?.ToString() : null,
                Quantity = row.Table.Columns.Contains("Quantity") ? Convert.ToInt32(row["Quantity"]) : 0,
                Instructions = row.Table.Columns.Contains("Instructions") ? row["Instructions"]?.ToString() : null,
                EstimatedTime = row.Table.Columns.Contains("EstimatedTime") ? Convert.ToInt32(row["EstimatedTime"]) : 0,
                TableNumber = row.Table.Columns.Contains("TableNumber") ? row["TableNumber"]?.ToString() : null,
                PriorityStatus = row.Table.Columns.Contains("PriorityStatus") ? row["PriorityStatus"]?.ToString() : null
            };
        }
    }
}