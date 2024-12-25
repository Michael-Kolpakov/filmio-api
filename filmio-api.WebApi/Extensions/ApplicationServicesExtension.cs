using filmio_api.BLL.Services.Interfaces.Logging;
using filmio_api.BLL.Services.Realizations.Logging;
using filmio_api.DAL.Repositories.Interfaces.Base;
using filmio_api.DAL.Repositories.Realizations.Base;

namespace filmio_api.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddCustomDbContext(configuration);
        services.AddSerilogLogging(configuration, environment);
        services.AddRepositoryServices();
        services.AddAutoMapper(currentAssemblies);
        services.AddScoped<ILoggerService, LoggerService>();

        return services;
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }
}