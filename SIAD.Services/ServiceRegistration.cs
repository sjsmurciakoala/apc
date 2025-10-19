using Microsoft.Extensions.DependencyInjection;
using SIAD.Core.Entities;
using SIAD.Services.Clientes;
namespace SIAD.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddSiadServices(this IServiceCollection services)
    {
        // Add AutoMapper profiles and service implementations here.
        services.AddAutoMapper(typeof(ServiceRegistration).Assembly);

         services.AddScoped<IClientesService, ClientesService>();
       // agregar más servicios después
        return services;
    }
}
