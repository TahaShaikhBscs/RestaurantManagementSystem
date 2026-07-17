// ============================================
// UI/Reports/InventoryReport.aspx.cs
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
    /// Inventory Report Page - Displays current inventory levels and status
    /// </summary>
    public partial class InventoryReport : Page
    {
        private ReportBAL reportBAL;
        private StockBAL stockBAL;
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
            stockBAL = new StockBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        /// <summary>
        /// Loads stock categories into filter dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    // Get distinct categories from stock
                    var stock = stockBAL.GetStockByBranch(branchID.Value);
                    var categories = new List<string>();
                    foreach (var item in stock)
                    {
                        if (!string.IsNullOrEmpty(item.Category) && !categories.Contains(item.Category))
                        {
                            categories.Add(item.Category);
                        }
                    }

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
        /// Generates the inventory report
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

                string category = ddlCategory.SelectedValue;
                if (string.IsNullOrEmpty(category))
                {
                    category = null;
                }

                // Get report data
                DateTime dateFrom = DateTime.Today;
                DateTime dateTo = DateTime.Today;

                if (dateFrom > dateTo)
                {
                    ShowErrorMessage("Start date cannot be after end date.");
                    return;
                }

                // Get report data
                DataTable reportData = reportBAL.GetPaymentReport(branchID.Value, dateFrom, dateTo);

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    // Apply stock status filter
                    if (!string.IsNullOrEmpty(ddlStockStatus.SelectedValue))
                    {
                        DataTable filteredData = reportData.Clone();
                        string statusFilter = ddlStockStatus.SelectedValue;

                        foreach (DataRow row in reportData.Rows)
                        {
                            string status = row["StockStatus"].ToString();
                            if ((statusFilter == "Low" && status == "Low Stock") ||
                                (statusFilter == "Normal" && status == "Normal") ||
                                (statusFilter == "Over" && status == "Over Stocked"))
                            {
                                filteredData.ImportRow(row);
                            }
                        }
                        reportData = filteredData;
                    }

                    if (reportData.Rows.Count > 0)
                    {
                        divSummary.Visible = true;
                        divReport.Visible = true;

                        gvReport.DataSource = reportData;
                        gvReport.DataBind();
                        ltrRecordCount.Text = reportData.Rows.Count.ToString();

                        // Calculate summary
                        int totalItems = 0;
                        int lowStock = 0;
                        int overStocked = 0;
                        decimal totalValue = 0;

                        foreach (DataRow row in reportData.Rows)
                        {
                            totalItems++;
                            string status = row["StockStatus"].ToString();
                            if (status == "Low Stock") lowStock++;
                            if (status == "Over Stocked") overStocked++;
                            totalValue += Convert.ToDecimal(row["TotalCost"]);
                        }

                        ltrTotalItems.Text = totalItems.ToString();
                        ltrLowStock.Text = lowStock.ToString();
                        ltrOverStocked.Text = overStocked.ToString();
                        ltrTotalValue.Text = totalValue.ToString("N2");

                        auditLogger.LogAction("ViewReport", "Reports", "InventoryReport", null,
                            $"BranchID: {branchID}, Category: {category ?? "All"}");
                    }
                    else
                    {
                        ShowErrorMessage("No inventory data found matching the filters.");
                        divSummary.Visible = false;
                        divReport.Visible = false;
                    }
                }
                else
                {
                    divSummary.Visible = false;
                    divReport.Visible = true;
                    gvReport.DataSource = null;
                    gvReport.DataBind();
                    ShowErrorMessage("No inventory data found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error generating report: {ex.Message}");
                auditLogger.LogAction("Error", "Reports", "InventoryReport", null, null, ex.Message);
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
                    reportHelper.ExportToExcel(reportData, $"InventoryReport_{DateTime.Now:yyyyMMdd_HHmmss}");

                    auditLogger.LogAction("Export", "Reports", "InventoryReport", null, "Exported to Excel");
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