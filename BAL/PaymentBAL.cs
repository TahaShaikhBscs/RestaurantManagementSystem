// ============================================
// BAL/PaymentBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Payment BAL - Business logic layer for payment operations
    /// Contains business rules and validation for payment processing
    /// </summary>
    public class PaymentBAL
    {
        private readonly PaymentDAL paymentDAL;
        private readonly OrderDAL orderDAL;

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public PaymentBAL()
        {
            paymentDAL = new PaymentDAL();
            orderDAL = new OrderDAL();
        }

        /// <summary>
        /// Processes a payment for an order
        /// </summary>
        public int ProcessPayment(Payment payment)
        {
            ValidatePayment(payment);

            // Verify order exists and is not cancelled
            var order = orderDAL.GetOrderById(payment.OrderID);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            //if (order.OrderStatus == "Cancelled" || order.OrderStatus == "Void")
            //{
            //    throw new Exception("Cannot process payment for cancelled or void order.");
            //}

            // Check if payment amount is valid
            if (payment.Amount <= 0)
            {
                throw new Exception("Payment amount must be greater than 0.");
            }

            // Check if payment exceeds order total
            //if (payment.Amount > order.TotalAmount)
            //{
            //    throw new Exception("Payment amount cannot exceed order total.");
            //}

            // Process payment
            int paymentID = paymentDAL.InsertPayment(payment);

            // Update order status if fully paid
            UpdateOrderPaymentStatus(payment.OrderID);

            return paymentID;
        }

        /// <summary>
        /// Validates payment data
        /// </summary>
        private void ValidatePayment(Payment payment)
        {
            if (payment.CompanyID <= 0)
            {
                throw new Exception("Company is required.");
            }

            if (payment.BranchID <= 0)
            {
                throw new Exception("Branch is required.");
            }

            if (payment.OrderID <= 0)
            {
                throw new Exception("Order ID is required.");
            }

            if (string.IsNullOrWhiteSpace(payment.PaymentMethod))
            {
                throw new Exception("Payment method is required.");
            }

            string[] validMethods = { "Cash", "Card", "Online", "Mobile Wallet" };
            if (!Array.Exists(validMethods, m => m == payment.PaymentMethod))
            {
                throw new Exception("Invalid payment method.");
            }

            if (payment.Amount <= 0)
            {
                throw new Exception("Payment amount must be greater than 0.");
            }
        }

        /// <summary>
        /// Updates order payment status
        /// </summary>
        private void UpdateOrderPaymentStatus(int orderID)
        {
            var payments = paymentDAL.GetPaymentsByOrder(orderID);
            var order = orderDAL.GetOrderById(orderID);

            decimal totalPaid = 0;
            foreach (var payment in payments)
            {
                if (payment.PaymentStatus == "Completed")
                {
                    totalPaid += payment.Amount;
                }
            }

            string paymentStatus = "Pending";
            //if (totalPaid >= order.TotalAmount)
            //{
            //    paymentStatus = "Completed";
            //}
            //else if (totalPaid > 0)
            //{
            //    paymentStatus = "Partial";
            //}

            // Update order payment status
            // This would need a stored procedure or direct update
            // For now, we'll use the existing order update method
        }

        /// <summary>
        /// Gets payments for an order
        /// </summary>
        public List<Payment> GetPaymentsByOrder(int orderID)
        {
            if (orderID <= 0)
            {
                throw new Exception("Invalid order ID.");
            }

            return paymentDAL.GetPaymentsByOrder(orderID);
        }

        /// <summary>
        /// Gets a payment by ID
        /// </summary>
        public Payment GetPaymentById(int paymentID)
        {
            if (paymentID <= 0)
            {
                throw new Exception("Invalid payment ID.");
            }

            return paymentDAL.GetPaymentById(paymentID);
        }

        /// <summary>
        /// Gets total payments for a date range
        /// </summary>
        public decimal GetTotalPayments(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            if (branchID <= 0)
            {
                throw new Exception("Invalid branch ID.");
            }

            return paymentDAL.GetTotalPayments(branchID, dateFrom, dateTo);
        }

        /// <summary>
        /// Processes a refund
        /// </summary>
        public bool ProcessRefund(int paymentID, decimal amount, int updatedBy)
        {
            if (paymentID <= 0)
            {
                throw new Exception("Invalid payment ID.");
            }

            var payment = paymentDAL.GetPaymentById(paymentID);
            if (payment == null)
            {
                throw new Exception("Payment not found.");
            }

            if (payment.PaymentStatus != "Completed")
            {
                throw new Exception("Only completed payments can be refunded.");
            }

            if (amount <= 0 || amount > payment.Amount)
            {
                throw new Exception("Invalid refund amount.");
            }

            // Process refund - create a negative payment
            Payment refund = new Payment
            {
                CompanyID = payment.CompanyID,
                BranchID = payment.BranchID,
                OrderID = payment.OrderID,
                PaymentMethod = payment.PaymentMethod,
                Amount = -amount,
                ReferenceNumber = $"REF-{payment.ReferenceNumber}",
                PaymentStatus = "Completed",
                CreatedBy = updatedBy
            };

            paymentDAL.InsertPayment(refund);
            return true;
        }
    }
}