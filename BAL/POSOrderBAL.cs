// ============================================
// BAL/POSOrderBAL.cs - Complete POS Order BAL
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// POS Order BAL - Business logic layer for POS orders
    /// </summary>
    public class POSOrderBAL
    {
        #region Private Fields

        private readonly POSOrderDAL orderDAL;
        private readonly MenuDAL menuDAL;
        private readonly CustomerDAL customerDAL;
        private readonly TableDAL tableDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        public POSOrderBAL()
        {
            orderDAL = new POSOrderDAL();
            menuDAL = new MenuDAL();
            customerDAL = new CustomerDAL();
            tableDAL = new TableDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Order Management

        /// <summary>
        /// Creates a new POS order
        /// </summary>
        public int CreateOrder(Order order, List<OrderItem> items)
        {
            ValidateOrder(order);
            ValidateOrderItems(items);

            // Calculate totals
            CalculateOrderTotals(order, items);

            // Generate order number
            order.OrderNumber = GenerateOrderNumber(order.BranchID);
            order.OrderStatus = "Pending";
            order.PaymentStatus = "Pending";
            order.OrderDate = DateTime.Now;

            // Insert order
            int orderID = orderDAL.InsertOrder(order);

            // Insert items
            foreach (var item in items)
            {
                item.OrderID = orderID;
                item.CreatedBy = order.CreatedBy;
                item.KitchenStatus = "Pending";
                orderDAL.InsertOrderItem(item);
            }

            auditLogger.LogInsert("POS", "Orders", orderID,
                $"OrderNumber: {order.OrderNumber}, Total: {order.TotalAmount}");

            return orderID;
        }

        /// <summary>
        /// Updates an existing POS order
        /// </summary>
        public bool UpdateOrder(Order order, List<OrderItem> items)
        {
            ValidateOrder(order);
            ValidateOrderItems(items);

            // Calculate totals
            CalculateOrderTotals(order, items);

            // Update order
            bool updated = orderDAL.UpdateOrder(order);

            if (updated)
            {
                auditLogger.LogUpdate("POS", "Orders", order.OrderID,
                    $"OrderID: {order.OrderID}", $"Total: {order.TotalAmount}");
            }

            return updated;
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets an order by ID with all details
        /// </summary>
        public DataSet GetOrderById(int orderID)
        {
            if (orderID <= 0)
                throw new Exception("Invalid order ID.");

            return orderDAL.GetOrderById(orderID);
        }

        /// <summary>
        /// Gets all orders with filters
        /// </summary>
        public DataTable GetOrders(int branchID, string status = null, DateTime? dateFrom = null,
            DateTime? dateTo = null, string searchTerm = null)
        {
            if (branchID <= 0)
                throw new Exception("Invalid branch ID.");

            return orderDAL.GetOrders(branchID, status, dateFrom, dateTo, searchTerm);
        }

        #endregion

        #region Status Management

        /// <summary>
        /// Updates order status
        /// </summary>
        public bool UpdateOrderStatus(int orderID, string status, int updatedBy)
        {
            if (orderID <= 0)
                throw new Exception("Invalid order ID.");

            if (string.IsNullOrWhiteSpace(status))
                throw new Exception("Status is required.");

            bool updated = orderDAL.UpdateOrderStatus(orderID, status, updatedBy);

            if (updated)
            {
                auditLogger.LogUpdate("POS", "Orders", orderID,
                    $"OrderID: {orderID}", $"Status: {status}");
            }

            return updated;
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        public bool CancelOrder(int orderID, string reason, int updatedBy)
        {
            if (orderID <= 0)
                throw new Exception("Invalid order ID.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Cancellation reason is required.");

            // Get order status
            DataSet orderData = orderDAL.GetOrderById(orderID);
            if (orderData.Tables.Count > 0 && orderData.Tables[0].Rows.Count > 0)
            {
                string currentStatus = orderData.Tables[0].Rows[0]["OrderStatus"].ToString();

                // Check if order can be cancelled
                if (currentStatus == "Completed")
                    throw new Exception("Cannot cancel a completed order.");
            }

            bool updated = orderDAL.UpdateOrderStatus(orderID, "Cancelled", updatedBy);

            if (updated)
            {
                auditLogger.LogUpdate("POS", "Orders", orderID,
                    $"OrderID: {orderID}", $"Cancelled, Reason: {reason}");
            }

            return updated;
        }

        #endregion

        #region Hold/Resume

        /// <summary>
        /// Holds an order
        /// </summary>
        public bool HoldOrder(int orderID, int heldBy)
        {
            if (orderID <= 0)
                throw new Exception("Invalid order ID.");

            bool held = orderDAL.HoldOrder(orderID, heldBy);

            if (held)
            {
                auditLogger.LogUpdate("POS", "Orders", orderID,
                    $"OrderID: {orderID}", "Held");
            }

            return held;
        }

        /// <summary>
        /// Resumes a held order
        /// </summary>
        public bool ResumeOrder(int orderID, int updatedBy)
        {
            if (orderID <= 0)
                throw new Exception("Invalid order ID.");

            bool resumed = orderDAL.ResumeOrder(orderID, updatedBy);

            if (resumed)
            {
                auditLogger.LogUpdate("POS", "Orders", orderID,
                    $"OrderID: {orderID}", "Resumed");
            }

            return resumed;
        }

        #endregion

        #region Kitchen Operations

        /// <summary>
        /// Sends order to kitchen
        /// </summary>
        public bool SendToKitchen(int orderID, int updatedBy)
        {
            if (orderID <= 0)
                throw new Exception("Invalid order ID.");

            bool sent = orderDAL.SendToKitchen(orderID, updatedBy);

            if (sent)
            {
                // Update order status to Cooking
                UpdateOrderStatus(orderID, "Cooking", updatedBy);

                auditLogger.LogUpdate("POS", "Orders", orderID,
                    $"OrderID: {orderID}", "Sent to Kitchen");
            }

            return sent;
        }

        #endregion

        #region Payment Processing

        /// <summary>
        /// Processes a payment
        /// </summary>
        public bool ProcessPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            if (payment.OrderID <= 0)
                throw new Exception("Invalid order ID.");

            if (payment.Amount <= 0)
                throw new Exception("Payment amount must be greater than 0.");

            if (string.IsNullOrWhiteSpace(payment.PaymentMethod))
                throw new Exception("Payment method is required.");

            int paymentID = orderDAL.InsertPayment(payment);

            if (paymentID > 0)
            {
                // Update order payment status
                DataSet orderData = orderDAL.GetOrderById(payment.OrderID);
                if (orderData.Tables.Count > 0 && orderData.Tables[0].Rows.Count > 0)
                {
                    decimal totalAmount = Convert.ToDecimal(orderData.Tables[0].Rows[0]["TotalAmount"]);
                    decimal paidAmount = Convert.ToDecimal(orderData.Tables[0].Rows[0]["PaidAmount"]) + payment.Amount;

                    string paymentStatus = paidAmount >= totalAmount ? "Completed" : "Partial";
                    orderDAL.UpdatePaymentStatus(payment.OrderID, paymentStatus, payment.CreatedBy);
                }

                auditLogger.LogInsert("POS", "Payments", paymentID,
                    $"OrderID: {payment.OrderID}, Amount: {payment.Amount}");

                return true;
            }

            return false;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validates order data
        /// </summary>
        private void ValidateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (order.CompanyID <= 0)
                throw new Exception("Company is required.");

            if (order.BranchID <= 0)
                throw new Exception("Branch is required.");

            if (string.IsNullOrWhiteSpace(order.OrderType))
                throw new Exception("Order type is required.");

            string[] validTypes = { "Dine In", "Take Away", "Delivery" };
            if (!Array.Exists(validTypes, t => t == order.OrderType))
                throw new Exception("Invalid order type.");

            if (order.OrderType == "Dine In" && !order.TableID.HasValue)
                throw new Exception("Table is required for dine-in orders.");

            if (order.OrderType == "Delivery" && string.IsNullOrWhiteSpace(order.DeliveryAddress))
                throw new Exception("Delivery address is required.");

            if (order.CustomerID.HasValue && order.CustomerID.Value > 0)
            {
                var customer = customerDAL.GetCustomerById(order.CustomerID.Value);
                if (customer == null)
                    throw new Exception("Customer not found.");
            }

            if (order.TableID.HasValue && order.TableID.Value > 0)
            {
                var table = tableDAL.GetTableById(order.TableID.Value);
                if (table == null)
                    throw new Exception("Table not found.");
            }
        }

        /// <summary>
        /// Validates order items
        /// </summary>
        private void ValidateOrderItems(List<OrderItem> items)
        {
            if (items == null || items.Count == 0)
                throw new Exception("Order must have at least one item.");

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                    throw new Exception("Item quantity must be greater than 0.");

                if (item.UnitPrice < 0)
                    throw new Exception("Item price cannot be negative.");

                // Validate menu item exists
                var menuItem = menuDAL.GetMenuItemById(item.MenuItemID);
                if (menuItem == null)
                    throw new Exception($"Menu item not found.");
            }
        }

        /// <summary>
        /// Calculates order totals
        /// </summary>
        private void CalculateOrderTotals(Order order, List<OrderItem> items)
        {
            decimal subTotal = 0;

            foreach (var item in items)
            {
                item.TotalPrice = (item.Quantity * item.UnitPrice) - item.Discount;
                subTotal += item.TotalPrice;
            }

            decimal tax = subTotal * 0.05m; // 5% tax - configurable
            decimal discount = order.Discount;
            decimal serviceCharge = order.ServiceCharge;
            decimal deliveryCharge = order.DeliveryCharge;
            decimal total = subTotal + tax - discount + serviceCharge + deliveryCharge;

            order.SubTotal = subTotal;
            order.Tax = tax;
            order.TotalAmount = total;
            order.ChangeAmount = 0;
            order.PaidAmount = 0;
        }

        /// <summary>
        /// Generates a unique order number
        /// </summary>
        private string GenerateOrderNumber(int branchID)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(1000, 9999).ToString();
            return $"POS-{branchID}-{date}-{random}";
        }

        #endregion
    }
}