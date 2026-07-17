// ============================================
// Website/Pages/Reservation.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Reservation Page - Public website reservation form
    /// </summary>
    public partial class Reservation : Page
    {
        private CompanyBAL companyBAL;
        private WebsiteSettingBAL settingBAL;
        private BranchBAL branchBAL;
        private int companyID;
        private int branchID;

        protected void Page_Load(object sender, EventArgs e)
        {
            companyBAL = new CompanyBAL();
            settingBAL = new WebsiteSettingBAL();
            branchBAL = new BranchBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
                var branches = branchBAL.GetBranchesByCompany(companyID);
                if (branches.Count > 0)
                {
                    branchID = branches[0].BranchID;
                }
            }

            if (!IsPostBack)
            {
                LoadContactInfo();
                txtDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
        }

        private void LoadContactInfo()
        {
            var settings = settingBAL.GetSettings(companyID);
            ltrPhone.Text = GetSettingValue(settings, "Phone", "+1 234 567 890");
            ltrEmail.Text = GetSettingValue(settings, "ContactEmail", "info@restaurantname.com");
            ltrAddress.Text = GetSettingValue(settings, "Address", "123 Restaurant Street, City, Country");
        }

        private string GetSettingValue(System.Collections.Generic.List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                // Save reservation to database
                // In a real implementation, you would save to the Reservations table

                ShowSuccessMessage("Your reservation has been submitted successfully! We will confirm shortly.");
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error submitting reservation: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtSpecialRequests.Text = string.Empty;
            ddlGuests.SelectedIndex = 0;
            ddlTime.SelectedIndex = 0;
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