// ============================================
// UI/Company/CompanyAddEdit.aspx.cs - Updated
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Company
{
    /// <summary>
    /// Company Add/Edit Page - Handles adding and editing companies
    /// </summary>
    public partial class CompanyAddEdit : Page
    {
        #region Private Fields

        private CompanyBAL companyBAL;
        private AuditLogger auditLogger;
        private int companyID = 0;
        private const string MODULE_NAME = "CompanyManagement";

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - loads company data for editing
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            if (Request.QueryString["CompanyID"] != null)
            {
                companyID = Convert.ToInt32(Request.QueryString["CompanyID"]);
            }

            if (!IsPostBack)
            {
                if (companyID > 0)
                {
                    SetEditMode();
                    LoadCompanyData(companyID);
                }
                else
                {
                    SetAddMode();
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes all required components
        /// </summary>
        private void InitializeComponents()
        {
            companyBAL = new CompanyBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Mode Configuration

        /// <summary>
        /// Sets the page to edit mode
        /// </summary>
        private void SetEditMode()
        {
            ltrPageTitle.Text = "Edit Company";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Company Information";
            btnSave.Text = "Update Company";
            btnSaveAndContinue.Visible = true;
        }

        /// <summary>
        /// Sets the page to add mode
        /// </summary>
        private void SetAddMode()
        {
            ltrPageTitle.Text = "Add New Company";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Add New Company";
            btnSave.Text = "Save Company";
            btnSaveAndContinue.Visible = false;
            chkStatus.Checked = true;
            txtCurrencyCode.Text = "USD";
            txtCurrencySymbol.Text = "$";
            ddlTimeZone.SelectedValue = "UTC";
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads company data for editing
        /// </summary>
        private void LoadCompanyData(int id)
        {
            try
            {
                Models.Company company = companyBAL.GetCompanyById(id);
                if (company != null)
                {
                    txtCompanyName.Text = company.CompanyName;
                    txtAddress.Text = company.Address;
                    txtPhone.Text = company.Phone;
                    txtEmail.Text = company.Email;
                    txtNTN.Text = company.NTN;
                    txtGST.Text = company.GST;
                    txtLogo.Text = company.Logo;
                    txtCurrencyCode.Text = company.CurrencyCode;
                    txtCurrencySymbol.Text = company.CurrencySymbol;
                    ddlTimeZone.SelectedValue = company.TimeZone;
                    chkStatus.Checked = company.Status;
                    UpdateStatusText();

                    auditLogger.LogAction("View", MODULE_NAME, "Company", id, $"CompanyName: {company.CompanyName}");
                }
                else
                {
                    ShowErrorMessage("Company not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadCompanyData", id, null, ex.Message);
                ShowErrorMessage($"Error loading company: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Updates the status text based on checkbox
        /// </summary>
        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkStatus.Checked ? "Active" : "Inactive";
        }

        /// <summary>
        /// Validates the company data
        /// </summary>
        private bool ValidateCompany()
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                ShowErrorMessage("Company name is required.");
                return false;
            }

            return true;
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Saves the company
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveCompany(false);
        }

        /// <summary>
        /// Saves the company and continues editing
        /// </summary>
        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveCompany(true);
        }

        /// <summary>
        /// Cancels and redirects to list page
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CompanyList.aspx");
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Saves the company
        /// </summary>
        private void SaveCompany(bool continueEditing)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidateCompany())
                    return;

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                Models.Company company = new Models.Company
                {
                    CompanyID = companyID,
                    CompanyName = txtCompanyName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NTN = txtNTN.Text.Trim(),
                    GST = txtGST.Text.Trim(),
                    Logo = txtLogo.Text.Trim(),
                    CurrencyCode = txtCurrencyCode.Text.Trim(),
                    CurrencySymbol = txtCurrencySymbol.Text.Trim(),
                    TimeZone = ddlTimeZone.SelectedValue,
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = companyBAL.SaveCompany(company);

                if (savedID > 0)
                {
                    if (companyID > 0)
                    {
                        auditLogger.LogUpdate(MODULE_NAME, "Companies", savedID,
                            $"CompanyID: {companyID}", $"CompanyName: {company.CompanyName}");
                        ShowSuccessMessage("Company updated successfully.");
                    }
                    else
                    {
                        auditLogger.LogInsert(MODULE_NAME, "Companies", savedID,
                            $"CompanyName: {company.CompanyName}");
                        ShowSuccessMessage("Company added successfully.");
                    }

                    if (continueEditing)
                    {
                        Response.Redirect($"CompanyAddEdit.aspx?CompanyID={savedID}");
                    }
                    else
                    {
                        Response.AddHeader("REFRESH", "2;URL=CompanyList.aspx");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to save company.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveCompany", companyID, null, ex.Message);
                ShowErrorMessage($"Error saving company: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

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

        #endregion
    }
}