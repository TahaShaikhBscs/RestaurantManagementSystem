// ============================================
// DAL/ExpenseDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Expense DAL - Data access layer for expense operations
    /// Handles all database operations for expenses
    /// </summary>
    public class ExpenseDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public ExpenseDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new expense into the database
        /// </summary>
        public int InsertExpense(Expense expense)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", expense.CompanyID),
                    dbHelper.CreateParameter("@BranchID", expense.BranchID),
                    dbHelper.CreateParameter("@ExpenseCategory", expense.ExpenseCategory),
                    dbHelper.CreateParameter("@Description", expense.Description),
                    dbHelper.CreateParameter("@Amount", expense.Amount),
                    dbHelper.CreateParameter("@ExpenseDate", expense.ExpenseDate),
                    dbHelper.CreateParameter("@PaymentMethod", expense.PaymentMethod),
                    dbHelper.CreateParameter("@ReceiptNumber", expense.ReceiptNumber),
                    dbHelper.CreateParameter("@IsRecurring", expense.IsRecurring),
                    dbHelper.CreateParameter("@RecurringType", expense.RecurringType),
                    dbHelper.CreateParameter("@CreatedBy", expense.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertExpense", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting expense: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing expense
        /// </summary>
        public bool UpdateExpense(Expense expense)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ExpenseID", expense.ExpenseID),
                    dbHelper.CreateParameter("@ExpenseCategory", expense.ExpenseCategory),
                    dbHelper.CreateParameter("@Description", expense.Description),
                    dbHelper.CreateParameter("@Amount", expense.Amount),
                    dbHelper.CreateParameter("@ExpenseDate", expense.ExpenseDate),
                    dbHelper.CreateParameter("@PaymentMethod", expense.PaymentMethod),
                    dbHelper.CreateParameter("@ReceiptNumber", expense.ReceiptNumber),
                    dbHelper.CreateParameter("@IsRecurring", expense.IsRecurring),
                    dbHelper.CreateParameter("@RecurringType", expense.RecurringType),
                    dbHelper.CreateParameter("@Status", expense.Status),
                    dbHelper.CreateParameter("@UpdatedBy", expense.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateExpense", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating expense: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes an expense
        /// </summary>
        public bool DeleteExpense(int expenseID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ExpenseID", expenseID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteExpense", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting expense: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets an expense by ID
        /// </summary>
        public Expense GetExpenseById(int expenseID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ExpenseID", expenseID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetExpenseById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToExpense(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting expense by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets expenses by branch with filters
        /// </summary>
        public List<Expense> GetExpensesByBranch(int branchID, DateTime? dateFrom = null, DateTime? dateTo = null, string category = null)
        {
            try
            {
                List<Expense> expenses = new List<Expense>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@Category", category)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetExpensesByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    expenses.Add(MapDataRowToExpense(row));
                }

                return expenses;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting expenses by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets expense categories
        /// </summary>
        public List<string> GetExpenseCategories(int branchID)
        {
            try
            {
                List<string> categories = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetExpenseCategories", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    categories.Add(row["ExpenseCategory"].ToString());
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting expense categories: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets expense summary for a period
        /// </summary>
        public DataTable GetExpensesSummary(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetExpensesSummary", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting expenses summary: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets total expenses for a period
        /// </summary>
        public decimal GetTotalExpenses(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var expenses = GetExpensesByBranch(branchID, dateFrom, dateTo);
                decimal total = 0;
                foreach (var expense in expenses)
                {
                    total += expense.Amount;
                }
                return total;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to an Expense object
        /// </summary>
        private Expense MapDataRowToExpense(DataRow row)
        {
            return new Expense
            {
                ExpenseID = row.GetInt("ExpenseID"),
                CompanyID = row.GetInt("CompanyID"),
                BranchID = row.GetInt("BranchID"),

                ExpenseCategory = row.GetString("ExpenseCategory"),
                Description = row.GetString("Description"),

                Amount = row.GetDecimal("Amount"),

                ExpenseDate = row.GetDateTime("ExpenseDate"),

                PaymentMethod = row.GetString("PaymentMethod"),
                ReceiptNumber = row.GetString("ReceiptNumber"),

                IsRecurring = row.GetBool("IsRecurring"),
                RecurringType = row.GetString("RecurringType"),

                Status = row.GetBool("Status"),

                CreatedDate = row.GetDateTime("CreatedDate"),
                UpdatedDate = row.GetDateTime("UpdatedDate"),

                CreatedBy = row.GetInt("CreatedBy"),
                UpdatedBy = row.GetInt("UpdatedBy"),

                IsDeleted = row.GetBool("IsDeleted")
            };
        }
    }
}