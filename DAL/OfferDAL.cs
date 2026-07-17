// ============================================
// DAL/OfferDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Offer DAL - Data access layer for promotional offers
    /// </summary>
    public class OfferDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public OfferDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all offers with filters
        /// </summary>
        public List<Offer> GetOffers(int companyID, int? branchID = null, bool? isActive = null, bool? isFeatured = null, int limit = 100)
        {
            try
            {
                List<Offer> offers = new List<Offer>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IsActive", isActive),
                    dbHelper.CreateParameter("@IsFeatured", isFeatured),
                    dbHelper.CreateParameter("@Limit", limit)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetOffers", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    offers.Add(MapDataRowToOffer(row));
                }

                return offers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting offers: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new offer
        /// </summary>
        public int InsertOffer(Offer offer)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", offer.CompanyID),
                    dbHelper.CreateParameter("@BranchID", offer.BranchID),
                    dbHelper.CreateParameter("@Title", offer.Title),
                    dbHelper.CreateParameter("@Description", offer.Description),
                    dbHelper.CreateParameter("@OfferCode", offer.OfferCode),
                    dbHelper.CreateParameter("@DiscountType", offer.DiscountType),
                    dbHelper.CreateParameter("@DiscountValue", offer.DiscountValue),
                    dbHelper.CreateParameter("@MinOrderAmount", offer.MinOrderAmount),
                    dbHelper.CreateParameter("@MaxDiscount", offer.MaxDiscount),
                    dbHelper.CreateParameter("@ImagePath", offer.ImagePath),
                    dbHelper.CreateParameter("@StartDate", offer.StartDate),
                    dbHelper.CreateParameter("@EndDate", offer.EndDate),
                    dbHelper.CreateParameter("@IsActive", offer.IsActive),
                    dbHelper.CreateParameter("@IsFeatured", offer.IsFeatured),
                    dbHelper.CreateParameter("@UsageLimit", offer.UsageLimit),
                    dbHelper.CreateParameter("@TermsAndConditions", offer.TermsAndConditions),
                    dbHelper.CreateParameter("@DisplayOrder", offer.DisplayOrder),
                    dbHelper.CreateParameter("@CreatedBy", offer.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertOffer", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting offer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing offer
        /// </summary>
        public bool UpdateOffer(Offer offer)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OfferID", offer.OfferID),
                    dbHelper.CreateParameter("@Title", offer.Title),
                    dbHelper.CreateParameter("@Description", offer.Description),
                    dbHelper.CreateParameter("@OfferCode", offer.OfferCode),
                    dbHelper.CreateParameter("@DiscountType", offer.DiscountType),
                    dbHelper.CreateParameter("@DiscountValue", offer.DiscountValue),
                    dbHelper.CreateParameter("@MinOrderAmount", offer.MinOrderAmount),
                    dbHelper.CreateParameter("@MaxDiscount", offer.MaxDiscount),
                    dbHelper.CreateParameter("@ImagePath", offer.ImagePath),
                    dbHelper.CreateParameter("@StartDate", offer.StartDate),
                    dbHelper.CreateParameter("@EndDate", offer.EndDate),
                    dbHelper.CreateParameter("@IsActive", offer.IsActive),
                    dbHelper.CreateParameter("@IsFeatured", offer.IsFeatured),
                    dbHelper.CreateParameter("@UsageLimit", offer.UsageLimit),
                    dbHelper.CreateParameter("@TermsAndConditions", offer.TermsAndConditions),
                    dbHelper.CreateParameter("@DisplayOrder", offer.DisplayOrder),
                    dbHelper.CreateParameter("@UpdatedBy", offer.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateOffer", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating offer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes an offer
        /// </summary>
        public bool DeleteOffer(int offerID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@OfferID", offerID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteOffer", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting offer: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to an Offer object
        /// </summary>
        private Offer MapDataRowToOffer(DataRow row)
        {
            return new Offer
            {
                OfferID = Convert.ToInt32(row["OfferID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                Title = row["Title"].ToString(),
                Description = row["Description"]?.ToString(),
                OfferCode = row["OfferCode"]?.ToString(),
                DiscountType = row["DiscountType"]?.ToString(),
                DiscountValue = Convert.ToDecimal(row["DiscountValue"]),
                MinOrderAmount = Convert.ToDecimal(row["MinOrderAmount"]),
                MaxDiscount = Convert.ToDecimal(row["MaxDiscount"]),
                ImagePath = row["ImagePath"]?.ToString(),
                StartDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : (DateTime?)null,
                EndDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : (DateTime?)null,
                IsActive = Convert.ToBoolean(row["IsActive"]),
                IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
                UsageLimit = Convert.ToInt32(row["UsageLimit"]),
                UsedCount = Convert.ToInt32(row["UsedCount"]),
                TermsAndConditions = row["TermsAndConditions"]?.ToString(),
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        #endregion
    }
}