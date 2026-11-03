using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "사용자명과 비밀번호를 입력해주세요.";
            return Page();
        }

        int? aid = null;
        string? hashedPasswordFromDb = null;

        try
        {
            using var conn = DbHelper.OpenConnection();
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT aid, password FROM dbo.account WHERE uname = @uname", conn);
            cmd.Parameters.AddWithValue("@uname", Username);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                aid = reader.GetInt32(0);
                hashedPasswordFromDb = reader.GetString(1);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"데이터베이스 오류: {ex.Message}";
            return Page();
        }

        if (aid == null || hashedPasswordFromDb == null)
        {
            ErrorMessage = "아이디 또는 비밀번호가 올바르지 않습니다.";
            return Page();
        }

        bool verified;
        try
        {
            verified = BCrypt.Net.BCrypt.Verify(Password, hashedPasswordFromDb);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"비밀번호 검증 오류: {ex.Message}";
            return Page();
        }

        if (!verified)
        {
            ErrorMessage = "아이디 또는 비밀번호가 올바르지 않습니다.";
            return Page();
        }

        HttpContext.Session.SetString("uname", Username);
        HttpContext.Session.SetInt32("aid", aid.Value);
        return RedirectToPage("/Index");
    }
}
