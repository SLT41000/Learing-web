using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Learing_web
{
    /// <summary>
    /// Helper class for database operations.
    /// Provides parameterized query execution to prevent SQL injection.
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Gets a connection string from web.config by name.
        /// </summary>
        public static string GetConnectionString(string name = "strconn")
        {
            return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Executes a parameterized query and returns results as a DataTable.
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var con = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Executes a parameterized non-query (INSERT/UPDATE/DELETE).
        /// </summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var con = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a parameterized query and reads results with a callback.
        /// </summary>
        public static void ReadQuery(string sql, Action<SqlDataReader> readerAction, params SqlParameter[] parameters)
        {
            using (var con = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        readerAction(reader);
                }
            }
        }

        /// <summary>
        /// Creates a SqlParameter with the given name, type, and value.
        /// </summary>
        public static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Creates a SqlParameter with explicit SqlDbType.
        /// </summary>
        public static SqlParameter Param(string name, SqlDbType type, object value)
        {
            return new SqlParameter(name, type) { Value = value ?? DBNull.Value };
        }
    }
}
