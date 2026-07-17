// ============================================
// BAL/ContactMessageBAL.cs
// ============================================

using System;
using System.Collections.Generic;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.DAL;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.BAL
{
    /// <summary>
    /// Contact Message BAL - Business logic layer for contact form messages
    /// </summary>
    public class ContactMessageBAL
    {
        #region Private Fields

        private readonly ContactMessageDAL messageDAL;
        private readonly AuditLogger auditLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes DAL objects
        /// </summary>
        public ContactMessageBAL()
        {
            messageDAL = new ContactMessageDAL();
            auditLogger = new AuditLogger();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all contact messages with filters
        /// </summary>
        public List<ContactMessage> GetContactMessages(int companyID, int? branchID = null, string status = null,
            DateTime? dateFrom = null, DateTime? dateTo = null, int limit = 100, int offset = 0)
        {
            if (companyID <= 0)
                throw new ArgumentException("Invalid company ID.");

            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000.");

            if (offset < 0)
                throw new ArgumentException("Offset cannot be negative.");

            if (dateFrom.HasValue && dateTo.HasValue && dateFrom > dateTo)
                throw new ArgumentException("Start date cannot be after end date.");

            return messageDAL.GetContactMessages(companyID, branchID, status, dateFrom, dateTo, limit, offset);
        }

        /// <summary>
        /// Gets unread messages count
        /// </summary>
        public int GetUnreadCount(int companyID, int? branchID = null)
        {
            var messages = GetContactMessages(companyID, branchID, "Pending", null, null, 1000);
            return messages.Count;
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new contact message
        /// </summary>
        public int InsertContactMessage(ContactMessage message)
        {
            ValidateContactMessage(message);

            // Set default values
            if (string.IsNullOrEmpty(message.Status))
                message.Status = "Pending";

            if (string.IsNullOrEmpty(message.Priority))
                message.Priority = "Normal";

            int result = messageDAL.InsertContactMessage(message);

            if (result > 0)
            {
                auditLogger.LogInsert("ContactMessages", "ContactMessages", result,
                    $"From: {message.Name}, Subject: {message.Subject}");
            }

            return result;
        }

        /// <summary>
        /// Updates the status of a contact message
        /// </summary>
        public bool UpdateContactMessageStatus(int contactMessageID, string status, int updatedBy)
        {
            if (contactMessageID <= 0)
                throw new ArgumentException("Invalid contact message ID.");

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status is required.");

            string[] validStatuses = { "Pending", "Read", "Replied" };
            if (!Array.Exists(validStatuses, s => s.Equals(status, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Invalid status.");

            bool updated = messageDAL.UpdateContactMessageStatus(contactMessageID, status, updatedBy);

            if (updated)
            {
                auditLogger.LogUpdate("ContactMessages", "ContactMessages", contactMessageID,
                    $"MessageID: {contactMessageID}", $"Status: {status}");
            }

            return updated;
        }

        /// <summary>
        /// Replies to a contact message
        /// </summary>
        public bool ReplyContactMessage(int contactMessageID, string replyMessage, int updatedBy)
        {
            if (contactMessageID <= 0)
                throw new ArgumentException("Invalid contact message ID.");

            if (string.IsNullOrWhiteSpace(replyMessage))
                throw new ArgumentException("Reply message is required.");

            bool replied = messageDAL.ReplyContactMessage(contactMessageID, replyMessage, updatedBy);

            if (replied)
            {
                auditLogger.LogUpdate("ContactMessages", "ContactMessages", contactMessageID,
                    $"MessageID: {contactMessageID}", "Status: Replied");
            }

            return replied;
        }

        /// <summary>
        /// Marks a contact message as read
        /// </summary>
        public bool MarkContactMessageRead(int contactMessageID, int updatedBy)
        {
            if (contactMessageID <= 0)
                throw new ArgumentException("Invalid contact message ID.");

            bool marked = messageDAL.MarkContactMessageRead(contactMessageID, updatedBy);

            if (marked)
            {
                auditLogger.LogUpdate("ContactMessages", "ContactMessages", contactMessageID,
                    $"MessageID: {contactMessageID}", "Status: Read");
            }

            return marked;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates a contact message
        /// </summary>
        private void ValidateContactMessage(ContactMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.CompanyID <= 0)
                throw new ArgumentException("Company ID is required.");

            if (string.IsNullOrWhiteSpace(message.Name))
                throw new ArgumentException("Name is required.");

            if (message.Name.Length > 200)
                throw new ArgumentException("Name cannot exceed 200 characters.");

            if (string.IsNullOrWhiteSpace(message.Email))
                throw new ArgumentException("Email is required.");

            if (!ValidationHelper.IsValidEmail(message.Email))
                throw new ArgumentException("Invalid email format.");

            if (message.Email.Length > 200)
                throw new ArgumentException("Email cannot exceed 200 characters.");

            if (string.IsNullOrWhiteSpace(message.Message))
                throw new ArgumentException("Message is required.");

            if (message.Subject != null && message.Subject.Length > 200)
                throw new ArgumentException("Subject cannot exceed 200 characters.");

            if (message.Phone != null && message.Phone.Length > 50)
                throw new ArgumentException("Phone cannot exceed 50 characters.");
        }

        #endregion
    }
}