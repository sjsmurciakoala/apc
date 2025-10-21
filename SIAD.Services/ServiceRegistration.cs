using Microsoft.Extensions.DependencyInjection;
using SIAD.Core.Entities;
using SIAD.Services;
using SIAD.Services.Clientes;
using SIAD.Services.Solicitudes;
using SIAD.Services.Medidores;
namespace SIAD.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddSiadServices(this IServiceCollection services)
    {
        // Add AutoMapper profiles and service implementations here.
        services.AddAutoMapper(typeof(ServiceRegistration).Assembly);

        services.AddScoped<IClientesService, ClientesService>();

        //solicitudes
        services.AddScoped<ISolicitudesService, SolicitudesService>();
        

        //medidores
        services.AddScoped<IMedidoresService, MedidoresService>();

       // agregar más servicios después
        return services;
    }
}
