# TMW2Play - Tell Me What to Play

A modern web application that helps gamers decide what to play next from their Steam library using AI-powered recommendations.

## 🚀 Quick Start

### Prerequisites
- Docker and Docker Compose
- .NET 9 SDK (for local development)
- Node.js 20+ (for local development)
- Steam Web API key ([Get it here](https://steamcommunity.com/dev/apikey))
- Google Gemini API key ([Get it here](https://makersuite.google.com/app/apikey))

### Environment Setup
1. Update the `appsettings.json` in the backend folder:
```json
{
  "SteamApiKey": "YOUR_STEAM_API_KEY",
  "GeminiApiKey": "YOUR_GEMINI_API_KEY"
}
```

### Running with Docker
```bash
docker compose up --build
```
Access the application at `http://localhost:1010`

### Local Development

#### Frontend (Angular 20)
```bash
cd frontend
npm install
ng serve --o
```
Frontend will be available at `http://localhost:4200`

#### Backend (.NET 9)
```bash
cd backend
dotnet restore
dotnet run
```
API will be available at `http://localhost:5262`

## 🏗️ Architecture

### Frontend (Angular 20)
- **Framework**: Angular 20
- **State Management**: Angular Signals
- **UI Components**: Bootstrap 5 + ng-bootstrap
- **Charts**: Chart.js + ng2-charts
- **Internationalization**: Transloco
- **Environment Configurations**:
  - Development: `environment.ts`
  - Docker: `environment.docker.ts`
  - Production: `environment.prod.ts`

### Key Features
- 🌐 Multi-language support (EN-US, PT-BR)
- 📊 Interactive game statistics
- 🤖 AI-powered game recommendations
- 🎮 Steam profile integration
- 📱 Responsive design

## 🛠️ Development Guide

### Frontend Structure
```
frontend/
├── src/
│   ├── app/
│   │   ├── home/           # Home page
│   │   ├── profile/        # Profile features
│   │   └── shared/         # Shared stuff
│   ├── assets/
│   │   ├── i18n/          # Translation files
│   │   └── img/           # Images
│   └── environments/      # Environment configurations
```

### Available Scripts
- `ng serve`: Development server
- `npm run build:docker`: Build optimized for Docker deployment
- `npm run build:github`: GitHub Pages build

## 🤝 Contributing
We welcome all contributions to improve TMW2Play! Here's how you can help:

### 🐛 Report Issues
- Found a bug? Feel free to open an issue
- Include steps to reproduce the problem
- Add screenshots if applicable

### 💡 Suggest Improvements
- Have an idea? We'd love to hear it! Open a discussion
- Looking for new features? Share your suggestions
- Propose UI/UX improvements

### 🔧 Submit Changes
If you'd like to contribute with code feel free to submit a Pull Request

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.