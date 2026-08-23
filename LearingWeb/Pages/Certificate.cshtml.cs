using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class CertificateModel : PageModel
{
    public bool Loading { get; set; } = true;
    public List<CompletedVideo> CompletedVideos { get; set; } = new();

    public async Task OnGetAsync()
    {
        var aid = HttpContext.Session.GetString("aid");
        if (string.IsNullOrEmpty(aid))
        {
            Loading = false;
            return;
        }

        await Task.Run(() =>
        {
            DbHelper.ReadQuery(
                "SELECT v.vname FROM dbo.watchcheck AS w INNER JOIN dbo.video AS v ON (v.vid = w.vid) WHERE w.aid = @aid AND w.alreadywatch = 1",
                reader =>
                {
                    CompletedVideos.Add(new CompletedVideo
                    {
                        Vname = reader.IsDBNull(0) ? "" : reader.GetString(0)
                    });
                },
                new SqlParameter("@aid", aid)
            );
        });

        Loading = false;
    }
}
