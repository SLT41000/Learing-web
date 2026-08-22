using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class SigninModel : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string[] Mids { get; set; } = Array.Empty<string>();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Username or password should not be empty";
            return Page();
        }

        // Check if username already exists (parameterized)
        var existing = await Task.Run(() =>
            DbHelper.ExecuteQuery(
                "SELECT COUNT(*) FROM account WHERE uname = @uname",
                new SqlParameter("@uname", Username)
            )
        );

        if (int.Parse(existing.Rows[0][0].ToString()!) > 0)
        {
            ErrorMessage = "This username already exists!";
            return Page();
        }

        if (Mids.Length == 0)
        {
            ErrorMessage = "You must select at least one subject";
            return Page();
        }

        // Insert new account (parameterized)
        await Task.Run(() =>
            DbHelper.ExecuteNonQuery(
                "INSERT INTO account (uname, password) VALUES (@uname, @password)",
                new SqlParameter("@uname", Username),
                new SqlParameter("@password", Password)
            )
        );

        // Get the new account's ID
        var newAcct = await Task.Run(() =>
            DbHelper.ExecuteQuery(
                "SELECT * FROM account WHERE uname = @uname AND password = @password",
                new SqlParameter("@uname", Username),
                new SqlParameter("@password", Password)
            )
        );

        var aid = int.Parse(newAcct.Rows[0][0].ToString()!);

        // Insert account-member relationships (parameterized)
        foreach (var midValue in Mids)
        {
            if (int.TryParse(midValue, out var mid))
            {
                await Task.Run(() =>
                    DbHelper.ExecuteNonQuery(
                        "INSERT INTO accountmember (aid, mid) VALUES (@aid, @mid)",
                        new SqlParameter("@aid", aid),
                        new SqlParameter("@mid", mid)
                    )
                );
            }
        }

        // Set session
        HttpContext.Session.SetString("uname", Username);
        HttpContext.Session.SetString("aid", aid.ToString());

        // Create watchcheck entries for all videos in enrolled subjects
        foreach (var midValue in Mids)
        {
            if (int.TryParse(midValue, out var mid))
            {
                var videos = await Task.Run(() =>
                {
                    var vlist = new List<VidData>();
                    DbHelper.ReadQuery(
                        "SELECT v.vid FROM video AS v WHERE v.mid = @mid",
                        reader => vlist.Add(new VidData { Vid = reader.GetString(0), Mid = midValue }),
                        new SqlParameter("@mid", mid)
                    );
                    return vlist;
                });

                foreach (var v in videos)
                {
                    await Task.Run(() =>
                        DbHelper.ExecuteNonQuery(
                            "INSERT INTO watchcheck (aid, vid, mid, alreadywatch) VALUES (@aid, @vid, @mid, @alreadywatch)",
                            new SqlParameter("@aid", aid),
                            new SqlParameter("@vid", v.Vid),
                            new SqlParameter("@mid", v.Mid),
                            new SqlParameter("@alreadywatch", 0)
                        )
                    );
                }
            }
        }

        SuccessMessage = "Account created successfully! You can now login.";
        return Redirect("/login");
    }
}
