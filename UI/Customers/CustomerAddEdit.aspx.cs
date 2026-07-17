// ============================================
// UI/Customers/CustomerAddEdit.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using CustomerModel = RestaurantManagementSystem.Models.Customer;

namespace RestaurantManagementSystem.UI.Customers
{
    /// <summary>
    /// Customer Add/Edit Page - Handles adding and editing customers
    /// </summary>
    public partial class CustomerAddEdit : Page
    {
        private CustomerBAL customerBAL;
        private AuditLogger auditLogger;
        private int customerID = 0;

        /// <summary>
        /// Page load event - loads customer data for editing
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            customerBAL = new CustomerBAL();
            auditLogger = new AuditLogger();

            if (Request.QueryString["CustomerID"] != null)
            {
                customerID = Convert.ToInt32(Request.QueryString["CustomerID"]);
            }

            if (!IsPostBack)
            {
                if (customerID > 0)
                {
                    ltrPageTitle.Text = "Edit Customer";
                    ltrBreadcrumb.Text = "Edit";
                    ltrFormTitle.Text = "Edit Customer Information";
                    btnSave.Text = "Update Customer";
                    LoadCustomerData(customerID);
                }
                else
                {
                    ltrPageTitle.Text = "Add New Customer";
                    ltrBreadcrumb.Text = "Add New";
                    ltrFormTitle.Text = "Add New Customer";
                    btnSave.Text = "Save Customer";
                    chkStatus.Checked = true;
                    ddlCustomerType.SelectedValue = "Regular";
                    txtLoyaltyPoints.Text = "0";
                }
            }
        }

        /// <summary>
        /// Loads customer data for editing
        /// </summary>
        private void LoadCustomerData(int id)
        {
            try
            {
                CustomerModel customer = customerBAL.GetCustomerById(id);
                if (customer != null)
                {
                    txtCustomerName.Text = customer.CustomerName;
                    txtPhone.Text = customer.Phone;
                    txtEmail.Text = customer.Email;
                    txtAddress.Text = customer.Address;
                    ddlCustomerType.SelectedValue = customer.CustomerType;
                    txtLoyaltyPoints.Text = customer.LoyaltyPoints.ToString();
                    txtMembershipNumber.Text = customer.MembershipNumber;

                    if (customer.Birthday.HasValue)
                    {
                        txtBirthday.Text = customer.Birthday.Value.ToString("yyyy-MM-dd");
                    }

                    if (!string.IsNullOrEmpty(customer.Gender))
                    {
                        ddlGender.SelectedValue = customer.Gender;
                    }

                    chkStatus.Checked = customer.Status;
                }
                else
                {
                    ShowErrorMessage("Customer not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading customer: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the customer
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

                CustomerModel customer = new CustomerModel
                {
                    CustomerID = customerID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    CustomerName = txtCustomerName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    CustomerType = ddlCustomerType.SelectedValue,
                    LoyaltyPoints = string.IsNullOrEmpty(txtLoyaltyPoints.Text) ? 0 : Convert.ToInt32(txtLoyaltyPoints.Text),
                    MembershipNumber = txtMembershipNumber.Text.Trim(),
                    Birthday = string.IsNullOrEmpty(txtBirthday.Text) ? (DateTime?)null : Convert.ToDateTime(txtBirthday.Text),
                    Gender = ddlGender.SelectedValue,
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = customerBAL.SaveCustomer(customer);

                if (customerID > 0)
                {
                    auditLogger.LogUpdate("CustomerManagement", "Customers", savedID,
                        $"CustomerID: {customerID}", $"CustomerName: {customer.CustomerName}");
                    ShowSuccessMessage("Customer updated successfully.");
                }
                else
                {
                    auditLogger.LogInsert("CustomerManagement", "Customers", savedID,
                        $"CustomerName: {customer.CustomerName}");
                    ShowSuccessMessage("Customer added successfully.");
                }

                Response.AddHeader("REFRESH", "2;URL=CustomerList.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving customer: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerList.aspx");
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