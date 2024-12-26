using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.Update;

public class UpdateFilmHandler : IRequestHandler<UpdateFilmCommand, Result<FilmResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public UpdateFilmHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<FilmResponseDto>> Handle(UpdateFilmCommand request, CancellationToken cancellationToken)
    {
        var film = _mapper.Map<DAL.Entities.Film.Film>(request.newFilm);

        try
        {
            _repositoryWrapper.FilmRepository.Update(film);
            await _repositoryWrapper.SaveChangesAsync();

            var filmResponse = _mapper.Map<FilmResponseDto>(film);

            return Result.Ok(filmResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);

            return Result.Fail(ex.Message);
        }
    }
}