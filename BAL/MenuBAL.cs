// ============================================
// BAL/MenuBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Menu BAL - Business logic layer for menu item operations
    /// Contains business rules and validation for menu management
    /// </summary>
    public class MenuBAL
    {
        private readonly MenuDAL menuDAL;
        private readonly CategoryDAL categoryDAL;

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public MenuBAL()
        {
            menuDAL = new MenuDAL();
            categoryDAL = new CategoryDAL();
        }

        /// <summary>
        /// Saves a menu item to the database
        /// </summary>
        public int SaveMenuItem(MenuItem menuItem)
        {
            ValidateMenuItem(menuItem);

            if (IsSKUExist(menuItem.BranchID, menuItem.SKU, menuItem.MenuItemID))
            {
                throw new Exception("A menu item with this SKU already exists.");
            }

            if (IsBarcodeExist(menuItem.BranchID, menuItem.Barcode, menuItem.MenuItemID))
            {
                throw new Exception("A menu item with this barcode already exists.");
            }

            if (menuItem.MenuItemID > 0)
            {
                bool updated = menuDAL.UpdateMenuItem(menuItem);
                if (!updated)
                {
                    throw new Exception("Failed to update menu item.");
                }
                return menuItem.MenuItemID;
            }
            else
            {
                int newID = menuDAL.InsertMenuItem(menuItem);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert menu item.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates menu item data
        /// </summary>
        private void ValidateMenuItem(MenuItem menuItem)
        {
            if (menuItem.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (menuItem.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (menuItem.CategoryID <= 0)
            {
                throw new Exception("Category is required.");
            }

            // Validate category exists
            var category = categoryDAL.GetCategoryById(menuItem.CategoryID);
            if (category == null || category.BranchID != menuItem.BranchID)
            {
                throw new Exception("Category is invalid or belongs to a different branch.");
            }

            if (string.IsNullOrWhiteSpace(menuItem.ItemName))
            {
                throw new Exception("Item name is required.");
            }

            if (menuItem.ItemName.Length > 200)
            {
                throw new Exception("Item name cannot exceed 200 characters.");
            }

            if (menuItem.Price <= 0)
            {
                throw new Exception("Price must be greater than 0.");
            }

            if (menuItem.Cost < 0)
            {
                throw new Exception("Cost cannot be negative.");
            }

            if (menuItem.Tax < 0)
            {
                throw new Exception("Tax cannot be negative.");
            }

            if (menuItem.Discount < 0)
            {
                throw new Exception("Discount cannot be negative.");
            }

            if (menuItem.Discount > 100)
            {
                throw new Exception("Discount cannot exceed 100%.");
            }

            if (menuItem.PreparationTime < 0)
            {
                throw new Exception("Preparation time cannot be negative.");
            }

            if (menuItem.MinStockAlert < 0)
            {
                throw new Exception("Minimum stock alert cannot be negative.");
            }
        }

        /// <summary>
        /// Checks if SKU already exists
        /// </summary>
        public bool IsSKUExist(int branchID, string sku, int excludeMenuItemID = 0)
        {
            if (string.IsNullOrWhiteSpace(sku))
                return false;

            try
            {
                var items = menuDAL.GetMenuItemsByBranch(branchID);
                foreach (var item in items)
                {
                    if (item.MenuItemID != excludeMenuItemID &&
                        !string.IsNullOrWhiteSpace(item.SKU) &&
                        item.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase))
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
        /// Checks if barcode already exists
        /// </summary>
        public bool IsBarcodeExist(int branchID, string barcode, int excludeMenuItemID = 0)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return false;

            try
            {
                var items = menuDAL.GetMenuItemsByBranch(branchID);
                foreach (var item in items)
                {
                    if (item.MenuItemID != excludeMenuItemID &&
                        !string.IsNullOrWhiteSpace(item.Barcode) &&
                        item.Barcode.Equals(barcode, StringComparison.OrdinalIgnoreCase))
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
        /// Deletes a menu item
        /// </summary>
        public bool DeleteMenuItem(int menuItemID, int updatedBy)
        {
            if (menuItemID <= 0)
            {
                throw new Exception("Invalid menu item ID.");
            }

            // Check if menu item is used in any order
            OrderBAL orderBAL = new OrderBAL();
            var orders = orderBAL.GetOrdersByBranch(
                menuDAL.GetMenuItemById(menuItemID).BranchID);
            // In a real implementation, we would check if the item is in any order
            // For now, we'll allow deletion

            return menuDAL.DeleteMenuItem(menuItemID, updatedBy);
        }

        /// <summary>
        /// Gets a menu item by ID
        /// </summary>
        public MenuItem GetMenuItemById(int menuItemID)
        {
            if (menuItemID <= 0)
            {
                throw new Exception("Invalid menu item ID.");
            }

            return menuDAL.GetMenuItemById(menuItemID);
        }

        /// <summary>
        /// Gets all menu items for a branch
        /// </summary>
        public List<MenuItem> GetMenuItemsByBranch(int branchID, int? categoryID = null, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return menuDAL.GetMenuItemsByBranch(branchID, categoryID, includeInactive);
        }

        /// <summary>
        /// Searches menu items
        /// </summary>
        public List<MenuItem> SearchMenuItems(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetMenuItemsByBranch(branchID);
            }

            return menuDAL.SearchMenuItems(branchID, searchTerm);
        }

        /// <summary>
        /// Updates menu item availability
        /// </summary>
        public bool UpdateAvailability(int menuItemID, bool isAvailable, int updatedBy)
        {
            if (menuItemID <= 0)
            {
                throw new Exception("Invalid menu item ID.");
            }

            return menuDAL.UpdateAvailability(menuItemID, isAvailable, updatedBy);
        }

        /// <summary>
        /// Gets total menu items count for a branch
        /// </summary>
        public int GetTotalMenuItems(int branchID)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return menuDAL.GetTotalMenuItems(branchID);
        }

        /// <summary>
        /// Gets menu items by category
        /// </summary>
        public List<MenuItem> GetMenuItemsByCategory(int categoryID)
        {
            if (categoryID <= 0)
            {
                throw new Exception("Invalid category ID.");
            }

            var category = categoryDAL.GetCategoryById(categoryID);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            return menuDAL.GetMenuItemsByBranch(category.BranchID, categoryID);
        }
    }
}