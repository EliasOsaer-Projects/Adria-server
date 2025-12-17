# Genesis-server

Minimal guide to run the API locally.

## Prerequisites
- .NET 8 SDK
- MySQL reachable with the connection string in `src/Adria.Main/appsettings.Development.json` (update as needed)

## Run
```bash
dotnet restore
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
