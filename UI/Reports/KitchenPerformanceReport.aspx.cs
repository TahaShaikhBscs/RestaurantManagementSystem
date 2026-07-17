// ============================================
// UI/Reports/KitchenPerformanceReport.aspx.cs
// ============================================

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Reports
{
    /// <summary>
    /// Kitchen Performance Report Page - Displays kitchen efficiency metrics
    /// </summary>
    public partial class KitchenPerformanceReport : Page
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
        /// Generates the kitchen performance report
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
                DataTable reportData = reportBAL.GetKitchenPerformanceReport(branchID.Value, dateFrom, dateTo);

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    divSummary.Visible = true;
                    divReport.Visible = true;

                    gvReport.DataSource = reportData;
                    gvReport.DataBind();
                    ltrRecordCount.Text = reportData.Rows.Count.ToString();

                    // Calculate summary
                    int totalOrders = 0;
                    int totalPrepTime = 0;
                    int onTimeCount = 0;
                    int cancelledCount = 0;
                    int completedCount = 0;

                    foreach (DataRow row in reportData.Rows)
                    {
                        totalOrders++;
                        totalPrepTime += Convert.ToInt32(row["AveragePreparationTime"]);
                        if (row["Status"].ToString() == "Cancelled") cancelledCount++;
                        if (row["Status"].ToString() == "Ready" || row["Status"].ToString() == "Served") completedCount++;
                        // On time calculation would require estimated vs actual comparison
                    }

                    int avgPrepTime = totalOrders > 0 ? totalPrepTime / totalOrders : 0;
                    int onTimePercent = totalOrders > 0 ? (onTimeCount / totalOrders) * 100 : 0;

                    ltrTotalOrders.Text = totalOrders.ToString();
                    ltrAvgPrepTime.Text = avgPrepTime.ToString();
                    ltrOnTime.Text = onTimePercent.ToString();
                    ltrCancelled.Text = cancelledCount.ToString();

                    auditLogger.LogAction("ViewReport", "Reports", "KitchenPerformanceReport", null,
                        $"BranchID: {branchID}");
                }
                else
                {
                    divSummary.Visible = false;
                    divReport.Visible = true;
                    gvReport.DataSource = null;
                    gvReport.DataBind();
                    ShowErrorMessage("No kitchen performance data found for the selected period.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "KitchenPerformanceReport", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Exports report to Excel
        /// </summary>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvReport.Rows.Count == 0)
                {
                    ShowErrorMessage("No data to export. Please generate the report first.");
                    return;
                }

                DataTable reportData = GetReportData();
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    ReportHelper reportHelper = new ReportHelper();
                    reportHelper.ExportToExcel(reportData, $"KitchenPerformanceReport_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", "Reports", "KitchenPerformanceReport", null, "Exported to Excel");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error exporting to Excel: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the report data from the grid view
        /// </summary>
        private DataTable GetReportData()
        {
            DataTable dt = new DataTable();

            foreach (DataControlField column in gvReport.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }

            foreach (GridViewRow row in gvReport.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < gvReport.Columns.Count; i++)
                {
                    dr[i] = row.Cells[i].Text;
                }
                dt.Rows.Add(dr);
            }

            return dt;
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