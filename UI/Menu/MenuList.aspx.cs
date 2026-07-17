// ============================================
// UI/Menu/MenuList.aspx.cs - Production-Ready
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Menu
{
    /// <summary>
    /// Menu List Page - Displays all menu items with advanced filtering and management
    /// </summary>
    public partial class MenuList : Page
    {
        #region Private Fields

        private MenuBAL menuBAL;
        private CategoryBAL categoryBAL;
        private AuditLogger auditLogger;
        private const string MODULE_NAME = "MenuManagement";

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

            if (!IsPostBack)
            {
                LoadCategories();
                LoadMenuItems();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            menuBAL = new MenuBAL();
            categoryBAL = new CategoryBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads categories into the filter dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                List<Category> categories = categoryBAL.GetCategoriesByBranch(branchID.Value, false);
                ddlCategoryFilter.DataSource = categories;
                ddlCategoryFilter.DataTextField = "CategoryName";
                ddlCategoryFilter.DataValueField = "CategoryID";
                ddlCategoryFilter.DataBind();
                ddlCategoryFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Categories", ""));
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCategories", null, null, ex.Message);
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads menu items into the grid view with statistics
        /// </summary>
        private void LoadMenuItems()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                // Build filter parameters
                int? categoryID = null;
                if (!string.IsNullOrEmpty(ddlCategoryFilter.SelectedValue))
                {
                    categoryID = Convert.ToInt32(ddlCategoryFilter.SelectedValue);
                }

                bool includeInactive = true;
                List<MenuItem> items = menuBAL.GetMenuItemsByBranch(branchID.Value, categoryID, includeInactive);

                // Apply status filter
                items = ApplyStatusFilter(items);

                // Apply search filter
                if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                {
                    string searchTerm = txtSearch.Text.Trim().ToLower();
                    items = items.FindAll(i =>
                        i.ItemName.ToLower().Contains(searchTerm) ||
                        (i.SKU != null && i.SKU.ToLower().Contains(searchTerm)) ||
                        (i.Barcode != null && i.Barcode.ToLower().Contains(searchTerm))
                    );
                }

                // Bind data
                gvMenuItems.DataSource = items;
                gvMenuItems.DataBind();

                // Update statistics
                UpdateStatistics(items);
                ltrTotalCount.Text = items.Count.ToString();

                auditLogger.LogAction("View", MODULE_NAME, "MenuItems", null, 
                    $"BranchID: {branchID}, TotalItems: {items.Count}");
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadMenuItems", null, null, ex.Message);
                ShowErrorMessage($"Error loading menu items: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies status filter to the items list
        /// </summary>
        private List<MenuItem> ApplyStatusFilter(List<MenuItem> items)
        {
            string filter = ddlStatusFilter.SelectedValue;

            if (string.IsNullOrEmpty(filter))
                return items;

            switch (filter)
            {
                case "Active":
                    return items.FindAll(i => i.Status);
                case "Inactive":
                    return items.FindAll(i => !i.Status);
                case "Available":
                    return items.FindAll(i => i.IsAvailable);
                case "Unavailable":
                    return items.FindAll(i => !i.IsAvailable);
                default:
                    return items;
            }
        }

        /// <summary>
        /// Updates statistics cards
        /// </summary>
        private void UpdateStatistics(List<MenuItem> items)
        {
            int total = items.Count;
            int active = 0;
            int available = 0;
            int featured = 0;

            foreach (var item in items)
            {
                if (item.Status) active++;
                if (item.IsAvailable) available++;
                if (item.IsFeatured) featured++;
            }

            ltrTotalItems.Text = total.ToString();
            ltrActiveItems.Text = active.ToString();
            ltrAvailableItems.Text = available.ToString();
            ltrFeaturedItems.Text = featured.ToString();
            ltrActiveCount.Text = active.ToString();
        }

        #endregion

        #region Grid View Events

        /// <summary>
        /// Handles row data bound event for formatting
        /// </summary>
        protected void gvMenuItems_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // Add hover effect or additional formatting if needed
            }
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvMenuItems_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                int menuItemID = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditMenuItem":
                        Response.Redirect($"MenuAddEdit.aspx?MenuItemID={menuItemID}");
                        break;

                    case "ToggleAvailability":
                        ToggleAvailability(menuItemID);
                        break;

                    case "DeleteMenuItem":
                        DeleteMenuItem(menuItemID);
                        break;
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "RowCommand", null, null, ex.Message);
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles page index changing
        /// </summary>
        protected void gvMenuItems_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvMenuItems.PageIndex = e.NewPageIndex;
            LoadMenuItems();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Handles search button click
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
            auditLogger.LogSearch(MODULE_NAME, txtSearch.Text.Trim());
        }

        /// <summary>
        /// Handles clear button click
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCategoryFilter.SelectedValue = "";
            ddlStatusFilter.SelectedValue = "";
            LoadMenuItems();
            ShowSuccessMessage("Filters cleared.");
        }

        /// <summary>
        /// Handles refresh button click
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
            ShowSuccessMessage("Menu items refreshed.");
        }

        /// <summary>
        /// Handles add new button click
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuAddEdit.aspx");
        }

        /// <summary>
        /// Handles category filter change
        /// </summary>
        protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenuItems();
        }

        /// <summary>
        /// Handles status filter change
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenuItems();
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Toggles the availability of a menu item
        /// </summary>
        private void ToggleAvailability(int menuItemID)
        {
            int? userID = SessionHelper.GetUserId();
            if (!userID.HasValue)
            {
                ShowErrorMessage("User session expired. Please login again.");
                return;
            }

            MenuItem item = menuBAL.GetMenuItemById(menuItemID);
            if (item != null)
            {
                bool newAvailability = !item.IsAvailable;
                bool updated = menuBAL.UpdateAvailability(menuItemID, newAvailability, userID.Value);
                
                if (updated)
                {
                    auditLogger.LogUpdate(MODULE_NAME, "MenuItems", menuItemID, 
                        $"Availability: {item.IsAvailable}", $"Availability: {newAvailability}");
                    ShowSuccessMessage($"Item availability toggled to {(newAvailability ? "Available" : "Unavailable")}.");
                    LoadMenuItems();
                }
                else
                {
                    ShowErrorMessage("Failed to toggle availability.");
                }
            }
        }

        /// <summary>
        /// Deletes a menu item
        /// </summary>
        private void DeleteMenuItem(int menuItemID)
        {
            int? userID = SessionHelper.GetUserId();
            if (!userID.HasValue)
            {
                ShowErrorMessage("User session expired. Please login again.");
                return;
            }

            bool deleted = menuBAL.DeleteMenuItem(menuItemID, userID.Value);
            if (deleted)
            {
                auditLogger.LogDelete(MODULE_NAME, "MenuItems", menuItemID, $"MenuItemID: {menuItemID}");
                ShowSuccessMessage("Menu item deleted successfully.");
                LoadMenuItems();
            }
            else
            {
                ShowErrorMessage("Failed to delete menu item.");
            }
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