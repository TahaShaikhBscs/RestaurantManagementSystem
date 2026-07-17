
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Restaurant Table Model - Represents a physical table in the restaurant
    /// </summary>
    public class RestaurantTable
    {
        public int TableID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string TableNumber { get; set; }
        public string Floor { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } // Available, Occupied, Reserved, Cleaning
        public bool IsActive { get; set; }
        public string QRCode { get; set; }
        public bool _Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string BranchName { get; set; }
        public string StatusDisplay
        {
            get
            {
                switch (Status)
                {
                    case "Available": return "Available";
                    case "Occupied": return "Occupied";
                    case "Reserved": return "Reserved";
                    case "Cleaning": return "Cleaning";
                    default: return "Unknown";
                }
            }
        }
    }
}