# PokeSync Server

A self-hosted, multi-user Pokemon storage and transfer server. Fork of [PKVault](https://github.com/Chnapy/PKVault) extended with multi-user authentication and a REST API designed for the [PokeSync Android app](https://github.com/gustavoarechigajr/PokeSync-Android).

## What's Different from PKVault

| Feature | PKVault | PokeSync |
|---|---|---|
| Users | Single user | Multi-user with JWT auth |
| Data isolation | Global | Per-user Banks, Boxes, Pokemon |
| Android support | None | Full companion app API |
| Save transfer | Manual | Cross-save Pokemon transfer via app |

## Quick Start (Docker)

```bash
docker compose up -d
```

The server starts at `http://localhost:5000`. Create your account at first launch.

## Stack

- **Backend:** C# / .NET 10, Entity Framework Core, SQLite
- **Frontend:** TypeScript / React 19
- **Save parsing:** PKHeX.Core (Gen 1–9)

## License

GPLv3 — inherited from PKVault. See [LICENSE](LICENSE).

## Credits

Built on top of [PKVault](https://github.com/Chnapy/PKVault) by Chnapy.
