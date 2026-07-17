// ============================================
// BAL/OfferBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Offer BAL - Business logic layer for promotional offers
    /// </summary>
    public class OfferBAL
    {
        #region Private Fields

        private readonly OfferDAL offerDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public OfferBAL()
        {
            offerDAL = new OfferDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all offers with filters
        /// </summary>
        public List<Offer> GetOffers(int companyID, int? branchID = null, bool? isActive = null, bool? isFeatured = null, int limit = 100)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            return offerDAL.GetOffers(companyID, branchID, isActive, isFeatured, limit);
        }

        /// <summary>
        /// Gets active offers
        /// </summary>
        public List<Offer> GetActiveOffers(int companyID, int? branchID = null, bool? isFeatured = null, int limit = 100)
        {
            return GetOffers(companyID, branchID, true, isFeatured, limit);
        }

        /// <summary>
        /// Gets featured offers
        /// </summary>
        public List<Offer> GetFeaturedOffers(int companyID, int? branchID = null, int limit = 10)
        {
            return GetActiveOffers(companyID, branchID, true, limit);
        }

        /// <summary>
        /// Validates an offer code
        /// </summary>
        public Offer ValidateOfferCode(int companyID, string offerCode, decimal orderAmount)
        {
            if (string.IsNullOrWhiteSpace(offerCode))
                return null;

            var offers = GetActiveOffers(companyID);

            foreach (var offer in offers)
            {
                if (offer.OfferCode.Equals(offerCode, StringComparison.OrdinalIgnoreCase))
                {
                    // Check if offer is valid
                    if (!offer.IsValid)
                        continue;

                    // Check minimum order amount
                    if (offer.MinOrderAmount > 0 && orderAmount < offer.MinOrderAmount)
                        continue;

                    // Check usage limit
                    if (offer.UsageLimit > 0 && offer.UsedCount >= offer.UsageLimit)
                        continue;

                    return offer;
                }
            }

            return null;
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves an offer
        /// </summary>
        public int SaveOffer(Offer offer)
        {
            ValidateOffer(offer);

            int result;

            if (offer.OfferID > 0)
            {
                bool updated = offerDAL.UpdateOffer(offer);
                result = offer.OfferID;
                if (updated)
                {
                    auditLogger.LogUpdate("Offers", "Offers", offer.OfferID,
                        $"Title: {offer.Title}", $"Updated: {DateTime.Now}");
                }
            }
            else
            {
                result = offerDAL.InsertOffer(offer);
                if (result > 0)
                {
                    auditLogger.LogInsert("Offers", "Offers", result,
                        $"Title: {offer.Title}");
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes an offer
        /// </summary>
        public bool DeleteOffer(int offerID, int updatedBy)
        {
            if (offerID <= 0)
                throw new ArgumentException("Invalid offer ID.");

            bool deleted = offerDAL.DeleteOffer(offerID, updatedBy);

            if (deleted)
            {
                auditLogger.LogDelete("Offers", "Offers", offerID,
                    $"OfferID: {offerID}");
            }

            return deleted;
        }

        /// <summary>
        /// Increments the usage count for an offer
        /// </summary>
        public bool IncrementOfferUsage(int offerID)
        {
            if (offerID <= 0)
                throw new ArgumentException("Invalid offer ID.");

            var offer = GetOfferById(offerID);
            if (offer == null)
                throw new Exception("Offer not found.");

            offer.UsedCount++;
            return offerDAL.UpdateOffer(offer);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets an offer by ID
        /// </summary>
        private Offer GetOfferById(int offerID)
        {
            var offers = GetOffers(1, null, null, null, 1000);
            return offers.Find(o => o.OfferID == offerID);
        }

        /// <summary>
        /// Validates an offer
        /// </summary>
        private void ValidateOffer(Offer offer)
        {
            if (offer == null)
                throw new ArgumentNullException(nameof(offer));

            if (offer.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(offer.Title))
                throw new ArgumentException("Title is required.");

            if (offer.Title.Length > 200)
                throw new ArgumentException("Title cannot exceed 200 characters.");

            if (offer.DiscountValue <= 0)
                throw new ArgumentException("Discount value must be greater than 0.");

            if (offer.DiscountType != "Percentage" && offer.DiscountType != "Fixed")
                throw new ArgumentException("Discount type must be 'Percentage' or 'Fixed'.");

            if (offer.DiscountType == "Percentage" && offer.DiscountValue > 100)
                throw new ArgumentException("Percentage discount cannot exceed 100%.");

            if (offer.MinOrderAmount < 0)
                throw new ArgumentException("Minimum order amount cannot be negative.");

            if (offer.MaxDiscount < 0)
                throw new ArgumentException("Maximum discount cannot be negative.");

            if (offer.UsageLimit < 0)
                throw new ArgumentException("Usage limit cannot be negative.");

            if (offer.StartDate.HasValue && offer.EndDate.HasValue && offer.StartDate > offer.EndDate)
                throw new ArgumentException("Start date cannot be after end date.");
        }

        #endregion
    }
}