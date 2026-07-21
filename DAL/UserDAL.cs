
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// User DAL - Data access layer for user operations
    /// Handles authentication, authorization, and user management
    /// </summary>
    public class UserDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public UserDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new user into the database
        /// </summary>
        /// <param name="user">User object to insert</param>
        /// <returns>ID of the newly inserted user</returns>
        public int InsertUser(User user)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", user.CompanyID),
                    dbHelper.CreateParameter("@BranchID", user.BranchID),
                    dbHelper.CreateParameter("@RoleID", user.RoleID),
                    dbHelper.CreateParameter("@Username", user.Username),
                    dbHelper.CreateParameter("@PasswordHash", user.PasswordHash),
                    dbHelper.CreateParameter("@PasswordSalt", user.PasswordSalt),
                    dbHelper.CreateParameter("@FullName", user.FullName),
                    dbHelper.CreateParameter("@Email", user.Email),
                    dbHelper.CreateParameter("@Phone", user.Phone),
                    dbHelper.CreateParameter("@ProfileImage", user.ProfileImage),
                    dbHelper.CreateParameter("@SessionTimeout", user.SessionTimeout),
                    dbHelper.CreateParameter("@IsActive", user.IsActive),
                    dbHelper.CreateParameter("@Status", user.Status),
                    dbHelper.CreateParameter("@CreatedBy", user.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertUser", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="user">User object with updated data</param>
        /// <returns>True if update successful</returns>
        public bool UpdateUser(User user)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@UserID", user.UserID),
                    dbHelper.CreateParameter("@BranchID", user.BranchID),
                    dbHelper.CreateParameter("@RoleID", user.RoleID),
                    dbHelper.CreateParameter("@FullName", user.FullName),
                    dbHelper.CreateParameter("@Email", user.Email),
                    dbHelper.CreateParameter("@Phone", user.Phone),
                    dbHelper.CreateParameter("@ProfileImage", user.ProfileImage),
                    dbHelper.CreateParameter("@SessionTimeout", user.SessionTimeout),
                    dbHelper.CreateParameter("@IsActive", user.IsActive),
                    dbHelper.CreateParameter("@Status", user.Status),
                    dbHelper.CreateParameter("@UpdatedBy", user.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateUser", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User object with role and company details</returns>
        public User GetUserByUsername(string username)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@Username", username)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetUserByUsername", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToUser(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by username: {ex.Message}", ex);
            }
        }
        public List<User> GetAllUser()
        {

            try
            {
                List<User> users = new List<User>();
                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAllUser");

                foreach (DataRow row in dataTable.Rows)
                {
                    users.Add(MapDataRowToUser(row));
                }

                return users;
            }

            catch (Exception ex)
            {
                throw new Exception($"Error getting user by username: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>User object</returns>
        public User GetUserById(int userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@UserID", userID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetUserById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToUser(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Authenticates a user with username and password hash
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="passwordHash">Hashed password</param>
        /// <returns>User object if authenticated, null otherwise</returns>
        public User AuthenticateUser(string username, string passwordHash)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@Username", username),
                    dbHelper.CreateParameter("@PasswordHash", passwordHash)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_AuthenticateUser", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToUser(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error authenticating user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates a user session
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>User object with full details</returns>
        public User ValidateUserSession(int userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@UserID", userID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_ValidateUserSession", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToUser(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error validating user session: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates user's last login date
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>True if update successful</returns>
        public bool UpdateLastLogin(int userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@UserID", userID)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateUserLastLogin", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating last login: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates user's password
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="passwordHash">New password hash</param>
        /// <param name="passwordSalt">New password salt</param>
        /// <param name="updatedBy">User ID performing the update</param>
        /// <returns>True if update successful</returns>
        public bool UpdatePassword(int userID, string passwordHash, string passwordSalt, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@UserID", userID),
                    dbHelper.CreateParameter("@PasswordHash", passwordHash),
                    dbHelper.CreateParameter("@PasswordSalt", passwordSalt),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateUserPassword", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating password: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Gets a user by email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User object with role and company details</returns>
        public User GetUserByEmail(string email)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@Email", email)
        };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetUserByEmail", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToUser(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by email: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets users by role ID
        /// </summary>
        /// <param name="roleID">Role ID</param>
        /// <returns>List of users</returns>
        public List<User> GetUsersByRole(int roleID)
        {
            try
            {
                List<User> users = new List<User>();
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@RoleID", roleID)
        };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetUsersByRole", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    users.Add(MapDataRowToUser(row));
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users by role: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Maps a DataRow to a User object
        /// </summary>
        /// <param name="row">DataRow containing user data</param>
        /// <returns>User object</returns>
        private User MapDataRowToUser(DataRow row)
        {
            return new User
            {
                UserID = row.GetInt("UserID"),
                CompanyID = row.GetInt("CompanyID"),
                BranchID = row.GetInt("BranchID"),
                RoleID = row.GetInt("RoleID"),

                Username = row.GetString("Username"),
                PasswordHash = row.GetString("PasswordHash"),
                PasswordSalt = row.GetString("PasswordSalt"),
                FullName = row.GetString("FullName"),
                Email = row.GetString("Email"),
                Phone = row.GetString("Phone"),
                ProfileImage = row.GetString("ProfileImage"),

                LastLoginDate = row.GetNullableDateTime("LastLoginDate"),

                SessionTimeout = row.GetInt("SessionTimeout"),

                IsActive = row.GetBool("IsActive"),
                Status = row.GetBool("Status"),

                CreatedDate = row.GetDateTime("CreatedDate"),
                UpdatedDate = row.GetDateTime("UpdatedDate"),

                CreatedBy = row.GetInt("CreatedBy"),
                UpdatedBy = row.GetInt("UpdatedBy"),

                IsDeleted = row.GetBool("IsDeleted"),

                // Additional properties from joins
                RoleName = row.Table.Columns.Contains("RoleName")
                    ? row.GetString("RoleName")
                    : null,

                BranchName = row.Table.Columns.Contains("BranchName")
                    ? row.GetString("BranchName")
                    : null,

                CompanyName = row.Table.Columns.Contains("CompanyName")
                    ? row.GetString("CompanyName")
                    : null
            };
        }
    }
}
