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

Do not commit API keys. `appsettings.json` keeps credential fields empty; `appsettings.example.json` contains reference placeholders. Configure production API keys through server configuration or environment variables (`SteamAPIKey`, `OpenRouterAPIKey`, and optional `DiscordWebHookURL`).

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

## Notificações de erros no Discord

O [PRD de notificações de erros](docs/prd/discord-error-notifications.md) descreve a integração opcional para alertas de erros da API e integrações Steam/OpenRouter. A exception é enviada como `exception.ToString()`, sem sanitização, reformatação ou truncamento. Quando não couber na mensagem, o texto original completo segue em anexo. Eventuais credenciais ou dados pessoais presentes na exception também serão enviados; use um canal restrito.

Cada alerta deverá conter uma tag visível `[Debug]` ou `[Release]` (produção), conforme o build do componente que originou o erro, além do ambiente de execução.

A configuração usa um webhook de entrada do Discord. Crie um webhook para o canal desejado nas integrações do Discord e copie a URL. Ela contém a credencial necessária, sem bot ou API key separada, e deve ficar exclusivamente no backend na chave simples `DiscordWebHookURL`, na raiz do appsettings, ou no secret/variável de ambiente de mesmo nome. Nunca inclua essa URL no frontend ou no repositório.

```json
{
  "DiscordWebHookURL": ""
}
```

Para desenvolvimento local, configure pelo diretório `backend/TMW2Play.Api`:

```powershell
dotnet user-secrets set "DiscordWebHookURL" "https://discord.com/api/webhooks/WEBHOOK_ID/WEBHOOK_TOKEN"
```

O arquivo [appsettings.example.json](backend/TMW2Play.Api/appsettings.example.json) contém os valores de referência. Preencha `DiscordWebHookURL` apenas na configuração local/do servidor ou em uma variável de ambiente. Para Docker Compose, defina `DiscordWebHookURL` no ambiente antes de executar `docker compose up --build`; o Compose encaminha essa variável somente ao backend. O workflow não configura o Discord. Reinicie/republique o backend após alterar a configuração.

Sem URL configurada, nenhuma tentativa de notificação é feita. Com URL configurada, o serviço faz uma única tentativa de envio assíncrono, com timeout de 5 segundos. Falhas de envio são ignoradas, sem logger e sem afetar o usuário.

A captura fica no `ApiExceptionHandler`, usando o `IExceptionHandler` nativo do ASP.NET Core. Ele envia a exception original ao Discord e devolve HTTP 500 com uma mensagem genérica ao usuário, inclusive em Development. Os serviços deixam falhas técnicas propagarem até esse handler e não dependem do notificador. Notifications continuam sendo usadas para validações de negócio, sem exception, stack trace ou detalhes de parsing. Validações 4xx sem exception não acionam o webhook.

Não existe controller ou endpoint de notificação exposto aos usuários. O alerta inclui `[Debug]` ou `[Release]`, ambiente, horário UTC, método/caminho da requisição e identificador de rastreamento. Erros exclusivos do navegador e tarefas fora das requisições não são capturados automaticamente.

A implementação usa apenas o exception handler e um serviço de envio. Não há fila, worker ou retries. A requisição não aguarda o Discord; se a aplicação encerrar antes do envio terminar, o alerta pode ser perdido. Use um canal restrito para os diagnósticos.
