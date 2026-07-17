// ============================================
// UI/Company/CompanyList.aspx.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Company
{
    /// <summary>
    /// Company List Page - Displays all companies with management options
    /// </summary>
    public partial class CompanyList : Page
    {
        #region Private Fields

        private CompanyBAL companyBAL;
        private AuditLogger auditLogger;
        private const string MODULE_NAME = "CompanyManagement";

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - loads companies
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
                LoadCompanies();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            companyBAL = new CompanyBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads companies into the grid view with statistics
        /// </summary>
        private void LoadCompanies()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                List<Models.Company> companies;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    companies = companyBAL.GetAllCompanies();
                }
                else
                {
                    companies = companyBAL.SearchCompanies(searchTerm);
                }

                gvCompanies.DataSource = companies;
                gvCompanies.DataBind();

                // Update statistics
                UpdateStatistics(companies);
                ltrTotalCount.Text = companies.Count.ToString();

                auditLogger.LogAction("View", MODULE_NAME, "Companies", null, $"TotalItems: {companies.Count}");
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCompanies", null, null, ex.Message);
                ShowErrorMessage($"Error loading companies: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates statistics cards
        /// </summary>
        private void UpdateStatistics(List<Models.Company> companies)
        {
            int total = companies.Count;
            int active = 0;
            int inactive = 0;

            foreach (var company in companies)
            {
                if (company.Status) active++;
                else inactive++;
            }

            ltrTotalCompanies.Text = total.ToString();
            ltrActiveCompanies.Text = active.ToString();
            ltrInactiveCompanies.Text = inactive.ToString();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Handles search button click
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCompanies();
            auditLogger.LogSearch(MODULE_NAME, txtSearch.Text.Trim());
        }

        /// <summary>
        /// Handles clear button click
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadCompanies();
            ShowSuccessMessage("Filters cleared.");
        }

        /// <summary>
        /// Handles refresh button click
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCompanies();
            ShowSuccessMessage("Companies refreshed.");
        }

        /// <summary>
        /// Handles add new button click
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("CompanyAddEdit.aspx");
        }

        #endregion

        #region Grid View Events

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvCompanies_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int companyID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditCompany")
                {
                    Response.Redirect($"CompanyAddEdit.aspx?CompanyID={companyID}");
                }
                else if (e.CommandName == "DeleteCompany")
                {
                    hfDeleteCompanyID.Value = companyID.ToString();
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
        protected void gvCompanies_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCompanies.PageIndex = e.NewPageIndex;
            LoadCompanies();
        }

        #endregion

        #region Delete Confirmation

        /// <summary>
        /// Handles confirm delete button click
        /// </summary>
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int companyID = Convert.ToInt32(hfDeleteCompanyID.Value);

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                bool deleted = companyBAL.DeleteCompany(companyID, userID.Value);
                if (deleted)
                {
                    auditLogger.LogDelete(MODULE_NAME, "Companies", companyID, $"CompanyID: {companyID}");
                    ShowSuccessMessage("Company deleted successfully.");
                    LoadCompanies();

                    string closeScript = "$('#deleteModal').modal('hide');";
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", closeScript, true);
                }
                else
                {
                    ShowErrorMessage("Failed to delete company.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "ConfirmDelete", null, null, ex.Message);
                ShowErrorMessage($"Error: {ex.Message}");
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