using AutoMapper;
using Filmio.BLL.DTO.Authentication.Login;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Authentication.Login;

public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResponseDto>>
{
    private const string _wrongLoginOrPasswordErrorMessage = "Неверное имя пользователя или пароль.";

    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IJwtTokenService _jwtTokenService;
    
    public LoginHandler(
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

    public async Task<Result<LoginResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository
            .GetSingleOrDefaultAsync(u => u.Email == request.loginRequestDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.loginRequestDto.Password, user.PasswordHash))
        {
            _logger.LogError(request, _wrongLoginOrPasswordErrorMessage);

            return Result.Fail(_wrongLoginOrPasswordErrorMessage);
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(user);

        var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
        loginResponseDto.AccessToken = accessToken;

        return Result.Ok(loginResponseDto);
    }
}