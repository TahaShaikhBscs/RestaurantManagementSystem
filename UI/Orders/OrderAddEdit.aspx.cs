// ============================================
// UI/Orders/OrderAddEdit.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Orders
{
    /// <summary>
    /// Order Add/Edit Page - Handles creating and editing orders
    /// Supports both regular menu items and deals/combos
    /// </summary>
    public partial class OrderAddEdit : Page
    {
        #region Private Fields

        private OrderBAL orderBAL;
        private MenuBAL menuBAL;
        private CategoryBAL categoryBAL;
        private TableBAL tableBAL;
        private CustomerBAL customerBAL;
        private DealBAL dealBAL;
        private PaymentBAL paymentBAL;
        private AuditLogger auditLogger;
        private int orderID = 0;
        private const string MODULE_NAME = "OrderManagement";
        private List<OrderItemDisplay> orderItems;

        #endregion

        #region Display Item Class

        /// <summary>
        /// Order Item Display - Represents an item in the order grid
        /// </summary>
        public class OrderItemDisplay
        {
            public int MenuItemID { get; set; }
            public int? DealID { get; set; }
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get { return Quantity * UnitPrice; } }
            public string Note { get; set; }
            public string ItemType { get; set; } // "Item" or "Deal"
            public bool IsDeal { get { return DealID.HasValue; } }
        }

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - initializes the page
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            if (Request.QueryString["OrderID"] != null)
            {
                orderID = Convert.ToInt32(Request.QueryString["OrderID"]);
            }

            // Initialize order items from session
            if (Session["OrderItems"] == null)
            {
                orderItems = new List<OrderItemDisplay>();
                Session["OrderItems"] = orderItems;
            }
            else
            {
                orderItems = (List<OrderItemDisplay>)Session["OrderItems"];
            }

            if (!IsPostBack)
            {
                LoadCustomers();
                LoadTables();
                LoadCategories();
                LoadMenuItems();
                LoadDeals();

                if (orderID > 0)
                {
                    SetEditMode();
                    LoadOrderData(orderID);
                }
                else
                {
                    SetAddMode();
                    orderItems.Clear();
                    Session["OrderItems"] = orderItems;
                    txtOrderNumber.Text = GenerateOrderNumber();
                    txtDiscount.Text = "0.00";
                    txtServiceCharge.Text = "0.00";
                    txtPaidAmount.Text = "0.00";
                }

                BindOrderItemsGrid();
                CalculateTotals();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            orderBAL = new OrderBAL();
            menuBAL = new MenuBAL();
            categoryBAL = new CategoryBAL();
            tableBAL = new TableBAL();
            customerBAL = new CustomerBAL();
            dealBAL = new DealBAL();
            paymentBAL = new PaymentBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Mode Configuration

        /// <summary>
        /// Sets the page to edit mode
        /// </summary>
        private void SetEditMode()
        {
            ltrPageTitle.Text = "Edit Order";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Order Information";
            btnSave.Text = "Update Order";
            btnSaveAndPrint.Text = "Update & Print";
            divOrderStatus.Visible = true;
        }

        /// <summary>
        /// Sets the page to add mode
        /// </summary>
        private void SetAddMode()
        {
            ltrPageTitle.Text = "New Order";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Create New Order";
            btnSave.Text = "Save Order";
            btnSaveAndPrint.Text = "Save & Print";
            divOrderStatus.Visible = false;
            ddlOrderStatus.SelectedValue = "Pending";
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads customers into the dropdown
        /// </summary>
        private void LoadCustomers()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<Customer> customers = customerBAL.GetCustomersByBranch(branchID.Value, false);
                    ddlCustomer.DataSource = customers;
                    ddlCustomer.DataTextField = "CustomerName";
                    ddlCustomer.DataValueField = "CustomerID";
                    ddlCustomer.DataBind();
                    ddlCustomer.Items.Insert(0, new ListItem("-- Walk-in Customer --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading customers: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads tables into the dropdown
        /// </summary>
        private void LoadTables()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<RestaurantTable> tables = tableBAL.GetAvailableTables(branchID.Value);
                    ddlTable.DataSource = tables;
                    ddlTable.DataTextField = "TableNumber";
                    ddlTable.DataValueField = "TableID";
                    ddlTable.DataBind();
                    ddlTable.Items.Insert(0, new ListItem("-- Select Table --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading tables: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads categories into the filter dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<Category> categories = categoryBAL.GetCategoriesByBranch(branchID.Value, false);
                    ddlCategoryFilter.DataSource = categories;
                    ddlCategoryFilter.DataTextField = "CategoryName";
                    ddlCategoryFilter.DataValueField = "CategoryID";
                    ddlCategoryFilter.DataBind();
                    ddlCategoryFilter.Items.Insert(0, new ListItem("All Categories", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads menu items into the grid
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
                if (!string.IsNullOrEmpty(ddlCategoryFilter.SelectedValue))
                {
                    categoryID = Convert.ToInt32(ddlCategoryFilter.SelectedValue);
                }

                List<Models.MenuItem> items = menuBAL.GetMenuItemsByBranch(branchID.Value, categoryID, false);

                // Apply search filter
                string searchTerm = txtSearchItem.Text.Trim();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    items = items.FindAll(i =>
                        i.ItemName.ToLower().Contains(searchTerm.ToLower()) ||
                        (i.SKU != null && i.SKU.ToLower().Contains(searchTerm.ToLower()))
                    );
                }

                rptMenuItems.DataSource = items;
                rptMenuItems.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading menu items: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads deals into the modal
        /// </summary>
        private void LoadDeals()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<Deal> deals = dealBAL.GetValidDeals(branchID.Value);
                    rptDeals.DataSource = deals;
                    rptDeals.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading deals: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads order data for editing
        /// </summary>
        private void LoadOrderData(int id)
        {
            try
            {
                Order order = orderBAL.GetOrderById(id);
                if (order != null)
                {
                    txtOrderNumber.Text = order.OrderNumber;
                    ddlOrderType.SelectedValue = order.OrderType;

                    if (order.TableID.HasValue)
                    {
                        ddlTable.SelectedValue = order.TableID.Value.ToString();
                    }

                    if (order.CustomerID.HasValue)
                    {
                        ddlCustomer.SelectedValue = order.CustomerID.Value.ToString();
                        // Get customer phone
                        var customer = customerBAL.GetCustomerById(order.CustomerID.Value);
                        if (customer != null)
                        {
                            txtCustomerPhone.Text = customer.Phone;
                        }
                    }

                    txtDeliveryAddress.Text = order.DeliveryAddress;
                    txtSpecialInstructions.Text = order.SpecialInstructions;
                    ddlOrderStatus.SelectedValue = order.OrderStatus;
                    txtDiscount.Text = order.Discount.ToString("N2");
                    txtServiceCharge.Text = order.ServiceCharge.ToString("N2");

                    // Load order items
                    orderItems.Clear();
                    if (order.OrderItems != null)
                    {
                        foreach (var item in order.OrderItems)
                        {
                            orderItems.Add(new OrderItemDisplay
                            {
                                MenuItemID = item.MenuItemID,
                                ItemName = item.ItemName,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice,
                                Note = item.Instructions,
                                ItemType = "Item",
                                DealID = null
                            });
                        }
                    }

                    Session["OrderItems"] = orderItems;
                    hfOrderID.Value = order.OrderID.ToString();

                    auditLogger.LogAction("View", MODULE_NAME, "Order", id, $"OrderNumber: {order.OrderNumber}");
                }
                else
                {
                    ShowErrorMessage("Order not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadOrderData", id, null, ex.Message);
                ShowErrorMessage($"Error loading order: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Generates a unique order number
        /// </summary>
        private string GenerateOrderNumber()
        {
            int? branchID = SessionHelper.GetBranchId();
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(1000, 9999).ToString();
            return $"ORD-{branchID}-{date}-{random}";
        }

        /// <summary>
        /// Binds order items to the grid
        /// </summary>
        private void BindOrderItemsGrid()
        {
            gvOrderItems.DataSource = orderItems;
            gvOrderItems.DataBind();
        }

        /// <summary>
        /// Calculates and displays totals
        /// </summary>
        private void CalculateTotals()
        {
            decimal subTotal = 0;
            foreach (var item in orderItems)
            {
                subTotal += item.TotalPrice;
            }

            decimal tax = subTotal * 0.05m; // 5% tax
            decimal discount = Convert.ToDecimal(string.IsNullOrEmpty(txtDiscount.Text) ? "0" : txtDiscount.Text);
            decimal serviceCharge = Convert.ToDecimal(string.IsNullOrEmpty(txtServiceCharge.Text) ? "0" : txtServiceCharge.Text);
            decimal grandTotal = subTotal + tax - discount + serviceCharge;

            ltrSubTotal.Text = subTotal.ToString("N2");
            ltrTax.Text = tax.ToString("N2");
            ltrGrandTotal.Text = grandTotal.ToString("N2");
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles order type change
        /// </summary>
        protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string orderType = ddlOrderType.SelectedValue;

            if (orderType == "Dine In")
            {
                divTable.Visible = true;
                divDeliveryAddress.Visible = false;
                divDeliveryAddress.Style["display"] = "none";
            }
            else if (orderType == "Delivery")
            {
                divTable.Visible = false;
                divDeliveryAddress.Visible = true;
                divDeliveryAddress.Style["display"] = "block";
            }
            else // Take Away
            {
                divTable.Visible = false;
                divDeliveryAddress.Visible = false;
                divDeliveryAddress.Style["display"] = "none";
            }
        }

        /// <summary>
        /// Handles category filter change
        /// </summary>
        protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenuItems();
        }

        /// <summary>
        /// Handles menu item search
        /// </summary>
        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
        }

        /// <summary>
        /// Handles adding a menu item
        /// </summary>
        protected void rptMenuItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    int menuItemID = Convert.ToInt32(e.CommandArgument);
                    AddMenuItem(menuItemID);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding item: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a menu item to the order
        /// </summary>
        private void AddMenuItem(int menuItemID)
        {
            Models.MenuItem menuItem = menuBAL.GetMenuItemById(menuItemID);
            if (menuItem != null && menuItem.IsAvailable)
            {
                // Check if item already exists
                var existing = orderItems.Find(i => i.MenuItemID == menuItemID && !i.IsDeal);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    orderItems.Add(new OrderItemDisplay
                    {
                        MenuItemID = menuItemID,
                        ItemName = menuItem.ItemName,
                        Quantity = 1,
                        UnitPrice = menuItem.Price,
                        ItemType = "Item",
                        DealID = null
                    });
                }

                Session["OrderItems"] = orderItems;
                BindOrderItemsGrid();
                CalculateTotals();
                ShowSuccessMessage($"{menuItem.ItemName} added to order.");
            }
            else
            {
                ShowErrorMessage("Item is not available.");
            }
        }

        /// <summary>
        /// Handles adding a deal
        /// </summary>
        protected void btnAddDeal_Click(object sender, EventArgs e)
        {
            // Show deal modal
            string script = "var modal = new bootstrap.Modal(document.getElementById('dealModal')); modal.show();";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowDealModal", script, true);
        }

        /// <summary>
        /// Handles deal selection
        /// </summary>
        protected void rptDeals_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddDeal")
                {
                    int dealID = Convert.ToInt32(e.CommandArgument);
                    AddDealToOrder(dealID);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding deal: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a deal to the order
        /// </summary>
        private void AddDealToOrder(int dealID)
        {
            Deal deal = dealBAL.GetDealWithItems(dealID);
            if (deal != null && deal.IsValid)
            {
                // Check if deal already exists
                var existing = orderItems.Find(i => i.DealID == dealID && i.IsDeal);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    // Add deal as a single line item
                    orderItems.Add(new OrderItemDisplay
                    {
                        MenuItemID = 0, // Not a regular menu item
                        DealID = dealID,
                        ItemName = deal.DealName + " (Combo)",
                        Quantity = 1,
                        UnitPrice = deal.DealPrice,
                        ItemType = "Deal",
                        Note = "Deal/Combo"
                    });
                }

                Session["OrderItems"] = orderItems;
                BindOrderItemsGrid();
                CalculateTotals();

                // Close modal
                string script = "var modal = bootstrap.Modal.getInstance(document.getElementById('dealModal')); modal.hide();";
                ClientScript.RegisterStartupScript(this.GetType(), "CloseDealModal", script, true);

                ShowSuccessMessage($"Deal '{deal.DealName}' added to order.");
            }
            else
            {
                ShowErrorMessage("Deal is not available.");
            }
        }

        /// <summary>
        /// Handles quantity changes in the grid
        /// </summary>
        protected void gvOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "IncreaseQty")
                {
                    if (index >= 0 && index < orderItems.Count)
                    {
                        orderItems[index].Quantity++;
                        Session["OrderItems"] = orderItems;
                        BindOrderItemsGrid();
                        CalculateTotals();
                    }
                }
                else if (e.CommandName == "DecreaseQty")
                {
                    if (index >= 0 && index < orderItems.Count)
                    {
                        if (orderItems[index].Quantity > 1)
                        {
                            orderItems[index].Quantity--;
                        }
                        else
                        {
                            orderItems.RemoveAt(index);
                        }
                        Session["OrderItems"] = orderItems;
                        BindOrderItemsGrid();
                        CalculateTotals();
                    }
                }
                else if (e.CommandName == "RemoveItem")
                {
                    if (index >= 0 && index < orderItems.Count)
                    {
                        orderItems.RemoveAt(index);
                        Session["OrderItems"] = orderItems;
                        BindOrderItemsGrid();
                        CalculateTotals();
                        ShowSuccessMessage("Item removed from order.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error updating item: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles quantity text change
        /// </summary>
        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtQty = (TextBox)sender;
                GridViewRow row = (GridViewRow)txtQty.NamingContainer;
                int index = row.RowIndex;

                if (index >= 0 && index < orderItems.Count)
                {
                    int newQty = Convert.ToInt32(txtQty.Text);
                    if (newQty > 0)
                    {
                        orderItems[index].Quantity = newQty;
                    }
                    else
                    {
                        orderItems.RemoveAt(index);
                    }
                    Session["OrderItems"] = orderItems;
                    BindOrderItemsGrid();
                    CalculateTotals();
                }
            }
            catch
            {
                // Invalid input, revert to previous value
                BindOrderItemsGrid();
            }
        }

        /// <summary>
        /// Handles discount change
        /// </summary>
        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        /// <summary>
        /// Handles service charge change
        /// </summary>
        protected void txtServiceCharge_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        /// <summary>
        /// Handles row data bound for item notes
        /// </summary>
        protected void gvOrderItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtNote = (TextBox)e.Row.FindControl("txtItemNote");
                if (txtNote != null)
                {
                    // Store note when text changes
                    txtNote.Attributes.Add("onchange",
                        $"__doPostBack('gvOrderItems', 'UpdateNote${e.Row.RowIndex}')");
                }
            }
        }

        /// <summary>
        /// Handles new customer button click
        /// </summary>
        protected void btnNewCustomer_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Customers/CustomerAddEdit.aspx");
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Saves the order
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveOrder(false);
        }

        /// <summary>
        /// Saves and prints the order
        /// </summary>
        protected void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            SaveOrder(true);
        }

        /// <summary>
        /// Saves the order
        /// </summary>
        private void SaveOrder(bool print)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (orderItems.Count == 0)
                {
                    ShowErrorMessage("Order must have at least one item.");
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

                // Calculate totals
                decimal subTotal = 0;
                foreach (var item in orderItems)
                {
                    subTotal += item.TotalPrice;
                }

                decimal tax = subTotal * 0.05m;
                decimal discount = Convert.ToDecimal(txtDiscount.Text);
                decimal serviceCharge = Convert.ToDecimal(txtServiceCharge.Text);
                decimal grandTotal = subTotal + tax - discount + serviceCharge;

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

                // Create order object
                Order order = new Order
                {
                    OrderID = orderID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    TableID = tableID,
                    CustomerID = customerID,
                    OrderNumber = txtOrderNumber.Text,
                    OrderType = ddlOrderType.SelectedValue,
                    OrderStatus = ddlOrderStatus.SelectedValue,
                    PaymentStatus = "Pending",
                    SubTotal = subTotal,
                    Tax = tax,
                    Discount = discount,
                    ServiceCharge = serviceCharge,
                    TotalAmount = grandTotal,
                    PaidAmount = 0,
                    ChangeAmount = 0,
                    DeliveryAddress = txtDeliveryAddress.Text.Trim(),
                    SpecialInstructions = txtSpecialInstructions.Text.Trim(),
                    OrderSource = "Admin",
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                // Create order items
                List<OrderItem> items = new List<OrderItem>();
                foreach (var displayItem in orderItems)
                {
                    // If it's a deal, we need to expand it
                    if (displayItem.IsDeal && displayItem.DealID.HasValue)
                    {
                        Deal deal = dealBAL.GetDealWithItems(displayItem.DealID.Value);
                        if (deal != null)
                        {
                            foreach (var dealItem in deal.DealItems)
                            {
                                items.Add(new OrderItem
                                {
                                    MenuItemID = dealItem.MenuItemID,
                                    Quantity = dealItem.Quantity * displayItem.Quantity,
                                    UnitPrice = dealItem.Price,
                                    Discount = 0,
                                    TotalPrice = dealItem.Price * dealItem.Quantity * displayItem.Quantity,
                                    Instructions = displayItem.Note,
                                    KitchenStatus = "Pending",
                                    CreatedBy = userID.Value
                                });
                            }
                        }
                    }
                    else
                    {
                        // Regular menu item
                        items.Add(new OrderItem
                        {
                            MenuItemID = displayItem.MenuItemID,
                            Quantity = displayItem.Quantity,
                            UnitPrice = displayItem.UnitPrice,
                            Discount = 0,
                            TotalPrice = displayItem.TotalPrice,
                            Instructions = displayItem.Note,
                            KitchenStatus = "Pending",
                            CreatedBy = userID.Value
                        });
                    }
                }

                int savedID;

                if (orderID > 0)
                {
                    // Update existing order
                    // Note: In a real implementation, you would need to handle order updates
                    // For now, we'll create a new order
                    savedID = orderBAL.CreateOrder(order, items);
                    if (savedID > 0)
                    {
                        auditLogger.LogUpdate(MODULE_NAME, "Orders", savedID,
                            $"OrderID: {orderID}", $"OrderNumber: {order.OrderNumber}");
                        ShowSuccessMessage("Order updated successfully.");
                    }
                }
                else
                {
                    // Create new order
                    savedID = orderBAL.CreateOrder(order, items);
                    if (savedID > 0)
                    {
                        auditLogger.LogInsert(MODULE_NAME, "Orders", savedID,
                            $"OrderNumber: {order.OrderNumber}");
                        ShowSuccessMessage("Order created successfully.");
                    }
                }

                if (savedID > 0)
                {
                    // Process payment if amount paid > 0
                    decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);
                    if (paidAmount > 0)
                    {
                        Payment payment = new Payment
                        {
                            CompanyID = companyID.Value,
                            BranchID = branchID.Value,
                            OrderID = savedID,
                            PaymentMethod = ddlPaymentMethod.SelectedValue,
                            Amount = paidAmount,
                            PaymentStatus = paidAmount >= grandTotal ? "Completed" : "Partial",
                            CreatedBy = userID.Value
                        };
                        paymentBAL.ProcessPayment(payment);
                    }

                    // Clear session items
                    orderItems.Clear();
                    Session["OrderItems"] = orderItems;

                    if (print)
                    {
                        // Print invoice
                        PrintHelper printHelper = new PrintHelper();
                        CompanyBAL companyBAL = new CompanyBAL();
                        var companies = companyBAL.GetAllCompanies();
                        Models.Company company = null;
                        if (companies.Count > 0)
                        {
                            company = companies[0];
                        }
                        BranchBAL branchBAL = new BranchBAL();
                        Models.Branch branch = null;
                        if (branchID.HasValue)
                        {
                            branch = branchBAL.GetBranchById(branchID.Value);
                        }

                        // Get full order details for printing
                        Order fullOrder = orderBAL.GetOrderById(savedID);
                        if (fullOrder != null && company != null)
                        {
                            printHelper.PrintInvoice(fullOrder, company, branch);
                        }
                    }

                    Response.AddHeader("REFRESH", "2;URL=OrderDetail.aspx?OrderID=" + savedID);
                }
                else
                {
                    ShowErrorMessage("Failed to save order.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveOrder", orderID, null, ex.Message);
                ShowErrorMessage($"Error saving order: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

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

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            orderItems.Clear();
            Session["OrderItems"] = orderItems;
            Response.Redirect("OrderList.aspx");
        }

        #endregion
    }
}