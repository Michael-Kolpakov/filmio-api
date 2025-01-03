using Filmio.DAL.Repositories.Interfaces.Film;
using Filmio.DAL.Repositories.Interfaces.User;

namespace Filmio.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IFilmRepository FilmRepository { get; }

    IUserRepository UserRepository { get; }

    public int SaveChanges();

    public Task<int> SaveChangesAsync();
}