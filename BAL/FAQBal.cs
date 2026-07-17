// ============================================
// BAL/FAQBal.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// FAQ BAL - Business logic layer for frequently asked questions
    /// </summary>
    public class FAQBAL
    {
        #region Private Fields

        private readonly FAQDAL faqDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public FAQBAL()
        {
            faqDAL = new FAQDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all FAQs with filters
        /// </summary>
        public List<FAQ> GetFAQs(int companyID, int? branchID = null, string category = null, bool? isFeatured = null, int limit = 100)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            return faqDAL.GetFAQs(companyID, branchID, category, isFeatured, limit);
        }

        /// <summary>
        /// Gets FAQs by category
        /// </summary>
        public List<FAQ> GetFAQsByCategory(int companyID, string category, int? branchID = null, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category is required.");

            return GetFAQs(companyID, branchID, category, null, limit);
        }

        /// <summary>
        /// Gets featured FAQs
        /// </summary>
        public List<FAQ> GetFeaturedFAQs(int companyID, int? branchID = null, int limit = 10)
        {
            return GetFAQs(companyID, branchID, null, true, limit);
        }

        /// <summary>
        /// Gets FAQ categories
        /// </summary>
        public List<string> GetFAQCategories(int companyID, int? branchID = null)
        {
            var faqs = GetFAQs(companyID, branchID);
            var categories = new List<string>();

            foreach (var faq in faqs)
            {
                if (!string.IsNullOrEmpty(faq.Category) && !categories.Contains(faq.Category))
                {
                    categories.Add(faq.Category);
                }
            }

            return categories;
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves an FAQ
        /// </summary>
        public int SaveFAQ(FAQ faq)
        {
            ValidateFAQ(faq);

            int result;

            if (faq.FAQID > 0)
            {
                bool updated = faqDAL.UpdateFAQ(faq);
                result = faq.FAQID;
                if (updated)
                {
                    auditLogger.LogUpdate("FAQs", "FAQs", faq.FAQID,
                        $"Question: {faq.Question}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = faqDAL.InsertFAQ(faq);
                if (result > 0)
                {
                    auditLogger.LogInsert("FAQs", "FAQs", result,
                        $"Question: {faq.Question}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes an FAQ
        /// </summary>
        public bool DeleteFAQ(int faqID, int updatedBy)
        {
            if (faqID <= 0)
                throw new ArgumentException("Invalid FAQ ID.");

            bool deleted = faqDAL.DeleteFAQ(faqID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("FAQs", "FAQs", faqID,
                    $"FAQID: {faqID}");
            }

            return deleted;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates an FAQ
        /// </summary>
        private void ValidateFAQ(FAQ faq)
        {
            if (faq == null)
                throw new ArgumentNullException(nameof(faq));

            if (faq.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(faq.Question))
                throw new ArgumentException("Question is required.");

            if (faq.Question.Length > 500)
                throw new ArgumentException("Question cannot exceed 500 characters.");

            if (string.IsNullOrWhiteSpace(faq.Answer))
                throw new ArgumentException("Answer is required.");

            if (faq.Category != null && faq.Category.Length > 50)
                throw new ArgumentException("Category cannot exceed 50 characters.");
        }

        #endregion
    }
}