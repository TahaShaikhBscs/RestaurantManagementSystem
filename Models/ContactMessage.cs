// ============================================
// Models/ContactMessage.cs
// ============================================

using System;

namespace RestaurantManagementSystem.Models
{
    /// <summary>
    /// Contact Message Model - Represents a message from the contact form
    /// </summary>
    public class ContactMessage
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the contact message
        /// </summary>
        public int ContactMessageID { get; set; }

        /// <summary>
        /// Company ID this message belongs to
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Branch ID this message belongs to
        /// </summary>
        public int BranchID { get; set; }

        /// <summary>
        /// Name of the sender
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email of the sender
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the sender
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Subject of the message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Status of the message (Pending, Read, Replied)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Priority of the message (Low, Normal, High, Urgent)
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// IP address of the sender
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// User agent of the sender
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Whether the message has been read
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Reply message content
        /// </summary>
        public string ReplyMessage { get; set; }

        /// <summary>
        /// Date when the reply was sent
        /// </summary>
        public DateTime? ReplyDate { get; set; }

        /// <summary>
        /// User ID who sent the reply
        /// </summary>
        public int? ReplyBy { get; set; }

        /// <summary>
        /// Status of the contact message
        /// </summary>
        public bool _Status { get; set; }

        /// <summary>
        /// Date when the message was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the message was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// User ID who created the message
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the message
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Display Properties

        /// <summary>
        /// Gets the status badge class
        /// </summary>
        public string StatusBadgeClass
        {
            get
            {
                switch (Status)
                {
                    case "Pending": return "bg-warning";
                    case "Read": return "bg-info";
                    case "Replied": return "bg-success";
                    default: return "bg-secondary";
                }
            }
        }

        /// <summary>
        /// Gets the priority badge class
        /// </summary>
        public string PriorityBadgeClass
        {
            get
            {
                switch (Priority)
                {
                    case "Urgent": return "bg-danger";
                    case "High": return "bg-danger";
                    case "Normal": return "bg-primary";
                    case "Low": return "bg-secondary";
                    default: return "bg-secondary";
                }
            }
        }

        #endregion
    }
}