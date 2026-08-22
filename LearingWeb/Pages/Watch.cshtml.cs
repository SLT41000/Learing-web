using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using LearingWeb;

namespace LearingWeb.Pages;

public class WatchModel : PageModel
{
    public string? VideoId { get; set; }
    public string? VideoName { get; set; }
    public string? Description { get; set; }
    public string? VideoLink { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool Authorized { get; set; }

    public async Task OnGetAsync()
    {
        Authorized = !string.IsNullOrEmpty(HttpContext.Session.GetString("uname"));
        UserName = HttpContext.Session.GetString("uname") ?? "";

        if (!Authorized)
            return;

        var vid = Request.Query["v"].FirstOrDefault() ?? Request.Query["vid"].FirstOrDefault();
        var vname = Request.Query["vname"].FirstOrDefault();
        var vlink = Request.Query["vlink"].FirstOrDefault();
        var vdes = Request.Query["vdes"].FirstOrDefault();

        if (!string.IsNullOrEmpty(vid))
        {
            VideoId = vid;
            VideoName = vname ?? "Untitled Video";
            VideoLink = vlink ?? "";
            Description = vdes ?? "";

            // Record check-in if user is authorized and video has a link
            if (!string.IsNullOrEmpty(VideoLink) && !string.IsNullOrEmpty(HttpContext.Session.GetString("aid")))
            {
                await RecordCheckInAsync(vid!);
            }
        }
        else
        {
            // No video specified, show a message or redirect
            VideoName = "No video selected";
            VideoLink = "";
            Description = "Please select a course from the home page.";
        }
    }

    private async Task RecordCheckInAsync(string vid)
    {
        var aid = HttpContext.Session.GetString("aid")!;
        var ontime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

        await Task.Run(() =>
            DbHelper.ExecuteNonQuery(
                "INSERT INTO catalog (aid, vid, ontime) VALUES (@aid, @vid, @ontime)",
                new SqlParameter("@aid", aid),
                new SqlParameter("@vid", vid),
                new SqlParameter("@ontime", ontime)
            )
        );
    }
}
