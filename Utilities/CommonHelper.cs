// ============================================
// Utilities/CommonHelper.cs
// ============================================

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Common Helper - Provides common utility functions
    /// Used throughout the application for various tasks
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Gets the current date and time in the system's timezone
        /// </summary>
        public static DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets the current date in the system's timezone
        /// </summary>
        public static DateTime Today
        {
            get { return DateTime.Today; }
        }

        /// <summary>
        /// Formats currency value
        /// </summary>
        /// <param name="amount">Amount to format</param>
        /// <param name="currencySymbol">Currency symbol (default: $)</param>
        /// <returns>Formatted currency string</returns>
        public static string FormatCurrency(decimal amount, string currencySymbol = "$")
        {
            return $"{currencySymbol}{amount:F2}";
        }

        /// <summary>
        /// Formats a number with thousand separators
        /// </summary>
        public static string FormatNumber(decimal number)
        {
            return number.ToString("#,##0.##");
        }

        /// <summary>
        /// Formats a phone number
        /// </summary>
        public static string FormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            // Remove non-digit characters
            string digits = Regex.Replace(phone, @"[^\d]", "");

            if (digits.Length == 10)
                return $"{digits.Substring(0, 3)}-{digits.Substring(3, 3)}-{digits.Substring(6, 4)}";
            else if (digits.Length == 11 && digits.StartsWith("1"))
                return $"+1-{digits.Substring(1, 3)}-{digits.Substring(4, 3)}-{digits.Substring(7, 4)}";
            else
                return phone;
        }

        /// <summary>
        /// Truncates a string to a maximum length
        /// </summary>
        public static string Truncate(string text, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength) + suffix;
        }

        /// <summary>
        /// Converts a string to Title Case
        /// </summary>
        public static string ToTitleCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(text.ToLower());
        }

        /// <summary>
        /// Generates a random alphanumeric string
        /// </summary>
        public static string GenerateRandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Checks if a string is null or empty
        /// </summary>
        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Gets the client's IP address
        /// </summary>
        public static string GetClientIP()
        {
            HttpRequest request = HttpContext.Current?.Request;
            if (request == null)
                return null;

            string ip = request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(ip))
                ip = request.UserHostAddress;

            return ip;
        }

        /// <summary>
        /// Gets the current user's browser info
        /// </summary>
        public static string GetBrowserInfo()
        {
            HttpRequest request = HttpContext.Current?.Request;
            if (request == null)
                return null;

            return request.UserAgent;
        }

        /// <summary>
        /// Converts a decimal to a word representation (for checks)
        /// </summary>
        public static string NumberToWords(decimal number)
        {
            // This is a simplified version - for production, use a more robust implementation
            return number.ToString("F2");
        }

        /// <summary>
        /// Gets the current page name
        /// </summary>
        public static string GetCurrentPageName()
        {
            HttpRequest request = HttpContext.Current?.Request;
            if (request == null)
                return null;

            return System.IO.Path.GetFileName(request.PhysicalPath);
        }
    }
}