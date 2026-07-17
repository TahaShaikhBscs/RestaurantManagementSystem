// ============================================
// Global.asax.cs
// ============================================

using System;
using System.Web;
using System.Web.SessionState;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Global Application Class - Handles application-level events
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// Application start event - initializes application
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            // Log application start
            System.Diagnostics.Debug.WriteLine("Application Started");
        }

        /// <summary>
        /// Session start event - creates new session
        /// </summary>
        protected void Session_Start(object sender, EventArgs e)
        {
            // Initialize session variables
            Session["LastActivity"] = DateTime.Now;
            Session["SessionTimeout"] = 30; // Default timeout in minutes
        }

        /// <summary>
        /// Application end event - cleans up application resources
        /// </summary>
        protected void Application_End(object sender, EventArgs e)
        {
            // Log application end
            System.Diagnostics.Debug.WriteLine("Application Ended");
        }

        /// <summary>
        /// Session end event - cleans up session resources
        /// </summary>
        protected void Session_End(object sender, EventArgs e)
        {
            // Clean up session
            Session.Clear();
        }

        /// <summary>
        /// Application error event - handles unhandled exceptions
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            //// Get the last error
            //Exception ex = Server.GetLastError();

            //// Log the error
            //System.Diagnostics.Debug.WriteLine($"Unhandled Error: {ex.Message}");
            //System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

            //// Clear the error
            //Server.ClearError();

            //// Redirect to error page
            //Response.Redirect("Error.aspx");
        }

        /// <summary>
        /// Authenticates request - validates user session
        /// </summary>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Check if request is for protected resource
            string path = Request.Path.ToLower();

            // Skip authentication for login, assets, and error pages
            if (path.Contains("/login.aspx") ||
                path.Contains("/error.aspx") ||
                path.Contains("/forgotpassword.aspx") ||
                path.Contains("/resetpassword.aspx") ||
                path.Contains("/content/") ||
                path.Contains("/scripts/") ||
                path.Contains("/fonts/") ||
                path.Contains("/images/") ||
                path.Contains("/css/") ||
                path.Contains("/js/"))
            {
                return;
            }

            // Check if user is authenticated
            if (Context.Session != null && Context.Session["UserID"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
            }
        }
    }
}