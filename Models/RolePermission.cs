
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Role Permission Model - Defines permissions for a role on specific modules
    /// </summary>
    public class RolePermission
    {
        public int PermissionID { get; set; }
        public int RoleID { get; set; }
        public string ModuleName { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanPrint { get; set; }
        public bool CanExport { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
