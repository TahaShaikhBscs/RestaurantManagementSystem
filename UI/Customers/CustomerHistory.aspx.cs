// ============================================
// UI/Customers/CustomerHistory.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using CustomerModel = RestaurantManagementSystem.Models.Customer;
using OrderModel = RestaurantManagementSystem.Models.Order;

namespace RestaurantManagementSystem.UI.Customers
{
    /// <summary>
    /// Customer History Page - Displays customer details and order history
    /// </summary>
    public partial class CustomerHistory : Page
    {
        private CustomerBAL customerBAL;
        private OrderBAL orderBAL;
        private AuditLogger auditLogger;
        private int customerID = 0;

        /// <summary>
        /// Page load event - loads customer history
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            customerBAL = new CustomerBAL();
            orderBAL = new OrderBAL();
            auditLogger = new AuditLogger();

            if (Request.QueryString["CustomerID"] != null)
            {
                customerID = Convert.ToInt32(Request.QueryString["CustomerID"]);
            }

            if (!IsPostBack && customerID > 0)
            {
                LoadCustomerInfo(customerID);
                LoadOrderHistory(customerID);
            }
        }

        /// <summary>
        /// Loads customer information
        /// </summary>
        private void LoadCustomerInfo(int id)
        {
            try
            {
                CustomerModel customer = customerBAL.GetCustomerById(id);
                if (customer != null)
                {
                    ltrCustomerName.Text = customer.CustomerName;
                    ltrName.Text = customer.CustomerName;
                    ltrPhone.Text = customer.Phone ?? "-";
                    ltrEmail.Text = customer.Email ?? "-";
                    ltrPoints.Text = customer.LoyaltyPoints.ToString();
                }
                else
                {
                    ShowErrorMessage("Customer not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading customer: {ex.Message}");
                auditLogger.LogAction("Error", "CustomerManagement", "CustomerHistory", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Loads customer order history
        /// </summary>
        private void LoadOrderHistory(int id)
        {
            try
            {
                // In a real implementation, you would have a method to get orders by customer
                // For now, we'll use a placeholder
                List<OrderModel> orders = new List<OrderModel>();
                gvOrders.DataSource = orders;
                gvOrders.DataBind();
                ltrOrderCount.Text = orders.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading order history: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets Bootstrap badge class for status
        /// </summary>
        protected string GetStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Pending": return "warning";
                case "Cooking": return "info";
                case "Ready": return "primary";
                case "Completed": return "success";
                case "Cancelled": return "danger";
                default: return "secondary";
            }
        }

        /// <summary>
        /// Handles view order click
        /// </summary>
        protected void btnViewOrder_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int orderID = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Orders/OrderDetail.aspx?OrderID={orderID}");
        }

        /// <summary>
        /// Redirects back to customer list
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerList.aspx");
        }

        /// <summary>
        /// Handles grid view page index changing
        /// </summary>
        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            LoadOrderHistory(customerID);
        }

        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            ltrSuccessMessage.Text = message;
        }

        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            ltrErrorMessage.Text = message;
        }
    }
}