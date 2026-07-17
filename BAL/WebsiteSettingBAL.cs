// ============================================
// BAL/WebsiteSettingBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Website Setting BAL - Business logic layer for website settings
    /// </summary>
    public class WebsiteSettingBAL
    {
        #region Private Fields

        private readonly WebsiteSettingDAL settingDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public WebsiteSettingBAL()
        {
            settingDAL = new WebsiteSettingDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all website settings for a company
        /// </summary>
        public List<WebsiteSetting> GetSettings(int companyID)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            return settingDAL.GetSettings(companyID);
        }

        /// <summary>
        /// Gets a specific setting by key
        /// </summary>
        public WebsiteSetting GetSetting(int companyID, string settingKey)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (string.IsNullOrWhiteSpace(settingKey))
                throw new ArgumentException("Setting key is required.");

            return settingDAL.GetSetting(companyID, settingKey);
        }

        /// <summary>
        /// Gets a setting value by key
        /// </summary>
        public string GetSettingValue(int companyID, string settingKey, string defaultValue = "")
        {
            if (companyID <= 0)
                return defaultValue;

            if (string.IsNullOrWhiteSpace(settingKey))
                return defaultValue;

            return settingDAL.GetSettingValue(companyID, settingKey, defaultValue);
        }

        /// <summary>
        /// Gets all settings by category
        /// </summary>
        public List<WebsiteSetting> GetSettingsByCategory(int companyID, string category)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category is required.");

            var settings = GetSettings(companyID);
            return settings.FindAll(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Saves a website setting
        /// </summary>
        public bool SaveSetting(WebsiteSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            if (setting.CompanyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (string.IsNullOrWhiteSpace(setting.SettingKey))
                throw new ArgumentException("Setting key is required.");

            if (string.IsNullOrWhiteSpace(setting.DataType))
                setting.DataType = "String";

            if (string.IsNullOrWhiteSpace(setting.Category))
                setting.Category = "General";

            bool result = settingDAL.SaveSetting(setting);

            if (result)
            {
                auditLogger.LogUpdate("WebsiteSettings", "WebsiteSettings", setting.SettingID,
                    $"Key: {setting.SettingKey}", $"Value: {setting.SettingValue}");
            }

            return result;
        }

        /// <summary>
        /// Saves a setting value by key
        /// </summary>
        public bool SaveSettingValue(int companyID, string settingKey, string settingValue, int updatedBy)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (string.IsNullOrWhiteSpace(settingKey))
                throw new ArgumentException("Setting key is required.");

            bool result = settingDAL.SaveSettingValue(companyID, settingKey, settingValue, updatedBy);

            if (result)
            {
                auditLogger.LogUpdate("WebsiteSettings", "WebsiteSettings", 0,
                    $"Key: {settingKey}", $"Value: {settingValue}");
            }

            return result;
        }

        #endregion

        #region Theme Settings

        /// <summary>
        /// Gets all theme settings
        /// </summary>
        public Dictionary<string, string> GetThemeSettings(int companyID)
        {
            var settings = GetSettingsByCategory(companyID, "Theme");
            var themeSettings = new Dictionary<string, string>();

            foreach (var setting in settings)
            {
                themeSettings[setting.SettingKey] = setting.SettingValue;
            }

            return themeSettings;
        }

        /// <summary>
        /// Saves theme settings
        /// </summary>
        public bool SaveThemeSettings(int companyID, Dictionary<string, string> themeSettings, int updatedBy)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (themeSettings == null || themeSettings.Count == 0)
                throw new ArgumentException("No theme settings to save.");

            bool allSaved = true;

            foreach (var setting in themeSettings)
            {
                bool saved = SaveSettingValue(companyID, setting.Key, setting.Value, updatedBy);
                if (!saved) allSaved = false;
            }

            return allSaved;
        }

        #endregion

        #region SEO Settings

        /// <summary>
        /// Gets all SEO settings
        /// </summary>
        public Dictionary<string, string> GetSEOSettings(int companyID)
        {
            var settings = GetSettingsByCategory(companyID, "SEO");
            var seoSettings = new Dictionary<string, string>();

            foreach (var setting in settings)
            {
                seoSettings[setting.SettingKey] = setting.SettingValue;
            }

            return seoSettings;
        }

        /// <summary>
        /// Saves SEO settings
        /// </summary>
        public bool SaveSEOSettings(int companyID, Dictionary<string, string> seoSettings, int updatedBy)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (seoSettings == null || seoSettings.Count == 0)
                throw new ArgumentException("No SEO settings to save.");

            bool allSaved = true;

            foreach (var setting in seoSettings)
            {
                bool saved = SaveSettingValue(companyID, setting.Key, setting.Value, updatedBy);
                if (!saved) allSaved = false;
            }

            return allSaved;
        }

        #endregion

        #region Contact Settings

        /// <summary>
        /// Gets all contact settings
        /// </summary>
        public Dictionary<string, string> GetContactSettings(int companyID)
        {
            var settings = GetSettingsByCategory(companyID, "Contact");
            var contactSettings = new Dictionary<string, string>();

            foreach (var setting in settings)
            {
                contactSettings[setting.SettingKey] = setting.SettingValue;
            }

            return contactSettings;
        }

        #endregion
    }
}