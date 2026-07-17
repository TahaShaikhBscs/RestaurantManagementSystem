// ============================================
// Models/WebsiteSetting.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Website Setting Model - Represents a website configuration setting
    /// </summary>
    public class WebsiteSetting
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the setting
        /// </summary>
        public int SettingID { get; set; }

        /// <summary>
        /// Company ID this setting belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Key of the setting (e.g., SiteName, PrimaryColor)
        /// </summary>
        public string SettingKey { get; set; }

        /// <summary>
        /// Value of the setting
        /// </summary>
        public string SettingValue { get; set; }

        /// <summary>
        /// Data type of the setting (String, Int, Boolean, Color, etc.)
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Category of the setting (General, Theme, SEO, etc.)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Whether this setting is visible to the public
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Status of the setting
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Date when the setting was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the setting was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the setting
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the setting
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the setting value as an integer
        /// </summary>
        public int IntValue
        {
            get
            {
                int result;
                return int.TryParse(SettingValue, out result) ? result : 0;
            }
        }

        /// <summary>
        /// Gets the setting value as a boolean
        /// </summary>
        public bool BoolValue
        {
            get
            {
                bool result;
                return bool.TryParse(SettingValue, out result) && result;
            }
        }

        /// <summary>
        /// Gets the setting value as a decimal
        /// </summary>
        public decimal DecimalValue
        {
            get
            {
                decimal result;
                return decimal.TryParse(SettingValue, out result) ? result : 0;
            }
        }

        #endregion
    }
}