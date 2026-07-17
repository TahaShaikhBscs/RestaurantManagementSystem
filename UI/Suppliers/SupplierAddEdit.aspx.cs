// ============================================
// UI/Suppliers/SupplierAddEdit.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Utilities;

// Use model aliases to avoid namespace conflicts
using SupplierModel = RestaurantManagementSystem.Models.Supplier;

namespace RestaurantManagementSystem.UI.Suppliers
{
    /// <summary>
    /// Supplier Add/Edit Page - Handles adding and editing suppliers
    /// </summary>
    public partial class SupplierAddEdit : Page
    {
        private SupplierBAL supplierBAL;
        private AuditLogger auditLogger;
        private int supplierID = 0;

        /// <summary>
        /// Page load event - loads supplier data for editing
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

            if (Request.QueryString["SupplierID"] != null)
            {
                supplierID = Convert.ToInt32(Request.QueryString["SupplierID"]);
            }

            if (!IsPostBack)
            {
                if (supplierID > 0)
                {
                    ltrPageTitle.Text = "Edit Supplier";
                    ltrBreadcrumb.Text = "Edit";
                    ltrFormTitle.Text = "Edit Supplier Information";
                    btnSave.Text = "Update Supplier";
                    LoadSupplierData(supplierID);
                }
                else
                {
                    ltrPageTitle.Text = "Add New Supplier";
                    ltrBreadcrumb.Text = "Add New";
                    ltrFormTitle.Text = "Add New Supplier";
                    btnSave.Text = "Save Supplier";
                    chkStatus.Checked = true;
                }
            }
        }

        /// <summary>
        /// Loads supplier data for editing
        /// </summary>
        private void LoadSupplierData(int id)
        {
            try
            {
                SupplierModel supplier = supplierBAL.GetSupplierById(id);
                if (supplier != null)
                {
                    txtSupplierName.Text = supplier.SupplierName;
                    txtContactPerson.Text = supplier.ContactPerson;
                    txtPhone.Text = supplier.Phone;
                    txtEmail.Text = supplier.Email;
                    txtAddress.Text = supplier.Address;
                    txtTaxNumber.Text = supplier.TaxNumber;
                    txtPaymentTerms.Text = supplier.PaymentTerms;
                    chkStatus.Checked = supplier.Status;
                    UpdateStatusText();
                }
                else
                {
                    ShowErrorMessage("Supplier not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading supplier: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status text based on checkbox
        /// </summary>
        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkStatus.Checked ? "Active" : "Inactive";
        }

        /// <summary>
        /// Saves the supplier
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

                SupplierModel supplier = new SupplierModel
                {
                    SupplierID = supplierID,
                    CompanyID = companyID.Value,
                    BranchID = branchID.Value,
                    SupplierName = txtSupplierName.Text.Trim(),
                    ContactPerson = txtContactPerson.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    TaxNumber = txtTaxNumber.Text.Trim(),
                    PaymentTerms = txtPaymentTerms.Text.Trim(),
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = supplierBAL.SaveSupplier(supplier);

                if (supplierID > 0)
                {
                    auditLogger.LogUpdate("SupplierManagement", "Suppliers", savedID,
                        $"SupplierID: {supplierID}", $"SupplierName: {supplier.SupplierName}");
                    ShowSuccessMessage("Supplier updated successfully.");
                }
                else
                {
                    auditLogger.LogInsert("SupplierManagement", "Suppliers", savedID,
                        $"SupplierName: {supplier.SupplierName}");
                    ShowSuccessMessage("Supplier added successfully.");
                }

                Response.AddHeader("REFRESH", "2;URL=SupplierList.aspx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving supplier: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SupplierList.aspx");
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