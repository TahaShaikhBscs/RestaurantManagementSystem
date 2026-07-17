// ============================================
// UI/Deals/DealsList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Deals
{
    /// <summary>
    /// Deals List Page - Displays all deals with management options
    /// </summary>
    public partial class DealsList : Page
    {
        #region Private Fields

        private DealBAL dealBAL;
        private AuditLogger auditLogger;
        private const string MODULE_NAME = "DealManagement";

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - loads deals
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            if (!IsPostBack)
            {
                LoadDeals();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            dealBAL = new DealBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads deals into the grid view with statistics
        /// </summary>
        private void LoadDeals()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                string searchTerm = txtSearch.Text.Trim();
                List<Deal> deals;

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    deals = dealBAL.GetAllDeals(branchID.Value, true);
                }
                else
                {
                    deals = dealBAL.SearchDeals(branchID.Value, searchTerm);
                }

                // Apply status filter
                deals = ApplyStatusFilter(deals);

                gvDeals.DataSource = deals;
                gvDeals.DataBind();

                // Update statistics
                UpdateStatistics(deals);
                ltrTotalCount.Text = deals.Count.ToString();

                auditLogger.LogAction("View", MODULE_NAME, "Deals", null,
                    $"BranchID: {branchID}, TotalItems: {deals.Count}");
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadDeals", null, null, ex.Message);
                ShowErrorMessage($"Error loading deals: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies status filter to the deals list
        /// </summary>
        private List<Deal> ApplyStatusFilter(List<Deal> deals)
        {
            string filter = ddlStatusFilter.SelectedValue;

            if (string.IsNullOrEmpty(filter))
                return deals;

            switch (filter)
            {
                case "Active":
                    return deals.FindAll(d => d.Status);
                case "Inactive":
                    return deals.FindAll(d => !d.Status);
                case "Valid":
                    return deals.FindAll(d => d.IsValid);
                case "Expired":
                    return deals.FindAll(d => !d.IsValid);
                default:
                    return deals;
            }
        }

        /// <summary>
        /// Updates statistics cards
        /// </summary>
        private void UpdateStatistics(List<Deal> deals)
        {
            int total = deals.Count;
            int active = 0;
            int valid = 0;
            int expired = 0;

            foreach (var deal in deals)
            {
                if (deal.Status) active++;
                if (deal.IsValid) valid++;
                else expired++;
            }

            ltrTotalDeals.Text = total.ToString();
            ltrActiveDeals.Text = active.ToString();
            ltrValidDeals.Text = valid.ToString();
            ltrExpiredDeals.Text = expired.ToString();
        }

        #endregion

        #region Grid View Events

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvDeals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int dealID = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditDeal":
                        Response.Redirect($"DealsAddEdit.aspx?DealID={dealID}");
                        break;

                    case "ToggleStatus":
                        ToggleDealStatus(dealID);
                        break;

                    case "DeleteDeal":
                        DeleteDeal(dealID);
                        break;
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "RowCommand", null, null, ex.Message);
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles page index changing
        /// </summary>
        protected void gvDeals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDeals.PageIndex = e.NewPageIndex;
            LoadDeals();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Handles search button click
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDeals();
            auditLogger.LogSearch(MODULE_NAME, txtSearch.Text.Trim());
        }

        /// <summary>
        /// Handles clear button click
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlStatusFilter.SelectedValue = "";
            LoadDeals();
            ShowSuccessMessage("Filters cleared.");
        }

        /// <summary>
        /// Handles refresh button click
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDeals();
            ShowSuccessMessage("Deals refreshed.");
        }

        /// <summary>
        /// Handles add new button click
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("DealsAddEdit.aspx");
        }

        /// <summary>
        /// Handles status filter change
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDeals();
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Toggles the status of a deal
        /// </summary>
        private void ToggleDealStatus(int dealID)
        {
            int? userID = SessionHelper.GetUserId();
            if (!userID.HasValue)
            {
                ShowErrorMessage("User session expired. Please login again.");
                return;
            }

            Deal deal = dealBAL.GetDealById(dealID);
            if (deal != null)
            {
                deal.Status = !deal.Status;
                deal.UpdatedBy = userID.Value;

                // Update deal
                List<DealItem> items = dealBAL.GetDealItems(dealID);
                int savedID = dealBAL.SaveDeal(deal, items);

                if (savedID > 0)
                {
                    auditLogger.LogUpdate(MODULE_NAME, "Deals", dealID,
                        $"Status: {!deal.Status}", $"Status: {deal.Status}");
                    ShowSuccessMessage($"Deal status toggled to {(deal.Status ? "Active" : "Inactive")}.");
                    LoadDeals();
                }
                else
                {
                    ShowErrorMessage("Failed to toggle deal status.");
                }
            }
        }

        /// <summary>
        /// Deletes a deal
        /// </summary>
        private void DeleteDeal(int dealID)
        {
            int? userID = SessionHelper.GetUserId();
            if (!userID.HasValue)
            {
                ShowErrorMessage("User session expired. Please login again.");
                return;
            }

            bool deleted = dealBAL.DeleteDeal(dealID, userID.Value);
            if (deleted)
            {
                auditLogger.LogDelete(MODULE_NAME, "Deals", dealID, $"DealID: {dealID}");
                ShowSuccessMessage("Deal deleted successfully.");
                LoadDeals();
            }
            else
            {
                ShowErrorMessage("Failed to delete deal.");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets status badge HTML
        /// </summary>
        protected string GetStatusBadge(object statusObj, object startDateObj, object endDateObj)
        {
            bool status = false;

            if (statusObj != DBNull.Value && statusObj != null)
                status = Convert.ToBoolean(statusObj);

            if (!status)
                return "<span class='badge bg-danger'>Inactive</span>";

            DateTime today = DateTime.Today;

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (startDateObj != DBNull.Value && startDateObj != null)
                startDate = Convert.ToDateTime(startDateObj);

            if (endDateObj != DBNull.Value && endDateObj != null)
                endDate = Convert.ToDateTime(endDateObj);

            if (startDate.HasValue && today < startDate.Value.Date)
                return "<span class='badge bg-warning text-dark'>Upcoming</span>";

            if (endDate.HasValue && today > endDate.Value.Date)
                return "<span class='badge bg-secondary'>Expired</span>";

            return "<span class='badge bg-success'>Active</span>";
        }
        //protected string GetStatusBadge(object status, object startDate, object endDate)
        //{
        //    bool isActive = Convert.ToBoolean(status);
        //    bool isValid = true;

        //    if (startDate != DBNull.Value && Convert.ToDateTime(startDate) > DateTime.Now)
        //        isValid = false;

        //    if (endDate != DBNull.Value && Convert.ToDateTime(endDate) < DateTime.Now)
        //        isValid = false;

        //    if (!isActive)
        //        return "<span class='badge bg-secondary px-3 py-2'>Inactive</span>";

        //    if (isValid)
        //        return "<span class='badge bg-success px-3 py-2'>Active</span>";
        //    else
        //        return "<span class='badge bg-danger px-3 py-2'>Expired</span>";
        //}

        /// <summary>
        /// Shows a success message
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            ltrSuccessMessage.Text = message;
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            ltrErrorMessage.Text = message;
        }

        #endregion
    }
}