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

        return services;
    }
}