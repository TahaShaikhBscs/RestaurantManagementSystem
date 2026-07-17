// ============================================
// DAL/GalleryDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Gallery DAL - Data access layer for gallery images
    /// </summary>
    public class GalleryDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public GalleryDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all gallery images with filters
        /// </summary>
        public List<GalleryImage> GetGalleryImages(int companyID, int? branchID = null, string category = null, int limit = 100, int offset = 0)
        {
            try
            {
                List<GalleryImage> images = new List<GalleryImage>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Category", category),
                    dbHelper.CreateParameter("@Limit", limit),
                    dbHelper.CreateParameter("@Offset", offset)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetGalleryImages", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    images.Add(MapDataRowToGalleryImage(row));
                }

                return images;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting gallery images: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new gallery image
        /// </summary>
        public int InsertGalleryImage(GalleryImage image)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", image.CompanyID),
                    dbHelper.CreateParameter("@BranchID", image.BranchID),
                    dbHelper.CreateParameter("@Title", image.Title),
                    dbHelper.CreateParameter("@Description", image.Description),
                    dbHelper.CreateParameter("@ImagePath", image.ImagePath),
                    dbHelper.CreateParameter("@ThumbnailPath", image.ThumbnailPath),
                    dbHelper.CreateParameter("@Category", image.Category),
                    dbHelper.CreateParameter("@DisplayOrder", image.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", image.IsFeatured),
                    dbHelper.CreateParameter("@AltText", image.AltText),
                    dbHelper.CreateParameter("@CreatedBy", image.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertGalleryImage", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting gallery image: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing gallery image
        /// </summary>
        public bool UpdateGalleryImage(GalleryImage image)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@GalleryImageID", image.GalleryImageID),
                    dbHelper.CreateParameter("@Title", image.Title),
                    dbHelper.CreateParameter("@Description", image.Description),
                    dbHelper.CreateParameter("@ImagePath", image.ImagePath),
                    dbHelper.CreateParameter("@ThumbnailPath", image.ThumbnailPath),
                    dbHelper.CreateParameter("@Category", image.Category),
                    dbHelper.CreateParameter("@DisplayOrder", image.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", image.IsFeatured),
                    dbHelper.CreateParameter("@AltText", image.AltText),
                    dbHelper.CreateParameter("@UpdatedBy", image.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateGalleryImage", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating gallery image: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a gallery image
        /// </summary>
        public bool DeleteGalleryImage(int galleryImageID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@GalleryImageID", galleryImageID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteGalleryImage", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting gallery image: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a GalleryImage object
        /// </summary>
        private GalleryImage MapDataRowToGalleryImage(DataRow row)
        {
            return new GalleryImage
            {
                GalleryImageID = Convert.ToInt32(row["GalleryImageID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                Title = row["Title"]?.ToString(),
                Description = row["Description"]?.ToString(),
                ImagePath = row["ImagePath"].ToString(),
                ThumbnailPath = row["ThumbnailPath"]?.ToString(),
                Category = row["Category"]?.ToString(),
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
                AltText = row["AltText"]?.ToString(),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        #endregion
    }
}