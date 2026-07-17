// ============================================
// BAL/StockBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using System.Data;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Stock BAL - Business logic layer for stock and inventory operations
    /// Contains business rules and validation for stock management
    /// </summary>
    public class StockBAL
    {
        private readonly StockDAL stockDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public StockBAL()
        {
            stockDAL = new StockDAL();
        }

        /// <summary>
        /// Saves a stock item to the database
        /// </summary>
        public int SaveStock(Stock stock)
        {
            ValidateStock(stock);

            // Check for duplicate barcode
            if (!string.IsNullOrWhiteSpace(stock.Barcode) &&
                IsBarcodeExist(stock.BranchID, stock.Barcode, stock.StockID))
            {
                throw new Exception("A stock item with this barcode already exists.");
            }

            // Calculate total cost
            stock.TotalCost = stock.Quantity * stock.UnitPrice;

            if (stock.StockID > 0)
            {
                bool updated = stockDAL.UpdateStock(stock);
                if (!updated)
                {
                    throw new Exception("Failed to update stock.");
                }
                return stock.StockID;
            }
            else
            {
                int newID = stockDAL.InsertStock(stock);
                if (newID <= 0)
                {
                    throw new Exception("Failed to insert stock.");
                }
                return newID;
            }
        }

        /// <summary>
        /// Validates stock data
        /// </summary>
        private void ValidateStock(Stock stock)
        {
            if (stock.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (stock.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(stock.ProductName))
            {
                throw new Exception("Product name is required.");
            }

            if (stock.ProductName.Length > 200)
            {
                throw new Exception("Product name cannot exceed 200 characters.");
            }

            if (stock.Quantity < 0)
            {
                throw new Exception("Quantity cannot be negative.");
            }

            if (stock.UnitPrice < 0)
            {
                throw new Exception("Unit price cannot be negative.");
            }

            if (stock.MinStockLevel < 0)
            {
                throw new Exception("Minimum stock level cannot be negative.");
            }

            if (stock.MaxStockLevel < 0)
            {
                throw new Exception("Maximum stock level cannot be negative.");
            }

            if (stock.MinStockLevel > stock.MaxStockLevel && stock.MaxStockLevel > 0)
            {
                throw new Exception("Minimum stock level cannot exceed maximum stock level.");
            }
        }

        /// <summary>
        /// Checks if a barcode already exists
        /// </summary>
        public bool IsBarcodeExist(int branchID, string barcode, int excludeStockID = 0)
        {
            try
            {
                var stocks = stockDAL.GetStockByBranch(branchID);
                foreach (var stock in stocks)
                {
                    if (stock.StockID != excludeStockID &&
                        !string.IsNullOrWhiteSpace(stock.Barcode) &&
                        stock.Barcode.Equals(barcode, StringComparison.OrdinalIgnoreCase))
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
        /// Deletes a stock item
        /// </summary>
        public bool DeleteStock(int stockID, int updatedBy)
        {
            if (stockID <= 0)
            {
                throw new Exception("Invalid stock ID.");
            }

            // Check if stock is used in any recipe
            // In a real implementation, we would check if stock is used in recipes

            return stockDAL.DeleteStock(stockID, updatedBy);
        }

        /// <summary>
        /// Gets a stock item by ID
        /// </summary>
        public Stock GetStockById(int stockID)
        {
            if (stockID <= 0)
            {
                throw new Exception("Invalid stock ID.");
            }

            return stockDAL.GetStockById(stockID);
        }

        /// <summary>
        /// Gets all stock items for a branch
        /// </summary>
        public List<Stock> GetStockByBranch(int branchID, string category = null, bool includeInactive = false)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return stockDAL.GetStockByBranch(branchID, category, includeInactive);
        }

        /// <summary>
        /// Searches stock items
        /// </summary>
        public List<Stock> SearchStock(int branchID, string searchTerm)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetStockByBranch(branchID);
            }

            return stockDAL.SearchStock(branchID, searchTerm);
        }

        /// <summary>
        /// Updates stock quantity
        /// </summary>
        public bool UpdateStockQuantity(int stockID, decimal quantity, int updatedBy)
        {
            if (stockID <= 0)
            {
                throw new Exception("Invalid stock ID.");
            }

            if (quantity < 0)
            {
                throw new Exception("Quantity cannot be negative.");
            }

            var stock = stockDAL.GetStockById(stockID);
            if (stock == null)
            {
                throw new Exception("Stock item not found.");
            }

            // Create stock transaction
            StockTransaction transaction = new StockTransaction
            {
                CompanyID = stock.CompanyID,
                BranchID = stock.BranchID,
                StockID = stockID,
                TransactionType = "Adjustment",
                ReferenceType = "Manual",
                Quantity = quantity - stock.Quantity,
                PreviousQuantity = stock.Quantity,
                NewQuantity = quantity,
                UnitPrice = stock.UnitPrice,
                TotalAmount = (quantity - stock.Quantity) * stock.UnitPrice,
                Notes = "Manual quantity adjustment",
                TransactionDate = DateTime.Now,
                CreatedBy = updatedBy
            };
            stockDAL.InsertStockTransaction(transaction);

            return stockDAL.UpdateStockQuantity(stockID, quantity, updatedBy);
        }

        /// <summary>
        /// Gets low stock items
        /// </summary>
        public List<Stock> GetLowStockItems(int branchID)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return stockDAL.GetLowStockItems(branchID);
        }

        /// <summary>
        /// Gets stock transactions for a stock item
        /// </summary>
        public List<StockTransaction> GetStockTransactions(int stockID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (stockID <= 0)
            {
                throw new Exception("Invalid stock ID.");
            }

            return stockDAL.GetStockTransactions(stockID, dateFrom, dateTo);
        }

        /// <summary>
        /// Adds stock (stock in)
        /// </summary>
        public bool AddStock(int stockID, decimal quantity, decimal unitPrice, string notes, int updatedBy)
        {
            if (stockID <= 0 || quantity <= 0)
            {
                throw new Exception("Invalid stock ID or quantity.");
            }

            var stock = stockDAL.GetStockById(stockID);
            if (stock == null)
            {
                throw new Exception("Stock item not found.");
            }

            decimal newQuantity = stock.Quantity + quantity;

            // Create stock transaction
            StockTransaction transaction = new StockTransaction
            {
                CompanyID = stock.CompanyID,
                BranchID = stock.BranchID,
                StockID = stockID,
                TransactionType = "StockIn",
                ReferenceType = "Manual",
                Quantity = quantity,
                PreviousQuantity = stock.Quantity,
                NewQuantity = newQuantity,
                UnitPrice = unitPrice,
                TotalAmount = quantity * unitPrice,
                Notes = notes ?? "Manual stock addition",
                TransactionDate = DateTime.Now,
                CreatedBy = updatedBy
            };
            stockDAL.InsertStockTransaction(transaction);

            return stockDAL.UpdateStockQuantity(stockID, newQuantity, updatedBy);
        }

        /// <summary>
        /// Updates stock from order (automatic inventory deduction)
        /// </summary>
        /// <param name="orderID">Order ID</param>
        /// <param name="updatedBy">User ID performing the update</param>
        /// <returns>True if successful</returns>
        public bool UpdateStockFromOrder(int orderID, int updatedBy)
        {
            if (orderID <= 0)
            {
                throw new Exception("Invalid order ID.");
            }

            return stockDAL.UpdateStockFromOrder(orderID, updatedBy);
        }

        /// <summary>
        /// Gets inventory report
        /// </summary>
        /// <param name="branchID">Branch ID</param>
        /// <param name="category">Category filter</param>
        /// <returns>DataTable with inventory data</returns>
        public DataTable GetInventoryReport(int branchID, string category = null)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return stockDAL.GetInventoryReport(branchID, category);
        }

        /// <summary>
        /// Deducts stock for an order item
        /// </summary>
        /// <param name="stockID">Stock ID</param>
        /// <param name="quantity">Quantity to deduct</param>
        /// <param name="updatedBy">User ID performing the update</param>
        /// <returns>True if successful</returns>
        public bool DeductStockForOrder(int stockID, decimal quantity, int updatedBy)
        {
            if (stockID <= 0)
            {
                throw new Exception("Invalid stock ID.");
            }

            if (quantity <= 0)
            {
                throw new Exception("Quantity must be greater than 0.");
            }

            var stock = stockDAL.GetStockById(stockID);
            if (stock == null)
            {
                throw new Exception("Stock item not found.");
            }

            if (stock.Quantity < quantity)
            {
                throw new Exception("Insufficient stock quantity.");
            }

            decimal newQuantity = stock.Quantity - quantity;

            // Create stock transaction
            StockTransaction transaction = new StockTransaction
            {
                CompanyID = stock.CompanyID,
                BranchID = stock.BranchID,
                StockID = stockID,
                TransactionType = "StockOut",
                ReferenceType = "Order",
                Quantity = -quantity,
                PreviousQuantity = stock.Quantity,
                NewQuantity = newQuantity,
                UnitPrice = stock.UnitPrice,
                TotalAmount = -quantity * stock.UnitPrice,
                Notes = "Stock deducted for order",
                TransactionDate = DateTime.Now,
                CreatedBy = updatedBy
            };
            stockDAL.InsertStockTransaction(transaction);

            return stockDAL.UpdateStockQuantity(stockID, newQuantity, updatedBy);
        }

        /// <summary>
        /// Removes stock (stock out)
        /// </summary>
        public bool RemoveStock(int stockID, decimal quantity, string notes, int updatedBy)
        {
            if (stockID <= 0 || quantity <= 0)
            {
                throw new Exception("Invalid stock ID or quantity.");
            }

            var stock = stockDAL.GetStockById(stockID);
            if (stock == null)
            {
                throw new Exception("Stock item not found.");
            }

            if (stock.Quantity < quantity)
            {
                throw new Exception("Insufficient stock quantity.");
            }

            decimal newQuantity = stock.Quantity - quantity;

            // Create stock transaction
            StockTransaction transaction = new StockTransaction
            {
                CompanyID = stock.CompanyID,
                BranchID = stock.BranchID,
                StockID = stockID,
                TransactionType = "StockOut",
                ReferenceType = "Manual",
                Quantity = -quantity,
                PreviousQuantity = stock.Quantity,
                NewQuantity = newQuantity,
                UnitPrice = stock.UnitPrice,
                TotalAmount = -quantity * stock.UnitPrice,
                Notes = notes ?? "Manual stock removal",
                TransactionDate = DateTime.Now,
                CreatedBy = updatedBy
            };
            stockDAL.InsertStockTransaction(transaction);

            return stockDAL.UpdateStockQuantity(stockID, newQuantity, updatedBy);
        }
        // ============================================
        // BAL/StockBAL.cs - Add Deal Stock Deduction
        // ============================================

        /// <summary>
        /// Deducts stock for all items in a deal
        /// </summary>
        public bool DeductStockForDeal(int dealID, int quantity, int updatedBy)
        {
            DealBAL dealBAL = new DealBAL();
            Deal deal = dealBAL.GetDealWithItems(dealID);

            if (deal == null)
            {
                throw new Exception("Deal not found.");
            }

            foreach (var item in deal.DealItems)
            {
                // Deduct stock for each menu item in the deal
                // We need to map menu items to stock items
                // This requires a mapping table or logic
                // For now, we'll use the menu item's stock association
                bool deducted = DeductStockForMenuItem(item.MenuItemID, item.Quantity * quantity, updatedBy);
                if (!deducted)
                {
                    throw new Exception($"Failed to deduct stock for {item.ItemName}");
                }
            }

            return true;
        }

        /// <summary>
        /// Deducts stock for a menu item (helper method)
        /// </summary>
        private bool DeductStockForMenuItem(int menuItemID, int quantity, int updatedBy)
        {
            // In a real implementation, you would have a mapping between menu items and stock
            // For now, we'll use a simplified approach
            // This would need to be implemented based on your specific mapping

            // Get stock item associated with this menu item
            // This could be from a MenuItemStock table or a StockID field in MenuItems
            // For now, we'll return true as placeholder
            return true;
        }
    }
}