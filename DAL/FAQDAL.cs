// ============================================
// DAL/FAQDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// FAQ DAL - Data access layer for frequently asked questions
    /// </summary>
    public class FAQDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public FAQDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all FAQs with filters
        /// </summary>
        public List<FAQ> GetFAQs(int companyID, int? branchID = null, string category = null, bool? isFeatured = null, int limit = 100)
        {
            try
            {
                List<FAQ> faqs = new List<FAQ>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Category", category),
                    dbHelper.CreateParameter("@IsFeatured", isFeatured),
                    dbHelper.CreateParameter("@Limit", limit)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetFAQs", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    faqs.Add(MapDataRowToFAQ(row));
                }

                return faqs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting FAQs: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new FAQ
        /// </summary>
        public int InsertFAQ(FAQ faq)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", faq.CompanyID),
                    dbHelper.CreateParameter("@BranchID", faq.BranchID),
                    dbHelper.CreateParameter("@Question", faq.Question),
                    dbHelper.CreateParameter("@Answer", faq.Answer),
                    dbHelper.CreateParameter("@Category", faq.Category),
                    dbHelper.CreateParameter("@DisplayOrder", faq.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", faq.IsFeatured),
                    dbHelper.CreateParameter("@CreatedBy", faq.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertFAQ", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting FAQ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing FAQ
        /// </summary>
        public bool UpdateFAQ(FAQ faq)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@FAQID", faq.FAQID),
                    dbHelper.CreateParameter("@Question", faq.Question),
                    dbHelper.CreateParameter("@Answer", faq.Answer),
                    dbHelper.CreateParameter("@Category", faq.Category),
                    dbHelper.CreateParameter("@DisplayOrder", faq.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", faq.IsFeatured),
                    dbHelper.CreateParameter("@UpdatedBy", faq.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateFAQ", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating FAQ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes an FAQ
        /// </summary>
        public bool DeleteFAQ(int faqID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@FAQID", faqID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteFAQ", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting FAQ: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a FAQ object
        /// </summary>
        private FAQ MapDataRowToFAQ(DataRow row)
        {
            return new FAQ
            {
                FAQID = Convert.ToInt32(row["FAQID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                Question = row["Question"].ToString(),
                Answer = row["Answer"].ToString(),
                Category = row["Category"]?.ToString(),
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        #endregion
    }
}