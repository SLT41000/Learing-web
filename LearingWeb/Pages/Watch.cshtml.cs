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

    public void OnGet()
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
        }
        else
        {
            // No video specified, show a message or redirect
            VideoName = "No video selected";
            VideoLink = "";
            Description = "Please select a course from the home page.";
        }
    }
}
