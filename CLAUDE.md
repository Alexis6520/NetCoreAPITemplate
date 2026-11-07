# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

Reference template for .NET 10 + PostgreSQL Web APIs. The `Donuts` feature is only an example of the pattern to follow; `README.md` covers the stack, configuration and the steps for adding a feature in more detail. The repo content (README, XML doc comments, commit messages) is written in Spanish: keep using Spanish there.

## Commands

```bash
docker compose up -d                                                         # local Postgres (krispy/kreme, port 5432)
dotnet ef database update --project src/Persistence --startup-project src/Host  # the app does NOT migrate on startup
dotnet run --project src/Host                                                # http://localhost:5010, Scalar at /scalar (Development only)
dotnet build NetCoreAPITemplate.slnx
dotnet test UnitTests                                                        # MSTest, no external dependencies
dotnet test IntegrationTests                                                 # xUnit v3, needs the Postgres from compose.yml
dotnet test UnitTests --filter "FullyQualifiedName~CreateDonutHandlerTests"  # single test/class (same --filter works for IntegrationTests)
dotnet ef migrations add <Name> --project src/Persistence --startup-project src/Host
```

There is no separate linter: every project has `TreatWarningsAsErrors=true`, so any warning breaks the build. `Host` generates XML documentation (`CS1591` is suppressed, but endpoints are documented with `/// <summary>` and `[ProducesResponseType]`).

## Architecture

- **Projects**: `src/Host` (API, features, mediator, middleware) → references `src/Persistence` (`ApplicationDbContext`, entities, migrations). There are no separate domain/application layers: handlers use `ApplicationDbContext` directly.
- **Vertical slices + light CQRS**: `src/Host/Features/<Feature>/Commands|Queries/*.cs`. Each file holds the request `record` (`IRequest<TResponse>`) and its `class ...Handler` (`IRequestHandler<TRequest, TResponse>`, `Execute` method).
- **Custom mediator, no MediatR** (`src/Host/Mediation`): `AddMediation()` uses reflection to scan **only the `Host` assembly** and registers as `Transient` every concrete class implementing `IBaseRequestHandler` or `IUseCaseCollection`. They are registered by **concrete type**, not by interface: inject `CreateDonutHandler`, not `IRequestHandler<...>`. A handler outside `Host` is not discovered.
- **Controllers** receive a `record <Feature>UseCases(...) : IUseCaseCollection` that groups the feature's handlers, and call `useCases.X.Execute(...)` directly (no dynamic dispatch). Responses are wrapped in `Response<T>` (`{ "value": ... }`). Parameterless requests use a singleton (`DonutsListRequest.Instance`).
- **Errors**: `ExceptionMiddleware` turns any unhandled exception into a `500` + generic `ProblemDetails`. There is no validation or mapping of business errors to 4xx yet.
- **Configuration**: `ConnectionStrings:Default` is required; if missing, `AddPersistence` throws when the `DbContext` is created. In Docker/other environments pass it via the `ConnectionStrings__Default` environment variable.

## Tests

- **UnitTests (MSTest)**: inherit from `UnitTests/Abstractions/BaseTest`, which provides a fresh InMemory `ApplicationDbContext` per test. Tests run in parallel at method level (`MSTestSettings.cs`), so don't share static state. Use `TestContext.CancellationToken`.
- **IntegrationTests (xUnit v3)**: inherit from `IntegrationTests/Abstractions/BaseTest` with `CustomWebAppFactory` as a class fixture. The factory forces the `Development` environment, uses the `krispy_kreme_test` database on `127.0.0.1` (hardcoded connection string), drops and recreates it with `EnsureCreated` **once per test run** (migrations are not applied), and replaces the app's `DbContext`. The database is **not cleaned between tests**: assertions must tolerate data from other tests (e.g. `>= n`, look up by id), never exact counts.

## CI and dependencies

`.github/workflows/CI.yml` runs `dotnet restore --locked-mode`. Every project uses `RestorePackagesWithLockFile`: when adding or updating a NuGet package, commit the regenerated `packages.lock.json` files or CI fails. Integration tests in CI run against a `postgres:18` service with the same credentials as `compose.yml`.
