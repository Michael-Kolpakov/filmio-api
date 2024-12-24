using filmio_api.DAL.Repositories.Interfaces.Film;

namespace filmio_api.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IFilmRepository FilmRepository { get; }

    public int SaveChanges();

    public Task<int> SaveChangesAsync();
}