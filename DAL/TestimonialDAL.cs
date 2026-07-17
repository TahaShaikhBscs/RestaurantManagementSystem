// ============================================
// DAL/TestimonialDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Testimonial DAL - Data access layer for testimonials
    /// </summary>
    public class TestimonialDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public TestimonialDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all testimonials with filters
        /// </summary>
        public List<Testimonial> GetTestimonials(int companyID, int? branchID = null, bool? isApproved = null, bool? isFeatured = null, int limit = 100)
        {
            try
            {
                List<Testimonial> testimonials = new List<Testimonial>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IsApproved", isApproved),
                    dbHelper.CreateParameter("@IsFeatured", isFeatured),
                    dbHelper.CreateParameter("@Limit", limit)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetTestimonials", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    testimonials.Add(MapDataRowToTestimonial(row));
                }

                return testimonials;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting testimonials: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new testimonial
        /// </summary>
        public int InsertTestimonial(Testimonial testimonial)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", testimonial.CompanyID),
                    dbHelper.CreateParameter("@BranchID", testimonial.BranchID),
                    dbHelper.CreateParameter("@CustomerName", testimonial.CustomerName),
                    dbHelper.CreateParameter("@CustomerImage", testimonial.CustomerImage),
                    dbHelper.CreateParameter("@CustomerDesignation", testimonial.CustomerDesignation),
                    dbHelper.CreateParameter("@Content", testimonial.Content),
                    dbHelper.CreateParameter("@Rating", testimonial.Rating),
                    dbHelper.CreateParameter("@DisplayOrder", testimonial.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", testimonial.IsFeatured),
                    dbHelper.CreateParameter("@IsApproved", testimonial.IsApproved),
                    dbHelper.CreateParameter("@CreatedBy", testimonial.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertTestimonial", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting testimonial: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing testimonial
        /// </summary>
        public bool UpdateTestimonial(Testimonial testimonial)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TestimonialID", testimonial.TestimonialID),
                    dbHelper.CreateParameter("@CustomerName", testimonial.CustomerName),
                    dbHelper.CreateParameter("@CustomerImage", testimonial.CustomerImage),
                    dbHelper.CreateParameter("@CustomerDesignation", testimonial.CustomerDesignation),
                    dbHelper.CreateParameter("@Content", testimonial.Content),
                    dbHelper.CreateParameter("@Rating", testimonial.Rating),
                    dbHelper.CreateParameter("@DisplayOrder", testimonial.DisplayOrder),
                    dbHelper.CreateParameter("@IsFeatured", testimonial.IsFeatured),
                    dbHelper.CreateParameter("@IsApproved", testimonial.IsApproved),
                    dbHelper.CreateParameter("@UpdatedBy", testimonial.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateTestimonial", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating testimonial: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a testimonial
        /// </summary>
        public bool DeleteTestimonial(int testimonialID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TestimonialID", testimonialID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteTestimonial", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting testimonial: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Approves a testimonial
        /// </summary>
        public bool ApproveTestimonial(int testimonialID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@TestimonialID", testimonialID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_ApproveTestimonial", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving testimonial: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Testimonial object
        /// </summary>
        private Testimonial MapDataRowToTestimonial(DataRow row)
        {
            return new Testimonial
            {
                TestimonialID = Convert.ToInt32(row["TestimonialID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                CustomerName = row["CustomerName"].ToString(),
                CustomerImage = row["CustomerImage"]?.ToString(),
                CustomerDesignation = row["CustomerDesignation"]?.ToString(),
                Content = row["Content"].ToString(),
                Rating = Convert.ToInt32(row["Rating"]),
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
                IsApproved = Convert.ToBoolean(row["IsApproved"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        #endregion
    }
}