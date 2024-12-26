using System.Text.Json;
using Filmio.DAL.Entities.Film;
using Filmio.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace filmio_api.DAL.Persistence.Seed;

public static class FilmioDbSeed
{
    public static async Task SeedAsync(FilmioDbContext context)
    {
        await SeedFilmsAsync(context);
    }

    private static async Task SeedFilmsAsync(FilmioDbContext context)
    {
        var currentFilms = await context.Films.ToListAsync();

        if (currentFilms.Count != 0)
        {
            return;
        }

        var assemblyPath = Path.GetDirectoryName(typeof(FilmioDbSeed).Assembly.Location);
        var filmsJsonPath = Path.Combine(assemblyPath!, @"Persistence\Seed\Content\Films.json");
        var jsonData = await File.ReadAllTextAsync(filmsJsonPath);

        var filmsToAdd = JsonSerializer.Deserialize<List<Film>>(jsonData);

        if (filmsToAdd == null || filmsToAdd.Count == 0)
        {
            return;
        }

        await context.Films.AddRangeAsync(filmsToAdd);
        await context.SaveChangesAsync();
    }
}