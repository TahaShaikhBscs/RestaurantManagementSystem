
using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Home Page - Public website homepage
    /// </summary>
    public partial class Home : Page
    {
        private SliderBAL sliderBAL;
        private MenuBAL menuBAL;
        private OfferBAL offerBAL;
        private TestimonialBAL testimonialBAL;
        private BlogBAL blogBAL;
        private WebsiteSettingBAL settingBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        /// <summary>
        /// Page load event - loads all homepage data
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeComponents();

            if (!IsPostBack)
            {
                LoadHomePageData();
            }
        }

        /// <summary>
        /// Initializes all BAL components
        /// </summary>
        private void InitializeComponents()
        {
            sliderBAL = new SliderBAL();
            menuBAL = new MenuBAL();
            offerBAL = new OfferBAL();
            testimonialBAL = new TestimonialBAL();
            blogBAL = new BlogBAL();
            settingBAL = new WebsiteSettingBAL();
            companyBAL = new CompanyBAL();

            // Get company ID (default to first company)
            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }
        }

        /// <summary>
        /// Loads all homepage data
        /// </summary>
        private void LoadHomePageData()
        {
            try
            {
                // Load sliders
                LoadSliders();

                // Load about section
                LoadAboutSection();

                // Load offers
                LoadOffers();

                // Load featured menu items
                LoadFeaturedItems();

                // Load testimonials
                LoadTestimonials();

                // Load blog posts
                LoadBlogPosts();
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error loading homepage: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads sliders
        /// </summary>
        private void LoadSliders()
        {
            var sliders = sliderBAL.GetActiveSliders(companyID, null, true, 10);
            if (sliders.Count > 0)
            {
                rptSliders.DataSource = sliders;
                rptSliders.DataBind();
                rptSliderIndicators.DataSource = sliders;
                rptSliderIndicators.DataBind();
            }
        }

        /// <summary>
        /// Loads about section data from settings
        /// </summary>
        private void LoadAboutSection()
        {
            var settings = settingBAL.GetSettings(companyID);

            // These would come from settings table
            ltrAboutTitle.Text = GetSettingValue(settings, "AboutTitle", "A Culinary Journey Like No Other");
            ltrAboutText1.Text = GetSettingValue(settings, "AboutText1", "Welcome to Restaurant Name, where passion for food meets artistic expression.");
            ltrAboutText2.Text = GetSettingValue(settings, "AboutText2", "Every dish is crafted with meticulous attention to detail.");
            ltrExperienceYears.Text = GetSettingValue(settings, "ExperienceYears", "20+");
        }

        /// <summary>
        /// Loads offers
        /// </summary>
        private void LoadOffers()
        {
            var offers = offerBAL.GetActiveOffers(companyID, null, true, 3);
            rptOffers.DataSource = offers;
            rptOffers.DataBind();
        }

        /// <summary>
        /// Loads featured menu items
        /// </summary>
        private void LoadFeaturedItems()
        {
            // Get featured items from menu
            // In a real implementation, you would get items marked as featured
            var items = menuBAL.GetMenuItemsByBranch(1, null, false); // Branch ID would come from settings
            var featured = new List<MenuItem>();

            // Get first 4 items as featured (or filter by IsFeatured)
            for (int i = 0; i < Math.Min(4, items.Count); i++)
            {
                featured.Add(items[i]);
            }

            rptFeaturedItems.DataSource = featured;
            rptFeaturedItems.DataBind();
        }

        /// <summary>
        /// Loads testimonials
        /// </summary>
        private void LoadTestimonials()
        {
            var testimonials = testimonialBAL.GetApprovedTestimonials(companyID, null, true, 3);
            rptTestimonials.DataSource = testimonials;
            rptTestimonials.DataBind();
        }

        /// <summary>
        /// Loads blog posts
        /// </summary>
        private void LoadBlogPosts()
        {
            var posts = blogBAL.GetPublishedBlogPosts(companyID, null, 3);
            rptBlogPosts.DataSource = posts;
            rptBlogPosts.DataBind();
        }

        /// <summary>
        /// Gets a setting value from settings list
        /// </summary>
        private string GetSettingValue(List<WebsiteSetting> settings, string key, string defaultValue = "")
        {
            var setting = settings.Find(s => s.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            return setting != null ? setting.SettingValue : defaultValue;
        }
    }
}