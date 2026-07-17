// ============================================
// UI/Purchases/PurchaseAddEdit.aspx.cs
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
using PurchaseOrderModel = RestaurantManagementSystem.Models.PurchaseOrder;
using PurchaseOrderItemModel = RestaurantManagementSystem.Models.PurchaseOrderItem;
using SupplierModel = RestaurantManagementSystem.Models.Supplier;
using StockModel = RestaurantManagementSystem.Models.Stock;

namespace RestaurantManagementSystem.UI.Purchases
{
    /// <summary>
    /// Purchase Order Add/Edit Page - Handles adding and editing purchase orders
    /// </summary>
    public partial class PurchaseAddEdit : Page
    {
        private PurchaseBAL purchaseBAL;
        private SupplierBAL supplierBAL;
        private StockBAL stockBAL;
        private AuditLogger auditLogger;
        private int purchaseOrderID = 0;
        private List<PurchaseOrderItemModel> items;

        /// <summary>
        /// Page load event - loads purchase order data for editing
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            purchaseBAL = new PurchaseBAL();
            supplierBAL = new SupplierBAL();
            stockBAL = new StockBAL();
            auditLogger = new AuditLogger();

            if (Request.QueryString["PurchaseOrderID"] != null)
            {
                purchaseOrderID = Convert.ToInt32(Request.QueryString["PurchaseOrderID"]);
            }

            // Initialize items from session
            if (Session["POItems"] == null)
            {
                items = new List<PurchaseOrderItemModel>();
                Session["POItems"] = items;
            }
            else
            {
                items = (List<PurchaseOrderItemModel>)Session["POItems"];
            }

            if (!IsPostBack)
            {
                LoadSuppliers();
                LoadStockItems();

                if (purchaseOrderID > 0)
                {
                    ltrPageTitle.Text = "Edit Purchase Order";
                    ltrBreadcrumb.Text = "Edit";
                    ltrFormTitle.Text = "Edit Purchase Order";
                    btnSave.Text = "Update Purchase Order";
                    LoadPurchaseOrderData(purchaseOrderID);
                }
                else
                {
                    ltrPageTitle.Text = "New Purchase Order";
                    ltrBreadcrumb.Text = "Add New";
                    ltrFormTitle.Text = "Create Purchase Order";
                    btnSave.Text = "Save Purchase Order";
                    txtOrderDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    items.Clear();
                    Session["POItems"] = items;
                }
            }

            BindItemsGrid();
            CalculateTotals();
        }

        /// <summary>
        /// Loads suppliers into the dropdown
        /// </summary>
        private void LoadSuppliers()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<SupplierModel> suppliers = supplierBAL.GetSuppliersByBranch(branchID.Value, false);
                    ddlSupplier.DataSource = suppliers;
                    ddlSupplier.DataTextField = "SupplierName";
                    ddlSupplier.DataValueField = "SupplierID";
                    ddlSupplier.DataBind();
                    ddlSupplier.Items.Insert(0, new ListItem("-- Select Supplier --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading suppliers: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads stock items into the dropdown
        /// </summary>
        private void LoadStockItems()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<StockModel> stock = stockBAL.GetStockByBranch(branchID.Value);
                    ddlStockItem.DataSource = stock;
                    ddlStockItem.DataTextField = "ProductName";
                    ddlStockItem.DataValueField = "StockID";
                    ddlStockItem.DataBind();
                    ddlStockItem.Items.Insert(0, new ListItem("-- Select Item --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading stock items: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads purchase order data for editing
        /// </summary>
        private void LoadPurchaseOrderData(int id)
        {
            try
            {
                DataSet dataSet = purchaseBAL.GetPurchaseOrderById(id);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    txtPONumber.Text = row["PurchaseOrderNumber"].ToString();
                    ddlSupplier.SelectedValue = row["SupplierID"].ToString();
                    txtOrderDate.Text = Convert.ToDateTime(row["OrderDate"]).ToString("yyyy-MM-dd");

                    if (row["ExpectedDeliveryDate"] != DBNull.Value)
                    {
                        txtExpectedDelivery.Text = Convert.ToDateTime(row["ExpectedDeliveryDate"]).ToString("yyyy-MM-dd");
                    }

                    ddlStatus.SelectedValue = row["OrderStatus"].ToString();
                    ddlPaymentStatus.SelectedValue = row["PaymentStatus"].ToString();
                    txtNotes.Text = row["Notes"]?.ToString();

                    // Load items
                    if (dataSet.Tables.Count > 1)
                    {
                        items.Clear();
                        foreach (DataRow itemRow in dataSet.Tables[1].Rows)
                        {
                            items.Add(new PurchaseOrderItemModel
                            {
                                StockID = Convert.ToInt32(itemRow["StockID"]),
                                ProductName = itemRow["ProductName"].ToString(),
                                Quantity = Convert.ToDecimal(itemRow["Quantity"]),
                                UnitPrice = Convert.ToDecimal(itemRow["UnitPrice"]),
                                TotalPrice = Convert.ToDecimal(itemRow["TotalPrice"])
                            });
                        }
                        Session["POItems"] = items;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading purchase order: {ex.Message}");
            }
        }

        /// <summary>
        /// Binds items to the grid view
        /// </summary>
        private void BindItemsGrid()
        {
            gvItems.DataSource = items;
            gvItems.DataBind();
        }

        /// <summary>
        /// Calculates and displays totals
        /// </summary>
        private void CalculateTotals()
        {
            decimal subTotal = 0;
            foreach (var item in items)
            {
                subTotal += item.TotalPrice;
            }

            decimal tax = subTotal * 0.05m; // 5% tax
            decimal discount = 0;
            decimal total = subTotal + tax - discount;

            ltrSubTotal.Text = subTotal.ToString("N2");
            ltrTax.Text = tax.ToString("N2");
            ltrDiscount.Text = discount.ToString("N2");
            ltrTotal.Text = total.ToString("N2");
        }

        /// <summary>
        /// Adds an item to the purchase order
        /// </summary>
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                int stockID = Convert.ToInt32(ddlStockItem.SelectedValue);
                decimal quantity = Convert.ToDecimal(txtQuantity.Text);
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);

                if (stockID <= 0)
                {
                    ShowErrorMessage("Please select an item.");
                    return;
                }

                if (quantity <= 0)
                {
                    ShowErrorMessage("Quantity must be greater than 0.");
                    return;
                }

                if (unitPrice < 0)
                {
                    ShowErrorMessage("Unit price cannot be negative.");
                    return;
                }

                // Get product name
                string productName = ddlStockItem.SelectedItem.Text;

                // Check if item already exists
                var existing = items.Find(i => i.StockID == stockID);
                if (existing != null)
                {
                    existing.Quantity += quantity;
                    existing.TotalPrice = existing.Quantity * existing.UnitPrice;
                }
                else
                {
                    items.Add(new PurchaseOrderItemModel
                    {
                        StockID = stockID,
                        ProductName = productName,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = quantity * unitPrice
                    });
                }

                Session["POItems"] = items;
                BindItemsGrid();
                CalculateTotals();

                // Clear inputs
                ddlStockItem.SelectedValue = "";
                txtQuantity.Text = "1";
                txtUnitPrice.Text = "0.00";
                txtItemTotal.Text = "0.00";

                ShowSuccessMessage("Item added successfully.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding item: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes an item from the purchase order
        /// </summary>
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "RemoveItem")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    if (index >= 0 && index < items.Count)
                    {
                        items.RemoveAt(index);
                        Session["POItems"] = items;
                        BindItemsGrid();
                        CalculateTotals();
                        ShowSuccessMessage("Item removed.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error removing item: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the purchase order
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (items.Count == 0)
                {
                    ShowErrorMessage("Please add at least one item to the purchase order.");
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

                PurchaseOrderModel order = new PurchaseOrderModel
                {
                    PurchaseOrderID = purchaseOrderID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    SupplierID = Convert.ToInt32(ddlSupplier.SelectedValue),
                    PurchaseOrderNumber = txtPONumber.Text,
                    OrderDate = Convert.ToDateTime(txtOrderDate.Text),
                    ExpectedDeliveryDate = string.IsNullOrEmpty(txtExpectedDelivery.Text) ?
                        (DateTime?)null : Convert.ToDateTime(txtExpectedDelivery.Text),
                    OrderStatus = ddlStatus.SelectedValue,
                    PaymentStatus = ddlPaymentStatus.SelectedValue,
                    Notes = txtNotes.Text.Trim(),
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                // Calculate totals
                decimal subTotal = 0;
                foreach (var item in items)
                {
                    subTotal += item.TotalPrice;
                }

                order.SubTotal = subTotal;
                order.Tax = subTotal * 0.05m;
                order.Discount = 0;
                order.TotalAmount = subTotal + order.Tax - order.Discount;

                int savedID = purchaseBAL.CreatePurchaseOrder(order, items);

                if (savedID > 0)
                {
                    auditLogger.LogInsert("PurchaseManagement", "PurchaseOrders", savedID,
                        $"PONumber: {order.PurchaseOrderNumber}, Supplier: {ddlSupplier.SelectedItem.Text}");
                    ShowSuccessMessage("Purchase order saved successfully.");

                    // Clear session items
                    items.Clear();
                    Session["POItems"] = items;

                    Response.AddHeader("REFRESH", "2;URL=PurchaseList.aspx");
                }
                else
                {
                    ShowErrorMessage("Failed to save purchase order.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving purchase order: {ex.Message}");
                auditLogger.LogAction("Error", "PurchaseManagement", "SaveOrder", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            items.Clear();
            Session["POItems"] = items;
            Response.Redirect("PurchaseList.aspx");
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
    }
}