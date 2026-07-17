// ============================================
// UI/Menu/MenuAddEdit.aspx.cs - Production-Ready
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
    /// Menu Item Add/Edit Page - Handles adding and editing menu items
    /// </summary>
    public partial class MenuAddEdit : Page
    {
        #region Private Fields

        private MenuBAL menuBAL;
        private CategoryBAL categoryBAL;
        private AuditLogger auditLogger;
        private int menuItemID = 0;
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

            if (Request.QueryString["MenuItemID"] != null)
            {
                menuItemID = Convert.ToInt32(Request.QueryString["MenuItemID"]);
            }

            if (!IsPostBack)
            {
                LoadCategories();

                if (menuItemID > 0)
                {
                    SetEditMode();
                    LoadMenuItemData(menuItemID);
                }
                else
                {
                    SetAddMode();
                }
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

        #region Mode Configuration

        /// <summary>
        /// Sets the page to edit mode
        /// </summary>
        private void SetEditMode()
        {
            ltrPageTitle.Text = "Edit Menu Item";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Menu Item Information";
            btnSave.Text = "Update Menu Item";
            btnSaveAndContinue.Visible = true;
        }

        /// <summary>
        /// Sets the page to add mode
        /// </summary>
        private void SetAddMode()
        {
            ltrPageTitle.Text = "Add New Menu Item";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Add New Menu Item";
            btnSave.Text = "Save Menu Item";
            btnSaveAndContinue.Visible = false;
            chkIsAvailable.Checked = true;
            chkIsKitchen.Checked = true;
            chkStatus.Checked = true;
            txtPreparationTime.Text = "15";
            txtTax.Text = "0";
            txtDiscount.Text = "0";
            txtMinStock.Text = "5";
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads categories into the dropdown
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
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Category --", ""));
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCategories", null, null, ex.Message);
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads menu item data for editing
        /// </summary>
        private void LoadMenuItemData(int id)
        {
            try
            {
                MenuItem item = menuBAL.GetMenuItemById(id);
                if (item != null)
                {
                    txtItemName.Text = item.ItemName;
                    txtDescription.Text = item.Description;
                    ddlCategory.SelectedValue = item.CategoryID.ToString();
                    txtPrice.Text = item.Price.ToString("F2");
                    txtCost.Text = item.Cost.ToString("F2");
                    txtTax.Text = item.Tax.ToString("F2");
                    txtDiscount.Text = item.Discount.ToString("F2");
                    txtSKU.Text = item.SKU;
                    txtBarcode.Text = item.Barcode;
                    txtPreparationTime.Text = item.PreparationTime.ToString();
                    txtMinStock.Text = item.MinStockAlert.ToString();
                    chkIsAvailable.Checked = item.IsAvailable;
                    chkIsKitchen.Checked = item.IsKitchen;
                    chkIsFeatured.Checked = item.IsFeatured;
                    chkStatus.Checked = item.Status;
                    UpdateStatusText();

                    auditLogger.LogAction("View", MODULE_NAME, "MenuItem", id, $"ItemName: {item.ItemName}");
                }
                else
                {
                    ShowErrorMessage("Menu item not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadMenuItemData", id, null, ex.Message);
                ShowErrorMessage($"Error loading menu item: {ex.Message}");
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
        /// Validates the menu item data
        /// </summary>
        private bool ValidateMenuItem()
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                ShowErrorMessage("Item name is required.");
                return false;
            }

            if (string.IsNullOrEmpty(ddlCategory.SelectedValue) || ddlCategory.SelectedValue == "")
            {
                ShowErrorMessage("Please select a category.");
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                ShowErrorMessage("Please enter a valid price greater than 0.");
                return false;
            }

            return true;
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Saves the menu item
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveMenuItem(false);
        }

        /// <summary>
        /// Saves the menu item and continues editing
        /// </summary>
        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveMenuItem(true);
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuList.aspx");
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Saves the menu item
        /// </summary>
        /// <param name="continueEditing">True to stay on page, false to redirect to list</param>
        private void SaveMenuItem(bool continueEditing)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidateMenuItem())
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

                MenuItem item = CreateMenuItemObject(userID.Value, companyID.Value, branchID.Value);

                int savedID = menuBAL.SaveMenuItem(item);

                if (savedID > 0)
                {
                    if (menuItemID > 0)
                    {
                        auditLogger.LogUpdate(MODULE_NAME, "MenuItems", savedID,
                            $"MenuItemID: {menuItemID}", $"ItemName: {item.ItemName}");
                        ShowSuccessMessage("Menu item updated successfully.");
                    }
                    else
                    {
                        auditLogger.LogInsert(MODULE_NAME, "MenuItems", savedID,
                            $"ItemName: {item.ItemName}");
                        ShowSuccessMessage("Menu item added successfully.");
                    }

                    if (continueEditing)
                    {
                        // Refresh the page with the new ID
                        Response.Redirect($"MenuAddEdit.aspx?MenuItemID={savedID}");
                    }
                    else
                    {
                        Response.AddHeader("REFRESH", "2;URL=MenuList.aspx");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to save menu item.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveMenuItem", menuItemID, null, ex.Message);
                ShowErrorMessage($"Error saving menu item: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a MenuItem object from form data
        /// </summary>
        private MenuItem CreateMenuItemObject(int userID, int companyID, int branchID)
        {
            return new MenuItem
            {
                MenuItemID = menuItemID,
                CompanyID = companyID,
                BranchID = branchID,
                CategoryID = Convert.ToInt32(ddlCategory.SelectedValue),
                ItemName = txtItemName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Price = Convert.ToDecimal(txtPrice.Text),
                Cost = string.IsNullOrEmpty(txtCost.Text) ? 0 : Convert.ToDecimal(txtCost.Text),
                Tax = Convert.ToDecimal(txtTax.Text),
                Discount = Convert.ToDecimal(txtDiscount.Text),
                SKU = txtSKU.Text.Trim(),
                Barcode = txtBarcode.Text.Trim(),
                PreparationTime = Convert.ToInt32(txtPreparationTime.Text),
                IsAvailable = chkIsAvailable.Checked,
                IsKitchen = chkIsKitchen.Checked,
                IsFeatured = chkIsFeatured.Checked,
                MinStockAlert = Convert.ToInt32(txtMinStock.Text),
                Status = chkStatus.Checked,
                CreatedBy = userID,
                UpdatedBy = userID
            };
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