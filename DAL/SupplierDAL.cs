// ============================================
// DAL/SupplierDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Supplier DAL - Data access layer for supplier operations
    /// Handles all database operations for suppliers
    /// </summary>
    public class SupplierDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public SupplierDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new supplier into the database
        /// </summary>
        public int InsertSupplier(Supplier supplier)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", supplier.CompanyID),
                    dbHelper.CreateParameter("@BranchID", supplier.BranchID),
                    dbHelper.CreateParameter("@SupplierName", supplier.SupplierName),
                    dbHelper.CreateParameter("@ContactPerson", supplier.ContactPerson),
                    dbHelper.CreateParameter("@Phone", supplier.Phone),
                    dbHelper.CreateParameter("@Email", supplier.Email),
                    dbHelper.CreateParameter("@Address", supplier.Address),
                    dbHelper.CreateParameter("@TaxNumber", supplier.TaxNumber),
                    dbHelper.CreateParameter("@PaymentTerms", supplier.PaymentTerms),
                    dbHelper.CreateParameter("@Status", supplier.Status),
                    dbHelper.CreateParameter("@CreatedBy", supplier.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertSupplier", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting supplier: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing supplier
        /// </summary>
        public bool UpdateSupplier(Supplier supplier)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SupplierID", supplier.SupplierID),
                    dbHelper.CreateParameter("@SupplierName", supplier.SupplierName),
                    dbHelper.CreateParameter("@ContactPerson", supplier.ContactPerson),
                    dbHelper.CreateParameter("@Phone", supplier.Phone),
                    dbHelper.CreateParameter("@Email", supplier.Email),
                    dbHelper.CreateParameter("@Address", supplier.Address),
                    dbHelper.CreateParameter("@TaxNumber", supplier.TaxNumber),
                    dbHelper.CreateParameter("@PaymentTerms", supplier.PaymentTerms),
                    dbHelper.CreateParameter("@Status", supplier.Status),
                    dbHelper.CreateParameter("@UpdatedBy", supplier.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateSupplier", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating supplier: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a supplier
        /// </summary>
        public bool DeleteSupplier(int supplierID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SupplierID", supplierID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteSupplier", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting supplier: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a supplier by ID
        /// </summary>
        public Supplier GetSupplierById(int supplierID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SupplierID", supplierID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetSupplierById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToSupplier(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting supplier by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all suppliers for a branch
        /// </summary>
        public List<Supplier> GetSuppliersByBranch(int branchID, bool includeInactive = false)
        {
            try
            {
                List<Supplier> suppliers = new List<Supplier>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetSuppliersByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    suppliers.Add(MapDataRowToSupplier(row));
                }

                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting suppliers by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches suppliers by name, contact, or tax number
        /// </summary>
        public List<Supplier> SearchSuppliers(int branchID, string searchTerm)
        {
            try
            {
                List<Supplier> suppliers = new List<Supplier>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchSuppliers", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    suppliers.Add(MapDataRowToSupplier(row));
                }

                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching suppliers: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps a DataRow to a Supplier object
        /// </summary>
        private Supplier MapDataRowToSupplier(DataRow row)
        {
            return new Supplier
            {
                SupplierID = Convert.ToInt32(row["SupplierID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                SupplierName = row["SupplierName"].ToString(),
                ContactPerson = row["ContactPerson"]?.ToString(),
                Phone = row["Phone"]?.ToString(),
                Email = row["Email"]?.ToString(),
                Address = row["Address"]?.ToString(),
                TaxNumber = row["TaxNumber"]?.ToString(),
                PaymentTerms = row["PaymentTerms"]?.ToString(),
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