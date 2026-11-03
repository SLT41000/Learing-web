using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearingWeb.Pages;

public class AboutModel : PageModel
{
    public string UserName { get; set; } = string.Empty;

    public void OnGet()
    {
        UserName = HttpContext.Session.GetString("uname") ?? "[signed in as nobody]";
    }
}
