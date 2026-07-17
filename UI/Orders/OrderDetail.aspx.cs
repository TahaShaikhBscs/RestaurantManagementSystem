// ============================================
// UI/Orders/OrderDetail.aspx.cs
// ============================================

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using OrderModel = RestaurantManagementSystem.Models.Order;
using OrderItemModel = RestaurantManagementSystem.Models.OrderItem;
using PaymentModel = RestaurantManagementSystem.Models.Payment;

namespace RestaurantManagementSystem.UI.Orders
{
    /// <summary>
    /// Order Detail Page - Displays complete order details
    /// </summary>
    public partial class OrderDetail : Page
    {
        private OrderBAL orderBAL;
        private AuditLogger auditLogger;
        private int orderID = 0;

        /// <summary>
        /// Page load event - loads order details
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            orderBAL = new OrderBAL();
            auditLogger = new AuditLogger();

            if (Request.QueryString["OrderID"] != null)
            {
                orderID = Convert.ToInt32(Request.QueryString["OrderID"]);
            }

            if (!IsPostBack)
            {
                if (orderID > 0)
                {
                    LoadOrderDetails(orderID);
                }
                else
                {
                    ShowErrorMessage("Invalid order ID.");
                }
            }
        }

        /// <summary>
        /// Loads complete order details
        /// </summary>
        private void LoadOrderDetails(int id)
        {
            try
            {
                OrderModel order = orderBAL.GetOrderById(id);
                if (order != null)
                {
                    // Set order number
                    ltrOrderNumber.Text = order.OrderNumber;
                    ltrOrderNum.Text = order.OrderNumber;
                    ltrOrderDate.Text = order.OrderDate.ToString("dd/MM/yyyy HH:mm");
                    ltrOrderType.Text = order.OrderType;
                    ltrOrderStatus.Text = GetStatusBadgeHtml(order.OrderStatus);
                    ltrPaymentStatus.Text = GetPaymentStatusBadgeHtml(order.PaymentStatus);
                    ltrOrderSource.Text = order.OrderSource ?? "POS";

                    // Customer info
                    ltrCustomerName.Text = order.CustomerName ?? "Walk-in";
                    ltrCustomerPhone.Text = order.CustomerPhone ?? "-";
                    ltrTableNumber.Text = order.TableNumber ?? "-";
                    ltrDeliveryAddress.Text = order.DeliveryAddress ?? "-";
                    ltrInstructions.Text = string.IsNullOrEmpty(order.SpecialInstructions) ? "None" : order.SpecialInstructions;

                    // Load order items
                    if (order.OrderItems != null && order.OrderItems.Count > 0)
                    {
                        gvItems.DataSource = order.OrderItems;
                        gvItems.DataBind();
                        ltrItemCount.Text = order.OrderItems.Count.ToString();

                        // Calculate totals
                        decimal subTotal = 0, tax = 0, discount = 0, serviceCharge = 0;
                        foreach (var item in order.OrderItems)
                        {
                            subTotal += item.UnitPrice * item.Quantity;
                            tax += (item.UnitPrice * item.Quantity * (order.Tax / 100));
                        }

                        ltrSubTotal.Text = order.SubTotal.ToString("N2");
                        ltrTax.Text = order.Tax.ToString("N2");
                        ltrDiscount.Text = order.Discount.ToString("N2");
                        ltrServiceCharge.Text = order.ServiceCharge.ToString("N2");
                        ltrTotalAmount.Text = order.TotalAmount.ToString("N2");
                        ltrPaidAmount.Text = order.PaidAmount.ToString("N2");
                        ltrChangeAmount.Text = order.ChangeAmount.ToString("N2");
                    }

                    // Load payments
                    if (order.Payments != null && order.Payments.Count > 0)
                    {
                        divPayments.Visible = true;
                        gvPayments.DataSource = order.Payments;
                        gvPayments.DataBind();
                    }

                    // Load timeline - would need separate query
                    // For now, we'll hide timeline
                    divTimeline.Visible = false;

                    auditLogger.LogAction("ViewOrder", "OrderManagement", "Orders", orderID, null, $"OrderID: {orderID}");
                }
                else
                {
                    ShowErrorMessage("Order not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading order details: {ex.Message}");
                auditLogger.LogAction("Error", "OrderManagement", "OrderDetail", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Gets status badge HTML
        /// </summary>
        private string GetStatusBadgeHtml(string status)
        {
            string badgeClass = GetStatusBadgeClass(status);
            string iconClass = GetStatusIconClass(status);
            return $"<span class='badge bg-{badgeClass} px-3 py-2'><i class='fas fa-{iconClass} me-1'></i>{status}</span>";
        }

        /// <summary>
        /// Gets payment status badge HTML
        /// </summary>
        private string GetPaymentStatusBadgeHtml(string status)
        {
            string badgeClass = GetPaymentStatusBadgeClass(status);
            return $"<span class='badge bg-{badgeClass} px-3 py-2'>{status}</span>";
        }

        /// <summary>
        /// Gets Bootstrap badge class for status
        /// </summary>
        private string GetStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Pending": return "warning";
                case "Cooking": return "info";
                case "Ready": return "primary";
                case "Completed": return "success";
                case "Cancelled": return "danger";
                default: return "secondary";
            }
        }

        /// <summary>
        /// Gets Font Awesome icon class for status
        /// </summary>
        private string GetStatusIconClass(string status)
        {
            switch (status)
            {
                case "Pending": return "clock";
                case "Cooking": return "utensils";
                case "Ready": return "check-circle";
                case "Completed": return "check-double";
                case "Cancelled": return "times-circle";
                default: return "question-circle";
            }
        }

        /// <summary>
        /// Gets Bootstrap badge class for payment status
        /// </summary>
        private string GetPaymentStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Completed": return "success";
                case "Partial": return "warning";
                case "Pending": return "danger";
                default: return "secondary";
            }
        }

        /// <summary>
        /// Redirects back to order list
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderList.aspx");
        }

        /// <summary>
        /// Prints the order details
        /// </summary>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            // Generate printable version
            string printScript = "window.print();";
            ClientScript.RegisterStartupScript(this.GetType(), "Print", printScript, true);

            auditLogger.LogAction("Print", "OrderManagement", "Orders", orderID, null, $"OrderID: {orderID}");
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