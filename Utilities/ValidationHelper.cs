// ============================================
// Utilities/ValidationHelper.cs
// ============================================

using System;
using System.Text.RegularExpressions;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Validation Helper - Provides validation methods for common data types
    /// Used for validating user input and data integrity
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <returns>True if email is valid</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a phone number
        /// </summary>
        /// <param name="phone">Phone number to validate</param>
        /// <returns>True if phone number is valid</returns>
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            try
            {
                // Remove spaces, dashes, parentheses
                string cleaned = Regex.Replace(phone, @"[\s\-\(\)]", "");
                return Regex.IsMatch(cleaned, @"^\+?[0-9]{10,15}$");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a password strength
        /// </summary>
        /// <param name="password">Password to validate</param>
        /// <returns>True if password meets strength requirements</returns>
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
                if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        /// <summary>
        /// Validates a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>True if date range is valid</returns>
        public static bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate && endDate <= DateTime.Now;
        }

        /// <summary>
        /// Validates a positive decimal number
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>True if value is positive</returns>
        public static bool IsPositiveDecimal(decimal value)
        {
            return value > 0;
        }

        /// <summary>
        /// Validates a non-negative decimal number
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>True if value is non-negative</returns>
        public static bool IsNonNegativeDecimal(decimal value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Validates a positive integer
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>True if value is positive</returns>
        public static bool IsPositiveInteger(int value)
        {
            return value > 0;
        }

        /// <summary>
        /// Validates a non-negative integer
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>True if value is non-negative</returns>
        public static bool IsNonNegativeInteger(int value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Validates a URL
        /// </summary>
        /// <param name="url">URL to validate</param>
        /// <returns>True if URL is valid</returns>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            try
            {
                return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                       (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sanitizes input to prevent XSS attacks
        /// </summary>
        /// <param name="input">Input string to sanitize</param>
        /// <returns>Sanitized string</returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Replace HTML tags
            input = Regex.Replace(input, @"<[^>]*>", string.Empty);

            // Replace JavaScript
            input = Regex.Replace(input, @"javascript:", string.Empty, RegexOptions.IgnoreCase);

            // Replace on* events
            input = Regex.Replace(input, @"on\w+\s*=", string.Empty, RegexOptions.IgnoreCase);

            return input.Trim();
        }
    }
}