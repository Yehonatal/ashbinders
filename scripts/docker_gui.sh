#!/usr/bin/env bash
set -e

echo "=== Launching Ashbinders GUI in Docker with X11 Forwarding ==="

# Allow local root and current user access to X11 display
if command -v xhost >/dev/null 2>&1; then
    xhost +local:root >/dev/null 2>&1 || true
    xhost +SI:localuser:$(id -un) >/dev/null 2>&1 || true
fi

docker compose build gui
docker compose run --rm gui "$@"
