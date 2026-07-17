// ============================================
// UI/MasterPage.master.cs
// ============================================

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.BAL;

namespace RestaurantManagementSystem.UI
{
    /// <summary>
    /// Master Page Code-Behind
    /// Handles user session validation and navigation
    /// </summary>
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private UserBAL userBAL;

        /// <summary>
        /// Page load event - validates user session
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            userBAL = new UserBAL();

            // Check if user is logged in
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Validate user session
            int userID = Convert.ToInt32(Session["UserID"]);
            User user = userBAL.ValidateUserSession(userID);

            if (user == null)
            {
                // User is no longer valid, log out
                Session.Clear();
                Response.Redirect("Login.aspx");
                return;
            }

            // Display user name in navigation
            ltrUserName.Text = HttpUtility.HtmlEncode(user.FullName);

            // Store user info in session
            Session["User"] = user;

            // Check session timeout
            CheckSessionTimeout();
        }

        /// <summary>
        /// Keeps file downloads as normal postbacks because an XMLHttpRequest cannot prompt a browser download.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            RegisterExportControlsForFullPostback(MainContent);
            base.OnPreRender(e);
        }

        private void RegisterExportControlsForFullPostback(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                Button button = control as Button;
                if (button != null &&
                    (button.ID.Equals("btnExport", StringComparison.OrdinalIgnoreCase) ||
                     button.ID.EndsWith("Excel", StringComparison.OrdinalIgnoreCase)))
                {
                    ScriptManager.GetCurrent(Page).RegisterPostBackControl(button);
                }

                RegisterExportControlsForFullPostback(control);
            }
        }

        /// <summary>
        /// Checks if session has expired based on timeout
        /// </summary>
        private void CheckSessionTimeout()
        {
            if (Session["LastActivity"] != null)
            {
                DateTime lastActivity = (DateTime)Session["LastActivity"];
                TimeSpan idleTime = DateTime.Now - lastActivity;

                // Get timeout from session or use default 30 minutes
                int timeout = Session["SessionTimeout"] != null ?
                    Convert.ToInt32(Session["SessionTimeout"]) : 30;

                if (idleTime.TotalMinutes > timeout)
                {
                    // Session expired
                    Session.Clear();
                    Response.Redirect("Login.aspx?timeout=true");
                }
            }

            // Update last activity time
            Session["LastActivity"] = DateTime.Now;
        }

        /// <summary>
        /// Checks if user has permission for a specific module and action
        /// </summary>
        //public bool HasPermission(string moduleName, string action)
        //{
        //    if (Session["UserID"] == null)
        //        return false;

        //    int userID = Convert.ToInt32(Session["UserID"]);
        //    return userBAL.HasPermission(userID, moduleName, action);
        //}

        /// <summary>
        /// Checks if user can view a module
        /// </summary>
        public bool CanView(string moduleName)
        {
            return HasPermission(moduleName, "View");
        }

        /// <summary>
        /// Checks if user can create in a module
        /// </summary>
        public bool CanCreate(string moduleName)
        {
            return HasPermission(moduleName, "Create");
        }

        /// <summary>
        /// Checks if user can edit in a module
        /// </summary>
        public bool CanEdit(string moduleName)
        {
            return HasPermission(moduleName, "Edit");
        }

        /// <summary>
        /// Checks if user can delete in a module
        /// </summary>
        public bool CanDelete(string moduleName)
        {
            return HasPermission(moduleName, "Delete");
        }

        /// <summary>
        /// Checks if user has permission for a specific module and action
        /// </summary>
        public bool HasPermission(string moduleName, string action)
        {
            if (Session["UserID"] == null)
                return false;

            int userID = Convert.ToInt32(Session["UserID"]);
            return userBAL.HasPermission(userID, moduleName, action);
        }
    }
}
