
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace RestaurantManagementSystem.DAL
{
    /// <summary>
    /// DatabaseHelper - Helper class for database operations
    /// Provides methods for executing SQL commands and stored procedures
    /// </summary>
    public class DatabaseHelper
    {
        // Connection string from web.config
        private readonly string connectionString;

        /// <summary>
        /// Constructor initializes connection string
        /// </summary>
        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;
        }

        /// <summary>
        /// Gets a new SQL connection
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Executes a stored procedure and returns a DataTable
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="parameters">List of SQL parameters</param>
        /// <returns>DataTable containing the results</returns>
        public DataTable ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error executing stored procedure {storedProcedureName}: {ex.Message}", ex);
            }

            return dataTable;
        }

        /// <summary>
        /// Executes a stored procedure that returns a single value
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="parameters">List of SQL parameters</param>
        /// <returns>Single value returned by the stored procedure</returns>
        public object ExecuteScalar(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        connection.Open();
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing scalar {storedProcedureName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure that doesn't return results
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="parameters">List of SQL parameters</param>
        /// <returns>Number of rows affected</returns>
        public int ExecuteNonQuery(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing non-query {storedProcedureName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns a SqlDataReader
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="parameters">List of SQL parameters</param>
        /// <returns>SqlDataReader with results</returns>
        public SqlDataReader ExecuteReader(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Creates a SQL parameter with value
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <returns>SqlParameter object</returns>
        public SqlParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }

        /// <summary>
        /// Creates a SQL parameter with specific type
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="sqlDbType">SQL data type</param>
        /// <returns>SqlParameter object</returns>
        public SqlParameter CreateParameter(string parameterName, object value, SqlDbType sqlDbType)
        {
            return new SqlParameter(parameterName, sqlDbType) { Value = value ?? DBNull.Value };
        }
    }
}