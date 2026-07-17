// ============================================
// Models/Slider.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Slider Model - Represents a homepage slider item
    /// </summary>
    public class Slider
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the slider
        /// </summary>
        public int SliderID { get; set; }

        /// <summary>
        /// Company ID this slider belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this slider belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Title of the slider
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Subtitle of the slider
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Description of the slider
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Path to the slider image
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Alt text for the image
        /// </summary>
        public string ImageAltText { get; set; }

        /// <summary>
        /// Text for the button
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// URL for the button link
        /// </summary>
        public string ButtonLink { get; set; }

        /// <summary>
        /// Target for the button link (_self, _blank)
        /// </summary>
        public string ButtonTarget { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether the slider is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Whether the slider is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Start date of the slider
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date of the slider
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Status of the slider
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the slider was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the slider was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the slider
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the slider
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the slider image URL
        /// </summary>
        public string ImageUrl
        {
            get { return string.IsNullOrEmpty(ImagePath) ? "/Assets/Images/default-slider.jpg" : ImagePath; }
        }

        /// <summary>
        /// Whether the slider is currently valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (!IsActive || !Status)
                    return false;

                if (StartDate.HasValue && StartDate.Value > DateTime.Now)
                    return false;

                if (EndDate.HasValue && EndDate.Value < DateTime.Now)
                    return false;

                return true;
            }
        }

        #endregion
    }
}