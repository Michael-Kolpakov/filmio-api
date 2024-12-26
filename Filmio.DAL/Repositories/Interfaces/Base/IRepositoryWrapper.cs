using Filmio.DAL.Repositories.Interfaces.Film;

namespace Filmio.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IFilmRepository FilmRepository { get; }

    public int SaveChanges();

    public Task<int> SaveChangesAsync();
}