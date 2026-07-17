// ============================================
// DAL/RoleDAL.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Role DAL - Data access layer for role operations
    /// </summary>
    public class RoleDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        public RoleDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region CRUD Operations

        public int InsertRole(Role role)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@RoleName", role.RoleName),
                    dbHelper.CreateParameter("@Description", role.Description),
                    dbHelper.CreateParameter("@Status", role.Status),
                    dbHelper.CreateParameter("@CreatedBy", role.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertRole", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting role: {ex.Message}", ex);
            }
        }

        public bool UpdateRole(Role role)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@RoleID", role.RoleID),
                    dbHelper.CreateParameter("@RoleName", role.RoleName),
                    dbHelper.CreateParameter("@Description", role.Description),
                    dbHelper.CreateParameter("@Status", role.Status),
                    dbHelper.CreateParameter("@UpdatedBy", role.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateRole", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating role: {ex.Message}", ex);
            }
        }

        public bool DeleteRole(int roleID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@RoleID", roleID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteRole", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting role: {ex.Message}", ex);
            }
        }

        #endregion

        #region Read Operations

        public Role GetRoleById(int roleID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@RoleID", roleID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetRoleById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToRole(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting role by ID: {ex.Message}", ex);
            }
        }

        public List<Role> GetAllRoles()
        {
            try
            {
                List<Role> roles = new List<Role>();
                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAllRoles");

                foreach (DataRow row in dataTable.Rows)
                {
                    roles.Add(MapDataRowToRole(row));
                }

                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all roles: {ex.Message}", ex);
            }
        }

        public List<Role> SearchRoles(string searchTerm)
        {
            try
            {
                List<Role> roles = new List<Role>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchRoles", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    roles.Add(MapDataRowToRole(row));
                }

                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching roles: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        private Role MapDataRowToRole(DataRow row)
        {
            return new Role
            {
                RoleID = Convert.ToInt32(row["RoleID"]),
                RoleName = row["RoleName"].ToString(),
                Description = row["Description"]?.ToString(),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
        }

        #endregion
    }
}