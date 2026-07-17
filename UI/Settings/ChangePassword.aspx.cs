// ============================================
// UI/Settings/ChangePassword.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Settings
{
    /// <summary>
    /// Change Password Page - Allows users to change their password
    /// </summary>
    public partial class ChangePassword : Page
    {
        private UserBAL userBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - initializes the page
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
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        protected void btnChangePassword_Click(object sender, EventArgs e)
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

                string currentPassword = txtCurrentPassword.Text;
                string newPassword = txtNewPassword.Text;

                // Validate password strength
                if (!ValidationHelper.IsValidPassword(newPassword))
                {
                    ShowErrorMessage("Password does not meet the required strength criteria.");
                    return;
                }

                bool changed = userBAL.ChangePassword(userID.Value, currentPassword, newPassword, userID.Value);
                if (changed)
                {
                    auditLogger.LogUpdate("UserManagement", "Users", userID.Value,
                        $"UserID: {userID}", "Password changed");
                    ShowSuccessMessage("Password changed successfully.");

                    // Clear form
                    txtCurrentPassword.Text = string.Empty;
                    txtNewPassword.Text = string.Empty;
                    txtConfirmPassword.Text = string.Empty;
                }
                else
                {
                    ShowErrorMessage("Failed to change password. Please check your current password.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error changing password: {ex.Message}");
                auditLogger.LogAction("Error", "UserManagement", "ChangePassword", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Redirects back to profile page
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
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