// ============================================
// Utilities/PrintHelper.cs - Production-Ready Refactored
// ============================================

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Print Helper - Centralized printing utility for the Restaurant Management System
    /// Supports Thermal (58mm & 80mm) and A4 printing formats
    /// Handles Invoices, Receipts, Kitchen Order Tickets (KOT), and Reports
    /// </summary>
    public class PrintHelper
    {
        #region Private Constants

        // Thermal printer widths
        private const int THERMAL_58MM_WIDTH = 32;
        private const int THERMAL_80MM_WIDTH = 48;
        private const int A4_WIDTH = 80;

        #endregion

        #region Public Enums

        /// <summary>
        /// Print format types
        /// </summary>
        public enum PrintFormat
        {
            Thermal58mm,
            Thermal80mm,
            A4
        }

        /// <summary>
        /// Document types
        /// </summary>
        public enum DocumentType
        {
            Invoice,
            Receipt,
            KitchenOrderTicket,
            Report
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets the line width for the specified format
        /// </summary>
        private int GetLineWidth(PrintFormat format)
        {
            switch (format)
            {
                case PrintFormat.Thermal58mm:
                    return THERMAL_58MM_WIDTH;
                case PrintFormat.Thermal80mm:
                    return THERMAL_80MM_WIDTH;
                case PrintFormat.A4:
                default:
                    return A4_WIDTH;
            }
        }

        /// <summary>
        /// Gets the CSS styles for the specified format
        /// </summary>
        private string GetStyles(PrintFormat format)
        {
            StringBuilder styles = new StringBuilder();

            switch (format)
            {
                case PrintFormat.Thermal58mm:
                case PrintFormat.Thermal80mm:
                    styles.Append(@"
                        body { 
                            font-family: 'Courier New', monospace; 
                            font-size: 10px;
                            padding: 5px; 
                            margin: 0;
                        }
                        .header { text-align: center; margin-bottom: 5px; }
                        .company { font-size: 14px; font-weight: bold; }
                        .title { font-size: 16px; font-weight: bold; text-align: center; margin: 5px 0; }
                        .divider { border-top: 1px dashed #000; margin: 5px 0; }
                        .divider-solid { border-top: 2px solid #000; margin: 5px 0; }
                        table { width: 100%; border-collapse: collapse; margin: 5px 0; }
                        th { border-bottom: 1px solid #000; padding: 3px; text-align: left; }
                        td { padding: 3px; }
                        .text-right { text-align: right; }
                        .text-center { text-align: center; }
                        .fw-bold { font-weight: bold; }
                        .total { font-weight: bold; border-top: 2px solid #000; }
                        .footer { text-align: center; margin-top: 10px; }
                        .thank-you { font-weight: bold; margin: 5px 0; }
                        .small { font-size: 8px; }
                    ");
                    break;

                case PrintFormat.A4:
                default:
                    styles.Append(@"
                        body { 
                            font-family: 'Segoe UI', Arial, sans-serif; 
                            font-size: 12px;
                            padding: 20px; 
                            margin: 0;
                            max-width: 210mm;
                            margin: 0 auto;
                        }
                        .header { text-align: center; margin-bottom: 10px; }
                        .company { font-size: 24px; font-weight: bold; }
                        .company-details { font-size: 12px; margin: 2px 0; }
                        .title { font-size: 20px; font-weight: bold; text-align: center; margin: 10px 0; }
                        .divider { border-top: 1px solid #ddd; margin: 10px 0; }
                        .divider-solid { border-top: 2px solid #000; margin: 10px 0; }
                        table { width: 100%; border-collapse: collapse; margin: 10px 0; }
                        th { background-color: #f8f9fa; border-bottom: 2px solid #000; padding: 8px; text-align: left; }
                        td { border-bottom: 1px solid #eee; padding: 8px; }
                        .text-right { text-align: right; }
                        .text-center { text-align: center; }
                        .fw-bold { font-weight: bold; }
                        .total { font-weight: bold; border-top: 2px solid #000; }
                        .footer { text-align: center; margin-top: 20px; padding-top: 10px; border-top: 2px solid #000; }
                        .thank-you { font-size: 18px; font-weight: bold; margin: 10px 0; }
                        .small { font-size: 10px; color: #6c757d; }
                        .info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 5px; margin: 10px 0; }
                        .info-item { padding: 5px; }
                        .info-label { font-weight: bold; }
                    ");
                    break;
            }

            // Print-specific styles
            styles.Append(@"
                @media print {
                    .no-print { display: none !important; }
                    body { margin: 0; padding: 0; }
                    .page-break { page-break-after: always; }
                }
                .print-only { display: none; }
                @media print {
                    .print-only { display: block !important; }
                }
            ");

            return styles.ToString();
        }

        /// <summary>
        /// Generates a divider line
        /// </summary>
        private string GenerateDivider(PrintFormat format, string type = "dashed")
        {
            int width = GetLineWidth(format);
            string line = new string('-', width);

            if (type == "solid")
                line = new string('=', width);

            return $"<div class='divider-{type}'>{line}</div>";
        }

        /// <summary>
        /// Formats currency for display
        /// </summary>
        private string FormatCurrency(decimal amount, string currencySymbol = "$")
        {
            return $"{currencySymbol}{amount:F2}";
        }

        /// <summary>
        /// Centers text within the line width
        /// </summary>
        private string CenterText(string text, PrintFormat format)
        {
            int width = GetLineWidth(format);
            int padding = (width - text.Length) / 2;
            if (padding < 0) padding = 0;
            return new string(' ', padding) + text;
        }

        #endregion

        #region Document Generation Methods

        /// <summary>
        /// Generates a complete printable document
        /// </summary>
        /// <param name="order">Order object</param>
        /// <param name="company">Company object</param>
        /// <param name="branch">Branch object</param>
        /// <param name="documentType">Type of document to generate</param>
        /// <param name="format">Print format</param>
        /// <returns>Complete HTML document for printing</returns>
        public string GenerateDocument(Order order, Company company, Branch branch,
            DocumentType documentType = DocumentType.Invoice, PrintFormat format = PrintFormat.A4)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            StringBuilder html = new StringBuilder();

            // Build HTML document
            html.Append("<html>");
            html.Append("<head>");
            html.Append("<meta charset='UTF-8'>");
            html.Append($"<title>{documentType}</title>");
            html.Append($"<style>{GetStyles(format)}</style>");
            html.Append("</head>");
            html.Append("<body>");

            // Generate document content based on type
            switch (documentType)
            {
                case DocumentType.Invoice:
                    html.Append(GenerateInvoiceContent(order, company, branch, format));
                    break;
                case DocumentType.Receipt:
                    html.Append(GenerateReceiptContent(order, company, format));
                    break;
                case DocumentType.KitchenOrderTicket:
                    html.Append(GenerateKOTContent(order, format));
                    break;
                default:
                    html.Append(GenerateInvoiceContent(order, company, branch, format));
                    break;
            }

            html.Append("</body></html>");

            return html.ToString();
        }

        /// <summary>
        /// Generates invoice content
        /// </summary>
        private string GenerateInvoiceContent(Order order, Company company, Branch branch, PrintFormat format)
        {
            StringBuilder content = new StringBuilder();

            // Header
            content.Append("<div class='header'>");
            content.Append($"<div class='company'>{company.CompanyName}</div>");
            if (format == PrintFormat.A4)
            {
                content.Append($"<div class='company-details'>{company.Address}</div>");
                content.Append($"<div class='company-details'>Phone: {company.Phone} | Email: {company.Email}</div>");
                content.Append($"<div class='company-details'>GST: {company.GST} | NTN: {company.NTN}</div>");
                content.Append($"<div class='company-details'>Branch: {branch?.BranchName}</div>");
            }
            else
            {
                content.Append($"<div>{company.Address}</div>");
                content.Append($"<div>Ph: {company.Phone}</div>");
            }
            content.Append("</div>");

            content.Append(GenerateDivider(format, "solid"));

            // Title
            content.Append($"<div class='title'>INVOICE</div>");

            // Order Details
            content.Append("<div class='info-grid'>");
            content.Append($"<div class='info-item'><span class='info-label'>Invoice #:</span> {order.OrderNumber}</div>");
            content.Append($"<div class='info-item'><span class='info-label'>Date:</span> {order.OrderDate:dd/MM/yyyy HH:mm}</div>");
            content.Append($"<div class='info-item'><span class='info-label'>Order Type:</span> {order.OrderType}</div>");
            content.Append($"<div class='info-item'><span class='info-label'>Table:</span> {order.TableNumber ?? "N/A"}</div>");
            content.Append($"<div class='info-item'><span class='info-label'>Customer:</span> {order.CustomerName ?? "Walk-in"}</div>");
            if (!string.IsNullOrEmpty(order.DeliveryAddress))
            {
                content.Append($"<div class='info-item'><span class='info-label'>Delivery Address:</span> {order.DeliveryAddress}</div>");
            }
            content.Append("</div>");

            content.Append(GenerateDivider(format));

            // Items Table
            content.Append(GenerateItemsTable(order.OrderItems, format));

            // Totals
            content.Append(GenerateTotals(order, format));

            // Payments
            if (order.Payments != null && order.Payments.Count > 0)
            {
                content.Append(GenerateDivider(format));
                content.Append(GeneratePayments(order.Payments, format));
            }

            // Footer
            content.Append(GenerateDivider(format, "solid"));
            content.Append("<div class='footer'>");
            content.Append("<div class='thank-you'>Thank you for your business!</div>");
            content.Append($"<div class='small'>Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}</div>");
            if (format == PrintFormat.A4)
            {
                content.Append("<div class='small'>This is a computer-generated invoice. No signature required.</div>");
            }
            content.Append("</div>");

            return content.ToString();
        }

        /// <summary>
        /// Generates receipt content (thermal printer optimized)
        /// </summary>
        private string GenerateReceiptContent(Order order, Company company, PrintFormat format)
        {
            StringBuilder content = new StringBuilder();

            // Header - Compact for thermal
            content.Append("<div class='header'>");
            content.Append($"<div class='company'>{company.CompanyName}</div>");
            content.Append($"<div>{company.Address}</div>");
            content.Append($"<div>{company.Phone}</div>");
            content.Append("</div>");

            content.Append(GenerateDivider(format, "solid"));

            content.Append($"<div>Invoice: {order.OrderNumber}</div>");
            content.Append($"<div>Date: {order.OrderDate:dd/MM/yyyy HH:mm}</div>");
            content.Append($"<div>Type: {order.OrderType}</div>");
            content.Append($"<div>Table: {order.TableNumber ?? "TA"}</div>");
            content.Append($"<div>Customer: {order.CustomerName ?? "Walk-in"}</div>");

            content.Append(GenerateDivider(format));

            // Items - Compact
            content.Append("<table>");
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    string itemLine = $"{item.Quantity}x {item.ItemName}";
                    string priceLine = $"{item.TotalPrice:F2}";

                    // Truncate item name if too long for thermal
                    if (format == PrintFormat.Thermal58mm || format == PrintFormat.Thermal80mm)
                    {
                        int maxLen = format == PrintFormat.Thermal58mm ? 20 : 30;
                        if (itemLine.Length > maxLen)
                            itemLine = itemLine.Substring(0, maxLen) + "...";
                    }

                    content.Append($"<tr><td>{itemLine}</td><td class='text-right'>{priceLine}</td></tr>");
                }
            }
            content.Append("</table>");

            content.Append(GenerateDivider(format));

            // Totals - Compact
            content.Append($"<div>Sub Total: {order.SubTotal:F2}</div>");
            if (order.Discount > 0)
                content.Append($"<div>Discount: {order.Discount:F2}</div>");
            if (order.Tax > 0)
                content.Append($"<div>Tax: {order.Tax:F2}</div>");
            content.Append($"<div class='total'>Total: {order.TotalAmount:F2}</div>");
            if (order.PaidAmount > 0)
            {
                content.Append($"<div>Paid: {order.PaidAmount:F2}</div>");
                if (order.ChangeAmount > 0)
                    content.Append($"<div>Change: {order.ChangeAmount:F2}</div>");
            }

            // Payment Method
            if (order.Payments != null && order.Payments.Count > 0)
            {
                content.Append(GenerateDivider(format));
                content.Append("<div>Payment: ");
                foreach (var payment in order.Payments)
                {
                    content.Append($"{payment.PaymentMethod} ");
                }
                content.Append("</div>");
            }

            content.Append(GenerateDivider(format, "solid"));

            // Footer - Compact
            content.Append("<div class='footer'>");
            content.Append("<div>Thank you!</div>");
            content.Append($"<div class='small'>{DateTime.Now:dd/MM/yyyy HH:mm}</div>");
            content.Append("</div>");

            return content.ToString();
        }

        /// <summary>
        /// Generates Kitchen Order Ticket content
        /// </summary>
        private string GenerateKOTContent(Order order, PrintFormat format)
        {
            StringBuilder content = new StringBuilder();

            content.Append("<div class='header'>");
            content.Append("<div class='title'>KITCHEN ORDER TICKET</div>");
            content.Append("</div>");

            content.Append(GenerateDivider(format, "solid"));

            // Order Information
            content.Append($"<div><strong>Order #:</strong> {order.OrderNumber}</div>");
            content.Append($"<div><strong>Table:</strong> {order.TableNumber ?? "Takeaway"}</div>");
            content.Append($"<div><strong>Type:</strong> {order.OrderType}</div>");
            content.Append($"<div><strong>Time:</strong> {DateTime.Now:HH:mm}</div>");

            if (!string.IsNullOrEmpty(order.SpecialInstructions))
            {
                content.Append($"<div><strong>Notes:</strong> {order.SpecialInstructions}</div>");
            }

            content.Append(GenerateDivider(format));

            // Items
            content.Append("<table>");
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    content.Append($"<tr><td><strong>{item.Quantity}x</strong></td><td>{item.ItemName}</td></tr>");
                    if (!string.IsNullOrEmpty(item.Instructions))
                    {
                        content.Append($"<tr><td colspan='2'><i>** {item.Instructions} **</i></td></tr>");
                    }
                }
            }
            content.Append("</table>");

            content.Append(GenerateDivider(format, "solid"));

            // Footer
            content.Append("<div class='footer'>");
            content.Append("<div>Prepare with care!</div>");
            content.Append($"<div class='small'>{DateTime.Now:dd/MM/yyyy HH:mm}</div>");
            content.Append("</div>");

            return content.ToString();
        }

        /// <summary>
        /// Generates items table
        /// </summary>
        private string GenerateItemsTable(List<OrderItem> items, PrintFormat format)
        {
            if (items == null || items.Count == 0)
                return "<p>No items found.</p>";

            StringBuilder table = new StringBuilder();

            if (format == PrintFormat.A4)
            {
                table.Append("<table>");
                table.Append("<thead><tr>");
                table.Append("<th>Qty</th>");
                table.Append("<th>Item</th>");
                table.Append("<th class='text-right'>Unit Price</th>");
                table.Append("<th class='text-right'>Total</th>");
                table.Append("</tr></thead>");
                table.Append("<tbody>");

                foreach (var item in items)
                {
                    table.Append("<tr>");
                    table.Append($"<td>{item.Quantity}</td>");
                    table.Append($"<td>{item.ItemName}</td>");
                    table.Append($"<td class='text-right'>{item.UnitPrice:F2}</td>");
                    table.Append($"<td class='text-right'>{item.TotalPrice:F2}</td>");
                    table.Append("</tr>");
                }

                table.Append("</tbody></table>");
            }
            else
            {
                // Thermal format - compact
                table.Append("<table>");
                foreach (var item in items)
                {
                    table.Append($"<tr><td>{item.Quantity}x</td><td>{item.ItemName}</td><td class='text-right'>{item.TotalPrice:F2}</td></tr>");
                }
                table.Append("</table>");
            }

            return table.ToString();
        }

        /// <summary>
        /// Generates totals section
        /// </summary>
        private string GenerateTotals(Order order, PrintFormat format)
        {
            StringBuilder totals = new StringBuilder();

            if (format == PrintFormat.A4)
            {
                totals.Append("<div style='text-align: right; margin-top: 10px;'>");
                totals.Append($"<div><strong>Sub Total:</strong> {order.SubTotal:F2}</div>");
                if (order.Discount > 0)
                    totals.Append($"<div><strong>Discount:</strong> ({order.Discount:F2})</div>");
                if (order.Tax > 0)
                    totals.Append($"<div><strong>Tax:</strong> {order.Tax:F2}</div>");
                if (order.ServiceCharge > 0)
                    totals.Append($"<div><strong>Service Charge:</strong> {order.ServiceCharge:F2}</div>");
                totals.Append($"<div style='font-size: 18px; font-weight: bold;'><strong>Total:</strong> {order.TotalAmount:F2}</div>");
                if (order.PaidAmount > 0)
                {
                    totals.Append($"<div><strong>Paid:</strong> {order.PaidAmount:F2}</div>");
                    if (order.ChangeAmount > 0)
                        totals.Append($"<div><strong>Change:</strong> {order.ChangeAmount:F2}</div>");
                }
                totals.Append("</div>");
            }
            else
            {
                // Thermal format - compact
                totals.Append($"<div>Sub Total: {order.SubTotal:F2}</div>");
                if (order.Discount > 0)
                    totals.Append($"<div>Discount: {order.Discount:F2}</div>");
                if (order.Tax > 0)
                    totals.Append($"<div>Tax: {order.Tax:F2}</div>");
                if (order.ServiceCharge > 0)
                    totals.Append($"<div>Service: {order.ServiceCharge:F2}</div>");
                totals.Append($"<div class='total'>Total: {order.TotalAmount:F2}</div>");
                if (order.PaidAmount > 0)
                {
                    totals.Append($"<div>Paid: {order.PaidAmount:F2}</div>");
                    if (order.ChangeAmount > 0)
                        totals.Append($"<div>Change: {order.ChangeAmount:F2}</div>");
                }
            }

            return totals.ToString();
        }

        /// <summary>
        /// Generates payments section
        /// </summary>
        private string GeneratePayments(List<Payment> payments, PrintFormat format)
        {
            StringBuilder paymentHtml = new StringBuilder();

            if (format == PrintFormat.A4)
            {
                paymentHtml.Append("<div><strong>Payment Details:</strong></div>");
                paymentHtml.Append("<table>");
                paymentHtml.Append("<thead><tr><th>Method</th><th>Amount</th><th>Reference</th><th>Date</th></tr></thead>");
                paymentHtml.Append("<tbody>");
                foreach (var payment in payments)
                {
                    paymentHtml.Append("<tr>");
                    paymentHtml.Append($"<td>{payment.PaymentMethod}</td>");
                    paymentHtml.Append($"<td class='text-right'>{payment.Amount:F2}</td>");
                    paymentHtml.Append($"<td>{payment.ReferenceNumber ?? "-"}</td>");
                    paymentHtml.Append($"<td>{payment.TransactionDate:dd/MM/yyyy HH:mm}</td>");
                    paymentHtml.Append("</tr>");
                }
                paymentHtml.Append("</tbody></table>");
            }
            else
            {
                // Thermal format - compact
                foreach (var payment in payments)
                {
                    paymentHtml.Append($"<div>{payment.PaymentMethod}: {payment.Amount:F2}</div>");
                }
            }

            return paymentHtml.ToString();
        }

        #endregion

        #region Printing Methods

        /// <summary>
        /// Prints a document (opens print dialog)
        /// </summary>
        /// <param name="htmlContent">HTML content to print</param>
        /// <param name="documentTitle">Title of the document</param>
        public void PrintDocument(string htmlContent, string documentTitle = "Print Document")
        {
            try
            {
                if (string.IsNullOrEmpty(htmlContent))
                    throw new ArgumentException("HTML content cannot be empty.", nameof(htmlContent));

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "text/html";
                HttpContext.Current.Response.AddHeader("Content-Disposition", $"inline; filename=\"{documentTitle}.html\"");
                HttpContext.Current.Response.Write(htmlContent);
                HttpContext.Current.Response.Write("<script>window.onload = function() { window.print(); };</script>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error printing document: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prints a document with auto-close after printing
        /// </summary>
        /// <param name="htmlContent">HTML content to print</param>
        /// <param name="documentTitle">Title of the document</param>
        public void PrintAndClose(string htmlContent, string documentTitle = "Print Document")
        {
            try
            {
                if (string.IsNullOrEmpty(htmlContent))
                    throw new ArgumentException("HTML content cannot be empty.", nameof(htmlContent));

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "text/html";
                HttpContext.Current.Response.AddHeader("Content-Disposition", $"inline; filename=\"{documentTitle}.html\"");
                HttpContext.Current.Response.Write(htmlContent);
                HttpContext.Current.Response.Write("<script>window.onload = function() { window.print(); window.close(); };</script>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error printing document: {ex.Message}", ex);
            }
        }

        #endregion

        #region Convenience Methods

        /// <summary>
        /// Generates and prints an invoice
        /// </summary>
        public void PrintInvoice(Order order, Company company, Branch branch, PrintFormat format = PrintFormat.A4)
        {
            string html = GenerateDocument(order, company, branch, DocumentType.Invoice, format);
            PrintDocument(html, $"Invoice_{order.OrderNumber}");
        }

        /// <summary>
        /// Generates and prints a receipt
        /// </summary>
        public void PrintReceipt(Order order, Company company, PrintFormat format = PrintFormat.Thermal80mm)
        {
            string html = GenerateDocument(order, company, null, DocumentType.Receipt, format);
            PrintDocument(html, $"Receipt_{order.OrderNumber}");
        }

        /// <summary>
        /// Generates and prints a Kitchen Order Ticket
        /// </summary>
        public void PrintKOT(Order order, PrintFormat format = PrintFormat.Thermal58mm)
        {
            string html = GenerateDocument(order, null, null, DocumentType.KitchenOrderTicket, format);
            PrintDocument(html, $"KOT_{order.OrderNumber}");
        }

        /// <summary>
        /// Generates and prints a report
        /// </summary>
        public void PrintReport(DataTable reportData, string title, PrintFormat format = PrintFormat.A4)
        {
            if (reportData == null || reportData.Rows.Count == 0)
                throw new ArgumentException("No data to print.", nameof(reportData));

            ReportHelper reportHelper = new ReportHelper();
            string html = reportHelper.ExportToHTML(reportData, title);

            // Wrap in full HTML document
            string fullHtml = $@"
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>{title}</title>
                    <style>
                        body {{ font-family: 'Segoe UI', Arial, sans-serif; padding: 20px; }}
                        h1 {{ text-align: center; }}
                        table {{ width: 100%; border-collapse: collapse; }}
                        th {{ background-color: #f8f9fa; border-bottom: 2px solid #000; padding: 8px; text-align: left; }}
                        td {{ border-bottom: 1px solid #eee; padding: 8px; }}
                        .footer {{ text-align: center; margin-top: 20px; border-top: 1px solid #ddd; padding-top: 10px; }}
                    </style>
                </head>
                <body>
                    {html}
                    <div class='footer'>Generated: {DateTime.Now:dd/MM/yyyy HH:mm}</div>
                    <script>window.onload = function() {{ window.print(); }}</script>
                </body>
                </html>
            ";

            PrintDocument(fullHtml, title);
        }

        #endregion
    }
}