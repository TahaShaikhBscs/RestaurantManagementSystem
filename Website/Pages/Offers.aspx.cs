// ============================================
// Website/Pages/Offers.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Offers Page - Public website offers listing
    /// </summary>
    public partial class Offers : Page
    {
        private OfferBAL offerBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        protected void Page_Load(object sender, EventArgs e)
        {
            offerBAL = new OfferBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }

            if (!IsPostBack)
            {
                LoadOffers();
            }
        }

        private void LoadOffers()
        {
            try
            {
                var offers = offerBAL.GetActiveOffers(companyID, null, null, 100);
                rptOffers.DataSource = offers;
                rptOffers.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading offers: {ex.Message}");
            }
        }
    }
}