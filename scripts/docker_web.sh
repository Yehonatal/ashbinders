#!/usr/bin/env bash
set -e

echo "=== Starting Ashbinders Web Server Container ==="
mkdir -p export/web

if [ ! -f export/web/index.html ]; then
    echo "[WARN] No web export found in export/web/. Creating placeholder index.html..."
    cat << 'EOF' > export/web/index.html
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
EOF
fi

docker compose build web
docker compose up -d web
echo "[OK] Web server running at: http://localhost:8080"
