#!/usr/bin/env bash
set -e

export QT_X11_NO_MITSHM=1
export _X11_NO_MITSHM=1

echo "=== Compiling C# Assemblies in Docker Container ==="
dotnet build /app/game/Ashbinders.csproj -c Debug

ARGS=("$@")
HAS_RENDER_DRIVER=false
for arg in "$@"; do
    if [[ "$arg" == "--rendering-driver" ]]; then
        HAS_RENDER_DRIVER=true
        break
    fi
done

if [ "$HAS_RENDER_DRIVER" = false ]; then
    ARGS+=("--rendering-driver" "opengl3")
fi

echo "=== Launching Godot Game Engine ==="
godot --path /app/game res://scenes/test/gym_underlevels.tscn "${ARGS[@]}"
