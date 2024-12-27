using Filmio.DAL.Persistence;
using Filmio.DAL.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace Filmio.WebApi.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationsAssembly = typeof(FilmioDbContext).Assembly.GetName().Name;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<FilmioDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                opt => opt.MigrationsAssembly(migrationsAssembly)));

        return services;
    }

    public static void InitializeDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            var filmioDbContext = serviceScope.ServiceProvider.GetRequiredService<FilmioDbContext>();
            filmioDbContext.Database.Migrate();
            FilmioDbSeed.SeedAsync(filmioDbContext).Wait();
        }
    }
}