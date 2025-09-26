# stemprep-backend
<div align="center">

# StemPrep Backend

Robust, modern, and thoughtfully engineered backend powering the StemPrep platform. Built with .NET 8, clean architecture, and production‑grade tooling. Fast to run locally, easy to deploy in containers, and delightful to work with.

</div>

---

## TL;DR (for recruiters and quick readers)

- Production‑ready .NET 8 API with clean architecture and great DX (Swagger, Serilog, global error handling)
- Real features: Auth with roles (Parent/Tutor/Admin/Student), documents with Cloudinary, PostgreSQL + EF Core, Redis
- Run locally in 60 seconds: `dotnet restore && dotnet run --project src/API/API.csproj`
- Dockerized for easy demo: `docker build -t stemprep-backend . && docker run -p 8080:8080 ...`

Badges (replace with your own if desired):

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4) ![EF Core](https://img.shields.io/badge/EF%20Core-9.0-informational) ![Swagger](https://img.shields.io/badge/Docs-Swagger-brightgreen) ![License](https://img.shields.io/badge/License-MIT-blue)

## Real Features (Details)

- __Authentication & Roles__
  - Roles: `Parent`, `Tutor`, `Admin`, `Student`
  - JWT bearer auth with role-based authorization via `[Authorize]` attributes
  - Login endpoints set HttpOnly cookies: `stem-prep-accessToken`, `stem-prep-refreshToken`
    - See: `src/API/Controllers/AuthenticationController.cs` and `StudentController.cs`
  - Config: `Jwt` section in `src/API/appsettings.json`; wired in `src/API/Program.cs`
  - Sample endpoints:
    - `POST /api/auth/tutor/login`
    - `POST /api/auth/parent/login`
    - `POST /api/auth/admin/login`
    - `POST /login` (student)

- __Documents & Media__
  - Public list: `GET /openall`
  - Tutor's documents: `GET /all` (role `Tutor`)
  - Upload documents: `POST /upload` (multipart/form-data, role `Tutor`)
  - Manage images: `POST /image`, `POST /editimage` (multipart/form-data, auth required)
  - Uses Cloudinary for storage (see `Cloudinary` keys in `appsettings.json`)
  - See: `src/API/Controllers/DocumentController.cs`

- __Users & Profiles__
  - Get current user profile: `GET /profile` (auth required)
  - See: `src/API/Controllers/UserController.cs`

- __Tutor Dashboard__
  - Overview: `GET /dashboardinfo` (auth required)
  - Update profile: `POST /update` (role `Tutor`)
  - See: `src/API/Controllers/TutorController.cs`

- __Students__
  - Login/Password flows: `POST /login`, `POST /forgotpassword`, `POST /resetpassword`
  - Parent adds student: `POST /register` (role `Parent`)
  - See: `src/API/Controllers/StudentController.cs`

- __Data & Persistence__
  - PostgreSQL with EF Core; migrations applied on startup (`context.Database.Migrate()`) in `Program.cs`
  - Providers: `Npgsql`, `Npgsql.EntityFrameworkCore.PostgreSQL`
  - Connection strings in `ConnectionStrings` of `appsettings.json`

- __Caching & Flows (Redis)__
  - Redis used for flows like student password reset
  - Keys/sections: `Redis:StudentResetPassword` (see `src/API/appsettings.json`)
  - Client: `StackExchange.Redis`

- __Error Handling__
  - Global exception middleware returns consistent JSON envelope (`Application.Common.Models.Result`)
  - Maps common cases (validation, not found, unauthorized, DB update) to proper HTTP codes
  - See: `src/API/Filters/ExceptionHandlerMiddleware.cs`

- __API Docs__
  - Swagger enabled in all environments
  - Security scheme for Bearer tokens is preconfigured; use the Authorize button to test secured endpoints
  - Endpoints overview: `docs/ENDPOINTS.md`

## Live Demo & Screenshots

- Live API (if available): https://stemprep-backend.onrender.com/swagger
- Frontend (if available): https://stemprep.netlify.app/
- Screenshots: add images to `docs/images/` and showcase key flows (login, uploading docs, Swagger UI)

## About the Project (1–2 sentences)

StemPrep helps parents and students connect with vetted tutors and manage study documents securely. This backend powers authentication, profiles, document uploads, and secure access to content.

## What I built (highlights recruiters care about)

- Secure auth with JWT and role-based access (Parent, Tutor, Admin, Student)
- Document management with Cloudinary storage and image processing endpoints
- PostgreSQL + EF Core with automatic migrations on startup
- Production-ready setup: Serilog logs, global exception middleware, and Swagger in all environments

## Contact

- Portfolio: add-your-portfolio-url
- LinkedIn: add-your-linkedin-url
- Email: add-your-email

## Why This Stands Out

- Clean layering: thin controllers, business logic in `Application/`, persistence and integrations in `Infrastructure/`
- Security basics done right: JWT bearer + roles, centralized exception handling, and DI throughout
- Dev‑friendly: automatic migrations, Swagger everywhere, and easy environment‑based configuration

## See It In 60 Seconds

1. Run the API locally

   ```bash
   dotnet restore && dotnet run --project src/API/API.csproj
   ```

2. Open Swagger UI and explore endpoints

   - https://localhost:7218/swagger

3. Try a couple of endpoints (shape may vary based on validators)

   ```bash
   # Public endpoint example
   curl -k "https://localhost:7218/openall"

   # Authentication example (Tutor login)
   curl -k -X POST "https://localhost:7218/api/auth/tutor/login" \
     -H "Content-Type: application/json" \
     -d '{
       "email": "tutor@example.com",
       "password": "YourPassword123!"
     }'
   ```

4. Paste your JWT into Swagger's Authorize dialog to try secured routes (`Authorize` button in the top-right).

Tip: A full, human-friendly route list lives in `docs/ENDPOINTS.md`.

## Code Tour: Start Here

- `src/API/Program.cs` — middleware, DI wiring, Swagger, and automatic EF migrations
- `src/API/Controllers/` — thin controllers mapping HTTP to commands/queries
- `src/Application/` — business logic (MediatR commands/queries, validation, interfaces)
- `src/Infrastructure/` — EF Core DbContext, PostgreSQL provider, external services (Cloudinary, Redis)
- `src/Domain/` — core entities and domain types

## Highlights

- Bold, clean architecture: `API` (presentation), `Application` (business logic), `Domain` (core models), `Infrastructure` (EF Core, persistence, integrations)
- First‑class developer experience: Swagger/OpenAPI, Serilog logging, global exception handling, and automatic database migrations
- Secure and scalable: JWT auth with role-based authorization, PostgreSQL, and Redis
- Media & messaging ready: Cloudinary for assets, Twilio package available, and extensible service boundaries

## Tech Stack

- .NET 8 / ASP.NET Core Web API
- EF Core 9 (PostgreSQL provider)
- MediatR, FluentValidation, AutoMapper
- Serilog (console sink)
- Swagger (Swashbuckle)
- Redis (StackExchange.Redis)
- Cloudinary SDK

Project files of note:
- `src/API/Program.cs` – composition root and middleware pipeline (JWT, CORS, Swagger, migrations)
- `src/API/Filters/ExceptionHandlerMiddleware.cs` – consistent error envelope across the API
- `src/Infrastructure` – EF Core and external services
- `Dockerfile` – multi-stage build for small, production‑ready images

## Getting Started

### Prerequisites

- .NET SDK 8.0+
- PostgreSQL database (local or hosted)
- Optional: Redis (for caching/flows) and Cloudinary account for media

### Clone and restore

```bash
# From the repository root
 dotnet restore
```

### Configuration (Environment & Secrets)

Never commit real secrets in `appsettings.json`. Prefer environment variables or .NET User Secrets in development.

Common keys (do NOT paste real secrets in code):

- ConnectionStrings
  - `postgresConnection`
  - `redisConnection`
- `Jwt`
  - `AccessKey`
  - `RefreshKey`
  - `Issuer`
  - `Audience`
- `Cloudinary`
  - `CloudName`
  - `ApiKey`
  - `ApiSecret`
- `Url`
  - `BaseUrl`
  - `StudentForgotPasswordUrl`

Recommended for local dev (User Secrets):

```bash
# from src/API directory
 dotnet user-secrets init
 dotnet user-secrets set "ConnectionStrings:postgresConnection" "Host=localhost;Port=5432;Database=stemprep;Username=postgres;Password=..."
 dotnet user-secrets set "Jwt:AccessKey" "..."
 dotnet user-secrets set "Jwt:RefreshKey" "..."
 dotnet user-secrets set "Jwt:Issuer" "local-issuer"
 dotnet user-secrets set "Jwt:Audience" "local-audience"
 dotnet user-secrets set "Cloudinary:CloudName" "..."
 dotnet user-secrets set "Cloudinary:ApiKey" "..."
 dotnet user-secrets set "Cloudinary:ApiSecret" "..."
```

### Run the API

```bash
# From repository root
 dotnet run --project src/API/API.csproj
```

By default, Swagger UI is available at:

- Swagger UI: `https://localhost:7218/swagger`
- OpenAPI JSON: `https://localhost:7218/swagger/v1/swagger.json`

The app applies EF Core migrations automatically on startup (`Program.cs`: `context.Database.Migrate();`). Ensure your database is reachable.

## Docker

The included `Dockerfile` uses a multi-stage build.

Build and run:

```bash
 docker build -t stemprep-backend .
 docker run -p 8080:8080 -e ASPNETCORE_URLS=http://+:8080 \
   -e ConnectionStrings__postgresConnection="..." \
   -e Jwt__AccessKey="..." -e Jwt__RefreshKey="..." -e Jwt__Issuer="..." -e Jwt__Audience="..." \
   -e Cloudinary__CloudName="..." -e Cloudinary__ApiKey="..." -e Cloudinary__ApiSecret="..." \
   stemprep-backend
```

Notes:
- Provide connection strings and secrets as environment variables when running containers.
- The image entrypoint is `dotnet API.dll` (see `Dockerfile`).

## API Documentation

- Interactive docs: Swagger UI at runtime
- Human-friendly reference: `docs/ENDPOINTS.md` (routes, auth, and request shapes)

## Project Structure

```
src/
  API/
    Controllers/
    Filters/
    Program.cs
    appsettings.json
  Application/
    ... application services, commands, queries ...
  Domain/
    ... entities and core types ...
  Infrastructure/
    ... EF Core, db context, external services ...
Dockerfile
```

## Development Notes

- CORS is open during development (`Program.cs`): allows any origin/method/header
- Auth: JWT bearer with role-based policies (`Authorize` attributes on controllers)
- Errors: unified response via `ExceptionHandlerMiddleware`

## Contributing

Contributions are welcome! A few suggestions:

- Use feature branches and concise pull requests
- Add or update Swagger annotations where relevant
- Keep domain logic in `Application`/`Domain`, ensure controllers stay thin
- Prefer configuration via environment variables in all environments

## License

This project is licensed under the terms of the repository’s `LICENSE` file.

---

If you’re reading this, you’re already part of something great. Thank you for helping make StemPrep’s backend a joy to build on and a pleasure to deploy. 💙