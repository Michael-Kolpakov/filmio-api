using Filmio.DAL.Persistence;
using Filmio.DAL.Repositories.Interfaces.Film;
using Filmio.DAL.Repositories.Realizations.Base;

namespace Filmio.DAL.Repositories.Realizations.Film;

public class FilmRepository : RepositoryBase<Filmio.DAL.Entities.Film.Film>, IFilmRepository
{
    public FilmRepository(FilmioDbContext context)
        : base(context)
    {
    }
}