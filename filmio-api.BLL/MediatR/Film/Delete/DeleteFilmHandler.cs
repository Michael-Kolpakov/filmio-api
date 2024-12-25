using AutoMapper;
using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.Services.Interfaces.Logging;
using filmio_api.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.Delete;

public class DeleteFilmHandler : IRequestHandler<DeleteFilmCommand, Result<FilmResponseDto>>
{
    private const string _notFoundErrorMessage = "There is no film with such Id";
    
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public DeleteFilmHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<FilmResponseDto>> Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await _repositoryWrapper.FilmRepository.GetFirstOrDefaultAsync(x => x.Id == request.id);

        if (film == null)
        {
            _logger.LogError(request, _notFoundErrorMessage);

            return Result.Fail(_notFoundErrorMessage);
        }

        try
        {
            _repositoryWrapper.FilmRepository.Delete(film);
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