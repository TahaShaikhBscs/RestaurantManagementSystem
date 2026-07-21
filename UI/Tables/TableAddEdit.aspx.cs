
using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using TableModel = RestaurantManagementSystem.Models.RestaurantTable;

namespace RestaurantManagementSystem.UI.Tables
{
    /// <summary>
    /// Table Add/Edit Page - Handles adding and editing restaurant tables
    /// </summary>
    public partial class TableAddEdit : Page
    {
        private TableBAL tableBAL;
        private AuditLogger auditLogger;
        private int tableID = 0;

        /// <summary>
        /// Page load event - loads table data for editing
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

            if (Request.QueryString["TableID"] != null)
            {
                tableID = Convert.ToInt32(Request.QueryString["TableID"]);
            }

            if (!IsPostBack)
            {
                if (tableID > 0)
                {
                    ltrPageTitle.Text = "Edit Table";
                    ltrBreadcrumb.Text = "Edit";
                    ltrFormTitle.Text = "Edit Table Information";
                    btnSave.Text = "Update Table";
                    LoadTableData(tableID);
                }
                else
                {
                    ltrPageTitle.Text = "Add New Table";
                    ltrBreadcrumb.Text = "Add New";
                    ltrFormTitle.Text = "Add New Table";
                    btnSave.Text = "Save Table";
                    chkIsActive.Checked = true;
                    txtCapacity.Text = "4";
                    ddlStatus.SelectedValue = "Available";
                }
            }
        }

        /// <summary>
        /// Loads table data for editing
        /// </summary>
        private void LoadTableData(int id)
        {
            try
            {
                TableModel table = tableBAL.GetTableById(id);
                if (table != null)
                {
                    txtTableNumber.Text = table.TableNumber;
                    txtFloor.Text = table.Floor;
                    txtCapacity.Text = table.Capacity.ToString();
                    ddlStatus.SelectedValue = table.Status;
                    chkIsActive.Checked = table.IsActive;
                }
                else
                {
                    ShowErrorMessage("Table not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading table: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the table
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                int? companyID = SessionHelper.GetCompanyId();
                int? branchID = SessionHelper.GetBranchId();

                if (!companyID.HasValue || !branchID.HasValue)
                {
                    ShowErrorMessage("Company or branch not found in session.");
                    return;
                }

                TableModel table = new TableModel
                {
                    TableID = tableID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    TableNumber = txtTableNumber.Text.Trim(),
                    Floor = txtFloor.Text.Trim(),
                    Capacity = Convert.ToInt32(txtCapacity.Text),
                    Status = ddlStatus.SelectedValue,
                    IsActive = chkIsActive.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = tableBAL.SaveTable(table);

                if (tableID > 0)
                {
                    auditLogger.LogUpdate("TableManagement", "Tables", savedID,
                        $"TableID: {tableID}", $"TableNumber: {table.TableNumber}");
                    ShowSuccessMessage("Table updated successfully.");
                }
                else
                {
                    auditLogger.LogInsert("TableManagement", "Tables", savedID,
                        $"TableNumber: {table.TableNumber}");
                    ShowSuccessMessage("Table added successfully.");
                }

                Response.AddHeader("REFRESH", "2;URL=TableList.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving table: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("TableList.aspx");
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