// ============================================
// DAL/PaymentDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Payment DAL - Data access layer for payment operations
    /// Handles all database operations for payments
    /// </summary>
    public class PaymentDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public PaymentDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new payment into the database
        /// </summary>
        public int InsertPayment(Payment payment)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", payment.CompanyID),
                    dbHelper.CreateParameter("@BranchID", payment.BranchID),
                    dbHelper.CreateParameter("@OrderID", payment.OrderID),
                    dbHelper.CreateParameter("@PaymentMethod", payment.PaymentMethod),
                    dbHelper.CreateParameter("@Amount", payment.Amount),
                    dbHelper.CreateParameter("@ReferenceNumber", payment.ReferenceNumber),
                    dbHelper.CreateParameter("@PaymentStatus", payment.PaymentStatus),
                    dbHelper.CreateParameter("@CreatedBy", payment.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertPayment", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting payment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all payments for an order
        /// </summary>
        public List<Payment> GetPaymentsByOrder(int orderID)
        {
            try
            {
                List<Payment> payments = new List<Payment>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OrderID", orderID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetPaymentsByOrder", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    payments.Add(MapDataRowToPayment(row));
                }

                return payments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting payments by order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a payment by ID
        /// </summary>
        public Payment GetPaymentById(int paymentID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@PaymentID", paymentID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetPaymentById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToPayment(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting payment by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets total payments for a date range
        /// </summary>
        public decimal GetTotalPayments(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var payments = GetPaymentsByBranch(branchID, dateFrom, dateTo);
                decimal total = 0;
                foreach (var payment in payments)
                {
                    if (payment.PaymentStatus == "Completed")
                    {
                        total += payment.Amount;
                    }
                }
                return total;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets payments by branch with date filter
        /// </summary>
        private List<Payment> GetPaymentsByBranch(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            // This would need a stored procedure - implement as needed
            return new List<Payment>();
        }

        /// <summary>
        /// Maps a DataRow to a Payment object
        /// </summary>
        private Payment MapDataRowToPayment(DataRow row)
        {
            return new Payment
            {
                PaymentID = Convert.ToInt32(row["PaymentID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                OrderID = Convert.ToInt32(row["OrderID"]),
                PaymentMethod = row["PaymentMethod"].ToString(),
                Amount = Convert.ToDecimal(row["Amount"]),
                ReferenceNumber = row["ReferenceNumber"]?.ToString(),
                TransactionDate = Convert.ToDateTime(row["TransactionDate"]),
                PaymentStatus = row["PaymentStatus"].ToString(),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
        }
    }
}