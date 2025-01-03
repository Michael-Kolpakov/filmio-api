using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.MediatR.Film.GetAll;
using Filmio.DAL.Entities.Films;
using Filmio.DAL.Helpers;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace Filmio.XUnitTest.MediatR.Films;

public class GetAllFilmsTest
{
    private const uint _listLength = 5;
    private const ushort _pageSize = 3;
    
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IFixture _fixture;

    public GetAllFilmsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        SetupPaginatedRepository(GetFilmsEnumeration(_listLength));
        SetupMapper(GetFilmResponseDtoEnumeration(_listLength));

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Films.Should().BeOfType<List<FilmResponseDto>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        var filmsList = GetFilmsEnumeration(_listLength);
        SetupPaginatedRepository(filmsList);
        SetupMapper(GetFilmResponseDtoEnumeration(_listLength));

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Films.Should().HaveCount(filmsList.Count);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_HandlerReturnsCorrectPageSize()
    {
        // Arrange
        SetupPaginatedRepository(GetFilmsEnumeration(_listLength).Take(_pageSize));
        SetupMapper(GetFilmResponseDtoEnumeration(_listLength).Take(_pageSize).ToList());

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(page: 1, pageSize: _pageSize), CancellationToken.None);

        // Assert
        result.Value.Films.Should().BeOfType<List<FilmResponseDto>>();
        result.Value.Films.Should().HaveCount(_pageSize);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenNoFilmsExist()
    {
        // Arrange
        SetupPaginatedRepository(new List<Film>());
        SetupMapper(new List<FilmResponseDto>());

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Films.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnCorrectTotalAmount()
    {
        // Arrange
        var filmsList = GetFilmsEnumeration(_listLength);
        SetupPaginatedRepository(filmsList);
        SetupMapper(GetFilmResponseDtoEnumeration(_listLength));

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.TotalAmount.Should().Be(filmsList.Count);
    }
    
    [Fact]
    public async Task ShouldReturnCorrectFilms_WhenPageAndPageSizeAreSpecified()
    {
        // Arrange
        var filmsList = GetFilmsEnumeration(_listLength);
        SetupPaginatedRepository(filmsList.Take(_pageSize));
        SetupMapper(GetFilmResponseDtoEnumeration(_listLength).Take(_pageSize).ToList());

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(1, _pageSize), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Films.Should().HaveCount(_pageSize);
        result.Value.Films.Should().BeEquivalentTo(filmsList.Take(_pageSize), options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task ShouldReturnCorrectFilms_WhenPageIsOutOfRange()
    {
        // Arrange
        SetupPaginatedRepository(new List<Film>());
        SetupMapper(new List<FilmResponseDto>());

        var handler = new GetAllFilmsHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllFilmsQuery(10, _pageSize), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Films.Should().BeEmpty();
    }

    private Film CreateFilm(int id)
    {
        return _fixture.Build<Film>()
            .With(f => f.Id, id)
            .With(f => f.Title, $"Title{id}")
            .With(f => f.Genre, $"Genre{id}")
            .With(f => f.Director, $"Director{id}")
            .With(f => f.ReleaseDate, DateTime.Parse("2022-07-29T08:05:23Z"))
            .With(f => f.Rating, id * 0.1F)
            .With(f => f.Description, $"Description{id}")
            .Create();
    }

    private FilmResponseDto CreateFilmResponseDto(int id)
    {
        return _fixture.Build<FilmResponseDto>()
            .With(f => f.Id, id)
            .With(f => f.Title, $"Title{id}")
            .With(f => f.Genre, $"Genre{id}")
            .With(f => f.Director, $"Director{id}")
            .With(f => f.ReleaseDate, DateTime.Parse("2022-07-29T08:05:23Z"))
            .With(f => f.Rating, id * 0.1F)
            .With(f => f.Description, $"Description{id}")
            .Create();
    }

    private List<Film> GetFilmsEnumeration(uint count)
    {
        var filmsList = new List<Film>();

        for (int i = 1; i <= count; i++)
        {
            filmsList.Add(CreateFilm(i));
        }

        return filmsList;
    }

    private List<FilmResponseDto> GetFilmResponseDtoEnumeration(uint count)
    {
        var filmResponseDtoList = new List<FilmResponseDto>();

        for (int i = 1; i <= count; i++)
        {
            filmResponseDtoList.Add(CreateFilmResponseDto(i));
        }

        return filmResponseDtoList;
    }

    private void SetupPaginatedRepository(IEnumerable<Film> returnList)
    {
        _mockRepository.Setup(repo => repo.FilmRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<Film, Film>>?>(),
                It.IsAny<Expression<Func<Film, bool>>?>(),
                It.IsAny<Func<IQueryable<Film>, IIncludableQueryable<Film, object>>?>(),
                It.IsAny<Expression<Func<Film, object>>?>(),
                It.IsAny<Expression<Func<Film, object>>?>()))
            .Returns(PaginationResponse<Film>.Create(returnList.AsQueryable()));
    }

    private void SetupMapper(IEnumerable<FilmResponseDto> returnList)
    {
        _mockMapper.Setup(x => x.Map<IEnumerable<FilmResponseDto>>(It.IsAny<IEnumerable<Film>>()))
            .Returns(returnList);
    }
}