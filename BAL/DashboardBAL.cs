// ============================================
// BAL/DashboardBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using System.Data.SqlClient;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Dashboard BAL - Business logic for dashboard operations
    /// Aggregates data from multiple sources for dashboard display
    /// </summary>
    public class DashboardBAL
    {
        private readonly DatabaseHelper dbHelper;
        private readonly OrderBAL orderBAL;
        private readonly CustomerBAL customerBAL;
        private readonly MenuBAL menuBAL;
        private readonly ExpenseBAL expenseBAL;

        /// <summary>
        /// Constructor initializes required BAL and DAL objects
        /// </summary>
        public DashboardBAL()
        {
            dbHelper = new DatabaseHelper();
            orderBAL = new OrderBAL();
            customerBAL = new CustomerBAL();
            menuBAL = new MenuBAL();
            expenseBAL = new ExpenseBAL();
        }

        /// <summary>
        /// Gets all dashboard data for a branch
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <returns>DashboardData object with all metrics</returns>
        public DashboardData GetDashboardData(int branchID)
        {
            try
            {
                DashboardData dashboard = new DashboardData();

                // Get date range for current month
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime endDate = DateTime.Now;

                // Get data from stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", startDate),
                    dbHelper.CreateParameter("@DateTo", endDate)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetDashboardData", parameters);

                // Process the results - assuming the stored procedure returns multiple result sets
                // For simplicity, we'll extract from the first result set
                if (dataTable.Rows.Count > 0)
                {
                    // Calculate from existing methods instead
                    dashboard.TodaySales = GetTodaySales(branchID);
                    dashboard.MonthlySales = GetMonthlySales(branchID);
                    dashboard.TotalOrders = GetTotalOrdersToday(branchID);
                    dashboard.RunningOrders = GetRunningOrders(branchID);
                    dashboard.KitchenOrders = GetKitchenOrders(branchID);
                    dashboard.TotalCustomers = customerBAL.GetTotalCustomers(branchID);
                    dashboard.TotalProducts = menuBAL.GetTotalMenuItems(branchID);
                    dashboard.TotalExpenses = expenseBAL.GetTotalExpenses(branchID, startDate, endDate);
                    dashboard.TopSellingItems = orderBAL.GetTopSellingItems(branchID, 5, startDate, endDate);
                    dashboard.RecentOrders = orderBAL.GetRecentOrders(branchID, 5);
                    dashboard.RecentPayments = orderBAL.GetRecentPayments(branchID, 5);
                    dashboard.DailySalesChart = GetDailySalesData(branchID, 7);
                }

                return dashboard;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting dashboard data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets today's sales
        /// </summary>
        private decimal GetTodaySales(int branchID)
        {
            var orders = orderBAL.GetOrdersByBranch(branchID, null, DateTime.Today, DateTime.Now);
            decimal total = 0;
            foreach (var order in orders)
            {
                if (order.OrderStatus != "Cancelled" && order.OrderStatus != "Void")
                {
                    total += order.TotalAmount;
                }
            }
            return total;
        }

        /// <summary>
        /// Gets monthly sales
        /// </summary>
        private decimal GetMonthlySales(int branchID)
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var orders = orderBAL.GetOrdersByBranch(branchID, null, startDate, DateTime.Now);
            decimal total = 0;
            foreach (var order in orders)
            {
                if (order.OrderStatus != "Cancelled" && order.OrderStatus != "Void")
                {
                    total += order.TotalAmount;
                }
            }
            return total;
        }

        /// <summary>
        /// Gets total orders today
        /// </summary>
        private int GetTotalOrdersToday(int branchID)
        {
            var orders = orderBAL.GetOrdersByBranch(branchID, null, DateTime.Today, DateTime.Now);
            return orders.Count;
        }

        /// <summary>
        /// Gets running orders
        /// </summary>
        private int GetRunningOrders(int branchID)
        {
            var orders = orderBAL.GetOrdersByBranch(branchID);
            int count = 0;
            foreach (var order in orders)
            {
                if (order.OrderStatus == "Pending" || order.OrderStatus == "Cooking" || order.OrderStatus == "Ready")
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Gets kitchen orders
        /// </summary>
        private int GetKitchenOrders(int branchID)
        {
            var orders = orderBAL.GetOrdersByBranch(branchID);
            int count = 0;
            foreach (var order in orders)
            {
                if (order.OrderStatus == "Pending" || order.OrderStatus == "Cooking")
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Gets daily sales data for chart
        /// </summary>
        private List<DailySalesData> GetDailySalesData(int branchID, int days)
        {
            List<DailySalesData> data = new List<DailySalesData>();

            for (int i = days - 1; i >= 0; i--)
            {
                DateTime date = DateTime.Now.AddDays(-i);
                var orders = orderBAL.GetOrdersByBranch(branchID, null, date.Date, date.Date.AddDays(1));

                DailySalesData dayData = new DailySalesData
                {
                    Date = date.ToString("MMM dd"),
                    Sales = 0,
                    Orders = 0
                };

                foreach (var order in orders)
                {
                    if (order.OrderStatus != "Cancelled" && order.OrderStatus != "Void")
                    {
                        dayData.Sales += order.TotalAmount;
                        dayData.Orders++;
                    }
                }

                data.Add(dayData);
            }

            return data;
        }
        // ============================================
        // BAL/DashboardBAL.cs - Add Deal Dashboard Methods
        // ============================================

        /// <summary>
        /// Gets top selling deals for dashboard
        /// </summary>
        public List<TopSellingDeal> GetTopSellingDeals(int branchID, int topCount = 5)
        {
            try
            {
                List<TopSellingDeal> deals = new List<TopSellingDeal>();

                // Get data from stored procedure
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@DateFrom", DateTime.Now.AddDays(-30)),
            dbHelper.CreateParameter("@DateTo", DateTime.Now),
            dbHelper.CreateParameter("@TopCount", topCount)
        };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetTopSellingDeals", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    deals.Add(new TopSellingDeal
                    {
                        DealID = Convert.ToInt32(row["DealID"]),
                        DealName = row["DealName"].ToString(),
                        OrderCount = Convert.ToInt32(row["OrderCount"]),
                        TotalRevenue = Convert.ToDecimal(row["TotalRevenue"]),
                        AverageOrderValue = Convert.ToDecimal(row["AverageOrderValue"])
                    });
                }

                return deals;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting top selling deals: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets deal revenue for dashboard
        /// </summary>
        public decimal GetDealRevenue(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@DateFrom", dateFrom),
            dbHelper.CreateParameter("@DateTo", dateTo)
        };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetDealSalesReport", parameters);

                decimal total = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    total += Convert.ToDecimal(row["TotalRevenue"]);
                }

                return total;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets deals sold today count
        /// </summary>
        public int GetDealsSoldToday(int branchID)
        {
            try
            {
                var deals = GetTopSellingDeals(branchID, 100);
                int count = 0;
                foreach (var deal in deals)
                {
                    // Check if deal was sold today
                    // This would need a separate query
                }
                return count;
            }
            catch
            {
                return 0;
            }
        }
    }
}