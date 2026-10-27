# NetCoreAPITemplate

Plantilla de referencia para construir Web APIs en **.NET 10** con PostgreSQL. Usa un dominio de ejemplo (donas de Krispy Kreme) para mostrar la estructura, los patrones y el tooling que debería tener un servicio nuevo: organización por features, casos de uso desacoplados del controlador, logging estructurado, manejo global de errores, tests unitarios y de integración, Docker y CI.

> La idea es copiar la plantilla, borrar la feature `Donuts` y empezar a agregar las propias siguiendo el mismo patrón.

## Stack

| Área | Tecnología |
| --- | --- |
| Runtime | .NET 10 / ASP.NET Core (controllers) |
| Persistencia | Entity Framework Core 10 + Npgsql (PostgreSQL 18) |
| Logging | Serilog (`Serilog.AspNetCore`, `Serilog.Expressions`) |
| Documentación de API | OpenAPI (`Microsoft.AspNetCore.OpenApi`) + Scalar UI |
| Tests unitarios | MSTest + EF Core InMemory |
| Tests de integración | xUnit v3 + `WebApplicationFactory` + PostgreSQL real |
| Contenedores | Docker (multi-stage) + Docker Compose |
| CI | GitHub Actions |

## Estructura

```
NetCoreAPITemplate.slnx
├── src/
│   ├── Host/                       # API (punto de entrada)
│   │   ├── Features/
│   │   │   └── Donuts/
│   │   │       ├── Commands/       # Escrituras (CreateDonut.cs: request + handler)
│   │   │       ├── Queries/        # Lecturas  (DonutsList.cs: request + handler)
│   │   │       ├── DonutUseCases.cs    # Agrupa los handlers de la feature
│   │   │       └── DonutsController.cs
│   │   ├── Mediation/              # Mediador propio (IRequest, IRequestHandler, registro en DI)
│   │   ├── Middlewares/            # ExceptionMiddleware (errores → ProblemDetails)
│   │   ├── Models/                 # Response<T>, envoltorio estándar de respuestas
│   │   ├── ServiceExtension.cs     # Configuración de Serilog
│   │   └── Program.cs
│   └── Persistence/                # Acceso a datos
│       ├── Entities/               # Entidades de EF Core
│       ├── Migrations/             # Migraciones de EF Core
│       ├── ApplicationDbContext.cs
│       └── PersistenceServiceExtension.cs
├── UnitTests/                      # MSTest, BD en memoria
├── IntegrationTests/               # xUnit, API levantada en memoria contra Postgres real
├── compose.yml                     # Postgres local para desarrollo
├── dockerfile                      # Imagen de la API
└── .github/workflows/CI.yml
```

## Arquitectura

### Vertical slices + CQRS ligero

Cada funcionalidad vive en su propia carpeta dentro de `src/Host/Features/<Feature>/`. Las escrituras van en `Commands/` y las lecturas en `Queries/`. Cada archivo contiene el **request** (un `record`) y su **handler**, así que todo lo necesario para entender un caso de uso está en un solo lugar.

### Mediador propio (sin MediatR)

En [`src/Host/Mediation`](src/Host/Mediation) hay una implementación mínima:

- `IRequest<TResponse>` / `IRequest`: marca un objeto como petición de un caso de uso.
- `IRequestHandler<TRequest, TResponse>` / `IRequestHandler<TRequest>`: expone `Execute(request, cancellationToken)`.
- `IUseCaseCollection`: marca un `record` que agrupa los handlers de una feature (por ejemplo, `DonutUseCases`).
- `AddMediation()`: recorre el ensamblado y registra como `Transient` todas las clases que implementan `IBaseRequestHandler` o `IUseCaseCollection`.

El controlador recibe la colección de casos de uso y llama directamente al handler. No hay despacho dinámico ni reflection en tiempo de ejecución:

```csharp
public class DonutsController(DonutUseCases donutUseCases) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateDonutRequest request, CancellationToken ct)
    {
        int id = await donutUseCases.Create.Execute(request, ct);
        return Created((string?)null, new Response<int>(id));
    }
}
```

### Aspectos transversales

- **Errores**: `ExceptionMiddleware` captura cualquier excepción no controlada, la registra y responde `500` con un `ProblemDetails` genérico, sin exponer detalles internos.
- **Respuestas**: los endpoints envuelven el resultado en `Response<T>` (`{ "value": ... }`).
- **Logging**: Serilog con un logger de arranque (para capturar fallos al iniciar), logging de cada request y un formato de consola que incluye trace/span id cuando existen. Los niveles se configuran en la sección `Serilog` de `appsettings.json`.
- **OpenAPI**: solo en `Development` se exponen `/openapi/v1.json` y la UI de Scalar en `/scalar`.

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Docker (para levantar PostgreSQL)
- Herramienta de EF Core para las migraciones:

```bash
dotnet tool install --global dotnet-ef
```

## Cómo correrlo en local

1. Levantar PostgreSQL:

```bash
docker compose up -d
```

   Crea el contenedor `krispy_db` en el puerto `5432` (usuario `krispy`, base `krispy_kreme`). Coincide con la cadena de conexión de `src/Host/appsettings.Development.json`.

2. Aplicar las migraciones. La app **no** las aplica al arrancar:

```bash
dotnet ef database update --project src/Persistence --startup-project src/Host
```

3. Ejecutar la API:

```bash
dotnet run --project src/Host
```

   Queda escuchando en `http://localhost:5010` (perfil `http`) o en `https://localhost:7074` (perfil `https`). La documentación interactiva está en `http://localhost:5010/scalar`.

### Configuración

| Clave | Dónde | Descripción |
| --- | --- | --- |
| `ConnectionStrings:Default` | `appsettings.Development.json` / variable `ConnectionStrings__Default` | Cadena de conexión a PostgreSQL. **Obligatoria**: si falta, la app falla al crear el `DbContext`. |
| `Serilog:*` | `appsettings.json` | Niveles mínimos de log y overrides por namespace. |

> Las credenciales de `appsettings.Development.json` y `compose.yml` son solo para desarrollo local. En otros entornos, inyecta la cadena de conexión por variable de entorno o con un gestor de secretos.

## Endpoints de ejemplo

| Método | Ruta | Body | Respuesta |
| --- | --- | --- | --- |
| `POST` | `/api/donuts` | `{ "name": "Frambuesa", "description": "...", "price": 19.99 }` | `201` con `{ "value": <id> }` |
| `GET` | `/api/donuts` | – | `200` con `{ "value": [ { "id", "name", "description", "price" } ] }` |

Restricciones de la entidad `Donut`: `name` es obligatorio (máx. 32 caracteres), `description` admite hasta 128 caracteres y `price` es `decimal(5,2)`.

## Agregar una nueva feature

1. **Entidad**: crea la clase en `src/Persistence/Entities/` y agrega su `DbSet<>` en `ApplicationDbContext`.
2. **Migración**:

```bash
dotnet ef migrations add <NombreMigracion> --project src/Persistence --startup-project src/Host
```

3. **Casos de uso**: en `src/Host/Features/<Feature>/` crea `Commands/` y/o `Queries/`. En cada archivo define un `record ... : IRequest<TResponse>` y su `class ...Handler : IRequestHandler<TRequest, TResponse>`.
4. **Colección**: crea `<Feature>UseCases` como `record` que implemente `IUseCaseCollection` y reciba los handlers. No hay que registrar nada a mano: `AddMediation()` lo descubre solo.
5. **Controlador**: inyecta `<Feature>UseCases`, devuelve `Response<T>` y documenta con comentarios XML y `[ProducesResponseType]`.
6. **Tests**: un test unitario por handler en `UnitTests/Features/<Feature>/` y tests de endpoint en `IntegrationTests/Features/<Feature>/`.

## Tests

### Unitarios (`UnitTests`, MSTest)

Prueban los handlers de forma aislada. `BaseTest` entrega un `ApplicationDbContext` sobre una base **InMemory** nueva por test.

```bash
dotnet test UnitTests
```

### Integración (`IntegrationTests`, xUnit v3)

Levantan la API completa en memoria con `CustomWebAppFactory` y la prueban por HTTP contra un **PostgreSQL real**. La fábrica:

- Usa la base `krispy_kreme_test` en `127.0.0.1:5432` (usuario `krispy` / `kreme`).
- La borra y la recrea con `EnsureCreated` una sola vez por ejecución. **No** aplica migraciones: crea el esquema directamente desde el modelo.
- Reemplaza el `DbContext` de la app para que use esa conexión.

Para correrlos en local necesitas el Postgres de `compose.yml` levantado. El usuario `krispy` tiene permisos para crear la base de test.

```bash
dotnet test IntegrationTests
```

## Docker

Construir y ejecutar la imagen de la API:

```bash
docker build -t netcoreapitemplate .
```

```bash
docker run -p 8080:8080 -e ConnectionStrings__Default="Server=host.docker.internal;Database=krispy_kreme;User Id=krispy;Password=kreme" netcoreapitemplate
```

Notas:

- La imagen corre en `Production`, así que OpenAPI y Scalar **no** están disponibles y no se carga `appsettings.Development.json`. La cadena de conexión hay que pasarla por variable de entorno.
- Dentro del contenedor, `127.0.0.1` es el propio contenedor. Para llegar al Postgres del host usa `host.docker.internal`, o agrega la API como servicio en `compose.yml`.
- La imagen base `aspnet:10.0` escucha en el puerto `8080`.

## CI

[`.github/workflows/CI.yml`](.github/workflows/CI.yml) se ejecuta en cada push y PR a `master`:

1. **build**: `dotnet restore --locked-mode` (falla si los `packages.lock.json` no están al día), compila los dos proyectos de test en `Release` y los publica como artefacto.
2. **unit-test** e **integration-test** corren en paralelo sobre ese artefacto. El job de integración levanta un servicio `postgres:18` con las mismas credenciales que usan los tests.

Si agregas o actualizas paquetes NuGet, haz commit de los `packages.lock.json` regenerados o el CI fallará.