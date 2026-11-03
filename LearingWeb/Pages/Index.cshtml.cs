using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearingWeb;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LearingWeb.Pages;

public class IndexModel : PageModel
{
    public List<ScreenData> Videos { get; set; } = new();
    public Dictionary<int, SubjectData> Subjects { get; set; } = new();
    public int? ActiveSubjectId { get; set; }

    public void OnGet(int? subject = null)
    {
        using var conn = DbHelper.OpenConnection();
        conn.Open();

        using (var subjectCommand = new SqlCommand(
            "SELECT mid, mname, description FROM dbo.subject ORDER BY mid", conn))
        {
            using var reader = subjectCommand.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                Subjects[id] = new SubjectData
                {
                    Cid = id.ToString(),
                    Cname = reader.GetString(1),
                    Cdsc = reader.IsDBNull(2) ? "" : reader.GetString(2)
                };
            }
        }

        ActiveSubjectId = subject.HasValue && Subjects.ContainsKey(subject.Value)
            ? subject
            : null;

        using (var videoCommand = new SqlCommand(
            "SELECT v.vid, v.mid, v.vname, v.vdesc, v.vfile " +
            "FROM dbo.video v WHERE (@mid IS NULL OR v.mid = @mid) ORDER BY v.vid",
            conn))
        {
            videoCommand.Parameters.Add("@mid", SqlDbType.Int).Value =
                (object?)ActiveSubjectId ?? DBNull.Value;
            using var reader = videoCommand.ExecuteReader();
            while (reader.Read())
            {
                Videos.Add(new ScreenData
                {
                    Vid = reader.GetInt32(0).ToString(),
                    Mid = reader.GetInt32(1).ToString(),
                    Vname = reader.GetString(2),
                    Vdesc = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Vlink = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }
        }
    }
}
