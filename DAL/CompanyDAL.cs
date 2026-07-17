// ============================================
// DAL/CompanyDAL.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Company DAL - Data access layer for company operations
    /// </summary>
    public class CompanyDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public CompanyDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Inserts a new company into the database
        /// </summary>
        public int InsertCompany(Company company)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyName", company.CompanyName),
                    dbHelper.CreateParameter("@Address", company.Address),
                    dbHelper.CreateParameter("@Phone", company.Phone),
                    dbHelper.CreateParameter("@Email", company.Email),
                    dbHelper.CreateParameter("@NTN", company.NTN),
                    dbHelper.CreateParameter("@GST", company.GST),
                    dbHelper.CreateParameter("@Logo", company.Logo),
                    dbHelper.CreateParameter("@CurrencyCode", company.CurrencyCode),
                    dbHelper.CreateParameter("@CurrencySymbol", company.CurrencySymbol),
                    dbHelper.CreateParameter("@TimeZone", company.TimeZone),
                    dbHelper.CreateParameter("@Status", company.Status),
                    dbHelper.CreateParameter("@CreatedBy", company.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertCompany", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing company
        /// </summary>
        public bool UpdateCompany(Company company)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", company.CompanyID),
                    dbHelper.CreateParameter("@CompanyName", company.CompanyName),
                    dbHelper.CreateParameter("@Address", company.Address),
                    dbHelper.CreateParameter("@Phone", company.Phone),
                    dbHelper.CreateParameter("@Email", company.Email),
                    dbHelper.CreateParameter("@NTN", company.NTN),
                    dbHelper.CreateParameter("@GST", company.GST),
                    dbHelper.CreateParameter("@Logo", company.Logo),
                    dbHelper.CreateParameter("@CurrencyCode", company.CurrencyCode),
                    dbHelper.CreateParameter("@CurrencySymbol", company.CurrencySymbol),
                    dbHelper.CreateParameter("@TimeZone", company.TimeZone),
                    dbHelper.CreateParameter("@Status", company.Status),
                    dbHelper.CreateParameter("@UpdatedBy", company.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateCompany", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a company
        /// </summary>
        public bool DeleteCompany(int companyID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteCompany", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting company: {ex.Message}", ex);
            }
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a company by ID
        /// </summary>
        public Company GetCompanyById(int companyID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetCompanyById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToCompany(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting company by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all companies
        /// </summary>
        public List<Company> GetAllCompanies()
        {
            try
            {
                List<Company> companies = new List<Company>();
                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetAllCompanies");

                foreach (DataRow row in dataTable.Rows)
                {
                    companies.Add(MapDataRowToCompany(row));
                }

                return companies;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all companies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches companies
        /// </summary>
        public List<Company> SearchCompanies(string searchTerm)
        {
            try
            {
                List<Company> companies = new List<Company>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchCompanies", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    companies.Add(MapDataRowToCompany(row));
                }

                return companies;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching companies: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Company object
        /// </summary>
        private Company MapDataRowToCompany(DataRow row)
        {
            return new Company
            {
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                CompanyName = row["CompanyName"].ToString(),
                Address = row["Address"]?.ToString(),
                Phone = row["Phone"]?.ToString(),
                Email = row["Email"]?.ToString(),
                NTN = row["NTN"]?.ToString(),
                GST = row["GST"]?.ToString(),
                Logo = row["Logo"]?.ToString(),
                CurrencyCode = row["CurrencyCode"]?.ToString() ?? "USD",
                CurrencySymbol = row["CurrencySymbol"]?.ToString() ?? "$",
                TimeZone = row["TimeZone"]?.ToString() ?? "UTC",
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
        }

        #endregion
    }
}