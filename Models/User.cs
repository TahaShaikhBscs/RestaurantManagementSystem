
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// User Model - Represents a system user
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int RoleID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int SessionTimeout { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties for display
        public string RoleName { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
    }
}