#!/usr/bin/env bash
set -euo pipefail

# PokeSync update script.
# Pulls the latest image from GHCR and restarts the container.
# Watchtower handles this automatically, but you can run this manually too.

cd "$(dirname "$0")"

echo "Pulling latest PokeSync image..."
docker compose pull pokesync

echo "Recreating container with latest image..."
docker compose up -d pokesync

echo "Removing old dangling images..."
docker image prune -f

echo "Done. Check logs with: docker compose logs -f pokesync"
