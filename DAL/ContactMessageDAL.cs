// ============================================
// DAL/ContactMessageDAL.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Contact Message DAL - Data access layer for contact form messages
    /// </summary>
    public class ContactMessageDAL
    {
        #region Private Fields

        private readonly DatabaseHelper dbHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializes database helper
        /// </summary>
        public ContactMessageDAL()
        {
            dbHelper = new DatabaseHelper();
        }

        #endregion

        #region Read Operations

        /// <summary>
        /// Gets all contact messages with filters
        /// </summary>
        public List<ContactMessage> GetContactMessages(int companyID, int? branchID = null, string status = null, DateTime? dateFrom = null, DateTime? dateTo = null, int limit = 100, int offset = 0)
        {
            try
            {
                List<ContactMessage> messages = new List<ContactMessage>();
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", companyID),
                    dbHelper.CreateParameter("@BranchID", branchID),
                    dbHelper.CreateParameter("@Status", status),
                    dbHelper.CreateParameter("@DateFrom", dateFrom),
                    dbHelper.CreateParameter("@DateTo", dateTo),
                    dbHelper.CreateParameter("@Limit", limit),
                    dbHelper.CreateParameter("@Offset", offset)
                };

                DataTable dataTable = dbHelper.ExecuteStoredProcedure("sp_GetContactMessages", parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    messages.Add(MapDataRowToContactMessage(row));
                }

                return messages;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting contact messages: {ex.Message}", ex);
            }
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Inserts a new contact message
        /// </summary>
        public int InsertContactMessage(ContactMessage message)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@CompanyID", message.CompanyID),
                    dbHelper.CreateParameter("@BranchID", message.BranchID),
                    dbHelper.CreateParameter("@Name", message.Name),
                    dbHelper.CreateParameter("@Email", message.Email),
                    dbHelper.CreateParameter("@Phone", message.Phone),
                    dbHelper.CreateParameter("@Subject", message.Subject),
                    dbHelper.CreateParameter("@Message", message.Message),
                    dbHelper.CreateParameter("@IPAddress", message.IPAddress),
                    dbHelper.CreateParameter("@UserAgent", message.UserAgent)
                };

                object result = dbHelper.ExecuteScalar("sp_InsertContactMessage", parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting contact message: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the status of a contact message
        /// </summary>
        public bool UpdateContactMessageStatus(int contactMessageID, string status, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ContactMessageID", contactMessageID),
                    dbHelper.CreateParameter("@Status", status),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_UpdateContactMessageStatus", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating contact message status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Replies to a contact message
        /// </summary>
        public bool ReplyContactMessage(int contactMessageID, string replyMessage, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ContactMessageID", contactMessageID),
                    dbHelper.CreateParameter("@ReplyMessage", replyMessage),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_ReplyContactMessage", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error replying to contact message: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Marks a contact message as read
        /// </summary>
        public bool MarkContactMessageRead(int contactMessageID, int updatedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    dbHelper.CreateParameter("@ContactMessageID", contactMessageID),
                    dbHelper.CreateParameter("@UpdatedBy", updatedBy)
                };

                int rowsAffected = dbHelper.ExecuteNonQuery("sp_MarkContactMessageRead", parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error marking contact message as read: {ex.Message}", ex);
            }
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a DataRow to a ContactMessage object
        /// </summary>
        private ContactMessage MapDataRowToContactMessage(DataRow row)
        {
            return new ContactMessage
            {
                ContactMessageID = Convert.ToInt32(row["ContactMessageID"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                BranchID = Convert.ToInt32(row["BranchID"]),
                Name = row["Name"].ToString(),
                Email = row["Email"].ToString(),
                Phone = row["Phone"]?.ToString(),
                Subject = row["Subject"]?.ToString(),
                Message = row["Message"].ToString(),
                Status = row["Status"].ToString(),
                Priority = row["Priority"]?.ToString() ?? "Normal",
                IPAddress = row["IPAddress"]?.ToString(),
                UserAgent = row["UserAgent"]?.ToString(),
                IsRead = Convert.ToBoolean(row["IsRead"]),
                ReplyMessage = row["ReplyMessage"]?.ToString(),
                ReplyDate = row["ReplyDate"] != DBNull.Value ? Convert.ToDateTime(row["ReplyDate"]) : (DateTime?)null,
                ReplyBy = row["ReplyBy"] != DBNull.Value ? Convert.ToInt32(row["ReplyBy"]) : (int?)null,
                _Status = Convert.ToBoolean(row["Status"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"])
            };
        }

        #endregion
    }
}