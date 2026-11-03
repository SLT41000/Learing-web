using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class WatchModel : PageModel
{
    public ScreenData Video { get; set; } = new();
    public WatchcheckRecord WatchCheck { get; set; } = new();
    public int Aid { get; set; }
    public string UserName { get; set; } = "";

    public void OnGet(int id)
    {
        UserName = HttpContext.Session.GetString("uname") ?? "";
        Aid = HttpContext.Session.GetInt32("aid") ?? 0;

        using (var conn = DbHelper.OpenConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT v.vid, v.vname, v.vdesc, v.vfile, v.mid, s.mname " +
                "FROM dbo.video v " +
                "JOIN dbo.subject s ON v.mid = s.mid " +
                "WHERE v.vid = @vid", conn);
            cmd.Parameters.AddWithValue("@vid", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Video = new ScreenData
                {
                    Vid = reader.GetInt32(0).ToString(),
                    Vname = reader.GetString(1),
                    Vdesc = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Vlink = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Mid = reader.GetInt32(4).ToString(),
                    MidName = reader.GetString(5)
                };
            }
        }

        if (Aid > 0)
        {
            using var conn = DbHelper.OpenConnection();
            conn.Open();
            var wc = new SqlCommand(
                "SELECT vid, ontime, alreadywatch FROM dbo.watchcheck " +
                "WHERE aid = @aid AND vid = @vid", conn);
            wc.Parameters.AddWithValue("@aid", Aid);
            wc.Parameters.AddWithValue("@vid", id);
            using var wcReader = wc.ExecuteReader();
            if (wcReader.Read())
            {
                WatchCheck = new WatchcheckRecord
                {
                    Vid = wcReader.GetInt32(0).ToString(),
                    Ontime = wcReader.IsDBNull(1) ? "" : wcReader.GetString(1),
                    Alreadywatch = wcReader.GetBoolean(2) ? "1" : "0"
                };
            }
        }
    }

    public IActionResult OnPostCheckin(int id)
    {
        var aid = HttpContext.Session.GetInt32("aid");
        if (aid is null) return RedirectToPage("/Login");

        using var conn = DbHelper.OpenConnection();
        conn.Open();
        var cmd = new SqlCommand(
            "INSERT INTO dbo.watchcheck (aid, vid, mid, ontime, alreadywatch) " +
            "SELECT @aid, v.vid, v.mid, @time, 0 FROM dbo.video v " +
            "WHERE v.vid = @vid AND NOT EXISTS " +
            "(SELECT 1 FROM dbo.watchcheck WHERE aid = @aid AND vid = @vid);", conn);
        cmd.Parameters.AddWithValue("@aid", aid.Value);
        cmd.Parameters.AddWithValue("@vid", id);
        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        if (cmd.ExecuteNonQuery() == 0 && !VideoExists(conn, id))
        {
            return NotFound();
        }

        return RedirectToPage(new { id });
    }

    public IActionResult OnPostMarkWatched(int id)
    {
        var aid = HttpContext.Session.GetInt32("aid");
        if (aid is null) return RedirectToPage("/Login");

        using var conn = DbHelper.OpenConnection();
        conn.Open();
        var cmd = new SqlCommand(
            "IF EXISTS (SELECT 1 FROM dbo.watchcheck WHERE aid = @aid AND vid = @vid) " +
            "UPDATE dbo.watchcheck SET alreadywatch = 1, ontime = @time WHERE aid = @aid AND vid = @vid; " +
            "ELSE INSERT INTO dbo.watchcheck (aid, vid, mid, ontime, alreadywatch) " +
            "SELECT @aid, v.vid, v.mid, @time, 1 FROM dbo.video v WHERE v.vid = @vid;", conn);
        cmd.Parameters.AddWithValue("@aid", aid.Value);
        cmd.Parameters.AddWithValue("@vid", id);
        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        if (cmd.ExecuteNonQuery() == 0 && !VideoExists(conn, id))
        {
            return NotFound();
        }

        return RedirectToPage(new { id });
    }

    private static bool VideoExists(SqlConnection conn, int id)
    {
        using var check = new SqlCommand("SELECT 1 FROM dbo.video WHERE vid = @vid", conn);
        check.Parameters.AddWithValue("@vid", id);
        return check.ExecuteScalar() is not null;
    }
}
