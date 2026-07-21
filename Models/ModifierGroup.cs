// ============================================
// Models/POS/ModifierGroup.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// Modifier Group Model
    /// </summary>
    public class ModifierGroup
    {
        public int ModifierGroupID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string SelectionType { get; set; } // Single, Multiple
        public int MinSelect { get; set; }
        public int MaxSelect { get; set; }
        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public List<Modifier> Modifiers { get; set; }
    }
}