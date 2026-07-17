// ============================================
// Models/Reservation.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Table Reservation Model - Manages table reservations
    /// </summary>
    public class TableReservation
    {
        public int ReservationID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int TableID { get; set; }
        public int CustomerID { get; set; }
        public string ReservationNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public int Duration { get; set; } // Minutes
        public int NumberOfGuests { get; set; }
        public string SpecialRequests { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Seated, Cancelled, Completed
        public bool _Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string TableNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string BranchName { get; set; }
        public int Capacity { get; set; }
    }
}