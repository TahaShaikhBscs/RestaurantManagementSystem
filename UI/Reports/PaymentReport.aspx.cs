// ============================================
// UI/Reports/PaymentReport.aspx.cs
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
    /// Payment Report Page - Displays payment analytics by method
    /// </summary>
    public partial class PaymentReport : Page
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
        /// Generates the payment report
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
                DataTable reportData = reportBAL.GetPaymentReport(branchID.Value, dateFrom, dateTo);

                // Apply payment method filter
                if (!string.IsNullOrEmpty(ddlPaymentMethod.SelectedValue))
                {
                    DataTable filteredData = reportData.Clone();
                    string methodFilter = ddlPaymentMethod.SelectedValue;

                    foreach (DataRow row in reportData.Rows)
                    {
                        if (row["PaymentMethod"].ToString() == methodFilter)
                        {
                            filteredData.ImportRow(row);
                        }
                    }
                    reportData = filteredData;
                }

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    divSummary.Visible = true;
                    divReport.Visible = true;

                    gvReport.DataSource = reportData;
                    gvReport.DataBind();
                    ltrRecordCount.Text = reportData.Rows.Count.ToString();

                    // Calculate summary
                    decimal totalAmount = 0;
                    int totalTransactions = 0;
                    decimal avgAmount = 0;
                    string topMethod = "";
                    decimal maxMethodAmount = 0;

                    foreach (DataRow row in reportData.Rows)
                    {
                        totalAmount += Convert.ToDecimal(row["TotalAmount"]);
                        totalTransactions += Convert.ToInt32(row["TransactionCount"]);
                        decimal methodAmount = Convert.ToDecimal(row["TotalAmount"]);
                        if (methodAmount > maxMethodAmount)
                        {
                            maxMethodAmount = methodAmount;
                            topMethod = row["PaymentMethod"].ToString();
                        }
                    }

                    avgAmount = totalTransactions > 0 ? totalAmount / totalTransactions : 0;

                    ltrTotalAmount.Text = totalAmount.ToString("N2");
                    ltrTotalTransactions.Text = totalTransactions.ToString();
                    ltrAverageAmount.Text = avgAmount.ToString("N2");
                    ltrTopMethod.Text = topMethod;

                    auditLogger.LogAction("ViewReport", "Reports", "PaymentReport", null,
                        $"BranchID: {branchID}, DateFrom: {dateFrom}, DateTo: {dateTo}");
                }
                else
                {
                    divSummary.Visible = false;
                    divReport.Visible = true;
                    gvReport.DataSource = null;
                    gvReport.DataBind();
                    ShowErrorMessage("No payment data found for the selected period.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "PaymentReport", null, null, ex.Message);
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
                    reportHelper.ExportToExcel(reportData, $"PaymentReport_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", "Reports", "PaymentReport", null, "Exported to Excel");
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