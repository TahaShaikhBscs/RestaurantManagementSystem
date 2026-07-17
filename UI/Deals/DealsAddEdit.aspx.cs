// ============================================
// UI/Deals/DealsAddEdit.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Deals
{
    /// <summary>
    /// Deal Add/Edit Page - Handles adding and editing deals
    /// </summary>
    public partial class DealsAddEdit : Page
    {
        #region Private Fields

        private DealBAL dealBAL;
        private MenuBAL menuBAL;
        private AuditLogger auditLogger;
        private int dealID = 0;
        private const string MODULE_NAME = "DealManagement";
        private List<DealItem> dealItems;

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - initializes the page
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            if (Request.QueryString["DealID"] != null)
            {
                dealID = Convert.ToInt32(Request.QueryString["DealID"]);
            }

            // Initialize deal items from session
            if (Session["DealItems"] == null)
            {
                dealItems = new List<DealItem>();
                Session["DealItems"] = dealItems;
            }
            else
            {
                dealItems = (List<DealItem>)Session["DealItems"];
            }

            if (!IsPostBack)
            {
                LoadMenuItems();

                if (dealID > 0)
                {
                    SetEditMode();
                    LoadDealData(dealID);
                }
                else
                {
                    SetAddMode();
                    dealItems.Clear();
                    Session["DealItems"] = dealItems;
                }

                BindItemsGrid();
                CalculateTotals();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            dealBAL = new DealBAL();
            menuBAL = new MenuBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Mode Configuration

        /// <summary>
        /// Sets the page to edit mode
        /// </summary>
        private void SetEditMode()
        {
            ltrPageTitle.Text = "Edit Deal";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Deal Information";
            btnSave.Text = "Update Deal";
        }

        /// <summary>
        /// Sets the page to add mode
        /// </summary>
        private void SetAddMode()
        {
            ltrPageTitle.Text = "Add New Deal";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Add New Deal";
            btnSave.Text = "Save Deal";
            chkStatus.Checked = true;
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads menu items into the dropdown
        /// </summary>
        private void LoadMenuItems()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<Models.MenuItem> items = menuBAL.GetMenuItemsByBranch(branchID.Value, null, false);
                    ddlMenuItem.DataSource = items;
                    ddlMenuItem.DataTextField = "ItemName";
                    ddlMenuItem.DataValueField = "MenuItemID";
                    ddlMenuItem.DataBind();
                    ddlMenuItem.Items.Insert(0, new ListItem("-- Select Menu Item --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading menu items: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads deal data for editing
        /// </summary>
        private void LoadDealData(int id)
        {
            try
            {
                Deal deal = dealBAL.GetDealWithItems(id);
                if (deal != null)
                {
                    txtDealName.Text = deal.DealName;
                    txtDescription.Text = deal.Description;
                    txtDealPrice.Text = deal.DealPrice.ToString("F2");
                    txtImage.Text = deal.Image;

                    if (deal.StartDate.HasValue)
                    {
                        txtStartDate.Text = deal.StartDate.Value.ToString("yyyy-MM-dd");
                    }

                    if (deal.EndDate.HasValue)
                    {
                        txtEndDate.Text = deal.EndDate.Value.ToString("yyyy-MM-dd");
                    }

                    chkStatus.Checked = deal.Status;
                    UpdateStatusText();

                    // Load deal items
                    dealItems.Clear();
                    foreach (var item in deal.DealItems)
                    {
                        dealItems.Add(item);
                    }
                    Session["DealItems"] = dealItems;

                    auditLogger.LogAction("View", MODULE_NAME, "Deal", id, $"DealName: {deal.DealName}");
                }
                else
                {
                    ShowErrorMessage("Deal not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadDealData", id, null, ex.Message);
                ShowErrorMessage($"Error loading deal: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Updates the status text based on checkbox
        /// </summary>
        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkStatus.Checked ? "Active" : "Inactive";
        }

        /// <summary>
        /// Binds items to the grid view
        /// </summary>
        private void BindItemsGrid()
        {
            gvItems.DataSource = dealItems;
            gvItems.DataBind();
        }

        /// <summary>
        /// Calculates and displays totals
        /// </summary>
        private void CalculateTotals()
        {
            decimal originalPrice = 0;
            foreach (var item in dealItems)
            {
                originalPrice += item.TotalPrice;
            }

            decimal dealPrice = 0;
            if (!string.IsNullOrEmpty(txtDealPrice.Text))
            {
                decimal.TryParse(txtDealPrice.Text, out dealPrice);
            }

            decimal discount = originalPrice - dealPrice;
            decimal discountPercent = originalPrice > 0 ? (discount / originalPrice) * 100 : 0;

            ltrOriginalPrice.Text = originalPrice.ToString("N2");
            ltrDealPrice.Text = dealPrice.ToString("N2");
            ltrDiscount.Text = discount.ToString("N2");
            ltrDiscountPercent.Text = discountPercent.ToString("F0") + "%";
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Adds an item to the deal
        /// </summary>
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                int menuItemID = Convert.ToInt32(ddlMenuItem.SelectedValue);
                int quantity = Convert.ToInt32(txtQuantity.Text);

                if (menuItemID <= 0)
                {
                    ShowErrorMessage("Please select a menu item.");
                    return;
                }

                if (quantity <= 0)
                {
                    ShowErrorMessage("Quantity must be greater than 0.");
                    return;
                }

                // Get menu item details
                Models.MenuItem menuItem = menuBAL.GetMenuItemById(menuItemID);
                if (menuItem == null)
                {
                    ShowErrorMessage("Menu item not found.");
                    return;
                }

                // Check if item already exists
                var existing = dealItems.Find(i => i.MenuItemID == menuItemID);
                if (existing != null)
                {
                    existing.Quantity += quantity;
                }
                else
                {
                    dealItems.Add(new DealItem
                    {
                        MenuItemID = menuItemID,
                        ItemName = menuItem.ItemName,
                        Quantity = quantity,
                        Price = menuItem.Price,
                        CategoryName = menuItem.CategoryName
                    });
                }

                Session["DealItems"] = dealItems;
                BindItemsGrid();
                CalculateTotals();

                // Clear inputs
                ddlMenuItem.SelectedValue = "";
                txtQuantity.Text = "1";

                ShowSuccessMessage("Item added successfully.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding item: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes an item from the deal
        /// </summary>
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "RemoveItem")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    if (index >= 0 && index < dealItems.Count)
                    {
                        dealItems.RemoveAt(index);
                        Session["DealItems"] = dealItems;
                        BindItemsGrid();
                        CalculateTotals();
                        ShowSuccessMessage("Item removed.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error removing item: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the deal
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (dealItems.Count == 0)
                {
                    ShowErrorMessage("Please add at least one menu item to the deal.");
                    return;
                }

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

                // Create deal object
                Deal deal = new Deal
                {
                    DealID = dealID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    DealName = txtDealName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    DealPrice = Convert.ToDecimal(txtDealPrice.Text),
                    Image = txtImage.Text.Trim(),
                    StartDate = string.IsNullOrEmpty(txtStartDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtStartDate.Text),
                    EndDate = string.IsNullOrEmpty(txtEndDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtEndDate.Text),
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                // Save deal
                int savedID = dealBAL.SaveDeal(deal, dealItems);

                if (savedID > 0)
                {
                    // Clear session items
                    dealItems.Clear();
                    Session["DealItems"] = dealItems;

                    ShowSuccessMessage("Deal saved successfully.");
                    Response.AddHeader("REFRESH", "2;URL=DealsList.aspx");
                }
                else
                {
                    ShowErrorMessage("Failed to save deal.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveDeal", dealID, null, ex.Message);
                ShowErrorMessage($"Error saving deal: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            dealItems.Clear();
            Session["DealItems"] = dealItems;
            Response.Redirect("DealsList.aspx");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Shows a success message
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            ltrSuccessMessage.Text = message;
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            ltrErrorMessage.Text = message;
        }

        #endregion
    }
}