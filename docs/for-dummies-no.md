# Movies API – For dummies (NO)

En mini‑POC som viser **.NET 8 Minimal API** med **Swagger**, **health check**, **CORS**, **Dockerfile**, **devcontainer (Codespaces)** og **GitHub Actions (CI)**.

## Hva kan du bruke dette til?

- Vise at du kan lage et **ekte REST‑API** (GET/POST/PUT/DELETE).
- Dele en **levende demo** i nettleser uten installasjon (Codespaces).
- Bygge et **Docker‑image** automatisk i GitHub og bruke det til deploy.
- Diskutere arkitektur og videre steg (DB, tester, Azure/Kubernetes).

---

## Slik kjører du (3 steg i Codespaces)

1. Repo → **Code → Codespaces → Create codespace on main**
2. Terminal:
   ```bash
   cd Movies.Api
   dotnet run
   ```
3. Når port **5000** dukker opp → **Open in Browser**
   - Swagger: `/swagger`
   - Health: `/health`

> Del med andre? I **PORTS**‑panelet, sett port 5000 til **Public** og kopier URL.

---

## Hva gjør filene?

| Fil / mappe                                 | Hva er det                                            | Hvorfor er den her                                |
| ------------------------------------------- | ----------------------------------------------------- | ------------------------------------------------- |
| `Movies.Api/Program.cs`                     | **Minimal API** + Swagger + Health + CORS + seed‑data | Selve API‑logikken og endepunktene                |
| `Movies.Api/Movies.Api.csproj`              | Prosjektfil (.NET 8 + Swagger‑pakke)                  | Forteller verktøyene hva som trengs for å bygge   |
| `Movies.Api/appsettings.json`               | Konfig (tom)                                          | Plass til nøkler/tilkoblingsstrenger senere       |
| `Movies.Api/Properties/launchSettings.json` | Dev‑kjøring                                           | Binder til `http://0.0.0.0:5000` i Codespaces     |
| `Movies.Api/Dockerfile`                     | Container‑oppskrift                                   | Bygger slankt runtime‑image av API‑et             |
| `.devcontainer/devcontainer.json`           | Codespaces‑miljø                                      | Starter .NET 8‑container og forwarder port 5000   |
| `.github/workflows/ci.yml`                  | GitHub Actions (CI)                                   | Bygger .NET + Docker og pusher image til **GHCR** |
| `README.md`                                 | Hurtigstart                                           | Kortversjon av kjøre‑instruksjoner                |

---

## Endepunkter

- `GET /api/movies` – liste (støtter `?q=` søk)
- `GET /api/movies/{id}` – hent én
- `POST /api/movies` – opprett
- `PUT /api/movies/{id}` – oppdater
- `DELETE /api/movies/{id}` – slett
- `GET /health` – health check
- `GET /` – velkomst

**Eksempel POST‑body**

```json
{
  "title": "Blade Runner",
  "year": 1982,
  "genres": ["Sci‑Fi", "Noir"],
  "rating": 8.1
}
```

---

## Feilsøking

- **Ingen port‑popup?**  
  Kjør: `dotnet run --urls http://0.0.0.0:5050` → åpne port 5050 i **PORTS**.
- **Restore feilet?**  
  `dotnet restore && dotnet run`
- **Ser 3 filmer uten å poste?**  
  Det er **seed‑data** lagt inn ved oppstart for demo.
- **Data blir borte når du restarter?**  
  POC‑en bruker **in‑memory** lagring (bevisst).

---

## Neste steg som kan legges til

- Bytt til **EF Core + SQLite/PostgreSQL** for ekte lagring
- Legg til **xUnit‑tester** (validering/endepunkter)
- Deploy container til **Azure Web App for Containers**, **Render** eller **Railway**
- Lag en liten **React‑klient** som kaller API‑et
