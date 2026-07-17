// ============================================
// BAL/CompanyBAL.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Company BAL - Business logic layer for company operations
    /// </summary>
    public class CompanyBAL
    {
        #region Private Fields

        private readonly CompanyDAL companyDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public CompanyBAL()
        {
            companyDAL = new CompanyDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Saves a company to the database
        /// </summary>
        public int SaveCompany(Company company)
        {
            ValidateCompany(company);

            if (IsCompanyNameExist(company.CompanyName, company.CompanyID))
            {
                throw new Exception("A company with this name already exists.");
            }

            if (company.CompanyID > 0)
            {
                bool updated = companyDAL.UpdateCompany(company);
                if (!updated)
                {
                    throw new Exception("Failed to update company.");
                }

                auditLogger.LogUpdate("CompanyManagement", "Companies", company.CompanyID,
                    $"CompanyID: {company.CompanyID}", $"CompanyName: {company.CompanyName}");

                return company.CompanyID;
            }
            else
            {
                int newID = companyDAL.InsertCompany(company);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert company.");
                }

                auditLogger.LogInsert("CompanyManagement", "Companies", newID,
                    $"CompanyName: {company.CompanyName}");

                return newID;
            }
        }

        /// <summary>
        /// Deletes a company
        /// </summary>
        public bool DeleteCompany(int companyID, int updatedBy)
        {
            if (companyID <= 0)
            {
                throw new Exception("Invalid company ID.");
            }

            // Check if company has branches
            BranchBAL branchBAL = new BranchBAL();
            List<Branch> branches = branchBAL.GetBranchesByCompany(companyID);
            if (branches.Count > 0)
            {
                throw new Exception("Cannot delete company with existing branches. Delete all branches first.");
            }

            bool deleted = companyDAL.DeleteCompany(companyID, updatedBy);
            if (deleted)
            {
                auditLogger.LogDelete("CompanyManagement", "Companies", companyID,
                    $"CompanyID: {companyID}");
            }

            return deleted;
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a company by ID
        /// </summary>
        public Company GetCompanyById(int companyID)
        {
            if (companyID <= 0)
            {
                throw new Exception("Invalid company ID.");
            }

            return companyDAL.GetCompanyById(companyID);
        }

        /// <summary>
        /// Gets all companies
        /// </summary>
        public List<Company> GetAllCompanies()
        {
            return companyDAL.GetAllCompanies();
        }

        /// <summary>
        /// Searches companies
        /// </summary>
        public List<Company> SearchCompanies(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetAllCompanies();
            }

            return companyDAL.SearchCompanies(searchTerm);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validates company data
        /// </summary>
        private void ValidateCompany(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.CompanyName))
            {
                throw new Exception("Company name is required.");
            }

            if (company.CompanyName.Length > 200)
            {
                throw new Exception("Company name cannot exceed 200 characters.");
            }

            if (!string.IsNullOrEmpty(company.Email) && !ValidationHelper.IsValidEmail(company.Email))
            {
                throw new Exception("Invalid email format.");
            }
        }

        /// <summary>
        /// Checks if a company name already exists
        /// </summary>
        public bool IsCompanyNameExist(string companyName, int excludeCompanyID = 0)
        {
            try
            {
                List<Company> companies = companyDAL.GetAllCompanies();
                foreach (Company company in companies)
                {
                    if (company.CompanyID != excludeCompanyID &&
                        company.CompanyName.Equals(companyName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}