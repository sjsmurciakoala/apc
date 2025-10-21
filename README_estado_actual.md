# Estado actual del módulo Clientes (Blazor + DevExpress)

## Resumen funcional
- Cliente demo `CLI-DEMO-001` sembrado con tarifas, movimientos y consumos:
  - Script: `Database/2025-10-18_seed_cliente_demo.sql`
  - Ejecutar: `psql -h 3.208.232.209 -U postgres -d bdnes -f Database/2025-10-18_seed_cliente_demo.sql`
  - Crea catálogo `Tarifa Demo Residencial`, configuración de tasas y movimientos en `transaccion_abonado`.
- UI (`ClienteDetail.razor`) con pestañas: Datos generales, Tarifas, Estado de cuenta, **Solicitudes** y **Medidores**.
- Servicios disponibles:
  - `ClientesService` (listado, detalle, filtros, tarifas, estado de cuenta, movimientos).
  - `SolicitudesService` (listar/filtrar, detalle, alta y catálogos).
  - `MedidoresService` (buscar medidores, detalle + historial, asignación a cliente y registro de lecturas sin medidor).
- API:
  - `ClientesController`: `GET /api/clientes*`, `GET /api/clientes/{id}/tarifas`, `.../estado-cuenta`, `.../movimientos`.
  - `SolicitudesController`: `GET /api/solicitudes`, `GET /api/solicitudes/{id}`, `POST /api/solicitudes`.
  - `MedidoresController`: `GET /api/medidores`, `GET /api/medidores/{id}`, `GET /api/medidores/{id}/historial`, `POST /api/medidores/asignar`, `POST /api/medidores/lecturas-sin-medidor`.
- Documentación viva:
  - `modulo_clientes.md`
  - `modulo_solicitudes.md`
  - `modulo_medidores.md` (incluye checklist, endpoints y tareas pendientes de seed).

## Cómo validar la semilla
1. Ejecutar el script indicado.
2. Iniciar la app: `dotnet run --project apc` → navegar a `https://localhost:5001/clientes` → abrir “Cliente Demo Blazor”.
3. Pestaña **Tarifas** → botón *Cargar tarifas* → ver grilla con datos demo.
4. Pestaña **Estado de cuenta** → *Cargar estado de cuenta* y *Cargar movimientos* → revisar resumen y grilla.

## Próximo bloque sugerido
- Completar seed de medidores (`Database/2025-10-XX_seed_medidores.sql`) y validar asignación end-to-end.
- Definir siguiente módulo del backlog legado (ej. AuxiliarLectura u Ordenes) replicando la dinámica de documentación → implementación.

## Notas
- Las advertencias CS8981 y WASM0001 provienen del scaffold EF/DevExpress y se mantienen bajo control.
- Actualiza este documento cuando cierres el pendiente del seed de medidores o arranques el siguiente módulo.
