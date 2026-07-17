// ============================================
// Models/GalleryImage.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Gallery Image Model - Represents an image in the website gallery
    /// </summary>
    public class GalleryImage
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the gallery image
        /// </summary>
        public int GalleryImageID { get; set; }

        /// <summary>
        /// Company ID this gallery image belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this gallery image belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Title of the image
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the image
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Path to the full-size image
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Path to the thumbnail image
        /// </summary>
        public string ThumbnailPath { get; set; }

        /// <summary>
        /// Category of the image (e.g., Food, Interior, Events)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether this image is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Alt text for accessibility
        /// </summary>
        public string AltText { get; set; }

        /// <summary>
        /// Status of the gallery image
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the image was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the image was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the image
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the image
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the full image URL
        /// </summary>
        public string ImageUrl
        {
            get { return string.IsNullOrEmpty(ImagePath) ? "/Assets/Images/default-placeholder.jpg" : ImagePath; }
        }

        /// <summary>
        /// Gets the thumbnail URL
        /// </summary>
        public string ThumbnailUrl
        {
            get { return string.IsNullOrEmpty(ThumbnailPath) ? ImageUrl : ThumbnailPath; }
        }

        #endregion
    }
}