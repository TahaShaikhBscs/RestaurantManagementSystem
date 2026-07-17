// ============================================
// DAL/ReportDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Report DAL - Data access layer for report generation
    /// Handles all reporting database operations
    /// </summary>
    public class ReportDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public ReportDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Gets sales report data
        /// </summary>
        public DataTable GetSalesReport(int branchID, DateTime dateFrom, DateTime dateTo, string groupBy = "Day")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@GroupBy", groupBy)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetSalesReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting sales report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets payment report data
        /// </summary>
        public DataTable GetPaymentReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetPaymentReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting payment report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets profit/loss report data
        /// </summary>
        public DataTable GetProfitLossReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetProfitLossReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting profit/loss report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets top selling items
        /// </summary>
        public DataTable GetTopSellingItems(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 10)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@TopCount", topCount)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetTopSellingItems", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting top selling items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets customer sales report
        /// </summary>
        public DataTable GetCustomerSalesReport(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 20)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@TopCount", topCount)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetCustomerSalesReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer sales report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets inventory report
        /// </summary>
        public DataTable GetInventoryReport(int branchID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID)
                };

                return dbHelper.ExecuteStoredProcedure("sp_GetInventoryReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting inventory report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets expense summary report
        /// </summary>
        public DataTable GetExpenseSummaryReport(int branchID, DateTime dateFrom, DateTime dateTo)
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
                throw new Exception($"Error getting expense summary report: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Gets expense report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="category">Category filter</param>
        /// <returns>DataTable with expense report</returns>
        public DataTable GetExpenseReport(int branchID, DateTime dateFrom, DateTime dateTo, string category = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@DateFrom", dateFrom),
            dbHelper.CreateParameter("@DateTo", dateTo),
            dbHelper.CreateParameter("@Category", category)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetExpenseReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting expense report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets payment report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with payment report</returns>
        public DataTable GetPaymentReportData(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@DateFrom", dateFrom),
            dbHelper.CreateParameter("@DateTo", dateTo)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetPaymentReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting payment report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets kitchen performance report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns>DataTable with kitchen performance</returns>
        public DataTable GetKitchenPerformanceReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@DateFrom", dateFrom),
            dbHelper.CreateParameter("@DateTo", dateTo)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetKitchenPerformanceReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting kitchen performance report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets dashboard statistics
        /// </summary>
        public DataSet GetDashboardStatistics(int branchID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID)
                };

                using (SqlConnection connection = dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand("sp_GetDashboardStatistics", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters.ToArray());

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting dashboard statistics: {ex.Message}", ex);
            }
        }
    }
}