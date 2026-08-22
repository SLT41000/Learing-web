using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearingWeb.Pages;

public class LogoutModel : PageModel
{
    public void OnPost()
    {
        HttpContext.Session.Clear();
        Response.Redirect("/");
    }
}
