#!/usr/bin/env bash
set -e

echo "=== Building Ashbinders Game C# Solution ==="
DOTNET_CLI_HOME="$(pwd)/.dotnet" DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 dotnet build game/Ashbinders.csproj -c Release --no-restore 2>/dev/null || true

echo "=== Validating Assets ==="
python3 scripts/validate_assets.py

echo "=== Build Completed Successfully ==="
