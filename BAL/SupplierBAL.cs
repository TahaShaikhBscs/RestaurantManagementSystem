// ============================================
// BAL/SupplierBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Supplier BAL - Business logic layer for supplier operations
    /// Contains business rules and validation for supplier management
    /// </summary>
    public class SupplierBAL
    {
        private readonly SupplierDAL supplierDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public SupplierBAL()
        {
            supplierDAL = new SupplierDAL();
        }

        /// <summary>
        /// Saves a supplier to the database
        /// </summary>
        public int SaveSupplier(Supplier supplier)
        {
            ValidateSupplier(supplier);

            // Check for duplicate supplier name
            if (IsSupplierNameExist(supplier.BranchID, supplier.SupplierName, supplier.SupplierID))
            {
                throw new Exception("A supplier with this name already exists.");
            }

            // Check for duplicate tax number
            if (!string.IsNullOrWhiteSpace(supplier.TaxNumber) &&
                IsTaxNumberExist(supplier.BranchID, supplier.TaxNumber, supplier.SupplierID))
            {
                throw new Exception("A supplier with this tax number already exists.");
            }

            if (supplier.SupplierID > 0)
            {
                bool updated = supplierDAL.UpdateSupplier(supplier);
                if (!updated)
                {
                    throw new Exception("Failed to update supplier.");
                }
                return supplier.SupplierID;
            }
            else
            {
                int newID = supplierDAL.InsertSupplier(supplier);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert supplier.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates supplier data
        /// </summary>
        private void ValidateSupplier(Supplier supplier)
        {
            if (supplier.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (supplier.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
            {
                throw new Exception("Supplier name is required.");
            }

            if (supplier.SupplierName.Length > 200)
            {
                throw new Exception("Supplier name cannot exceed 200 characters.");
            }

            if (supplier.Email != null && !IsValidEmail(supplier.Email))
            {
                throw new Exception("Invalid email format.");
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
        /// Checks if a supplier name already exists
        /// </summary>
        public bool IsSupplierNameExist(int branchID, string supplierName, int excludeSupplierID = 0)
        {
            try
            {
                List<Supplier> suppliers = supplierDAL.GetSuppliersByBranch(branchID);
                foreach (Supplier supplier in suppliers)
                {
                    if (supplier.SupplierID != excludeSupplierID &&
                        supplier.SupplierName.Equals(supplierName, StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// Checks if a tax number already exists
        /// </summary>
        public bool IsTaxNumberExist(int branchID, string taxNumber, int excludeSupplierID = 0)
        {
            try
            {
                List<Supplier> suppliers = supplierDAL.GetSuppliersByBranch(branchID);
                foreach (Supplier supplier in suppliers)
                {
                    if (supplier.SupplierID != excludeSupplierID &&
                        !string.IsNullOrWhiteSpace(supplier.TaxNumber) &&
                        supplier.TaxNumber.Equals(taxNumber, StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// Deletes a supplier
        /// </summary>
        public bool DeleteSupplier(int supplierID, int updatedBy)
        {
            if (supplierID <= 0)
            {
                throw new Exception("Invalid supplier ID.");
            }

            // Check if supplier has purchase orders
            PurchaseBAL purchaseBAL = new PurchaseBAL();
            var purchaseOrders = purchaseBAL.GetPurchaseOrdersBySupplier(supplierID);
            if (purchaseOrders.Count > 0)
            {
                throw new Exception("Cannot delete supplier with existing purchase orders.");
            }

            return supplierDAL.DeleteSupplier(supplierID, updatedBy);
        }

        /// <summary>
        /// Gets a supplier by ID
        /// </summary>
        public Supplier GetSupplierById(int supplierID)
        {
            if (supplierID <= 0)
            {
                throw new Exception("Invalid supplier ID.");
            }

            return supplierDAL.GetSupplierById(supplierID);
        }

        /// <summary>
        /// Gets all suppliers for a branch
        /// </summary>
        public List<Supplier> GetSuppliersByBranch(int branchID, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return supplierDAL.GetSuppliersByBranch(branchID, includeInactive);
        }

        /// <summary>
        /// Searches suppliers
        /// </summary>
        public List<Supplier> SearchSuppliers(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetSuppliersByBranch(branchID);
            }

            return supplierDAL.SearchSuppliers(branchID, searchTerm);
        }

        /// <summary>
        /// Gets active suppliers
        /// </summary>
        public List<Supplier> GetActiveSuppliers(int branchID)
        {
            return GetSuppliersByBranch(branchID, false);
        }
    }
}