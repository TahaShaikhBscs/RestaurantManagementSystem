// ============================================
// BAL/GalleryBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Gallery BAL - Business logic layer for gallery images
    /// </summary>
    public class GalleryBAL
    {
        #region Private Fields

        private readonly GalleryDAL galleryDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public GalleryBAL()
        {
            galleryDAL = new GalleryDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all gallery images with filters
        /// </summary>
        public List<GalleryImage> GetGalleryImages(int companyID, int? branchID = null, string category = null, int limit = 100, int offset = 0)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            if (offset < 0)
                throw new ArgumentException("Offset cannot be negative.");

            return galleryDAL.GetGalleryImages(companyID, branchID, category, limit, offset);
        }

        /// <summary>
        /// Gets featured gallery images
        /// </summary>
        public List<GalleryImage> GetFeaturedGalleryImages(int companyID, int? branchID = null, int limit = 10)
        {
            var images = GetGalleryImages(companyID, branchID, null, limit);
            return images.FindAll(i => i.IsFeatured);
        }

        /// <summary>
        /// Gets gallery images by category
        /// </summary>
        public List<GalleryImage> GetGalleryImagesByCategory(int companyID, string category, int? branchID = null, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category is required.");

            return GetGalleryImages(companyID, branchID, category, limit);
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a gallery image
        /// </summary>
        public int SaveGalleryImage(GalleryImage image)
        {
            ValidateGalleryImage(image);

            int result;

            if (image.GalleryImageID > 0)
            {
                bool updated = galleryDAL.UpdateGalleryImage(image);
                result = image.GalleryImageID;
                if (updated)
                {
                    auditLogger.LogUpdate("Gallery", "GalleryImages", image.GalleryImageID,
                        $"Title: {image.Title}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = galleryDAL.InsertGalleryImage(image);
                if (result > 0)
                {
                    auditLogger.LogInsert("Gallery", "GalleryImages", result,
                        $"Title: {image.Title}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a gallery image
        /// </summary>
        public bool DeleteGalleryImage(int galleryImageID, int updatedBy)
        {
            if (galleryImageID <= 0)
                throw new ArgumentException("Invalid gallery image ID.");

            bool deleted = galleryDAL.DeleteGalleryImage(galleryImageID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("Gallery", "GalleryImages", galleryImageID,
                    $"ImageID: {galleryImageID}");
            }

            return deleted;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates a gallery image
        /// </summary>
        private void ValidateGalleryImage(GalleryImage image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (image.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(image.ImagePath))
                throw new ArgumentException("Image path is required.");

            if (image.Title != null && image.Title.Length > 200)
                throw new ArgumentException("Title cannot exceed 200 characters.");

            if (image.AltText != null && image.AltText.Length > 200)
                throw new ArgumentException("Alt text cannot exceed 200 characters.");

            if (image.Description != null && image.Description.Length > 500)
                throw new ArgumentException("Description cannot exceed 500 characters.");
        }

        #endregion
    }
}