using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Film.GetById;

public class GetFilmByIdHandler : IRequestHandler<GetFilmByIdQuery, Result<FilmResponseDto>>
{
    private const string _notFoundErrorMessage = "There is no film with such Id:";

    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetFilmByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<FilmResponseDto>> Handle(GetFilmByIdQuery request, CancellationToken cancellationToken)
    {
        var film = await _repositoryWrapper.FilmRepository.GetSingleOrDefaultAsync(x => x.Id == request.id);

        if (film == null)
        {
            _logger.LogError(request, $"{_notFoundErrorMessage} {request.id}");

            return Result.Fail($"{_notFoundErrorMessage} {request.id}");
        }

        return Result.Ok(_mapper.Map<FilmResponseDto>(film));
    }
}