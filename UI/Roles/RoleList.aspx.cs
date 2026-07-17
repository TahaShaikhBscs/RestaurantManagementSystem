// ============================================
// UI/Roles/RoleList.aspx.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Roles
{
    /// <summary>
    /// Role List Page - Displays all roles with management options
    /// </summary>
    public partial class RoleList : Page
    {
        #region Private Fields

        private RoleBAL roleBAL;
        private AuditLogger auditLogger;
        private const string MODULE_NAME = "RoleManagement";

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
                LoadRoles();
            }
        }

        #endregion

        #region Initialization

        private void InitializeComponents()
        {
            roleBAL = new RoleBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Data Loading

        private void LoadRoles()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                List<Role> roles;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    roles = roleBAL.GetAllRoles();
                }
                else
                {
                    roles = roleBAL.SearchRoles(searchTerm);
                }

                gvRoles.DataSource = roles;
                gvRoles.DataBind();

                UpdateStatistics(roles);
                ltrTotalCount.Text = roles.Count.ToString();

                auditLogger.LogAction("View", MODULE_NAME, "Roles", null, $"TotalItems: {roles.Count}");
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadRoles", null, null, ex.Message);
                ShowErrorMessage($"Error loading roles: {ex.Message}");
            }
        }

        private void UpdateStatistics(List<Role> roles)
        {
            int total = roles.Count;
            int active = 0;
            int inactive = 0;

            foreach (var role in roles)
            {
                if (role.Status) active++;
                else inactive++;
            }

            ltrTotalRoles.Text = total.ToString();
            ltrActiveRoles.Text = active.ToString();
            ltrInactiveRoles.Text = inactive.ToString();
        }

        #endregion

        #region Button Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadRoles();
            auditLogger.LogSearch(MODULE_NAME, txtSearch.Text.Trim());
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadRoles();
            ShowSuccessMessage("Filters cleared.");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRoles();
            ShowSuccessMessage("Roles refreshed.");
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("RoleAddEdit.aspx");
        }

        #endregion

        #region Grid View Events

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int roleID = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditRole":
                        Response.Redirect($"RoleAddEdit.aspx?RoleID={roleID}");
                        break;
                    case "Permissions":
                        Response.Redirect($"RolePermissions.aspx?RoleID={roleID}");
                        break;
                    case "DeleteRole":
                        hfDeleteRoleID.Value = roleID.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "RowCommand", null, null, ex.Message);
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        protected void gvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoles.PageIndex = e.NewPageIndex;
            LoadRoles();
        }

        #endregion

        #region Delete Confirmation

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int roleID = Convert.ToInt32(hfDeleteRoleID.Value);
                int? userID = SessionHelper.GetUserId();

                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                bool deleted = roleBAL.DeleteRole(roleID, userID.Value);
                if (deleted)
                {
                    auditLogger.LogDelete(MODULE_NAME, "Roles", roleID, $"RoleID: {roleID}");
                    ShowSuccessMessage("Role deleted successfully.");
                    LoadRoles();

                    string closeScript = "$('#deleteModal').modal('hide');";
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", closeScript, true);
                }
                else
                {
                    ShowErrorMessage("Failed to delete role.");
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