// ============================================
// DAL/CustomerDAL.cs
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
    /// Customer DAL - Data access layer for customer operations
    /// Handles all database operations for customers
    /// </summary>
    public class CustomerDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public CustomerDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new customer into the database
        /// </summary>
        public int InsertCustomer(Customer customer)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", customer.CompanyID),
                    dbHelper.CreateParameter("@BranchID", customer.BranchID),
                    dbHelper.CreateParameter("@CustomerName", customer.CustomerName),
                    dbHelper.CreateParameter("@Phone", customer.Phone),
                    dbHelper.CreateParameter("@Email", customer.Email),
                    dbHelper.CreateParameter("@Address", customer.Address),
                    dbHelper.CreateParameter("@CustomerType", customer.CustomerType),
                    dbHelper.CreateParameter("@LoyaltyPoints", customer.LoyaltyPoints),
                    dbHelper.CreateParameter("@MembershipNumber", customer.MembershipNumber),
                    dbHelper.CreateParameter("@Birthday", customer.Birthday),
                    dbHelper.CreateParameter("@Gender", customer.Gender),
                    dbHelper.CreateParameter("@Status", customer.Status),
                    dbHelper.CreateParameter("@CreatedBy", customer.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertCustomer", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CustomerID", customer.CustomerID),
                    dbHelper.CreateParameter("@CustomerName", customer.CustomerName),
                    dbHelper.CreateParameter("@Phone", customer.Phone),
                    dbHelper.CreateParameter("@Email", customer.Email),
                    dbHelper.CreateParameter("@Address", customer.Address),
                    dbHelper.CreateParameter("@CustomerType", customer.CustomerType),
                    dbHelper.CreateParameter("@LoyaltyPoints", customer.LoyaltyPoints),
                    dbHelper.CreateParameter("@MembershipNumber", customer.MembershipNumber),
                    dbHelper.CreateParameter("@Birthday", customer.Birthday),
                    dbHelper.CreateParameter("@Gender", customer.Gender),
                    dbHelper.CreateParameter("@Status", customer.Status),
                    dbHelper.CreateParameter("@UpdatedBy", customer.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateCustomer", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a customer
        /// </summary>
        public bool DeleteCustomer(int customerID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CustomerID", customerID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteCustomer", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        public Customer GetCustomerById(int customerID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CustomerID", customerID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCustomerById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToCustomer(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all customers for a branch
        /// </summary>
        public List<Customer> GetCustomersByBranch(int branchID, bool includeInactive = false)
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCustomersByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    customers.Add(MapDataRowToCustomer(row));
                }

                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customers by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches customers by name, phone, or email
        /// </summary>
        public List<Customer> SearchCustomers(int branchID, string searchTerm)
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchCustomers", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    customers.Add(MapDataRowToCustomer(row));
                }

                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching customers: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a customer by phone number
        /// </summary>
        public Customer GetCustomerByPhone(string phone, int? branchID = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@Phone", phone),
                    dbHelper.CreateParameter("@BranchID", branchID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCustomerByPhone", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToCustomer(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer by phone: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates customer loyalty points
        /// </summary>
        public bool UpdateLoyaltyPoints(int customerID, int points, string operation, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CustomerID", customerID),
                    dbHelper.CreateParameter("@Points", points),
                    dbHelper.CreateParameter("@Operation", operation),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateCustomerLoyalty", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating loyalty points: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets total customer count for a branch
        /// </summary>
        public int GetTotalCustomers(int branchID)
        {
            try
            {
                var customers = GetCustomersByBranch(branchID);
                return customers.Count;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets customers with loyalty points
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="minPoints">Minimum loyalty points</param>
        /// <param name="topCount">Number of top customers to return</param>
        /// <returns>DataTable with customer loyalty data</returns>
        public DataTable GetCustomersWithLoyalty(int branchID, int minPoints = 0, int topCount = 20)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            dbHelper.CreateParameter("@BranchID", branchID),
            dbHelper.CreateParameter("@MinPoints", minPoints),
            dbHelper.CreateParameter("@TopCount", topCount)
        };

                return dbHelper.ExecuteStoredProcedure("sp_GetCustomersWithLoyalty", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customers with loyalty: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets customer summary report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <param name="topCount">Number of customers to return</param>
        /// <returns>DataTable with customer summary</returns>
        public DataTable GetCustomerSummaryReport(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 20)
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

                return dbHelper.ExecuteStoredProcedure("sp_GetCustomerSummaryReport", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer summary report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to a Customer object
        /// </summary>
        private Customer MapDataRowToCustomer(DataRow row)
        {
            return new Customer
            {
                CustomerID = row.GetInt("CustomerID"),
                CompanyID = row.GetInt("CompanyID"),
                BranchID = row.GetInt("BranchID"),

                CustomerName = row.GetString("CustomerName"),
                Phone = row.GetString("Phone"),
                Email = row.GetString("Email"),
                Address = row.GetString("Address"),

                CustomerType = string.IsNullOrWhiteSpace(row.GetString("CustomerType"))
                    ? "Regular"
                    : row.GetString("CustomerType"),

                LoyaltyPoints = row.GetInt("LoyaltyPoints"),

                MembershipNumber = row.GetString("MembershipNumber"),

                Birthday = row.GetNullableDateTime("Birthday"),

                Gender = row.GetString("Gender"),

                TotalPurchases = row.GetDecimal("TotalPurchases"),

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