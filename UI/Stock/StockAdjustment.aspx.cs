// ============================================
// UI/Stock/StockAdjustment.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using StockModel = RestaurantManagementSystem.Models.Stock;

namespace RestaurantManagementSystem.UI.Stock
{
    /// <summary>
    /// Stock Adjustment Page - Handles stock quantity adjustments
    /// </summary>
    public partial class StockAdjustment : Page
    {
        private StockBAL stockBAL;
        private AuditLogger auditLogger;
        private int stockID = 0;

        /// <summary>
        /// Page load event - loads stock items
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            stockBAL = new StockBAL();
            auditLogger = new AuditLogger();

            if (Request.QueryString["StockID"] != null)
            {
                stockID = Convert.ToInt32(Request.QueryString["StockID"]);
            }

            if (!IsPostBack)
            {
                LoadStockItems();

                if (stockID > 0)
                {
                    ddlStockItem.SelectedValue = stockID.ToString();
                    LoadStockDetails(stockID);
                }
            }
        }

        /// <summary>
        /// Loads stock items into the dropdown
        /// </summary>
        private void LoadStockItems()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<StockModel> stock = stockBAL.GetStockByBranch(branchID.Value);
                    ddlStockItem.DataSource = stock;
                    ddlStockItem.DataTextField = "ProductName";
                    ddlStockItem.DataValueField = "StockID";
                    ddlStockItem.DataBind();
                    ddlStockItem.Items.Insert(0, new ListItem("-- Select Item --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading stock items: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads stock details when an item is selected
        /// </summary>
        protected void ddlStockItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlStockItem.SelectedValue))
            {
                LoadStockDetails(Convert.ToInt32(ddlStockItem.SelectedValue));
            }
        }

        /// <summary>
        /// Loads stock details for the selected item
        /// </summary>
        private void LoadStockDetails(int id)
        {
            try
            {
                StockModel stock = stockBAL.GetStockById(id);
                if (stock != null)
                {
                    txtCurrentQuantity.Text = stock.Quantity.ToString();
                    txtNewQuantity.Text = stock.Quantity.ToString();
                    ltrCurrentQtyDisplay.Text = stock.Quantity.ToString();
                    ltrNewQtyDisplay.Text = stock.Quantity.ToString();
                    txtAdjustmentQty.Text = "0";
                    divComparison.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading stock details: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies the stock adjustment
        /// </summary>
        protected void btnAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                int stockID = Convert.ToInt32(ddlStockItem.SelectedValue);
                string adjustmentType = ddlAdjustmentType.SelectedValue;
                decimal adjustmentQty = Convert.ToDecimal(txtAdjustmentQty.Text);
                string reason = txtReason.Text.Trim();

                if (string.IsNullOrEmpty(reason))
                {
                    ShowErrorMessage("Reason is required.");
                    return;
                }

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                StockModel stock = stockBAL.GetStockById(stockID);
                if (stock == null)
                {
                    ShowErrorMessage("Stock item not found.");
                    return;
                }

                decimal newQuantity = stock.Quantity;

                switch (adjustmentType)
                {
                    case "Add":
                        newQuantity = stock.Quantity + adjustmentQty;
                        break;
                    case "Remove":
                        if (adjustmentQty > stock.Quantity)
                        {
                            ShowErrorMessage("Cannot remove more than current quantity.");
                            return;
                        }
                        newQuantity = stock.Quantity - adjustmentQty;
                        break;
                    case "Set":
                        newQuantity = adjustmentQty;
                        break;
                }

                if (newQuantity < 0)
                {
                    ShowErrorMessage("New quantity cannot be negative.");
                    return;
                }

                // Update stock quantity
                bool updated = stockBAL.UpdateStockQuantity(stockID, newQuantity, userID.Value);
                if (updated)
                {
                    auditLogger.LogUpdate("StockManagement", "Stock", stockID,
                        $"Old Qty: {stock.Quantity}, New Qty: {newQuantity}",
                        $"Adjustment Type: {adjustmentType}, Reason: {reason}");

                    ShowSuccessMessage($"Stock adjusted successfully. New quantity: {newQuantity}");

                    // Reset form
                    LoadStockItems();
                    if (stockID > 0)
                    {
                        ddlStockItem.SelectedValue = stockID.ToString();
                        LoadStockDetails(stockID);
                    }
                    txtReason.Text = string.Empty;
                    txtAdjustmentQty.Text = "0";
                }
                else
                {
                    ShowErrorMessage("Failed to update stock quantity.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error applying adjustment: {ex.Message}");
                auditLogger.LogAction("Error", "StockManagement", "StockAdjustment", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockList.aspx");
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