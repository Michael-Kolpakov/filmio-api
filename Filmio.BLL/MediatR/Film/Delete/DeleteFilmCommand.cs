using Filmio.BLL.DTO.Film;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.Delete;

public record DeleteFilmCommand(int id)
    : IRequest<Result<FilmResponseDto>>;