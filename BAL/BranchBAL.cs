// ============================================
// BAL/BranchBAL.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Branch BAL - Business logic layer for branch operations
    /// </summary>
    public class BranchBAL
    {
        #region Private Fields

        private readonly BranchDAL branchDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public BranchBAL()
        {
            branchDAL = new BranchDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Saves a branch to the database
        /// </summary>
        public int SaveBranch(Branch branch)
        {
            ValidateBranch(branch);

            if (IsBranchNameExist(branch.CompanyID, branch.BranchName, branch.BranchID))
            {
                throw new Exception("A branch with this name already exists in this company.");
            }

            if (branch.BranchID > 0)
            {
                bool updated = branchDAL.UpdateBranch(branch);
                if (!updated)
                {
                    throw new Exception("Failed to update branch.");
                }

                auditLogger.LogUpdate("BranchManagement", "Branches", branch.BranchID,
                    $"BranchID: {branch.BranchID}", $"BranchName: {branch.BranchName}");

                return branch.BranchID;
            }
            else
            {
                int newID = branchDAL.InsertBranch(branch);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert branch.");
                }

                auditLogger.LogInsert("BranchManagement", "Branches", newID,
                    $"BranchName: {branch.BranchName}");

                return newID;
            }
        }

        /// <summary>
        /// Deletes a branch
        /// </summary>
        public bool DeleteBranch(int branchID, int updatedBy)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            // Check if branch has orders
            OrderBAL orderBAL = new OrderBAL();
            var orders = orderBAL.GetOrdersByBranch(branchID);
            if (orders.Count > 0)
            {
                throw new Exception("Cannot delete branch with existing orders.");
            }

            bool deleted = branchDAL.DeleteBranch(branchID, updatedBy);
            if (deleted)
            {
                auditLogger.LogDelete("BranchManagement", "Branches", branchID,
                    $"BranchID: {branchID}");
            }

            return deleted;
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a branch by ID
        /// </summary>
        public Branch GetBranchById(int branchID)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return branchDAL.GetBranchById(branchID);
        }

        /// <summary>
        /// Gets all branches for a company
        /// </summary>
        public List<Branch> GetBranchesByCompany(int companyID)
        {
            if (companyID <= 0)
            {
                throw new Exception("Invalid company ID.");
            }

            return branchDAL.GetBranchesByCompany(companyID);
        }

        /// <summary>
        /// Gets all branches
        /// </summary>
        public List<Branch> GetAllBranches()
        {
            return branchDAL.GetAllBranches();
        }

        /// <summary>
        /// Searches branches
        /// </summary>
        public List<Branch> SearchBranches(string searchTerm, int? companyID = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return companyID.HasValue ? GetBranchesByCompany(companyID.Value) : GetAllBranches();
            }

            return branchDAL.SearchBranches(searchTerm, companyID);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validates branch data
        /// </summary>
        private void ValidateBranch(Branch branch)
        {
            if (branch.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (string.IsNullOrWhiteSpace(branch.BranchName))
            {
                throw new Exception("Branch name is required.");
            }

            if (branch.BranchName.Length > 200)
            {
                throw new Exception("Branch name cannot exceed 200 characters.");
            }

            if (!string.IsNullOrEmpty(branch.Email) && !ValidationHelper.IsValidEmail(branch.Email))
            {
                throw new Exception("Invalid email format.");
            }
        }

        /// <summary>
        /// Checks if a branch name already exists in a company
        /// </summary>
        public bool IsBranchNameExist(int companyID, string branchName, int excludeBranchID = 0)
        {
            try
            {
                List<Branch> branches = branchDAL.GetBranchesByCompany(companyID);
                foreach (Branch branch in branches)
                {
                    if (branch.BranchID != excludeBranchID &&
                        branch.BranchName.Equals(branchName, StringComparison.OrdinalIgnoreCase))
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