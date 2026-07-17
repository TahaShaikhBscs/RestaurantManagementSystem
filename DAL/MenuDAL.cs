// ============================================
// DAL/MenuDAL.cs
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
    /// Menu DAL - Data access layer for menu item operations
    /// Handles all database operations for menu items
    /// </summary>
    public class MenuDAL
    {
        private readonly DatabaseHelper dbHelper;

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public MenuDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Inserts a new menu item into the database
        /// </summary>
        /// <param name="menuItem">MenuItem object to insert</param>
        /// <returns>ID of the newly inserted menu item</returns>
        public int InsertMenuItem(MenuItem menuItem)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", menuItem.CompanyID),
                    dbHelper.CreateParameter("@BranchID", menuItem.BranchID),
                    dbHelper.CreateParameter("@CategoryID", menuItem.CategoryID),
                    dbHelper.CreateParameter("@ItemName", menuItem.ItemName),
                    dbHelper.CreateParameter("@Description", menuItem.Description),
                    dbHelper.CreateParameter("@Price", menuItem.Price),
                    dbHelper.CreateParameter("@Cost", menuItem.Cost),
                    dbHelper.CreateParameter("@Tax", menuItem.Tax),
                    dbHelper.CreateParameter("@Discount", menuItem.Discount),
                    dbHelper.CreateParameter("@Barcode", menuItem.Barcode),
                    dbHelper.CreateParameter("@SKU", menuItem.SKU),
                    dbHelper.CreateParameter("@Image", menuItem.Image),
                    dbHelper.CreateParameter("@PreparationTime", menuItem.PreparationTime),
                    dbHelper.CreateParameter("@IsKitchen", menuItem.IsKitchen),
                    dbHelper.CreateParameter("@IsAvailable", menuItem.IsAvailable),
                    dbHelper.CreateParameter("@IsFeatured", menuItem.IsFeatured),
                    dbHelper.CreateParameter("@MinStockAlert", menuItem.MinStockAlert),
                    dbHelper.CreateParameter("@Status", menuItem.Status),
                    dbHelper.CreateParameter("@AlterdBy", menuItem.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertMenuItem", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting menu item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing menu item
        /// </summary>
        /// <param name="menuItem">MenuItem object with updated data</param>
        /// <returns>True if update successful</returns>
        public bool UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@MenuItemID", menuItem.MenuItemID),
                    dbHelper.CreateParameter("@CategoryID", menuItem.CategoryID),
                    dbHelper.CreateParameter("@ItemName", menuItem.ItemName),
                    dbHelper.CreateParameter("@Description", menuItem.Description),
                    dbHelper.CreateParameter("@Price", menuItem.Price),
                    dbHelper.CreateParameter("@Cost", menuItem.Cost),
                    dbHelper.CreateParameter("@Tax", menuItem.Tax),
                    dbHelper.CreateParameter("@Discount", menuItem.Discount),
                    dbHelper.CreateParameter("@Barcode", menuItem.Barcode),
                    dbHelper.CreateParameter("@SKU", menuItem.SKU),
                    dbHelper.CreateParameter("@Image", menuItem.Image),
                    dbHelper.CreateParameter("@PreparationTime", menuItem.PreparationTime),
                    dbHelper.CreateParameter("@IsKitchen", menuItem.IsKitchen),
                    dbHelper.CreateParameter("@IsAvailable", menuItem.IsAvailable),
                    dbHelper.CreateParameter("@IsFeatured", menuItem.IsFeatured),
                    dbHelper.CreateParameter("@MinStockAlert", menuItem.MinStockAlert),
                    dbHelper.CreateParameter("@Status", menuItem.Status),
                    dbHelper.CreateParameter("@UpdatedBy", menuItem.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateMenuItem", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a menu item (marks as deleted)
        /// </summary>
        /// <param name="menuItemID">ID of the menu item to delete</param>
        /// <param name="updatedBy">User ID performing the deletion</param>
        /// <returns>True if deletion successful</returns>
        public bool DeleteMenuItem(int menuItemID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@MenuItemID", menuItemID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteMenuItem", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a menu item by ID
        /// </summary>
        /// <param name="menuItemID">Menu item ID</param>
        /// <returns>MenuItem object</returns>
        public MenuItem GetMenuItemById(int menuItemID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@MenuItemID", menuItemID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetMenuItemById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToMenuItem(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting menu item by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all menu items for a branch
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="categoryID">Optional category ID filter</param>
        /// <param name="includeInactive">Include inactive items</param>
        /// <returns>List of menu items</returns>
        public List<MenuItem> GetMenuItemsByBranch(int branchID, int? categoryID = null, bool includeInactive = false)
        {
            try
            {
                List<MenuItem> items = new List<MenuItem>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@CategoryID", categoryID),
                    dbHelper.CreateParameter("@IncludeInactive", includeInactive)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetMenuItemsByBranch", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    items.Add(MapDataRowToMenuItem(row));
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting menu items by branch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches menu items by name, barcode, or SKU
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching menu items</returns>
        public List<MenuItem> SearchMenuItems(int branchID, string searchTerm)
        {
            try
            {
                List<MenuItem> items = new List<MenuItem>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@SearchTerm", searchTerm)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_SearchMenuItems", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    items.Add(MapDataRowToMenuItem(row));
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching menu items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates menu item availability
        /// </summary>
        /// <param name="menuItemID">Menu item ID</param>
        /// <param name="isAvailable">Availability status</param>
        /// <param name="updatedBy">User ID performing the update</param>
        /// <returns>True if update successful</returns>
        public bool UpdateAvailability(int menuItemID, bool isAvailable, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@MenuItemID", menuItemID),
                    dbHelper.CreateParameter("@IsAvailable", isAvailable),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateMenuItemAvailability", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu item availability: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the total count of menu items for a branch
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="activeOnly">Count only active items</param>
        /// <returns>Total count</returns>
        public int GetTotalMenuItems(int branchID, bool activeOnly = true)
        {
            try
            {
                var items = GetMenuItemsByBranch(branchID, null, !activeOnly);
                return items.Count;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to a MenuItem object
        /// </summary>
        /// <param name="row">DataRow containing menu item data</param>
        /// <returns>MenuItem object</returns>
        private MenuItem MapDataRowToMenuItem(DataRow row)
        {
            return new MenuItem
            {
                MenuItemID = row.GetInt("MenuItemID"),
                CompanyID = row.GetInt("CompanyID"),
                BranchID = row.GetInt("BranchID"),
                CategoryID = row.GetInt("CategoryID"),

                ItemName = row.GetString("ItemName"),
                Description = row.GetString("Description"),

                Price = row.GetDecimal("Price"),
                Cost = row.GetDecimal("Cost"),
                Tax = row.GetDecimal("Tax"),
                Discount = row.GetDecimal("Discount"),

                Barcode = row.GetString("Barcode"),
                SKU = row.GetString("SKU"),
                Image = row.GetString("Image"),

                PreparationTime = row.GetInt("PreparationTime"),

                IsKitchen = row.GetBool("IsKitchen"),
                IsAvailable = row.GetBool("IsAvailable"),
                IsFeatured = row.GetBool("IsFeatured"),

                MinStockAlert = row.GetInt("MinStockAlert"),

                Status = row.GetBool("Status"),

                CreatedDate = row.GetDateTime("CreatedDate"),
                UpdatedDate = row.GetDateTime("UpdatedDate"),

                CreatedBy = row.GetInt("CreatedBy"),
                UpdatedBy = row.GetInt("UpdatedBy"),

                IsDeleted = row.GetBool("IsDeleted"),

                CategoryName = row.GetString("CategoryName")
            };
        }
        //private MenuItem MapDataRowToMenuItem(DataRow row)
        //{
        //    return new MenuItem
        //    {
        //        MenuItemID = Convert.ToInt32(row["MenuItemID"]),
        //        CompanyID = Convert.ToInt32(row["CompanyID"]),
        //        BranchID = Convert.ToInt32(row["BranchID"]),
        //        CategoryID = Convert.ToInt32(row["CategoryID"]),
        //        ItemName = row["ItemName"].ToString(),
        //        Description = row["Description"]?.ToString(),
        //        Price = Convert.ToDecimal(row["Price"]),
        //        Cost = row["Cost"] != DBNull.Value ? Convert.ToDecimal(row["Cost"]) : 0,
        //        Tax = Convert.ToDecimal(row["Tax"]),
        //        Discount = Convert.ToDecimal(row["Discount"]),
        //        Barcode = row["Barcode"]?.ToString(),
        //        SKU = row["SKU"]?.ToString(),
        //        Image = row["Image"]?.ToString(),
        //        PreparationTime = Convert.ToInt32(row["PreparationTime"]),
        //        IsKitchen = Convert.ToBoolean(row["IsKitchen"]),
        //        IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
        //        IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
        //        MinStockAlert = Convert.ToInt32(row["MinStockAlert"]),
        //        Status = Convert.ToBoolean(row["Status"]),
        //        CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
        //        UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
        //        CreatedBy = Convert.ToInt32(row["CreatedBy"]),
        //        UpdatedBy = Convert.ToInt32(row["UpdatedBy"]),
        //        IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
        //        CategoryName = row.Table.Columns.Contains("CategoryName") ? row["CategoryName"]?.ToString() : null
        //    };
        //}
    }
}