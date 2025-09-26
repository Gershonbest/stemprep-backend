# StemPrep Backend API Endpoints

This document describes the public HTTP API exposed by the backend. Authentication uses JWT bearer tokens. Some endpoints set `stem-prep-accessToken` and `stem-prep-refreshToken` cookies upon successful login.

Base URL examples:
- Local: `https://localhost:7218/` (see `src/API/appsettings.Development.json`)
- Production: `https://stemprep-backend.onrender.com/` (see `src/API/appsettings.json:Url:BaseUrl`)

Notes:
- Controllers without a `[Route]` attribute expose endpoints at the root based on their verb attributes, e.g., `[HttpPost("login")]` maps to `POST /login`.
- `AuthenticationController` is explicitly routed at `api/auth/*`.
- Authorization requirements are noted under each endpoint.
- Errors are returned in a consistent envelope by `ExceptionHandlerMiddleware` (`src/API/Filters/ExceptionHandlerMiddleware.cs`).

---

## Authentication
Controller: `src/API/Controllers/AuthenticationController.cs`
Base route: `/api/auth`

- POST `/api/auth/parent/register`
  - Body: `RegisterParentCommand`
  - Auth: None
  - Response: `Result` wrapping created parent/user details

- POST `/api/auth/tutor/register`
  - Body: `RegisterTutorCommand`
  - Auth: None

- POST `/api/auth/tutor/additionaldetails`
  - Body: multipart/form-data `AddAdditionalTutorDetailsCommand`
  - Auth: None

- GET `/api/auth/parent/all`
  - Query: `GetParentQuery`
  - Auth: None

- POST `/api/auth/confirmregistration`
  - Body: `VerifyEmailCommand`
  - Auth: None

- POST `/api/auth/resendverificationcode`
  - Body: `ResendVerificationCodeCommand`
  - Auth: None

- POST `/api/auth/tutor/login`
  - Body: `LoginUserCommand<Tutor>`
  - Auth: None
  - Side effects: Sets `stem-prep-accessToken` and `stem-prep-refreshToken` cookies

- POST `/api/auth/parent/login`
  - Body: `LoginUserCommand<Parent>`
  - Auth: None
  - Side effects: Sets auth cookies

- POST `/api/auth/admin/login`
  - Body: `LoginUserCommand<Admin>`
  - Auth: None
  - Side effects: Sets auth cookies

- POST `/api/auth/forgotpassword`
  - Body: `ForgotPasswordCommand`
  - Auth: None

- POST `/api/auth/verifyresetcode`
  - Body: `VerifyForgotPasswordCodeCommand`
  - Auth: None

- POST `/api/auth/resetpassword`
  - Body: `ResetPasswordCommand`
  - Auth: None

---

## Students
Controller: `src/API/Controllers/StudentController.cs`
Base route: none (verb attributes only)

- POST `/login`
  - Body: `LoginStudentCommand`
  - Auth: None
  - Side effects: Sets auth cookies

- POST `/forgotpassword`
  - Body: `StudentForgotPasswordCommand`
  - Auth: None

- POST `/resetpassword`
  - Body: `StudentResetPasswordCommand`
  - Auth: None

- POST `/register`
  - Body: `RegisterStudentCommand`
  - Auth: Bearer, role `Parent`
  - Notes: Uses parent ID from JWT to set `ParentGuid`

---

## Tutors
Controller: `src/API/Controllers/TutorController.cs`
Base route: none (verb attributes only)

- GET `/dashboardinfo`
  - Auth: Bearer (any authenticated user)
  - Notes: Uses JWT to extract `TutorGuid`

- POST `/update`
  - Body: `UpdateTutorCommand`
  - Auth: Bearer, role `Tutor`
  - Notes: Uses JWT to set `TutorGuid`

---

## Users
Controller: `src/API/Controllers/UserController.cs`
Base route: none (verb attributes only)

- GET `/profile`
  - Auth: Bearer (any authenticated user)
  - Response: `GetUserInfoCommand` result using `UserGuid` from JWT

---

## Documents
Controller: `src/API/Controllers/DocumentController.cs`
Base route: none (verb attributes only)

- GET `/openall`
  - Query: `GetDocumentsRequest`
  - Auth: None

- GET `/all`
  - Auth: Bearer, role `Tutor`
  - Notes: Returns documents for the authenticated tutor

- POST `/image`
  - Body: multipart/form-data `UploadImageCommand`
  - Auth: Bearer

- POST `/editimage`
  - Body: multipart/form-data `EditImageCommand`
  - Auth: Bearer

- POST `/upload`
  - Body: multipart/form-data `UploadDocumentCommand`
  - Auth: Bearer, role `Tutor`
  - Notes: Uses JWT to set `UserGuid`

- POST `/Delete`
  - Body: multipart/form-data `DeleteDocumentCommand`
  - Auth: Bearer
  - Notes: Uses JWT to set `UserGuid`

---

## Authentication and Authorization
- JWT configuration is under `Jwt` in `src/API/appsettings.json` (Issuer, Audience, AccessKey, RefreshKey).
- Middleware is configured in `src/API/Program.cs` with `JwtBearer` authentication and role-based authorization.
- CORS is set to allow any origin/method/header in development (see `Program.cs`).

---

## Error Handling
`ExceptionHandlerMiddleware` returns errors as JSON with camelCase properties and populated message.

- 400 Bad Request: Validation errors or EF update issues
- 401 Unauthorized: Missing/invalid token
- 404 Not Found: NotFoundException
- 500 Internal Server Error: Unhandled exception

---

## Swagger/OpenAPI
Swagger is enabled in all environments.

- JSON: `/swagger/v1/swagger.json`
- UI: `/swagger` (in non-development) or root in development
- Security: Bearer scheme configured in Swagger; supply a JWT via the Authorize button.
