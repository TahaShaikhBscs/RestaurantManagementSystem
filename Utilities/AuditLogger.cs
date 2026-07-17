// ============================================
// Utilities/AuditLogger.cs
// ============================================

using System;
using System.Web;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Audit Logger - Handles system audit logging
    /// Logs all user actions for security and compliance
    /// </summary>
    public class AuditLogger
    {
        private readonly AuditLogDAL auditLogDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public AuditLogger()
        {
            auditLogDAL = new AuditLogDAL();
        }

        /// <summary>
        /// Logs an action to the audit log
        /// </summary>
        /// <param name="actionType">Type of action (Insert, Update, Delete, Login, etc.)</param>
        /// <param name="module">Module name</param>
        /// <param name="tableName">Table name</param>
        /// <param name="recordID">Record ID</param>
        /// <param name="oldValues">Old values (JSON)</param>
        /// <param name="newValues">New values (JSON)</param>
        public void LogAction(string actionType, string module, string tableName, int? recordID = null,
                              string oldValues = null, string newValues = null)
        {
            try
            {
                // Get current user info
                int? userID = SessionHelper.GetUserId();
                int? companyID = SessionHelper.GetCompanyId();
                int? branchID = SessionHelper.GetBranchId();

                // Get IP address and user agent
                string ipAddress = GetClientIPAddress();
                string userAgent = HttpContext.Current?.Request?.UserAgent;

                AuditLog auditLog = new AuditLog
                {
                    CompanyID = companyID ?? 0,
                    BranchID = branchID ?? 0,
                    UserID = userID ?? 0,
                    ActionType = actionType,
                    Module = module,
                    TableName = tableName,
                    RecordID = recordID,
                    OldValues = oldValues,
                    NewValues = newValues,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    ActionDate = DateTime.Now
                };

                auditLogDAL.InsertAuditLog(auditLog);
            }
            catch (Exception ex)
            {
                // Log error but don't throw - audit logging shouldn't break the application
                System.Diagnostics.Debug.WriteLine($"Audit Log Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs a login action
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="success">Whether login was successful</param>
        public void LogLogin(string username, bool success)
        {
            try
            {
                string actionType = success ? "Login" : "LoginFailed";
                LogAction(actionType, "Authentication", "Users", null,
                          $"Username: {username}", $"Success: {success}");
            }
            catch
            {
                // Silently fail
            }
        }

        /// <summary>
        /// Logs a logout action
        /// </summary>
        public void LogLogout()
        {
            try
            {
                LogAction("Logout", "Authentication", "Users", SessionHelper.GetUserId());
            }
            catch
            {
                // Silently fail
            }
        }

        /// <summary>
        /// Logs an insert action
        /// </summary>
        public void LogInsert(string module, string tableName, int recordID, string newValues)
        {
            LogAction("Insert", module, tableName, recordID, null, newValues);
        }

        /// <summary>
        /// Logs an update action
        /// </summary>
        public void LogUpdate(string module, string tableName, int recordID, string oldValues, string newValues)
        {
            LogAction("Update", module, tableName, recordID, oldValues, newValues);
        }

        /// <summary>
        /// Logs a delete action
        /// </summary>
        public void LogDelete(string module, string tableName, int recordID, string oldValues)
        {
            LogAction("Delete", module, tableName, recordID, oldValues, null);
        }

        /// <summary>
        /// Logs a print action
        /// </summary>
        public void LogPrint(string module, string tableName, int recordID)
        {
            LogAction("Print", module, tableName, recordID);
        }

        /// <summary>
        /// Logs an export action
        /// </summary>
        public void LogExport(string module, string tableName, int? recordID = null, string format = null)
        {
            LogAction("Export", module, tableName, recordID, null, $"Format: {format}");
        }

        /// <summary>
        /// Logs a view action
        /// </summary>
        public void LogView(string module, string tableName, int? recordID = null)
        {
            LogAction("View", module, tableName, recordID);
        }

        /// <summary>
        /// Logs a search action
        /// </summary>
        public void LogSearch(string module, string searchTerm)
        {
            LogAction("Search", module, module, null, null, $"Search Term: {searchTerm}");
        }

        /// <summary>
        /// Logs a login attempt
        /// </summary>
        public void LogLoginAttempt(string username, bool success, string ipAddress)
        {
            LogAction(success ? "LoginSuccess" : "LoginFailed", "Authentication", "Users", null,
                $"Username: {username}", $"IP: {ipAddress}, Success: {success}");
        }

        /// <summary>
        /// Logs a password reset request
        /// </summary>
        public void LogPasswordResetRequest(string email)
        {
            LogAction("PasswordResetRequest", "Authentication", "Users", null, $"Email: {email}");
        }
        /// <summary>
        /// Gets the client IP address
        /// </summary>
        /// <returns>IP address</returns>
        private string GetClientIPAddress()
        {
            try
            {
                HttpRequest request = HttpContext.Current?.Request;
                if (request == null)
                    return null;

                string ip = request.Headers["X-Forwarded-For"];
                if (string.IsNullOrEmpty(ip))
                    ip = request.UserHostAddress;

                return ip;
            }
            catch
            {
                return null;
            }
        }
    }
}