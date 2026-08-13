#!/usr/bin/env bash
set -e

echo "=== Running Ashbinders Automated Test Suite ==="
DOTNET_CLI_HOME="$(pwd)/.dotnet" dotnet run --project tests/Ashbinders.Tests.Runner.csproj
