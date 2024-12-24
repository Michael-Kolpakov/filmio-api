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
        services.AddCustomDbContext(configuration);
        services.AddSerilogLogging(configuration, environment);
        services.AddRepositoryServices();

        return services;
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }
}