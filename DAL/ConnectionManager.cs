
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// Connection Manager - Manages database connections
    /// </summary>
    public static class ConnectionManager
    {
        private static readonly string connectionString;

        /// <summary>
        /// Static constructor initializes connection string
        /// </summary>
        static ConnectionManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;
        }

        /// <summary>
        /// Gets a new SQL connection
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        /// <returns>True if connection is successful</returns>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}