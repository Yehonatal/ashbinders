#!/usr/bin/env bash
set -e

echo "=== Compiling C# Assemblies in Docker Container ==="
dotnet build /app/game/Ashbinders.csproj -c Debug

echo "=== Launching Godot Game Engine ==="
godot --path /app/game res://scenes/test/gym_underlevels.tscn
