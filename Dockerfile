# ============================================================
# Learing Web - Dockerfile
# ============================================================
# .NET Framework 4.8 ASP.NET Web Forms + WCF Services
# Requires Windows containers (not compatible with Linux containers)
# ------------------------------------------------------------
# Usage:
#   docker build -t learning-web .
#   docker run -d -p 8080:80 --name learning-web learning-web
#
# To run with SQL Server, use docker-compose:
#   docker compose up -d
# ============================================================

# ---- Stage 1: Build image (SDK) ----
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /src

# Restore NuGet packages
COPY ["Learing web/Learing web.csproj", "Learing web/"]
COPY ["Learing web/packages.config", "Learing web/"]
RUN nuget restore "Learing web/Learing web.csproj" -PackagesDirectory ../packages

# Copy all source and build
COPY Learing-web/ ./Learing-web/
WORKDIR /src/Learing web
RUN msbuild "Learing web.csproj" /p:Configuration=Release /p:OutputPath=bin /t:Build /v:quiet

# ---- Stage 2: Runtime image (IIS) ----
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime
WORKDIR /inetpub/wwwroot

# Copy build output
COPY --from=build /src/Learing web/bin/ ./bin/
COPY --from=build /src/Learing web/ ./

# Set default page
RUN echo "<%@ Page Language=\"C#\" AutoEventWireup=\"true\" %><html><body>Redirecting...<script>window.location='default.aspx';</script></body></html>" > default.aspx

# Connection string via environment variable (set with docker run -e or docker-compose)
# Default is for local SQL Server (development only)
ENV ConnectionString="Data Source=localhost;Initial Catalog=Learningweb;Integrated Security=True;"

EXPOSE 80
