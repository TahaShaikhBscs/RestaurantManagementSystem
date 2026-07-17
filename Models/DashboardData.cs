
using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Dashboard Model - Aggregates data for the dashboard view
    /// </summary>
    public class DashboardData
    {
        public decimal TodaySales { get; set; }
        public decimal MonthlySales { get; set; }
        public int TotalOrders { get; set; }
        public int RunningOrders { get; set; }
        public int PendingOrders { get; set; }
        public int KitchenOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Profit { get; set; }
        public List<TopSellingItem> TopSellingItems { get; set; }
        public List<RecentOrder> RecentOrders { get; set; }
        public List<RecentPayment> RecentPayments { get; set; }
        public List<DailySalesData> DailySalesChart { get; set; }
    }

    public class TopSellingItem
    {
        public string ItemName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class RecentOrder
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class RecentPayment
    {
        public int PaymentID { get; set; }
        public string OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class DailySalesData
    {
        public string Date { get; set; }
        public decimal Sales { get; set; }
        public int Orders { get; set; }
    }
}