using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearingWeb;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class LoginModel : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Username and password are required";
            return Page();
        }

        var dt = await Task.Run(() =>
            DbHelper.ExecuteQuery(
                "SELECT * FROM dbo.account WHERE uname = @uname AND password = @password",
                new SqlParameter("@uname", Username),
                new SqlParameter("@password", Password)
            )
        );

        if (dt.Rows.Count == 0)
        {
            ErrorMessage = "Your username and password are incorrect";
            return Page();
        }

        var aid = dt.Rows[0][0].ToString()!;

        HttpContext.Session.SetString("uname", Username);
        HttpContext.Session.SetString("aid", aid);

        var mids = new List<string>();
        await Task.Run(() =>
        {
            DbHelper.ReadQuery(
                "SELECT mid FROM dbo.accountmember WHERE aid = @aid",
                reader => mids.Add(reader.GetString(0)),
                new SqlParameter("@aid", aid)
            );
        });

        HttpContext.Session.SetString("mids", string.Join(",", mids));

        var watchcheckCount = await Task.Run(() =>
        {
            var dt2 = DbHelper.ExecuteQuery(
                "SELECT COUNT(*) FROM dbo.watchcheck WHERE aid = @aid",
                new SqlParameter("@aid", aid)
            );
            return dt2.Rows[0][0].ToString();
        });

        if (watchcheckCount == "0")
        {
            foreach (var mid in mids)
            {
                var videos = await Task.Run(() =>
                {
                    var vdt = new List<VidData>();
                    DbHelper.ReadQuery(
                        "SELECT v.vid FROM dbo.video AS v WHERE v.mid = @mid",
                        reader => vdt.Add(new VidData { Vid = reader.GetString(0), Mid = mid }),
                        new SqlParameter("@mid", mid)
                    );
                    return vdt;
                });

                foreach (var v in videos)
                {
                    await Task.Run(() =>
                        DbHelper.ExecuteNonQuery(
                            "INSERT INTO dbo.watchcheck (aid, vid, mid, alreadywatch) VALUES (@aid, @vid, @mid, @alreadywatch)",
                            new SqlParameter("@aid", aid),
                            new SqlParameter("@vid", v.Vid),
                            new SqlParameter("@mid", v.Mid),
                            new SqlParameter("@alreadywatch", 0)
                        )
                    );
                }
            }
        }

        return Redirect("/");
    }
}
