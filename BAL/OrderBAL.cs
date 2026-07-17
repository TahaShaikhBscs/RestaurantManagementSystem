// ============================================
// BAL/OrderBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Order BAL - Business logic layer for order operations
    /// Contains business rules and validation for order management
    /// </summary>
    public class OrderBAL
    {
        private readonly OrderDAL orderDAL;
        private readonly MenuDAL menuDAL;
        private readonly TableDAL tableDAL;
        private readonly CustomerDAL customerDAL;

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public OrderBAL()
        {
            orderDAL = new OrderDAL();
            menuDAL = new MenuDAL();
            tableDAL = new TableDAL();
            customerDAL = new CustomerDAL();
        }

        /// <summary>
        /// Creates a new order with items
        /// </summary>
        public int CreateOrder(Order order, List<OrderItem> items)
        {
            ValidateOrder(order);
            ValidateOrderItems(items);

            // Generate order number
            order.OrderNumber = GenerateOrderNumber(order.BranchID);

            // Calculate totals
            CalculateOrderTotals(order, items);

            // Insert order
            int orderID = orderDAL.InsertOrder(order);

            // Insert order items
            foreach (var item in items)
            {
                item.OrderID = orderID;
                orderDAL.InsertOrderItem(item);
            }

            // Update table status if dine-in
            if (order.OrderType == "Dine In" && order.TableID.HasValue)
            {
                tableDAL.UpdateTableStatus(order.TableID.Value, "Occupied", order.CreatedBy);
            }

            // Update customer loyalty points
            if (order.CustomerID.HasValue && order.TotalAmount > 0)
            {
                // Add loyalty points (1 point per dollar spent)
                int points = (int)Math.Floor(order.TotalAmount);
                customerDAL.UpdateLoyaltyPoints(order.CustomerID.Value, points, "Add", order.CreatedBy);
            }

            return orderID;
        }

        /// <summary>
        /// Validates order data
        /// </summary>
        private void ValidateOrder(Order order)
        {
            if (order.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (order.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(order.OrderType))
            {
                throw new Exception("Order type is required.");
            }

            string[] validTypes = { "Dine In", "Take Away", "Delivery" };
            if (!Array.Exists(validTypes, t => t == order.OrderType))
            {
                throw new Exception("Invalid order type.");
            }

            // Validate table for dine-in
            if (order.OrderType == "Dine In" && !order.TableID.HasValue)
            {
                throw new Exception("Table is required for dine-in orders.");
            }

            // Validate delivery address for delivery
            if (order.OrderType == "Delivery" && string.IsNullOrWhiteSpace(order.DeliveryAddress))
            {
                throw new Exception("Delivery address is required for delivery orders.");
            }

            if (order.SubTotal < 0 || order.TotalAmount < 0)
            {
                throw new Exception("Order amounts cannot be negative.");
            }
        }

        /// <summary>
        /// Validates order items
        /// </summary>
        private void ValidateOrderItems(List<OrderItem> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new Exception("Order must have at least one item.");
            }

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                {
                    throw new Exception("Item quantity must be greater than 0.");
                }

                if (item.UnitPrice < 0)
                {
                    throw new Exception("Item price cannot be negative.");
                }

                // Validate menu item exists
                var menuItem = menuDAL.GetMenuItemById(item.MenuItemID);
                if (menuItem == null || !menuItem.IsAvailable)
                {
                    throw new Exception($"Menu item is not available.");
                }
            }
        }

        internal List<Order> SearchOrders(int value, string searchTerm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates order totals
        /// </summary>
        private void CalculateOrderTotals(Order order, List<OrderItem> items)
        {
            decimal subTotal = 0;
            decimal totalTax = 0;
            decimal totalDiscount = 0;

            foreach (var item in items)
            {
                // Get menu item for tax and discount
                var menuItem = menuDAL.GetMenuItemById(item.MenuItemID);

                // Calculate item total
                decimal itemTotal = item.UnitPrice * item.Quantity;

                // Apply item discount
                decimal itemDiscount = item.Discount > 0 ? item.Discount : (menuItem?.Discount ?? 0);
                decimal discountAmount = (itemTotal * itemDiscount) / 100;

                // Apply tax
                decimal taxRate = menuItem?.Tax ?? 0;
                decimal taxAmount = ((itemTotal - discountAmount) * taxRate) / 100;

                item.TotalPrice = itemTotal - discountAmount + taxAmount;

                subTotal += itemTotal;
                totalTax += taxAmount;
                totalDiscount += discountAmount;
            }

            order.SubTotal = subTotal;
            order.Tax = totalTax;
            order.Discount = totalDiscount;
            order.ServiceCharge = 0; // Calculate if needed
            order.TotalAmount = subTotal + totalTax - totalDiscount + order.ServiceCharge;
            order.PaidAmount = 0;
            order.PaymentStatus = "Pending";
        }

        /// <summary>
        /// Generates a unique order number
        /// </summary>
        private string GenerateOrderNumber(int branchID)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(1000, 9999).ToString();
            return $"ORD-{branchID}-{date}-{random}";
        }

        /// <summary>
        /// Gets an order by ID with all details
        /// </summary>
        public Order GetOrderById(int orderID)
        {
            if (orderID <= 0)
            {
                throw new Exception("Invalid order ID.");
            }

            DataSet dataSet = orderDAL.GetOrderById(orderID);
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            // Map order header
            Order order = MapDataRowToOrder(dataSet.Tables[0].Rows[0]);

            // Map order items
            if (dataSet.Tables.Count > 1)
            {
                order.OrderItems = new List<OrderItem>();
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    order.OrderItems.Add(MapDataRowToOrderItem(row));
                }
            }

            // Map payments
            if (dataSet.Tables.Count > 2)
            {
                order.Payments = new List<Payment>();
                foreach (DataRow row in dataSet.Tables[2].Rows)
                {
                    order.Payments.Add(MapDataRowToPayment(row));
                }
            }

            return order;
        }

        /// <summary>
        /// Gets orders by branch with filters
        /// </summary>
        public List<Order> GetOrdersByBranch(int branchID, string orderStatus = null, DateTime? dateFrom = null, DateTime? dateTo = null, string orderType = null)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return orderDAL.GetOrdersByBranch(branchID, orderStatus, dateFrom, dateTo, orderType);
        }

        /// <summary>
        /// Updates order status
        /// </summary>
        public bool UpdateOrderStatus(int orderID, string status, string notes, int updatedBy)
        {
            if (orderID <= 0)
            {
                throw new Exception("Invalid order ID.");
            }

            string[] validStatuses = { "Pending", "Cooking", "Ready", "Completed", "Cancelled", "Void" };
            if (!Array.Exists(validStatuses, s => s == status))
            {
                throw new Exception("Invalid order status.");
            }

            return orderDAL.UpdateOrderStatus(orderID, status, notes, updatedBy);
        }

        /// <summary>
        /// Updates kitchen status for an order item
        /// </summary>
        public bool UpdateKitchenStatus(int orderItemID, string status, int updatedBy)
        {
            if (orderItemID <= 0)
            {
                throw new Exception("Invalid order item ID.");
            }

            string[] validStatuses = { "Pending", "Cooking", "Ready", "Served", "Cancelled" };
            if (!Array.Exists(validStatuses, s => s == status))
            {
                throw new Exception("Invalid kitchen status.");
            }

            return orderDAL.UpdateKitchenStatus(orderItemID, status, updatedBy);
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        public bool CancelOrder(int orderID, string reason, int updatedBy)
        {
            if (orderID <= 0)
            {
                throw new Exception("Invalid order ID.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new Exception("Cancellation reason is required.");
            }

            return orderDAL.CancelOrder(orderID, reason, updatedBy);
        }

        /// <summary>
        /// Gets kitchen orders
        /// </summary>
        public List<KitchenOrder> GetKitchenOrders(int branchID, string status = null)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return orderDAL.GetKitchenOrders(branchID, status);
        }

        /// <summary>
        /// Gets top selling items
        /// </summary>
        public List<TopSellingItem> GetTopSellingItems(int branchID, int topCount, DateTime dateFrom, DateTime dateTo)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            var items = new List<TopSellingItem>();
            DataTable dataTable = new ReportDAL().GetTopSellingItems(branchID, dateFrom, dateTo, topCount);

            foreach (DataRow row in dataTable.Rows)
            {
                items.Add(new TopSellingItem
                {
                    ItemName = row["ItemName"].ToString(),
                    TotalQuantity = Convert.ToInt32(row["TotalQuantity"]),
                    TotalSales = Convert.ToDecimal(row["TotalSales"])
                });
            }

            return items;
        }

        /// <summary>
        /// Gets recent orders
        /// </summary>
        public List<RecentOrder> GetRecentOrders(int branchID, int count)
        {
            var orders = GetOrdersByBranch(branchID);
            var recentOrders = new List<RecentOrder>();

            int i = 0;
            foreach (var order in orders)
            {
                if (i >= count) break;
                recentOrders.Add(new RecentOrder
                {
                    OrderID = order.OrderID,
                    OrderNumber = order.OrderNumber,
                    CustomerName = order.CustomerName ?? "Walk-in",
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus,
                    OrderDate = order.OrderDate
                });
                i++;
            }

            return recentOrders;
        }

        /// <summary>
        /// Gets recent payments
        /// </summary>
        public List<RecentPayment> GetRecentPayments(int branchID, int count)
        {
            var recentPayments = new List<RecentPayment>();
            // In a real implementation, this would query payments directly
            // For now, we'll return empty list
            return recentPayments;
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
            if (customerID <= 0)
            {
                throw new Exception("Invalid customer ID.");
            }

            return orderDAL.GetOrdersByCustomer(customerID, dateFrom, dateTo);
        }

        /// <summary>
        /// Gets daily sales for dashboard chart
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="days">Number of days</param>
        /// <returns>DataTable with daily sales</returns>
        public DataTable GetDailySales(int branchID, int days = 7)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (days <= 0 || days > 30)
            {
                throw new Exception("Days must be between 1 and 30.");
            }

            return orderDAL.GetDailySales(branchID, days);
        }

        /// <summary>
        /// Gets weekly sales for dashboard chart
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="weeks">Number of weeks</param>
        /// <returns>DataTable with weekly sales</returns>
        public DataTable GetWeeklySales(int branchID, int weeks = 4)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (weeks <= 0 || weeks > 12)
            {
                throw new Exception("Weeks must be between 1 and 12.");
            }

            return orderDAL.GetWeeklySales(branchID, weeks);
        }

        /// <summary>
        /// Gets top selling items
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="topCount">Number of items to return</param>
        /// <returns>DataTable with top selling items</returns>
        public DataTable GetTopSellingItemsData(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 10)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (dateFrom > dateTo)
            {
                throw new Exception("Start date cannot be after end date.");
            }

            ReportDAL reportDAL = new ReportDAL();
            return reportDAL.GetTopSellingItems(branchID, dateFrom, dateTo, topCount);
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
        /// Maps a DataRow to an OrderItem object
        /// </summary>
        private OrderItem MapDataRowToOrderItem(DataRow row)
        {
            return new OrderItem
            {
                OrderItemID = Convert.ToInt32(row["OrderItemID"]),
                OrderID = Convert.ToInt32(row["OrderID"]),
                MenuItemID = Convert.ToInt32(row["MenuItemID"]),
                Quantity = Convert.ToInt32(row["Quantity"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                Discount = Convert.ToDecimal(row["Discount"]),
                TotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                Instructions = row["Instructions"]?.ToString(),
                KitchenStatus = row["KitchenStatus"].ToString(),
                PreparationTime = row["PreparationTime"] != DBNull.Value ? Convert.ToInt32(row["PreparationTime"]) : (int?)null,
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                ItemName = row.Table.Columns.Contains("ItemName") ? row["ItemName"]?.ToString() : null,
                Barcode = row.Table.Columns.Contains("Barcode") ? row["Barcode"]?.ToString() : null
            };
        }

        /// <summary>
        /// Maps a DataRow to a Payment object
        /// </summary>
        private Payment MapDataRowToPayment(DataRow row)
        {
            return new Payment
            {
                PaymentID = Convert.ToInt32(row["PaymentID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                OrderID = Convert.ToInt32(row["OrderID"]),
                PaymentMethod = row["PaymentMethod"].ToString(),
                Amount = Convert.ToDecimal(row["Amount"]),
                ReferenceNumber = row["ReferenceNumber"]?.ToString(),
                TransactionDate = Convert.ToDateTime(row["TransactionDate"]),
                PaymentStatus = row["PaymentStatus"].ToString(),
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