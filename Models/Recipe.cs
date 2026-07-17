// ============================================
// Models/Recipe.cs
// ============================================

using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Recipe Model - Manages recipes for menu items
    /// </summary>
    public class Recipe
    {
        public int RecipeID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int MenuItemID { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public int Yield { get; set; }
        public int? PreparationTime { get; set; } // Minutes
        public int? CookingTime { get; set; } // Minutes
        public string Instructions { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string MenuItemName { get; set; }
        public string BranchName { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
        public decimal TotalCost { get; set; }
    }

    /// <summary>
    /// Recipe Ingredient Model
    /// </summary>
    public class RecipeIngredient
    {
        public int RecipeIngredientID { get; set; }
        public int RecipeID { get; set; }
        public int StockID { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } // kg, g, ml, l, pieces, etc.
        public decimal? Cost { get; set; }
        public bool IsWaste { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost { get; set; }
    }
}