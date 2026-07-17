// ============================================
// Utilities/EncryptionHelper.cs
// ============================================

using System;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantManagementSystem.Utilities
{
    /// <summary>
    /// Encryption Helper - Provides encryption and decryption functionality
    /// Used for securing sensitive data like passwords and connection strings
    /// </summary>
    public class EncryptionHelper
    {
        private static readonly string EncryptionKey = "RMS@2024#SecureKey!";

        /// <summary>
        /// Encrypts a string using AES encryption
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32).Substring(0, 32));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Generate IV
                    aes.GenerateIV();
                    byte[] iv = aes.IV;

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                        // Combine IV and encrypted data
                        byte[] result = new byte[iv.Length + encryptedBytes.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
            catch (Exception)
            {
                return plainText;
            }
        }

        /// <summary>
        /// Decrypts an encrypted string
        /// </summary>
        /// <param name="cipherText">Encrypted text</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32).Substring(0, 32));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Extract IV
                    byte[] iv = new byte[aes.BlockSize / 8];
                    Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    // Extract encrypted data
                    byte[] encryptedBytes = new byte[cipherBytes.Length - iv.Length];
                    Buffer.BlockCopy(cipherBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                        return Encoding.UTF8.GetString(plainBytes);
                    }
                }
            }
            catch (Exception)
            {
                return cipherText;
            }
        }

        /// <summary>
        /// Generates a random password
        /// </summary>
        /// <param name="length">Password length</param>
        /// <returns>Random password</returns>
        public static string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        /// <summary>
        /// Generates a random token for password reset
        /// </summary>
        /// <returns>Random token</returns>
        public static string GenerateToken()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData).Replace("+", "-").Replace("/", "_").Replace("=", "");
            }
        }
    }
}