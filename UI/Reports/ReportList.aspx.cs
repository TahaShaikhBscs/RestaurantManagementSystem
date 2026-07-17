// ============================================
// UI/Reports/ReportList.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Reports
{
    /// <summary>
    /// Reports Dashboard Page - Central hub for all reports
    /// </summary>
    public partial class ReportList : Page
    {
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - initializes reports dashboard
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                auditLogger.LogAction("View", "Reports", "ReportList", null, null, "Reports dashboard viewed");
            }
        }

        /// <summary>
        /// Navigates to Sales Report
        /// </summary>
        protected void btnSalesReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReport.aspx");
        }

        /// <summary>
        /// Navigates to Profit & Loss Report
        /// </summary>
        protected void btnProfitLoss_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProfitLoss.aspx");
        }

        /// <summary>
        /// Navigates to Expense Report
        /// </summary>
        protected void btnExpenseReport_Click(object sender, EventArgs e)
        {
            // In a real implementation, you would have an ExpenseReport.aspx
            Response.Redirect("ExpenseReport.aspx");
        }

        /// <summary>
        /// Navigates to Customer Report
        /// </summary>
        protected void btnCustomerReport_Click(object sender, EventArgs e)
        {
            // In a real implementation, you would have a CustomerReport.aspx
            Response.Redirect("CustomerReport.aspx");
        }

        /// <summary>
        /// Navigates to Inventory Report
        /// </summary>
        protected void btnInventoryReport_Click(object sender, EventArgs e)
        {
            // In a real implementation, you would have an InventoryReport.aspx
            Response.Redirect("InventoryReport.aspx");
        }

        /// <summary>
        /// Navigates to Payment Report
        /// </summary>
        protected void btnPaymentReport_Click(object sender, EventArgs e)
        {
            // In a real implementation, you would have a PaymentReport.aspx
            Response.Redirect("PaymentReport.aspx");
        }
    }
}