# Genesis-server

Minimal guide to run the API locally.

## Prerequisites
- .NET 8 SDK
- Docker Desktop

## Run
```bash
git clone <server-repo-url>
cd <server-repo-folder>
docker compose -f config/docker/docker-compose.yml up -d
dotnet run --project src/Adria.Main
```
Server listens on `http://localhost:8000` by default; Swagger UI is at `/swagger`.

## Config
- Primary settings: `src/Adria.Main/appsettings.json`
- Local overrides: `src/Adria.Main/appsettings.Development.json`

## Tests
```bash
dotnet test
```
