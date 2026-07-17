// ============================================
// UI/Roles/RoleAddEdit.aspx.cs - Updated
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Roles
{
    /// <summary>
    /// Role Add/Edit Page - Handles adding and editing roles
    /// </summary>
    public partial class RoleAddEdit : Page
    {
        #region Private Fields

        private RoleBAL roleBAL;
        private AuditLogger auditLogger;
        private int roleID = 0;
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

            if (Request.QueryString["RoleID"] != null)
            {
                roleID = Convert.ToInt32(Request.QueryString["RoleID"]);
            }

            if (!IsPostBack)
            {
                if (roleID > 0)
                {
                    SetEditMode();
                    LoadRoleData(roleID);
                }
                else
                {
                    SetAddMode();
                }
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

        #region Mode Configuration

        private void SetEditMode()
        {
            ltrPageTitle.Text = "Edit Role";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Role Information";
            btnSave.Text = "Update Role";
            btnSaveAndContinue.Visible = true;
        }

        private void SetAddMode()
        {
            ltrPageTitle.Text = "Add New Role";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Add New Role";
            btnSave.Text = "Save Role";
            btnSaveAndContinue.Visible = false;
            chkStatus.Checked = true;
        }

        #endregion

        #region Data Loading

        private void LoadRoleData(int id)
        {
            try
            {
                Role role = roleBAL.GetRoleById(id);
                if (role != null)
                {
                    txtRoleName.Text = role.RoleName;
                    txtDescription.Text = role.Description;
                    chkStatus.Checked = role.Status;
                    UpdateStatusText();

                    auditLogger.LogAction("View", MODULE_NAME, "Role", id, $"RoleName: {role.RoleName}");
                }
                else
                {
                    ShowErrorMessage("Role not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadRoleData", id, null, ex.Message);
                ShowErrorMessage($"Error loading role: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkStatus.Checked ? "Active" : "Inactive";
        }

        private bool ValidateRole()
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                ShowErrorMessage("Role name is required.");
                return false;
            }

            return true;
        }

        #endregion

        #region Button Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRole(false);
        }

        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveRole(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("RoleList.aspx");
        }

        #endregion

        #region Business Logic

        private void SaveRole(bool continueEditing)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidateRole())
                    return;

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                Role role = new Role
                {
                    RoleID = roleID,
                    RoleName = txtRoleName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = roleBAL.SaveRole(role);

                if (savedID > 0)
                {
                    if (roleID > 0)
                    {
                        auditLogger.LogUpdate(MODULE_NAME, "Roles", savedID,
                            $"RoleID: {roleID}", $"RoleName: {role.RoleName}");
                        ShowSuccessMessage("Role updated successfully.");
                    }
                    else
                    {
                        auditLogger.LogInsert(MODULE_NAME, "Roles", savedID,
                            $"RoleName: {role.RoleName}");
                        ShowSuccessMessage("Role added successfully.");
                    }

                    if (continueEditing)
                    {
                        Response.Redirect($"RoleAddEdit.aspx?RoleID={savedID}");
                    }
                    else
                    {
                        Response.AddHeader("REFRESH", "2;URL=RoleList.aspx");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to save role.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveRole", roleID, null, ex.Message);
                ShowErrorMessage($"Error saving role: {ex.Message}");
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