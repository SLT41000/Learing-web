# ============================================================
# Learing Web - Dockerfile (ASP.NET Core 8, Linux containers)
# ============================================================
# Build:   docker build -t learning-web .
# Run:     docker run -d -p 8080:80 --name learning-web learning-web
# Compose: docker compose up -d
# ============================================================

# ---- Stage 1: Publish the app ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore (cached layer)
COPY ["LearingWeb.csproj", "./"]
RUN dotnet restore "LearingWeb.csproj"

# Copy source and publish
COPY . .
RUN dotnet publish "LearingWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- Stage 2: Runtime image (Linux, ASP.NET 8) ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "LearingWeb.dll"]
