# TMW2Play — Tell Me What to Play

TMW2Play helps players explore their Steam libraries, review playtime, get AI game recommendations, and discover upcoming releases.

## Features

- Steam profile lookup and owned-game library view. Profiles must be public.
- Playtime statistics for the last two weeks and all time.
- AI recommendations based on the player's library and recent playtime.
- AI-generated humorous commentary on the player's library.
- AI recommendations for upcoming games.
- English (US) and Brazilian Portuguese, with the selected language sent to the API.
- Responsive Angular interface and interactive charts.

## Requirements

- Docker and Docker Compose for containerized setup.
- .NET 10 SDK for backend development.
- Node.js 22 and npm for frontend development.
- Steam Web API key: [request a key](https://steamcommunity.com/dev/apikey).
- OpenRouter API key: [create an API key](https://openrouter.ai/keys).

## Configuration

The API reads `SteamAPIKey`, `OpenRouterAPIKey`, and `OpenRouterModel` from .NET configuration. The configured model is `openrouter/free`; OpenRouter selects an available free model for each request ([model details](https://openrouter.ai/collections/free-models)).

For local backend development, set credentials with .NET user secrets from the API project directory:

```bash
cd backend/TMW2Play.Api
dotnet user-secrets set "SteamAPIKey" "YOUR_STEAM_API_KEY"
dotnet user-secrets set "OpenRouterAPIKey" "YOUR_OPENROUTER_API_KEY"
dotnet user-secrets set "OpenRouterModel" "openrouter/free"
```

Do not commit API keys. Production deployments use GitHub Actions secrets for Steam, OpenRouter, and hosting credentials.

## Run with Docker

```bash
docker compose up --build
```

- Frontend: `http://localhost:1010`
- Backend API: `http://localhost:1011`

The API needs valid Steam and OpenRouter keys in its runtime configuration for profile lookup and AI features.

## Run locally

### Backend

```bash
cd backend/TMW2Play.Api
dotnet restore
dotnet run
```

The API runs at `http://localhost:5262`; Swagger opens at `/swagger` in Development.

### Frontend

```bash
cd frontend
npm ci
npm start
```

Angular serves the app at `http://localhost:4200`.

## Recent updates

README last changed on January 4, 2026. Since then, changes merged into `main` added or updated:

- Upcoming game recommendations based on the player's library and recent playtime.
- OpenRouter integration for AI recommendations and library commentary.
- Language selector fixes and API language propagation.
- Frontend deployment to GitHub Pages and backend deployment workflow for Somee.
- `robots.txt`, sitemap, and `llms.txt` for site discovery.
- Angular 22 migration and related chart, cookie, and notification dependencies.

The remote repository currently has no Git tags, so this summary uses changes merged into `main` rather than tagged release notes.

## Architecture

- **Frontend:** Angular 22, Angular Signals, Bootstrap 5, ng-bootstrap, Transloco, Chart.js.
- **Backend:** ASP.NET Core on .NET 10, Steam Web API, OpenRouter Chat Completions API.
- **Deployment:** Docker Compose; GitHub Actions workflows for frontend and backend deployments.

## Repository layout

```text
backend/       ASP.NET Core API and services
frontend/      Angular application
docs/          GitHub Pages site and static assets
.github/       CI, deployment, and dependency update workflows
```

## License

MIT. See [LICENSE](LICENSE).
