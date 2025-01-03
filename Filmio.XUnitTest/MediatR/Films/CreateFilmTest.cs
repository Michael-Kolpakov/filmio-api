using AutoFixture;
using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Create;
using Filmio.BLL.MediatR.Film.Create;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Entities.Films;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentAssertions;
using Moq;

namespace Filmio.XUnitTest.MediatR.Films;

public class CreateFilmTest
{
    private const string _failedCreateFilmError = "Failed to create a film";

    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly IFixture _fixture;

    public CreateFilmTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupMapper(testFilm);

        _mockRepository.Setup(x => x.FilmRepository.CreateAsync(It.Is<Film>(y => y.Id == testFilm.Id)))
            .ReturnsAsync(testFilm);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var handler = new CreateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreateFilmCommand(GetFilmCreateRequestDto()), CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<FilmResponseDto>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenFilmAdded()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupMapper(testFilm);

        _mockRepository.Setup(x => x.FilmRepository.CreateAsync(It.Is<Film>(y => y.Id == testFilm.Id)))
            .ReturnsAsync(testFilm);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var handler = new CreateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreateFilmCommand(GetFilmCreateRequestDto()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenFilmCreationFails()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupMapper(testFilm);

        _mockRepository.Setup(x => x.FilmRepository.CreateAsync(It.Is<Film>(y => y.Id == testFilm.Id)))
            .ThrowsAsync(new Exception(_failedCreateFilmError));

        var handler = new CreateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreateFilmCommand(GetFilmCreateRequestDto()), CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(_failedCreateFilmError);
    }

    [Fact]
    public async Task ShouldThrowError_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrage
        var testFilm = GetFilm();

        SetupMapper(testFilm);

        _mockRepository.Setup(x => x.FilmRepository.CreateAsync(It.Is<Film>(y => y.Id == testFilm.Id)))
            .ReturnsAsync(testFilm);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ThrowsAsync(new Exception(_failedCreateFilmError));

        var handler = new CreateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreateFilmCommand(GetFilmCreateRequestDto()), CancellationToken.None);

        // Assert
        result.Errors[0].Message.Should().Be(_failedCreateFilmError);
    }

    [Fact]
    public async Task ShouldLogError_WhenExceptionThrown()
    {
        // Arrange
        var testFilm = GetFilm();

        SetupMapper(testFilm);

        _mockRepository.Setup(x => x.FilmRepository.CreateAsync(It.Is<Film>(y => y.Id == testFilm.Id)))
            .ReturnsAsync(testFilm);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ThrowsAsync(new Exception(_failedCreateFilmError));

        var handler = new CreateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        await handler.Handle(new CreateFilmCommand(GetFilmCreateRequestDto()), CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.LogError(It.IsAny<object>(), _failedCreateFilmError), Times.Once);
    }

    private Film GetFilm()
    {
        return _fixture.Build<Film>()
            .With(temp => temp.Id, 1)
            .Create();
    }

    private FilmResponseDto GetFilmResponseDto()
    {
        return _fixture.Create<FilmResponseDto>();
    }

    private FilmCreateRequestDto GetFilmCreateRequestDto()
    {
        return _fixture.Create<FilmCreateRequestDto>();
    }

    private void SetupMapper(Film testFilm)
    {
        _mockMapper.Setup(x => x.Map<Film>(It.IsAny<FilmCreateRequestDto>()))
            .Returns(testFilm);
        _mockMapper.Setup(x => x.Map<FilmResponseDto>(It.IsAny<Film>()))
            .Returns(GetFilmResponseDto());
    }
}