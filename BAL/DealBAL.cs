// ============================================
// BAL/DealBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Deal BAL - Business logic layer for deal operations
    /// Contains business rules and validation for deal management
    /// </summary>
    public class DealBAL
    {
        #region Private Fields

        private readonly DealDAL dealDAL;
        private readonly MenuDAL menuDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public DealBAL()
        {
            dealDAL = new DealDAL();
            menuDAL = new MenuDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Saves a deal to the database
        /// Validates business rules before saving
        /// </summary>
        public int SaveDeal(Deal deal, List<DealItem> items)
        {
            // Validate deal data
            ValidateDeal(deal);

            // Validate deal items
            ValidateDealItems(items);

            // Check for duplicate deal name
            if (dealDAL.DealNameExists(deal.BranchID, deal.DealName,
                deal.DealID > 0 ? (int?)deal.DealID : null))
            {
                throw new Exception("A deal with this name already exists.");
            }

            // Check if deal has items
            if (items == null || items.Count == 0)
            {
                throw new Exception("Deal must have at least one menu item.");
            }

            // Calculate original price for validation
            decimal originalPrice = CalculateOriginalPrice(items);
            if (deal.DealPrice >= originalPrice)
            {
                throw new Exception("Deal price must be less than the total original price.");
            }

            int dealID;

            // Insert or update based on whether ID exists
            if (deal.DealID > 0)
            {
                // Update deal
                bool updated = dealDAL.UpdateDeal(deal);
                if (!updated)
                {
                    throw new Exception("Failed to update deal.");
                }
                dealID = deal.DealID;

                // Delete existing items and re-insert
                // In a real implementation, you would update items individually
                // For simplicity, we'll delete and re-insert
                var existingItems = dealDAL.GetDealItems(dealID);
                foreach (var item in existingItems)
                {
                    dealDAL.DeleteDealItem(item.DealItemID, deal.UpdatedBy);
                }
            }
            else
            {
                // Insert new deal
                dealID = dealDAL.InsertDeal(deal);
                if (dealID <= 0)
                {
                    throw new Exception("Failed to insert deal.");
                }
            }

            // Insert deal items
            foreach (var item in items)
            {
                item.DealID = dealID;
                item.CreatedBy = deal.CreatedBy;
                item.UpdatedBy = deal.UpdatedBy;
                dealDAL.InsertDealItem(item);
            }

            // Log audit
            if (deal.DealID > 0)
            {
                auditLogger.LogUpdate("DealManagement", "Deals", dealID,
                    $"DealID: {deal.DealID}", $"DealName: {deal.DealName}");
            }
            else
            {
                auditLogger.LogInsert("DealManagement", "Deals", dealID,
                    $"DealName: {deal.DealName}");
            }

            return dealID;
        }

        /// <summary>
        /// Deletes a deal
        /// </summary>
        public bool DeleteDeal(int dealID, int updatedBy)
        {
            if (dealID <= 0)
            {
                throw new Exception("Invalid deal ID.");
            }

            // Check if deal exists
            var deal = dealDAL.GetDealById(dealID);
            if (deal == null)
            {
                throw new Exception("Deal not found.");
            }

            // Delete deal items first
            var items = dealDAL.GetDealItems(dealID);
            foreach (var item in items)
            {
                dealDAL.DeleteDealItem(item.DealItemID, updatedBy);
            }

            // Delete deal
            bool deleted = dealDAL.DeleteDeal(dealID, updatedBy);
            if (deleted)
            {
                auditLogger.LogDelete("DealManagement", "Deals", dealID, $"DealID: {dealID}");
            }

            return deleted;
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a deal by ID with all items
        /// </summary>
        public Deal GetDealWithItems(int dealID)
        {
            if (dealID <= 0)
            {
                throw new Exception("Invalid deal ID.");
            }

            Deal deal = dealDAL.GetDealById(dealID);
            if (deal != null)
            {
                deal.DealItems = dealDAL.GetDealItems(dealID);
                deal.OriginalPrice = CalculateOriginalPrice(deal.DealItems);
            }

            return deal;
        }

        /// <summary>
        /// Gets a deal by ID (without items)
        /// </summary>
        public Deal GetDealById(int dealID)
        {
            if (dealID <= 0)
            {
                throw new Exception("Invalid deal ID.");
            }

            return dealDAL.GetDealById(dealID);
        }

        /// <summary>
        /// Gets all deals for a branch
        /// </summary>
        public List<Deal> GetAllDeals(int branchID, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return dealDAL.GetAllDeals(branchID, includeInactive);
        }

        /// <summary>
        /// Gets active deals for a branch
        /// </summary>
        public List<Deal> GetActiveDeals(int branchID)
        {
            return GetAllDeals(branchID, false);
        }

        /// <summary>
        /// Gets valid deals (active and within date range)
        /// </summary>
        public List<Deal> GetValidDeals(int branchID)
        {
            var deals = GetAllDeals(branchID, false);
            var validDeals = new List<Deal>();

            foreach (var deal in deals)
            {
                if (deal.IsValid)
                {
                    validDeals.Add(deal);
                }
            }

            return validDeals;
        }

        /// <summary>
        /// Searches deals
        /// </summary>
        public List<Deal> SearchDeals(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetAllDeals(branchID);
            }

            return dealDAL.SearchDeals(branchID, searchTerm);
        }

        #endregion

        #region Deal Items

        /// <summary>
        /// Gets all items for a deal
        /// </summary>
        public List<DealItem> GetDealItems(int dealID)
        {
            if (dealID <= 0)
            {
                throw new Exception("Invalid deal ID.");
            }

            return dealDAL.GetDealItems(dealID);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validates deal data
        /// </summary>
        private void ValidateDeal(Deal deal)
        {
            if (deal.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (deal.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(deal.DealName))
            {
                throw new Exception("Deal name is required.");
            }

            if (deal.DealName.Length > 200)
            {
                throw new Exception("Deal name cannot exceed 200 characters.");
            }

            if (deal.DealPrice <= 0)
            {
                throw new Exception("Deal price must be greater than 0.");
            }

            if (deal.Description != null && deal.Description.Length > 500)
            {
                throw new Exception("Description cannot exceed 500 characters.");
            }

            if (deal.StartDate.HasValue && deal.EndDate.HasValue && deal.StartDate > deal.EndDate)
            {
                throw new Exception("Start date cannot be after end date.");
            }
        }

        /// <summary>
        /// Validates deal items
        /// </summary>
        private void ValidateDealItems(List<DealItem> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new Exception("Deal must have at least one menu item.");
            }

            foreach (var item in items)
            {
                if (item.MenuItemID <= 0)
                {
                    throw new Exception("Invalid menu item.");
                }

                if (item.Quantity <= 0)
                {
                    throw new Exception("Quantity must be greater than 0.");
                }

                // Validate menu item exists
                var menuItem = menuDAL.GetMenuItemById(item.MenuItemID);
                if (menuItem == null)
                {
                    throw new Exception($"Menu item not found.");
                }
            }

            // Check for duplicate menu items
            var menuItemIds = new List<int>();
            foreach (var item in items)
            {
                if (menuItemIds.Contains(item.MenuItemID))
                {
                    throw new Exception($"Duplicate menu item found.");
                }
                menuItemIds.Add(item.MenuItemID);
            }
        }

        /// <summary>
        /// Calculates the original price of all items in the deal
        /// </summary>
        private decimal CalculateOriginalPrice(List<DealItem> items)
        {
            decimal total = 0;
            foreach (var item in items)
            {
                var menuItem = menuDAL.GetMenuItemById(item.MenuItemID);
                if (menuItem != null)
                {
                    total += menuItem.Price * item.Quantity;
                }
            }
            return total;
        }

        #endregion
    }
}