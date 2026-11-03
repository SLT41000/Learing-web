using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LearingWeb;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// Connection string: env var > appsettings > fallback
// -----------------------------------------------------------------------
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=Learningweb;Trusted_Connection=True;TrustServerCertificate=True;";

// Normalise for Docker: replace colon-separated host:port with comma form,
// force Encrypt=false when talking to the SQL Server container.
connectionString = connectionString
    .Replace("Server=sqlserver:1433", "Server=sqlserver,1433")
    .Replace("Encrypt=true", "Encrypt=false")
    .Replace("Encrypt=True", "Encrypt=false");
if (!connectionString.Contains("Encrypt=", StringComparison.OrdinalIgnoreCase))
    connectionString += ";Encrypt=false";

DbHelper.ConnectionString = connectionString;

// -----------------------------------------------------------------------
// Services
// -----------------------------------------------------------------------
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// -----------------------------------------------------------------------
// Ensure SQL Server is reachable and schema exists
// -----------------------------------------------------------------------
EnsureDatabaseAndSchema(connectionString);

// -----------------------------------------------------------------------
// Middleware pipeline
// -----------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapRazorPages();

// -----------------------------------------------------------------------
// Minimal API endpoints  (parameterized — no SQL injection)
// -----------------------------------------------------------------------

// GET /api/all-videos  — all video courses
app.MapGet("/api/all-videos", () =>
{
    var videos = new List<VideoSeed>();
    DbHelper.ReadQuery(
        "SELECT vid, mid, vname, vdesc, vfile FROM dbo.video",
        reader =>
        {
            videos.Add(new VideoSeed
            {
                Vid = reader.GetInt32(0).ToString(),
                Mid = reader.GetInt32(1).ToString(),
                Vname = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Vlink = reader.IsDBNull(4) ? "" : reader.GetString(4)
            });
        });
    return Results.Ok(videos);
}).WithName("GetAllVideos");

app.Run();

// =======================================================================
// Database initialisation — creates schema if missing.
// =======================================================================
static void EnsureDatabaseAndSchema(string connStr)
{
    // Make sure the Learningweb database itself exists.
    var masterConn = connStr.Replace("Database=Learningweb", "Database=master");
    using var master = new SqlConnection(masterConn);
    master.Open();
    using var cmd = new SqlCommand(
        "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Learningweb') " +
        "CREATE DATABASE Learningweb;", master);
    cmd.ExecuteNonQuery();
    master.Close();

    using var db = new SqlConnection(connStr);
    db.Open();

    // --- account table ---
    ExecuteIfNotExists(db, @"
        IF OBJECT_ID('dbo.account', 'U') IS NULL
        CREATE TABLE dbo.account (
            aid         INT            IDENTITY(1,1) PRIMARY KEY,
            uname       NVARCHAR(100)  NOT NULL UNIQUE,
            password    NVARCHAR(200)  NOT NULL,
            email       NVARCHAR(200)  NULL
        );");

    // --- subject table (손흥민 학습 주제) ---
    ExecuteIfNotExists(db, @"
        IF OBJECT_ID('dbo.subject', 'U') IS NULL
        CREATE TABLE dbo.subject (
            mid      INT            NOT NULL PRIMARY KEY,
            mname    NVARCHAR(100)  NOT NULL,
            description NVARCHAR(500) NULL
        );

        IF NOT EXISTS (SELECT 1 FROM dbo.subject)
        BEGIN
            INSERT INTO dbo.subject (mid, mname, description) VALUES
                (1, 'Childhood & Family',
                    'Born in Chuncheon, South Korea. Son learned football from his father, former player Son Woong-jung. They made a promise: never give up, no matter what.'),
                (2, 'Hamburg & Leverkusen',
                    'At 16, Son moved to Europe alone. He joined Hamburger SV, then Bayer Leverkusen — learning German by watching SpongeBob SquarePants in German. He also learned English through daily life.'),
                (3, 'Tottenham Hotspur',
                    'Ten years at Spurs. He became the club''s all-time top Asian scorer, won the Puskás Award in 2020, and was named captain. He speaks confidently in English interviews.'),
                (4, 'South Korea National Team',
                    'Son captains the South Korea national team. He led the team to an Asian Games gold medal in 2018, and has represented Korea in multiple World Cups. He speaks about responsibility as captain.'),
                (5, 'Language & Communication',
                    'Son is fluent in Korean, German, and English. Cambridge University Press made a video about his language journey. He says: ''Of course you will make mistakes, but it''s most important you try and then fix the problems.'''),
                (6, 'LAFC & Beyond',
                    'In 2025, Son moved to Los Angeles FC. His debut goal was an instant classic. He speaks about new challenges, teamwork, and his vision for the future of football.');
        END;");

    // --- video table ---
    ExecuteIfNotExists(db, @"
        IF OBJECT_ID('dbo.video', 'U') IS NULL
        CREATE TABLE dbo.video (
            vid     INT            IDENTITY(1,1) PRIMARY KEY,
            mid     INT            NOT NULL,
            vname   NVARCHAR(200)  NOT NULL,
            vdesc   NVARCHAR(500)  NULL,
            vfile   NVARCHAR(500)  NULL
        );");

    // Seed videos only if table is empty.
    SeedVideosIfEmpty(db);
    SeedSubjectsIntoDB();

    // --- catalog (watch history) ---
    ExecuteIfNotExists(db, @"
        IF OBJECT_ID('dbo.catalog', 'U') IS NULL
        CREATE TABLE dbo.catalog (
            cid     INT            IDENTITY(1,1) PRIMARY KEY,
            aid     INT            NOT NULL REFERENCES dbo.account(aid),
            vid     INT            NOT NULL REFERENCES dbo.video(vid),
            ontime  NVARCHAR(50)   NOT NULL
        );");

    // --- watchcheck ---
    ExecuteIfNotExists(db, @"
        IF OBJECT_ID('dbo.watchcheck', 'U') IS NULL
        CREATE TABLE dbo.watchcheck (
            wcid        INT            IDENTITY(1,1) PRIMARY KEY,
            aid         INT            NOT NULL REFERENCES dbo.account(aid),
            vid         INT            NOT NULL REFERENCES dbo.video(vid),
            mid         INT            NOT NULL,
            ontime      NVARCHAR(50)   NOT NULL,
            alreadywatch BIT          NOT NULL DEFAULT 0
        );");

    ExecuteIfNotExists(db, @"
        IF COL_LENGTH('dbo.watchcheck', 'ontime') IS NULL
            ALTER TABLE dbo.watchcheck ADD ontime NVARCHAR(50) NULL;");

    // --- accountmember (user × subject) ---
    ExecuteIfNotExists(db, @"
        IF OBJECT_ID('dbo.accountmember', 'U') IS NULL
        CREATE TABLE dbo.accountmember (
            amid    INT            IDENTITY(1,1) PRIMARY KEY,
            aid     INT            NOT NULL REFERENCES dbo.account(aid),
            mid     INT            NOT NULL
        );" );

    db.Close();
}

static void ExecuteIfNotExists(SqlConnection conn, string sql)
{
    using var cmd = new SqlCommand(sql, conn);
    cmd.ExecuteNonQuery();
}

static void SeedVideosIfEmpty(SqlConnection conn)
{
    using var check = new SqlCommand("SELECT COUNT(*) FROM dbo.video", conn);
    var count = (int)check.ExecuteScalar();
    if (count > 0) return;

    var subjects = new Dictionary<int, (string name, string desc)>
    {
        [1] = ("Childhood & Family", "Born in Chuncheon, South Korea. Son learned football from his father, former player Son Woong-jung. They made a promise: never give up, no matter what."),
        [2] = ("Hamburg & Leverkusen", "At 16, Son moved to Europe alone. He joined Hamburger SV, then Bayer Leverkusen — learning German by watching SpongeBob SquarePants in German. He also learned English through daily life."),
        [3] = ("Tottenham Hotspur", "Ten years at Spurs. He became the club's all-time top Asian scorer, won the Puskás Award in 2020, and was named captain. He speaks confidently in English interviews."),
        [4] = ("South Korea National Team", "Son captains the South Korea national team. He led the team to an Asian Games gold medal in 2018, and has represented Korea in multiple World Cups. He speaks about responsibility as captain."),
        [5] = ("Language & Communication", "Son is fluent in Korean, German, and English. Cambridge University Press made a video about his language journey. He says: \"Of course you will make mistakes, but it's most important you try and then fix the problems.\""),
        [6] = ("LAFC & Beyond", "In 2025, Son moved to Los Angeles FC. His debut goal was an instant classic. He speaks about new challenges, teamwork, and his vision for the future of football.")
    };

    var videos = new (int mid, string vname, string vdesc, string vfile)[]
    {
        // Childhood & Family (mid=1)
        (1, "Tottenham and Sonny — Tom Holland Interviews Heung-min Son",
            "Tom Holland (Spider-Man actor) interviews Son. Son talks about his Puskás Award goal, his dad, and how he got into football. Real English conversation.",
            "https://www.youtube.com/embed/WoMbFNgh-9I"),
        (1, "Son's Father: 'I Just Want My Son to Be Happy in the Stadium'",
            "Son Woong-jung, Son's father and former footballer, talks about his son's journey. Includes the childhood promise they made — never give up.",
            "https://www.youtube.com/embed/rsgWhk4lZIQ"),
        // Hamburg & Leverkusen (mid=2)
        (2, "From Hamburg to Leverkusen — Son's Early European Years",
            "Son moved to Germany at 16. He learned German by watching SpongeBob in German. He played for Hamburger SV and Bayer Leverkusen before moving to England.",
            "https://www.youtube.com/embed/rMI1Ev2QkWk"),
        (2, "Son Learns German with SpongeBob — Cambridge English",
            "Cambridge University Press documented how Son learned German. He watched SpongeBob SquarePants in German to pick up the language. Also talks about learning English through friends.",
            "https://www.youtube.com/embed/k90apZnDtzo"),
        // Tottenham Hotspur (mid=3)
        (3, "Son Heung-min | First Interview at Tottenham (10 Years)",
            "Son's first interview reflecting on 10 years at Spurs. He speaks in clear, natural English about his journey, his captaincy, and what it means to him.",
            "https://www.youtube.com/embed/K4L0ChJ-FHI"),
        (3, "Son's Puskás Award Goal — The Famous Solo Run",
            "Son's incredible solo goal that won the 2020 Puskás Award. In interviews he explains how he did it. A perfect example of his skill and his ability to talk about it in English.",
            "https://www.youtube.com/embed/rsgWhk4lZIQ"),
        // South Korea National Team (mid=4)
        (4, "Son Heung-min — South Korea National Team Captain",
            "Son captains South Korea. He talks about the responsibility of leading his country, learning from national heroes like Park Ji-sung and Ki Sung-yueng.",
            "https://www.youtube.com/embed/nktY2p9nVro"),
        (4, "Son Leads Korea to Asian Games Gold — 2018",
            "Son helped South Korea win gold at the 2018 Asian Games. This exempted him from military service. He speaks about the pressure and the team spirit.",
            "https://www.youtube.com/embed/rsgWhk4lZIQ"),
        // Language & Communication (mid=5)
        (5, "Cambridge English: Son Heung-min's Language Journey",
            "Cambridge University Press video — Son talks about learning English, making mistakes, and why practice matters. A great English-learning resource directly from Son.",
            "https://www.youtube.com/embed/WoMbFNgh-9I"),
        (5, "Son's English Interview Tips — How He Communicates",
            "Son explains how he learned to speak English confidently. He says the most important thing is to try, make mistakes, and fix them. He practiced with friends in the UK.",
            "https://www.youtube.com/embed/WoMbFNgh-9I"),
        // LAFC & Beyond (mid=6)
        (6, "Son Heung-min — LAFC Debut Goal (First Chapter)",
            "Son's instant classic debut goal for LAFC. He speaks about the move to Los Angeles, the dream come true, and his vision for the future.",
            "https://www.youtube.com/embed/rMI1Ev2QkWk"),
        (6, "Son Heung-min — FIFA World Cup 2026 'Next Starts Now'",
            "Son's Hyundai campaign for the FIFA World Cup 2026. He speaks about the future of football and his own next chapter. Short, powerful English.",
            "https://www.youtube.com/embed/rsgWhk4lZIQ")
    };

    using var ins = new SqlCommand(
        "INSERT INTO dbo.video (mid, vname, vdesc, vfile) VALUES (@mid, @vname, @vdesc, @vfile)", conn);
    ins.Parameters.Add("@mid", SqlDbType.Int);
    ins.Parameters.Add("@vname", SqlDbType.NVarChar, 200);
    ins.Parameters.Add("@vdesc", SqlDbType.NVarChar, 500);
    ins.Parameters.Add("@vfile", SqlDbType.NVarChar, 500);

    foreach (var (mid, vname, vdesc, vfile) in videos)
    {
        ins.Parameters["@mid"].Value = mid;
        ins.Parameters["@vname"].Value = vname;
        ins.Parameters["@vdesc"].Value = vdesc;
        ins.Parameters["@vfile"].Value = vfile;
        ins.ExecuteNonQuery();
    }
}

static void SeedSubjectsIntoDB()
{
    // No-op: subjects are seeded inside EnsureDatabaseAndSchema.
}
