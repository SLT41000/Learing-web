using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LearingWeb;

var builder = WebApplication.CreateBuilder(args);

// Configure DbHelper connection string from appsettings or environment
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? "Server=localhost;Database=Learningweb;Trusted_Connection=True;TrustServerCertificate=True;";

LearingWeb.DbHelper.ConnectionString = connectionString;

// Add Razor Pages
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapRazorPages();

// Minimal API endpoints (replaces old WCF services)

// GET /api/videos — returns all video courses
app.MapGet("/api/videos", () =>
{
    var videos = new List<ScreenData>();
    DbHelper.ReadQuery(
        "SELECT vid, mid, vname, description, vlink FROM video",
        reader =>
        {
            videos.Add(new ScreenData
            {
                Vid = reader.GetString(0),
                Mid = reader.GetString(1),
                Vname = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Vlink = reader.IsDBNull(4) ? "" : reader.GetString(4)
            });
        }
    );
    return Results.Ok(videos);
}).WithName("GetAllVideos");

// GET /api/catalog?aid={accountId} — returns user's watch history
app.MapGet("/api/catalog", (string? aid) =>
{
    if (string.IsNullOrEmpty(aid))
        return Results.BadRequest("aid parameter is required");

    var catalog = new List<CatalogData>();
    DbHelper.ReadQuery(
        "SELECT c.vid, c.ontime, v.vname FROM catalog AS c INNER JOIN video AS v ON (c.vid = v.vid) WHERE c.aid = @aid",
        reader =>
        {
            catalog.Add(new CatalogData
            {
                Vid = reader.GetString(0),
                Ontime = reader.IsDBNull(1) ? "" : reader.GetString(1),
                Vname = reader.IsDBNull(2) ? "" : reader.GetString(2)
            });
        },
        new SqlParameter("@aid", aid)
    );
    return Results.Ok(catalog);
}).WithName("GetCatalog");

// GET /api/completed?aid={accountId} — returns completed videos (certificates)
app.MapGet("/api/completed", (string? aid) =>
{
    if (string.IsNullOrEmpty(aid))
        return Results.BadRequest("aid parameter is required");

    var completed = new List<CompletedVideo>();
    DbHelper.ReadQuery(
        "SELECT v.vname FROM watchcheck AS w INNER JOIN video AS v ON (v.vid = w.vid) WHERE w.aid = @aid AND w.alreadywatch = 1",
        reader =>
        {
            completed.Add(new CompletedVideo
            {
                Vname = reader.IsDBNull(0) ? "" : reader.GetString(0)
            });
        },
        new SqlParameter("@aid", aid)
    );
    return Results.Ok(completed);
}).WithName("GetCompletedVideos");

// POST /api/checkin?vid={vid} — record that user started watching
app.MapPost("/api/checkin", (string? vid, HttpContext http) =>
{
    var aid = http.Session.GetString("aid");
    if (string.IsNullOrEmpty(aid) || string.IsNullOrEmpty(vid))
        return Results.Unauthorized();

    DbHelper.ExecuteNonQuery(
        "INSERT INTO catalog (aid, vid, ontime) VALUES (@aid, @vid, @ontime)",
        new SqlParameter("@aid", aid),
        new SqlParameter("@vid", vid),
        new SqlParameter("@ontime", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"))
    );

    return Results.Ok(new { success = true, message = "Check-in recorded" });
}).WithName("CheckIn");

// POST /api/mark-watched?vid={vid} — mark video as watched
app.MapPost("/api/mark-watched", (string? vid, HttpContext http) =>
{
    var aid = http.Session.GetString("aid");
    if (string.IsNullOrEmpty(aid) || string.IsNullOrEmpty(vid))
        return Results.Unauthorized();

    DbHelper.ExecuteNonQuery(
        "UPDATE watchcheck SET alreadywatch = 1 WHERE aid = @aid AND vid = @vid",
        new SqlParameter("@aid", aid),
        new SqlParameter("@vid", vid)
    );

    return Results.Ok(new { success = true, message = "Marked as watched" });
}).WithName("MarkWatched");

// Serve img folder from wwwroot (if not already served as static files)
// The img/ folder in this project root is copied to wwwroot/img during build

app.Run();
