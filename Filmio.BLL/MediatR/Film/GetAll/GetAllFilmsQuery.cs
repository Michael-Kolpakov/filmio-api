using Filmio.BLL.DTO.Film;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.GetAll;

public record GetAllFilmsQuery(ushort? page = null, ushort? pageSize = null)
    : IRequest<Result<GetAllFilmsResponseDto>>;