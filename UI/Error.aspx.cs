// ============================================
// UI/Error.aspx.cs
// ============================================

using System;
using System.Web.UI;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Error Page - Displays user-friendly error messages
    /// </summary>
    public partial class Error : Page
    {
        /// <summary>
        /// Page load event - displays error details
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if error details were passed
            if (Session["LastError"] != null)
            {
                Exception ex = Session["LastError"] as Exception;
                if (ex != null)
                {
                    ShowErrorDetails(ex);
                }
                Session.Remove("LastError");
            }

            // Check for error code in query string
            if (Request.QueryString["code"] != null)
            {
                string code = Request.QueryString["code"];
                ShowErrorCodeMessage(code);
            }
        }

        /// <summary>
        /// Shows error details
        /// </summary>
        private void ShowErrorDetails(Exception ex)
        {
            if (ex != null)
            {
                // For development, show details
                // In production, only show in debug mode
#if DEBUG
                pnlErrorDetails.Visible = true;
                ltrErrorDetails.Text = $"{ex.Message}\n\n{ex.StackTrace}";
                ltrErrorMessage.Text = "An error occurred while processing your request.";
#else
                ltrErrorMessage.Text = "An unexpected error occurred. Please try again later.";
#endif
            }
        }

        /// <summary>
        /// Shows error based on error code
        /// </summary>
        private void ShowErrorCodeMessage(string code)
        {
            switch (code)
            {
                case "404":
                    ltrErrorMessage.Text = "The page you are looking for could not be found.";
                    break;
                case "403":
                    ltrErrorMessage.Text = "You do not have permission to access this page.";
                    break;
                case "500":
                    ltrErrorMessage.Text = "An internal server error occurred. Please try again later.";
                    break;
                default:
                    ltrErrorMessage.Text = "An error occurred. Please try again later.";
                    break;
            }
        }

        /// <summary>
        /// Navigates to dashboard
        /// </summary>
        protected void btnGoHome_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                Response.Redirect("Dashboard.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        /// <summary>
        /// Goes back to previous page
        /// </summary>
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null)
            {
                Response.Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                Response.Redirect("Dashboard.aspx");
            }
        }
    }
}