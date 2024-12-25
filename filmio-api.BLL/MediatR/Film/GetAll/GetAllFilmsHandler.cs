using AutoMapper;
using filmio_api.BLL.DTO.Film;
using filmio_api.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace filmio_api.BLL.MediatR.Film.GetAll;

public class GetAllFilmsHandler : IRequestHandler<GetAllFilmsQuery, Result<GetAllFilmsResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllFilmsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public Task<Result<GetAllFilmsResponseDto>> Handle(GetAllFilmsQuery request, CancellationToken cancellationToken)
    {
        var paginatedFilms = _repositoryWrapper.FilmRepository.GetAllPaginated(request.page, request.pageSize);

        var filmsResponse = new GetAllFilmsResponseDto()
        {
            TotalAmount = paginatedFilms.TotalItems,
            Films = _mapper.Map<IEnumerable<FilmResponseDto>>(paginatedFilms.Entities)
        };

        return Task.FromResult(Result.Ok(filmsResponse));
    }
}