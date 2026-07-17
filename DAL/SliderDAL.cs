// ============================================
// DAL/SliderDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Slider DAL - Data access layer for homepage sliders
    /// </summary>
    public class SliderDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public SliderDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all sliders with filters
        /// </summary>
        public List<Slider> GetSliders(int companyID, int? branchID = null, bool? isActive = null, bool? isFeatured = null, int limit = 100)
        {
            try
            {
                List<Slider> sliders = new List<Slider>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@IsActive", isActive),
                    dbHelper.CreateParameter("@IsFeatured", isFeatured),
                    dbHelper.CreateParameter("@Limit", limit)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetSliders", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    sliders.Add(MapDataRowToSlider(row));
                }

                return sliders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting sliders: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new slider
        /// </summary>
        public int InsertSlider(Slider slider)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", slider.CompanyID),
                    dbHelper.CreateParameter("@BranchID", slider.BranchID),
                    dbHelper.CreateParameter("@Title", slider.Title),
                    dbHelper.CreateParameter("@Subtitle", slider.Subtitle),
                    dbHelper.CreateParameter("@Description", slider.Description),
                    dbHelper.CreateParameter("@ImagePath", slider.ImagePath),
                    dbHelper.CreateParameter("@ImageAltText", slider.ImageAltText),
                    dbHelper.CreateParameter("@ButtonText", slider.ButtonText),
                    dbHelper.CreateParameter("@ButtonLink", slider.ButtonLink),
                    dbHelper.CreateParameter("@ButtonTarget", slider.ButtonTarget),
                    dbHelper.CreateParameter("@DisplayOrder", slider.DisplayOrder),
                    dbHelper.CreateParameter("@IsActive", slider.IsActive),
                    dbHelper.CreateParameter("@IsFeatured", slider.IsFeatured),
                    dbHelper.CreateParameter("@StartDate", slider.StartDate),
                    dbHelper.CreateParameter("@EndDate", slider.EndDate),
                    dbHelper.CreateParameter("@CreatedBy", slider.CreatedBy)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertSlider", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting slider: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing slider
        /// </summary>
        public bool UpdateSlider(Slider slider)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SliderID", slider.SliderID),
                    dbHelper.CreateParameter("@Title", slider.Title),
                    dbHelper.CreateParameter("@Subtitle", slider.Subtitle),
                    dbHelper.CreateParameter("@Description", slider.Description),
                    dbHelper.CreateParameter("@ImagePath", slider.ImagePath),
                    dbHelper.CreateParameter("@ImageAltText", slider.ImageAltText),
                    dbHelper.CreateParameter("@ButtonText", slider.ButtonText),
                    dbHelper.CreateParameter("@ButtonLink", slider.ButtonLink),
                    dbHelper.CreateParameter("@ButtonTarget", slider.ButtonTarget),
                    dbHelper.CreateParameter("@DisplayOrder", slider.DisplayOrder),
                    dbHelper.CreateParameter("@IsActive", slider.IsActive),
                    dbHelper.CreateParameter("@IsFeatured", slider.IsFeatured),
                    dbHelper.CreateParameter("@StartDate", slider.StartDate),
                    dbHelper.CreateParameter("@EndDate", slider.EndDate),
                    dbHelper.CreateParameter("@UpdatedBy", slider.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateSlider", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating slider: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes a slider
        /// </summary>
        public bool DeleteSlider(int sliderID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@SliderID", sliderID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_DeleteSlider", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting slider: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a Slider object
        /// </summary>
        private Slider MapDataRowToSlider(DataRow row)
        {
            return new Slider
            {
                SliderID = Convert.ToInt32(row["SliderID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                Title = row["Title"]?.ToString(),
                Subtitle = row["Subtitle"]?.ToString(),
                Description = row["Description"]?.ToString(),
                ImagePath = row["ImagePath"].ToString(),
                ImageAltText = row["ImageAltText"]?.ToString(),
                ButtonText = row["ButtonText"]?.ToString(),
                ButtonLink = row["ButtonLink"]?.ToString(),
                ButtonTarget = row["ButtonTarget"]?.ToString() ?? "_self",
                DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                IsActive = Convert.ToBoolean(row["IsActive"]),
                IsFeatured = Convert.ToBoolean(row["IsFeatured"]),
                StartDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : (DateTime?)null,
                EndDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : (DateTime?)null,
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        #endregion
    }
}