// ============================================
// BAL/RoleBAL.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Role BAL - Business logic layer for role operations
    /// </summary>
    public class RoleBAL
    {
        #region Private Fields

        private readonly RoleDAL roleDAL;
        private readonly RolePermissionDAL permissionDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        public RoleBAL()
        {
            roleDAL = new RoleDAL();
            permissionDAL = new RolePermissionDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region CRUD Operations

        public int SaveRole(Role role)
        {
            ValidateRole(role);

            if (IsRoleNameExist(role.RoleName, role.RoleID))
            {
                throw new Exception("A role with this name already exists.");
            }

            if (role.RoleID > 0)
            {
                bool updated = roleDAL.UpdateRole(role);
                if (!updated)
                {
                    throw new Exception("Failed to update role.");
                }

                auditLogger.LogUpdate("RoleManagement", "Roles", role.RoleID,
                    $"RoleID: {role.RoleID}", $"RoleName: {role.RoleName}");

                return role.RoleID;
            }
            else
            {
                int newID = roleDAL.InsertRole(role);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert role.");
                }

                auditLogger.LogInsert("RoleManagement", "Roles", newID,
                    $"RoleName: {role.RoleName}");

                return newID;
            }
        }

        public bool DeleteRole(int roleID, int updatedBy)
        {
            if (roleID <= 0)
            {
                throw new Exception("Invalid role ID.");
            }

            // Check if role is assigned to any user
            UserBAL userBAL = new UserBAL();
            var users = userBAL.GetUsersByRole(roleID);
            if (users.Count > 0)
            {
                throw new Exception("Cannot delete role that is assigned to users.");
            }

            bool deleted = roleDAL.DeleteRole(roleID, updatedBy);
            if (deleted)
            {
                auditLogger.LogDelete("RoleManagement", "Roles", roleID,
                    $"RoleID: {roleID}");
            }

            return deleted;
        }

        #endregion

        #region Read Operations

        public Role GetRoleById(int roleID)
        {
            if (roleID <= 0)
            {
                throw new Exception("Invalid role ID.");
            }

            return roleDAL.GetRoleById(roleID);
        }

        public List<Role> GetAllRoles()
        {
            return roleDAL.GetAllRoles();
        }

        public List<Role> SearchRoles(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetAllRoles();
            }

            return roleDAL.SearchRoles(searchTerm);
        }

        #endregion

        #region Permissions

        public bool SaveRolePermissions(int roleID, List<RolePermission> permissions, int createdBy)
        {
            if (roleID <= 0)
            {
                throw new Exception("Invalid role ID.");
            }

            try
            {
                foreach (var permission in permissions)
                {
                    permission.RoleID = roleID;
                    permission.CreatedBy = createdBy;
                    permissionDAL.InsertRolePermission(permission);
                }

                auditLogger.LogUpdate("RoleManagement", "RolePermissions", roleID,
                    $"RoleID: {roleID}", "Permissions updated");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving role permissions: {ex.Message}", ex);
            }
        }

        public List<RolePermission> GetRolePermissions(int roleID)
        {
            if (roleID <= 0)
            {
                throw new Exception("Invalid role ID.");
            }

            return permissionDAL.GetRolePermissions(roleID);
        }

        #endregion

        #region Validation Methods

        private void ValidateRole(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new Exception("Role name is required.");
            }

            if (role.RoleName.Length > 100)
            {
                throw new Exception("Role name cannot exceed 100 characters.");
            }
        }

        public bool IsRoleNameExist(string roleName, int excludeRoleID = 0)
        {
            try
            {
                List<Role> roles = roleDAL.GetAllRoles();
                foreach (Role role in roles)
                {
                    if (role.RoleID != excludeRoleID &&
                        role.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}