# Launch Ashbinders GUI in Docker on Windows (Docker Desktop + WSL2/WSLg)
Write-Host "=== Launching Ashbinders GUI in Docker ===" -ForegroundColor Cyan

if (-not $env:DISPLAY) { $env:DISPLAY = ":0" }

docker compose --profile windows build gui-win
docker compose --profile windows run --rm gui-win @args