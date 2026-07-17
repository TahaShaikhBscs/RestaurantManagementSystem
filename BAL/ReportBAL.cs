// ============================================
// BAL/ReportBAL.cs - Production-Ready Refactored
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Report Business Logic Layer - Handles all reporting operations
    /// Provides methods for generating sales, expense, inventory, customer, 
    /// payment, profit/loss, and kitchen performance reports
    /// </summary>
    public class ReportBAL
    {
        #region Private Fields

        private readonly ReportDAL reportDAL;
        private readonly OrderDAL orderDAL;
        private readonly StockDAL stockDAL;
        private readonly ExpenseDAL expenseDAL;
        private readonly CustomerDAL customerDAL;
        private readonly PaymentDAL paymentDAL;
        private readonly SupplierDAL supplierDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ReportBAL class
        /// </summary>
        public ReportBAL()
        {
            reportDAL = new ReportDAL();
            orderDAL = new OrderDAL();
            stockDAL = new StockDAL();
            expenseDAL = new ExpenseDAL();
            customerDAL = new CustomerDAL();
            paymentDAL = new PaymentDAL();
            supplierDAL = new SupplierDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Validates branch ID
        /// </summary>
        private void ValidateBranch(int branchID)
        {
            if (branchID <= 0)
                throw new ArgumentException("Branch ID must be greater than 0.", nameof(branchID));
        }

        /// <summary>
        /// Validates date range
        /// </summary>
        private void ValidateDateRange(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo)
                throw new ArgumentException("Start date cannot be after end date.");
        }

        /// <summary>
        /// Validates top count
        /// </summary>
        private void ValidateTopCount(int topCount)
        {
            if (topCount <= 0 || topCount > 100)
                throw new ArgumentException("Top count must be between 1 and 100.", nameof(topCount));
        }

        /// <summary>
        /// Validates period (Day, Month, Year)
        /// </summary>
        private void ValidateGroupBy(string groupBy)
        {
            string[] validGroups = { "Day", "Month", "Year" };
            if (!Array.Exists(validGroups, g => g.Equals(groupBy, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("GroupBy must be Day, Month, or Year.", nameof(groupBy));
        }

        /// <summary>
        /// Validates stock status filter
        /// </summary>
        private void ValidateStockStatus(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                string[] validStatuses = { "Low", "Normal", "Over" };
                if (!Array.Exists(validStatuses, s => s.Equals(status, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException("Status must be Low, Normal, or Over.", nameof(status));
            }
        }

        #endregion

        #region Sales Reports

        /// <summary>
        /// Gets sales report data
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="groupBy">Group by Day, Month, or Year</param>
        /// <returns>DataTable with sales report</returns>
        public DataTable GetSalesReport(int branchID, DateTime dateFrom, DateTime dateTo, string groupBy = "Day")
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);
                ValidateGroupBy(groupBy);

                DataTable result = reportDAL.GetSalesReport(branchID, dateFrom, dateTo, groupBy);
                auditLogger.LogAction("ViewReport", "Reports", "SalesReport", null,
                    $"BranchID: {branchID}, From: {dateFrom:yyyy-MM-dd}, To: {dateTo:yyyy-MM-dd}, Group: {groupBy}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "SalesReport", null, null, ex.Message);
                throw new Exception($"Error generating sales report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets weekly sales data for dashboard charts
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="weeks">Number of weeks</param>
        /// <returns>DataTable with weekly sales data</returns>
        public DataTable GetWeeklySalesData(int branchID, int weeks = 4)
        {
            try
            {
                ValidateBranch(branchID);
                if (weeks <= 0 || weeks > 12)
                    throw new ArgumentException("Weeks must be between 1 and 12.", nameof(weeks));

                DataTable result = orderDAL.GetWeeklySales(branchID, weeks);
                auditLogger.LogAction("ViewReport", "Reports", "WeeklySales", null,
                    $"BranchID: {branchID}, Weeks: {weeks}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "WeeklySales", null, null, ex.Message);
                throw new Exception($"Error generating weekly sales data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets daily sales data for dashboard charts
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="days">Number of days</param>
        /// <returns>DataTable with daily sales data</returns>
        public DataTable GetDailySalesData(int branchID, int days = 7)
        {
            try
            {
                ValidateBranch(branchID);
                if (days <= 0 || days > 30)
                    throw new ArgumentException("Days must be between 1 and 30.", nameof(days));

                DataTable result = orderDAL.GetDailySales(branchID, days);
                auditLogger.LogAction("ViewReport", "Reports", "DailySales", null,
                    $"BranchID: {branchID}, Days: {days}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "DailySales", null, null, ex.Message);
                throw new Exception($"Error generating daily sales data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets top selling items
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="topCount">Number of items to return</param>
        /// <returns>DataTable with top selling items</returns>
        public DataTable GetTopSellingItems(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 10)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);
                ValidateTopCount(topCount);

                DataTable result = reportDAL.GetTopSellingItems(branchID, dateFrom, dateTo, topCount);
                auditLogger.LogAction("ViewReport", "Reports", "TopSellingItems", null,
                    $"BranchID: {branchID}, Top: {topCount}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "TopSellingItems", null, null, ex.Message);
                throw new Exception($"Error generating top selling items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets sales report by order type
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="orderType">Order type (Dine In, Take Away, Delivery)</param>
        /// <returns>DataTable with sales by order type</returns>
        public DataTable GetSalesByOrderType(int branchID, DateTime dateFrom, DateTime dateTo, string orderType = null)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                // Get all orders and filter by type
                var orders = orderDAL.GetOrdersByBranch(branchID, null, dateFrom, dateTo, orderType);

                DataTable result = new DataTable();
                result.Columns.Add("OrderType");
                result.Columns.Add("TotalOrders");
                result.Columns.Add("TotalSales");
                result.Columns.Add("TotalTax");
                result.Columns.Add("TotalDiscount");
                result.Columns.Add("AverageOrder");

                // Group by order type
                var groups = new Dictionary<string, (int Count, decimal Sales, decimal Tax, decimal Discount)>();

                foreach (var order in orders)
                {
                    string type = order.OrderType;
                    if (!groups.ContainsKey(type))
                        groups[type] = (0, 0, 0, 0);

                    var current = groups[type];
                    groups[type] = (
                        current.Count + 1,
                        current.Sales + order.TotalAmount,
                        current.Tax + order.Tax,
                        current.Discount + order.Discount
                    );
                }

                foreach (var group in groups)
                {
                    var row = result.NewRow();
                    row["OrderType"] = group.Key;
                    row["TotalOrders"] = group.Value.Count;
                    row["TotalSales"] = group.Value.Sales;
                    row["TotalTax"] = group.Value.Tax;
                    row["TotalDiscount"] = group.Value.Discount;
                    row["AverageOrder"] = group.Value.Count > 0 ? group.Value.Sales / group.Value.Count : 0;
                    result.Rows.Add(row);
                }

                auditLogger.LogAction("ViewReport", "Reports", "SalesByOrderType", null,
                    $"BranchID: {branchID}, Type: {orderType ?? "All"}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "SalesByOrderType", null, null, ex.Message);
                throw new Exception($"Error generating sales by order type: {ex.Message}", ex);
            }
        }

        #endregion

        #region Expense Reports

        /// <summary>
        /// Gets expense report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="category">Category filter</param>
        /// <returns>DataTable with expense report</returns>
        public DataTable GetExpenseReport(int branchID, DateTime dateFrom, DateTime dateTo, string category = null)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                DataTable result = reportDAL.GetExpenseReport(branchID, dateFrom, dateTo, category);
                auditLogger.LogAction("ViewReport", "Reports", "ExpenseReport", null,
                    $"BranchID: {branchID}, Category: {category ?? "All"}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "ExpenseReport", null, null, ex.Message);
                throw new Exception($"Error generating expense report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets expense summary by category
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with expense summary by category</returns>
        public DataTable GetExpenseSummaryByCategory(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                DataTable result = expenseDAL.GetExpensesSummary(branchID, dateFrom, dateTo);
                auditLogger.LogAction("ViewReport", "Reports", "ExpenseSummaryByCategory", null,
                    $"BranchID: {branchID}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "ExpenseSummaryByCategory", null, null, ex.Message);
                throw new Exception($"Error generating expense summary by category: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets expense categories
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <returns>List of expense categories</returns>
        public List<string> GetExpenseCategories(int branchID)
        {
            try
            {
                ValidateBranch(branchID);
                return expenseDAL.GetExpenseCategories(branchID);
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "GetExpenseCategories", null, null, ex.Message);
                throw new Exception($"Error getting expense categories: {ex.Message}", ex);
            }
        }

        #endregion

        #region Payment Reports

        /// <summary>
        /// Gets payment report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with payment report</returns>
        public DataTable GetPaymentReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                DataTable result = reportDAL.GetPaymentReportData(branchID, dateFrom, dateTo);
                auditLogger.LogAction("ViewReport", "Reports", "PaymentReport", null,
                    $"BranchID: {branchID}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "PaymentReport", null, null, ex.Message);
                throw new Exception($"Error generating payment report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets payment summary by method
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with payment summary by method</returns>
        public DataTable GetPaymentSummaryByMethod(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                DataTable result = reportDAL.GetPaymentReport(branchID, dateFrom, dateTo);
                auditLogger.LogAction("ViewReport", "Reports", "PaymentSummaryByMethod", null,
                    $"BranchID: {branchID}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "PaymentSummaryByMethod", null, null, ex.Message);
                throw new Exception($"Error generating payment summary by method: {ex.Message}", ex);
            }
        }

        #endregion

        #region Profit & Loss Reports

        /// <summary>
        /// Gets profit and loss report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with profit and loss report</returns>
        public DataTable GetProfitLossReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                DataTable result = reportDAL.GetProfitLossReport(branchID, dateFrom, dateTo);
                auditLogger.LogAction("ViewReport", "Reports", "ProfitLossReport", null,
                    $"BranchID: {branchID}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "ProfitLossReport", null, null, ex.Message);
                throw new Exception($"Error generating profit and loss report: {ex.Message}", ex);
            }
        }

        #endregion

        #region Customer Reports

        /// <summary>
        /// Gets customer summary report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="topCount">Number of customers to return</param>
        /// <returns>DataTable with customer summary</returns>
        public DataTable GetCustomerSummaryReport(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 20)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);
                ValidateTopCount(topCount);

                DataTable result = customerDAL.GetCustomerSummaryReport(branchID, dateFrom, dateTo, topCount);
                auditLogger.LogAction("ViewReport", "Reports", "CustomerSummaryReport", null,
                    $"BranchID: {branchID}, Top: {topCount}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "CustomerSummaryReport", null, null, ex.Message);
                throw new Exception($"Error generating customer summary report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets customers with loyalty points
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="minPoints">Minimum loyalty points</param>
        /// <param name="topCount">Number of customers to return</param>
        /// <returns>DataTable with customer loyalty data</returns>
        public DataTable GetCustomerLoyaltyReport(int branchID, int minPoints = 0, int topCount = 20)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateTopCount(topCount);

                DataTable result = customerDAL.GetCustomersWithLoyalty(branchID, minPoints, topCount);
                auditLogger.LogAction("ViewReport", "Reports", "CustomerLoyaltyReport", null,
                    $"BranchID: {branchID}, MinPoints: {minPoints}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "CustomerLoyaltyReport", null, null, ex.Message);
                throw new Exception($"Error generating customer loyalty report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets VIP customers
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <returns>List of VIP customers</returns>
        public List<Customer> GetVIPCustomers(int branchID)
        {
            try
            {
                ValidateBranch(branchID);
                var customers = customerDAL.GetCustomersByBranch(branchID);
                var vipCustomers = new List<Customer>();

                foreach (var customer in customers)
                {
                    if (customer.CustomerType == "VIP")
                        vipCustomers.Add(customer);
                }

                auditLogger.LogAction("ViewReport", "Reports", "VIPCustomers", null, $"BranchID: {branchID}");
                return vipCustomers;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "VIPCustomers", null, null, ex.Message);
                throw new Exception($"Error getting VIP customers: {ex.Message}", ex);
            }
        }

        #endregion

        #region Inventory Reports

        /// <summary>
        /// Gets inventory report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="category">Category filter</param>
        /// <returns>DataTable with inventory report</returns>
        public DataTable GetInventoryReport(int branchID, string category = null)
        {
            try
            {
                ValidateBranch(branchID);

                DataTable result = stockDAL.GetInventoryReport(branchID, category);
                auditLogger.LogAction("ViewReport", "Reports", "InventoryReport", null,
                    $"BranchID: {branchID}, Category: {category ?? "All"}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "InventoryReport", null, null, ex.Message);
                throw new Exception($"Error generating inventory report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets low stock items
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <returns>List of low stock items</returns>
        public List<Stock> GetLowStockItems(int branchID)
        {
            try
            {
                ValidateBranch(branchID);
                var result = stockDAL.GetLowStockItems(branchID);
                auditLogger.LogAction("ViewReport", "Reports", "LowStockItems", null, $"BranchID: {branchID}");
                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "LowStockItems", null, null, ex.Message);
                throw new Exception($"Error getting low stock items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets stock transactions report
        /// </summary>
        /// <param name="stockID">Stock ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with stock transactions</returns>
        public DataTable GetStockTransactionsReport(int stockID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                if (stockID <= 0)
                    throw new ArgumentException("Stock ID must be greater than 0.", nameof(stockID));

                var transactions = stockDAL.GetStockTransactions(stockID, dateFrom, dateTo);

                DataTable result = new DataTable();
                result.Columns.Add("TransactionID");
                result.Columns.Add("Type");
                result.Columns.Add("Quantity");
                result.Columns.Add("UnitPrice");
                result.Columns.Add("TotalAmount");
                result.Columns.Add("Notes");
                result.Columns.Add("Date");

                foreach (var trans in transactions)
                {
                    var row = result.NewRow();
                    row["TransactionID"] = trans.StockTransactionID;
                    row["Type"] = trans.TransactionType;
                    row["Quantity"] = trans.Quantity;
                    row["UnitPrice"] = trans.UnitPrice ?? 0;
                    row["TotalAmount"] = trans.TotalAmount ?? 0;
                    row["Notes"] = trans.Notes;
                    row["Date"] = trans.TransactionDate;
                    result.Rows.Add(row);
                }

                auditLogger.LogAction("ViewReport", "Reports", "StockTransactions", null, $"StockID: {stockID}");
                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "StockTransactions", null, null, ex.Message);
                throw new Exception($"Error getting stock transactions: {ex.Message}", ex);
            }
        }

        #endregion

        #region Kitchen Reports

        /// <summary>
        /// Gets kitchen performance report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with kitchen performance report</returns>
        public DataTable GetKitchenPerformanceReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                ValidateBranch(branchID);
                ValidateDateRange(dateFrom, dateTo);

                DataTable result = reportDAL.GetKitchenPerformanceReport(branchID, dateFrom, dateTo);
                auditLogger.LogAction("ViewReport", "Reports", "KitchenPerformanceReport", null,
                    $"BranchID: {branchID}");

                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "KitchenPerformanceReport", null, null, ex.Message);
                throw new Exception($"Error generating kitchen performance report: {ex.Message}", ex);
            }
        }

        #endregion

        #region Supplier Reports

        /// <summary>
        /// Gets supplier report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="includeInactive">Include inactive suppliers</param>
        /// <returns>List of suppliers with purchase summary</returns>
        public DataTable GetSupplierReport(int branchID, bool includeInactive = false)
        {
            try
            {
                ValidateBranch(branchID);

                var suppliers = supplierDAL.GetSuppliersByBranch(branchID, includeInactive);

                DataTable result = new DataTable();
                result.Columns.Add("SupplierID");
                result.Columns.Add("SupplierName");
                result.Columns.Add("ContactPerson");
                result.Columns.Add("Phone");
                result.Columns.Add("Email");
                result.Columns.Add("Status");
                result.Columns.Add("TotalPurchases");

                foreach (var supplier in suppliers)
                {
                    var row = result.NewRow();
                    row["SupplierID"] = supplier.SupplierID;
                    row["SupplierName"] = supplier.SupplierName;
                    row["ContactPerson"] = supplier.ContactPerson;
                    row["Phone"] = supplier.Phone;
                    row["Email"] = supplier.Email;
                    row["Status"] = supplier.Status ? "Active" : "Inactive";
                    row["TotalPurchases"] = 0; // Would calculate from purchase orders
                    result.Rows.Add(row);
                }

                auditLogger.LogAction("ViewReport", "Reports", "SupplierReport", null, $"BranchID: {branchID}");
                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "SupplierReport", null, null, ex.Message);
                throw new Exception($"Error generating supplier report: {ex.Message}", ex);
            }
        }

        #endregion

        #region Dashboard Reports

        /// <summary>
        /// Gets dashboard statistics
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <returns>DataSet with dashboard statistics</returns>
        public DataSet GetDashboardStatistics(int branchID)
        {
            try
            {
                ValidateBranch(branchID);
                DataSet result = reportDAL.GetDashboardStatistics(branchID);
                auditLogger.LogAction("ViewReport", "Reports", "DashboardStatistics", null, $"BranchID: {branchID}");
                return result;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "DashboardStatistics", null, null, ex.Message);
                throw new Exception($"Error getting dashboard statistics: {ex.Message}", ex);
            }
        }

        #endregion

        #region Export Methods

        /// <summary>
        /// Exports report data to CSV
        /// </summary>
        /// <param name="dataTable">DataTable to export</param>
        /// <returns>CSV string</returns>
        public string ExportToCSV(DataTable dataTable)
        {
            try
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                    throw new ArgumentException("No data to export.");

                ReportHelper reportHelper = new ReportHelper();
                string csv = reportHelper.ExportToCSV(dataTable);

                auditLogger.LogAction("Export", "Reports", "ExportToCSV", null, $"Rows: {dataTable.Rows.Count}");
                return csv;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "ExportToCSV", null, null, ex.Message);
                throw new Exception($"Error exporting to CSV: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Exports report data to HTML
        /// </summary>
        /// <param name="dataTable">DataTable to export</param>
        /// <param name="title">Report title</param>
        /// <returns>HTML string</returns>
        public string ExportToHTML(DataTable dataTable, string title = null)
        {
            try
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                    throw new ArgumentException("No data to export.");

                ReportHelper reportHelper = new ReportHelper();
                string html = reportHelper.ExportToHTML(dataTable, title);

                auditLogger.LogAction("Export", "Reports", "ExportToHTML", null, $"Rows: {dataTable.Rows.Count}");
                return html;
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", "Reports", "ExportToHTML", null, null, ex.Message);
                throw new Exception($"Error exporting to HTML: {ex.Message}", ex);
            }
        }

        #endregion
    }
}