// ============================================
// UI/Dashboard.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.BAL;
using System.Linq;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Dashboard Page Code-Behind
    /// Displays key metrics and analytics for the restaurant
    /// </summary>
    public partial class Dashboard : Page
    {
        private DashboardBAL dashboardBAL;
        private OrderBAL orderBAL;
        private CustomerBAL customerBAL;
        private MenuBAL menuBAL;
        private ExpenseBAL expenseBAL;

        /// <summary>
        /// Page load event - loads dashboard data
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Initialize BAL objects
            dashboardBAL = new DashboardBAL();
            orderBAL = new OrderBAL();
            customerBAL = new CustomerBAL();
            menuBAL = new MenuBAL();
            expenseBAL = new ExpenseBAL();

            if (!IsPostBack)
            {
                // Get branch ID from session
                int branchID = Convert.ToInt32(Session["BranchID"]);

                // Load dashboard data
                LoadDashboardData(branchID);

                // Display user name
                User user = Session["User"] as User;
                if (user != null)
                {
                    ltrUserName.Text = user.FullName;
                }
            }
        }

        /// <summary>
        /// Loads all dashboard data
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        private void LoadDashboardData(int branchID)
        {
            try
            {
                // Get dashboard data
                DashboardData dashboard = dashboardBAL.GetDashboardData(branchID);

                // Set statistics
                ltrTodaySales.Text = dashboard.TodaySales.ToString("N2");
                ltrMonthlySales.Text = dashboard.MonthlySales.ToString("N2");
                ltrTotalOrders.Text = dashboard.TotalOrders.ToString();
                ltrRunningOrders.Text = dashboard.RunningOrders.ToString();
                ltrKitchenOrders.Text = dashboard.KitchenOrders.ToString();
                ltrTotalCustomers.Text = dashboard.TotalCustomers.ToString();
                ltrTotalProducts.Text = dashboard.TotalProducts.ToString();
                ltrTotalExpenses.Text = dashboard.TotalExpenses.ToString("N2");

                // Bind top selling items
                rptTopSellingItems.DataSource = dashboard.TopSellingItems;
                rptTopSellingItems.DataBind();

                // Bind recent orders
                rptRecentOrders.DataSource = dashboard.RecentOrders;
                rptRecentOrders.DataBind();

                // Bind recent payments
                rptRecentPayments.DataSource = dashboard.RecentPayments;
                rptRecentPayments.DataBind();
            }
            catch (Exception ex)
            {
                // Show error message
                System.Diagnostics.Debug.WriteLine($"Dashboard Error: {ex.Message}");
                // Could use a user control for error messages
            }
        }

        /// <summary>
        /// Gets chart labels (dates for the last 7 days)
        /// </summary>
        /// <returns>JSON array of labels</returns>
        public string GetChartLabels()
        {
            List<string> labels = new List<string>();
            for (int i = 6; i >= 0; i--)
            {
                labels.Add(DateTime.Now.AddDays(-i).ToString("MMM dd"));
            }
            return $"[{string.Join(",", labels.Select(l => $"'{l}'"))}]";
        }

        /// <summary>
        /// Gets chart data (sales for the last 7 days)
        /// </summary>
        /// <returns>JSON array of data</returns>
        public string GetChartData()
        {
            // This would normally come from the database
            // For demonstration, we'll generate sample data
            Random random = new Random();
            List<decimal> data = new List<decimal>();
            for (int i = 0; i < 7; i++)
            {
                data.Add((decimal)(random.Next(500, 2000) + random.NextDouble()));
            }
            return $"[{string.Join(",", data.Select(d => d.ToString("N2")))}]";
        }

        /// <summary>
        /// Gets the Bootstrap badge color for an order status
        /// </summary>
        /// <param name="status">Order status</param>
        /// <returns>Bootstrap badge class</returns>
        protected string GetOrderStatusBadgeClass(string status)
        {
            switch (status.ToLower())
            {
                case "pending": return "warning";
                case "cooking": return "info";
                case "ready": return "success";
                case "completed": return "success";
                case "cancelled": return "danger";
                case "void": return "secondary";
                default: return "secondary";
            }
        }
    }
}