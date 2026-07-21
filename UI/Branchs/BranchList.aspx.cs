
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Branch
{
    /// <summary>
    /// Branch List Page - Displays all branches with management options
    /// </summary>
    public partial class BranchList : Page
    {
        #region Private Fields

        private BranchBAL branchBAL;
        private CompanyBAL companyBAL;
        private AuditLogger auditLogger;
        private const string MODULE_NAME = "BranchManagement";

        #endregion

        #region Page Events

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
                LoadBranches();
            }
        }

        #endregion

        #region Initialization

        private void InitializeComponents()
        {
            branchBAL = new BranchBAL();
            companyBAL = new CompanyBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Data Loading

        private void LoadCompanies()
        {
            try
            {
                List<Models.Company> companies = companyBAL.GetAllCompanies();
                ddlCompanyFilter.DataSource = companies;
                ddlCompanyFilter.DataTextField = "CompanyName";
                ddlCompanyFilter.DataValueField = "CompanyID";
                ddlCompanyFilter.DataBind();
                ddlCompanyFilter.Items.Insert(0, new ListItem("All Companies", "0"));
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading companies: {ex.Message}");
            }
        }

        private void LoadBranches()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                int companyFilter = Convert.ToInt32(ddlCompanyFilter.SelectedValue);
                List<Models.Branch> branches;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    if (companyFilter > 0)
                    {
                        branches = branchBAL.GetBranchesByCompany(companyFilter);
                    }
                    else
                    {
                        branches = branchBAL.GetAllBranches();
                    }
                }
                else
                {
                    branches = branchBAL.SearchBranches(searchTerm, companyFilter > 0 ? companyFilter : (int?)null);
                }

                gvBranches.DataSource = branches;
                gvBranches.DataBind();

                UpdateStatistics(branches);
                ltrTotalCount.Text = branches.Count.ToString();

                auditLogger.LogAction("View", MODULE_NAME, "Branches", null, $"TotalItems: {branches.Count}");
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadBranches", null, null, ex.Message);
                ShowErrorMessage($"Error loading branches: {ex.Message}");
            }
        }

        private void UpdateStatistics(List<Models.Branch> branches)
        {
            int total = branches.Count;
            int active = 0;
            int inactive = 0;
            int kitchen = 0;

            foreach (var branch in branches)
            {
                if (branch.Status) active++;
                else inactive++;
                if (branch.Kitchen) kitchen++;
            }

            ltrTotalBranches.Text = total.ToString();
            ltrActiveBranches.Text = active.ToString();
            ltrInactiveBranches.Text = inactive.ToString();
            ltrKitchenBranches.Text = kitchen.ToString();
        }

        #endregion

        #region Button Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadBranches();
            auditLogger.LogSearch(MODULE_NAME, txtSearch.Text.Trim());
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCompanyFilter.SelectedValue = "0";
            LoadBranches();
            ShowSuccessMessage("Filters cleared.");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBranches();
            ShowSuccessMessage("Branches refreshed.");
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("BranchAddEdit.aspx");
        }

        protected void ddlCompanyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBranches();
        }

        #endregion

        #region Grid View Events

        protected void gvBranches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int branchID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditBranch")
                {
                    Response.Redirect($"BranchAddEdit.aspx?BranchID={branchID}");
                }
                else if (e.CommandName == "DeleteBranch")
                {
                    hfDeleteBranchID.Value = branchID.ToString();
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "RowCommand", null, null, ex.Message);
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        protected void gvBranches_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBranches.PageIndex = e.NewPageIndex;
            LoadBranches();
        }

        #endregion

        #region Delete Confirmation

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int branchID = Convert.ToInt32(hfDeleteBranchID.Value);
                int? userID = SessionHelper.GetUserId();

                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                bool deleted = branchBAL.DeleteBranch(branchID, userID.Value);
                if (deleted)
                {
                    auditLogger.LogDelete(MODULE_NAME, "Branches", branchID, $"BranchID: {branchID}");
                    ShowSuccessMessage("Branch deleted successfully.");
                    LoadBranches();

                    string closeScript = "$('#deleteModal').modal('hide');";
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", closeScript, true);
                }
                else
                {
                    ShowErrorMessage("Failed to delete branch.");
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

        #endregion
    }
}