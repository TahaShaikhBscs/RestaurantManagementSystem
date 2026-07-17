// ============================================
// BAL/TableBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Table BAL - Business logic layer for restaurant table operations
    /// Contains business rules and validation for table management
    /// </summary>
    public class TableBAL
    {
        private readonly TableDAL tableDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public TableBAL()
        {
            tableDAL = new TableDAL();
        }

        /// <summary>
        /// Saves a table to the database
        /// </summary>
        public int SaveTable(RestaurantTable table)
        {
            ValidateTable(table);

            if (IsTableNumberExist(table.BranchID, table.TableNumber, table.TableID))
            {
                throw new Exception("A table with this number already exists in this branch.");
            }

            if (table.TableID > 0)
            {
                bool updated = tableDAL.UpdateTable(table);
                if (!updated)
                {
                    throw new Exception("Failed to update table.");
                }
                return table.TableID;
            }
            else
            {
                int newID = tableDAL.InsertTable(table);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert table.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates table data
        /// </summary>
        private void ValidateTable(RestaurantTable table)
        {
            if (table.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (table.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(table.TableNumber))
            {
                throw new Exception("Table number is required.");
            }

            if (table.TableNumber.Length > 50)
            {
                throw new Exception("Table number cannot exceed 50 characters.");
            }

            if (table.Capacity <= 0)
            {
                throw new Exception("Capacity must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(table.Status))
            {
                table.Status = "Available";
            }

            string[] validStatuses = { "Available", "Occupied", "Reserved", "Cleaning" };
            if (!Array.Exists(validStatuses, s => s == table.Status))
            {
                throw new Exception("Invalid table status.");
            }
        }

        /// <summary>
        /// Checks if a table number already exists in a branch
        /// </summary>
        public bool IsTableNumberExist(int branchID, string tableNumber, int excludeTableID = 0)
        {
            try
            {
                List<RestaurantTable> tables = tableDAL.GetTablesByBranch(branchID);
                foreach (RestaurantTable table in tables)
                {
                    if (table.TableID != excludeTableID &&
                        table.TableNumber.Equals(tableNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking table number: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a table
        /// </summary>
        public bool DeleteTable(int tableID, int updatedBy)
        {
            if (tableID <= 0)
            {
                throw new Exception("Invalid table ID.");
            }

            // Check if table is occupied
            var table = tableDAL.GetTableById(tableID);
            if (table == null)
            {
                throw new Exception("Table not found.");
            }

            if (table.Status == "Occupied")
            {
                throw new Exception("Cannot delete an occupied table.");
            }

            return tableDAL.DeleteTable(tableID, updatedBy);
        }

        /// <summary>
        /// Gets a table by ID
        /// </summary>
        public RestaurantTable GetTableById(int tableID)
        {
            if (tableID <= 0)
            {
                throw new Exception("Invalid table ID.");
            }

            return tableDAL.GetTableById(tableID);
        }

        /// <summary>
        /// Gets all tables for a branch
        /// </summary>
        public List<RestaurantTable> GetTablesByBranch(int branchID, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return tableDAL.GetTablesByBranch(branchID, includeInactive);
        }

        /// <summary>
        /// Gets available tables for a branch
        /// </summary>
        public List<RestaurantTable> GetAvailableTables(int branchID, int? capacity = null)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return tableDAL.GetAvailableTables(branchID, capacity);
        }

        /// <summary>
        /// Updates table status
        /// </summary>
        public bool UpdateTableStatus(int tableID, string status, int updatedBy)
        {
            if (tableID <= 0)
            {
                throw new Exception("Invalid table ID.");
            }

            string[] validStatuses = { "Available", "Occupied", "Reserved", "Cleaning" };
            if (!Array.Exists(validStatuses, s => s == status))
            {
                throw new Exception("Invalid table status.");
            }

            return tableDAL.UpdateTableStatus(tableID, status, updatedBy);
        }

        /// <summary>
        /// Searches tables
        /// </summary>
        public List<RestaurantTable> SearchTables(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetTablesByBranch(branchID);
            }

            return tableDAL.SearchTables(branchID, searchTerm);
        }
    }
}