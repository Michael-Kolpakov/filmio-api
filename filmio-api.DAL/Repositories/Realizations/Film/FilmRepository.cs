using filmio_api.DAL.Persistence;
using filmio_api.DAL.Repositories.Interfaces.Film;
using filmio_api.DAL.Repositories.Realizations.Base;

namespace filmio_api.DAL.Repositories.Realizations.Film;

public class FilmRepository : RepositoryBase<Entities.Film.Film>, IFilmRepository
{
    public FilmRepository(FilmioDbContext context)
        : base(context)
    {
    }
}