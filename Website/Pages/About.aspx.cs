// ============================================
// Website/Pages/About.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// About Page - Public website about page
    /// </summary>
    public partial class About : Page
    {
        private ChefBAL chefBAL;
        private WebsiteSettingBAL settingBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        protected void Page_Load(object sender, EventArgs e)
        {
            chefBAL = new ChefBAL();
            settingBAL = new WebsiteSettingBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }

            if (!IsPostBack)
            {
                LoadAboutData();
            }
        }

        private void LoadAboutData()
        {
            try
            {
                var settings = settingBAL.GetSettings(companyID);

                // Load story text from settings
                ltrStoryTitle.Text = GetSettingValue(settings, "StoryTitle", "A Passion for Culinary Excellence");
                ltrStoryText1.Text = GetSettingValue(settings, "StoryText1", "Restaurant Name was founded with a simple yet ambitious vision...");
                ltrStoryText2.Text = GetSettingValue(settings, "StoryText2", "Over the years, we have honed our craft...");
                ltrStoryText3.Text = GetSettingValue(settings, "StoryText3", "We believe that great food is an art form...");

                // Load chefs
                var chefs = chefBAL.GetChefs(companyID, null, null, 100);
                rptChefs.DataSource = chefs;
                rptChefs.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading about page: {ex.Message}");
            }
        }

        private string GetSettingValue(List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }
    }
}