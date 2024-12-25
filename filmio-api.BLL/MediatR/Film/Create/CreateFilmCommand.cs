using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.DTO.Film.Create;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.Create;

public record CreateFilmCommand(FilmCreateRequestDto newFilm)
    : IRequest<Result<FilmResponseDto>>;