# Movies API – Mini‑POC (.NET 8, Swagger, Docker)

En liten **proof‑of‑concept** for et REST‑API i **.NET 8**. Prosjektet kjører rett i **GitHub Codespaces** (ingen lokal installasjon), har **Swagger**, **health check**, **CORS**, **Dockerfile** og **GitHub Actions (CI)** for bygg og container.

> Rask demo, lett å vise frem i intervju, og et fint utgangspunkt for å lære .NET 8 + cloud‑flyt.

---

## ⚡️ TL;DR (kjør i Codespaces – null installasjon)
1. Åpne repoet → **Code → Codespaces → Create codespace on main**
2. I terminalen:
   ```bash
   cd Movies.Api
   dotnet run
   ```
3. Når port **5000** dukker opp → **Open in Browser**  
   - Swagger: `/swagger`  
   - Health: `/health`

> Tips: I **PORTS**-panelet kan du sette port 5000 til **Public** for å dele lenken.

---

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

---

## 🔌 Endepunkter
- `GET /api/movies` – liste (støtter `?q=` søk)
- `GET /api/movies/{id}` – hente én
- `POST /api/movies` – opprett
- `PUT /api/movies/{id}` – oppdater
- `DELETE /api/movies/{id}` – slett
- `GET /health` – health check
- `GET /` – velkomst

### Eksempel POST‑body
```json
{
  "title": "Blade Runner",
  "year": 1982,
  "genres": ["Sci‑Fi", "Noir"],
  "rating": 8.1
}
```

---

## 📁 Strukturen i repoet
```
.
├─ Movies.Api/
│  ├─ Program.cs                  # Minimal API + Swagger + Health + CORS + seed
│  ├─ Movies.Api.csproj           # .NET 8 web + Swagger-pakke
│  ├─ Dockerfile                  # Bygger slankt runtime-image
│  ├─ appsettings.json            # Konfig (tom nå)
│  └─ Properties/
│     └─ launchSettings.json      # Binder til http://0.0.0.0:5000 i dev
├─ .devcontainer/
│  └─ devcontainer.json           # Codespaces-miljø (port 5000 forwardes)
├─ .github/workflows/
│  └─ ci.yml                      # CI: dotnet build + docker build + push til GHCR
└─ docs/
   └─ for-dummies-no.md           # En enklere guide med forklaringer
```

---

## 🤖 CI (GitHub Actions + GHCR)
Workflow `./.github/workflows/ci.yml` gjør:
- `dotnet restore/build/publish`
- Docker buildx (multi‑platform klart)
- Push til **GitHub Container Registry (GHCR)** med tagger `latest` og commit‑SHA

Image publiseres som:  
`ghcr.io/<owner>/<repo>:<tag>`

> For push holder standard `GITHUB_TOKEN` (permissions er satt i workflow).

---

## 🛠 Feilsøking
- **Ingen port-popup?** Kjør: `dotnet run --urls http://0.0.0.0:5050` → åpne port 5050 i **PORTS**-panelet.
- **Restore feilet første gang?** `dotnet restore && dotnet run`
- **Ser 3 filmer uten å poste noe?** Det er **seed‑data** for demo.
- **Data forsvinner ved restart?** Lagring er **in-memory** i POC‑en (ingen database).

---

## 📘 Mer dokumentasjon
Se **[docs/for-dummies-no.md](docs/for-dummies-no.md)** for en veldig enkel forklaring på hva alt er og hvorfor.
