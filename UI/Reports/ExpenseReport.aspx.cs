// ============================================
// UI/Reports/ExpenseReport.aspx.cs
// ============================================

using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;
using System.Web.UI.WebControls;

namespace RestaurantManagementSystem.UI.Reports
{
    /// <summary>
    /// Expense Report Page - Displays expense reports with filtering and export
    /// </summary>
    public partial class ExpenseReport : Page
    {
        private ReportBAL reportBAL;
        private ExpenseBAL expenseBAL;
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
            expenseBAL = new ExpenseBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime lastDay = DateTime.Now;

                txtDateFrom.Text = firstDay.ToString("yyyy-MM-dd");
                txtDateTo.Text = lastDay.ToString("yyyy-MM-dd");
                LoadCategories();
            }
        }

        /// <summary>
        /// Loads expense categories into filter dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<string> categories = expenseBAL.GetExpenseCategories(branchID.Value);
                    ddlCategory.DataSource = categories;
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("All Categories", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates the expense report
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

                string category = ddlCategory.SelectedValue;
                if (string.IsNullOrEmpty(category))
                {
                    category = null;
                }

                // Get report data
                DataTable reportData = reportBAL.GetExpenseReport(branchID.Value, dateFrom, dateTo, category);

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    divSummary.Visible = true;
                    divReport.Visible = true;

                    gvReport.DataSource = reportData;
                    gvReport.DataBind();
                    ltrRecordCount.Text = reportData.Rows.Count.ToString();

                    // Calculate summary
                    decimal totalAmount = 0;
                    decimal avgAmount = 0;
                    int totalTransactions = 0;

                    foreach (DataRow row in reportData.Rows)
                    {
                        totalAmount += Convert.ToDecimal(row["TotalAmount"]);
                        totalTransactions += Convert.ToInt32(row["TransactionCount"]);
                    }

                    avgAmount = totalTransactions > 0 ? totalAmount / totalTransactions : 0;

                    ltrTotalAmount.Text = totalAmount.ToString("N2");
                    ltrAverageAmount.Text = avgAmount.ToString("N2");
                    ltrTotalTransactions.Text = totalTransactions.ToString();
                    ltrCategoryCount.Text = reportData.Rows.Count.ToString();

                    auditLogger.LogAction("ViewReport", "Reports", "ExpenseReport", null,
                        $"BranchID: {branchID}, DateFrom: {dateFrom}, DateTo: {dateTo}");
                }
                else
                {
                    divSummary.Visible = false;
                    divReport.Visible = true;
                    gvReport.DataSource = null;
                    gvReport.DataBind();
                    ShowErrorMessage("No expense data found for the selected period.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "ExpenseReport", null, null, ex.Message);
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
                    reportHelper.ExportToExcel(reportData, $"ExpenseReport_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", "Reports", "ExpenseReport", null, "Exported to Excel");
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