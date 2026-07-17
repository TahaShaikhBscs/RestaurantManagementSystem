// ============================================
// UI/Orders/OrderList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using OrderModel = RestaurantManagementSystem.Models.Order;

namespace RestaurantManagementSystem.UI.Orders
{
    /// <summary>
    /// Order List Page - Displays all orders with management options
    /// </summary>
    public partial class OrderList : Page
    {
        private OrderBAL orderBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads orders
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            orderBAL = new OrderBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        /// <summary>
        /// Loads orders into the grid view with statistics
        /// </summary>
        private void LoadOrders()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                string statusFilter = ddlStatusFilter.SelectedValue;
                string typeFilter = ddlTypeFilter.SelectedValue;

                List<OrderModel> orders = orderBAL.GetOrdersByBranch(
                    branchID.Value,
                    string.IsNullOrEmpty(statusFilter) ? null : statusFilter,
                    null, null,
                    string.IsNullOrEmpty(typeFilter) ? null : typeFilter);

                gvOrders.DataSource = orders;
                gvOrders.DataBind();

                // Update statistics
                UpdateStatistics(orders);
                ltrTotalCount.Text = orders.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading orders: {ex.Message}");
                auditLogger.LogAction("Error", "OrderManagement", "Orders", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates order statistics
        /// </summary>
        private void UpdateStatistics(List<OrderModel> orders)
        {
            int total = orders.Count;
            int pending = 0, cooking = 0, completed = 0;

            foreach (var order in orders)
            {
                switch (order.OrderStatus)
                {
                    case "Pending": pending++; break;
                    case "Cooking": cooking++; break;
                    case "Completed": completed++; break;
                }
            }

            ltrTotalOrders.Text = total.ToString();
            ltrPendingOrders.Text = pending.ToString();
            ltrCookingOrders.Text = cooking.ToString();
            ltrCompletedOrders.Text = completed.ToString();
        }

        /// <summary>
        /// Handles status filter change
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrders();
        }

        /// <summary>
        /// Handles type filter change
        /// </summary>
        protected void ddlTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrders();
        }

        /// <summary>
        /// Searches orders
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    int? branchID = SessionHelper.GetBranchId();
                    if (branchID.HasValue)
                    {
                        List<OrderModel> orders = orderBAL.SearchOrders(branchID.Value, searchTerm);
                        gvOrders.DataSource = orders;
                        gvOrders.DataBind();
                        UpdateStatistics(orders);
                        ltrTotalCount.Text = orders.Count.ToString();
                        return;
                    }
                }
                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching orders: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears search and reloads all orders
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlStatusFilter.SelectedValue = "";
            ddlTypeFilter.SelectedValue = "";
            LoadOrders();
        }

        /// <summary>
        /// Refreshes the order list
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
            ShowSuccessMessage("Orders refreshed successfully.");
        }

        /// <summary>
        /// Redirects to new order page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int orderID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "ViewOrder")
                {
                    Response.Redirect($"OrderDetail.aspx?OrderID={orderID}");
                }
                else if (e.CommandName == "UpdateStatus")
                {
                    // Open modal for status update
                    string script = $"openStatusModal({orderID});";
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", script, true);
                }
                else if (e.CommandName == "CancelOrder")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool cancelled = orderBAL.CancelOrder(orderID, "Cancelled by user", userID.Value);
                    if (cancelled)
                    {
                        auditLogger.LogUpdate("OrderManagement", "Orders", orderID,
                            $"OrderID: {orderID}", "Status: Cancelled");
                        ShowSuccessMessage("Order cancelled successfully.");
                        LoadOrders();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to cancel order.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates order status from modal
        /// </summary>
        protected void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            try
            {
                int orderID = Convert.ToInt32(hfOrderID.Value);
                string newStatus = ddlNewStatus.SelectedValue;
                string notes = txtStatusNotes.Text.Trim();

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                bool updated = orderBAL.UpdateOrderStatus(orderID, newStatus, notes, userID.Value);
                if (updated)
                {
                    auditLogger.LogUpdate("OrderManagement", "Orders", orderID,
                        $"OrderID: {orderID}", $"Status: {newStatus}");
                    ShowSuccessMessage($"Order status updated to {newStatus}.");
                    LoadOrders();
                }
                else
                {
                    ShowErrorMessage("Failed to update order status.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error updating status: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles grid view page index changing
        /// </summary>
        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            LoadOrders();
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
        /// Gets Font Awesome icon class for status
        /// </summary>
        protected string GetStatusIconClass(string status)
        {
            switch (status)
            {
                case "Pending": return "clock";
                case "Cooking": return "utensils";
                case "Ready": return "check-circle";
                case "Completed": return "check-double";
                case "Cancelled": return "times-circle";
                default: return "question-circle";
            }
        }

        /// <summary>
        /// Gets Bootstrap badge class for payment status
        /// </summary>
        protected string GetPaymentStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Completed": return "success";
                case "Partial": return "warning";
                case "Pending": return "danger";
                default: return "secondary";
            }
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