// ============================================
// Models/BlogPost.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Blog Post Model - Represents a blog post on the website
    /// </summary>
    public class BlogPost
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the blog post
        /// </summary>
        public int BlogPostID { get; set; }

        /// <summary>
        /// Company ID this blog post belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this blog post belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Title of the blog post
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL-friendly slug for the blog post
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Full content of the blog post
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Short excerpt/summary of the blog post
        /// </summary>
        public string Excerpt { get; set; }

        /// <summary>
        /// Path to the featured image
        /// </summary>
        public string FeaturedImage { get; set; }

        /// <summary>
        /// Author of the blog post
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Category of the blog post
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Tags for the blog post (comma-separated)
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Meta title for SEO
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta description for SEO
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Meta keywords for SEO
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Whether the blog post is published
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Date when the blog post was published
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// Number of views for the blog post
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Status of the blog post
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the blog post was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the blog post was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the blog post
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the blog post
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the formatted publish date
        /// </summary>
        public string FormattedPublishDate
        {
            get { return PublishDate.HasValue ? PublishDate.Value.ToString("MMMM dd, yyyy") : "Not Published"; }
        }

        /// <summary>
        /// Gets the URL-friendly slug
        /// </summary>
        public string FriendlyUrl
        {
            get { return $"/blog/{Slug}"; }
        }

        /// <summary>
        /// Gets the tag list as an array
        /// </summary>
        public string[] TagList
        {
            get { return string.IsNullOrEmpty(Tags) ? new string[0] : Tags.Split(','); }
        }

        /// <summary>
        /// Gets the preview content (first 200 characters)
        /// </summary>
        public string PreviewContent
        {
            get
            {
                if (string.IsNullOrEmpty(Content))
                    return string.Empty;

                if (Content.Length <= 200)
                    return Content;

                return Content.Substring(0, 200) + "...";
            }
        }

        #endregion
    }
}