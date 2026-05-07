<p align="center">
    <img height="200" src="frontend\public\logo.svg" alt="PKVault logo" />
</p>

<h1 align="center">PokeSync</h1>

<h4 align="center">A self-hosted Pokémon save manager with Android companion app</h4>

<p align="center">
    <img src="https://img.shields.io/badge/Platform-Docker%20|%20Android-informational" />
    <img src="https://img.shields.io/badge/License-GPLv3-blue.svg" />
    <img src="https://img.shields.io/badge/Backend-.NET%2010-purple" />
    <img src="https://img.shields.io/badge/Android-API%2029%2B-green" />
</p>

---

**PokeSync** is a fork of [PKVault](https://github.com/Chnapy/PKVault) extended with multi-user JWT authentication and an Android companion app. Run it on your homelab and sync Pokémon save files directly from your Android emulators.

## What's new in this fork

### Multi-user server
- **JWT authentication** — register and log in with a username/password; every API endpoint is protected
- **Per-user databases** — each user gets isolated storage (`pkvault-{userId}.db`); original PKVault data model unchanged
- **Android save upload API** — `POST /api/android/saves/upload` accepts a raw save file, parses it with PKHeX, and caches the result
- **Browse API** — `GET /api/android/saves/{saveId}/pokemon` returns the full Pokémon roster for a save

### Android companion app ([PokeSync-Android](https://github.com/gustavoarechigajr/PokeSync-Android))
- **Emulator scanner** — automatically finds save files from RetroArch, Azahar, Eden, DraStic, My Boy!, and ClassicBoy
- **Save registry** — add a save once; the app remembers it forever
- **One-tap sync** — tap Sync on any registered save to re-upload the latest version from your device
- **Browse Pokémon** — view your full box roster with species, level, shiny status, and nature
- **Save as backup** — every sync keeps a server-side copy of your save, recoverable if the device file is lost
- **Manual file picker** — pick any `.sav`/`.bin`/etc. file via the system file browser as a fallback
- **Unrestricted file access** — supports `MANAGE_EXTERNAL_STORAGE` to read Eden's `Android/data/` directory

## Docker deployment

```yaml
services:
  pokesync:
    image: ghcr.io/gustavoarechigajr/pokesync:latest
    container_name: pokesync
    restart: unless-stopped
    ports:
      - "5100:5000"
    environment:
      - JWT_KEY=your-secret-key-min-32-chars-change-this
    volumes:
      - /path/to/pokesync-data:/pkvault
```

Generate a secure key:
```bash
openssl rand -base64 32
```

## API

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | No | Create account |
| POST | `/api/auth/login` | No | Get JWT token |
| POST | `/api/android/saves/upload` | Yes | Upload save file (multipart) |
| GET | `/api/android/saves/{saveId}` | Yes | Get save info |
| GET | `/api/android/saves/{saveId}/pokemon` | Yes | Get Pokémon list |

All other PKVault endpoints require auth and work as documented in the original project.

## Original PKVault features

- Compatible with all Pokémon games from Gen 1 to **Pokémon Legends: Z-A**
- Move Pokémon between saves
- Convert Pokémon across generations (e.g. Gen 7 → Gen 2)
- Banks & boxes for storage outside saves
- Edit moves, EVs, nickname, evolve trade-evolution Pokémon
- Centralized Pokédex across all saves
- Automatic backups before every save action

---

*Forked from [PKVault](https://github.com/Chnapy/PKVault) by Chnapy — licensed under GPLv3.*
