// ============================================
// BAL/SliderBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Slider BAL - Business logic layer for homepage sliders
    /// </summary>
    public class SliderBAL
    {
        #region Private Fields

        private readonly SliderDAL sliderDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public SliderBAL()
        {
            sliderDAL = new SliderDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all sliders with filters
        /// </summary>
        public List<Slider> GetSliders(int companyID, int? branchID = null, bool? isActive = null, bool? isFeatured = null, int limit = 100)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            return sliderDAL.GetSliders(companyID, branchID, isActive, isFeatured, limit);
        }

        /// <summary>
        /// Gets active sliders
        /// </summary>
        public List<Slider> GetActiveSliders(int companyID, int? branchID = null, bool? isFeatured = null, int limit = 100)
        {
            return GetSliders(companyID, branchID, true, isFeatured, limit);
        }

        /// <summary>
        /// Gets featured sliders
        /// </summary>
        public List<Slider> GetFeaturedSliders(int companyID, int? branchID = null, int limit = 10)
        {
            return GetActiveSliders(companyID, branchID, true, limit);
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a slider
        /// </summary>
        public int SaveSlider(Slider slider)
        {
            ValidateSlider(slider);

            int result;

            if (slider.SliderID > 0)
            {
                bool updated = sliderDAL.UpdateSlider(slider);
                result = slider.SliderID;
                if (updated)
                {
                    auditLogger.LogUpdate("Sliders", "Sliders", slider.SliderID,
                        $"Title: {slider.Title}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = sliderDAL.InsertSlider(slider);
                if (result > 0)
                {
                    auditLogger.LogInsert("Sliders", "Sliders", result,
                        $"Title: {slider.Title}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a slider
        /// </summary>
        public bool DeleteSlider(int sliderID, int updatedBy)
        {
            if (sliderID <= 0)
                throw new ArgumentException("Invalid slider ID.");

            bool deleted = sliderDAL.DeleteSlider(sliderID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("Sliders", "Sliders", sliderID,
                    $"SliderID: {sliderID}");
            }

            return deleted;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates a slider
        /// </summary>
        private void ValidateSlider(Slider slider)
        {
            if (slider == null)
                throw new ArgumentNullException(nameof(slider));

            if (slider.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(slider.ImagePath))
                throw new ArgumentException("Image path is required.");

            if (slider.Title != null && slider.Title.Length > 200)
                throw new ArgumentException("Title cannot exceed 200 characters.");

            if (slider.Subtitle != null && slider.Subtitle.Length > 500)
                throw new ArgumentException("Subtitle cannot exceed 500 characters.");

            if (slider.ButtonText != null && slider.ButtonText.Length > 50)
                throw new ArgumentException("Button text cannot exceed 50 characters.");

            if (slider.ButtonLink != null && slider.ButtonLink.Length > 500)
                throw new ArgumentException("Button link cannot exceed 500 characters.");

            if (slider.ButtonTarget != "_self" && slider.ButtonTarget != "_blank")
                throw new ArgumentException("Button target must be '_self' or '_blank'.");

            if (slider.StartDate.HasValue && slider.EndDate.HasValue && slider.StartDate > slider.EndDate)
                throw new ArgumentException("Start date cannot be after end date.");
        }

        #endregion
    }
}