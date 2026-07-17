// ============================================
// DAL/BlogDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Blog DAL - Data access layer for blog posts
    /// </summary>
    public class BlogDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public BlogDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all blog posts with filters
        /// </summary>
        public List<BlogPost> GetBlogPosts(int companyID, int? branchID = null, bool? isPublished = null, int limit = 100, int offset = 0)
        {
            try
            {
                List<BlogPost> posts = new List<BlogPost>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IsPublished", isPublished),
                    dbHelper.CreateParameter("@Limit", limit),
                    dbHelper.CreateParameter("@Offset", offset)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetBlogPosts", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    posts.Add(MapDataRowToBlogPost(row));
                }

                return posts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting blog posts: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a blog post by ID
        /// </summary>
        public BlogPost GetBlogPostById(int blogPostID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BlogPostID", blogPostID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetBlogPostById", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToBlogPost(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting blog post by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a blog post by slug
        /// </summary>
        public BlogPost GetBlogPostBySlug(string slug)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@Slug", slug)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetBlogPostBySlug", parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToBlogPost(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting blog post by slug: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new blog post
        /// </summary>
        public int InsertBlogPost(BlogPost post)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", post.CompanyID),
                    dbHelper.CreateParameter("@BranchID", post.BranchID),
                    dbHelper.CreateParameter("@Title", post.Title),
                    dbHelper.CreateParameter("@Slug", post.Slug),
                    dbHelper.CreateParameter("@Content", post.Content),
                    dbHelper.CreateParameter("@Excerpt", post.Excerpt),
                    dbHelper.CreateParameter("@FeaturedImage", post.FeaturedImage),
                    dbHelper.CreateParameter("@Author", post.Author),
                    dbHelper.CreateParameter("@Category", post.Category),
                    dbHelper.CreateParameter("@Tags", post.Tags),
                    dbHelper.CreateParameter("@MetaTitle", post.MetaTitle),
                    dbHelper.CreateParameter("@MetaDescription", post.MetaDescription),
                    dbHelper.CreateParameter("@MetaKeywords", post.MetaKeywords),
                    dbHelper.CreateParameter("@IsPublished", post.IsPublished),
                    dbHelper.CreateParameter("@PublishDate", post.PublishDate),
                    dbHelper.CreateParameter("@CreatedBy", post.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertBlogPost", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting blog post: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing blog post
        /// </summary>
        public bool UpdateBlogPost(BlogPost post)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BlogPostID", post.BlogPostID),
                    dbHelper.CreateParameter("@Title", post.Title),
                    dbHelper.CreateParameter("@Slug", post.Slug),
                    dbHelper.CreateParameter("@Content", post.Content),
                    dbHelper.CreateParameter("@Excerpt", post.Excerpt),
                    dbHelper.CreateParameter("@FeaturedImage", post.FeaturedImage),
                    dbHelper.CreateParameter("@Author", post.Author),
                    dbHelper.CreateParameter("@Category", post.Category),
                    dbHelper.CreateParameter("@Tags", post.Tags),
                    dbHelper.CreateParameter("@MetaTitle", post.MetaTitle),
                    dbHelper.CreateParameter("@MetaDescription", post.MetaDescription),
                    dbHelper.CreateParameter("@MetaKeywords", post.MetaKeywords),
                    dbHelper.CreateParameter("@IsPublished", post.IsPublished),
                    dbHelper.CreateParameter("@PublishDate", post.PublishDate),
                    dbHelper.CreateParameter("@UpdatedBy", post.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateBlogPost", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating blog post: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a blog post
        /// </summary>
        public bool DeleteBlogPost(int blogPostID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BlogPostID", blogPostID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteBlogPost", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting blog post: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Increments the view count for a blog post
        /// </summary>
        public bool IncrementViewCount(int blogPostID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@BlogPostID", blogPostID)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_IncrementBlogViewCount", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error incrementing view count: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a BlogPost object
        /// </summary>
        private BlogPost MapDataRowToBlogPost(DataRow row)
        {
            return new BlogPost
            {
                BlogPostID = Convert.ToInt32(row["BlogPostID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                Title = row["Title"].ToString(),
                Slug = row["Slug"].ToString(),
                Content = row["Content"]?.ToString(),
                Excerpt = row["Excerpt"]?.ToString(),
                FeaturedImage = row["FeaturedImage"]?.ToString(),
                Author = row["Author"]?.ToString(),
                Category = row["Category"]?.ToString(),
                Tags = row["Tags"]?.ToString(),
                MetaTitle = row["MetaTitle"]?.ToString(),
                MetaDescription = row["MetaDescription"]?.ToString(),
                MetaKeywords = row["MetaKeywords"]?.ToString(),
                IsPublished = Convert.ToBoolean(row["IsPublished"]),
                PublishDate = row["PublishDate"] != DBNull.Value ? Convert.ToDateTime(row["PublishDate"]) : (DateTime?)null,
                ViewCount = Convert.ToInt32(row["ViewCount"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"])
            };
        }

        #endregion
    }
}