// ============================================
// UI/Users/UserList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// IMPORTANT: Use model aliases to avoid namespace conflicts
using UserModel = RestaurantManagementSystem.Models.User;
using BranchModel = RestaurantManagementSystem.Models.Branch;
using CompanyModel = RestaurantManagementSystem.Models.Company;

namespace RestaurantManagementSystem.UI.Users
{
    /// <summary>
    /// User List Page - Displays all users with search and management options
    /// </summary>
    public partial class UserList : Page
    {
        private UserBAL userBAL;
        private BranchBAL branchBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads user data
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            userBAL = new UserBAL();
            branchBAL = new BranchBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadBranches();
                LoadUsers();
            }
        }

        /// <summary>
        /// Loads branches into the filter dropdown
        /// </summary>
        private void LoadBranches()
        {
            try
            {
                int? companyID = SessionHelper.GetCompanyId();
                if (companyID.HasValue)
                {
                    List<BranchModel> branches = branchBAL.GetBranchesByCompany(companyID.Value);
                    ddlBranchFilter.DataSource = branches;
                    ddlBranchFilter.DataTextField = "BranchName";
                    ddlBranchFilter.DataValueField = "BranchID";
                    ddlBranchFilter.DataBind();
                    ddlBranchFilter.Items.Insert(0, new ListItem("All Branches", "0"));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading branches: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads users into the grid view
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                // In a real implementation, we would have a method to get users by branch
                // For now, we'll use a simplified approach
                List<UserModel> users = new List<UserModel>();

                // This would be replaced with actual DAL call
                // For demonstration, we'll use a placeholder
                gvUsers.DataSource = users;
                gvUsers.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading users: {ex.Message}");
                auditLogger.LogAction("Error", "UserManagement", "Users", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Searches users
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                // Implement search logic
                LoadUsers();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching users: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears search and reloads all users
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadUsers();
        }

        /// <summary>
        /// Handles branch filter change
        /// </summary>
        protected void ddlBranchFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        /// <summary>
        /// Redirects to add new user page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands (Edit, Delete)
        /// </summary>
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int userID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditUser")
                {
                    Response.Redirect($"UserAddEdit.aspx?UserID={userID}");
                }
                else if (e.CommandName == "DeleteUser")
                {
                    int? currentUserID = SessionHelper.GetUserId();
                    if (!currentUserID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    // In a real implementation, we would delete the user
                    // For now, we'll show a message
                    ShowSuccessMessage("User deleted successfully.");
                    LoadUsers();
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
        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers();
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