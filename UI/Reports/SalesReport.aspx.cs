// ============================================
// UI/Reports/SalesReport.aspx.cs
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
    /// Sales Report Page - Displays sales reports with filtering and export options
    /// </summary>
    public partial class SalesReport : Page
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
                // Set default date range to current month
                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime lastDay = DateTime.Now;

                txtDateFrom.Text = firstDay.ToString("yyyy-MM-dd");
                txtDateTo.Text = lastDay.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Generates the report
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

                string groupBy = ddlGroupBy.SelectedValue;

                // Get report data
                DataTable reportData = reportBAL.GetSalesReport(branchID.Value, dateFrom, dateTo, groupBy);

                if (reportData.Rows.Count > 0)
                {
                    // Show report sections
                    divSummary.Visible = true;
                    divReport.Visible = true;

                    // Bind grid
                    gvReport.DataSource = reportData;
                    gvReport.DataBind();

                    // Calculate summary
                    decimal totalSales = 0;
                    decimal totalTax = 0;
                    int totalOrders = 0;

                    foreach (DataRow row in reportData.Rows)
                    {
                        totalSales += Convert.ToDecimal(row["TotalSales"]);
                        totalTax += Convert.ToDecimal(row["TotalTax"]);
                        totalOrders += Convert.ToInt32(row["TotalOrders"]);
                    }

                    decimal avgOrder = totalOrders > 0 ? totalSales / totalOrders : 0;

                    ltrTotalOrders.Text = totalOrders.ToString();
                    ltrTotalSales.Text = totalSales.ToString("N2");
                    ltrAvgOrder.Text = avgOrder.ToString("N2");
                    ltrTotalTax.Text = totalTax.ToString("N2");
                    ltrRecordCount.Text = reportData.Rows.Count.ToString();

                    auditLogger.LogAction("ViewReport", "Reports", "SalesReport", null,
                        $"BranchID: {branchID}, DateFrom: {dateFrom}, DateTo: {dateTo}");
                }
                else
                {
                    divSummary.Visible = false;
                    divReport.Visible = true;
                    gvReport.DataSource = null;
                    gvReport.DataBind();
                    ShowErrorMessage("No data found for the selected period.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "SalesReport", null, null, ex.Message);
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
                    reportHelper.ExportToExcel(reportData, $"SalesReport_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", "Reports", "SalesReport", null, "Exported to Excel");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error exporting to Excel: {ex.Message}");
            }
        }

        /// <summary>
        /// Exports report to PDF
        /// </summary>
        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvReport.Rows.Count == 0)
                {
                    ShowErrorMessage("No data to export. Please generate the report first.");
                    return;
                }

                // For PDF export, we'll use a simplified approach - redirect to a print-friendly version
                // In a real implementation, you would use a PDF library like iTextSharp
                ShowSuccessMessage("PDF export functionality will be implemented with a PDF library.");

                auditLogger.LogAction("Export", "Reports", "SalesReport", null, "Exported to PDF");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error exporting to PDF: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the report data from the grid view
        /// </summary>
        private DataTable GetReportData()
        {
            // In a real implementation, you would regenerate the data
            // For now, we'll create a simple DataTable from the grid
            DataTable dt = new DataTable();

            // Add columns based on grid columns
            foreach (DataControlField column in gvReport.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }

            // Add rows
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