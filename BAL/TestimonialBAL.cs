// ============================================
// BAL/TestimonialBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Testimonial BAL - Business logic layer for testimonials
    /// </summary>
    public class TestimonialBAL
    {
        #region Private Fields

        private readonly TestimonialDAL testimonialDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public TestimonialBAL()
        {
            testimonialDAL = new TestimonialDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all testimonials with filters
        /// </summary>
        public List<Testimonial> GetTestimonials(int companyID, int? branchID = null, bool? isApproved = null, bool? isFeatured = null, int limit = 100)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            return testimonialDAL.GetTestimonials(companyID, branchID, isApproved, isFeatured, limit);
        }

        /// <summary>
        /// Gets approved testimonials
        /// </summary>
        public List<Testimonial> GetApprovedTestimonials(int companyID, int? branchID = null, bool? isFeatured = null, int limit = 100)
        {
            return GetTestimonials(companyID, branchID, true, isFeatured, limit);
        }

        /// <summary>
        /// Gets featured testimonials
        /// </summary>
        public List<Testimonial> GetFeaturedTestimonials(int companyID, int? branchID = null, int limit = 10)
        {
            return GetApprovedTestimonials(companyID, branchID, true, limit);
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a testimonial
        /// </summary>
        public int SaveTestimonial(Testimonial testimonial)
        {
            ValidateTestimonial(testimonial);

            int result;

            if (testimonial.TestimonialID > 0)
            {
                bool updated = testimonialDAL.UpdateTestimonial(testimonial);
                result = testimonial.TestimonialID;
                if (updated)
                {
                    auditLogger.LogUpdate("Testimonials", "Testimonials", testimonial.TestimonialID,
                        $"Customer: {testimonial.CustomerName}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = testimonialDAL.InsertTestimonial(testimonial);
                if (result > 0)
                {
                    auditLogger.LogInsert("Testimonials", "Testimonials", result,
                        $"Customer: {testimonial.CustomerName}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a testimonial
        /// </summary>
        public bool DeleteTestimonial(int testimonialID, int updatedBy)
        {
            if (testimonialID <= 0)
                throw new ArgumentException("Invalid testimonial ID.");

            bool deleted = testimonialDAL.DeleteTestimonial(testimonialID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("Testimonials", "Testimonials", testimonialID,
                    $"TestimonialID: {testimonialID}");
            }

            return deleted;
        }

        /// <summary>
        /// Approves a testimonial
        /// </summary>
        public bool ApproveTestimonial(int testimonialID, int updatedBy)
        {
            if (testimonialID <= 0)
                throw new ArgumentException("Invalid testimonial ID.");

            bool approved = testimonialDAL.ApproveTestimonial(testimonialID, updatedBy);

            if (approved)
            {
                auditLogger.LogUpdate("Testimonials", "Testimonials", testimonialID,
                    $"TestimonialID: {testimonialID}", "Status: Approved");
            }

            return approved;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates a testimonial
        /// </summary>
        private void ValidateTestimonial(Testimonial testimonial)
        {
            if (testimonial == null)
                throw new ArgumentNullException(nameof(testimonial));

            if (testimonial.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(testimonial.CustomerName))
                throw new ArgumentException("Customer name is required.");

            if (testimonial.CustomerName.Length > 200)
                throw new ArgumentException("Customer name cannot exceed 200 characters.");

            if (string.IsNullOrWhiteSpace(testimonial.Content))
                throw new ArgumentException("Content is required.");

            if (testimonial.Rating < 1 || testimonial.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            if (testimonial.CustomerDesignation != null && testimonial.CustomerDesignation.Length > 100)
                throw new ArgumentException("Designation cannot exceed 100 characters.");
        }

        #endregion
    }
}