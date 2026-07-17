// ============================================
// UI/Reports/CustomerReport.aspx.cs
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
    /// Customer Report Page - Displays customer analytics and loyalty data
    /// </summary>
    public partial class CustomerReport : Page
    {
        private ReportBAL reportBAL;
        private CustomerBAL customerBAL;
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
            customerBAL = new CustomerBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime lastDay = DateTime.Now;

                txtDateFrom.Text = firstDay.ToString("yyyy-MM-dd");
                txtDateTo.Text = lastDay.ToString("yyyy-MM-dd");
                txtMinPoints.Text = "0";
            }
        }

        /// <summary>
        /// Generates the customer report
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

                int minPoints = Convert.ToInt32(txtMinPoints.Text);

                // Get report data
                DataTable reportData = reportBAL.GetCustomerSummaryReport(branchID.Value, dateFrom, dateTo, 50);

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    divSummary.Visible = true;
                    divReport.Visible = true;

                    gvReport.DataSource = reportData;
                    gvReport.DataBind();
                    ltrRecordCount.Text = reportData.Rows.Count.ToString();

                    // Calculate summary
                    int totalCustomers = 0;
                    int vipCustomers = 0;
                    int totalPoints = 0;
                    decimal topSpender = 0;

                    foreach (DataRow row in reportData.Rows)
                    {
                        totalCustomers++;
                        if (row["CustomerType"].ToString() == "VIP") vipCustomers++;
                        totalPoints += Convert.ToInt32(row["LoyaltyPoints"]);
                        decimal spent = Convert.ToDecimal(row["TotalSpent"]);
                        if (spent > topSpender) topSpender = spent;
                    }

                    ltrTotalCustomers.Text = totalCustomers.ToString();
                    ltrVIPCustomers.Text = vipCustomers.ToString();
                    ltrTotalPoints.Text = totalPoints.ToString("N0");
                    ltrTopSpender.Text = topSpender.ToString("N2");

                    auditLogger.LogAction("ViewReport", "Reports", "CustomerReport", null,
                        $"BranchID: {branchID}, DateFrom: {dateFrom}, DateTo: {dateTo}");
                }
                else
                {
                    divSummary.Visible = false;
                    divReport.Visible = true;
                    gvReport.DataSource = null;
                    gvReport.DataBind();
                    ShowErrorMessage("No customer data found for the selected period.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "CustomerReport", null, null, ex.Message);
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
                    reportHelper.ExportToExcel(reportData, $"CustomerReport_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", "Reports", "CustomerReport", null, "Exported to Excel");
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