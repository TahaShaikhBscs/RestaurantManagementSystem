// ============================================
// DAL/AuditLogDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Audit Log DAL - Data access layer for audit logging
    /// Handles all database operations for audit logs
    /// </summary>
    public class AuditLogDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public AuditLogDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new audit log entry
        /// </summary>
        public int InsertAuditLog(AuditLog auditLog)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", auditLog.CompanyID),
                    dbHelper.CreateParameter("@BranchID", auditLog.BranchID),
                    dbHelper.CreateParameter("@UserID", auditLog.UserID),
                    dbHelper.CreateParameter("@ActionType", auditLog.ActionType),
                    dbHelper.CreateParameter("@Module", auditLog.Module),
                    dbHelper.CreateParameter("@TableName", auditLog.TableName),
                    dbHelper.CreateParameter("@RecordID", auditLog.RecordID),
                    dbHelper.CreateParameter("@OldValues", auditLog.OldValues),
                    dbHelper.CreateParameter("@NewValues", auditLog.NewValues),
                    dbHelper.CreateParameter("@IPAddress", auditLog.IPAddress),
                    dbHelper.CreateParameter("@UserAgent", auditLog.UserAgent)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertAuditLog", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting audit log: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets audit logs with filters
        /// </summary>
        public List<AuditLog> GetAuditLogs(int? companyID = null, int? branchID = null, int? userID = null,
            string module = null, DateTime? dateFrom = null, DateTime? dateTo = null,
            int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                List<AuditLog> logs = new List<AuditLog>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@UserID", userID),
                    dbHelper.CreateParameter("@Module", module),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@PageNumber", pageNumber),
                    dbHelper.CreateParameter("@PageSize", pageSize)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAuditLogs", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    logs.Add(MapDataRowToAuditLog(row));
                }

                return logs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting audit logs: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to an AuditLog object
        /// </summary>
        private AuditLog MapDataRowToAuditLog(DataRow row)
        {
            return new AuditLog
            {
                AuditLogID = Convert.ToInt32(row["AuditLogID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                UserID = Convert.ToInt32(row["UserID"]),
                ActionType = row["ActionType"].ToString(),
                Module = row["Module"]?.ToString(),
                TableName = row["TableName"]?.ToString(),
                RecordID = row["RecordID"] != DBNull.Value ? Convert.ToInt32(row["RecordID"]) : (int?)null,
                OldValues = row["OldValues"]?.ToString(),
                NewValues = row["NewValues"]?.ToString(),
                IPAddress = row["IPAddress"]?.ToString(),
                UserAgent = row["UserAgent"]?.ToString(),
                ActionDate = Convert.ToDateTime(row["ActionDate"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
        }
    }
}