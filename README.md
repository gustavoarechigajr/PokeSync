<h1 align="center">PokeSync</h1>

<h4 align="center">Self-hosted, multi-user Pokémon save manager with an Android companion app</h4>

<p align="center">
    <img src="https://img.shields.io/badge/Platform-Docker%20|%20Android-informational" />
    <img src="https://img.shields.io/badge/License-GPLv3-blue.svg" />
    <img src="https://img.shields.io/badge/Backend-.NET%2010-purple" />
    <img src="https://img.shields.io/badge/Android-API%2029%2B-green" />
</p>

---

**PokeSync** extends the original [PKVault](https://github.com/Chnapy/PKVault) with multi-user JWT authentication and a dedicated REST API for its Android companion app. Run it on your own server, sync emulator save files from your phone, and manage your full Pokémon collection across games — all under your control.

## Features

- **Multi-user authentication** — register/log in with username/password; every endpoint is JWT-protected
- **Per-user isolation** — each user has their own database (`pkvault-{userId}.db`)
- **Vault-style storage** — Pokémon decoupled from save files, persisted across container restarts
- **Cross-format conversion** — PK1 → PK9, PA8 (Legends: Arceus), PB8 (BDSP), PA9 (Legends: Z-A); preserves moves, held items, ball, and shiny status
- **Pre-transfer compatibility validation** — `POST /api/android/vault/{id}/validate-export` blocks transfers the destination game can't render before any byte hits the save
- **Drag-and-drop transfers** — move Pokémon between saves and the vault from the Android app
- **Save session disk persistence** — uploaded saves survive container restarts
- **Full save browsing** — view every Pokémon in every box

## Deployment (Docker)

```yaml
services:
  pokesync:
    image: ghcr.io/gustavoarechigajr/pokesync:latest
    restart: unless-stopped
    ports:
      - "5100:5000"
    environment:
      - JWT_KEY=your-secret-key-min-32-chars-change-this
    volumes:
      - /path/to/pokesync-data:/pkvault
```

> The container's internal HTTP port is `5000`; the example maps it to host port `5100`.

## Configuration

| Variable           | Required | Default                       | Description |
|--------------------|----------|-------------------------------|-------------|
| `JWT_KEY`          | Yes      | —                             | Base64 secret (≥32 chars) used to sign JSON Web Tokens |
| `ASPNETCORE_URLS`  | No       | `http://0.0.0.0:5000`         | Override the listening address/port |

## API endpoints

| Method | Endpoint                                                    | Auth | Description |
|--------|-------------------------------------------------------------|------|-------------|
| POST   | `/api/auth/register`                                        | No   | Create a new account |
| POST   | `/api/auth/login`                                           | No   | Obtain a JWT token |
| POST   | `/api/android/saves/upload`                                 | Yes  | Upload a save file (multipart) |
| GET    | `/api/android/saves/{saveId}`                               | Yes  | Get save metadata |
| GET    | `/api/android/saves/{saveId}/pokemon`                       | Yes  | List Pokémon in the save |
| GET    | `/api/android/vault`                                        | Yes  | List vault contents |
| POST   | `/api/android/vault/import/{saveId}`                        | Yes  | Bulk-import a save's Pokémon into the vault |
| POST   | `/api/android/vault/add/{saveId}`                           | Yes  | Add one Pokémon from a save into the vault |
| POST   | `/api/android/vault/{vaultId}/validate-export`              | Yes  | Validate compatibility before exporting |
| POST   | `/api/android/vault/{vaultId}/export`                       | Yes  | Export a Pokémon back into a cached save |
| DELETE | `/api/android/vault/{id}`                                   | Yes  | Remove a Pokémon from the vault |

For canonical definitions, see [`PKVault.Backend/android/routes/AndroidController.cs`](PKVault.Backend/android/routes/AndroidController.cs). Original PKVault endpoints (banks, boxes, transfers, editing) all require auth and work as documented upstream.

## Setup from scratch

### 1. Get the software

**Option A — Pull the prebuilt Docker image** (recommended)
```bash
docker pull ghcr.io/gustavoarechigajr/pokesync:latest
```

**Option B — Build from source** (.NET 10 SDK required)
```bash
git clone https://github.com/gustavoarechigajr/PokeSync.git
cd PokeSync
dotnet publish PKVault.Backend/PKVault.Backend.csproj -c Release -o ./publish
```

### 2. Generate a JWT signing key
```bash
openssl rand -base64 32
```
Copy the output — you'll need it for `JWT_KEY`.

### 3. Pick a deployment method

**a) Plain Docker Compose** — `docker-compose.yml`:
```yaml
services:
  pokesync:
    image: ghcr.io/gustavoarechigajr/pokesync:latest
    restart: unless-stopped
    ports:
      - "5100:5000"
    environment:
      - JWT_KEY=<key from step 2>
    volumes:
      - ./pokesync-data:/pkvault
```
```bash
docker compose up -d
```

**b) TrueNAS Custom App** — paste this YAML into the Custom App editor:
```yaml
services:
  pokesync:
    image: ghcr.io/gustavoarechigajr/pokesync:latest
    restart: unless-stopped
    ports:
      - "5100:5000"
    environment:
      - JWT_KEY=<key from step 2>
    volumes:
      - /mnt/pool/apps/pokesync/data:/pkvault
```
Make sure the volume path exists. Redeploy via `midclt call app.redeploy pokesync` after image updates.

### 4. Verify it's running
```bash
curl -i http://localhost:5100/api/auth/register
# Expect HTTP/1.1 405 — server alive, just rejecting GET
```

### 5. (Optional) Expose remotely via Cloudflare Tunnel
1. Have a domain managed in Cloudflare and a Cloudflare Tunnel running on the host (TrueNAS app `cloudflared` works).
2. Add a public hostname route from `pokesync.<your-domain>` to `http://localhost:5100` and a CNAME pointing the subdomain at the tunnel.
3. The companion `gustavoarechigajr/home-network` repo has `.github/workflows/add-tunnel-route.yml` that automates both via the Cloudflare API — supply `CLOUDFLARE_GLOBAL_TOKEN` as a repo secret and run `gh workflow run add-tunnel-route.yml -f hostname=pokesync -f origin=http://localhost:5100`.
4. Use the resulting `https://pokesync.<your-domain>` as the backend URL on the Android client.

### 6. Register your first user
```bash
curl -X POST http://localhost:5100/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"trainer1","password":"Pikachu123"}'
```

### 7. Point the Android app at it
- Install the [PokeSync-Android](https://github.com/gustavoarechigajr/PokeSync-Android) APK on your device.
- Open the app and type your server URL into the Connect screen (e.g. `http://10.0.0.10:5100` on LAN, or `https://pokesync.<your-domain>` over Cloudflare).
- Log in with the credentials from step 6.

You're ready to upload saves and start moving Pokémon around.

## Companion Android app

[PokeSync-Android](https://github.com/gustavoarechigajr/PokeSync-Android) — scan emulator save files, drag-and-drop transfers, vault management, and remote access from anywhere.

## Disclaimer

For personal and research use only. **Do not use it for cheating in online play.** Modifying or transferring Pokémon in ways that violate game rules can lead to bans on official services.

## License

Licensed under the **GNU General Public License v3.0** — see [LICENSE](LICENSE).

## Credits

- [PKVault](https://github.com/Chnapy/PKVault) — original save manager by **Chnapy**
- [PKHeX.Core](https://github.com/kwsch/PKHeX) — Pokémon save file parsing library by **kwsch**
