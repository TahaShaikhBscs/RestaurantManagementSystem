// ============================================
// Models/Chef.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Chef Model - Represents a chef profile on the website
    /// </summary>
    public class Chef
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the chef
        /// </summary>
        public int ChefID { get; set; }

        /// <summary>
        /// Company ID this chef belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this chef belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Name of the chef
        /// </summary>
        public string ChefName { get; set; }

        /// <summary>
        /// Designation of the chef (e.g., Executive Chef, Sous Chef)
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Biography of the chef
        /// </summary>
        public string Biography { get; set; }

        /// <summary>
        /// Path to the chef's image
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Facebook social media URL
        /// </summary>
        public string SocialFacebook { get; set; }

        /// <summary>
        /// Twitter social media URL
        /// </summary>
        public string SocialTwitter { get; set; }

        /// <summary>
        /// Instagram social media URL
        /// </summary>
        public string SocialInstagram { get; set; }

        /// <summary>
        /// LinkedIn social media URL
        /// </summary>
        public string SocialLinkedIn { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether this chef is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Status of the chef
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the chef was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the chef was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the chef
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the chef
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the chef image URL
        /// </summary>
        public string ImageUrl
        {
            get { return string.IsNullOrEmpty(ImagePath) ? "/Assets/Images/default-chef.jpg" : ImagePath; }
        }

        /// <summary>
        /// Gets the display name (Name - Designation)
        /// </summary>
        public string DisplayName
        {
            get { return string.IsNullOrEmpty(Designation) ? ChefName : $"{ChefName} - {Designation}"; }
        }

        /// <summary>
        /// Gets the full social media links HTML
        /// </summary>
        public string SocialLinksHTML
        {
            get
            {
                string html = string.Empty;

                if (!string.IsNullOrEmpty(SocialFacebook))
                    html += $"<a href='{SocialFacebook}' target='_blank' class='me-2'><i class='fab fa-facebook-f'></i></a>";

                if (!string.IsNullOrEmpty(SocialTwitter))
                    html += $"<a href='{SocialTwitter}' target='_blank' class='me-2'><i class='fab fa-twitter'></i></a>";

                if (!string.IsNullOrEmpty(SocialInstagram))
                    html += $"<a href='{SocialInstagram}' target='_blank' class='me-2'><i class='fab fa-instagram'></i></a>";

                if (!string.IsNullOrEmpty(SocialLinkedIn))
                    html += $"<a href='{SocialLinkedIn}' target='_blank' class='me-2'><i class='fab fa-linkedin-in'></i></a>";

                return html;
            }
        }

        #endregion
    }
}