using System.Data;
using Microsoft.Data.SqlClient;

namespace LearingWeb;

public static class DbHelper
{
    private static string? _connectionString;

    public static string ConnectionString
    {
        get => _connectionString ?? throw new InvalidOperationException(
            "DbHelper.ConnectionString must be set before use.");
        set => _connectionString = value;
    }

    public static SqlConnection OpenConnection() => new SqlConnection(ConnectionString);

    public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
    {
        using var con = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, con);
        if (parameters != null) cmd.Parameters.AddRange(parameters);
        var adapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
    {
        using var con = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, con);
        if (parameters != null) cmd.Parameters.AddRange(parameters);
        con.Open();
        return cmd.ExecuteNonQuery();
    }

    public static void ReadQuery(string sql, Action<SqlDataReader> onRow, params SqlParameter[] parameters)
    {
        using var con = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, con);
        if (parameters != null) cmd.Parameters.AddRange(parameters);
        con.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) onRow(reader);
    }
}
