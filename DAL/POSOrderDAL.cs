// ============================================
// DAL/POSOrderDAL.cs - Complete POS Order DAL
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// POS Order DAL - Data access layer for POS orders
    /// </summary>
    public class POSOrderDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        public POSOrderDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Order CRUD Operations

        /// <summary>
        /// Inserts a new POS order
        /// </summary>
        public int InsertOrder(Order order)
        {
            //try
            //{
            //    List<SqlParameter> parameters = new List<SqlParameter>
            //    {
            //        dbHelper.CreateParameter("@CompanyID", order.CompanyID),
            //        dbHelper.CreateParameter("@BranchID", order.BranchID),
            //        dbHelper.CreateParameter("@TableID", order.TableID),
            //        dbHelper.CreateParameter("@CustomerID", order.CustomerID),
            //        dbHelper.CreateParameter("@OrderNumber", order.OrderNumber),
            //        dbHelper.CreateParameter("@OrderType", order.OrderType),
            //        dbHelper.CreateParameter("@OrderStatus", order.OrderStatus),
            //        dbHelper.CreateParameter("@PaymentStatus", order.PaymentStatus),
            //        dbHelper.CreateParameter("@SubTotal", order.SubTotal),
            //        dbHelper.CreateParameter("@Tax", order.Tax),
            //        dbHelper.CreateParameter("@Discount", order.Discount),
            //        dbHelper.CreateParameter("@ServiceCharge", order.ServiceCharge),
            //        dbHelper.CreateParameter("@DeliveryCharge", order.DeliveryCharge),
            //        dbHelper.CreateParameter("@TotalAmount", order.TotalAmount),
            //        dbHelper.CreateParameter("@PaidAmount", order.PaidAmount),
            //        dbHelper.CreateParameter("@ChangeAmount", order.ChangeAmount),
            //        dbHelper.CreateParameter("@TipAmount", order.TipAmount),
            //        dbHelper.CreateParameter("@DeliveryAddress", order.DeliveryAddress),
            //        dbHelper.CreateParameter("@SpecialInstructions", order.SpecialInstructions),
            //        dbHelper.CreateParameter("@OrderSource", order.OrderSource),
            //        //dbHelper.CreateParameter("@ShiftID", order.ShiftID),
            //        dbHelper.CreateParameter("@CreatedBy", order.CreatedBy)
            //    };

            //    object result = dbHelper.ExecuteScalar("sp_InsertOrder", parameters);
            //    return Convert.ToInt32(result);
            //}
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
        dbHelper.CreateParameter("@DeliveryCharge", order.DeliveryCharge),
        dbHelper.CreateParameter("@TotalAmount", order.TotalAmount),
        dbHelper.CreateParameter("@PaidAmount", order.PaidAmount),
        dbHelper.CreateParameter("@ChangeAmount", order.ChangeAmount),
        dbHelper.CreateParameter("@TipAmount", order.TipAmount), // Added to resolve the error
        dbHelper.CreateParameter("@DeliveryDate", order.DeliveryDate ?? (object)DBNull.Value),
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
                throw new Exception($"Error inserting POS order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing POS order
        /// </summary>
        public bool UpdateOrder(Order order)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", order.OrderID),
                    dbHelper.CreateParameter("@TableID", order.TableID),
                    dbHelper.CreateParameter("@CustomerID", order.CustomerID),
                    dbHelper.CreateParameter("@OrderType", order.OrderType),
                    dbHelper.CreateParameter("@OrderStatus", order.OrderStatus),
                    dbHelper.CreateParameter("@PaymentStatus", order.PaymentStatus),
                    dbHelper.CreateParameter("@SubTotal", order.SubTotal),
                    dbHelper.CreateParameter("@Tax", order.Tax),
                    dbHelper.CreateParameter("@Discount", order.Discount),
                    dbHelper.CreateParameter("@ServiceCharge", order.ServiceCharge),
                    dbHelper.CreateParameter("@DeliveryCharge", order.DeliveryCharge),
                    dbHelper.CreateParameter("@TotalAmount", order.TotalAmount),
                    dbHelper.CreateParameter("@PaidAmount", order.PaidAmount),
                    dbHelper.CreateParameter("@ChangeAmount", order.ChangeAmount),
                    dbHelper.CreateParameter("@TipAmount", order.TipAmount),
                    dbHelper.CreateParameter("@DeliveryAddress", order.DeliveryAddress),
                    dbHelper.CreateParameter("@SpecialInstructions", order.SpecialInstructions),
                    dbHelper.CreateParameter("@ShiftID", order.ShiftID),
                    dbHelper.CreateParameter("@IsHeld", order.IsHeld),
                    dbHelper.CreateParameter("@UpdatedBy", order.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrder_Update", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating POS order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a POS order by ID with all details
        /// </summary>
        public DataSet GetOrderById(int orderID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID)
                };

                using (SqlConnection connection = dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand("sp_POSOrder_GetByID", connection))
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
                throw new Exception($"Error getting POS order by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all POS orders with filters
        /// </summary>
        public DataTable GetOrders(int branchID, string status = null, DateTime? dateFrom = null,
            DateTime? dateTo = null, string searchTerm = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@OrderStatus", status),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                return dbHelper.ExecuteStoredProcedure("sp_POSOrder_GetAll", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting POS orders: {ex.Message}", ex);
            }
        }

        #endregion

        #region Order Status Operations

        /// <summary>
        /// Updates order status
        /// </summary>
        public bool UpdateOrderStatus(int orderID, string status, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@OrderStatus", status),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrder_UpdateStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating order status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates order payment status
        /// </summary>
        public bool UpdatePaymentStatus(int orderID, string paymentStatus, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@PaymentStatus", paymentStatus),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrder_UpdatePaymentStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payment status: {ex.Message}", ex);
            }
        }

        #endregion

        #region Order Item Operations

        /// <summary>
        /// Inserts an order item
        /// </summary>
        public int InsertOrderItem(OrderItem item)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", item.OrderID),
                    dbHelper.CreateParameter("@MenuItemID", item.MenuItemID),
                    //dbHelper.CreateParameter("@DealID", item.DealID),
                    dbHelper.CreateParameter("@Quantity", item.Quantity),
                    dbHelper.CreateParameter("@UnitPrice", item.UnitPrice),
                    dbHelper.CreateParameter("@Discount", item.Discount),
                    dbHelper.CreateParameter("@TotalPrice", item.TotalPrice),
                    dbHelper.CreateParameter("@Instructions", item.Instructions),
                    dbHelper.CreateParameter("@KitchenStatus", item.KitchenStatus),
                    dbHelper.CreateParameter("@PreparationTime", item.PreparationTime),
                    dbHelper.CreateParameter("@CreatedBy", item.CreatedBy)
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
        /// Updates an order item
        /// </summary>
        public bool UpdateOrderItem(OrderItem item)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderItemID", item.OrderItemID),
                    dbHelper.CreateParameter("@Quantity", item.Quantity),
                    dbHelper.CreateParameter("@UnitPrice", item.UnitPrice),
                    dbHelper.CreateParameter("@Discount", item.Discount),
                    dbHelper.CreateParameter("@TotalPrice", item.TotalPrice),
                    dbHelper.CreateParameter("@Instructions", item.Instructions),
                    dbHelper.CreateParameter("@KitchenStatus", item.KitchenStatus),
                    dbHelper.CreateParameter("@UpdatedBy", item.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrderItem_Update", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating order item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes an order item
        /// </summary>
        public bool DeleteOrderItem(int orderItemID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderItemID", orderItemID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrderItem_Delete", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting order item: {ex.Message}", ex);
            }
        }

        #endregion

        #region Payment Operations

        /// <summary>
        /// Inserts a payment
        /// </summary>
        public int InsertPayment(Payment payment)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", payment.CompanyID),
                    dbHelper.CreateParameter("@BranchID", payment.BranchID),
                    dbHelper.CreateParameter("@OrderID", payment.OrderID),
                    dbHelper.CreateParameter("@PaymentMethod", payment.PaymentMethod),
                    dbHelper.CreateParameter("@Amount", payment.Amount),
                    dbHelper.CreateParameter("@ReferenceNumber", payment.ReferenceNumber),
                    dbHelper.CreateParameter("@PaymentStatus", payment.PaymentStatus),
                    dbHelper.CreateParameter("@CreatedBy", payment.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_POSPayment_Insert", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting payment: {ex.Message}", ex);
            }
        }

        #endregion

        #region Hold/Resume Operations

        /// <summary>
        /// Holds an order
        /// </summary>
        public bool HoldOrder(int orderID, int heldBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@HeldBy", heldBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrder_Hold", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error holding order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Resumes a held order
        /// </summary>
        public bool ResumeOrder(int orderID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrder_Resume", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error resuming order: {ex.Message}", ex);
            }
        }

        #endregion

        #region Kitchen Operations

        /// <summary>
        /// Sends order to kitchen
        /// </summary>
        public bool SendToKitchen(int orderID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_POSOrder_SendToKitchen", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending order to kitchen: {ex.Message}", ex);
            }
        }

        #endregion
    }
}