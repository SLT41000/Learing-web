using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class CertificateModel : PageModel
{
    public List<CompletedVideo> CompletedList { get; set; } = new();
    public string UserName { get; set; } = "";
    public int Id { get; set; }

    public void OnGet(int id)
    {
        UserName = HttpContext.Session.GetString("uname") ?? "";
        Id = id;
        CompletedList = new List<CompletedVideo>();

        if (!int.TryParse(id.ToString(), out var aid))
        {
            return;
        }

        using (var conn = DbHelper.OpenConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT w.vid, w.ontime, v.vname, v.vdesc, v.mid, s.mname " +
                "FROM dbo.watchcheck w " +
                "JOIN dbo.video v ON w.vid = v.vid " +
                "JOIN dbo.subject s ON v.mid = s.mid " +
                "WHERE w.aid = @aid AND w.alreadywatch = 1 ORDER BY v.vid", conn);
            cmd.Parameters.AddWithValue("@aid", aid);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompletedList.Add(new CompletedVideo
                {
                    Vid = reader.GetInt32(0).ToString(),
                    Ontime = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Vname = reader.GetString(2),
                    Vdesc = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    MidName = reader.GetString(5)
                });
            }
        }
    }
}
