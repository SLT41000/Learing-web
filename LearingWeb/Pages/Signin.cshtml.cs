using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace LearingWeb.Pages;

public class SigninModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string PasswordConfirm { get; set; } = string.Empty;

    public List<SubjectData> Subjects { get; set; } = new();
    public Dictionary<int, string> SelectedSubjects { get; set; } = new();
    public string SuccessMessage { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet()
    {
        LoadSubjects();
    }

    private void LoadSubjects()
    {
        Subjects.Clear();
        using var conn = DbHelper.OpenConnection();
        conn.Open();
        var cmd = new SqlCommand("SELECT mid, mname, description FROM dbo.subject ORDER BY mid", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Subjects.Add(new SubjectData
            {
                Cid = reader.GetInt32(0).ToString(),
                Cname = reader.GetString(1),
                Cdsc = reader.IsDBNull(2) ? "" : reader.GetString(2)
            });
        }
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordConfirm))
        {
            ErrorMessage = "모든 필드를 입력해주세요.";
            LoadSubjects();
            return Page();
        }

        if (Password.Length < 6)
        {
            ErrorMessage = "비밀번호는 6자 이상이어야 합니다.";
            LoadSubjects();
            return Page();
        }

        if (Password != PasswordConfirm)
        {
            ErrorMessage = "비밀번호가 일치하지 않습니다.";
            LoadSubjects();
            return Page();
        }

        if (Username.Length < 3 || Username.Length > 50)
        {
            ErrorMessage = "사용자명은 3~50자 사이여야 합니다.";
            LoadSubjects();
            return Page();
        }

        string hashedPassword;
        try
        {
            hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password, workFactor: 10);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"비밀번호 해싱 오류: {ex.Message}";
            LoadSubjects();
            return Page();
        }

        int newAid = -1;

        using var conn = DbHelper.OpenConnection();
        conn.Open();

        var checkCmd = new SqlCommand("SELECT COUNT(*) FROM dbo.account WHERE uname = @uname", conn);
        checkCmd.Parameters.AddWithValue("@uname", Username);
        if ((int)checkCmd.ExecuteScalar() > 0)
        {
            ErrorMessage = "이미 존재하는 사용자명입니다.";
            LoadSubjects();
            return Page();
        }

        var insertCmd = new SqlCommand(
            "INSERT INTO dbo.account (uname, password) VALUES (@uname, @pwd); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn);
        insertCmd.Parameters.AddWithValue("@uname", Username);
        insertCmd.Parameters.AddWithValue("@pwd", hashedPassword);
        newAid = (int)insertCmd.ExecuteScalar();

        if (newAid < 0)
        {
            ErrorMessage = "계정 생성 실패.";
            return Page();
        }

        var selected = Request.Form["subject"];
        if (selected.Count > 0)
        {
            foreach (var val in selected)
            {
                if (int.TryParse(val, out var sid))
                {
                    var memberCmd = new SqlCommand(
                        "INSERT INTO dbo.accountmember (aid, mid) VALUES (@aid, @mid)", conn);
                    memberCmd.Parameters.AddWithValue("@aid", newAid);
                    memberCmd.Parameters.AddWithValue("@mid", sid);
                    memberCmd.ExecuteNonQuery();
                    SelectedSubjects[sid] = Subjects.FirstOrDefault(s => s.Cid == sid.ToString())?.Cname ?? "";
                }
            }
        }

        SuccessMessage = "계정이 생성되었습니다!";
        ViewData["Username"] = Username;
        ViewData["Aid"] = newAid.ToString();

        return RedirectToPage("/Login");
    }
}
