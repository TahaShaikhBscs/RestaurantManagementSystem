// ============================================
// UI/Stock/StockAddEdit.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using StockModel = RestaurantManagementSystem.Models.Stock;

namespace RestaurantManagementSystem.UI.Stock
{
    /// <summary>
    /// Stock Add/Edit Page - Handles adding and editing stock items
    /// </summary>
    public partial class StockAddEdit : Page
    {
        private StockBAL stockBAL;
        private AuditLogger auditLogger;
        private int stockID = 0;

        /// <summary>
        /// Page load event - loads stock data for editing
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
                if (stockID > 0)
                {
                    ltrPageTitle.Text = "Edit Stock Item";
                    ltrBreadcrumb.Text = "Edit";
                    ltrFormTitle.Text = "Edit Stock Item";
                    btnSave.Text = "Update Stock Item";
                    LoadStockData(stockID);
                }
                else
                {
                    ltrPageTitle.Text = "Add New Stock Item";
                    ltrBreadcrumb.Text = "Add New";
                    ltrFormTitle.Text = "Add Stock Item";
                    btnSave.Text = "Save Stock Item";
                    chkStatus.Checked = true;
                    txtQuantity.Text = "0";
                    txtUnitPrice.Text = "0.00";
                    txtMinLevel.Text = "5";
                    txtMaxLevel.Text = "100";
                }
            }
        }

        /// <summary>
        /// Loads stock data for editing
        /// </summary>
        private void LoadStockData(int id)
        {
            try
            {
                StockModel stock = stockBAL.GetStockById(id);
                if (stock != null)
                {
                    txtProductName.Text = stock.ProductName;
                    txtCategory.Text = stock.Category;
                    txtBarcode.Text = stock.Barcode;
                    txtQuantity.Text = stock.Quantity.ToString();
                    txtUnitPrice.Text = stock.UnitPrice.ToString("F2");
                    txtTotalCost.Text = stock.TotalCost.ToString("F2");
                    txtMinLevel.Text = stock.MinStockLevel.ToString();
                    txtMaxLevel.Text = stock.MaxStockLevel.ToString();
                    txtLocation.Text = stock.Location;
                    chkStatus.Checked = stock.Status;
                    UpdateStatusText();
                }
                else
                {
                    ShowErrorMessage("Stock item not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading stock: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status text based on checkbox
        /// </summary>
        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkStatus.Checked ? "Active" : "Inactive";
        }

        /// <summary>
        /// Saves the stock item
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                int? companyID = SessionHelper.GetCompanyId();
                int? branchID = SessionHelper.GetBranchId();

                if (!companyID.HasValue || !branchID.HasValue)
                {
                    ShowErrorMessage("Company or branch not found in session.");
                    return;
                }

                decimal quantity = Convert.ToDecimal(txtQuantity.Text);
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                decimal totalCost = quantity * unitPrice;

                StockModel stock = new StockModel
                {
                    StockID = stockID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    ProductName = txtProductName.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Barcode = txtBarcode.Text.Trim(),
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalCost = totalCost,
                    MinStockLevel = Convert.ToInt32(txtMinLevel.Text),
                    MaxStockLevel = Convert.ToInt32(txtMaxLevel.Text),
                    Location = txtLocation.Text.Trim(),
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = stockBAL.SaveStock(stock);

                if (stockID > 0)
                {
                    auditLogger.LogUpdate("StockManagement", "Stock", savedID,
                        $"StockID: {stockID}", $"Product: {stock.ProductName}");
                    ShowSuccessMessage("Stock item updated successfully.");
                }
                else
                {
                    auditLogger.LogInsert("StockManagement", "Stock", savedID,
                        $"Product: {stock.ProductName}");
                    ShowSuccessMessage("Stock item added successfully.");
                }

                Response.AddHeader("REFRESH", "2;URL=StockList.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving stock: {ex.Message}");
                auditLogger.LogAction("Error", "StockManagement", "SaveStock", null, null, ex.Message);
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