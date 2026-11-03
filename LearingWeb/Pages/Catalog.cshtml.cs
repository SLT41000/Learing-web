using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class CatalogModel : PageModel
{
    public List<CatalogData> WatchedList { get; set; } = new();
    public string UserName { get; set; } = string.Empty;
    public int Id { get; set; }

    public void OnGet(int id)
    {
        UserName = HttpContext.Session.GetString("uname") ?? "";
        Id = id;
        WatchedList = new List<CatalogData>();
        if (!int.TryParse(id.ToString(), out var aid))
        {
            return;
        }
        using (var conn = DbHelper.OpenConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT w.vid, v.vname, v.vfile, v.mid, s.mname, w.ontime, " +
                "CAST(w.alreadywatch AS NVARCHAR(10)) as alreadywatch " +
                "FROM dbo.watchcheck w " +
                "JOIN dbo.video v ON w.vid = v.vid " +
                "JOIN dbo.subject s ON v.mid = s.mid " +
                "WHERE w.aid = @aid " +
                "ORDER BY w.ontime DESC",
                conn);
            cmd.Parameters.AddWithValue("@aid", aid);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                WatchedList.Add(new CatalogData
                {
                    Vid = reader.GetInt32(0).ToString(),
                    Vname = reader.GetString(1),
                    Vlink = reader.GetString(2),
                    Mid = reader.GetInt32(3).ToString(),
                    MidName = reader.GetString(4),
                    Ontime = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Alreadywatch = reader.GetString(6)
                });
            }
        }
    }
}
