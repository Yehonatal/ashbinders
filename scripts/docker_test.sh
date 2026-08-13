#!/usr/bin/env bash
set -e

echo "=== Running Ashbinders Tests in Isolated Docker Container ==="
docker compose build test
docker compose run --rm test
