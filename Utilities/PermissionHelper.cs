// ============================================
// Utilities/PermissionHelper.cs
// ============================================

using System;
using RestaurantManagementSystem.BAL;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Permission Helper - Provides permission checking methods for UI
    /// </summary>
    public class PermissionHelper
    {
        private static UserBAL userBAL;

        static PermissionHelper()
        {
            userBAL = new UserBAL();
        }

        /// <summary>
        /// Checks if current user has permission for a module and action
        /// </summary>
        /// <param name="module">Module name</param>
        /// <param name="action">Action (View, Create, Edit, Delete, Print, Export)</param>
        /// <returns>True if user has permission</returns>
        public static bool HasPermission(string module, string action)
        {
            try
            {
                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                    return false;

                return userBAL.HasPermission(userID.Value, module, action);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current user can view a module
        /// </summary>
        public static bool CanView(string module)
        {
            return HasPermission(module, "View");
        }

        /// <summary>
        /// Checks if current user can create in a module
        /// </summary>
        public static bool CanCreate(string module)
        {
            return HasPermission(module, "Create");
        }

        /// <summary>
        /// Checks if current user can edit in a module
        /// </summary>
        public static bool CanEdit(string module)
        {
            return HasPermission(module, "Edit");
        }

        /// <summary>
        /// Checks if current user can delete in a module
        /// </summary>
        public static bool CanDelete(string module)
        {
            return HasPermission(module, "Delete");
        }

        /// <summary>
        /// Checks if current user can print from a module
        /// </summary>
        public static bool CanPrint(string module)
        {
            return HasPermission(module, "Print");
        }

        /// <summary>
        /// Checks if current user can export from a module
        /// </summary>
        public static bool CanExport(string module)
        {
            return HasPermission(module, "Export");
        }

        /// <summary>
        /// Gets the module name from the current page
        /// </summary>
        public static string GetCurrentModule()
        {
            string path = System.Web.HttpContext.Current.Request.Path;
            string[] segments = path.Split('/');

            if (segments.Length >= 2)
            {
                string module = segments[segments.Length - 2];
                if (module == "UI")
                    module = "Dashboard";
                return module;
            }
            return "Dashboard";
        }
    }
}