// ============================================
// Website/Controls/Header.ascx.cs - Fixed
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Website.Controls
{
    /// <summary>
    /// Header User Control - Website navigation
    /// </summary>
    public partial class Header : UserControl
    {
        #region Private Fields

        private WebsiteSettingBAL settingBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event - loads header data
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            settingBAL = new WebsiteSettingBAL();
            companyBAL = new CompanyBAL();

            // Get company ID (default to first company)
            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }

            if (!IsPostBack)
            {
                LoadHeaderData();
            }
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads header data from settings
        /// </summary>
        private void LoadHeaderData()
        {
            try
            {
                var settings = settingBAL.GetSettings(companyID);

                // Set site name
                string siteName = GetSettingValue(settings, "SiteName", "Restaurant Name");
                ltrSiteName.Text = siteName;

                // Set logo
                string logo = GetSettingValue(settings, "Logo", "/Website/Assets/images/logo.png");
                imgLogo.ImageUrl = logo;
                imgLogo.AlternateText = siteName;

                // Get current page for active state
                string currentPage = System.IO.Path.GetFileNameWithoutExtension(Request.Path);

                // Find and highlight active page using server-side approach
                // We'll use a repeater or manual navigation instead of accessing client controls
                SetActivePage(currentPage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading header data: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the active page in navigation
        /// </summary>
        private void SetActivePage(string currentPage)
        {
            // This method is no longer needed as we'll use client-side approach
            // The active state will be handled by CSS or JavaScript
            // Or we can use server-side navigation with Repeater
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets a setting value from the settings list
        /// </summary>
        private string GetSettingValue(List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }

        #endregion
    }
}