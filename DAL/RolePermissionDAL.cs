
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Role Permission DAL - Data access layer for role permissions
    /// Handles permission management for roles
    /// </summary>
    public class RolePermissionDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public RolePermissionDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts or updates permissions for a role
        /// </summary>
        /// <param name="rolePermission">Role permission object</param>
        /// <returns>True if operation successful</returns>
        public bool InsertRolePermission(RolePermission rolePermission)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@RoleID", rolePermission.RoleID),
                    dbHelper.CreateParameter("@ModuleName", rolePermission.ModuleName),
                    dbHelper.CreateParameter("@CanView", rolePermission.CanView),
                    dbHelper.CreateParameter("@CanCreate", rolePermission.CanCreate),
                    dbHelper.CreateParameter("@CanEdit", rolePermission.CanEdit),
                    dbHelper.CreateParameter("@CanDelete", rolePermission.CanDelete),
                    dbHelper.CreateParameter("@CanPrint", rolePermission.CanPrint),
                    dbHelper.CreateParameter("@CanExport", rolePermission.CanExport),
                    dbHelper.CreateParameter("@CreatedBy", rolePermission.CreatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_InsertRolePermission", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting role permission: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all permissions for a role
        /// </summary>
        /// <param name="roleID">Role ID</param>
        /// <returns>List of role permissions</returns>
        public List<RolePermission> GetRolePermissions(int roleID)
        {
            try
            {
                List<RolePermission> permissions = new List<RolePermission>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@RoleID", roleID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetRolePermissions", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    permissions.Add(MapDataRowToRolePermission(row));
                }

                return permissions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting role permissions: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets permissions for a specific user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>List of permissions</returns>
        public List<RolePermission> GetUserPermissions(int userID)
        {
            try
            {
                List<RolePermission> permissions = new List<RolePermission>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@UserID", userID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetUserPermissions", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    permissions.Add(MapDataRowToRolePermission(row));
                }

                return permissions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user permissions: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a user has permission for a specific module and action
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="moduleName">Module name</param>
        /// <param name="action">Action (View, Create, Edit, Delete, Print, Export)</param>
        /// <returns>True if user has permission</returns>
        public bool HasPermission(int userID, string moduleName, string action)
        {
            try
            {
                List<RolePermission> permissions = GetUserPermissions(userID);

                foreach (var permission in permissions)
                {
                    if (permission.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                    {
                        switch (action.ToLower())
                        {
                            case "view": return permission.CanView;
                            case "create": return permission.CanCreate;
                            case "edit": return permission.CanEdit;
                            case "delete": return permission.CanDelete;
                            case "print": return permission.CanPrint;
                            case "export": return permission.CanExport;
                            default: return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking permission: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to a RolePermission object
        /// </summary>
        /// <param name="row">DataRow containing permission data</param>
        /// <returns>RolePermission object</returns>
        private RolePermission MapDataRowToRolePermission(DataRow row)
        {
            return new RolePermission
            {
                PermissionID = Convert.ToInt32(row["PermissionID"]),
                RoleID = Convert.ToInt32(row["RoleID"]),
                ModuleName = row["ModuleName"].ToString(),
                CanView = Convert.ToBoolean(row["CanView"]),
                CanCreate = Convert.ToBoolean(row["CanCreate"]),
                CanEdit = Convert.ToBoolean(row["CanEdit"]),
                CanDelete = Convert.ToBoolean(row["CanDelete"]),
                CanPrint = Convert.ToBoolean(row["CanPrint"]),
                CanExport = Convert.ToBoolean(row["CanExport"]),
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