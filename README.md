# Learing Web

A .NET 8 Razor Pages learning platform for practicing English through lessons based on Son Heung-min's story. Browse topics, search lessons, watch embedded videos, and track completed lessons after signing in.

## Features

- Responsive course catalog with topic filters and instant search
- Account registration and login with BCrypt password hashing
- Video lessons with check-in and completion tracking
- Watch history and completed lesson pages
- SQL Server-backed data with startup schema and sample content initialization

## Run with Docker Compose

Prerequisites: Docker Desktop with Docker Compose. On Apple Silicon, SQL Server runs as `linux/amd64` and uses Docker Desktop's x86 emulation.

```bash
cp .env.example .env
```

Edit `.env` and replace `MSSQL_SA_PASSWORD` with a unique strong password (at least 8 characters, including uppercase, lowercase, a number, and a symbol). Then run:

```bash
docker compose up --build -d
docker compose logs -f web
```

Open [http://localhost:8080](http://localhost:8080). Set `WEB_PORT` in `.env` to change the web host port. SQL Server is also available on `127.0.0.1:1433` for local database tools; set `SQLSERVER_PORT` to change that host port. Compose waits until SQL Server passes its health check before starting the app.

Stop the containers while keeping database data:

```bash
docker compose down
```

To also delete the database volume and all stored data:

```bash
docker compose down -v
```

The app creates the database schema and sample course content on first startup. SQL Server data is stored in the `sql-data` Docker volume. If you reuse a database volume from an earlier run, keep using that volume's existing SA password; changing the environment value does not update SQL Server's persisted credentials. `.env` is ignored by Git; do not commit it.

## Run locally

Install the .NET 8 SDK and provide a SQL Server connection string through `CONNECTION_STRING` or `ConnectionStrings:DefaultConnection`, then run:

```bash
dotnet run --project LearingWeb/LearingWeb.csproj
```

## Project layout

- `LearingWeb/` — ASP.NET Core Razor Pages application
- `LearingWeb/Pages/` — home, authentication, lesson, history, certificate, and about pages
- `LearingWeb/wwwroot/` — local stylesheets and course images
- `docker-compose.yml` — web and SQL Server orchestration
- `Dockerfile` — root-context container build
- `LearingWeb/Dockerfile` — application-context multi-stage build

## API

| Route | Method | Description |
|---|---|---|
| `/api/all-videos` | GET | Returns the available lessons as JSON |
