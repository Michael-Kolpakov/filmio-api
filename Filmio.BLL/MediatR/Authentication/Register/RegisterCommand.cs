using Filmio.BLL.DTO.Authentication.Register;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Authentication.Register;

public record RegisterCommand(RegisterRequestDto registerRequestDto)
    : IRequest<Result<RegisterResponseDto>>;