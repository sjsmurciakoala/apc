# Plan de Migración – Módulo Solicitudes de Servicio

## Tablas mapeadas
- solicitud_servicio

## Comando scaffold ejecutado
`dotnet tool run dotnet-ef dbcontext scaffold "...bdnes..." Npgsql.EntityFrameworkCore.PostgreSQL -p SIAD.Data/SIAD.Data.csproj -s apc/apc.csproj -c SiadDbContext --no-onconfiguring --context-dir TempScaffold --context-namespace TempScaffold --namespace TempScaffold.Entities --use-database-names --output-dir TempScaffold/Entities --force --no-build -t solicitud_servicio -t categoria_servicio`
- Copiar `TempScaffold/Entities/solicitud_servicio.cs` a `SIAD.Core/Entities/` y eliminar la carpeta temporal.
- Ajustar namespace a `SIAD.Core.Entities`.

## Próximos pasos
- DTOs: SolicitudListDto, SolicitudDetailDto
- Servicio: ISolicitudesService / SolicitudesService
- Endpoints: GET /api/solicitudes, GET /api/solicitudes/{id}, (POST opcional)
- UI: pestaña "Solicitudes" en ClienteDetail o páginas específicas

## Implementado
- DTOs `SolicitudListDto` y `SolicitudDetailDto`.
- Servicio `ISolicitudesService` / `SolicitudesService` con filtro por identidad (`GetSolicitudesAsync(string? clienteIdentidad, ...)`, `GetSolicitudAsync`, `CreateSolicitudAsync`).
- Controlador `SolicitudesController` (`GET /api/solicitudes?clienteIdentidad=...`, `GET /api/solicitudes/{id}`, `POST /api/solicitudes`).
- UI Blazor (ClienteDetail.razor): pestaña "Solicitudes" con `LoadSolicitudesAsync` que llama al endpoint filtrando por `cliente.Identidad`.
- Script de seed: `Database/2025-10-20_seed_solicitud_servicio.sql` (dos solicitudes para `CLI-DEMO-001`).
