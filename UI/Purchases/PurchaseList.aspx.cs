// ============================================
// UI/Purchases/PurchaseList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using PurchaseOrderModel = RestaurantManagementSystem.Models.PurchaseOrder;

namespace RestaurantManagementSystem.UI.Purchases
{
    /// <summary>
    /// Purchase List Page - Displays all purchase orders with management options
    /// </summary>
    public partial class PurchaseList : Page
    {
        private PurchaseBAL purchaseBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads purchase orders
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            purchaseBAL = new PurchaseBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadPurchaseOrders();
            }
        }

        /// <summary>
        /// Loads purchase orders into the grid view with statistics
        /// </summary>
        private void LoadPurchaseOrders()
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
                DateTime? dateFrom = null;
                if (!string.IsNullOrEmpty(txtDateFrom.Text))
                {
                    dateFrom = Convert.ToDateTime(txtDateFrom.Text);
                }

                List<PurchaseOrderModel> orders = purchaseBAL.GetPurchaseOrdersByBranch(
                    branchID.Value,
                    string.IsNullOrEmpty(statusFilter) ? null : statusFilter,
                    dateFrom, null);

                gvPurchaseOrders.DataSource = orders;
                gvPurchaseOrders.DataBind();

                // Update statistics
                UpdateStatistics(orders);
                ltrTotalCount.Text = orders.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading purchase orders: {ex.Message}");
                auditLogger.LogAction("Error", "PurchaseManagement", "PurchaseOrders", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates purchase order statistics
        /// </summary>
        private void UpdateStatistics(List<PurchaseOrderModel> orders)
        {
            int total = orders.Count;
            int pending = 0, received = 0, cancelled = 0;

            foreach (var order in orders)
            {
                switch (order.OrderStatus)
                {
                    case "Draft":
                    case "Sent": pending++; break;
                    case "Received": received++; break;
                    case "Cancelled": cancelled++; break;
                }
            }

            ltrTotalOrders.Text = total.ToString();
            ltrPendingOrders.Text = pending.ToString();
            ltrReceivedOrders.Text = received.ToString();
            ltrCancelledOrders.Text = cancelled.ToString();
        }

        /// <summary>
        /// Handles status filter change
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPurchaseOrders();
        }

        /// <summary>
        /// Searches purchase orders
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPurchaseOrders();
        }

        /// <summary>
        /// Clears search and reloads all purchase orders
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlStatusFilter.SelectedValue = "";
            txtDateFrom.Text = string.Empty;
            LoadPurchaseOrders();
        }

        /// <summary>
        /// Redirects to add new purchase order page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("PurchaseAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvPurchaseOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int orderID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "ViewOrder")
                {
                    Response.Redirect($"PurchaseDetail.aspx?PurchaseOrderID={orderID}");
                }
                else if (e.CommandName == "EditOrder")
                {
                    Response.Redirect($"PurchaseAddEdit.aspx?PurchaseOrderID={orderID}");
                }
                else if (e.CommandName == "ReceiveOrder")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool received = purchaseBAL.ReceivePurchaseOrder(orderID, userID.Value);
                    if (received)
                    {
                        auditLogger.LogUpdate("PurchaseManagement", "PurchaseOrders", orderID,
                            $"PurchaseOrderID: {orderID}", "Status: Received");
                        ShowSuccessMessage("Purchase order received successfully. Inventory updated.");
                        LoadPurchaseOrders();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to receive purchase order.");
                    }
                }
                else if (e.CommandName == "DeleteOrder")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    // In a real implementation, you would have a delete method
                    // For now, we'll update status to cancelled
                    bool deleted = purchaseBAL.UpdatePurchaseOrderStatus(orderID, "Cancelled", userID.Value);
                    if (deleted)
                    {
                        auditLogger.LogDelete("PurchaseManagement", "PurchaseOrders", orderID, $"PurchaseOrderID: {orderID}");
                        ShowSuccessMessage("Purchase order cancelled.");
                        LoadPurchaseOrders();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to cancel purchase order.");
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
        protected void gvPurchaseOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPurchaseOrders.PageIndex = e.NewPageIndex;
            LoadPurchaseOrders();
        }

        /// <summary>
        /// Gets Bootstrap badge class for status
        /// </summary>
        protected string GetStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Draft": return "secondary";
                case "Sent": return "warning";
                case "Received": return "success";
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
                case "Draft": return "file";
                case "Sent": return "paper-plane";
                case "Received": return "check-circle";
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
                case "Paid": return "success";
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