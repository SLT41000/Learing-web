using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class SignupModel : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    public string[] SelectedSubjects { get; set; } = Array.Empty<string>();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("uname")))
        {
            HttpContext.Response.Redirect("/");
        }
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Username or password shouldn't be empty";
            return Page();
        }

        // Read selected subjects from form
        var formSubjects = Request.Form["SelectedSubjects"].ToArray();
        SelectedSubjects = formSubjects.Length > 0 ? formSubjects : Array.Empty<string>();

        // Check if username already exists
        var existing = DbHelper.ExecuteQuery(
            "SELECT COUNT(*) FROM account WHERE uname = @uname",
            new SqlParameter("@uname", Username)
        );

        if (int.Parse(existing.Rows[0][0].ToString()!) > 0)
        {
            ErrorMessage = "This username already exists!";
            return Page();
        }

        // Must select at least one subject
        if (SelectedSubjects.Length == 0)
        {
            ErrorMessage = "You must select at least one subject";
            return Page();
        }

        // Insert new account
        DbHelper.ExecuteNonQuery(
            "INSERT INTO account (uname, password) VALUES (@uname, @password)",
            new SqlParameter("@uname", Username),
            new SqlParameter("@password", Password)
        );

        // Get the new account's ID
        var newAcct = DbHelper.ExecuteQuery(
            "SELECT * FROM account WHERE uname = @uname AND password = @password",
            new SqlParameter("@uname", Username),
            new SqlParameter("@password", Password)
        );

        var aid = int.Parse(newAcct.Rows[0][0].ToString()!);

        // Insert account-member relationships
        foreach (var subjectValue in SelectedSubjects)
        {
            DbHelper.ExecuteNonQuery(
                "INSERT INTO accountmember (aid, mid) VALUES (@aid, @mid)",
                new SqlParameter("@aid", aid),
                new SqlParameter("@mid", int.Parse(subjectValue))
            );
        }

        // Store session
        HttpContext.Session.SetString("uname", Username);
        HttpContext.Session.SetString("aid", aid.ToString());

        SuccessMessage = "Account created successfully!";
        return Redirect("/login");
    }
}
