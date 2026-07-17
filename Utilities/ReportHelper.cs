// ============================================
// Utilities/ReportHelper.cs
// ============================================

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.DAL;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Report Helper - Provides reporting functionality
    /// Handles report generation, export, and printing
    /// </summary>
    public class ReportHelper
    {
        private readonly ReportDAL reportDAL;

        /// <summary>
        /// Constructor initializes DAL object
        /// </summary>
        public ReportHelper()
        {
            reportDAL = new ReportDAL();
        }

        /// <summary>
        /// Gets sales report data
        /// </summary>
        public DataTable GetSalesReport(int branchID, DateTime dateFrom, DateTime dateTo, string groupBy = "Day")
        {
            return reportDAL.GetSalesReport(branchID, dateFrom, dateTo, groupBy);
        }

        /// <summary>
        /// Gets payment report data
        /// </summary>
        public DataTable GetPaymentReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            return reportDAL.GetPaymentReport(branchID, dateFrom, dateTo);
        }

        /// <summary>
        /// Gets profit/loss report data
        /// </summary>
        public DataTable GetProfitLossReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            return reportDAL.GetProfitLossReport(branchID, dateFrom, dateTo);
        }

        /// <summary>
        /// Gets top selling items
        /// </summary>
        public DataTable GetTopSellingItems(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 10)
        {
            return reportDAL.GetTopSellingItems(branchID, dateFrom, dateTo, topCount);
        }

        /// <summary>
        /// Gets customer sales report
        /// </summary>
        public DataTable GetCustomerSalesReport(int branchID, DateTime dateFrom, DateTime dateTo, int topCount = 20)
        {
            return reportDAL.GetCustomerSalesReport(branchID, dateFrom, dateTo, topCount);
        }

        /// <summary>
        /// Gets inventory report
        /// </summary>
        public DataTable GetInventoryReport(int branchID)
        {
            return reportDAL.GetInventoryReport(branchID);
        }

        /// <summary>
        /// Gets expense summary report
        /// </summary>
        public DataTable GetExpenseSummaryReport(int branchID, DateTime dateFrom, DateTime dateTo)
        {
            return reportDAL.GetExpenseSummaryReport(branchID, dateFrom, dateTo);
        }

        /// <summary>
        /// Exports DataTable to CSV
        /// </summary>
        public string ExportToCSV(DataTable dataTable)
        {
            StringBuilder csv = new StringBuilder();

            // Headers
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                csv.Append(dataTable.Columns[i].ColumnName);
                if (i < dataTable.Columns.Count - 1)
                    csv.Append(",");
            }
            csv.AppendLine();

            // Data
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string value = row[i]?.ToString() ?? string.Empty;
                    // Escape commas and quotes
                    if (value.Contains(",") || value.Contains("\""))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    }
                    csv.Append(value);
                    if (i < dataTable.Columns.Count - 1)
                        csv.Append(",");
                }
                csv.AppendLine();
            }

            return csv.ToString();
        }

        /// <summary>
        /// Exports DataTable to HTML
        /// </summary>
        public string ExportToHTML(DataTable dataTable, string title = null)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<html>");
            html.Append("<head>");
            html.Append("<style>");
            html.Append("table { border-collapse: collapse; width: 100%; font-family: Arial, sans-serif; }");
            html.Append("th { background-color: #4CAF50; color: white; padding: 12px; text-align: left; }");
            html.Append("td { border: 1px solid #ddd; padding: 8px; }");
            html.Append("tr:nth-child(even) { background-color: #f2f2f2; }");
            html.Append("tr:hover { background-color: #ddd; }");
            html.Append(".title { font-size: 20px; font-weight: bold; margin-bottom: 20px; }");
            html.Append("</style>");
            html.Append("</head>");
            html.Append("<body>");

            if (!string.IsNullOrEmpty(title))
            {
                html.Append($"<div class='title'>{title}</div>");
            }

            html.Append("<table>");
            html.Append("<thead><tr>");
            foreach (DataColumn column in dataTable.Columns)
            {
                html.Append($"<th>{column.ColumnName}</th>");
            }
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            foreach (DataRow row in dataTable.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dataTable.Columns)
                {
                    html.Append($"<td>{row[column]}</td>");
                }
                html.Append("</tr>");
            }

            html.Append("</tbody></table>");
            html.Append("</body></html>");

            return html.ToString();
        }

        /// <summary>
        /// Exports DataTable to Excel (via HTML table)
        /// </summary>
        public void ExportToExcel(DataTable dataTable, string fileName)
        {
            string html = ExportToHTML(dataTable);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Exports DataTable to CSV download
        /// </summary>
        public void ExportToCSV(DataTable dataTable, string fileName)
        {
            string csv = ExportToCSV(dataTable);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}.csv");
            HttpContext.Current.Response.Write(csv);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}