// ============================================
// UI/Expenses/ExpenseList.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using ExpenseModel = RestaurantManagementSystem.Models.Expense;

namespace RestaurantManagementSystem.UI.Expenses
{
    /// <summary>
    /// Expense List Page - Displays all expenses with management options
    /// </summary>
    public partial class ExpenseList : Page
    {
        private ExpenseBAL expenseBAL;
        private AuditLogger auditLogger;

        /// <summary>
        /// Page load event - loads expenses
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            expenseBAL = new ExpenseBAL();
            auditLogger = new AuditLogger();

            if (!IsPostBack)
            {
                LoadCategories();
                LoadExpenses();
            }
        }

        /// <summary>
        /// Loads expense categories into the filter dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (branchID.HasValue)
                {
                    List<string> categories = expenseBAL.GetExpenseCategories(branchID.Value);
                    ddlCategoryFilter.DataSource = categories;
                    ddlCategoryFilter.DataBind();
                    ddlCategoryFilter.Items.Insert(0, new ListItem("All Categories", ""));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads expenses into the grid view with statistics
        /// </summary>
        private void LoadExpenses()
        {
            try
            {
                int? branchID = SessionHelper.GetBranchId();
                if (!branchID.HasValue)
                {
                    ShowErrorMessage("Branch not found in session.");
                    return;
                }

                DateTime? dateFrom = null;
                DateTime? dateTo = null;

                if (!string.IsNullOrEmpty(txtDateFrom.Text))
                {
                    dateFrom = Convert.ToDateTime(txtDateFrom.Text);
                    dateTo = DateTime.Now;
                }

                string category = ddlCategoryFilter.SelectedValue;
                if (string.IsNullOrEmpty(category))
                {
                    category = null;
                }

                List<ExpenseModel> expenses = expenseBAL.GetExpensesByBranch(
                    branchID.Value, dateFrom, dateTo, category);

                // Apply search filter
                if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                {
                    string searchTerm = txtSearch.Text.Trim().ToLower();
                    expenses = expenses.FindAll(e =>
                        e.Description.ToLower().Contains(searchTerm) ||
                        e.ExpenseCategory.ToLower().Contains(searchTerm) ||
                        e.ReceiptNumber.ToLower().Contains(searchTerm));
                }

                gvExpenses.DataSource = expenses;
                gvExpenses.DataBind();

                // Update statistics
                UpdateStatistics(expenses);
                ltrTotalCount.Text = expenses.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading expenses: {ex.Message}");
                auditLogger.LogAction("Error", "ExpenseManagement", "Expenses", null, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates expense statistics
        /// </summary>
        private void UpdateStatistics(List<ExpenseModel> expenses)
        {
            int total = expenses.Count;
            decimal totalAmount = 0;
            decimal monthAmount = 0;

            foreach (var expense in expenses)
            {
                totalAmount += expense.Amount;

                // Check if expense is from current month
                if (expense.ExpenseDate.Month == DateTime.Now.Month &&
                    expense.ExpenseDate.Year == DateTime.Now.Year)
                {
                    monthAmount += expense.Amount;
                }
            }

            decimal average = total > 0 ? totalAmount / total : 0;

            ltrTotalExpenses.Text = total.ToString();
            ltrTotalAmount.Text = totalAmount.ToString("N2");
            ltrAverageAmount.Text = average.ToString("N2");
            ltrMonthAmount.Text = monthAmount.ToString("N2");
        }

        /// <summary>
        /// Handles category filter change
        /// </summary>
        protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExpenses();
        }

        /// <summary>
        /// Searches expenses
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadExpenses();
        }

        /// <summary>
        /// Clears search and reloads all expenses
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCategoryFilter.SelectedValue = "";
            txtDateFrom.Text = string.Empty;
            LoadExpenses();
        }

        /// <summary>
        /// Redirects to add new expense page
        /// </summary>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExpenseAddEdit.aspx");
        }

        /// <summary>
        /// Handles grid view row commands
        /// </summary>
        protected void gvExpenses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int expenseID = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditExpense")
                {
                    Response.Redirect($"ExpenseAddEdit.aspx?ExpenseID={expenseID}");
                }
                else if (e.CommandName == "DeleteExpense")
                {
                    int? userID = SessionHelper.GetUserId();
                    if (!userID.HasValue)
                    {
                        ShowErrorMessage("User session expired. Please login again.");
                        return;
                    }

                    bool deleted = expenseBAL.DeleteExpense(expenseID, userID.Value);
                    if (deleted)
                    {
                        auditLogger.LogDelete("ExpenseManagement", "Expenses", expenseID, $"ExpenseID: {expenseID}");
                        ShowSuccessMessage("Expense deleted successfully.");
                        LoadExpenses();
                    }
                    else
                    {
                        ShowErrorMessage("Failed to delete expense.");
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
        protected void gvExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvExpenses.PageIndex = e.NewPageIndex;
            LoadExpenses();
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