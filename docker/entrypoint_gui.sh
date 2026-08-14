#!/usr/bin/env bash
set -e

export QT_X11_NO_MITSHM=1
export _X11_NO_MITSHM=1

echo "=== Compiling C# Assemblies in Docker Container ==="
dotnet build /app/game/Ashbinders.csproj -c Debug

echo "=== Launching Godot Game Engine ==="
godot --path /app/game res://scenes/test/gym_underlevels.tscn "$@"
