namespace SIAD.Core.DTOs.Clientes;

    public record ClienteTarifaDto
    (
        int ConfiguracionId,
       decimal? Monto,
       int? ServicioId,
       DateTime? FechaCracion,
       DateTime? FechaModificacion,
       bool Activa);
    
