// ============================================
// UI/Menu/CategoryList.aspx.cs - Complete Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Menu
{
    /// <summary>
    /// Category List Page - Displays all categories with management options
    /// </summary>
    public partial class CategoryList : Page
    {
        #region Private Fields

        private CategoryBAL categoryBAL;
        private AuditLogger auditLogger;
        private const string MODULE_NAME = "CategoryManagement";

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - loads categories
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

        #region Data Loading

        /// <summary>
        /// Loads categories into the grid view with statistics
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

                string searchTerm = txtSearch.Text.Trim();
                List<Category> categories;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    categories = categoryBAL.GetCategoriesByBranch(branchID.Value);
                }
                else
                {
                    categories = categoryBAL.SearchCategories(branchID.Value, searchTerm);
                }

                gvCategories.DataSource = categories;
                gvCategories.DataBind();

                // Update statistics
                UpdateStatistics(categories);
                ltrGridCount.Text = categories.Count.ToString();

                auditLogger.LogAction("View", MODULE_NAME, "Categories", null,
                    $"BranchID: {branchID}, TotalItems: {categories.Count}");
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCategories", null, null, ex.Message);
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates statistics cards
        /// </summary>
        private void UpdateStatistics(List<Category> categories)
        {
            int total = categories.Count;
            int active = 0;
            int parentCount = 0;
            int subCount = 0;

            foreach (var category in categories)
            {
                if (category.Status) active++;
                if (category.IsRootCategory) parentCount++;
                else subCount++;
            }

            ltrTotalCount.Text = total.ToString();
            ltrActiveCount.Text = active.ToString();
            ltrParentCount.Text = parentCount.ToString();
            ltrSubCount.Text = subCount.ToString();
        }

        #endregion

        #region Grid View Events

        /// <summary>
        /// Handles row data bound event
        /// </summary>
        protected void gvCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Additional formatting if needed
            }
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int categoryID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditCategory")
                {
                    Response.Redirect($"CategoryAddEdit.aspx?CategoryID={categoryID}");
                }
                else if (e.CommandName == "DeleteCategory")
                {
                    // Set category ID for modal
                    hfDeleteCategoryID.Value = categoryID.ToString();
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
        protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategories.PageIndex = e.NewPageIndex;
            LoadCategories();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Handles search button click
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCategories();
            auditLogger.LogSearch(MODULE_NAME, txtSearch.Text.Trim());
        }

        /// <summary>
        /// Handles clear button click
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadCategories();
            ShowSuccessMessage("Filters cleared.");
        }

        /// <summary>
        /// Handles refresh button click
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
            ShowSuccessMessage("Categories refreshed.");
        }

        /// <summary>
        /// Handles add new button click
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("CategoryAddEdit.aspx");
        }

        /// <summary>
        /// Handles confirm delete button click
        /// </summary>
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int categoryID = Convert.ToInt32(hfDeleteCategoryID.Value);

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                bool deleted = categoryBAL.DeleteCategory(categoryID, userID.Value);
                if (deleted)
                {
                    auditLogger.LogDelete(MODULE_NAME, "Categories", categoryID, $"CategoryID: {categoryID}");
                    ShowSuccessMessage("Category deleted successfully.");
                    LoadCategories();

                    // Close modal
                    string closeScript = "$('#deleteModal').modal('hide');";
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", closeScript, true);
                }
                else
                {
                    ShowErrorMessage("Failed to delete category.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "ConfirmDelete", null, null, ex.Message);
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles export button click
        /// </summary>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable exportData = GetExportData();
                if (exportData != null && exportData.Rows.Count > 0)
                {
                    ReportHelper reportHelper = new ReportHelper();
                    reportHelper.ExportToExcel(exportData, $"Categories_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", MODULE_NAME, "Categories", null, "Exported to Excel");
                }
                else
                {
                    ShowErrorMessage("No data to export.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "Export", null, null, ex.Message);
                ShowErrorMessage($"Error exporting: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets data for export
        /// </summary>
        private DataTable GetExportData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CategoryID");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("ParentCategory");
            dt.Columns.Add("Description");
            dt.Columns.Add("DisplayOrder");
            dt.Columns.Add("Status");

            foreach (GridViewRow row in gvCategories.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["CategoryID"] = gvCategories.DataKeys[row.RowIndex].Value;
                dr["CategoryName"] = row.Cells[1].Text;
                dr["ParentCategory"] = row.Cells[2].Text;
                dr["Description"] = row.Cells[3].Text;
                dr["DisplayOrder"] = row.Cells[4].Text;
                dr["Status"] = row.Cells[5].Text;
                dt.Rows.Add(dr);
            }

            return dt;
        }

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