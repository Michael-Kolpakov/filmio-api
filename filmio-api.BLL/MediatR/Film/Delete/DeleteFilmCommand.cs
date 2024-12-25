using filmio_api.BLL.DTO.Film;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.Delete;

public record DeleteFilmCommand(int id)
    : IRequest<Result<FilmResponseDto>>;