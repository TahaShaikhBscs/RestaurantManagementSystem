// ============================================
// UI/Kitchen/KitchenDisplay.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using KitchenOrderModel = RestaurantManagementSystem.Models.KitchenOrder;

namespace RestaurantManagementSystem.UI.Kitchen
{
    /// <summary>
    /// Kitchen Display Page - Shows kitchen orders with status management
    /// </summary>
    public partial class KitchenDisplay : Page
    {
        private OrderBAL orderBAL;
        private AuditLogger auditLogger;
        private bool autoRefresh = false;

        /// <summary>
        /// Page load event - loads kitchen orders
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
                LoadKitchenOrders();
            }
        }

        /// <summary>
        /// Loads kitchen orders into the display
        /// </summary>
        private void LoadKitchenOrders()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                // Get pending orders
                List<KitchenOrderModel> pending = orderBAL.GetKitchenOrders(branchID.Value, "Pending");
                rptPendingOrders.DataSource = pending;
                rptPendingOrders.DataBind();
                ltrPendingCount.Text = pending.Count.ToString();
                ltrPendingBadge.Text = pending.Count.ToString();

                // Get cooking orders
                List<KitchenOrderModel> cooking = orderBAL.GetKitchenOrders(branchID.Value, "Cooking");
                rptCookingOrders.DataSource = cooking;
                rptCookingOrders.DataBind();
                ltrCookingCount.Text = cooking.Count.ToString();
                ltrCookingBadge.Text = cooking.Count.ToString();

                // Get ready orders
                List<KitchenOrderModel> ready = orderBAL.GetKitchenOrders(branchID.Value, "Ready");
                rptReadyOrders.DataSource = ready;
                rptReadyOrders.DataBind();
                ltrReadyCount.Text = ready.Count.ToString();
                ltrReadyBadge.Text = ready.Count.ToString();

                // Count delayed orders
                int delayed = 0;
                foreach (var order in pending)
                {
                    if (order.PriorityStatus == "Delayed")
                        delayed++;
                }
                foreach (var order in cooking)
                {
                    if (order.PriorityStatus == "Delayed")
                        delayed++;
                }
                ltrDelayedCount.Text = delayed.ToString();

                auditLogger.LogAction("ViewKitchen", "Kitchen", "KitchenDisplay", null, null, "Kitchen display loaded");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading kitchen orders: {ex.Message}");
                auditLogger.LogAction("Error", "Kitchen", "LoadOrders", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Handles kitchen order commands
        /// </summary>
        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                int kitchenOrderID = Convert.ToInt32(e.CommandArgument);
                int? userID = SessionHelper.GetUserId();

                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                if (e.CommandName == "StartCooking")
                {
                    // Start cooking - update kitchen status
                    bool updated = orderBAL.UpdateKitchenStatus(kitchenOrderID, "Cooking", userID.Value);
                    if (updated)
                    {
                        auditLogger.LogUpdate("Kitchen", "KitchenOrders", kitchenOrderID,
                            $"KitchenOrderID: {kitchenOrderID}", "Status: Cooking");
                        ShowSuccessMessage("Order started cooking.");
                        LoadKitchenOrders();
                    }
                }
                else if (e.CommandName == "MarkReady")
                {
                    // Mark as ready
                    bool updated = orderBAL.UpdateKitchenStatus(kitchenOrderID, "Ready", userID.Value);
                    if (updated)
                    {
                        auditLogger.LogUpdate("Kitchen", "KitchenOrders", kitchenOrderID,
                            $"KitchenOrderID: {kitchenOrderID}", "Status: Ready");
                        ShowSuccessMessage("Order marked as ready.");
                        LoadKitchenOrders();
                    }
                }
                else if (e.CommandName == "MarkServed")
                {
                    // Mark as served
                    bool updated = orderBAL.UpdateKitchenStatus(kitchenOrderID, "Served", userID.Value);
                    if (updated)
                    {
                        auditLogger.LogUpdate("Kitchen", "KitchenOrders", kitchenOrderID,
                            $"KitchenOrderID: {kitchenOrderID}", "Status: Served");
                        ShowSuccessMessage("Order marked as served.");
                        LoadKitchenOrders();
                    }
                }
                else if (e.CommandName == "CancelOrder")
                {
                    // Cancel order
                    // In a real implementation, you would cancel the specific kitchen order
                    // For now, we'll update status to Cancelled
                    bool updated = orderBAL.UpdateKitchenStatus(kitchenOrderID, "Cancelled", userID.Value);
                    if (updated)
                    {
                        auditLogger.LogUpdate("Kitchen", "KitchenOrders", kitchenOrderID,
                            $"KitchenOrderID: {kitchenOrderID}", "Status: Cancelled");
                        ShowSuccessMessage("Order cancelled.");
                        LoadKitchenOrders();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error updating order: {ex.Message}");
                auditLogger.LogAction("Error", "Kitchen", "UpdateOrder", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Gets priority badge class
        /// </summary>
        protected string GetPriorityBadgeClass(string priority)
        {
            switch (priority)
            {
                case "Urgent": return "danger";
                case "Delayed": return "danger";
                case "Normal": return "secondary";
                default: return "secondary";
            }
        }

        /// <summary>
        /// Refreshes the kitchen display
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadKitchenOrders();
            ShowSuccessMessage("Kitchen display refreshed.");
        }

        /// <summary>
        /// Toggles auto-refresh
        /// </summary>
        protected void btnAutoRefresh_Click(object sender, EventArgs e)
        {
            autoRefresh = !autoRefresh;
            btnAutoRefresh.Text = autoRefresh ? "Auto Refresh On" : "Auto Refresh";
            btnAutoRefresh.CssClass = autoRefresh ? "btn btn-success" : "btn btn-secondary";

            if (autoRefresh)
            {
                ShowSuccessMessage("Auto-refresh enabled (30 seconds).");
            }
            else
            {
                ShowSuccessMessage("Auto-refresh disabled.");
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