using AutoMapper;
using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.Services.Interfaces.Logging;
using filmio_api.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.Create;

public class CreateFilmHandler : IRequestHandler<CreateFilmCommand, Result<FilmResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public CreateFilmHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<FilmResponseDto>> Handle(CreateFilmCommand request, CancellationToken cancellationToken)
    {
        var newFilm = _mapper.Map<DAL.Entities.Film.Film>(request.newFilm);

        try
        {
            newFilm = await _repositoryWrapper.FilmRepository.CreateAsync(newFilm);
            await _repositoryWrapper.SaveChangesAsync();

            return Result.Ok(_mapper.Map<FilmResponseDto>(newFilm));
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);

            return Result.Fail(ex.Message);
        }
    }
}