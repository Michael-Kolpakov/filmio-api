using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.MediatR.Film.GetById;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Entities.Films;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace Filmio.XUnitTest.MediatR.Films;

public class GetFilmByIdTest
{
    private const string _noFilmWithSuchIdErorr = "There is no film with such Id:";
    private const string _notFoundFilmError = "The film wasn`t found";
    
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly IFixture _fixture;

    public GetFilmByIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingId()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupRepository(testFilm);
        SetupMapper(GetFilmResponseDto());

        var handler = new GetFilmByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetFilmByIdQuery(testFilm.Id), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(testFilm.Id);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupRepository(testFilm);
        SetupMapper(GetFilmResponseDto());

        var handler = new GetFilmByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetFilmByIdQuery(testFilm.Id), CancellationToken.None);

        // Assert
        result.ValueOrDefault.Should().NotBeNull();
        result.ValueOrDefault.Should().BeOfType<FilmResponseDto>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenFilmIsMappedCorrectly()
    {
        // Arrange
        var testFilm = GetFilm();
        var testFilmResponseDto = GetFilmResponseDto();

        SetupRepository(testFilm);
        SetupMapper(testFilmResponseDto);

        var handler = new GetFilmByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetFilmByIdQuery(testFilm.Id), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(testFilmResponseDto);
    }

    [Fact]
    public async Task ShouldReturnErrorResponse_NotExistingId()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupRepository(GetFilmWithNotExistingId());
        SetupMapper(GetFilmResponseDtoWithNotExistingId());

        var handler = new GetFilmByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetFilmByIdQuery(testFilm.Id), CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be($"{_noFilmWithSuchIdErorr} {testFilm.Id}");
    }

    [Fact]
    public async Task ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var testFilm = GetFilm();

        _mockRepository.Setup(x => x.FilmRepository
                .GetSingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Film, bool>>>(),
                    It.IsAny<Func<IQueryable<Film>,
                        IIncludableQueryable<Film, object>>>()))
            .ThrowsAsync(new Exception(_notFoundFilmError));

        var handler = new GetFilmByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        Func<Task> act = async () => await handler.Handle(new GetFilmByIdQuery(testFilm.Id), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(_notFoundFilmError);
    }

    [Fact]
    public async Task ShouldLogError_WhenFilmNotFound()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupRepository(GetFilmWithNotExistingId());
        SetupMapper(GetFilmResponseDtoWithNotExistingId());

        var handler = new GetFilmByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        await handler.Handle(new GetFilmByIdQuery(testFilm.Id), CancellationToken.None);

        // Assert
        _mockLogger.Verify(x =>
            x.LogError(It.IsAny<object>(), $"{_noFilmWithSuchIdErorr} {testFilm.Id}"), Times.Once);
    }

    private Film GetFilm()
    {
        return _fixture.Build<Film>()
            .With(temp => temp.Id, 1)
            .Create();
    }

    private FilmResponseDto GetFilmResponseDto()
    {
        return _fixture.Build<FilmResponseDto>()
            .With(temp => temp.Id, 1)
            .Create();
    }

    private static Film? GetFilmWithNotExistingId()
    {
        return null;
    }

    private static FilmResponseDto? GetFilmResponseDtoWithNotExistingId()
    {
        return null;
    }
    
    private void SetupRepository(Film? film)
    {
        _mockRepository.Setup(x => x.FilmRepository
                .GetSingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Film, bool>>>(),
                    It.IsAny<Func<IQueryable<Film>,
                        IIncludableQueryable<Film, object>>>()))
            .ReturnsAsync(film);
    }
    
    private void SetupMapper(FilmResponseDto? filmResponseDto)
    {
        _mockMapper
            .Setup(x => x
                .Map<FilmResponseDto?>(It.IsAny<Film>()))
            .Returns(filmResponseDto);
    }
}