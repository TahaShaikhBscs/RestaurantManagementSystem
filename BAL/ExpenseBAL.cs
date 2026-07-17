// ============================================
// BAL/ExpenseBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Expense BAL - Business logic layer for expense operations
    /// Contains business rules and validation for expense management
    /// </summary>
    public class ExpenseBAL
    {
        private readonly ExpenseDAL expenseDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public ExpenseBAL()
        {
            expenseDAL = new ExpenseDAL();
        }

        /// <summary>
        /// Saves an expense to the database
        /// </summary>
        public int SaveExpense(Expense expense)
        {
            ValidateExpense(expense);

            if (expense.ExpenseID > 0)
            {
                bool updated = expenseDAL.UpdateExpense(expense);
                if (!updated)
                {
                    throw new Exception("Failed to update expense.");
                }
                return expense.ExpenseID;
            }
            else
            {
                int newID = expenseDAL.InsertExpense(expense);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert expense.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates expense data
        /// </summary>
        private void ValidateExpense(Expense expense)
        {
            if (expense.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (expense.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(expense.ExpenseCategory))
            {
                throw new Exception("Expense category is required.");
            }

            if (expense.ExpenseCategory.Length > 100)
            {
                throw new Exception("Expense category cannot exceed 100 characters.");
            }

            if (expense.Amount <= 0)
            {
                throw new Exception("Expense amount must be greater than 0.");
            }

            if (expense.ExpenseDate == DateTime.MinValue)
            {
                expense.ExpenseDate = DateTime.Now;
            }

            // Validate recurring type if recurring
            if (expense.IsRecurring && string.IsNullOrWhiteSpace(expense.RecurringType))
            {
                throw new Exception("Recurring type is required for recurring expenses.");
            }

            if (expense.IsRecurring)
            {
                string[] validTypes = { "Daily", "Weekly", "Monthly", "Yearly" };
                if (!Array.Exists(validTypes, t => t == expense.RecurringType))
                {
                    throw new Exception("Invalid recurring type.");
                }
            }
        }

        /// <summary>
        /// Deletes an expense
        /// </summary>
        public bool DeleteExpense(int expenseID, int updatedBy)
        {
            if (expenseID <= 0)
            {
                throw new Exception("Invalid expense ID.");
            }

            return expenseDAL.DeleteExpense(expenseID, updatedBy);
        }

        /// <summary>
        /// Gets an expense by ID
        /// </summary>
        public Expense GetExpenseById(int expenseID)
        {
            if (expenseID <= 0)
            {
                throw new Exception("Invalid expense ID.");
            }

            return expenseDAL.GetExpenseById(expenseID);
        }

        /// <summary>
        /// Gets all expenses for a branch
        /// </summary>
        public List<Expense> GetExpensesByBranch(int branchID, DateTime? dateFrom = null, DateTime? dateTo = null, string category = null)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return expenseDAL.GetExpensesByBranch(branchID, dateFrom, dateTo, category);
        }

        /// <summary>
        /// Gets expense categories
        /// </summary>
        public List<string> GetExpenseCategories(int branchID)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return expenseDAL.GetExpenseCategories(branchID);
        }

        /// <summary>
        /// Gets expense summary for a period
        /// </summary>
        public DataTable GetExpensesSummary(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return expenseDAL.GetExpensesSummary(branchID, dateFrom, dateTo);
        }

        /// <summary>
        /// Gets total expenses for a period
        /// </summary>
        public decimal GetTotalExpenses(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return expenseDAL.GetTotalExpenses(branchID, dateFrom, dateTo);
        }

        /// <summary>
        /// Gets expenses by category
        /// </summary>
        public Dictionary<string, decimal> GetExpensesByCategory(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            var expenses = GetExpensesByBranch(branchID, dateFrom, dateTo);
            var categoryTotals = new Dictionary<string, decimal>();

            foreach (var expense in expenses)
            {
                if (!categoryTotals.ContainsKey(expense.ExpenseCategory))
                {
                    categoryTotals[expense.ExpenseCategory] = 0;
                }
                categoryTotals[expense.ExpenseCategory] += expense.Amount;
            }

            return categoryTotals;
        }

        /// <summary>
        /// Gets monthly expense trend
        /// </summary>
        public Dictionary<string, decimal> GetMonthlyExpenseTrend(int branchID, int months)
        {
            var trend = new Dictionary<string, decimal>();
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddMonths(-months);

            for (int i = 0; i < months; i++)
            {
                DateTime monthDate = startDate.AddMonths(i);
                string monthKey = monthDate.ToString("MMM yyyy");

                var expenses = GetExpensesByBranch(branchID,
                    new DateTime(monthDate.Year, monthDate.Month, 1),
                    new DateTime(monthDate.Year, monthDate.Month,
                        DateTime.DaysInMonth(monthDate.Year, monthDate.Month)));

                decimal total = 0;
                foreach (var expense in expenses)
                {
                    total += expense.Amount;
                }

                trend[monthKey] = total;
            }

            return trend;
        }
    }
}