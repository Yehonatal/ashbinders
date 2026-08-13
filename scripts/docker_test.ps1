# PowerShell script to run Ashbinders automated tests in Docker
Write-Host "=== Running Ashbinders Tests in Docker ===" -ForegroundColor Cyan
docker compose build test
docker compose run --rm test
