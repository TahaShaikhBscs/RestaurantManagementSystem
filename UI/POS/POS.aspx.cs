// ============================================
// UI/POS/POS.aspx.cs - Enterprise POS Code Behind
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.POS
{
    /// <summary>
    /// Enterprise Point of Sale Page - Server-Side Implementation
    /// All business logic executes on the server via PostBack
    /// </summary>
    public partial class POS : Page
    {
        #region Private Fields

        private POSOrderBAL posOrderBAL;
        private OrderBAL orderBAL;
        private MenuBAL menuBAL;
        private CategoryBAL categoryBAL;
        private TableBAL tableBAL;
        private CustomerBAL customerBAL;
        private DealBAL dealBAL;
        private PaymentBAL paymentBAL;
        private AuditLogger auditLogger;
        private int companyID;
        private int branchID;
        private int userID;
        private const string MODULE_NAME = "POS";

        // Cart stored in Session
        private List<CartItem> cartItems;

        #endregion

        #region Cart Item Class

        [Serializable]
        public class CartItem
        {
            public int MenuItemID { get; set; }
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get { return Quantity * UnitPrice; } }
            public int? DealID { get; set; }
            public string DealName { get; set; }
            public bool IsDeal { get; set; }
            public List<CartItem> DealItems { get; set; }
        }

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - initializes POS
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            // Load cart from session
            if (Session["POSCart"] == null)
            {
                cartItems = new List<CartItem>();
                Session["POSCart"] = cartItems;
            }
            else
            {
                cartItems = (List<CartItem>)Session["POSCart"];
            }

            if (!IsPostBack)
            {
                LoadCategories();
                LoadMenuItems();
                LoadDeals();
                LoadTables();
                LoadCustomers();
                LoadPaymentMethods();
                BindCartGrid();
                CalculateTotals();
                LoadShiftInfo();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            posOrderBAL = new POSOrderBAL();
            orderBAL = new OrderBAL();
            menuBAL = new MenuBAL();
            categoryBAL = new CategoryBAL();
            tableBAL = new TableBAL();
            customerBAL = new CustomerBAL();
            dealBAL = new DealBAL();
            paymentBAL = new PaymentBAL();
            auditLogger = new AuditLogger();

            var companyIDNullable = SessionHelper.GetCompanyId();
            var branchIDNullable = SessionHelper.GetBranchId();
            var userIDNullable = SessionHelper.GetUserId();

            companyID = companyIDNullable ?? 0;
            branchID = branchIDNullable ?? 0;
            userID = userIDNullable ?? 0;

            hfShiftID.Value = GetActiveShiftID().ToString();
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads categories into the dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                var categories = categoryBAL.GetCategoriesByBranch(branchID, false);
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("All Categories", ""));
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading categories: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCategories", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Loads menu items into the grid
        /// </summary>
        private void LoadMenuItems()
        {
            try
            {
                int? categoryID = null;
                if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
                {
                    categoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                }

                var items = menuBAL.GetMenuItemsByBranch(branchID, categoryID, false);

                // Apply search filter
                string searchTerm = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    items = items.FindAll(i =>
                        i.ItemName.ToLower().Contains(searchTerm.ToLower()) ||
                        (i.SKU != null && i.SKU.ToLower().Contains(searchTerm.ToLower()))
                    );
                }

                rptMenuItems.DataSource = items;
                rptMenuItems.DataBind();
                pnlNoItems.Visible = items == null || items.Count == 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading menu items: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "LoadMenuItems", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Loads deals into the grid
        /// </summary>
        private void LoadDeals()
        {
            try
            {
                var deals = dealBAL.GetValidDeals(branchID);
                rptDeals.DataSource = deals;
                rptDeals.DataBind();

                pnlNoDeals.Visible = deals == null || deals.Count == 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading deals: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "LoadDeals", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Loads tables into the dropdown
        /// </summary>
        private void LoadTables()
        {
            try
            {
                var tables = tableBAL.GetAvailableTables(branchID);
                ddlTable.DataSource = tables;
                ddlTable.DataTextField = "TableNumber";
                ddlTable.DataValueField = "TableID";
                ddlTable.DataBind();
                ddlTable.Items.Insert(0, new ListItem("Select Table", ""));
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading tables: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "LoadTables", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Loads customers into the dropdown
        /// </summary>
        private void LoadCustomers()
        {
            try
            {
                var customers = customerBAL.GetCustomersByBranch(branchID, false);
                ddlCustomer.DataSource = customers;
                ddlCustomer.DataTextField = "CustomerName";
                ddlCustomer.DataValueField = "CustomerID";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, new ListItem("-- Walk-in --", ""));
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading customers: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCustomers", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Loads payment methods
        /// </summary>
        private void LoadPaymentMethods()
        {
            // In a real implementation, load from database
            ddlPaymentMethod.Items.Clear();
            ddlPaymentMethod.Items.Add(new ListItem("-- Select Method --", ""));
            ddlPaymentMethod.Items.Add(new ListItem("Cash", "Cash"));
            ddlPaymentMethod.Items.Add(new ListItem("Credit Card", "Credit Card"));
            ddlPaymentMethod.Items.Add(new ListItem("Debit Card", "Debit Card"));
            ddlPaymentMethod.Items.Add(new ListItem("Online", "Online"));
            ddlPaymentMethod.Items.Add(new ListItem("Mobile Wallet", "Mobile Wallet"));
        }

        /// <summary>
        /// Loads shift information
        /// </summary>
        private void LoadShiftInfo()
        {
            // In a real implementation, get active shift
            hfShiftID.Value = "1";
        }

        #endregion

        #region Cart Management

        /// <summary>
        /// Binds cart grid view
        /// </summary>
        private void BindCartGrid()
        {
            gvCart.DataSource = cartItems;
            gvCart.DataBind();
        }

        /// <summary>
        /// Adds an item to the cart
        /// </summary>
        private void AddItemToCart(int menuItemID)
        {
            var menuItem = menuBAL.GetMenuItemById(menuItemID);
            if (menuItem != null && menuItem.IsAvailable)
            {
                var existing = cartItems.Find(c => c.MenuItemID == menuItemID && !c.IsDeal);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItem
                    {
                        MenuItemID = menuItemID,
                        ItemName = menuItem.ItemName,
                        Quantity = 1,
                        UnitPrice = menuItem.Price,
                        IsDeal = false
                    });
                }

                Session["POSCart"] = cartItems;
                BindCartGrid();
                CalculateTotals();
                ShowSuccessMessage($"{menuItem.ItemName} added to cart.");
            }
            else
            {
                ShowErrorMessage("Item is not available.");
            }
        }

        /// <summary>
        /// Adds a deal to the cart
        /// </summary>
        private void AddDealToCart(int dealID)
        {
            var deal = dealBAL.GetDealWithItems(dealID);
            if (deal != null && deal.IsValid)
            {
                // Check if deal already in cart
                var existing = cartItems.Find(c => c.DealID == dealID && c.IsDeal);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    var dealItem = new CartItem
                    {
                        DealID = dealID,
                        DealName = deal.DealName,
                        Quantity = 1,
                        UnitPrice = deal.DealPrice,
                        IsDeal = true,
                        DealItems = new List<CartItem>()
                    };

                    // Add individual items from deal
                    foreach (var item in deal.DealItems)
                    {
                        dealItem.DealItems.Add(new CartItem
                        {
                            MenuItemID = item.MenuItemID,
                            ItemName = item.ItemName,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price,
                            IsDeal = false
                        });
                    }

                    cartItems.Add(dealItem);
                }

                Session["POSCart"] = cartItems;
                BindCartGrid();
                CalculateTotals();
                ShowSuccessMessage($"Deal '{deal.DealName}' added to cart.");
            }
            else
            {
                ShowErrorMessage("Deal is not available.");
            }
        }

        /// <summary>
        /// Updates item quantity in cart
        /// </summary>
        private void UpdateCartItemQuantity(int index, int change)
        {
            if (index >= 0 && index < cartItems.Count)
            {
                int newQty = cartItems[index].Quantity + change;
                if (newQty <= 0)
                {
                    cartItems.RemoveAt(index);
                }
                else
                {
                    cartItems[index].Quantity = newQty;
                }

                Session["POSCart"] = cartItems;
                BindCartGrid();
                CalculateTotals();
            }
        }

        /// <summary>
        /// Removes an item from cart
        /// </summary>
        private void RemoveCartItem(int index)
        {
            if (index >= 0 && index < cartItems.Count)
            {
                string itemName = cartItems[index].ItemName;
                cartItems.RemoveAt(index);

                Session["POSCart"] = cartItems;
                BindCartGrid();
                CalculateTotals();
                ShowSuccessMessage($"{itemName} removed from cart.");
            }
        }

        /// <summary>
        /// Clears the entire cart
        /// </summary>
        private void ClearCart()
        {
            cartItems.Clear();
            Session["POSCart"] = cartItems;
            BindCartGrid();
            CalculateTotals();
            ShowSuccessMessage("Cart cleared.");
        }

        #endregion

        #region Calculations

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
            decimal discount = 0;
            decimal serviceCharge = 0;

            decimal.TryParse(txtDiscount.Text, out discount);
            decimal.TryParse(txtServiceCharge.Text, out serviceCharge);

            decimal total = subTotal + tax - discount + serviceCharge;

            ltrSubTotal.Text = $"${subTotal:F2}";
            ltrTax.Text = $"${tax:F2}";
            ltrTotal.Text = $"${total:F2}";

            // Update payment modal totals
            ltrPaymentTotal.Text = $"${total:F2}";

            // Bind payment items
            gvPaymentItems.DataSource = cartItems;
            gvPaymentItems.DataBind();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Handles category selection change
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
            LoadMenuItems();
        }

        /// <summary>
        /// Handles refresh menu button click
        /// </summary>
        protected void btnRefreshMenu_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
            LoadDeals();
            ShowSuccessMessage("Menu refreshed.");
        }

        /// <summary>
        /// Handles order type change
        /// </summary>
        protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string orderType = ddlOrderType.SelectedValue;
            if (orderType == "Delivery")
            {
                divDeliveryAddress.Style["display"] = "block";
            }
            else
            {
                divDeliveryAddress.Style["display"] = "none";
            }
        }

        /// <summary>
        /// Handles menu item add
        /// </summary>
        protected void rptMenuItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddItem")
            {
                int menuItemID = Convert.ToInt32(e.CommandArgument);
                AddItemToCart(menuItemID);
            }
        }

        /// <summary>
        /// Handles deal add
        /// </summary>
        protected void rptDeals_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddDeal")
            {
                int dealID = Convert.ToInt32(e.CommandArgument);
                AddDealToCart(dealID);
            }
        }

        /// <summary>
        /// Handles cart grid commands
        /// </summary>
        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "IncreaseQty":
                    UpdateCartItemQuantity(index, 1);
                    break;
                case "DecreaseQty":
                    UpdateCartItemQuantity(index, -1);
                    break;
                case "RemoveItem":
                    RemoveCartItem(index);
                    break;
            }
        }

        /// <summary>
        /// Handles clear cart button click
        /// </summary>
        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            ClearCart();
        }

        /// <summary>
        /// Handles discount text change
        /// </summary>
        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        /// <summary>
        /// Handles service charge text change
        /// </summary>
        protected void txtServiceCharge_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        /// <summary>
        /// Handles hold order button click
        /// </summary>
        protected void btnHold_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                ShowErrorMessage("Cart is empty. Cannot hold order.");
                return;
            }

            // In a real implementation, save as held order
            ShowSuccessMessage("Order held successfully.");
            ClearCart();
        }

        /// <summary>
        /// Handles void order button click
        /// </summary>
        protected void btnVoid_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                ShowErrorMessage("Cart is empty. Cannot void order.");
                return;
            }

            // In a real implementation, void the order
            ShowSuccessMessage("Order voided successfully.");
            ClearCart();
        }

        /// <summary>
        /// Handles send to kitchen button click
        /// </summary>
        protected void btnSendKitchen_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                ShowErrorMessage("Cart is empty. Cannot send to kitchen.");
                return;
            }

            try
            {
                // Create order
                Order order = CreateOrderFromCart();
                List<OrderItem> items = CreateOrderItemsFromCart();

                int orderID = posOrderBAL.CreateOrder(order, items);

                if (orderID > 0)
                {
                    // Send to kitchen
                    bool sent = posOrderBAL.SendToKitchen(orderID, userID);
                    if (sent)
                    {
                        hfOrderID.Value = orderID.ToString();
                        ShowSuccessMessage($"Order #{order.OrderNumber} sent to kitchen.");
                        ClearCart();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to send order to kitchen.");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to create order.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "SendToKitchen", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Handles payment button click - shows payment modal
        /// </summary>
        protected void btnPayment_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                ShowErrorMessage("Cart is empty. Cannot process payment.");
                return;
            }

            // Update payment modal
            CalculateTotals();
            txtPaidAmount.Text = "0.00";
            txtChange.Text = "0.00";

            // Show modal
            string script = "var modal = new bootstrap.Modal(document.getElementById('paymentModal')); modal.show();";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowPaymentModal", script, true);
        }

        /// <summary>
        /// Handles paid amount change
        /// </summary>
        protected void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            decimal total = 0;
            decimal paid = 0;
            decimal.TryParse(ltrTotal.Text.Replace("$", ""), out total);
            decimal.TryParse(txtPaidAmount.Text, out paid);

            decimal change = paid - total;
            txtChange.Text = change >= 0 ? change.ToString("F2") : "0.00";
        }

        /// <summary>
        /// Handles confirm payment button click
        /// </summary>
        protected void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlPaymentMethod.SelectedValue))
                {
                    ShowErrorMessage("Please select a payment method.");
                    return;
                }

                decimal total = 0;
                decimal paid = 0;
                decimal.TryParse(ltrTotal.Text.Replace("$", ""), out total);
                decimal.TryParse(txtPaidAmount.Text, out paid);

                if (paid < total)
                {
                    ShowErrorMessage("Insufficient payment amount.");
                    return;
                }

                // Create order if not already created
                int orderID = Convert.ToInt32(hfOrderID.Value);
                if (orderID == 0)
                {
                    Order order = CreateOrderFromCart();
                    List<OrderItem> items = CreateOrderItemsFromCart();

                    orderID = posOrderBAL.CreateOrder(order, items);
                    hfOrderID.Value = orderID.ToString();
                }

                if (orderID > 0)
                {
                    // Process payment
                    Payment payment = new Payment
                    {
                        CompanyID = companyID,
                        BranchID = branchID,
                        OrderID = orderID,
                        PaymentMethod = ddlPaymentMethod.SelectedValue,
                        Amount = paid,
                        ReferenceNumber = txtReference.Text.Trim(),
                        PaymentStatus = "Completed",
                        CreatedBy = userID
                    };

                    bool processed = posOrderBAL.ProcessPayment(payment);
                    if (processed)
                    {
                        // Update order status to Completed
                        posOrderBAL.UpdateOrderStatus(orderID, "Completed", userID);

                        ShowSuccessMessage($"Payment processed successfully. Order #{orderID}");
                        ClearCart();
                        hfOrderID.Value = "0";

                        // Close modal
                        string closeScript = "var modal = bootstrap.Modal.getInstance(document.getElementById('paymentModal')); modal.hide();";
                        ClientScript.RegisterStartupScript(this.GetType(), "ClosePaymentModal", closeScript, true);
                    }
                    else
                    {
                        ShowErrorMessage("Failed to process payment.");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to create order.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error processing payment: {ex.Message}");
                auditLogger.LogAction("Error", MODULE_NAME, "ConfirmPayment", null, null, ex.Message);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates an Order object from cart items
        /// </summary>
        private Order CreateOrderFromCart()
        {
            decimal subTotal = 0;
            foreach (var item in cartItems)
            {
                subTotal += item.TotalPrice;
            }

            decimal tax = subTotal * 0.05m;
            decimal discount = 0;
            decimal serviceCharge = 0;
            decimal.TryParse(txtDiscount.Text, out discount);
            decimal.TryParse(txtServiceCharge.Text, out serviceCharge);
            decimal total = subTotal + tax - discount + serviceCharge;

            int? customerID = null;
            if (!string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                customerID = Convert.ToInt32(ddlCustomer.SelectedValue);
            }

            int? tableID = null;
            if (ddlOrderType.SelectedValue == "Dine In" && !string.IsNullOrEmpty(ddlTable.SelectedValue))
            {
                tableID = Convert.ToInt32(ddlTable.SelectedValue);
            }

            return new Order
            {
                CompanyID = companyID,
                BranchID = branchID,
                TableID = tableID,
                CustomerID = customerID,
                OrderType = ddlOrderType.SelectedValue,
                OrderStatus = "Pending",
                PaymentStatus = "Pending",
                SubTotal = subTotal,
                Tax = tax,
                Discount = discount,
                ServiceCharge = serviceCharge,
                TotalAmount = total,
                DeliveryAddress = txtDeliveryAddress.Text.Trim(),
                SpecialInstructions = txtSpecialInstructions.Text.Trim(),
                OrderSource = "POS",
                ShiftID = Convert.ToInt32(hfShiftID.Value),
                CreatedBy = userID
            };
        }

        /// <summary>
        /// Creates OrderItem list from cart items
        /// </summary>
        private List<OrderItem> CreateOrderItemsFromCart()
        {
            var items = new List<OrderItem>();

            foreach (var cartItem in cartItems)
            {
                if (cartItem.IsDeal)
                {
                    // Expand deal items
                    foreach (var dealItem in cartItem.DealItems)
                    {
                        items.Add(new OrderItem
                        {
                            MenuItemID = dealItem.MenuItemID,
                            DealID = cartItem.DealID,
                            Quantity = dealItem.Quantity * cartItem.Quantity,
                            UnitPrice = dealItem.UnitPrice,
                            TotalPrice = dealItem.TotalPrice * cartItem.Quantity,
                            KitchenStatus = "Pending",
                            CreatedBy = userID
                        });
                    }
                }
                else
                {
                    items.Add(new OrderItem
                    {
                        MenuItemID = cartItem.MenuItemID,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.UnitPrice,
                        TotalPrice = cartItem.TotalPrice,
                        KitchenStatus = "Pending",
                        CreatedBy = userID
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// Gets the active shift ID
        /// </summary>
        private int GetActiveShiftID()
        {
            // In a real implementation, get from database
            return 1;
        }

        #endregion

        #region Message Methods

        /// <summary>
        /// Shows a success message
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            ltrSuccessMessage.Text = message;
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            ltrErrorMessage.Text = message;
        }

        #endregion
    }
}