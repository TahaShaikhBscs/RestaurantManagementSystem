// ============================================
// BAL/DashboardBAL.cs
// ============================================

namespace RestaurantManagementSystem.BAL
{
    public class TopSellingDeal
    {
        public int DealID { get; internal set; }
        public string DealName { get; internal set; }
        public int OrderCount { get; internal set; }
        public decimal TotalRevenue { get; internal set; }
        public decimal AverageOrderValue { get; internal set; }
    }
}