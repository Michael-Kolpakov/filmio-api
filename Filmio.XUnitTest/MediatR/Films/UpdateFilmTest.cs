using AutoFixture;
using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Update;
using Filmio.BLL.MediatR.Film.Update;
using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.DAL.Entities.Film;
using Filmio.DAL.Repositories.Interfaces.Base;
using FluentAssertions;
using Moq;

namespace Filmio.XUnitTest.MediatR.Films;

public class UpdateFilmTest
{
    private const string _filmNotUpdatedError = "The film wasn`t updated";

    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly IFixture _fixture;

    public UpdateFilmTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldUpdateSuccessfully_WhenFilmUpdated()
    {
        // Arrange
        var testFilm = GetFilm();
        var testFilmUpdateRequestDto = GetFilmUpdateRequestDto();
        var testFilmResponseDto = GetFilmResponseDto();

        SetupMapper(testFilm, testFilmResponseDto);

        _mockRepository.Setup(x => x.FilmRepository.Update(It.IsAny<Film>()));
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var handler = new UpdateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new UpdateFilmCommand(testFilmUpdateRequestDto), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(testFilmResponseDto);

        _mockRepository.Verify(x => x.FilmRepository.Update(It.Is<Film>(x => x.Id == testFilm.Id)), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenExceptionThrownDuringUpdate()
    {
        // Arrange
        var testFilm = GetFilm();
        var testFilmUpdateRequestDto = GetFilmUpdateRequestDto();
        var testFilmResponseDto = GetFilmResponseDto();

        SetupMapper(testFilm, testFilmResponseDto);

        _mockRepository.Setup(x => x.FilmRepository.Update(It.IsAny<Film>()))
            .Throws(new Exception(_filmNotUpdatedError));

        var handler = new UpdateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new UpdateFilmCommand(testFilmUpdateRequestDto), CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(_filmNotUpdatedError);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenSaveChangesFails()
    {
        // Arrange
        var testFilm = GetFilm();
        var testFilmUpdateRequestDto = GetFilmUpdateRequestDto();

        SetupMapper(testFilm);

        _mockRepository.Setup(x => x.FilmRepository.Update(It.IsAny<Film>()));
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ThrowsAsync(new Exception(_filmNotUpdatedError));

        var handler = new UpdateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new UpdateFilmCommand(testFilmUpdateRequestDto), CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(_filmNotUpdatedError);
    }

    [Fact]
    public async Task ShouldLogError_WhenExceptionThrownDuringUpdate()
    {
        // Arrange
        var testFilm = GetFilm();
        var testFilmUpdateRequestDto = GetFilmUpdateRequestDto();
        var testFilmResponseDto = GetFilmResponseDto();

        SetupMapper(testFilm, testFilmResponseDto);

        _mockRepository.Setup(x => x.FilmRepository.Update(It.IsAny<Film>()))
            .Throws(new Exception(_filmNotUpdatedError));

        var handler = new UpdateFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        await handler.Handle(new UpdateFilmCommand(testFilmUpdateRequestDto), CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.LogError(It.IsAny<object>(), _filmNotUpdatedError), Times.Once);
    }

    private Film GetFilm()
    {
        return _fixture.Build<Film>()
            .With(temp => temp.Id, 1)
            .Create();
    }

    private FilmUpdateRequestDto GetFilmUpdateRequestDto()
    {
        return _fixture.Build<FilmUpdateRequestDto>()
            .With(temp => temp.Id, 1)
            .Create();
    }

    private FilmResponseDto GetFilmResponseDto()
    {
        return _fixture.Build<FilmResponseDto>()
            .With(temp => temp.Id, 1)
            .Create();
    }

    private void SetupMapper(Film? testFilm = null, FilmResponseDto? testFilmResponseDto = null)
    {
        if (testFilm != null)
        {
            _mockMapper.Setup(x => x.Map<Film>(It.IsAny<FilmUpdateRequestDto>()))
                .Returns(testFilm);
        }

        if (testFilmResponseDto != null)
        {
            _mockMapper.Setup(x => x.Map<FilmResponseDto>(It.IsAny<Film>()))
                .Returns(testFilmResponseDto);
        }
    }
}