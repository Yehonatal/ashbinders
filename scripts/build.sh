#!/usr/bin/env bash
set -e

echo "=== Building Ashbinders Game C# Solution ==="
dotnet build game/Ashbinders.csproj -c Release

echo "=== Validating Assets ==="
python3 scripts/validate_assets.py

echo "=== Build Completed Successfully ==="
