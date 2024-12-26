using System.Reflection;
using filmio_api.BLL.Services.Interfaces.Logging;
using filmio_api.BLL.Services.Realizations.Logging;
using filmio_api.DAL.Repositories.Interfaces.Base;
using filmio_api.DAL.Repositories.Realizations.Base;
using Microsoft.AspNetCore.Mvc;

namespace filmio_api.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var bllAssembly = Assembly.Load("filmio-api.BLL");

        services.AddScoped<ILoggerService, LoggerService>();

        services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));
        });
        services.AddCustomDbContext(configuration);
        services.AddSwagger();
        services.AddSerilogLogging(configuration, environment);
        services.AddRepositoryServices();
        services.AddAutoMapper(currentAssemblies);
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(bllAssembly));
        services.AddCors();

        return services;
    }

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            { 
                policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }
}