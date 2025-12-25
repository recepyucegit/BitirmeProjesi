# Copilot / AI Agent Instructions for TeknoRoma

Be concise: make small, focused commits and prefer changes only in the layer you are addressing.

## Big picture (short)
- Architecture: Onion / Clean Architecture with 4 layers: Domain, Application, Infrastructure, Presentation.
- Key folders: `src/Core/TeknoRoma.Domain`, `src/Core/TeknoRoma.Application`, `src/Infrastructure/TeknoRoma.Infrastructure`, `src/Presentation/TeknoRoma.API`, `teknoroma-frontend`.
- Data flow: Controllers → Application services / DTOs → Repository interfaces → Infrastructure repositories → EF Core DbContext.

## Important patterns & conventions
- DTOs: request/response DTOs live under `TeknoRoma.Application/DTOs`. Names like `CreateCategoryDto`, `UpdateEmployeeDto` are used.
- Validation: FluentValidation validators exist in `TeknoRoma.Application/Validators` (example: `CreateCategoryDtoValidator.cs`). Use validators rather than ad-hoc model checks.
- Repositories & Services: interfaces in `TeknoRoma.Application/Interfaces` and implementations in `TeknoRoma.Infrastructure/Repositories` and `.../Services`.
- Soft-delete: Entities inherit from `BaseEntity` and soft-delete uses a global query filter in the EF DbContext. Avoid deleting rows directly; follow soft-delete semantics.
- Auth: JWT + Refresh tokens; password hashing with BCrypt. See authentication controllers in `TeknoRoma.API/Controllers` and token handling in `Infrastructure/Services`.
- Naming: keep `PascalCase` for C# types and folders matching project namespaces.

## Developer workflows (commands you can run)
- Build & run API (development):
  - `cd src/Presentation/TeknoRoma.API && dotnet restore && dotnet build && dotnet run`
- Run frontend dev server:
  - `cd teknoroma-frontend && npm install && npm run dev`
- Database migrations (EF Core):
  - `cd src/Infrastructure/TeknoRoma.Infrastructure && dotnet ef migrations add <Name> && dotnet ef database update`
- Tests:
  - `cd tests/TeknoRoma.Tests && dotnet test`

## How to make changes safely
- When modifying persistence models, update EntityTypeConfiguration and run/verify migrations in `TeknoRoma.Infrastructure`.
- If adding a new endpoint: add DTO in `TeknoRoma.Application/DTOs`, a validator in `Validators`, service interface in `Interfaces/Services`, implementation in `Infrastructure/Services`, and controller in `Presentation/TeknoRoma.API/Controllers` that injects the service via DI.
- Prefer adding small focused tests in `tests/TeknoRoma.Tests` for new business logic. Look at `Entities` tests for examples.

## Integration points & external deps
- SQL Server (use connection string in `appsettings.json` / `appsettings.Development.json`).
- EF Core is the ORM; migrations live under `src/Infrastructure/TeknoRoma.Infrastructure/Migrations`.
- Frontend calls REST endpoints under `/api/*` (see `teknoroma-frontend/src/services/api.js`).

## Files to inspect for context when changing code
- Startup & DI: `src/Presentation/TeknoRoma.API/Program.cs` (how services and repositories are wired).
- DbContext & global filters: search for `DbContext` under `TeknoRoma.Infrastructure/Data`.
- DTOs & validators: `src/Core/TeknoRoma.Application/DTOs`, `.../Validators`.
- Controllers: `src/Presentation/TeknoRoma.API/Controllers` for request shapes and route patterns.
- Tests: `tests/TeknoRoma.Tests` for existing assertions and examples.

## Quick examples (copyable)
- DTO + validator naming:
  - `TeknoRoma.Application/DTOs/Customer/CreateCustomerDto.cs`
  - `TeknoRoma.Application/Validators/CreateCustomerDtoValidator.cs`
- Sample route: GET stores → `GET /api/stores` implemented in `StoresController` (`TeknoRoma.API/Controllers/StoresController.cs`).

## Constraints & things to avoid
- Don't bypass soft-delete: use provided repository methods.
- Avoid changing multiple layers in one PR; follow the layer-by-layer approach described above.

If anything here is unclear or you want additional examples (DI wiring, DbContext code snippets, or common PR checklist), tell me which section to expand.
