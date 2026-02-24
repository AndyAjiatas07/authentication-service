using AuthService.Domain.Entitis;
using AuthService.Domain.Constants;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;
 
namespace AuthService.Api.Extensions;
 
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention());
 
        // INICIALIZANDO LA CONEXION A LA BASE DE DATOS
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("EmailServerConnection"))
                .UseSnakeCaseNamingConvention());
 
        // INICIALIZANDO EL SERVICIO DE EMAIL
        services.AddScoped<IEmailService, EmailService>();
 
        services.AddHealthChecks();
 
        return services;
    }
}