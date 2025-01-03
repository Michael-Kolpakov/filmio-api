using Filmio.BLL.DTO.Authentication.Login;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Authentication.Login;

public record LoginQuery(LoginRequestDto loginRequestDto)
    : IRequest<Result<LoginResponseDto>>;