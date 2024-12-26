using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.MediatR.Film.Delete;
using filmio_api.BLL.Services.Interfaces.Logging;
using filmio_api.DAL.Entities.Film;
using filmio_api.DAL.Repositories.Interfaces.Base;
using FluentAssertions;
using Moq;

namespace Filmio.XUnitTest.MediatR.Films;

public class DeleteFilmTest
{
    private const string _filmNotDeletedError = "The film wasn`t deleted";
    private const string _noFilmWithSuchIdError = "There is no film with such Id";

    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly IFixture _fixture;

    public DeleteFilmTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully_WhenFilmDeleted()
    {
        // Arrange
        var testPartner = GetFilm();

        _mockMapper.Setup(x => x.Map<FilmResponseDto>(It.IsAny<Film>()))
            .Returns(GetFilmResponseDto());

        _mockRepository
            .Setup(x => x.FilmRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Film, bool>>>(), null))
            .ReturnsAsync(testPartner);

        var handler = new DeleteFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteFilmCommand(testPartner.Id), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _mockRepository.Verify(x => x.FilmRepository.Delete(It.Is<Film>(x => x.Id == testPartner.Id)), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenExceptionThrownDuringDeletion()
    {
        // Arrange
        var testPartner = GetFilm();

        _mockMapper.Setup(x => x.Map<FilmResponseDto>(It.IsAny<Film>()))
            .Returns(GetFilmResponseDto());

        _mockRepository
            .Setup(x => x.FilmRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Film, bool>>>(), null))
            .ReturnsAsync(testPartner);
        _mockRepository.Setup(x => x.FilmRepository.Delete(It.IsAny<Film>()))
            .Throws(new Exception(_filmNotDeletedError));

        var handler = new DeleteFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteFilmCommand(testPartner.Id), CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(_filmNotDeletedError);
    }

    [Fact]
    public async Task ShouldThrowException_IdNotExist()
    {
        // Arrange
        var testPartner = GetFilm();

        _mockRepository
            .Setup(x => x.FilmRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Film, bool>>>(), null))
            .ReturnsAsync(GetFilmWithNotExistingId());

        var handler = new DeleteFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteFilmCommand(testPartner.Id), CancellationToken.None);

        // Assert
        result.Errors[0].Message.Should().Be(_noFilmWithSuchIdError);

        _mockRepository.Verify(x => x.FilmRepository.Delete(It.IsAny<Film>()), Times.Never);
    }

    [Fact]
    public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        var testPartner = GetFilm();

        _mockMapper.Setup(x => x.Map<FilmResponseDto>(It.IsAny<Film>()))
            .Returns(GetFilmResponseDto());

        _mockRepository
            .Setup(x => x.FilmRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Film, bool>>>(), null))
            .ReturnsAsync(testPartner);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ThrowsAsync(new Exception(_filmNotDeletedError));

        var handler = new DeleteFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteFilmCommand(testPartner.Id), CancellationToken.None);

        // Assert
        result.Errors[0].Message.Should().Be(_filmNotDeletedError);
    }

    [Fact]
    public async Task ShouldLogError_WhenFilmNotFound()
    {
        // Arrange
        var testPartner = GetFilm();

        _mockRepository
            .Setup(x => x.FilmRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Film, bool>>>(), null))
            .ReturnsAsync(GetFilmWithNotExistingId());

        var handler = new DeleteFilmHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

        // Act
        await handler.Handle(new DeleteFilmCommand(testPartner.Id), CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.LogError(It.IsAny<object>(), _noFilmWithSuchIdError), Times.Once);
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

    private Film? GetFilmWithNotExistingId()
    {
        return null;
    }
}