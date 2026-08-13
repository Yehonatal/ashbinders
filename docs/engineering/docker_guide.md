# Docker Development & Execution Guide (Linux & Windows)

This document provides complete instructions for running Ashbinders in Docker containers on both **Linux** and **Windows**.

---

## 1. Prerequisites

### Linux
- Docker Engine & Docker Compose (`docker compose version` >= 2.0).
- For Desktop GUI mode: X11 display server (`xhost` utility).

### Windows
- [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop/) with WSL 2 backend enabled.
- For Desktop GUI mode on Windows:
  - **Windows 11**: Native GUI support via WSLg (works out of the box).
  - **Windows 10**: An X11 server like [VcXsrv](https://sourceforge.net/projects/vcxsrv/) with "Disable access control" checked.

---

## 2. Container Services Overview

| Service | Target Use Case | Linux Command | Windows (PowerShell) Command |
| :--- | :--- | :--- | :--- |
| **`test`** | Automated compilation, asset checks, and unit tests | `./scripts/docker_test.sh` | `.\scripts\docker_test.ps1` |
| **`web`** | Serve HTML5/WASM export with COOP/COEP headers on port 8080 | `./scripts/docker_web.sh` | `.\scripts\docker_web.ps1` |
| **`gui`** | Run Godot desktop game client directly from container | `./scripts/docker_gui.sh` | `.\scripts\docker_gui.ps1` |

---

## 3. Running on Linux

### A. Run Automated Tests
```bash
./scripts/docker_test.sh
```
Or with direct Docker Compose:
```bash
docker compose run --rm test
```

### B. Run Web Server
```bash
./scripts/docker_web.sh
```
Open your browser at `http://localhost:8080`.

### C. Run Desktop GUI Game Client
1. Allow local X11 display access:
   ```bash
   xhost +local:root
   ```
2. Launch GUI container:
   ```bash
   ./scripts/docker_gui.sh
   ```

---

## 4. Running on Windows

### A. Run Automated Tests (PowerShell)
```powershell
.\scripts\docker_test.ps1
```
Or direct command:
```powershell
docker compose run --rm test
```

### B. Run Web Server (PowerShell)
```powershell
.\scripts\docker_web.ps1
```
Open your browser at `http://localhost:8080`.

### C. Run Desktop GUI Game Client on Windows

#### Option 1: Windows 11 (WSLg - Recommended)
1. Open your WSL 2 terminal (e.g. Ubuntu).
2. Run:
   ```bash
   ./scripts/docker_gui.sh
   ```
   *WSLg will automatically forward the Godot window onto your Windows desktop.*

#### Option 2: Windows 10 (VcXsrv)
1. Launch **XLaunch** (VcXsrv).
2. Select:
   - Multiple windows
   - Display number: `0`
   - Start no client
   - Check **"Disable access control"**
3. In PowerShell, run:
   ```powershell
   .\scripts\docker_gui.ps1
   ```

---

## 5. Web Export Deployment

Godot 4 WebAssembly uses multi-threaded SharedArrayBuffers, which require strict Cross-Origin Isolation headers:
- `Cross-Origin-Opener-Policy: same-origin`
- `Cross-Origin-Embedder-Policy: require-corp`

The included `docker/nginx.conf` applies these headers automatically. When you export your HTML5 build from Godot into `export/web/`, it becomes instantly playable through the `web` container.
