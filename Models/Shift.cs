// ============================================
// Models/Shift.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Shift Model - Represents a work shift
    /// </summary>
    public class Shift
    {
        public int ShiftID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int UserID { get; set; }
        public string ShiftNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCard { get; set; }
        public decimal TotalOnline { get; set; }
        public bool IsOpen { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public bool _Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public string UserFullName { get; set; }
        public decimal NetSales { get { return TotalSales - TotalCash - TotalCard - TotalOnline; } }
    }
}