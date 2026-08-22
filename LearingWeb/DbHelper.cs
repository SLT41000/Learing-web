using System.Data;
using Microsoft.Data.SqlClient;

namespace LearingWeb;

/// <summary>
/// Database helper that provides parameterized query execution to prevent SQL injection.
/// Uses Microsoft.Data.SqlClient (cross-platform).
/// </summary>
public static class DbHelper
{
    private static string? _connectionString;

    /// <summary>
    /// Gets or sets the connection string. 
    /// In production, set from appsettings.json or environment variable.
    /// </summary>
    public static string ConnectionString
    {
        get => _connectionString ?? throw new InvalidOperationException(
            "DbHelper.ConnectionString must be configured before use. " +
            "Set it in Program.cs or via the CONNECTION_STRING environment variable.");
        set => _connectionString = value;
    }

    /// <summary>
    /// Executes a parameterized query and returns results as a DataTable.
    /// </summary>
    public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
    {
        using var con = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, con);
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);

        var adapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    /// <summary>
    /// Executes a parameterized non-query (INSERT/UPDATE/DELETE).
    /// </summary>
    public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
    {
        using var con = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, con);
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);

        con.Open();
        return cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Executes a query and reads rows with a callback.
    /// </summary>
    public static void ReadQuery(string sql, Action<SqlDataReader> onRow, params SqlParameter[] parameters)
    {
        using var con = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, con);
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);

        con.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            onRow(reader);
    }

    /// <summary>
    /// Creates a parameterized SqlParameter.
    /// </summary>
    public static SqlParameter Param(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }
}
