# PowerShell script to run Ashbinders Web Server container
Write-Host "=== Starting Ashbinders Web Server Container ===" -ForegroundColor Cyan

if (-not (Test-Path "export/web")) {
    New-Item -ItemType Directory -Path "export/web" -Force | Out-Null
}

if (-not (Test-Path "export/web/index.html")) {
    Set-Content -Path "export/web/index.html" -Value @"
<!DOCTYPE html>
<html>
<head>
    <title>Ashbinders Web</title>
    <style>body { background: #14161a; color: #f0f0f0; font-family: sans-serif; text-align: center; padding-top: 50px; }</style>
</head>
<body>
    <h1>Ashbinders Web Export Container</h1>
    <p>Web server is active with Cross-Origin Isolation headers (COOP/COEP).</p>
    <p>Export your Godot HTML5 build to <code>export/web/</code> to play.</p>
</body>
</html>
"@
}

docker compose build web
docker compose up -d web
Write-Host "[OK] Web server running at: http://localhost:8080" -ForegroundColor Green
