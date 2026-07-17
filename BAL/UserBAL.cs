
using System;
using System.Security.Cryptography;
using System.Text;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using System.Linq;
using RestaurantManagementSystem.Utilities;
using System.Collections.Generic;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// User BAL - Business logic layer for user operations
    /// Handles authentication, password management, and user validation
    /// </summary>
    public class UserBAL
    {
        private readonly UserDAL userDAL;
        private readonly RolePermissionDAL rolePermissionDAL;

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public UserBAL()
        {
            userDAL = new UserDAL();
            rolePermissionDAL = new RolePermissionDAL();
        }

        /// <summary>
        /// Saves a user to the database
        /// Validates business rules before saving
        /// </summary>
        /// <param name="user">User object to save</param>
        /// <returns>ID of the saved user</returns>
        public int SaveUser(User user)
        {
            // Validate user data
            ValidateUser(user);

            // Check for duplicate username
            if (IsUsernameExist(user.Username, user.UserID))
            {
                throw new Exception("A user with this username already exists.");
            }

            // If new user, generate password hash
            if (user.UserID == 0)
            {
                // Default password is "Password123!" - should be changed on first login
                string defaultPassword = "Password123!";
                HashPassword(defaultPassword, out string passwordHash, out string passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            // Insert or update based on whether ID exists
            if (user.UserID > 0)
            {
                bool updated = userDAL.UpdateUser(user);
                if (!updated)
                {
                    throw new Exception("Failed to update user.");
                }
                return user.UserID;
            }
            else
            {
                int newID = userDAL.InsertUser(user);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert user.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates user data
        /// </summary>
        /// <param name="user">User object to validate</param>
        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new Exception("Username is required.");
            }

            if (user.Username.Length > 100)
            {
                throw new Exception("Username cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new Exception("Full name is required.");
            }

            if (user.FullName.Length > 200)
            {
                throw new Exception("Full name cannot exceed 200 characters.");
            }

            if (user.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (user.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (user.RoleID <= 0)
            {
                throw new Exception("Role is required.");
            }

            if (user.Email != null && !IsValidEmail(user.Email))
            {
                throw new Exception("Invalid email format.");
            }
        }

        /// <summary>
        /// Checks if an email is valid
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <returns>True if valid</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a username already exists
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <param name="excludeUserID">User ID to exclude from check</param>
        /// <returns>True if username exists</returns>
        public bool IsUsernameExist(string username, int excludeUserID = 0)
        {
            try
            {
                User existingUser = userDAL.GetUserByUsername(username);
                return existingUser != null && existingUser.UserID != excludeUserID;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hashes a password using SHA256 with salt
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="passwordHash">Output password hash</param>
        /// <param name="passwordSalt">Output password salt</param>
        public void HashPassword(string password, out string passwordHash, out string passwordSalt)
        {
            // Generate random salt
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            passwordSalt = Convert.ToBase64String(saltBytes);

            // Combine password with salt and hash
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytesFull = Convert.FromBase64String(passwordSalt);
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytesFull.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytesFull, 0, combinedBytes, passwordBytes.Length, saltBytesFull.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                passwordHash = Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Verifies a password against stored hash
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="storedHash">Stored password hash</param>
        /// <param name="storedSalt">Stored password salt</param>
        /// <returns>True if password matches</returns>
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // Combine password with salt and hash
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == storedHash;
            }
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Plain text password</param>
        /// <returns>User object if authenticated</returns>
        public User AuthenticateUser(string username, string password)
        {
            // Get user from database
            User user = userDAL.GetUserByUsername(username);
            if (user == null)
            {
                throw new Exception("Invalid username or password.");
            }

            // Verify password
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Invalid username or password.");
            }

            // Update last login
            userDAL.UpdateLastLogin(user.UserID);

            return user;
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="updatedBy">User ID performing the change</param>
        /// <returns>True if password changed</returns>
        public bool ChangePassword(int userID, string oldPassword, string newPassword, int updatedBy)
        {
            // Validate new password strength
            ValidatePasswordStrength(newPassword);

            // Get user
            User user = userDAL.GetUserById(userID);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Verify old password
            if (!VerifyPassword(oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Current password is incorrect.");
            }

            // Hash new password
            HashPassword(newPassword, out string newHash, out string newSalt);

            // Update password
            return userDAL.UpdatePassword(userID, newHash, newSalt, updatedBy);
        }

        /// <summary>
        /// Validates password strength
        /// </summary>
        /// <param name="password">Password to validate</param>
        private void ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is required.");
            }

            if (password.Length < 8)
            {
                throw new Exception("Password must be at least 8 characters long.");
            }

            if (!HasUpperCase(password))
            {
                throw new Exception("Password must contain at least one uppercase letter.");
            }

            if (!HasLowerCase(password))
            {
                throw new Exception("Password must contain at least one lowercase letter.");
            }

            if (!HasDigit(password))
            {
                throw new Exception("Password must contain at least one digit.");
            }

            if (!HasSpecialChar(password))
            {
                throw new Exception("Password must contain at least one special character.");
            }
        }

        private bool HasUpperCase(string password) => password.Any(char.IsUpper);
        private bool HasLowerCase(string password) => password.Any(char.IsLower);
        private bool HasDigit(string password) => password.Any(char.IsDigit);
        private bool HasSpecialChar(string password) => password.Any(c => !char.IsLetterOrDigit(c));

        /// <summary>
        /// Gets user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User object</returns>
        public User GetUserByUsername(string username)
        {
            return userDAL.GetUserByUsername(username);
        }

        /// <summary>
        /// Gets user by ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>User object</returns>
        public User GetUserById(int userID)
        {
            return userDAL.GetUserById(userID);
        }

        /// <summary>
        /// Validates user session
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>User object with full details</returns>
        public User ValidateUserSession(int userID)
        {
            return userDAL.ValidateUserSession(userID);
        }

        /// <summary>
        /// Gets a user by email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User object</returns>
        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email address is required.");
            }

            if (!ValidationHelper.IsValidEmail(email))
            {
                throw new Exception("Invalid email format.");
            }

            return userDAL.GetUserByEmail(email);
        }

        /// <summary>
        /// Gets users by role
        /// </summary>
        /// <param name="roleID">Role ID</param>
        /// <returns>List of users</returns>
        public List<User> GetUsersByRole(int roleID)
        {
            if (roleID <= 0)
            {
                throw new Exception("Invalid role ID.");
            }

            // In a real implementation, you would have a stored procedure for this
            // For now, we'll return an empty list
            return new List<User>();
        }

        /// <summary>
        /// Gets users by role
        /// </summary>
        /// <param name="roleID">Role ID</param>
        /// <returns>List of users</returns>
        //public List<User> GetUsersByRole(int roleID)
        //{
        //    if (roleID <= 0)
        //    {
        //        throw new Exception("Invalid role ID.");
        //    }

        //    return userDAL.GetUsersByRole(roleID);
        //}
        /// <summary>
        /// Checks if user has a specific permission
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="moduleName">Module name</param>
        /// <param name="action">Action (View, Create, Edit, Delete, Print, Export)</param>
        /// <returns>True if user has permission</returns>
        public bool HasPermission(int userID, string moduleName, string action)
        {
            return rolePermissionDAL.HasPermission(userID, moduleName, action);
        }
        
    }
}