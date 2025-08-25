# Movies API – Cloud‑Ready Plug‑and‑Play

Dette repoet er klart for **GitHub Codespaces** (ingen lokal installasjon), har **.devcontainer** konfig, **Swagger**, **health check**, **Dockerfile**, og en **GitHub Actions** CI som bygger og (valgfritt) pusher container til GHCR.

## 🚀 Start i Codespaces (anbefalt – null installasjon)
1. Opprett et nytt GitHub‑repo og last opp filene i denne ZIP‑en.
2. Klikk **Code ▸ Codespaces ▸ Create codespace on main**.
3. Når Codespace er klart, åpnes VS Code i nettleser. Terminalen kjører `postCreateCommand` (restore).
4. Kjør appen:
   ```bash
   cd Movies.Api
   dotnet run
   ```
5. Når Codespaces spør om å åpne port **5000**, velg **Open in Browser**.
   - Swagger: `http://<your-codespace-url>/swagger`
   - Health: `http://<your-codespace-url>/health`

> Devcontainer setter `ASPNETCORE_URLS=http://0.0.0.0:5000` og forwarder port **5000**.

## 🧪 Lokalt (hvis du har .NET 8 installert)
```bash
cd Movies.Api
dotnet restore
dotnet run
# Swagger: http://localhost:5000/swagger
```

## 🐳 Docker (valgfritt, lokalt eller i CI)
```bash
cd Movies.Api
docker build -t movies-api .
docker run -p 8080:8080 movies-api
# Swagger: http://localhost:8080/swagger
```

## 📦 GH Actions CI (build + Docker + push til GHCR)
Workflow: `.github/workflows/ci.yml` gjør:
- .NET restore/build/publish
- Docker buildx (multi‑platform ready)
- Push til **GitHub Container Registry (GHCR)** med tagger `latest` og commit‑SHA

For push må repoet ha Actions‑tillatelse **packages: write** (satt i workflow) og det holder å bruke `GITHUB_TOKEN` (default). Image legges da på:
`ghcr.io/<owner>/<repo>:latest`

## 🔌 Endepunkter
- `GET /api/movies` – liste (støtter `?q=` søk)
- `GET /api/movies/{id}` – hente én
- `POST /api/movies` – opprett
- `PUT /api/movies/{id}` – oppdater
- `DELETE /api/movies/{id}` – slett
- `GET /health` – health check
- `GET /` – velkomst

### Eksempel POST
```json
{
  "title": "Blade Runner",
  "year": 1982,
  "genres": ["Sci‑Fi", "Noir"],
  "rating": 8.1
}
```

## 🔧 Neste steg (valgfritt)
- EF Core + SQLite i stedet for in‑memory
- GitHub Actions: deploy til Azure Web App for Containers
- Legg på en enkel React‑frontend som kaller API‑et
