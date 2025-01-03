using Filmio.DAL.Entities.Films;
using Filmio.DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Filmio.DAL.Persistence;

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
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}