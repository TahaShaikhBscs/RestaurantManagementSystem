// ============================================
// Website/Pages/Gallery.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Gallery Page - Public website gallery
    /// </summary>
    public partial class Gallery : Page
    {
        private GalleryBAL galleryBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        protected void Page_Load(object sender, EventArgs e)
        {
            galleryBAL = new GalleryBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }

            if (!IsPostBack)
            {
                LoadGalleryImages();
            }
        }

        private void LoadGalleryImages()
        {
            try
            {
                var images = galleryBAL.GetGalleryImages(companyID, null, null, 100);
                rptGallery.DataSource = images;
                rptGallery.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading gallery: {ex.Message}");
            }
        }
    }
}