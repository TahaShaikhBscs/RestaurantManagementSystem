// ============================================
// BAL/BlogBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Blog BAL - Business logic layer for blog posts
    /// </summary>
    public class BlogBAL
    {
        #region Private Fields

        private readonly BlogDAL blogDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public BlogBAL()
        {
            blogDAL = new BlogDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all blog posts with filters
        /// </summary>
        public List<BlogPost> GetBlogPosts(int companyID, int? branchID = null, bool? isPublished = null, int limit = 100, int offset = 0)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            if (offset < 0)
                throw new ArgumentException("Offset cannot be negative.");

            return blogDAL.GetBlogPosts(companyID, branchID, isPublished, limit, offset);
        }

        /// <summary>
        /// Gets published blog posts
        /// </summary>
        public List<BlogPost> GetPublishedBlogPosts(int companyID, int? branchID = null, int limit = 100, int offset = 0)
        {
            return GetBlogPosts(companyID, branchID, true, limit, offset);
        }

        /// <summary>
        /// Gets a blog post by ID
        /// </summary>
        public BlogPost GetBlogPostById(int blogPostID)
        {
            if (blogPostID <= 0)
                throw new ArgumentException("Invalid blog post ID.");

            return blogDAL.GetBlogPostById(blogPostID);
        }

        /// <summary>
        /// Gets a blog post by slug
        /// </summary>
        public BlogPost GetBlogPostBySlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug is required.");

            var post = blogDAL.GetBlogPostBySlug(slug);

            // Increment view count if post is found and published
            if (post != null && post.IsPublished)
            {
                blogDAL.IncrementViewCount(post.BlogPostID);
                post.ViewCount++;
            }

            return post;
        }

        /// <summary>
        /// Gets related blog posts
        /// </summary>
        public List<BlogPost> GetRelatedBlogPosts(int blogPostID, int count = 3)
        {
            if (blogPostID <= 0)
                throw new ArgumentException("Invalid blog post ID.");

            var post = GetBlogPostById(blogPostID);
            if (post == null)
                return new List<BlogPost>();

            // Get posts with same category or tags
            var allPosts = GetPublishedBlogPosts(post.CompanyID, post.BranchID, 100);
            var related = new List<BlogPost>();

            foreach (var p in allPosts)
            {
                if (p.BlogPostID == blogPostID) continue;

                // Check if same category or shares tags
                if (p.Category == post.Category)
                {
                    related.Add(p);
                }
                else if (!string.IsNullOrEmpty(post.Tags) && !string.IsNullOrEmpty(p.Tags))
                {
                    var postTags = post.Tags.Split(',');
                    var pTags = p.Tags.Split(',');

                    foreach (var tag in postTags)
                    {
                        foreach (var pt in pTags)
                        {
                            if (tag.Trim().Equals(pt.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                related.Add(p);
                                break;
                            }
                        }
                        if (related.Contains(p)) break;
                    }
                }

                if (related.Count >= count) break;
            }

            return related;
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a blog post
        /// </summary>
        public int SaveBlogPost(BlogPost post)
        {
            ValidateBlogPost(post);

            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(post.Slug))
            {
                post.Slug = GenerateSlug(post.Title);
            }
            else
            {
                post.Slug = GenerateSlug(post.Slug);
            }

            // Check for duplicate slug
            var existing = blogDAL.GetBlogPostBySlug(post.Slug);
            if (existing != null && existing.BlogPostID != post.BlogPostID)
            {
                throw new Exception("A blog post with this slug already exists.");
            }

            int result;

            if (post.BlogPostID > 0)
            {
                bool updated = blogDAL.UpdateBlogPost(post);
                result = post.BlogPostID;
                if (updated)
                {
                    auditLogger.LogUpdate("Blog", "BlogPosts", post.BlogPostID,
                        $"Title: {post.Title}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = blogDAL.InsertBlogPost(post);
                if (result > 0)
                {
                    auditLogger.LogInsert("Blog", "BlogPosts", result,
                        $"Title: {post.Title}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a blog post
        /// </summary>
        public bool DeleteBlogPost(int blogPostID, int updatedBy)
        {
            if (blogPostID <= 0)
                throw new ArgumentException("Invalid blog post ID.");

            var post = GetBlogPostById(blogPostID);
            if (post == null)
                throw new Exception("Blog post not found.");

            bool deleted = blogDAL.DeleteBlogPost(blogPostID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("Blog", "BlogPosts", blogPostID,
                    $"Title: {post.Title}");
            }

            return deleted;
        }

        /// <summary>
        /// Publishes a blog post
        /// </summary>
        public bool PublishBlogPost(int blogPostID, int updatedBy)
        {
            if (blogPostID <= 0)
                throw new ArgumentException("Invalid blog post ID.");

            var post = GetBlogPostById(blogPostID);
            if (post == null)
                throw new Exception("Blog post not found.");

            post.IsPublished = true;
            post.PublishDate = DateTime.Now;
            post.UpdatedBy = updatedBy;

            return blogDAL.UpdateBlogPost(post);
        }

        /// <summary>
        /// Unpublishes a blog post
        /// </summary>
        public bool UnpublishBlogPost(int blogPostID, int updatedBy)
        {
            if (blogPostID <= 0)
                throw new ArgumentException("Invalid blog post ID.");

            var post = GetBlogPostById(blogPostID);
            if (post == null)
                throw new Exception("Blog post not found.");

            post.IsPublished = false;
            post.UpdatedBy = updatedBy;

            return blogDAL.UpdateBlogPost(post);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates a blog post
        /// </summary>
        private void ValidateBlogPost(BlogPost post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            if (post.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(post.Title))
                throw new ArgumentException("Title is required.");

            if (post.Title.Length > 200)
                throw new ArgumentException("Title cannot exceed 200 characters.");

            if (string.IsNullOrWhiteSpace(post.Content))
                throw new ArgumentException("Content is required.");
        }

        /// <summary>
        /// Generates a URL-friendly slug
        /// </summary>
        private string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "post";

            // Convert to lowercase
            string slug = text.ToLower();

            // Remove special characters
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace spaces with hyphens
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");

            // Remove multiple hyphens
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");

            // Trim hyphens
            slug = slug.Trim('-');

            if (string.IsNullOrEmpty(slug))
                return "post";

            return slug;
        }

        #endregion
    }
}