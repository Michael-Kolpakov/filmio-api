using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Create;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.Create;

public record CreateFilmCommand(FilmCreateRequestDto newFilm)
    : IRequest<Result<FilmResponseDto>>;