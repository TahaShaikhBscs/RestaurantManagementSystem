// ============================================
// UI/POS/POS.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using MenuItemModel = RestaurantManagementSystem.Models.MenuItem;
using OrderModel = RestaurantManagementSystem.Models.Order;
using OrderItemModel = RestaurantManagementSystem.Models.OrderItem;
using TableModel = RestaurantManagementSystem.Models.RestaurantTable;
using CustomerModel = RestaurantManagementSystem.Models.Customer;

namespace RestaurantManagementSystem.UI.POS
{
    /// <summary>
    /// Point of Sale Page - Handles order processing and billing
    /// </summary>
    public partial class POS : Page
    {
        private MenuBAL menuBAL;
        private CategoryBAL categoryBAL;
        private TableBAL tableBAL;
        private OrderBAL orderBAL;
        private CustomerBAL customerBAL;
        private PaymentBAL paymentBAL;
        private AuditLogger auditLogger;

        private List<CartItem> cartItems;

        /// <summary>
        /// Cart Item class for holding cart items
        /// </summary>
        public class CartItem
        {
            public int MenuItemID { get; set; }
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get { return Quantity * UnitPrice; } }
        }

        /// <summary>
        /// Page load event - initializes POS
        /// </summary>
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!SessionHelper.IsLoggedIn())
        //    {
        //        Response.Redirect("~/Login.aspx");
        //        return;
        //    }

        //    menuBAL = new MenuBAL();
        //    categoryBAL = new CategoryBAL();
        //    tableBAL = new TableBAL();
        //    orderBAL = new OrderBAL();
        //    customerBAL = new CustomerBAL();
        //    paymentBAL = new PaymentBAL();
        //    auditLogger = new AuditLogger();

        //    // Initialize cart from session
        //    if (Session["POSCart"] == null)
        //    {
        //        cartItems = new List<CartItem>();
        //        Session["POSCart"] = cartItems;
        //    }
        //    else
        //    {
        //        cartItems = (List<CartItem>)Session["POSCart"];
        //    }

        //    if (!IsPostBack)
        //    {
        //        LoadCategories();
        //        LoadMenuItems();
        //        LoadTables();
        //        UpdateCartDisplay();
        //        CalculateTotals();
        //        hfOrderID.Value = "0";
        //    }
        //}
        #region Page_Load - Initialize Deal Cart

        protected void Page_Load(object sender, EventArgs e)
        {
            // ... existing code ...

            // Initialize deal cart from session
            if (Session["POSDealCart"] == null)
            {
                dealCartItems = new List<DealCartItem>();
                Session["POSDealCart"] = dealCartItems;
            }
            else
            {
                dealCartItems = (List<DealCartItem>)Session["POSDealCart"];
            }

            if (!IsPostBack)
            {
                // ... existing code ...
                LoadDeals();
            }
        }

        #endregion

        /// <summary>
        /// Loads categories for filter
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<Category> categories = categoryBAL.GetCategoriesByBranch(branchID.Value, false);
                    ddlCategory.DataSource = categories;
                    ddlCategory.DataTextField = "CategoryName";
                    ddlCategory.DataValueField = "CategoryID";
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("All Categories", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads menu items for display
        /// </summary>
        private void LoadMenuItems()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                int? categoryID = null;
                if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
                {
                    categoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                }

                List<MenuItemModel> items = menuBAL.GetMenuItemsByBranch(branchID.Value, categoryID, false);
                rptMenuItems.DataSource = items;
                rptMenuItems.DataBind();
                ltrItemCount.Text = items.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading menu items: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads available tables for dine-in
        /// </summary>
        private void LoadTables()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<TableModel> tables = tableBAL.GetAvailableTables(branchID.Value);
                    ddlTable.DataSource = tables;
                    ddlTable.DataTextField = "TableNumber";
                    ddlTable.DataValueField = "TableID";
                    ddlTable.DataBind();
                    ddlTable.Items.Insert(0, new ListItem("Select Table", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading tables: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates cart display
        /// </summary>
        //private void UpdateCartDisplay()
        //{
        //    rptCart.DataSource = cartItems;
        //    rptCart.DataBind();

        //    pnlEmptyCart.Visible = (cartItems == null || cartItems.Count == 0);
        //    rptCart.Visible = !pnlEmptyCart.Visible;
        //}
        #region Update Cart Display with Deals

        /// <summary>
        /// Updated UpdateCartDisplay to show deals
        /// </summary>
        private void UpdateCartDisplay()
        {
            // Combine cart items and deal items for display
            var displayItems = new List<object>();

            foreach (var item in cartItems)
            {
                displayItems.Add(new
                {
                    Type = "Item",
                    MenuItemID = item.MenuItemID,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                    IsDeal = false
                });
            }

            foreach (var deal in dealCartItems)
            {
                displayItems.Add(new
                {
                    Type = "Deal",
                    DealID = deal.DealID,
                    ItemName = deal.DealName + " (Combo)",
                    Quantity = deal.Quantity,
                    UnitPrice = deal.DealPrice,
                    TotalPrice = deal.TotalPrice,
                    IsDeal = true,
                    DealItems = deal.Items
                });
            }

            // Bind to repeater
            rptCart.DataSource = displayItems;
            rptCart.DataBind();
        }

        #endregion

        /// <summary>
        /// Calculates and displays totals
        /// </summary>
        private void CalculateTotals()
        {
            decimal subTotal = 0;
            foreach (var item in cartItems)
            {
                subTotal += item.TotalPrice;
            }

            decimal tax = subTotal * 0.05m; // 5% tax
            decimal discount = 0; // Calculate discount if any
            decimal total = subTotal + tax - discount;

            ltrSubTotal.Text = subTotal.ToString("N2");
            ltrTax.Text = tax.ToString("N2");
            ltrDiscount.Text = discount.ToString("N2");
            ltrTotal.Text = total.ToString("N2");
        }

        /// <summary>
        /// Handles category filter change
        /// </summary>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenuItems();
        }

        /// <summary>
        /// Handles search text change
        /// </summary>
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                int? branchID = SessionHelper.GetBranchId();

                if (branchID.HasValue && !string.IsNullOrEmpty(searchTerm))
                {
                    List<MenuItemModel> items = menuBAL.SearchMenuItems(branchID.Value, searchTerm);
                    rptMenuItems.DataSource = items;
                    rptMenuItems.DataBind();
                    ltrItemCount.Text = items.Count.ToString();
                }
                else
                {
                    LoadMenuItems();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching items: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles menu item click - adds to cart
        /// </summary>
        protected void rptMenuItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    int menuItemID = Convert.ToInt32(e.CommandArgument);

                    // Get item details
                    MenuItemModel item = menuBAL.GetMenuItemById(menuItemID);
                    if (item != null && item.IsAvailable)
                    {
                        // Check if item already in cart
                        var existing = cartItems.Find(c => c.MenuItemID == menuItemID);
                        if (existing != null)
                        {
                            existing.Quantity++;
                        }
                        else
                        {
                            cartItems.Add(new CartItem
                            {
                                MenuItemID = menuItemID,
                                ItemName = item.ItemName,
                                Quantity = 1,
                                UnitPrice = item.Price
                            });
                        }

                        Session["POSCart"] = cartItems;
                        UpdateCartDisplay();
                        CalculateTotals();
                        ShowSuccessMessage($"{item.ItemName} added to cart.");
                    }
                    else
                    {
                        ShowErrorMessage("Item is not available.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding item: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles cart item commands (add/remove)
        /// </summary>
        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                int menuItemID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "RemoveItem")
                {
                    var item = cartItems.Find(c => c.MenuItemID == menuItemID);
                    if (item != null)
                    {
                        if (item.Quantity > 1)
                        {
                            item.Quantity--;
                        }
                        else
                        {
                            cartItems.Remove(item);
                        }
                        Session["POSCart"] = cartItems;
                        UpdateCartDisplay();
                        CalculateTotals();
                    }
                }
                else if (e.CommandName == "AddItem")
                {
                    var item = cartItems.Find(c => c.MenuItemID == menuItemID);
                    if (item != null)
                    {
                        item.Quantity++;
                        Session["POSCart"] = cartItems;
                        UpdateCartDisplay();
                        CalculateTotals();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error updating cart: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears the cart
        /// </summary>
        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            cartItems.Clear();
            Session["POSCart"] = cartItems;
            UpdateCartDisplay();
            CalculateTotals();
            ShowSuccessMessage("Cart cleared.");
        }

        /// <summary>
        /// Finds customer by phone number
        /// </summary>
        protected void btnFindCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string phone = txtCustomer.Text.Trim();
                if (!string.IsNullOrEmpty(phone))
                {
                    int? branchID = SessionHelper.GetBranchId();
                    if (branchID.HasValue)
                    {
                        CustomerModel customer = customerBAL.GetCustomerByPhone(phone, branchID.Value);
                        if (customer != null)
                        {
                            txtCustomer.Text = customer.CustomerName;
                            ShowSuccessMessage($"Customer found: {customer.CustomerName}");
                        }
                        else
                        {
                            ShowErrorMessage("Customer not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error finding customer: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the order
        /// </summary>
        protected void btnProcessOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (cartItems.Count == 0 && dealCartItems.Count == 0)
                {
                    ShowErrorMessage("Cart is empty. Please add items first.");
                    return;
                }

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                int? companyID = SessionHelper.GetCompanyId();
                int? branchID = SessionHelper.GetBranchId();

                if (!companyID.HasValue || !branchID.HasValue)
                {
                    ShowErrorMessage("Company or branch not found in session.");
                    return;
                }

                // Get order details
                string orderType = ddlOrderType.SelectedValue;
                int? tableID = null;
                if (!string.IsNullOrEmpty(ddlTable.SelectedValue))
                {
                    tableID = Convert.ToInt32(ddlTable.SelectedValue);
                }

                string customerName = txtCustomer.Text.Trim();
                int? customerID = null;

                // Create order items from both regular items and deals
                List<OrderItem> orderItems = new List<OrderItem>();

                // Add regular cart items
                foreach (var cartItem in cartItems)
                {
                    orderItems.Add(new OrderItem
                    {
                        MenuItemID = cartItem.MenuItemID,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.UnitPrice,
                        Discount = 0,
                        TotalPrice = cartItem.TotalPrice,
                        KitchenStatus = "Pending",
                        CreatedBy = userID.Value
                    });
                }

                // Add deal items (each menu item from the deal)
                foreach (var deal in dealCartItems)
                {
                    foreach (var item in deal.Items)
                    {
                        orderItems.Add(new OrderItem
                        {
                            MenuItemID = item.MenuItemID,
                            Quantity = item.Quantity * deal.Quantity,
                            UnitPrice = item.UnitPrice,
                            Discount = 0,
                            TotalPrice = item.TotalPrice * deal.Quantity,
                            KitchenStatus = "Pending",
                            CreatedBy = userID.Value
                        });
                    }
                }

                // Calculate totals
                decimal subTotal = 0;
                foreach (var item in orderItems)
                {
                    subTotal += item.TotalPrice;
                }

                // Apply deal discounts (the deal price is already discounted)
                decimal dealTotal = 0;
                foreach (var deal in dealCartItems)
                {
                    dealTotal += deal.TotalPrice;
                }

                decimal totalAmount = subTotal - (subTotal - dealTotal);

                // Create order
                OrderModel order = new OrderModel
                {
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    TableID = tableID,
                    CustomerID = customerID,
                    OrderType = orderType,
                    OrderStatus = "Pending",
                    PaymentStatus = "Pending",
                    SubTotal = subTotal,
                    Tax = 0,
                    Discount = subTotal - totalAmount,
                    TotalAmount = totalAmount,
                    OrderSource = "POS",
                    CreatedBy = userID.Value
                };

                // Save order
                int orderID = orderBAL.CreateOrder(order, orderItems);

                if (orderID > 0)
                {
                    // Process payment
                    decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);
                    if (paidAmount > 0)
                    {
                        Payment payment = new Payment
                        {
                            CompanyID = companyID.Value,
                            BranchID = branchID.Value,
                            OrderID = orderID,
                            PaymentMethod = ddlPaymentMethod.SelectedValue,
                            Amount = paidAmount,
                            PaymentStatus = paidAmount >= totalAmount ? "Completed" : "Partial",
                            CreatedBy = userID.Value
                        };
                        paymentBAL.ProcessPayment(payment);
                    }

                    auditLogger.LogInsert("OrderManagement", "Orders", orderID,
                        $"OrderNumber: {order.OrderNumber}, Type: {orderType}, Total: {order.TotalAmount}");

                    ShowSuccessMessage($"Order #{order.OrderNumber} processed successfully!");

                    // Clear cart
                    cartItems.Clear();
                    dealCartItems.Clear();
                    Session["POSCart"] = cartItems;
                    Session["POSDealCart"] = dealCartItems;
                    UpdateCartDisplay();
                    CalculateTotals();
                    txtCustomer.Text = "Walk-in";
                    txtPaidAmount.Text = "0.00";
                    hfOrderID.Value = "0";
                }
                else
                {
                    ShowErrorMessage("Failed to process order.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error processing order: {ex.Message}");
                auditLogger.LogAction("Error", "POS", "ProcessOrder", null, null, ex.Message);
            }
        }
        //protected void btnProcessOrder_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cartItems.Count == 0)
        //        {
        //            ShowErrorMessage("Cart is empty. Please add items first.");
        //            return;
        //        }

        //        int? userID = SessionHelper.GetUserId();
        //        if (!userID.HasValue)
        //        {
        //            ShowErrorMessage("User session expired. Please login again.");
        //            return;
        //        }

        //        int? companyID = SessionHelper.GetCompanyId();
        //        int? branchID = SessionHelper.GetBranchId();

        //        if (!companyID.HasValue || !branchID.HasValue)
        //        {
        //            ShowErrorMessage("Company or branch not found in session.");
        //            return;
        //        }

        //        // Get order details
        //        string orderType = ddlOrderType.SelectedValue;
        //        int? tableID = null;
        //        if (!string.IsNullOrEmpty(ddlTable.SelectedValue))
        //        {
        //            tableID = Convert.ToInt32(ddlTable.SelectedValue);
        //        }

        //        string customerName = txtCustomer.Text.Trim();
        //        int? customerID = null;

        //        // Find or create customer
        //        if (!string.IsNullOrEmpty(customerName) && customerName != "Walk-in")
        //        {
        //            // Check if customer exists by name (simplified)
        //            // In real implementation, you would search by phone or email
        //            customerID = null; // Placeholder
        //        }

        //        // Create order
        //        OrderModel order = new OrderModel
        //        {
        //            CompanyID = companyID.Value,
        //            BranchID = branchID.Value,
        //            TableID = tableID,
        //            CustomerID = customerID,
        //            OrderType = orderType,
        //            OrderStatus = "Pending",
        //            PaymentStatus = "Pending",
        //            OrderSource = "POS",
        //            CreatedBy = userID.Value
        //        };

        //        List<OrderItemModel> items = new List<OrderItemModel>();
        //        foreach (var cartItem in cartItems)
        //        {
        //            items.Add(new OrderItemModel
        //            {
        //                MenuItemID = cartItem.MenuItemID,
        //                Quantity = cartItem.Quantity,
        //                UnitPrice = cartItem.UnitPrice,
        //                Discount = 0,
        //                TotalPrice = cartItem.TotalPrice,
        //                KitchenStatus = "Pending",
        //                CreatedBy = userID.Value
        //            });
        //        }

        //        // Save order
        //        int orderID = orderBAL.CreateOrder(order, items);

        //        if (orderID > 0)
        //        {
        //            // Process payment
        //            decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);
        //            if (paidAmount > 0)
        //            {
        //                Payment payment = new Payment
        //                {
        //                    CompanyID = companyID.Value,
        //                    BranchID = branchID.Value,
        //                    OrderID = orderID,
        //                    PaymentMethod = ddlPaymentMethod.SelectedValue,
        //                    Amount = paidAmount,
        //                    PaymentStatus = paidAmount >= order.TotalAmount ? "Completed" : "Partial",
        //                    CreatedBy = userID.Value
        //                };
        //                paymentBAL.ProcessPayment(payment);
        //            }

        //            auditLogger.LogInsert("OrderManagement", "Orders", orderID,
        //                $"OrderNumber: {order.OrderNumber}, Type: {orderType}, Total: {order.TotalAmount}");

        //            ShowSuccessMessage($"Order #{order.OrderNumber} processed successfully!");

        //            // Clear cart
        //            cartItems.Clear();
        //            Session["POSCart"] = cartItems;
        //            UpdateCartDisplay();
        //            CalculateTotals();
        //            hfOrderID.Value = "0";
        //            txtCustomer.Text = "Walk-in";
        //            txtPaidAmount.Text = "0.00";
        //        }
        //        else
        //        {
        //            ShowErrorMessage("Failed to process order.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowErrorMessage($"Error processing order: {ex.Message}");
        //        auditLogger.LogAction("Error", "POS", "ProcessOrder", null, null, ex.Message);
        //    }
        //}

        /// <summary>
        /// Holds the order for later
        /// </summary>
        protected void btnHoldOrder_Click(object sender, EventArgs e)
        {
            // In a real implementation, you would save the order as "Held"
            // and store it in a held orders list
            ShowSuccessMessage("Order held successfully. You can resume it from the Orders list.");
        }

        /// <summary>
        /// Starts a new order
        /// </summary>
        protected void btnNewOrder_Click(object sender, EventArgs e)
        {
            cartItems.Clear();
            Session["POSCart"] = cartItems;
            UpdateCartDisplay();
            CalculateTotals();
            txtCustomer.Text = "Walk-in";
            txtPaidAmount.Text = "0.00";
            hfOrderID.Value = "0";
            ShowSuccessMessage("New order started.");
        }

        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            ltrSuccessMessage.Text = message;
        }

        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            ltrErrorMessage.Text = message;
        }

        // ============================================
        // UI/POS/POS.aspx.cs - Add Deal Methods
        // ============================================

        // Add these methods to the existing POS class

        #region Deal Cart Item Class

        /// <summary>
        /// Deal Cart Item - Represents a deal in the cart
        /// </summary>
        public class DealCartItem
        {
            public int DealID { get; set; }
            public string DealName { get; set; }
            public decimal DealPrice { get; set; }
            public int Quantity { get; set; }
            public List<CartItem> Items { get; set; }
            public decimal TotalPrice { get { return DealPrice * Quantity; } }
        }

        #endregion

        #region Load Deals

        /// <summary>
        /// Loads deals for POS display
        /// </summary>
        private void LoadDeals()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    DealBAL dealBAL = new DealBAL();
                    var deals = dealBAL.GetValidDeals(branchID.Value);
                    rptDeals.DataSource = deals;
                    rptDeals.DataBind();
                    ltrDealCount.Text = deals.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                // Log error
            }
        }

        #endregion

        #region Deal Cart Handling

        /// <summary>
        /// Adds a deal to the cart
        /// </summary>
        protected void rptDeals_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddDeal")
                {
                    int dealID = Convert.ToInt32(e.CommandArgument);
                    AddDealToCart(dealID);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding deal: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a deal to the cart with all its items
        /// </summary>
        private void AddDealToCart(int dealID)
        {
            DealBAL dealBAL = new DealBAL();
            Deal deal = dealBAL.GetDealWithItems(dealID);

            if (deal != null && deal.IsValid)
            {
                // Check if deal already in cart
                var existingDeal = dealCartItems.Find(d => d.DealID == dealID);
                if (existingDeal != null)
                {
                    existingDeal.Quantity++;
                }
                else
                {
                    // Create deal cart item
                    DealCartItem dealItem = new DealCartItem
                    {
                        DealID = deal.DealID,
                        DealName = deal.DealName,
                        DealPrice = deal.DealPrice,
                        Quantity = 1,
                        Items = new List<CartItem>()
                    };

                    // Add each menu item from the deal
                    foreach (var item in deal.DealItems)
                    {
                        dealItem.Items.Add(new CartItem
                        {
                            MenuItemID = item.MenuItemID,
                            ItemName = item.ItemName,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price
                        });
                    }

                    dealCartItems.Add(dealItem);
                }

                Session["POSDealCart"] = dealCartItems;
                UpdateCartDisplay();
                CalculateTotals();
                ShowSuccessMessage($"{deal.DealName} added to cart.");
            }
            else
            {
                ShowErrorMessage("Deal is not available.");
            }
        }

        /// <summary>
        /// Removes a deal from the cart
        /// </summary>
        private void RemoveDealFromCart(int dealID)
        {
            var deal = dealCartItems.Find(d => d.DealID == dealID);
            if (deal != null)
            {
                if (deal.Quantity > 1)
                {
                    deal.Quantity--;
                }
                else
                {
                    dealCartItems.Remove(deal);
                }
                Session["POSDealCart"] = dealCartItems;
                UpdateCartDisplay();
                CalculateTotals();
            }
        }

        #endregion

        #region Process Order with Deals

        /// <summary>
        /// Updated Process Order method to handle deals
        /// </summary>


        #endregion



        #region Private Fields - Add Deal Cart

        private List<DealCartItem> dealCartItems;

        #endregion


    }
}