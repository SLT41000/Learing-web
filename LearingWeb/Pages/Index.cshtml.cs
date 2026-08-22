using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class IndexModel : PageModel
{
    public bool Loading { get; set; } = true;
    public List<ScreenData> GDATA { get; set; } = new();
    public string SelectedSubject { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public async Task OnGetAsync(string? subject)
    {
        UserName = HttpContext.Session.GetString("uname") ?? "";
        SelectedSubject = subject ?? "";
        await LoadVideosAsync();
    }

    private async Task LoadVideosAsync()
    {
        Loading = true;
        GDATA.Clear();

        await Task.Run(() =>
        {
            DbHelper.ReadQuery(
                "SELECT vid, mid, vname, description, vlink FROM video",
                reader =>
                {
                    GDATA.Add(new ScreenData
                    {
                        Vid = reader.GetString(0),
                        Mid = reader.GetString(1),
                        Vname = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Vlink = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    });
                }
            );
        });

        Loading = false;
    }
}
