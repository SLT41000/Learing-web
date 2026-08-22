# Learing Web

A .NET Framework ASP.NET Web Forms learning platform — an educational web application for browsing, watching, and tracking progress through video courses across multiple subjects.

## Features

- **Course Catalog** — Browse video courses by subject (Physics, Chemistry, Biology, Calculus, Statistics, Applied Mathematics)
- **User Authentication** — Register and login with username/password
- **Subject Enrollment** — Users select subjects during signup; the home page filters to enrolled subjects only
- **Video Player** — Watch course videos with a responsive 16:9 iframe player
- **Progress Tracking** — "Check In" to record when you start a video; "Mark as Watched" to track completion
- **Certificate Gallery** — View certificates for courses you've completed
- **Watch History** — Catalog page shows all videos you've started watching with dates

## Project Structure

```
Learing-web/
├── Learing web/
│   ├── default.aspx          — Home page with subject cards
│   ├── default.aspx.cs       — Home page code-behind (auth, subject filtering)
│   ├── Login.aspx            — Login page
│   ├── Login.aspx.cs         — Login logic (auth, session setup)
│   ├── Signin.aspx           — Registration page
│   ├── Signin.aspx.cs        — Registration logic (account creation, subject selection)
│   ├── watch.aspx            — Video player page
│   ├── watch.aspx.cs         — Video check-in / mark-watched handlers
│   ├── Catalog.aspx          — Watch history table
│   ├── Catalog.aspx.cs       — Catalog data retrieval
│   ├── Certificate.aspx      — Earned certificates gallery
│   ├── Certificate.aspx.cs   — Certificate page code-behind
│   ├── urldata.js            — Parses URL params to load video info
│   ├── DbHelper.cs           — Database helper (parameterized queries)
│   ├── member.cs             — Member data model
│   ├── StyleSheethome.css    — Main stylesheet
│   ├── videos.css            — Video page styles
│   ├── navbar.css            — Navbar overrides
│   ├── login.css             — Login page styles
│   ├── Service1.svc          — WCF service: get all videos
│   ├── Service2.svc          — WCF service: get user's watch history
│   ├── Service3.svc          — WCF service: get user's completed videos
│   └── img/                  — Subject icons and certificate image
└── README.md
```

## Tech Stack

- **Framework**: ASP.NET Web Forms (.NET Framework 4.7.2)
- **Frontend**: Bootstrap 4.6.1, jQuery 3.6.0, custom CSS
- **Backend**: C# code-behind, WCF Services (REST/JSON)
- **Database**: SQL Server (via ADO.NET with parameterized queries)
- **Authentication**: ASP.NET Session state

## Docker

This project requires **Windows containers** because it targets .NET Framework 4.8, which does not run on Linux.

### Prerequisites

- **Docker Desktop** installed and running
- Docker Desktop must be in **Windows containers mode** (Settings -> General -> "Use Windows containers")

### Build and Run (Single Container)

```powershell
# Build the image
docker build -t learning-web .

# Run it (will use local SQL Server via integrated security)
docker run -d -p 8080:80 --name learning-web learning-web
```

Access at `http://localhost:8080`

### Build and Run with SQL Server (Docker Compose)

```powershell
# Start both app and SQL Server
docker compose up -d

# View logs
docker compose logs -f

# Stop everything
docker compose down

# Stop and wipe database
docker compose down -v
```

Access the web app at `http://localhost:8080`

The database is accessible at `localhost:1433` with:
- User: `sa`
- Password: `YourStrong@Passw0rd1`

### Connection String

The connection string is configured via the `ConnectionString` environment variable or through `ConnectionStrings.config` (mounted as a volume). The default in `docker-compose.yml` connects to the included SQL Server container. For production, replace the connection string with your actual SQL Server credentials.

### Switching Docker Desktop to Windows Containers

If you see `no match for platform in manifest` errors:
1. Right-click the Docker tray icon
2. Select **"Switch to Windows containers..."**
3. Restart Docker Desktop
4. Try `docker build .` again

## API Endpoints

| Endpoint | Method | Description |
|---|---|---|
| `/Service1.svc/getScreenData` | GET | Returns all video courses (JSON) |
| `/Service2.svc/Submit_Click?aid={aid}` | GET | Returns user's watch history (JSON) |
| `/Service3.svc/getalrdy?aid={aid}` | GET | Returns user's completed videos (JSON) |

## Future Enhancements

- Password hashing (currently stored in plain text)
- ASP.NET membership/identity integration
- Video progress tracking with actual playback position
- PDF certificate generation
- Search and filter functionality
- Admin dashboard for course management
- Responsive video player with play/pause controls
