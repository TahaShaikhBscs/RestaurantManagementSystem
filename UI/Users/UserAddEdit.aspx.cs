// ============================================
// UI/Users/UserAddEdit.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// IMPORTANT: Use model aliases to avoid namespace conflicts
using UserModel = RestaurantManagementSystem.Models.User;
using BranchModel = RestaurantManagementSystem.Models.Branch;
using RoleModel = RestaurantManagementSystem.Models.Role;
using System.Web.UI.WebControls;

namespace RestaurantManagementSystem.UI.Users
{
    /// <summary>
    /// User Add/Edit Page - Handles adding and editing users
    /// </summary>
    public partial class UserAddEdit : Page
    {
        private UserBAL userBAL;
        private BranchBAL branchBAL;
        private RoleBAL roleBAL;
        private AuditLogger auditLogger;
        private int userID = 0;

        /// <summary>
        /// Page load event - loads user data for editing
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
            roleBAL = new RoleBAL();
            auditLogger = new AuditLogger();

            if (Request.QueryString["UserID"] != null)
            {
                userID = Convert.ToInt32(Request.QueryString["UserID"]);
            }

            if (!IsPostBack)
            {
                LoadBranches();
                LoadRoles();

                if (userID > 0)
                {
                    ltrPageTitle.Text = "Edit User";
                    btnSave.Text = "Update User";
                    divPassword.Visible = false;
                    LoadUserData(userID);
                }
                else
                {
                    ltrPageTitle.Text = "Add New User";
                    btnSave.Text = "Save User";
                    divPassword.Visible = true;
                    chkIsActive.Checked = true;
                    txtSessionTimeout.Text = "30";
                }
            }
        }

        /// <summary>
        /// Loads branches into the dropdown
        /// </summary>
        private void LoadBranches()
        {
            try
            {
                int? companyID = SessionHelper.GetCompanyId();
                if (companyID.HasValue)
                {
                    List<BranchModel> branches = branchBAL.GetBranchesByCompany(companyID.Value);
                    ddlBranch.DataSource = branches;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchID";
                    ddlBranch.DataBind();
                    ddlBranch.Items.Insert(0, new ListItem("-- Select Branch --", "0"));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading branches: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads roles into the dropdown
        /// </summary>
        private void LoadRoles()
        {
            try
            {
                List<RoleModel> roles = roleBAL.GetAllRoles();
                ddlRole.DataSource = roles;
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("-- Select Role --", "0"));
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading roles: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads user data for editing
        /// </summary>
        private void LoadUserData(int id)
        {
            try
            {
                UserModel user = userBAL.GetUserById(id);
                if (user != null)
                {
                    txtUsername.Text = user.Username;
                    txtFullName.Text = user.FullName;
                    txtEmail.Text = user.Email;
                    txtPhone.Text = user.Phone;
                    ddlBranch.SelectedValue = user.BranchID.ToString();
                    ddlRole.SelectedValue = user.RoleID.ToString();
                    txtSessionTimeout.Text = user.SessionTimeout.ToString();
                    chkIsActive.Checked = user.IsActive;
                    UpdateStatusText();
                }
                else
                {
                    ShowErrorMessage("User not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading user: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status text based on checkbox
        /// </summary>
        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkIsActive.Checked ? "Active" : "Inactive";
        }

        /// <summary>
        /// Saves the user
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                int? currentUserID = SessionHelper.GetUserId();
                if (!currentUserID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                int? companyID = SessionHelper.GetCompanyId();
                if (!companyID.HasValue)
                {
                    ShowErrorMessage("Company not found in session.");
                    return;
                }

                UserModel user = new UserModel
                {
                    UserID = userID,
                    CompanyID = companyID.Value,
                    BranchID = Convert.ToInt32(ddlBranch.SelectedValue),
                    RoleID = Convert.ToInt32(ddlRole.SelectedValue),
                    Username = txtUsername.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    SessionTimeout = Convert.ToInt32(txtSessionTimeout.Text),
                    IsActive = chkIsActive.Checked,
                    Status = true,
                    CreatedBy = currentUserID.Value,
                    UpdatedBy = currentUserID.Value
                };

                // For new users, set password
                if (userID == 0)
                {
                    string password = txtPassword.Text;
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        ShowErrorMessage("Password is required for new users.");
                        return;
                    }
                    // Password will be hashed in BAL
                }

                int savedID = userBAL.SaveUser(user);

                if (userID > 0)
                {
                    auditLogger.LogUpdate("UserManagement", "Users", savedID,
                        $"UserID: {userID}", $"Username: {user.Username}");
                    ShowSuccessMessage("User updated successfully.");
                }
                else
                {
                    auditLogger.LogInsert("UserManagement", "Users", savedID,
                        $"Username: {user.Username}");
                    ShowSuccessMessage("User added successfully.");
                }

                Response.AddHeader("REFRESH", "2;URL=UserList.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving user: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserList.aspx");
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