// ============================================
// Website/Controls/Footer.ascx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Website.Controls
{
    /// <summary>
    /// Footer User Control - Website footer
    /// </summary>
    public partial class Footer : UserControl
    {
        private WebsiteSettingBAL settingBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        /// <summary>
        /// Page load event - loads footer data
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            settingBAL = new WebsiteSettingBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }

            if (!IsPostBack)
            {
                LoadFooterData();
            }
        }

        /// <summary>
        /// Loads footer data from settings
        /// </summary>
        private void LoadFooterData()
        {
            try
            {
                var settings = settingBAL.GetSettings(companyID);

                // Set footer logo
                string footerLogo = GetSettingValue(settings, "FooterLogo", "/Website/Assets/images/logo-footer.png");
                imgFooterLogo.ImageUrl = footerLogo;

                // Set footer description
                string siteName = GetSettingValue(settings, "SiteName", "Restaurant Name");
                ltrFooterDescription.Text = $"Experience fine dining at its best. {siteName} where culinary artistry meets warm hospitality.";

                // Set opening hours
                ltrWeekdayHours.Text = GetSettingValue(settings, "WeekdayHours", "11:00 AM - 11:00 PM");
                ltrSaturdayHours.Text = GetSettingValue(settings, "SaturdayHours", "10:00 AM - 12:00 AM");
                ltrSundayHours.Text = GetSettingValue(settings, "SundayHours", "10:00 AM - 10:00 PM");

                // Set contact info
                ltrPhone.Text = GetSettingValue(settings, "Phone", "+1 234 567 890");
                ltrEmail.Text = GetSettingValue(settings, "ContactEmail", "info@restaurantname.com");
                ltrAddress.Text = GetSettingValue(settings, "Address", "123 Restaurant Street, City, Country");

                // Set copyright
                ltrCurrentYear.Text = DateTime.Now.Year.ToString();
                ltrCopyright.Text = siteName;

                // Load social links
                var socialLinks = new List<SocialLink>();
                AddSocialLink(socialLinks, "Facebook", GetSettingValue(settings, "Facebook"), "facebook-f");
                AddSocialLink(socialLinks, "Twitter", GetSettingValue(settings, "Twitter"), "twitter");
                AddSocialLink(socialLinks, "Instagram", GetSettingValue(settings, "Instagram"), "instagram");
                AddSocialLink(socialLinks, "YouTube", GetSettingValue(settings, "YouTube"), "youtube");
                AddSocialLink(socialLinks, "LinkedIn", GetSettingValue(settings, "LinkedIn"), "linkedin-in");

                rptSocialLinks.DataSource = socialLinks;
                rptSocialLinks.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading footer data: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a setting value from the settings list
        /// </summary>
        private string GetSettingValue(List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }

        /// <summary>
        /// Adds a social link to the list
        /// </summary>
        private void AddSocialLink(List<SocialLink> links, string name, string url, string icon)
        {
            if (!string.IsNullOrEmpty(url))
            {
                links.Add(new SocialLink { SocialName = name, SocialUrl = url, SocialIcon = icon });
            }
        }

        /// <summary>
        /// Social Link Class
        /// </summary>
        public class SocialLink
        {
            public string SocialName { get; set; }
            public string SocialUrl { get; set; }
            public string SocialIcon { get; set; }
        }
    }
}