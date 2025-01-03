using Filmio.DAL.Persistence;
using Filmio.DAL.Repositories.Interfaces.User;
using Filmio.DAL.Repositories.Realizations.Base;

namespace Filmio.DAL.Repositories.Realizations.User;

public class UserRepository : RepositoryBase<Entities.Users.User>, IUserRepository
{
    public UserRepository(FilmioDbContext context)
        : base(context)
    {
    }
}