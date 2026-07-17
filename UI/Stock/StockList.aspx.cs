// ============================================
// UI/Stock/StockList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using StockModel = RestaurantManagementSystem.Models.Stock;

namespace RestaurantManagementSystem.UI.Stock
{
    /// <summary>
    /// Stock List Page - Displays all stock items with management options
    /// </summary>
    public partial class StockList : Page
    {
        private StockBAL stockBAL;
        private AuditLogger auditLogger;

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

            if (!IsPostBack)
            {
                LoadStockItems();
            }
        }

        /// <summary>
        /// Loads stock items into the grid view with statistics
        /// </summary>
        private void LoadStockItems()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                string category = ddlCategory.SelectedValue;
                if (string.IsNullOrEmpty(category))
                {
                    category = null;
                }

                List<StockModel> stock = stockBAL.GetStockByBranch(branchID.Value, category, true);

                // Apply stock status filter
                if (ddlStockStatus.SelectedValue == "Low")
                {
                    stock = stock.FindAll(s => s.Quantity <= s.MinStockLevel);
                }
                else if (ddlStockStatus.SelectedValue == "Over")
                {
                    stock = stock.FindAll(s => s.Quantity >= s.MaxStockLevel && s.MaxStockLevel > 0);
                }
                else if (ddlStockStatus.SelectedValue == "Normal")
                {
                    stock = stock.FindAll(s => s.Quantity > s.MinStockLevel && s.Quantity < s.MaxStockLevel);
                }

                gvStock.DataSource = stock;
                gvStock.DataBind();

                // Update statistics
                UpdateStatistics(stock);
                ltrTotalCount.Text = stock.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading stock: {ex.Message}");
                auditLogger.LogAction("Error", "StockManagement", "Stock", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates stock statistics
        /// </summary>
        private void UpdateStatistics(List<StockModel> stock)
        {
            int total = stock.Count;
            int lowStock = 0, normalStock = 0;
            decimal totalValue = 0;

            foreach (var item in stock)
            {
                totalValue += item.TotalCost;

                if (item.Quantity <= item.MinStockLevel)
                {
                    lowStock++;
                }
                else
                {
                    normalStock++;
                }
            }

            ltrTotalItems.Text = total.ToString();
            ltrLowStock.Text = lowStock.ToString();
            ltrNormalStock.Text = normalStock.ToString();
            ltrTotalValue.Text = totalValue.ToString("N2");
        }

        /// <summary>
        /// Handles category filter change
        /// </summary>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockItems();
        }

        /// <summary>
        /// Handles stock status filter change
        /// </summary>
        protected void ddlStockStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockItems();
        }

        /// <summary>
        /// Searches stock items
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
                    LoadStockItems();
                }
                else
                {
                    List<StockModel> stock = stockBAL.SearchStock(branchID.Value, searchTerm);
                    gvStock.DataSource = stock;
                    gvStock.DataBind();
                    UpdateStatistics(stock);
                    ltrTotalCount.Text = stock.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching stock: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears search and reloads all stock
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCategory.SelectedValue = "";
            ddlStockStatus.SelectedValue = "";
            LoadStockItems();
        }

        /// <summary>
        /// Redirects to add new stock page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockAddEdit.aspx");
        }

        /// <summary>
        /// Redirects to stock adjustment page
        /// </summary>
        protected void btnAdjustment_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockAdjustment.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int stockID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditStock")
                {
                    Response.Redirect($"StockAddEdit.aspx?StockID={stockID}");
                }
                else if (e.CommandName == "AdjustStock")
                {
                    Response.Redirect($"StockAdjustment.aspx?StockID={stockID}");
                }
                else if (e.CommandName == "DeleteStock")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool deleted = stockBAL.DeleteStock(stockID, userID.Value);
                    if (deleted)
                    {
                        auditLogger.LogDelete("StockManagement", "Stock", stockID, $"StockID: {stockID}");
                        ShowSuccessMessage("Stock item deleted successfully.");
                        LoadStockItems();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to delete stock item.");
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
        protected void gvStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStock.PageIndex = e.NewPageIndex;
            LoadStockItems();
        }

        /// <summary>
        /// Gets stock status badge class
        /// </summary>
        protected string GetStockStatusBadgeClass(object quantity, object minLevel, object maxLevel)
        {
            decimal qty = Convert.ToDecimal(quantity);
            decimal min = Convert.ToDecimal(minLevel);
            decimal max = Convert.ToDecimal(maxLevel);

            if (qty <= min) return "danger";
            if (max > 0 && qty >= max) return "warning";
            return "success";
        }

        /// <summary>
        /// Gets stock status icon
        /// </summary>
        protected string GetStockStatusIcon(object quantity, object minLevel, object maxLevel)
        {
            decimal qty = Convert.ToDecimal(quantity);
            decimal min = Convert.ToDecimal(minLevel);
            decimal max = Convert.ToDecimal(maxLevel);

            if (qty <= min) return "exclamation-triangle";
            if (max > 0 && qty >= max) return "arrow-up";
            return "check-circle";
        }

        /// <summary>
        /// Gets stock status text
        /// </summary>
        protected string GetStockStatusText(object quantity, object minLevel, object maxLevel)
        {
            decimal qty = Convert.ToDecimal(quantity);
            decimal min = Convert.ToDecimal(minLevel);
            decimal max = Convert.ToDecimal(maxLevel);

            if (qty <= min) return "Low Stock";
            if (max > 0 && qty >= max) return "Over Stocked";
            return "Normal";
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