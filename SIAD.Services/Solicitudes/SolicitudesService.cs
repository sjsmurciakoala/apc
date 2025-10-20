using System.Linq;
using Microsoft.EntityFrameworkCore;
using SIAD.Core.DTOs.Solicitudes;
using SIAD.Core.Entities;
using SIAD.Data;

namespace SIAD.Services.Solicitudes;

public class SolicitudesService : ISolicitudesService
{
    private readonly SiadDbContext _context;

    public SolicitudesService(SiadDbContext context) => _context = context;

    public async Task<IReadOnlyList<SolicitudListDto>> GetSolicitudesAsync(string? clienteIdentidad, CancellationToken ct = default)
    {
        var query = _context.solicitud_servicios
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(clienteIdentidad))
        {
            query = query.Where(s => s.cliente_identidad == clienteIdentidad);
        }

        return await query
            .OrderByDescending(s => s.fechacreacion)
            .Select(s => new SolicitudListDto(
                s.solicitud_servicio_id,
                s.cliente_identidad,
                s.cliente_nombre,
                s.fechacreacion ?? DateTime.MinValue,
                s.estado))
            .ToListAsync(ct);
    }

    public async Task<SolicitudDetailDto?> GetSolicitudAsync(int id, CancellationToken ct = default)
    {
        return await _context.solicitud_servicios
            .AsNoTracking()
            .Where(s => s.solicitud_servicio_id == id)
            .Select(s => new SolicitudDetailDto(
                s.solicitud_servicio_id,
                s.cliente_identidad,
                s.cliente_nombre,
                s.cliente_telefono ?? string.Empty,
                s.cliente_direccion,
                s.observacion,
                s.fechacreacion ?? DateTime.MinValue,
                s.estado))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<int> CreateSolicitudAsync(SolicitudDetailDto dto, CancellationToken ct = default)
    {
        var entity = new solicitud_servicio
        {
            cliente_identidad = dto.IdentificacionCliente,
            cliente_nombre = dto.NombreCliente,
            cliente_telefono = dto.Telefono,
            cliente_direccion = dto.Direccion,
            observacion = dto.Observacion,
            estado = dto.Estado,
            fechacreacion = DateTime.UtcNow,
            usuariocreacion = "api"
        };

        _context.solicitud_servicios.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity.solicitud_servicio_id;
    }
}
