using filmio_api.DAL.Entities.Film;
using Microsoft.EntityFrameworkCore;

namespace filmio_api.DAL.Persistence;

public class FilmioDbContext : DbContext
{
    public FilmioDbContext()
    {
    }

    public FilmioDbContext(DbContextOptions<FilmioDbContext> options)
        : base(options)
    {
    }

    public DbSet<Film> Films { get; set; } = null!;
}