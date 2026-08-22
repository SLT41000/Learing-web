# =============================================================
# Dockerfile for Learing Web (ASP.NET Web Forms, .NET Framework 4.7.2)
# =============================================================
# REQUIRES Windows containers mode in Docker Desktop.
# Switch: Docker tray menu -> "Switch to Windows containers..."
#
# Build:   docker build -t learing-web .
# Run:     docker run -d -p 8080:80 --name learing-web learing-web
# =============================================================

FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.2-windowsservercore-ltsc2019 AS build

WORKDIR /src

# Restore NuGet packages
COPY ["Learing web/Learing web.csproj", "Learing web/"]
COPY ["Learing web/packages.config", "Learing web/"]
RUN nuget restore "Learing web/Learing web.csproj"

# Copy project source and build
COPY "Learing web/" "Learing web/"
WORKDIR "/src/Learing web"
RUN msbuild "Learing web.csproj" /t:Build /p:Configuration=Release /p:DeployIisAppPath="Default Web Site"

# Stage 2: IIS runtime
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

WORKDIR C:/inetpub/wwwroot

# Copy built application from build stage
COPY --from=build "C:/src/Learing web/" C:/inetpub/wwwroot/

# Remove default IIS start page
RUN Remove-Item -Recurse -Force C:/inetpub/wwwroot/iisstart.htm -ErrorAction SilentlyContinue; \
    Remove-Item -Recurse -Force C:/inetpub/wwwroot/iisstart.png -ErrorAction SilentlyContinue

EXPOSE 80

# IIS starts automatically; keep container running
CMD ["C:\\ServiceMonitor.exe", "C:\\inetpub\\wwwroot"]
