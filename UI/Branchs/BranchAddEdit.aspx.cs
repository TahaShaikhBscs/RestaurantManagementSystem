// ============================================
// UI/Branch/BranchAddEdit.aspx.cs - Updated
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.UI.Branch
{
    /// <summary>
    /// Branch Add/Edit Page - Handles adding and editing branches
    /// </summary>
    public partial class BranchAddEdit : Page
    {
        #region Private Fields

        private BranchBAL branchBAL;
        private CompanyBAL companyBAL;
        private AuditLogger auditLogger;
        private int branchID = 0;
        private const string MODULE_NAME = "BranchManagement";

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            InitializeComponents();

            if (Request.QueryString["BranchID"] != null)
            {
                branchID = Convert.ToInt32(Request.QueryString["BranchID"]);
            }

            if (!IsPostBack)
            {
                LoadCompanies();

                if (branchID > 0)
                {
                    SetEditMode();
                    LoadBranchData(branchID);
                }
                else
                {
                    SetAddMode();
                }
            }
        }

        #endregion

        #region Initialization

        private void InitializeComponents()
        {
            branchBAL = new BranchBAL();
            companyBAL = new CompanyBAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Mode Configuration

        private void SetEditMode()
        {
            ltrPageTitle.Text = "Edit Branch";
            ltrBreadcrumb.Text = "Edit";
            ltrFormTitle.Text = "Edit Branch Information";
            btnSave.Text = "Update Branch";
            btnSaveAndContinue.Visible = true;
        }

        private void SetAddMode()
        {
            ltrPageTitle.Text = "Add New Branch";
            ltrBreadcrumb.Text = "Add New";
            ltrFormTitle.Text = "Add New Branch";
            btnSave.Text = "Save Branch";
            btnSaveAndContinue.Visible = false;
            chkStatus.Checked = true;
            chkKitchen.Checked = true;
            chkHasTables.Checked = true;
            chkHasCashCounter.Checked = true;
            chkHasWarehouse.Checked = true;
        }

        #endregion

        #region Data Loading

        private void LoadCompanies()
        {
            try
            {
                List<Models.Company> companies = companyBAL.GetAllCompanies();
                ddlCompany.DataSource = companies;
                ddlCompany.DataTextField = "CompanyName";
                ddlCompany.DataValueField = "CompanyID";
                ddlCompany.DataBind();
                ddlCompany.Items.Insert(0, new ListItem("-- Select Company --", "0"));
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading companies: {ex.Message}");
            }
        }

        private void LoadBranchData(int id)
        {
            try
            {
                Models.Branch branch = branchBAL.GetBranchById(id);
                if (branch != null)
                {
                    ddlCompany.SelectedValue = branch.CompanyID.ToString();
                    txtBranchName.Text = branch.BranchName;
                    txtAddress.Text = branch.Address;
                    txtPhone.Text = branch.Phone;
                    txtEmail.Text = branch.Email;
                    chkKitchen.Checked = branch.Kitchen;
                    chkHasTables.Checked = branch.HasTables;
                    chkHasCashCounter.Checked = branch.HasCashCounter;
                    chkHasWarehouse.Checked = branch.HasWarehouse;
                    chkStatus.Checked = branch.Status;
                    UpdateStatusText();

                    auditLogger.LogAction("View", MODULE_NAME, "Branch", id, $"BranchName: {branch.BranchName}");
                }
                else
                {
                    ShowErrorMessage("Branch not found.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "LoadBranchData", id, null, ex.Message);
                ShowErrorMessage($"Error loading branch: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateStatusText()
        {
            ltrStatusText.Text = chkStatus.Checked ? "Active" : "Inactive";
        }

        private bool ValidateBranch()
        {
            if (ddlCompany.SelectedValue == "0")
            {
                ShowErrorMessage("Please select a company.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBranchName.Text))
            {
                ShowErrorMessage("Branch name is required.");
                return false;
            }

            return true;
        }

        #endregion

        #region Button Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveBranch(false);
        }

        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveBranch(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BranchList.aspx");
        }

        #endregion

        #region Business Logic

        private void SaveBranch(bool continueEditing)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!ValidateBranch())
                    return;

                int? userID = SessionHelper.GetUserId();
                if (!userID.HasValue)
                {
                    ShowErrorMessage("User session expired. Please login again.");
                    return;
                }

                Models.Branch branch = new Models.Branch
                {
                    BranchID = branchID,
                    CompanyID = Convert.ToInt32(ddlCompany.SelectedValue),
                    BranchName = txtBranchName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Kitchen = chkKitchen.Checked,
                    HasTables = chkHasTables.Checked,
                    HasCashCounter = chkHasCashCounter.Checked,
                    HasWarehouse = chkHasWarehouse.Checked,
                    Status = chkStatus.Checked,
                    CreatedBy = userID.Value,
                    UpdatedBy = userID.Value
                };

                int savedID = branchBAL.SaveBranch(branch);

                if (savedID > 0)
                {
                    if (branchID > 0)
                    {
                        auditLogger.LogUpdate(MODULE_NAME, "Branches", savedID,
                            $"BranchID: {branchID}", $"BranchName: {branch.BranchName}");
                        ShowSuccessMessage("Branch updated successfully.");
                    }
                    else
                    {
                        auditLogger.LogInsert(MODULE_NAME, "Branches", savedID,
                            $"BranchName: {branch.BranchName}");
                        ShowSuccessMessage("Branch added successfully.");
                    }

                    if (continueEditing)
                    {
                        Response.Redirect($"BranchAddEdit.aspx?BranchID={savedID}");
                    }
                    else
                    {
                        Response.AddHeader("REFRESH", "2;URL=BranchList.aspx");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to save branch.");
                }
            }
            catch (Exception ex)
            {
                auditLogger.LogAction("Error", MODULE_NAME, "SaveBranch", branchID, null, ex.Message);
                ShowErrorMessage($"Error saving branch: {ex.Message}");
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