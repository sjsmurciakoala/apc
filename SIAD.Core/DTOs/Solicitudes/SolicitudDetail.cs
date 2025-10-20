namespace SIAD.Core.DTOs.Solicitudes;

public record SolicitudDetailDto(
    int Id,
    string IdentificacionCliente,
    string NombreCliente,
    string Telefono,
    string Direccion,
    string? Observacion,
    DateTime Fecha,
    bool Estado);
