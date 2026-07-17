// ============================================
// Utilities/ToastHelper.cs
// ============================================

using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Toast Notification Helper - Displays toast notifications in UI
    /// </summary>
    public class ToastHelper
    {
        /// <summary>
        /// Shows a toast notification
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="message">Message to display</param>
        /// <param name="type">Toast type (success, error, warning, info)</param>
        public static void ShowToast(Page page, string message, string type = "success")
        {
            string icon = GetIcon(type);
            string color = GetColor(type);

            string script = $@"
                $(document).ready(function() {{
                    var toastHtml = `
                        <div class='toast align-items-center text-white bg-{color} border-0' role='alert' aria-live='assertive' aria-atomic='true'>
                            <div class='d-flex'>
                                <div class='toast-body'>
                                    <i class='fas {icon} me-2'></i>
                                    {message.Replace("'", "\\'")}
                                </div>
                                <button type='button' class='btn-close btn-close-white me-2 m-auto' data-bs-dismiss='toast'></button>
                            </div>
                        </div>
                    `;
                    
                    $('.toast-container').append(toastHtml);
                    var toast = $('.toast-container .toast:last');
                    var bsToast = new bootstrap.Toast(toast);
                    bsToast.show();
                    
                    setTimeout(function() {{
                        toast.remove();
                    }}, 5000);
                }});
            ";

            page.ClientScript.RegisterStartupScript(
                page.GetType(),
                "ToastScript",
                script,
                true
            );
        }

        /// <summary>
        /// Shows a success toast
        /// </summary>
        public static void ShowSuccess(Page page, string message)
        {
            ShowToast(page, message, "success");
        }

        /// <summary>
        /// Shows an error toast
        /// </summary>
        public static void ShowError(Page page, string message)
        {
            ShowToast(page, message, "danger");
        }

        /// <summary>
        /// Shows a warning toast
        /// </summary>
        public static void ShowWarning(Page page, string message)
        {
            ShowToast(page, message, "warning");
        }

        /// <summary>
        /// Shows an info toast
        /// </summary>
        public static void ShowInfo(Page page, string message)
        {
            ShowToast(page, message, "info");
        }

        private static string GetIcon(string type)
        {
            switch (type)
            {
                case "success": return "fa-check-circle";
                case "danger": return "fa-exclamation-circle";
                case "warning": return "fa-exclamation-triangle";
                case "info": return "fa-info-circle";
                default: return "fa-info-circle";
            }
        }

        private static string GetColor(string type)
        {
            switch (type)
            {
                case "success": return "success";
                case "danger": return "danger";
                case "warning": return "warning";
                case "info": return "info";
                default: return "primary";
            }
        }
    }
}