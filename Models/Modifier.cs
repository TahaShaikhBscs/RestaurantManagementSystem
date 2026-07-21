// ============================================
// Models/POS/Modifier.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// Modifier Model
    /// </summary>
    public class Modifier
    {
        public int ModifierID { get; set; }
        public int ModifierGroupID { get; set; }
        public string ModifierName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}