using filmio_api.BLL.DTO.Film;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.GetById;

public record GetFilmByIdQuery(int id)
    : IRequest<Result<FilmResponseDto>>;