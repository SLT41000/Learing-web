# ============================================================
# Learing Web - Dockerfile
# ============================================================
# .NET Framework 4.8 ASP.NET Web Forms + WCF Services
# REQUIRES Windows containers (not compatible with Linux containers)
#
# Before building, switch Docker Desktop to Windows containers:
#   Docker tray menu -> "Switch to Windows containers..."
#
# Usage:
#   docker build -t learning-web .
#   docker run -d -p 8080:80 --name learning-web learning-web
#
# With docker-compose (uses SQL Server):
#   docker compose up -d
# ============================================================

# ---- Stage 1: Build image (SDK) ----
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build

WORKDIR /src

# Restore NuGet packages first (cached layer)
COPY ["Learing web/Learing web.csproj", "Learing web/"]
COPY ["Learing web/packages.config", "Learing web/"]
RUN nuget restore "Learing web/Learing web.csproj" -PackagesDirectory ./packages

# Copy all source files and build
COPY Learing-web/ ./Learing-web/
WORKDIR /src/Learing web
RUN msbuild "Learing web.csproj" /p:Configuration=Release /p:Platform=AnyCPU /t:Build /v:quiet

# ---- Stage 2: Runtime image (IIS + ASP.NET 4.8) ----
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# Enable Windows features needed by ASP.NET
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Set working directory to IIS default web root
WORKDIR /inetpub/wwwroot

# Copy build output from stage 1
COPY --from=build /src/Learing\ web/bin/ ./bin/
COPY --from=build /src/Learing\ web/ ./

# Remove any default IIS files and set our content as root
RUN Remove-Item -Recurse -Force C:/inetpub/wwwroot/iisstart.* -ErrorAction SilentlyContinue

# Connection string for SQL Server (override at runtime with -e flag or compose)
# Format: Data Source=hostname;Initial Catalog=Learningweb;User Id=sa;Password=...;
ENV ConnectionString="Data Source=localhost;Initial Catalog=Learningweb;Integrated Security=True;"

# HTTP port
EXPOSE 80

# IIS runs automatically in the base image
