using filmio_api.DAL.Persistence;
using filmio_api.DAL.Repositories.Interfaces.Base;
using filmio_api.DAL.Repositories.Interfaces.Film;
using filmio_api.DAL.Repositories.Realizations.Film;

namespace filmio_api.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly FilmioDbContext _filmioDbContext;

    private IFilmRepository? _filmRepository;

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

    public int SaveChanges()
    {
        return _filmioDbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _filmioDbContext.SaveChangesAsync();
    }
}