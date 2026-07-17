// ============================================
// DAL/WebsiteSettingDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Website Setting DAL - Data access layer for website settings
    /// </summary>
    public class WebsiteSettingDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public WebsiteSettingDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all website settings for a company
        /// </summary>
        public List<WebsiteSetting> GetSettings(int companyID)
        {
            try
            {
                List<WebsiteSetting> settings = new List<WebsiteSetting>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetWebsiteSettings", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    settings.Add(MapDataRowToSetting(row));
                }

                return settings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting website settings: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a specific website setting by key
        /// </summary>
        public WebsiteSetting GetSetting(int companyID, string settingKey)
        {
            try
            {
                var settings = GetSettings(companyID);
                return settings.Find(s => s.SettingKey.Equals(settingKey, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting website setting: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the setting value as a string
        /// </summary>
        public string GetSettingValue(int companyID, string settingKey, string defaultValue = "")
        {
            try
            {
                var setting = GetSetting(companyID, settingKey);
                return setting != null ? setting.SettingValue : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a website setting
        /// </summary>
        public bool SaveSetting(WebsiteSetting setting)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", setting.CompanyID),
                    dbHelper.CreateParameter("@SettingKey", setting.SettingKey),
                    dbHelper.CreateParameter("@SettingValue", setting.SettingValue),
                    dbHelper.CreateParameter("@DataType", setting.DataType),
                    dbHelper.CreateParameter("@Category", setting.Category),
                    dbHelper.CreateParameter("@IsPublic", setting.IsPublic),
                    dbHelper.CreateParameter("@UpdatedBy", setting.UpdatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_SaveWebsiteSetting", parameters);
                return rowsAffected >= 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving website setting: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves a single setting value by key
        /// </summary>
        public bool SaveSettingValue(int companyID, string settingKey, string settingValue, int updatedBy)
        {
            try
            {
                var setting = GetSetting(companyID, settingKey);
                if (setting == null)
                {
                    setting = new WebsiteSetting
                    {
                        CompanyID = companyID,
                        SettingKey = settingKey,
                        DataType = "String",
                        Category = "General",
                        IsPublic = true
                    };
                }

                setting.SettingValue = settingValue;
                setting.UpdatedBy = updatedBy;

                return SaveSetting(setting);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving setting value: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a WebsiteSetting object
        /// </summary>
        private WebsiteSetting MapDataRowToSetting(DataRow row)
        {
            return new WebsiteSetting
            {
                SettingID = Convert.ToInt32(row["SettingID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                SettingKey = row["SettingKey"].ToString(),
                SettingValue = row["SettingValue"]?.ToString(),
                DataType = row["DataType"]?.ToString() ?? "String",
                Category = row["Category"]?.ToString() ?? "General",
                IsPublic = Convert.ToBoolean(row["IsPublic"]),
                Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"])
            };
        }

        #endregion
    }
}