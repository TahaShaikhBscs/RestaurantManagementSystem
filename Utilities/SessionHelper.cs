// ============================================
// Utilities/SessionHelper.cs
// ============================================

using System;
using System.Web;
using System.Web.SessionState;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Session Helper - Manages user session state
    /// Handles session creation, validation, and management
    /// </summary>
    public class SessionHelper
    {
        private const string USER_ID_KEY = "UserID";
        private const string USERNAME_KEY = "Username";
        private const string USER_KEY = "User";
        private const string COMPANY_ID_KEY = "CompanyID";
        private const string BRANCH_ID_KEY = "BranchID";
        private const string ROLE_ID_KEY = "RoleID";
        private const string LAST_ACTIVITY_KEY = "LastActivity";
        private const string SESSION_TIMEOUT_KEY = "SessionTimeout";

        /// <summary>
        /// Gets the current HTTP session
        /// </summary>
        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        /// <summary>
        /// Creates a new user session
        /// </summary>
        /// <param name="user">User object</param>
        public static void CreateSession(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            Session[USER_ID_KEY] = user.UserID;
            Session[USERNAME_KEY] = user.Username;
            Session[USER_KEY] = user;
            Session[COMPANY_ID_KEY] = user.CompanyID;
            Session[BRANCH_ID_KEY] = user.BranchID;
            Session[ROLE_ID_KEY] = user.RoleID;
            Session[SESSION_TIMEOUT_KEY] = user.SessionTimeout;
            Session[LAST_ACTIVITY_KEY] = DateTime.Now;
        }

        /// <summary>
        /// Gets the current user ID
        /// </summary>
        /// <returns>User ID or null if not logged in</returns>
        public static int? GetUserId()
        {
            return Session[USER_ID_KEY] as int?;
        }

        /// <summary>
        /// Gets the current username
        /// </summary>
        /// <returns>Username or null if not logged in</returns>
        public static string GetUsername()
        {
            return Session[USERNAME_KEY] as string;
        }

        /// <summary>
        /// Gets the current user object
        /// </summary>
        /// <returns>User object or null if not logged in</returns>
        public static User GetCurrentUser()
        {
            return Session[USER_KEY] as User;
        }

        /// <summary>
        /// Gets the current company ID
        /// </summary>
        /// <returns>Company ID or null if not logged in</returns>
        public static int? GetCompanyId()
        {
            return Session[COMPANY_ID_KEY] as int?;
        }

        /// <summary>
        /// Gets the current branch ID
        /// </summary>
        /// <returns>Branch ID or null if not logged in</returns>
        public static int? GetBranchId()
        {
            return Session[BRANCH_ID_KEY] as int?;
        }

        /// <summary>
        /// Gets the current role ID
        /// </summary>
        /// <returns>Role ID or null if not logged in</returns>
        public static int? GetRoleId()
        {
            return Session[ROLE_ID_KEY] as int?;
        }

        /// <summary>
        /// Checks if user is logged in
        /// </summary>
        /// <returns>True if user is logged in</returns>
        public static bool IsLoggedIn()
        {
            return Session[USER_ID_KEY] != null;
        }

        /// <summary>
        /// Checks if session is valid (not expired)
        /// </summary>
        /// <returns>True if session is valid</returns>
        public static bool IsSessionValid()
        {
            if (!IsLoggedIn())
                return false;

            DateTime? lastActivity = Session[LAST_ACTIVITY_KEY] as DateTime?;
            if (!lastActivity.HasValue)
                return false;

            int timeout = Session[SESSION_TIMEOUT_KEY] as int? ?? 30;
            TimeSpan idleTime = DateTime.Now - lastActivity.Value;

            return idleTime.TotalMinutes <= timeout;
        }

        /// <summary>
        /// Updates the last activity time
        /// </summary>
        public static void UpdateLastActivity()
        {
            if (IsLoggedIn())
            {
                Session[LAST_ACTIVITY_KEY] = DateTime.Now;
            }
        }

        /// <summary>
        /// Destroys the current session
        /// </summary>
        public static void DestroySession()
        {
            Session.Clear();
            Session.Abandon();
        }

        /// <summary>
        /// Gets session timeout in minutes
        /// </summary>
        /// <returns>Session timeout</returns>
        public static int GetSessionTimeout()
        {
            return Session[SESSION_TIMEOUT_KEY] as int? ?? 30;
        }
    }
}