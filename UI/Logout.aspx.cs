// ============================================
// UI/Logout.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Logout Page - Handles user logout
    /// </summary>
    public partial class Logout : Page
    {
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - logs out the user
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                // Log the logout action
                if (SessionHelper.IsLoggedIn())
                {
                    auditLogger.LogLogout();
                }

                // Clear session
                SessionHelper.DestroySession();

                // Clear authentication cookie if using forms authentication
                if (Request.Cookies[".ASPXAUTH"] != null)
                {
                    Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
                }

                // Clear remember me cookie
                if (Request.Cookies["RMS_Username"] != null)
                {
                    Response.Cookies["RMS_Username"].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }

        /// <summary>
        /// Redirects to login page
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        /// <summary>
        /// Redirects to dashboard
        /// </summary>
        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }
    }
}