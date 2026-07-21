using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RestaurantManagementSystem.Utilities
{

    public static class DataRowExtensions
    {
        /// <summary>
        /// Generic method to safely get value from DataRow.
        /// Returns default value if column is missing or NULL.
        /// </summary>
        public static T GetValue<T>(this DataRow row, string columnName)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (!row.Table.Columns.Contains(columnName))
                return default(T);

            if (row[columnName] == DBNull.Value || row[columnName] == null)
                return default(T);

            Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            return (T)Convert.ChangeType(row[columnName], targetType);
        }

        public static string GetString(this DataRow row, string columnName)
        {
            return row.GetValue<string>(columnName) ?? string.Empty;
        }

        public static int GetInt(this DataRow row, string columnName)
        {
            return row.GetValue<int>(columnName);
        }

        public static decimal GetDecimal(this DataRow row, string columnName)
        {
            return row.GetValue<decimal>(columnName);
        }

        public static bool GetBool(this DataRow row, string columnName)
        {
            return row.GetValue<bool>(columnName);
        }

        public static DateTime GetDateTime(this DataRow row, string columnName)
        {
            return row.GetValue<DateTime>(columnName);
        }
        public static DateTime? GetNullableDateTime(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? Convert.ToDateTime(row[columnName])
                : (DateTime?)null;
        }
        public static double GetDouble(this DataRow row, string columnName)
        {
            return row.GetValue<double>(columnName);
        }

        public static float GetFloat(this DataRow row, string columnName)
        {
            return row.GetValue<float>(columnName);
        }

        public static long GetLong(this DataRow row, string columnName)
        {
            return row.GetValue<long>(columnName);
        }

        public static short GetShort(this DataRow row, string columnName)
        {
            return row.GetValue<short>(columnName);
        }

        public static Guid GetGuid(this DataRow row, string columnName)
        {
            return row.GetValue<Guid>(columnName);
        }
        public static int? GetNullableInt(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? Convert.ToInt32(row[columnName])
                : (int?)null;
        }

        public static decimal? GetNullableDecimal(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? Convert.ToDecimal(row[columnName])
                : (decimal?)null;
        }
        public static byte[] GetBytes(this DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) ||
                row[columnName] == DBNull.Value)
                return null;

            return (byte[])row[columnName];
        }
    }
}