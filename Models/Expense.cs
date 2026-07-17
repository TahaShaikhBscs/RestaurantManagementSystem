
using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Expense Model - Represents business expenses
    /// </summary>
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string ExpenseCategory { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string PaymentMethod { get; set; }
        public string ReceiptNumber { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurringType { get; set; } // Daily, Weekly, Monthly, Yearly
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
