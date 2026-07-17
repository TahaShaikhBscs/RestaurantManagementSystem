// ============================================
// BAL/CategoryBAL.cs - Complete Updated
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Category BAL - Business logic layer for category operations
    /// Contains business rules and validation for category management
    /// </summary>
    public class CategoryBAL
    {
        #region Private Fields

        private readonly CategoryDAL categoryDAL;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public CategoryBAL()
        {
            categoryDAL = new CategoryDAL();
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Saves a category to the database
        /// Validates business rules before saving
        /// </summary>
        public int SaveCategory(Category category)
        {
            // Validate category data
            ValidateCategory(category);

            // Check for duplicate category name
            if (categoryDAL.CategoryNameExists(category.BranchID, category.CategoryName,
                category.CategoryID > 0 ? (int?)category.CategoryID : null))
            {
                throw new Exception("A category with this name already exists in this branch.");
            }

            // Check for circular parent reference
            //if (category.ParentCategoryID.HasValue)
            //{
            //    // Check if parent is itself
            //    //if (category.ParentCategoryID.Value == category.CategoryID)
            //    //{
            //    //    throw new Exception("Category cannot be its own parent.");
            //    //}

            //    // Check for circular reference
            //    if (categoryDAL.IsCircularParentReference(category.CategoryID, category.ParentCategoryID.Value))
            //    {
            //        throw new Exception("Circular parent reference detected. Please select a different parent category.");
            //    }
            //}

            // Insert or update based on whether ID exists
            if (category.CategoryID > 0)
            {
                bool updated = categoryDAL.UpdateCategory(category);
                if (!updated)
                {
                    throw new Exception("Failed to update category.");
                }
                return category.CategoryID;
            }
            else
            {
                int newID = categoryDAL.InsertCategory(category);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert category.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        public bool DeleteCategory(int categoryID, int updatedBy)
        {
            if (categoryID <= 0)
            {
                throw new Exception("Invalid category ID.");
            }

            // Get the category
            Category category = categoryDAL.GetCategoryById(categoryID);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            // Check if category has sub-categories
            List<Category> subCategories = categoryDAL.GetCategoriesByBranch(category.BranchID);
            foreach (var sub in subCategories)
            {
                if (sub.ParentCategoryID == categoryID)
                {
                    throw new Exception("Cannot delete category with sub-categories. Delete sub-categories first.");
                }
            }

            // Check if category has menu items
            MenuDAL menuDAL = new MenuDAL();
            var menuItems = menuDAL.GetMenuItemsByBranch(category.BranchID, categoryID);
            if (menuItems.Count > 0)
            {
                throw new Exception("Cannot delete category with menu items. Move or delete menu items first.");
            }

            return categoryDAL.DeleteCategory(categoryID, updatedBy);
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets a category by ID
        /// </summary>
        public Category GetCategoryById(int categoryID)
        {
            if (categoryID <= 0)
            {
                throw new Exception("Invalid category ID.");
            }

            return categoryDAL.GetCategoryById(categoryID);
        }

        /// <summary>
        /// Gets all categories for a branch
        /// </summary>
        public List<Category> GetCategoriesByBranch(int branchID, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return categoryDAL.GetCategoriesByBranch(branchID, includeInactive);
        }

        /// <summary>
        /// Gets all categories for a company
        /// </summary>
        public List<Category> GetCategoriesByCompany(int companyID, bool includeInactive = false)
        {
            if (companyID <= 0)
            {
                throw new Exception("Invalid company ID.");
            }

            return categoryDAL.GetCategoriesByCompany(companyID, includeInactive);
        }

        /// <summary>
        /// Gets root categories (categories with no parent)
        /// </summary>
        public List<Category> GetRootCategories(int branchID, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return categoryDAL.GetRootCategories(branchID, includeInactive);
        }

        /// <summary>
        /// Searches categories
        /// </summary>
        public List<Category> SearchCategories(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetCategoriesByBranch(branchID);
            }

            return categoryDAL.SearchCategories(branchID, searchTerm);
        }

        /// <summary>
        /// Gets category tree for display
        /// </summary>
        public List<Category> GetCategoryTree(int branchID)
        {
            return GetRootCategories(branchID, false);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validates category data
        /// </summary>
        private void ValidateCategory(Category category)
        {
            if (category.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (category.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new Exception("Category name is required.");
            }

            if (category.CategoryName.Length > 200)
            {
                throw new Exception("Category name cannot exceed 200 characters.");
            }

            if (category.Description != null && category.Description.Length > 500)
            {
                throw new Exception("Description cannot exceed 500 characters.");
            }

            if (category.DisplayOrder < 0)
            {
                throw new Exception("Display order cannot be negative.");
            }

            if (!string.IsNullOrEmpty(category.Icon) && category.Icon.Length > 100)
            {
                throw new Exception("Icon name cannot exceed 100 characters.");
            }

            if (!string.IsNullOrEmpty(category.Image) && category.Image.Length > 500)
            {
                throw new Exception("Image path cannot exceed 500 characters.");
            }

            // Check if parent category exists and belongs to same branch
            //if (category.ParentCategoryID.HasValue)
            //{
            //    var parent = categoryDAL.GetCategoryById(category.ParentCategoryID.Value);
            //    if (parent == null || parent.BranchID != category.BranchID)
            //    {
            //        throw new Exception("Parent category is invalid or belongs to a different branch.");
            //    }
            //}
        }

        #endregion
    }
}