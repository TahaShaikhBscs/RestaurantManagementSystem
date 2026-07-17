// ============================================
// Models/Testimonial.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Testimonial Model - Represents a customer testimonial on the website
    /// </summary>
    public class Testimonial
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the testimonial
        /// </summary>
        public int TestimonialID { get; set; }

        /// <summary>
        /// Company ID this testimonial belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this testimonial belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Name of the customer
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Path to the customer's image
        /// </summary>
        public string CustomerImage { get; set; }

        /// <summary>
        /// Designation of the customer (e.g., Food Critic, Regular Customer)
        /// </summary>
        public string CustomerDesignation { get; set; }

        /// <summary>
        /// Content of the testimonial
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Rating (1-5 stars)
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether this testimonial is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Whether this testimonial is approved
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Status of the testimonial
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the testimonial was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the testimonial was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the testimonial
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the testimonial
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the star rating as HTML
        /// </summary>
        public string StarRatingHTML
        {
            get
            {
                string stars = string.Empty;
                for (int i = 1; i <= 5; i++)
                {
                    if (i <= Rating)
                        stars += "<i class='fas fa-star text-warning'></i>";
                    else
                        stars += "<i class='far fa-star text-muted'></i>";
                }
                return stars;
            }
        }

        /// <summary>
        /// Gets the customer image URL
        /// </summary>
        public string CustomerImageUrl
        {
            get { return string.IsNullOrEmpty(CustomerImage) ? "/Assets/Images/default-avatar.jpg" : CustomerImage; }
        }

        #endregion
    }
}