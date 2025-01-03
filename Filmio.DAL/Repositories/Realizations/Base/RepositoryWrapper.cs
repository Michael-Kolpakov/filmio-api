using Filmio.DAL.Persistence;
using Filmio.DAL.Repositories.Interfaces.Base;
using Filmio.DAL.Repositories.Interfaces.Film;
using Filmio.DAL.Repositories.Interfaces.User;
using Filmio.DAL.Repositories.Realizations.Film;
using Filmio.DAL.Repositories.Realizations.User;

namespace Filmio.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly FilmioDbContext _filmioDbContext;

    private IFilmRepository? _filmRepository;
    private IUserRepository? _userRepository;

    public RepositoryWrapper(FilmioDbContext filmioDbContext)
    {
        _filmioDbContext = filmioDbContext;
    }

    public IFilmRepository FilmRepository
    {
        get
        {
            return _filmRepository ??= new FilmRepository(_filmioDbContext);
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepository ??= new UserRepository(_filmioDbContext);
        }
    }

    public int SaveChanges()
    {
        return _filmioDbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _filmioDbContext.SaveChangesAsync();
    }
}