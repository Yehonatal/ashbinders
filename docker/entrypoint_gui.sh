#!/usr/bin/env bash
set -e

export QT_X11_NO_MITSHM=1
export _X11_NO_MITSHM=1

if [ -f /tmp/pulse-cookie ]; then
    export PULSE_COOKIE=/tmp/pulse-cookie
fi

echo "=== Compiling C# Assemblies in Docker Container ==="
dotnet build /app/game/Ashbinders.csproj -c Debug

ARGS=("$@")
HAS_RENDER_DRIVER=false
HAS_AUDIO_DRIVER=false

for arg in "$@"; do
    if [[ "$arg" == "--rendering-driver" ]]; then
        HAS_RENDER_DRIVER=true
    fi
    if [[ "$arg" == "--audio-driver" ]]; then
        HAS_AUDIO_DRIVER=true
    fi
done

if [ "$HAS_RENDER_DRIVER" = false ]; then
    ARGS+=("--rendering-driver" "opengl3")
fi

if [ "$HAS_AUDIO_DRIVER" = false ]; then
    if [ -S /tmp/pulse-socket ] || [ -c /dev/snd/controlC0 ]; then
        ARGS+=("--audio-driver" "PulseAudio")
    else
        ARGS+=("--audio-driver" "Dummy")
    fi
fi

echo "=== Launching Godot Game Engine ==="
godot --path /app/game res://world/regions/underlevels/gym_underlevels.tscn "${ARGS[@]}"
