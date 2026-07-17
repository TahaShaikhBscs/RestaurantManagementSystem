// ============================================
// UI/Expenses/ExpenseAddEdit.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using ExpenseModel = RestaurantManagementSystem.Models.Expense;

namespace RestaurantManagementSystem.UI.Expenses
{
    /// <summary>
    /// Expense Add/Edit Page - Handles adding and editing expenses
    /// </summary>
    public partial class ExpenseAddEdit : Page
    {
        private ExpenseBAL expenseBAL;
        private AuditLogger auditLogger;
        private int expenseID = 0;

        /// <summary>
        /// Page load event - loads expense data for editing
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

            if (Request.QueryString["ExpenseID"] != null)
            {
                expenseID = Convert.ToInt32(Request.QueryString["ExpenseID"]);
            }

            if (!IsPostBack)
            {
                if (expenseID > 0)
                {
                    ltrPageTitle.Text = "Edit Expense";
                    ltrBreadcrumb.Text = "Edit";
                    ltrFormTitle.Text = "Edit Expense Information";
                    btnSave.Text = "Update Expense";
                    LoadExpenseData(expenseID);
                }
                else
                {
                    ltrPageTitle.Text = "Add New Expense";
                    ltrBreadcrumb.Text = "Add New";
                    ltrFormTitle.Text = "Add New Expense";
                    btnSave.Text = "Save Expense";
                    chkStatus.Checked = true;
                    txtExpenseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    ddlPaymentMethod.SelectedValue = "Cash";
                    ddlRecurringType.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Loads expense data for editing
        /// </summary>
        private void LoadExpenseData(int id)
        {
            try
            {
                ExpenseModel expense = expenseBAL.GetExpenseById(id);
                if (expense != null)
                {
                    ddlCategory.SelectedValue = expense.ExpenseCategory;
                    txtDescription.Text = expense.Description;
                    txtAmount.Text = expense.Amount.ToString("F2");
                    txtExpenseDate.Text = expense.ExpenseDate.ToString("yyyy-MM-dd");
                    ddlPaymentMethod.SelectedValue = expense.PaymentMethod;
                    txtReceiptNumber.Text = expense.ReceiptNumber;
                    chkIsRecurring.Checked = expense.IsRecurring;
                    ddlRecurringType.Enabled = expense.IsRecurring;

                    if (expense.IsRecurring && !string.IsNullOrEmpty(expense.RecurringType))
                    {
                        ddlRecurringType.SelectedValue = expense.RecurringType;
                    }

                    chkStatus.Checked = expense.Status;
                }
                else
                {
                    ShowErrorMessage("Expense not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading expense: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the expense
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

                ExpenseModel expense = new ExpenseModel
                {
                    ExpenseID = expenseID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    ExpenseCategory = ddlCategory.SelectedValue,
                    Description = txtDescription.Text.Trim(),
                    Amount = Convert.ToDecimal(txtAmount.Text),
                    ExpenseDate = Convert.ToDateTime(txtExpenseDate.Text),
                    PaymentMethod = ddlPaymentMethod.SelectedValue,
                    ReceiptNumber = txtReceiptNumber.Text.Trim(),
                    IsRecurring = chkIsRecurring.Checked,
                    RecurringType = chkIsRecurring.Checked ? ddlRecurringType.SelectedValue : null,
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = expenseBAL.SaveExpense(expense);

                if (expenseID > 0)
                {
                    auditLogger.LogUpdate("ExpenseManagement", "Expenses", savedID,
                        $"ExpenseID: {expenseID}", $"Category: {expense.ExpenseCategory}");
                    ShowSuccessMessage("Expense updated successfully.");
                }
                else
                {
                    auditLogger.LogInsert("ExpenseManagement", "Expenses", savedID,
                        $"Category: {expense.ExpenseCategory}, Amount: {expense.Amount}");
                    ShowSuccessMessage("Expense added successfully.");
                }

                Response.AddHeader("REFRESH", "2;URL=ExpenseList.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving expense: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExpenseList.aspx");
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