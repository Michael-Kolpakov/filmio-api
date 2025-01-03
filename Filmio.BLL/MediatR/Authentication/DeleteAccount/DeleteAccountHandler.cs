using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Authentication.DeleteAccount;

public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, Result<Unit>>
{
    private const string _userNotFoundErrorMessage = "User with such Id not found";

    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    
    public DeleteAccountHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository.GetSingleOrDefaultAsync(u => u.Id == request.userId);

        if (user == null)
        {
            _logger.LogError(request, _userNotFoundErrorMessage);

            return Result.Fail(_userNotFoundErrorMessage);
        }

        try
        {
            _repositoryWrapper.UserRepository.Delete(user);
            await _repositoryWrapper.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);

            return Result.Fail(ex.Message);
        }
    }
}