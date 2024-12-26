using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Update;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.Update;

public record UpdateFilmCommand(FilmUpdateRequestDto newFilm)
    : IRequest<Result<FilmResponseDto>>;