// ============================================
// Website/MasterPages/WebSiteMaster.master.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Website.MasterPages
{
    /// <summary>
    /// Website Master Page - Public-facing website master
    /// </summary>
    public partial class WebSiteMaster : System.Web.UI.MasterPage
    {
        private WebsiteSettingBAL settingBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        /// <summary>
        /// Page load event - loads website settings
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
                LoadWebsiteSettings();
            }
        }

        /// <summary>
        /// Loads all website settings
        /// </summary>
        private void LoadWebsiteSettings()
        {
            try
            {
                var settings = settingBAL.GetSettings(companyID);

                // Load theme settings
                string primaryColor = GetSettingValue(settings, "PrimaryColor", "#667eea");
                string secondaryColor = GetSettingValue(settings, "SecondaryColor", "#764ba2");
                string accentColor = GetSettingValue(settings, "AccentColor", "#f39c12");
                string backgroundColor = GetSettingValue(settings, "BackgroundColor", "#ffffff");
                string textColor = GetSettingValue(settings, "TextColor", "#333333");
                string buttonColor = GetSettingValue(settings, "ButtonColor", "#667eea");
                string fontFamily = GetSettingValue(settings, "WebsiteFont", "Poppins");
                string borderRadius = GetSettingValue(settings, "BorderRadius", "10px");

                // Inject dynamic CSS
                string themeCSS = $@"
                    :root {{
                        --primary-color: {primaryColor};
                        --secondary-color: {secondaryColor};
                        --accent-color: {accentColor};
                        --background-color: {backgroundColor};
                        --text-color: {textColor};
                        --button-color: {buttonColor};
                        --font-family: '{fontFamily}', sans-serif;
                        --border-radius: {borderRadius};
                    }}
                    
                    .btn-primary {{
                        background: {primaryColor} !important;
                        border-color: {primaryColor} !important;
                    }}
                    
                    .btn-primary:hover {{
                        background: {secondaryColor} !important;
                        border-color: {secondaryColor} !important;
                    }}
                    
                    .btn-outline-primary {{
                        border-color: {primaryColor} !important;
                        color: {primaryColor} !important;
                    }}
                    
                    .btn-outline-primary:hover {{
                        background: {primaryColor} !important;
                        color: #fff !important;
                    }}
                    
                    .section-subtitle {{
                        color: {primaryColor} !important;
                    }}
                    
                    .navbar-nav .nav-link::after,
                    .navbar-nav .nav-link.active::after {{
                        background: {primaryColor} !important;
                    }}
                    
                    .navbar-nav .nav-link:hover,
                    .navbar-nav .nav-link.active {{
                        color: {primaryColor} !important;
                    }}
                    
                    .btn-reserve {{
                        background: {primaryColor} !important;
                    }}
                    
                    .btn-reserve:hover {{
                        background: {secondaryColor} !important;
                    }}
                    
                    .about-experience {{
                        background: {primaryColor} !important;
                    }}
                    
                    .about-features .feature-item i {{
                        color: {primaryColor} !important;
                    }}
                    
                    .offer-details .price {{
                        color: {primaryColor} !important;
                    }}
                    
                    .menu-price {{
                        color: {primaryColor} !important;
                    }}
                    
                    .cta-section {{
                        background: linear-gradient(135deg, {primaryColor}, {secondaryColor}) !important;
                    }}
                    
                    .footer-social a:hover {{
                        background: {primaryColor} !important;
                    }}
                    
                    .footer-contact li i {{
                        color: {primaryColor} !important;
                    }}
                    
                    .footer-links a:hover {{
                        color: {primaryColor} !important;
                    }}
                    
                    .back-to-top {{
                        background: {primaryColor} !important;
                    }}
                    
                    .back-to-top:hover {{
                        background: {secondaryColor} !important;
                    }}
                ";

                // Find the theme styles placeholder and inject CSS
                var styleTag = FindControl("themeStyles") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (styleTag != null)
                {
                    styleTag.InnerHtml = themeCSS;
                }

                // Set favicon
                string favicon = GetSettingValue(settings, "Favicon", "/Website/Assets/images/favicon.ico");
                ltrFavicon.Text = favicon;

                // Set title
                string siteName = GetSettingValue(settings, "SiteName", "Restaurant Name");
                Page.Title = siteName + " - " + (Page.Title ?? "Fine Dining");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading website settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a setting value from the settings list
        /// </summary>
        private string GetSettingValue(System.Collections.Generic.List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }
    }
}