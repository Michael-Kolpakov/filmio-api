using Filmio.BLL.DTO.Film;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.GetById;

public record GetFilmByIdQuery(int id)
    : IRequest<Result<FilmResponseDto>>;