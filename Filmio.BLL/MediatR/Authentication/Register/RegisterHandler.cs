using AutoMapper;
using Filmio.BLL.DTO.Authentication.Register;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Entities.Users;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Authentication.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, Result<RegisterResponseDto>>
{
    private const string _notUniqueUserErrorMessage = "A user with this email already exists";

    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IJwtTokenService _jwtTokenService;
    
    public RegisterHandler(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IJwtTokenService jwtTokenService)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _jwtTokenService = jwtTokenService;
    }
    
    public async Task<Result<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<User>(request.registerRequestDto);
        var uniqueUserResult = await IsUserUnique(newUser);

        if (uniqueUserResult.IsFailed)
        {
            _logger.LogError(request, uniqueUserResult.Errors[0].Message);

            return Result.Fail(uniqueUserResult.Errors);
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(newUser);

        try
        {
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);

            newUser = await _repositoryWrapper.UserRepository.CreateAsync(newUser);
            await _repositoryWrapper.SaveChangesAsync();

            var registerResponseDto = _mapper.Map<RegisterResponseDto>(newUser);
            registerResponseDto.AccessToken = accessToken;
            
            return Result.Ok(registerResponseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);

            return Result.Fail(ex.Message);
        }
    }
    
    private async Task<Result> IsUserUnique(User user)
    {
        var userFromDbDyEmail = await _repositoryWrapper.UserRepository
            .GetFirstOrDefaultAsync(userFromDb => userFromDb.Email == user.Email);

        if (userFromDbDyEmail != null)
        {
            return Result.Fail(_notUniqueUserErrorMessage);
        }

        return Result.Ok();
    }
}