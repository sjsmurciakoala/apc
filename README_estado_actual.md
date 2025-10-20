# Estado actual del módulo Clientes (Blazor + DevExpress)

## Resumen funcional
- Cliente demo `CLI-DEMO-001` sembrado con tarifas, movimientos y consumos:
  - Script: `Database/2025-10-18_seed_cliente_demo.sql`
  - Ejecutar: `psql -h 3.208.232.209 -U postgres -d bdnes -f Database/2025-10-18_seed_cliente_demo.sql`
  - Crea catálogo `Tarifa Demo Residencial`, configuración de tasas y movimientos en `transaccion_abonado`.
- UI (`ClienteDetail.razor`) con pestañas: Datos generales, Tarifas y Estado de cuenta (resumen + movimientos).
- Servicios (`ClientesService`) implementan listado, detalle, filtros, tarifas, estado de cuenta y movimientos.
- API (`ClientesController`) expone:
  - `GET /api/clientes`, `/api/clientes/{id}`, `/api/clientes/search`
  - `GET /api/clientes/{id}/tarifas`
  - `GET /api/clientes/{id}/estado-cuenta`
  - `GET /api/clientes/{id}/movimientos`
- Documentación del módulo: `modulo_clientes.md` (resumen, métodos y endpoints).

## Cómo validar la semilla
1. Ejecutar el script indicado.
2. Iniciar la app: `dotnet run --project apc` → navegar a `https://localhost:5001/clientes` → abrir “Cliente Demo Blazor”.
3. Pestaña **Tarifas** → botón *Cargar tarifas* → ver grilla con datos demo.
4. Pestaña **Estado de cuenta** → *Cargar estado de cuenta* y *Cargar movimientos* → revisar resumen y grilla.

## Próximo bloque sugerido
- Migrar “Solicitudes de servicio”:
  1. Scaffold tablas `solicitudes_*`.
  2. Crear DTOs/servicios (`ISolicitudesService`, etc.).
  3. Endpoints `GET/POST`.
  4. Agregar UI (pestaña/página) en Blazor.
  5. Actualizar `modulo_clientes.md`.

## Notas
- Cambios listos para commit, se sugiere:
  ```bash
  git add Database/2025-10-18_seed_cliente_demo.sql \
          SIAD.Core/DTOs/Clientes/*.cs \
          SIAD.Core/Entities/configuracion_tasa.cs \
          SIAD.Core/Entities/tarifas_catalogo.cs \
          SIAD.Data/SiadDbContext.cs \
          SIAD.Services/Clientes/IClientesService.cs \
          SIAD.Services/Clientes/ClientesServices.cs \
          apc/Controllers/ClientesController.cs \
          apc.Client/Pages/Clientes/ClienteDetail.razor \
          modulo_clientes.md \
          README_estado_actual.md
  git commit -m "feat: add client demo seed and account summary"
  ```
