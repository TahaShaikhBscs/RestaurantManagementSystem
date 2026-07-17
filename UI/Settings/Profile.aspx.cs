// ============================================
// UI/Settings/Profile.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Settings
{
    /// <summary>
    /// User Profile Page - View and edit user profile
    /// </summary>
    public partial class Profile : Page
    {
        private UserBAL userBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads user profile
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            userBAL = new UserBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        /// <summary>
        /// Loads user profile data
        /// </summary>
        private void LoadProfile()
        {
            try
            {
                int? userID = SessionHelper.GetUserId();
                if (userID.HasValue)
                {
                    User user = userBAL.GetUserById(userID.Value);
                    if (user != null)
                    {
                        txtUsername.Text = user.Username;
                        txtFullName.Text = user.FullName;
                        txtEmail.Text = user.Email;
                        txtPhone.Text = user.Phone;
                        txtRole.Text = user.RoleName;
                        txtBranch.Text = user.BranchName;
                        txtSessionTimeout.Text = user.SessionTimeout.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading profile: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves profile updates
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("Session expired. Please login again.");
                    return;
                }

                User user = userBAL.GetUserById(userID.Value);
                if (user != null)
                {
                    user.FullName = txtFullName.Text.Trim();
                    user.Email = txtEmail.Text.Trim();
                    user.Phone = txtPhone.Text.Trim();
                    user.SessionTimeout = Convert.ToInt32(txtSessionTimeout.Text);
                    user.UpdatedBy = userID.Value;

                    int savedID = userBAL.SaveUser(user);
                    if (savedID > 0)
                    {
                        // Update session
                        Session["User"] = user;
                        auditLogger.LogUpdate("UserManagement", "Users", savedID,
                            $"UserID: {userID}", "Profile updated");
                        ShowSuccessMessage("Profile updated successfully.");
                    }
                    else
                    {
                        ShowErrorMessage("Failed to update profile.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error updating profile: {ex.Message}");
            }
        }

        /// <summary>
        /// Redirects to change password page
        /// </summary>
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx");
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