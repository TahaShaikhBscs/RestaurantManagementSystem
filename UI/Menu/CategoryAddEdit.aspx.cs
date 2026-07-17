// ============================================
// UI/Menu/CategoryAddEdit.aspx.cs - Complete Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Menu
{
    /// <summary>
    /// Category Add/Edit Page - Handles adding and editing categories
    /// </summary>
    public partial class CategoryAddEdit : Page
    {
        #region Private Fields

        private CategoryBAL categoryBAL;
        private AuditLogger auditLogger;
        private int categoryID = 0;
        private const string MODULE_NAME = "CategoryManagement";

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - loads category data for editing
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            if (Request.QueryString["CategoryID"] != null)
            {
                categoryID = Convert.ToInt32(Request.QueryString["CategoryID"]);
            }

            if (!IsPostBack)
            {
                if (categoryID > 0)
                {
                    SetEditMode();
                    //LoadParentCategories();
                    LoadCategoryData(categoryID);
                }
                else
                {
                    SetAddMode();
                    //LoadParentCategories();
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
            ltrPageTitle.Text = "Edit Category";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Category Information";
            btnSave.Text = "Update Category";
            btnSaveAndContinue.Visible = true;
        }

        /// <summary>
        /// Sets the page to add mode
        /// </summary>
        private void SetAddMode()
        {
            ltrPageTitle.Text = "Add New Category";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Add New Category";
            btnSave.Text = "Save Category";
            btnSaveAndContinue.Visible = false;
            chkStatus.Checked = true;
            txtDisplayOrder.Text = "0";
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads parent categories into the dropdown
        /// </summary>
        //private void LoadParentCategories()
        //{
        //    try
        //    {
        //        int? branchID = SessionHelper.GetBranchId();
        //        if (!branchID.HasValue)
        //        {
        //            ShowErrorMessage("Branch not found in session.");
        //            return;
        //        }

        //        List<Category> categories = categoryBAL.GetCategoriesByBranch(branchID.Value, false);

        //        // Remove current category from list if editing
        //        if (categoryID > 0)
        //        {
        //            categories = categories.FindAll(c => c.CategoryID != categoryID);
        //        }

        //        ddlParentCategory.DataSource = categories;
        //        ddlParentCategory.DataTextField = "CategoryName";
        //        ddlParentCategory.DataValueField = "CategoryID";
        //        ddlParentCategory.DataBind();
        //        ddlParentCategory.Items.Insert(0, new ListItem("-- None (Top Level) --", ""));
        //    }
        //    catch (Exception ex)
        //    {
        //        auditLogger.LogAction("Error", MODULE_NAME, "LoadParentCategories", null, null, ex.Message);
        //        ShowErrorMessage($"Error loading parent categories: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Loads category data for editing
        /// </summary>
        private void LoadCategoryData(int id)
        {
            try
            {
                Category category = categoryBAL.GetCategoryById(id);
                if (category != null)
                {
                    txtCategoryName.Text = category.CategoryName;
                    txtDescription.Text = category.Description;
                    txtDisplayOrder.Text = category.DisplayOrder.ToString();
                    //txtIcon.Text = category.Icon;
                    //txtImage.Text = category.Image;
                    chkStatus.Checked = category.Status;
                    UpdateStatusText();

                    // Set parent category
                    //if (category.ParentCategoryID.HasValue)
                    //{
                    //    ddlParentCategory.SelectedValue = category.ParentCategoryID.Value.ToString();
                    //}

                    auditLogger.LogAction("View", MODULE_NAME, "Category", id, $"CategoryName: {category.CategoryName}");
                }
                else
                {
                    ShowErrorMessage("Category not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCategoryData", id, null, ex.Message);
                ShowErrorMessage($"Error loading category: {ex.Message}");
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
        /// Validates the category data
        /// </summary>
        private bool ValidateCategory()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                ShowErrorMessage("Category name is required.");
                return false;
            }

            int displayOrder;
            if (!int.TryParse(txtDisplayOrder.Text, out displayOrder) || displayOrder < 0)
            {
                ShowErrorMessage("Display order must be a valid number greater than or equal to 0.");
                return false;
            }

            return true;
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Saves the category
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveCategory(false);
        }

        /// <summary>
        /// Saves the category and continues editing
        /// </summary>
        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveCategory(true);
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CategoryList.aspx");
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Saves the category
        /// </summary>
        /// <param name="continueEditing">True to stay on page, false to redirect to list</param>
        private void SaveCategory(bool continueEditing)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidateCategory())
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

                Category category = CreateCategoryObject(userID.Value, companyID.Value, branchID.Value);

                int savedID = categoryBAL.SaveCategory(category);

                if (savedID > 0)
                {
                    if (categoryID > 0)
                    {
                        auditLogger.LogUpdate(MODULE_NAME, "Categories", savedID,
                            $"CategoryID: {categoryID}", $"CategoryName: {category.CategoryName}");
                        ShowSuccessMessage("Category updated successfully.");
                    }
                    else
                    {
                        auditLogger.LogInsert(MODULE_NAME, "Categories", savedID,
                            $"CategoryName: {category.CategoryName}");
                        ShowSuccessMessage("Category added successfully.");
                    }

                    if (continueEditing)
                    {
                        // Refresh the page with the new ID
                        Response.Redirect($"CategoryAddEdit.aspx?CategoryID={savedID}");
                    }
                    else
                    {
                        Response.AddHeader("REFRESH", "2;URL=CategoryList.aspx");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to save category.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveCategory", categoryID, null, ex.Message);
                ShowErrorMessage($"Error saving category: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a Category object from form data
        /// </summary>
        private Category CreateCategoryObject(int userID, int companyID, int branchID)
        {
            int? parentCategoryID = 0;
            //if (!string.IsNullOrEmpty(ddlParentCategory.SelectedValue))
            //{
            //    parentCategoryID = Convert.ToInt32(ddlParentCategory.SelectedValue);
            //}

            return new Category
            {
                CategoryID = categoryID,
                CompanyID = companyID,
                BranchID = branchID,
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                ParentCategoryID = parentCategoryID,
                DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text),
                Icon = "Icon",
                Image = "Image",
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