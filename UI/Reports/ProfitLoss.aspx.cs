// ============================================
// UI/Reports/ProfitLoss.aspx.cs
// ============================================

using System;
using System.Data;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Reports
{
    /// <summary>
    /// Profit & Loss Report Page - Displays profit and loss summary
    /// </summary>
    public partial class ProfitLoss : Page
    {
        private ReportBAL reportBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - initializes report
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            reportBAL = new ReportBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime lastDay = DateTime.Now;

                txtDateFrom.Text = firstDay.ToString("yyyy-MM-dd");
                txtDateTo.Text = lastDay.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Generates the profit & loss report
        /// </summary>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                DateTime dateFrom = Convert.ToDateTime(txtDateFrom.Text);
                DateTime dateTo = Convert.ToDateTime(txtDateTo.Text);

                if (dateFrom > dateTo)
                {
                    ShowErrorMessage("Start date cannot be after end date.");
                    return;
                }

                // Get report data
                DataTable reportData = reportBAL.GetProfitLossReport(branchID.Value, dateFrom, dateTo);

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    DataRow row = reportData.Rows[0];

                    // Revenue
                    decimal totalSales = Convert.ToDecimal(row["TotalSales"]);
                    decimal totalOrders = 0; // Not in this report, would need separate query

                    // Expenses
                    decimal totalExpenses = Convert.ToDecimal(row["TotalExpenses"]);
                    decimal totalCOGS = Convert.ToDecimal(row["TotalCOGS"]);
                    decimal totalTax = Convert.ToDecimal(row["TotalTax"]);
                    decimal totalDiscount = Convert.ToDecimal(row["TotalDiscount"]);

                    // Profit
                    decimal grossProfit = Convert.ToDecimal(row["GrossProfit"]);
                    decimal grossMargin = Convert.ToDecimal(row["GrossProfitMargin"]);
                    decimal netProfit = Convert.ToDecimal(row["NetProfit"]);
                    decimal netMargin = Convert.ToDecimal(row["NetProfitMargin"]);

                    // Show sections
                    divSummary.Visible = true;
                    divResults.Visible = true;

                    // Update revenue section
                    ltrTotalSales.Text = totalSales.ToString("N2");
                    ltrTotalOrders.Text = "0"; // Would need separate query
                    ltrAvgOrder.Text = "0.00"; // Would need separate query

                    // Update expense section
                    ltrTotalExpenses.Text = totalExpenses.ToString("N2");
                    ltrTotalCOGS.Text = totalCOGS.ToString("N2");
                    ltrTotalTax.Text = totalTax.ToString("N2");
                    ltrTotalDiscount.Text = totalDiscount.ToString("N2");

                    // Update profit section
                    ltrGrossProfit.Text = grossProfit.ToString("N2");
                    ltrGrossMargin.Text = grossMargin.ToString("F2") + "%";
                    ltrGrossProfitColor.Text = grossProfit >= 0 ? "success" : "danger";
                    ltrGrossMarginColor.Text = grossMargin >= 0 ? "success" : "danger";

                    ltrNetProfit.Text = netProfit.ToString("N2");
                    ltrNetMargin.Text = netMargin.ToString("F2") + "%";
                    ltrNetProfitColor.Text = netProfit >= 0 ? "success" : "danger";
                    ltrNetMarginColor.Text = netMargin >= 0 ? "success" : "danger";

                    // Calculate profit per sale
                    decimal profitPerSale = totalOrders > 0 ? netProfit / totalOrders : 0;
                    ltrProfitPerSale.Text = profitPerSale.ToString("N2");
                    ltrProfitPerSaleColor.Text = profitPerSale >= 0 ? "success" : "danger";
                    ltrProfitPerSaleBg.Text = profitPerSale >= 0 ? "success" : "danger";

                    auditLogger.LogAction("ViewReport", "Reports", "ProfitLoss", null,
                        $"BranchID: {branchID}, DateFrom: {dateFrom}, DateTo: {dateTo}");
                }
                else
                {
                    ShowErrorMessage("No data found for the selected period.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "ProfitLoss", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Exports report to Excel
        /// </summary>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!divResults.Visible)
                {
                    ShowErrorMessage("No data to export. Please generate the report first.");
                    return;
                }

                // Create a DataTable with the summary data
                DataTable dt = new DataTable();
                dt.Columns.Add("Metric");
                dt.Columns.Add("Value");

                dt.Rows.Add("Total Sales", $"${ltrTotalSales.Text}");
                dt.Rows.Add("Total Expenses", $"${ltrTotalExpenses.Text}");
                dt.Rows.Add("Cost of Goods Sold", $"${ltrTotalCOGS.Text}");
                dt.Rows.Add("Total Tax", $"${ltrTotalTax.Text}");
                dt.Rows.Add("Total Discount", $"${ltrTotalDiscount.Text}");
                dt.Rows.Add("Gross Profit", $"${ltrGrossProfit.Text} ({ltrGrossMargin.Text})");
                dt.Rows.Add("Net Profit", $"${ltrNetProfit.Text} ({ltrNetMargin.Text})");

                ReportHelper reportHelper = new ReportHelper();
                reportHelper.ExportToExcel(dt, $"ProfitLoss_{DateTime.Now:yyyyMMdd_HHmmss}");

                auditLogger.LogAction("Export", "Reports", "ProfitLoss", null, "Exported to Excel");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error exporting to Excel: {ex.Message}");
            }
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