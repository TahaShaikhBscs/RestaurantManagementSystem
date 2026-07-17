// ============================================
// Website/Pages/Blog.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Blog Page - Public website blog listing
    /// </summary>
    public partial class Blog : Page
    {
        private BlogBAL blogBAL;
        private CompanyBAL companyBAL;
        private int companyID;

        protected void Page_Load(object sender, EventArgs e)
        {
            blogBAL = new BlogBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
            }

            if (!IsPostBack)
            {
                LoadBlogPosts();
            }
        }

        private void LoadBlogPosts()
        {
            try
            {
                var posts = blogBAL.GetPublishedBlogPosts(companyID, null, 100);
                rptBlogPosts.DataSource = posts;
                rptBlogPosts.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading blog: {ex.Message}");
            }
        }
    }
}