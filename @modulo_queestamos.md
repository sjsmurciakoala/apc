# Plan de Migración – Módulo Clientes y Servicio al Usuario

Este documento desmenuza el alcance del módulo Clientes/Servicio al Usuario del sistema legado y define cómo lo vamos a portar a la solución actual Blazor + DevExpress. Sigue este orden para evitar saltos y mantener sincronizados los artefactos lado servidor, cliente y reporting.

---

## 1. Inventario del Legado

| Área / Controlador | Alcance actual | Dependencias clave |
| --- | --- | --- |
| `Areas/ClientesArea/ClientesController` | ABM completo de clientes: búsqueda, detalle, configuración tarifaria, estado de cuenta, históricos de consumo/pago. Genera RDLC `RptCortesPorCliente` y `rptaccioncobranza`. | Tablas `clientes`, `clientes_direccion`, `clientes_tarifa`, vistas/funciones para estado de cuenta e históricos, repositorio mixto EF+Dapper. |
| `Areas/SolicitudServicioArea/SolicitudServicioController` | Administración de solicitudes (listado, formularios parciales, alta rápida). | Tablas `solicitudes_servicio`, `solicitudes_documentos`, vistas para catálogos. |
| `Areas/MedidorArea/MedidorController` | CRUD de medidores y asociación a clientes. | Tablas `medidores`, `medidores_historial`, `clientes_medidores`. |
| `Areas/AuxiliarLecturaArea/AuxiliarLecturaController` | Soporte a lecturas masivas y valores por defecto para cuadrillas. | Tablas `lecturas`, `auxiliar_lectura`, `periodos`. |
| `Areas/OrdenesArea/OrdenesController` | Órdenes de corte/trabajo, geolocalización, asignación de cuadrillas. | Tablas `ordenes_corte`, `ordenes_detalle`, `ordenes_coordenadas`, funciones para rutas/cuadrillas. |
| `Areas/RutasArea/RutasController` | Mantenimiento de rutas de lectura/distribución. | Tablas `rutas`, `rutas_detalle`, `cuadrillas`, vistas auxiliares. |

La migración respetará este orden: **Clientes → Solicitudes → Medidores → Auxiliar Lectura → Órdenes → Rutas**.

---

## 2. Preparación por Bloques

### 2.1 Scaffold de Entidades (Database-First)
1. Para cada bloque (!no todo de golpe!), ejecutar `dotnet tool run dotnet-ef dbcontext scaffold` contra las tablas correspondientes.  
   - Salida de entidades → `SIAD.Core/Entities`.  
   - `SiadDbContext` se actualiza automáticamente (revisar y limpiar entidades sin clave si no se usarán).  
   - Mantener una lista de tablas/vistas ya escaffoldizadas para evitar duplicados.
2. Asegurarse de conservar el nombre de las columnas originales (`--use-database-names`).  
3. Documentar en este archivo el comando exacto usado por bloque.

### 2.2 Servicios de Dominio (`SIAD.Services`)
1. Crear carpeta por bloque, por ejemplo `SIAD.Services/Clientes`.  
2. Definir interfaces (`IClientesService`, `ISolicitudesService`, ...).  
3. Implementar servicios usando `SiadDbContext`.  
   - Donde el legado usa Dapper/SQL directo, mover la consulta a métodos privados o repositorios internos manteniendo la lógica.  
   - Reutilizar AutoMapper si simplifica DTOs.
4. Registrar el servicio en `ServiceRegistration.cs`.  
5. Documentar dependencias (por ejemplo, si Clientes depende de tarifas o catálogos comunes).

### 2.3 DTOs y Validaciones Compartidas
1. Crear carpeta `SIAD.Core/DTOs/Clientes` (y subcarpetas para cada bloque).  
2. Trasladar modelos view-model del legado (simplificándolos si procede).  
3. Especificar validaciones con `DataAnnotations` o `FluentValidation` (si se introduces).

---

## 3. API de Compatibilidad (Proyecto `apc`)

### 3.1 Controladores
1. Crear `Controllers/ClientesController.cs` (y similares para cada bloque).  
2. Mapear endpoints equivalentes (GET búsquedas, GET detalle, POST/PUT/DELETE).  
3. Usar DTOs del paso 2.3 como contratos.  
4. Aplicar `[Authorize]` y políticas/roles heredados del legado.  
5. Registrar rutas especiales (p. ej. geolocalización `Ordenes/ObtenerCoordenadas`).

### 3.2 Endpoints auxiliares
1. Exponer endpoints para catálogos o combos usados por formularios (barrios, ciclos, tarifas, cuadrillas, etc.).  
2. Centralizar endpoints comunes para evitar duplicidad (ej. un `CatalogosController` compartido).

### 3.3 Reporting DevExpress
1. Convertir RDLC a `XtraReport` dentro de `SIAD.Reports` siguiendo la estructura `Reports/Clientes/RptCortesPorCliente.cs`.  
2. Registrar reportes en `CustomReportStorageWebExtension`.  
3. Crear endpoints `ReportsController` o utilizar el Report Designer integrado.  
4. En clientes, agregar páginas que consuman `DxDocumentViewer` / `DxReportDesigner`.

---

## 4. UI Blazor + DevExpress (`apc.Client`)

### 4.1 Estructura
- Crear carpeta `Pages/Clientes` con componentes:
  - `ClientesList.razor` (DxGrid con filtros, paginación, exportación).
  - `ClientesDetail.razor` (DxFormLayout + tabs para datos general, tarifas, medidores, historial).
  - `ClientesAccount.razor` (estado de cuenta, historiales, grillas cruzadas).
- Componentes parciales reusables (ej. `ClienteSearchForm.razor`, `MedidorList.razor`, etc.).
- Reubicar formularios parciales del legacy como componentes `Shared` si conviene.

### 4.2 Integración con API
- Consumir endpoints con `HttpClient`.
- Gestionar estados (loading/empty/error) con componentes DevExpress (p. ej. `DxLoadPanel`).
- Confirmaciones con `DxPopup` o `DxDialog`.

### 4.3 UI/UX
- Replicar experiencia del legado pero adaptada al layout actual (Drawer + NavMenu).  
- Agregar navegaciones cruzadas (Clientes ↔ Solicitudes ↔ Ordenes) sin recargar la app (usar `NavigationManager`).  
- Cuidar responsividad (hacer uso de `DxFormLayout` con `ItemsBreakpoints`).

---

## 5. Seguridad y Roles
- Revisar roles/claims asignados en el legado (administrador, cobranza, cuadrillas, etc.).  
- Crear un seed inicial (en un servicio o migración) para replicar esos roles en Identity.  
- Proteger endpoints y menú según permisos (usar `AuthorizeView` en Blazor).

---

## 6. Testing y Validación
- **Unit tests**: servicios críticos (estado de cuenta, cálculo de tarifas, generación de ordenes).  
- **Integration tests**: controladores API con `WebApplicationFactory`.  
- **UI tests**: opcionalmente bUnit para componentes sensibles.  
- **Reportes**: validar que DevExpress genera la misma salida que RDLC (comparar PDFs si es posible).

---

## 7. Documentación / Seguimiento
- Sección en este documento por cada bloque con:
  - Comando scaffold usado.
  - Servicios creados (interfaces/implementaciones).
  - Endpoints expuestos.
  - Páginas/componentes Blazor.
  - Reportes migrados.
  - Pendientes o diferencias con el legado.
- Actualizar `readme.md` y `trabajo.md` con hitos completados y próximos pasos.

---

## 8. Orden de Ejecución Detallado
1. **Clientes (nucleo)**  
   ***Base del cliente
      Base del cliente:
      Database/TablasPosgrets/cliente_maestro.sql
      Database/TablasPosgrets/cliente_detalle.sql
      Catálogos asociados a clientes:
      Database/TablasPosgrets/barrio.sql
      Database/TablasPosgrets/ciclos.sql
      Database/TablasPosgrets/categoria_servicio.sql
      Tarifas/configuración:
      Database/TablasPosgrets/tarifas.sql
      Database/TablasPosgrets/tarifas_contador.sql
      Database/TablasPosgrets/configuracion_tasas.sql
      Database/TablasPosgrets/configuracion_tasas_detalle.sql
      Database/TablasPosgrets/configuracion_cobros_adicionales.sql
      Database/TablasPosgrets/concepto_cobro_adicional.sql
      Historiales/estados de cuenta:
      Database/TablasPosgrets/historialmes.sql
      Database/TablasPosgrets/historicomedicion.sql
      Database/TablasPosgrets/historicosinmedidor.sql
      Database/TablasPosgrets/transaccion_abonado.sql
      Medidores:
      Database/TablasPosgrets/maestro_medidor.sql
      Otros catálogos auxiliares de clientes:
      Database/TablasPosgrets/servicios.sql
      Database/TablasPosgrets/condicion_lectura.sql
      Database/TablasPosgrets/tipo_uso_servicio.sql
   **esto se ejecuto
   dotnet tool run dotnet-ef dbcontext scaffold `
    "Host=3.208.232.209;Port=5432;Database=bdnes;Username=postgres;Password=Koala@2021;Timeout=10;SslMode=Prefer" `
    Npgsql.EntityFrameworkCore.PostgreSQL `
    -p SIAD.Data/SIAD.Data.csproj `
    -s apc/apc.csproj `
    -c SiadDbContext `
    --no-onconfiguring `
    --context-dir . `
    --context-namespace SIAD.Data `
    --namespace SIAD.Core.Entities `
    --use-database-names `
    --output-dir SIAD.Core/Entities `
    --force `
    -t cliente_maestro `
    -t cliente_detalle `
    -t barrio `
    -t ciclos `
    -t categoria_servicio `
    -t tarifas `
    -t tarifas_contador `
    -t configuracion_tasas `
    -t configuracion_tasas_detalle `
    -t configuracion_cobros_adicionales `
    -t concepto_cobro_adicional `
    -t historialmes `
    -t historicomedicion `
    -t historicosinmedidor `
    -t transaccion_abonado `
    -t maestro_medidor `
    -t servicios `
    -t condicion_lectura `
    -t tipo_uso_servicio
##--------------
DTOs: `ClienteListItemDto`, `ClienteDetailDto`, `ClienteFilterDto` (SIAD.Core/DTOs/Clientes).
Servicios: `IClientesService`, `ClientesService` con `GetClientesAsync`, `GetClienteAsync`, `SearchClientesAsync` (filtros con `EF.Functions.ILike`).
Endpoints: `GET /api/clientes`, `GET /api/clientes/{id}`, `GET /api/clientes/search` (por ahora `[AllowAnonymous]`).
UI: `ClientesList.razor` (grilla con filtros y enlace al detalle), `ClienteDetail.razor` (DxFormLayout con datos básicos).
##-----------------------
   a. Scaffold tablas base (`clientes`, `clientes_direccion`, `clientes_tarifa`, `clientes_historial_consumo`, `clientes_historial_pago`, etc.).  
   b. Servicios `ClientesService`, `ClientesTarifaService`, `EstadoCuentaService`.  
   c. Controladores `ClientesController`, endpoints auxiliares (historial, tarifas, etc.).  
   d. Páginas Blazor (`ClientesList`, `ClienteDetail`, `EstadoCuenta`).  
   e. Reportes `RptCortesPorCliente`, `RptAccionCobranza` en DevExpress.  
   f. Validar seguridad/permisos.  
   g. Documentar estado (TO‑DO, DONE, diferencias).
2. **Solicitud de Servicio**  
   a. Scaffold tablas `solicitudes`, etc.  
   b. Servicios `SolicitudesService`.  
   c. UI para altas rápidas, formularios parciales.  
   d. Flujo de integración con Clientes (al crear cliente, reflejar en la lista principal).
3. **Medidores**  
   a. Scaffold `medidores`, relaciones.  
   b. CRUD y asociación a clientes (UI + API).
4. **Auxiliar de Lectura**  
   a. Scaffold `lecturas`, `auxiliar_lectura`.  
   b. Módulo UI para lecturas masivas, soporte cuadrillas.
5. **Órdenes y Rutas**  
   a. Scaffold tablas y funciones para órdenes/rutas.  
   b. Servicios, API, componentes con mapas/geolocalización si aplica.  
   c. Reportes relacionados.

---

## 9. Pendientes Globales
- Definir DTOs compartidos para combos (barrios, ciclos, tarifas).  
- Unificar utilidades heredadas (generación de PDF, exportaciones, conversión número→letras).  
- Revisar dependencias Dapper del legado: evaluar si se mantienen en servicios específicos o se migran a EF Core.  
- Traducción de validaciones y mensajes al nuevo front.  
- Actualizar pipeline de CI/CD para incluir tests y publicación de reportes DevExpress.

---

## 10. Próxima Acción
- Ejecutar scaffold del bloque **Clientes** y registrar en este documento el comando exacto y las tablas generadas.  
- A partir de allí, crear el servicio y el primer endpoint (búsqueda de clientes) para verificar que leemos datos reales de `bdnes`.  
- En paralelo, preparar la conversión del primer reporte (`RptCortesPorCliente`) a DevExpress para validar el flujo completo.
