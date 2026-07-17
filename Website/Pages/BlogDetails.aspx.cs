// ============================================
// Website/Pages/BlogDetails.aspx.cs
// ============================================

using System;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Blog Details Page - Public website blog post details
    /// </summary>
    public partial class BlogDetails : Page
    {
        private BlogBAL blogBAL;
        private int blogPostID;

        protected void Page_Load(object sender, EventArgs e)
        {
            blogBAL = new BlogBAL();

            if (Request.QueryString["ID"] != null)
            {
                blogPostID = Convert.ToInt32(Request.QueryString["ID"]);
            }

            if (!IsPostBack && blogPostID > 0)
            {
                LoadBlogPost(blogPostID);
            }
        }

        private void LoadBlogPost(int id)
        {
            try
            {
                var post = blogBAL.GetBlogPostById(id);
                if (post != null && post.IsPublished)
                {
                    // Set page title and meta
                    ltrPageTitle.Text = post.Title;
                    ltrMetaDescription.Text = post.MetaDescription ?? post.Excerpt;

                    // Set blog content
                    ltrBlogTitle.Text = post.Title;
                    ltrBreadcrumb.Text = post.Title;
                    ltrContent.Text = post.Content;
                    ltrPublishDate.Text = post.PublishDate.HasValue ? post.PublishDate.Value.ToString("MMMM dd, yyyy") : "Not Published";
                    ltrAuthor.Text = post.Author ?? "Admin";
                    ltrCategory.Text = post.Category ?? "General";
                    ltrViews.Text = post.ViewCount.ToString();

                    // Set image
                    if (!string.IsNullOrEmpty(post.FeaturedImage))
                    {
                        imgFeatured.ImageUrl = post.FeaturedImage;
                        imgFeatured.Visible = true;
                    }

                    // Set tags
                    if (!string.IsNullOrEmpty(post.Tags))
                    {
                        var tags = post.Tags.Split(',');
                        foreach (var tag in tags)
                        {
                            ltrTags.Text += $"<span class='badge bg-secondary me-1'>{tag.Trim()}</span> ";
                        }
                    }
                }
                else
                {
                    Response.Redirect("Blog.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading blog post: {ex.Message}");
            }
        }
    }
}