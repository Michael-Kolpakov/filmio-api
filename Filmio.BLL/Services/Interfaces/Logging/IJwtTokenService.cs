using System.Security.Claims;
using Filmio.DAL.Entities.Users;

namespace Filmio.BLL.Services.Interfaces.Logging;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
}