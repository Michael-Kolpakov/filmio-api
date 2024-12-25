using filmio_api.BLL.DTO.Film;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.GetAll;

public record GetAllFilmsQuery(ushort? page = null, ushort? pageSize = null)
    : IRequest<Result<GetAllFilmsResponseDto>>;