// ============================================
// UI/Suppliers/SupplierList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using SupplierModel = RestaurantManagementSystem.Models.Supplier;

namespace RestaurantManagementSystem.UI.Suppliers
{
    /// <summary>
    /// Supplier List Page - Displays all suppliers with management options
    /// </summary>
    public partial class SupplierList : Page
    {
        private SupplierBAL supplierBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads suppliers
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            supplierBAL = new SupplierBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadSuppliers();
            }
        }

        /// <summary>
        /// Loads suppliers into the grid view with statistics
        /// </summary>
        private void LoadSuppliers()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                bool includeInactive = true;
                List<SupplierModel> suppliers = supplierBAL.GetSuppliersByBranch(branchID.Value, includeInactive);

                // Apply status filter
                if (ddlStatusFilter.SelectedValue == "Active")
                {
                    suppliers = suppliers.FindAll(s => s.Status == true);
                }
                else if (ddlStatusFilter.SelectedValue == "Inactive")
                {
                    suppliers = suppliers.FindAll(s => s.Status == false);
                }

                gvSuppliers.DataSource = suppliers;
                gvSuppliers.DataBind();

                // Update statistics
                UpdateStatistics(suppliers);
                ltrTotalCount.Text = suppliers.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading suppliers: {ex.Message}");
                auditLogger.LogAction("Error", "SupplierManagement", "Suppliers", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates supplier statistics
        /// </summary>
        private void UpdateStatistics(List<SupplierModel> suppliers)
        {
            int total = suppliers.Count;
            int active = 0;
            int inactive = 0;

            foreach (var supplier in suppliers)
            {
                if (supplier.Status)
                    active++;
                else
                    inactive++;
            }

            ltrTotalSuppliers.Text = total.ToString();
            ltrActiveSuppliers.Text = active.ToString();
            ltrInactiveSuppliers.Text = inactive.ToString();
        }

        /// <summary>
        /// Handles status filter change
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        /// <summary>
        /// Searches suppliers
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
                    LoadSuppliers();
                }
                else
                {
                    List<SupplierModel> suppliers = supplierBAL.SearchSuppliers(branchID.Value, searchTerm);
                    gvSuppliers.DataSource = suppliers;
                    gvSuppliers.DataBind();
                    UpdateStatistics(suppliers);
                    ltrTotalCount.Text = suppliers.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error searching suppliers: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears search and reloads all suppliers
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlStatusFilter.SelectedValue = "";
            LoadSuppliers();
        }

        /// <summary>
        /// Redirects to add new supplier page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("SupplierAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int supplierID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditSupplier")
                {
                    Response.Redirect($"SupplierAddEdit.aspx?SupplierID={supplierID}");
                }
                else if (e.CommandName == "DeleteSupplier")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool deleted = supplierBAL.DeleteSupplier(supplierID, userID.Value);
                    if (deleted)
                    {
                        auditLogger.LogDelete("SupplierManagement", "Suppliers", supplierID, $"SupplierID: {supplierID}");
                        ShowSuccessMessage("Supplier deleted successfully.");
                        LoadSuppliers();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to delete supplier. It may have purchase orders.");
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
        protected void gvSuppliers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSuppliers.PageIndex = e.NewPageIndex;
            LoadSuppliers();
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