// ============================================
// BAL/CustomerBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using System.Data;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Customer BAL - Business logic layer for customer operations
    /// Contains business rules and validation for customer management
    /// </summary>
    public class CustomerBAL
    {
        private readonly CustomerDAL customerDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public CustomerBAL()
        {
            customerDAL = new CustomerDAL();
        }

        /// <summary>
        /// Saves a customer to the database
        /// </summary>
        public int SaveCustomer(Customer customer)
        {
            ValidateCustomer(customer);

            // Check for duplicate phone number
            if (!string.IsNullOrWhiteSpace(customer.Phone))
            {
                var existing = customerDAL.GetCustomerByPhone(customer.Phone, customer.BranchID);
                if (existing != null && existing.CustomerID != customer.CustomerID)
                {
                    throw new Exception("A customer with this phone number already exists.");
                }
            }

            // Check for duplicate email
            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                // In a real implementation, we would check for duplicate email
                // For now, we'll skip this check
            }

            // Generate membership number for new customers
            if (customer.CustomerID == 0 && string.IsNullOrWhiteSpace(customer.MembershipNumber))
            {
                customer.MembershipNumber = GenerateMembershipNumber(customer.BranchID);
            }

            if (customer.CustomerID > 0)
            {
                bool updated = customerDAL.UpdateCustomer(customer);
                if (!updated)
                {
                    throw new Exception("Failed to update customer.");
                }
                return customer.CustomerID;
            }
            else
            {
                int newID = customerDAL.InsertCustomer(customer);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert customer.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates customer data
        /// </summary>
        private void ValidateCustomer(Customer customer)
        {
            if (customer.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (customer.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                throw new Exception("Customer name is required.");
            }

            if (customer.CustomerName.Length > 200)
            {
                throw new Exception("Customer name cannot exceed 200 characters.");
            }

            if (customer.Email != null && !IsValidEmail(customer.Email))
            {
                throw new Exception("Invalid email format.");
            }

            if (customer.CustomerType != null &&
                !string.IsNullOrWhiteSpace(customer.CustomerType) &&
                customer.CustomerType != "Regular" &&
                customer.CustomerType != "VIP")
            {
                throw new Exception("Customer type must be 'Regular' or 'VIP'.");
            }
        }

        /// <summary>
        /// Checks if an email is valid
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generates a unique membership number
        /// </summary>
        private string GenerateMembershipNumber(int branchID)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(1000, 9999).ToString();
            return $"MEM-{branchID}-{date}-{random}";
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        public bool DeleteCustomer(int customerID, int updatedBy)
        {
            if (customerID <= 0)
            {
                throw new Exception("Invalid customer ID.");
            }

            // Check if customer has orders
            OrderBAL orderBAL = new OrderBAL();
            var orders = orderBAL.GetOrdersByBranch(
                customerDAL.GetCustomerById(customerID).BranchID);
            // In a real implementation, we would check if customer has orders

            return customerDAL.DeleteCustomer(customerID, updatedBy);
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        public Customer GetCustomerById(int customerID)
        {
            if (customerID <= 0)
            {
                throw new Exception("Invalid customer ID.");
            }

            return customerDAL.GetCustomerById(customerID);
        }

        /// <summary>
        /// Gets all customers for a branch
        /// </summary>
        public List<Customer> GetCustomersByBranch(int branchID, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return customerDAL.GetCustomersByBranch(branchID, includeInactive);
        }

        /// <summary>
        /// Searches customers
        /// </summary>
        public List<Customer> SearchCustomers(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetCustomersByBranch(branchID);
            }

            return customerDAL.SearchCustomers(branchID, searchTerm);
        }

        /// <summary>
        /// Gets a customer by phone number
        /// </summary>
        public Customer GetCustomerByPhone(string phone, int? branchID = null)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new Exception("Phone number is required.");
            }

            return customerDAL.GetCustomerByPhone(phone, branchID);
        }

        /// <summary>
        /// Updates customer loyalty points
        /// </summary>
        public bool UpdateLoyaltyPoints(int customerID, int points, string operation, int updatedBy)
        {
            if (customerID <= 0)
            {
                throw new Exception("Invalid customer ID.");
            }

            if (points <= 0)
            {
                throw new Exception("Points must be greater than 0.");
            }

            string[] validOperations = { "Add", "Redeem", "Adjust" };
            if (!Array.Exists(validOperations, o => o == operation))
            {
                throw new Exception("Invalid operation.");
            }

            // Check if customer has enough points for redemption
            if (operation == "Redeem")
            {
                var customer = customerDAL.GetCustomerById(customerID);
                if (customer == null)
                {
                    throw new Exception("Customer not found.");
                }

                if (customer.LoyaltyPoints < points)
                {
                    throw new Exception("Insufficient loyalty points.");
                }
            }

            return customerDAL.UpdateLoyaltyPoints(customerID, points, operation, updatedBy);
        }

        /// <summary>
        /// Gets total customers count for a branch
        /// </summary>
        public int GetTotalCustomers(int branchID)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return customerDAL.GetTotalCustomers(branchID);
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
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return customerDAL.GetCustomersWithLoyalty(branchID, minPoints, topCount);
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
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (dateFrom > dateTo)
            {
                throw new Exception("Start date cannot be after end date.");
            }

            return customerDAL.GetCustomerSummaryReport(branchID, dateFrom, dateTo, topCount);
        }
        /// <summary>
        /// Gets VIP customers
        /// </summary>
        public List<Customer> GetVIPCustomers(int branchID)
        {
            var customers = GetCustomersByBranch(branchID);
            var vipCustomers = new List<Customer>();

            foreach (var customer in customers)
            {
                if (customer.CustomerType == "VIP")
                {
                    vipCustomers.Add(customer);
                }
            }

            return vipCustomers;
        }
    }
}