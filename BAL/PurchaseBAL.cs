// ============================================
// BAL/PurchaseBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Purchase BAL - Business logic layer for purchase order operations
    /// Contains business rules and validation for purchase management
    /// </summary>
    public class PurchaseBAL
    {
        private readonly PurchaseDAL purchaseDAL;
        private readonly StockDAL stockDAL;
        private readonly SupplierDAL supplierDAL;

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public PurchaseBAL()
        {
            purchaseDAL = new PurchaseDAL();
            stockDAL = new StockDAL();
            supplierDAL = new SupplierDAL();
        }

        /// <summary>
        /// Creates a new purchase order
        /// </summary>
        public int CreatePurchaseOrder(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> items)
        {
            ValidatePurchaseOrder(purchaseOrder);
            ValidatePurchaseOrderItems(items);

            // Generate purchase order number
            purchaseOrder.PurchaseOrderNumber = GeneratePurchaseOrderNumber(purchaseOrder.BranchID);

            // Calculate totals
            CalculatePurchaseTotals(purchaseOrder, items);

            // Insert purchase order
            int orderID = purchaseDAL.InsertPurchaseOrder(purchaseOrder);

            // Insert purchase order items
            foreach (var item in items)
            {
                item.PurchaseOrderID = orderID;
                purchaseDAL.InsertPurchaseOrderItem(item);
            }

            return orderID;
        }

        /// <summary>
        /// Validates purchase order data
        /// </summary>
        private void ValidatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (purchaseOrder.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (purchaseOrder.SupplierID <= 0)
            {
                throw new Exception("Supplier is required.");
            }

            // Validate supplier exists
            var supplier = supplierDAL.GetSupplierById(purchaseOrder.SupplierID);
            if (supplier == null || supplier.BranchID != purchaseOrder.BranchID)
            {
                throw new Exception("Supplier is invalid or belongs to a different branch.");
            }

            if (purchaseOrder.SubTotal < 0 || purchaseOrder.TotalAmount < 0)
            {
                throw new Exception("Amounts cannot be negative.");
            }
        }

        /// <summary>
        /// Validates purchase order items
        /// </summary>
        private void ValidatePurchaseOrderItems(List<PurchaseOrderItem> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new Exception("Purchase order must have at least one item.");
            }

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                {
                    throw new Exception("Item quantity must be greater than 0.");
                }

                if (item.UnitPrice < 0)
                {
                    throw new Exception("Item price cannot be negative.");
                }

                // Validate stock item exists
                var stock = stockDAL.GetStockById(item.StockID);
                if (stock == null)
                {
                    throw new Exception($"Stock item not found.");
                }
            }
        }

        /// <summary>
        /// Calculates purchase order totals
        /// </summary>
        private void CalculatePurchaseTotals(PurchaseOrder purchaseOrder, List<PurchaseOrderItem> items)
        {
            decimal subTotal = 0;

            foreach (var item in items)
            {
                item.TotalPrice = item.Quantity * item.UnitPrice;
                subTotal += item.TotalPrice;
            }

            purchaseOrder.SubTotal = subTotal;
            purchaseOrder.TotalAmount = subTotal + purchaseOrder.Tax - purchaseOrder.Discount;
        }

        /// <summary>
        /// Generates a unique purchase order number
        /// </summary>
        private string GeneratePurchaseOrderNumber(int branchID)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(1000, 9999).ToString();
            return $"PO-{branchID}-{date}-{random}";
        }

        /// <summary>
        /// Receives a purchase order (updates inventory)
        /// </summary>
        public bool ReceivePurchaseOrder(int purchaseOrderID, int updatedBy)
        {
            if (purchaseOrderID <= 0)
            {
                throw new Exception("Invalid purchase order ID.");
            }

            // Get purchase order with items
            DataSet dataSet = purchaseDAL.GetPurchaseOrderById(purchaseOrderID);
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                throw new Exception("Purchase order not found.");
            }

            // Update purchase order status
            purchaseDAL.UpdatePurchaseOrderStatus(purchaseOrderID, "Received", updatedBy);

            // Update stock quantities
            if (dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    int stockID = Convert.ToInt32(row["StockID"]);
                    decimal quantity = Convert.ToDecimal(row["Quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);

                    // Update stock quantity
                    var stock = stockDAL.GetStockById(stockID);
                    if (stock != null)
                    {
                        decimal newQuantity = stock.Quantity + quantity;
                        stockDAL.UpdateStockQuantity(stockID, newQuantity, updatedBy);

                        // Create stock transaction
                        StockTransaction transaction = new StockTransaction
                        {
                            CompanyID = stock.CompanyID,
                            BranchID = stock.BranchID,
                            StockID = stockID,
                            TransactionType = "StockIn",
                            ReferenceType = "Purchase",
                            ReferenceID = purchaseOrderID,
                            Quantity = quantity,
                            PreviousQuantity = stock.Quantity,
                            NewQuantity = newQuantity,
                            UnitPrice = unitPrice,
                            TotalAmount = quantity * unitPrice,
                            Notes = $"Received from purchase order {purchaseOrderID}",
                            TransactionDate = DateTime.Now,
                            CreatedBy = updatedBy
                        };
                        stockDAL.InsertStockTransaction(transaction);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets purchase orders by branch
        /// </summary>
        public List<PurchaseOrder> GetPurchaseOrdersByBranch(int branchID, string status = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return purchaseDAL.GetPurchaseOrdersByBranch(branchID, status, dateFrom, dateTo);
        }

        /// <summary>
        /// Gets purchase orders by supplier
        /// </summary>
        public List<PurchaseOrder> GetPurchaseOrdersBySupplier(int supplierID)
        {
            if (supplierID <= 0)
            {
                throw new Exception("Invalid supplier ID.");
            }

            // In a real implementation, we would have a stored procedure for this
            // For now, we'll return empty list
            return new List<PurchaseOrder>();
        }

        /// <summary>
        /// Gets a purchase order by ID
        /// </summary>
        public DataSet GetPurchaseOrderById(int purchaseOrderID)
        {
            if (purchaseOrderID <= 0)
            {
                throw new Exception("Invalid purchase order ID.");
            }

            return purchaseDAL.GetPurchaseOrderById(purchaseOrderID);
        }

        /// <summary>
        /// Updates purchase order status
        /// </summary>
        public bool UpdatePurchaseOrderStatus(int purchaseOrderID, string status, int updatedBy)
        {
            if (purchaseOrderID <= 0)
            {
                throw new Exception("Invalid purchase order ID.");
            }

            string[] validStatuses = { "Draft", "Sent", "Received", "Cancelled" };
            if (!Array.Exists(validStatuses, s => s == status))
            {
                throw new Exception("Invalid purchase order status.");
            }

            return purchaseDAL.UpdatePurchaseOrderStatus(purchaseOrderID, status, updatedBy);
        }
    }
}