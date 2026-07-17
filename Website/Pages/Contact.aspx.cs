// ============================================
// Website/Pages/Contact.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Contact Page - Public website contact form
    /// </summary>
    public partial class Contact : Page
    {
        private ContactMessageBAL messageBAL;
        private WebsiteSettingBAL settingBAL;
        private CompanyBAL companyBAL;
        private int companyID;
        private int branchID;

        protected void Page_Load(object sender, EventArgs e)
        {
            messageBAL = new ContactMessageBAL();
            settingBAL = new WebsiteSettingBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
                var branchBAL = new BranchBAL();
                var branches = branchBAL.GetBranchesByCompany(companyID);
                if (branches.Count > 0)
                {
                    branchID = branches[0].BranchID;
                }
            }

            if (!IsPostBack)
            {
                LoadContactInfo();
            }
        }

        private void LoadContactInfo()
        {
            var settings = settingBAL.GetSettings(companyID);
            ltrPhone.Text = GetSettingValue(settings, "Phone", "+1 234 567 890");
            ltrEmail.Text = GetSettingValue(settings, "ContactEmail", "info@restaurantname.com");
            ltrAddress.Text = GetSettingValue(settings, "Address", "123 Restaurant Street, City, Country");
            ltrHours.Text = GetSettingValue(settings, "WeekdayHours", "Mon-Fri: 11:00 AM - 11:00 PM");
            ltrSaturday.Text = GetSettingValue(settings, "SaturdayHours", "Sat: 10:00 AM - 12:00 AM");
            ltrSunday.Text = GetSettingValue(settings, "SundayHours", "Sun: 10:00 AM - 10:00 PM");
        }

        private string GetSettingValue(List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                var message = new ContactMessage
                {
                    CompanyID = companyID,
                    BranchID = branchID,
                    Name = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Subject = txtSubject.Text.Trim(),
                    Message = txtMessage.Text.Trim(),
                    IPAddress = GetClientIP(),
                    UserAgent = Request.UserAgent
                };

                int result = messageBAL.InsertContactMessage(message);
                if (result > 0)
                {
                    ShowSuccessMessage("Your message has been sent successfully! We'll get back to you soon.");
                    ClearForm();
                }
                else
                {
                    ShowErrorMessage("Failed to send message. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error sending message: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtMessage.Text = string.Empty;
        }

        private string GetClientIP()
        {
            string ip = Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(ip))
                ip = Request.UserHostAddress;
            return ip;
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