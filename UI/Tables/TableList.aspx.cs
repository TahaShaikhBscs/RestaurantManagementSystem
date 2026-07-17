// ============================================
// UI/Tables/TableList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using TableModel = RestaurantManagementSystem.Models.RestaurantTable;

namespace RestaurantManagementSystem.UI.Tables
{
    /// <summary>
    /// Table List Page - Displays all tables with management options
    /// </summary>
    public partial class TableList : Page
    {
        private TableBAL tableBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads tables
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            tableBAL = new TableBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadTables();
            }
        }

        /// <summary>
        /// Loads tables into the grid view with statistics
        /// </summary>
        private void LoadTables()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                string statusFilter = ddlStatusFilter.SelectedValue;
                List<TableModel> tables = tableBAL.GetTablesByBranch(branchID.Value, true);

                // Apply status filter
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    tables = tables.FindAll(t => t.Status == statusFilter);
                }

                gvTables.DataSource = tables;
                gvTables.DataBind();

                // Update statistics
                UpdateStatistics(tables);
                ltrTotalCount.Text = tables.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading tables: {ex.Message}");
                auditLogger.LogAction("Error", "TableManagement", "Tables", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates table statistics
        /// </summary>
        private void UpdateStatistics(List<TableModel> tables)
        {
            int total = tables.Count;
            int available = 0, occupied = 0, reserved = 0, cleaning = 0, inactive = 0;

            foreach (var table in tables)
            {
                if (!table.IsActive)
                {
                    inactive++;
                }
                else
                {
                    switch (table.Status)
                    {
                        case "Available": available++; break;
                        case "Occupied": occupied++; break;
                        case "Reserved": reserved++; break;
                        case "Cleaning": cleaning++; break;
                    }
                }
            }

            ltrTotalTables.Text = total.ToString();
            ltrAvailableTables.Text = available.ToString();
            ltrOccupiedTables.Text = occupied.ToString();
            ltrReservedTables.Text = reserved.ToString();
            ltrCleaningTables.Text = cleaning.ToString();
            ltrInactiveTables.Text = inactive.ToString();
        }

        /// <summary>
        /// Handles status filter change
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTables();
        }

        /// <summary>
        /// Searches tables
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                int? branchID = SessionHelper.GetBranchId();

                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    LoadTables();
                }
                else
                {
                    List<TableModel> tables = tableBAL.SearchTables(branchID.Value, searchTerm);
                    gvTables.DataSource = tables;
                    gvTables.DataBind();
                    UpdateStatistics(tables);
                    ltrTotalCount.Text = tables.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching tables: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears search and reloads all tables
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlStatusFilter.SelectedValue = "";
            LoadTables();
        }

        /// <summary>
        /// Redirects to add new table page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("TableAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvTables_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int tableID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditTable")
                {
                    Response.Redirect($"TableAddEdit.aspx?TableID={tableID}");
                }
                else if (e.CommandName == "ChangeStatus")
                {
                    // In a real implementation, show status change modal
                    Response.Redirect($"TableStatus.aspx?TableID={tableID}");
                }
                else if (e.CommandName == "QRCode")
                {
                    // Generate and display QR code
                    ShowSuccessMessage($"QR Code generated for table {tableID}");
                }
                else if (e.CommandName == "DeleteTable")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool deleted = tableBAL.DeleteTable(tableID, userID.Value);
                    if (deleted)
                    {
                        auditLogger.LogDelete("TableManagement", "Tables", tableID, $"TableID: {tableID}");
                        ShowSuccessMessage("Table deleted successfully.");
                        LoadTables();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to delete table.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles grid view page index changing
        /// </summary>
        protected void gvTables_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTables.PageIndex = e.NewPageIndex;
            LoadTables();
        }

        /// <summary>
        /// Gets Bootstrap badge class for status
        /// </summary>
        protected string GetStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Available": return "success";
                case "Occupied": return "danger";
                case "Reserved": return "warning";
                case "Cleaning": return "info";
                default: return "secondary";
            }
        }

        /// <summary>
        /// Gets Font Awesome icon class for status
        /// </summary>
        protected string GetStatusIconClass(string status)
        {
            switch (status)
            {
                case "Available": return "check-circle";
                case "Occupied": return "user-circle";
                case "Reserved": return "clock";
                case "Cleaning": return "broom";
                default: return "question-circle";
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