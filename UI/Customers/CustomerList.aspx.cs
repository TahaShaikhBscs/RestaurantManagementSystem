// ============================================
// UI/Customers/CustomerList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using CustomerModel = RestaurantManagementSystem.Models.Customer;

namespace RestaurantManagementSystem.UI.Customers
{
    /// <summary>
    /// Customer List Page - Displays all customers with management options
    /// </summary>
    public partial class CustomerList : Page
    {
        private CustomerBAL customerBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads customers
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            customerBAL = new CustomerBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadCustomers();
            }
        }

        /// <summary>
        /// Loads customers into the grid view with statistics
        /// </summary>
        private void LoadCustomers()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                string customerType = ddlCustomerType.SelectedValue;
                List<CustomerModel> customers = customerBAL.GetCustomersByBranch(branchID.Value);

                // Apply type filter
                if (!string.IsNullOrEmpty(customerType))
                {
                    customers = customers.FindAll(c => c.CustomerType == customerType);
                }

                gvCustomers.DataSource = customers;
                gvCustomers.DataBind();

                // Update statistics
                UpdateStatistics(customers);
                ltrTotalCount.Text = customers.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading customers: {ex.Message}");
                auditLogger.LogAction("Error", "CustomerManagement", "Customers", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates customer statistics
        /// </summary>
        private void UpdateStatistics(List<CustomerModel> customers)
        {
            int total = customers.Count;
            int active = 0, vip = 0, totalPoints = 0;

            foreach (var customer in customers)
            {
                if (customer.Status) active++;
                if (customer.CustomerType == "VIP") vip++;
                totalPoints += customer.LoyaltyPoints;
            }

            ltrTotalCustomers.Text = total.ToString();
            ltrActiveCustomers.Text = active.ToString();
            ltrVIPCustomers.Text = vip.ToString();
            ltrTotalPoints.Text = totalPoints.ToString("N0");
        }

        /// <summary>
        /// Handles customer type filter change
        /// </summary>
        protected void ddlCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        /// <summary>
        /// Searches customers
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                int? branchID = SessionHelper.GetBranchId();

                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    LoadCustomers();
                }
                else
                {
                    List<CustomerModel> customers = customerBAL.SearchCustomers(branchID.Value, searchTerm);
                    gvCustomers.DataSource = customers;
                    gvCustomers.DataBind();
                    UpdateStatistics(customers);
                    ltrTotalCount.Text = customers.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching customers: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears search and reloads all customers
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCustomerType.SelectedValue = "";
            LoadCustomers();
        }

        /// <summary>
        /// Redirects to add new customer page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int customerID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditCustomer")
                {
                    Response.Redirect($"CustomerAddEdit.aspx?CustomerID={customerID}");
                }
                else if (e.CommandName == "ViewHistory")
                {
                    Response.Redirect($"CustomerHistory.aspx?CustomerID={customerID}");
                }
                else if (e.CommandName == "DeleteCustomer")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool deleted = customerBAL.DeleteCustomer(customerID, userID.Value);
                    if (deleted)
                    {
                        auditLogger.LogDelete("CustomerManagement", "Customers", customerID, $"CustomerID: {customerID}");
                        ShowSuccessMessage("Customer deleted successfully.");
                        LoadCustomers();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to delete customer.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles grid view page index changing
        /// </summary>
        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            LoadCustomers();
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