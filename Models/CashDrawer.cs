// ============================================
// Models/POS/CashDrawer.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models.POS
{
    /// <summary>
    /// Cash Drawer Model
    /// </summary>
    public class CashDrawer
    {
        public int CashDrawerID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string DrawerName { get; set; }
        public string DrawerNumber { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal ExpectedClosing { get; set; }
        public decimal CashIn { get; set; }
        public decimal CashOut { get; set; }
        public bool IsOpen { get; set; }
        public DateTime? OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? OpenedBy { get; set; }
        public int? ClosedBy { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Display Properties
        public decimal Variance { get { return ExpectedClosing - ClosingBalance; } }
        public string StatusText { get { return IsOpen ? "Open" : "Closed"; } }
    }
}