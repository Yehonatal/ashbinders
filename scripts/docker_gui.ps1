# PowerShell script to launch Ashbinders GUI in Docker on Windows (WSLg / VcXsrv)
Write-Host "=== Launching Ashbinders GUI in Docker ===" -ForegroundColor Cyan

# Check for WSLg or set DISPLAY to host IP
if (-not $env:DISPLAY) {
    # Default for VcXsrv / Xming
    $env:DISPLAY = "host.docker.internal:0.0"
}

docker compose build gui
docker compose run --rm -e DISPLAY=$env:DISPLAY gui
