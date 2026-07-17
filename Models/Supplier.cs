
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Supplier Model - Represents a product supplier
    /// </summary>
    public class Supplier
    {
        public int SupplierID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        public string PaymentTerms { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
