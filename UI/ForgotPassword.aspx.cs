// ============================================
// UI/ForgotPassword.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Forgot Password Page - Handles password reset requests
    /// </summary>
    public partial class ForgotPassword : Page
    {
        private UserBAL userBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - initializes the page
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            userBAL = new UserBAL();
            auditLogger = new AuditLogger();
        }

        /// <summary>
        /// Sends password reset link to the user's email
        /// </summary>
        protected void btnSendResetLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string email = txtEmail.Text.Trim();

                // Check if user exists with this email
                // In a real implementation, you would have a method to get user by email
                // For now, we'll simulate the process

                // Generate reset token
                string resetToken = EncryptionHelper.GenerateToken();

                // Store token in session or database with expiration
                // For now, we'll use session
                Session["ResetToken"] = resetToken;
                Session["ResetEmail"] = email;
                Session["ResetExpiry"] = DateTime.Now.AddHours(24);

                // In a real implementation, send email with reset link
                // For demo, we'll just show success message

                auditLogger.LogAction("PasswordResetRequest", "Authentication", "Users", null, $"Email: {email}");

                ShowSuccessMessage("A password reset link has been sent to your email address.");
                txtEmail.Text = string.Empty;

                // In a real implementation, you would send an email with:
                // string resetLink = $"{Request.Url.Scheme}://{Request.Url.Authority}/ResetPassword.aspx?token={resetToken}&email={email}";
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error sending reset link: {ex.Message}");
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