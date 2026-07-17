// ============================================
// UI/Roles/RolePermissions.aspx.cs - Updated
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
    /// Role Permissions Page - Manages permissions for a role
    /// </summary>
    public partial class RolePermissions : Page
    {
        #region Private Fields

        private RoleBAL roleBAL;
        private AuditLogger auditLogger;
        private int roleID = 0;
        private List<RolePermission> permissions;
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
                LoadRoleInfo();
                LoadPermissions();
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

        private void LoadRoleInfo()
        {
            try
            {
                Role role = roleBAL.GetRoleById(roleID);
                if (role != null)
                {
                    ltrRoleName.Text = role.RoleName;
                    ltrRoleDisplay.Text = role.RoleName;
                }
                else
                {
                    ShowErrorMessage("Role not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading role: {ex.Message}");
            }
        }

        private void LoadPermissions()
        {
            try
            {
                permissions = roleBAL.GetRolePermissions(roleID);
                gvPermissions.DataSource = GetDefaultPermissions();
                gvPermissions.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading permissions: {ex.Message}");
            }
        }

        private List<RolePermission> GetDefaultPermissions()
        {
            string[] modules = {
                "Dashboard", "POS", "Orders", "Kitchen", "Customers",
                "Products", "Categories", "Tables", "Expenses",
                "Suppliers", "Purchases", "Reports", "Settings",
                "Deals", "Users", "Roles", "Companies", "Branches"
            };

            List<RolePermission> defaultPermissions = new List<RolePermission>();

            foreach (string module in modules)
            {
                RolePermission perm = new RolePermission
                {
                    ModuleName = module,
                    CanView = false,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CanPrint = false,
                    CanExport = false
                };

                if (permissions != null)
                {
                    foreach (var existing in permissions)
                    {
                        if (existing.ModuleName == module)
                        {
                            perm.CanView = existing.CanView;
                            perm.CanCreate = existing.CanCreate;
                            perm.CanEdit = existing.CanEdit;
                            perm.CanDelete = existing.CanDelete;
                            perm.CanPrint = existing.CanPrint;
                            perm.CanExport = existing.CanExport;
                            break;
                        }
                    }
                }

                defaultPermissions.Add(perm);
            }

            return defaultPermissions;
        }

        #endregion

        #region Grid View Events

        protected void gvPermissions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RolePermission permission = (RolePermission)e.Row.DataItem;

                CheckBox chkView = (CheckBox)e.Row.FindControl("chkView");
                CheckBox chkCreate = (CheckBox)e.Row.FindControl("chkCreate");
                CheckBox chkEdit = (CheckBox)e.Row.FindControl("chkEdit");
                CheckBox chkDelete = (CheckBox)e.Row.FindControl("chkDelete");
                CheckBox chkPrint = (CheckBox)e.Row.FindControl("chkPrint");

                if (chkView != null) chkView.Checked = permission.CanView;
                if (chkCreate != null) chkCreate.Checked = permission.CanCreate;
                if (chkEdit != null) chkEdit.Checked = permission.CanEdit;
                if (chkDelete != null) chkDelete.Checked = permission.CanDelete;
                if (chkPrint != null) chkPrint.Checked = permission.CanPrint;
            }
        }

        #endregion

        #region Button Events

        protected void btnSavePermissions_Click(object sender, EventArgs e)
        {
            try
            {
                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                List<RolePermission> updatedPermissions = new List<RolePermission>();

                foreach (GridViewRow row in gvPermissions.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string moduleName = row.Cells[0].Text;

                        CheckBox chkView = (CheckBox)row.FindControl("chkView");
                        CheckBox chkCreate = (CheckBox)row.FindControl("chkCreate");
                        CheckBox chkEdit = (CheckBox)row.FindControl("chkEdit");
                        CheckBox chkDelete = (CheckBox)row.FindControl("chkDelete");
                        CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");

                        RolePermission perm = new RolePermission
                        {
                            RoleID = roleID,
                            ModuleName = moduleName,
                            CanView = chkView != null && chkView.Checked,
                            CanCreate = chkCreate != null && chkCreate.Checked,
                            CanEdit = chkEdit != null && chkEdit.Checked,
                            CanDelete = chkDelete != null && chkDelete.Checked,
                            CanPrint = chkPrint != null && chkPrint.Checked,
                            CanExport = false,
                            CreatedBy = userID.Value
                        };

                        updatedPermissions.Add(perm);
                    }
                }

                bool saved = roleBAL.SaveRolePermissions(roleID, updatedPermissions, userID.Value);
                if (saved)
                {
                    auditLogger.LogUpdate(MODULE_NAME, "RolePermissions", roleID,
                        $"RoleID: {roleID}", "Permissions updated");
                    ShowSuccessMessage("Permissions saved successfully.");
                }
                else
                {
                    ShowErrorMessage("Failed to save permissions.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving permissions: {ex.Message}");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("RoleList.aspx");
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