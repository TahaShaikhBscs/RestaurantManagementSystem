// ============================================
// UI/Login.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.BAL;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Login Page Code-Behind
    /// Handles user authentication and session management
    /// </summary>
    public partial class Login : Page
    {
        private UserBAL userBAL;

        /// <summary>
        /// Page load event - checks for timeout parameter
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            userBAL = new UserBAL();

            // Check if user is already logged in
            if (Session["UserID"] != null)
            {
                Response.Redirect("Dashboard.aspx");
            }

            // Check for timeout parameter
            if (Request.QueryString["timeout"] == "true")
            {
                ShowErrorMessage("Your session has expired. Please login again.");
            }

            // Check for logout parameter
            if (Request.QueryString["logout"] == "true")
            {
                ShowSuccessMessage("You have been logged out successfully.");
            }

            // Set focus to username field
            txtUsername.Focus();
        }

        /// <summary>
        /// Login button click handler - authenticates user
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate page
                if (!Page.IsValid)
                    return;

                // Get credentials
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Authenticate user
                User user = userBAL.AuthenticateUser(username, password);

                if (user != null)
                {
                    // Create session
                    Session["UserID"] = user.UserID;
                    Session["Username"] = user.Username;
                    Session["User"] = user;
                    Session["CompanyID"] = user.CompanyID;
                    Session["BranchID"] = user.BranchID;
                    Session["RoleID"] = user.RoleID;
                    Session["SessionTimeout"] = user.SessionTimeout;
                    Session["LastActivity"] = DateTime.Now;

                    // Set remember me cookie if checked
                    if (chkRememberMe.Checked)
                    {
                        Response.Cookies["RMS_Username"].Value = username;
                        Response.Cookies["RMS_Username"].Expires = DateTime.Now.AddDays(30);
                    }

                    // Redirect to dashboard
                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    ShowErrorMessage("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                // Log exception
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            ltrErrorMessage.Text = message;
        }

        /// <summary>
        /// Shows a success message
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            ltrSuccessMessage.Text = message;
        }
    }
}