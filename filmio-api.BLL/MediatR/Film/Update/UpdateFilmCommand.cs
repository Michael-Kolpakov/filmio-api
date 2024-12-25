using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.DTO.Film.Update;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.Update;

public record UpdateFilmCommand(FilmUpdateRequestDto newFilm)
    : IRequest<Result<FilmResponseDto>>;