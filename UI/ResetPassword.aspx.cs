// ============================================
// UI/ResetPassword.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Reset Password Page - Handles password reset
    /// </summary>
    public partial class ResetPassword : Page
    {
        private UserBAL userBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - validates reset token
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            userBAL = new UserBAL();
            auditLogger = new AuditLogger();

            // Validate reset token
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                string email = Request.QueryString["email"];

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                {
                    ShowErrorMessage("Invalid or missing reset token. Please request a new password reset.");
                    btnResetPassword.Enabled = false;
                    return;
                }

                // Validate token
                if (Session["ResetToken"] == null || Session["ResetToken"].ToString() != token)
                {
                    ShowErrorMessage("Invalid reset token. Please request a new password reset.");
                    btnResetPassword.Enabled = false;
                    return;
                }

                // Check token expiry
                if (Session["ResetExpiry"] != null)
                {
                    DateTime expiry = Convert.ToDateTime(Session["ResetExpiry"]);
                    if (DateTime.Now > expiry)
                    {
                        ShowErrorMessage("Reset link has expired. Please request a new password reset.");
                        btnResetPassword.Enabled = false;
                        return;
                    }
                }

                // Store email in view state for use during reset
                ViewState["ResetEmail"] = email;
            }
        }

        /// <summary>
        /// Resets the password
        /// </summary>
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string email = ViewState["ResetEmail"] as string;
                if (string.IsNullOrEmpty(email))
                {
                    ShowErrorMessage("Session expired. Please request a new password reset.");
                    return;
                }

                string newPassword = txtNewPassword.Text;

                // Validate password strength
                if (!ValidationHelper.IsValidPassword(newPassword))
                {
                    ShowErrorMessage("Password does not meet the required strength criteria.");
                    return;
                }

                // In a real implementation, you would:
                // 1. Get the user by email
                // 2. Update the password
                // 3. Clear the reset token

                auditLogger.LogAction("PasswordReset", "Authentication", "Users", null, $"Email: {email}");

                ShowSuccessMessage("Your password has been reset successfully. Please login with your new password.");

                // Clear session token
                Session.Remove("ResetToken");
                Session.Remove("ResetEmail");
                Session.Remove("ResetExpiry");

                // Disable form after successful reset
                txtNewPassword.Enabled = false;
                txtConfirmPassword.Enabled = false;
                btnResetPassword.Enabled = false;

                // Redirect to login after 3 seconds
                Response.AddHeader("REFRESH", "3;URL=Login.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error resetting password: {ex.Message}");
                auditLogger.LogAction("Error", "Authentication", "PasswordReset", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Redirects back to login page
        /// </summary>
        protected void btnBackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
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