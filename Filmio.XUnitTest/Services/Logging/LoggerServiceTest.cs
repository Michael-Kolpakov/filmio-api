using Filmio.BLL.Services.Interfaces.Logging;
using Filmio.BLL.Services.Realizations.Logging;
using Moq;
using Serilog;

namespace Filmio.XUnitTest.Services.Logging;

public class LoggerServiceTest
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly ILoggerService _loggerService;

    public LoggerServiceTest()
    {
        _mockLogger = new Mock<ILogger>();
        _loggerService = new LoggerService(_mockLogger.Object);
    }

    [Fact]
    public void ShouldLogInformation_WhenLogInformationCalled()
    {
        // Arrange
        var message = "Information message";

        // Act
        _loggerService.LogInformation(message);

        // Assert
        _mockLogger.Verify(x => x.Information(message), Times.Once);
    }

    [Fact]
    public void ShouldLogWarning_WhenLogWarningCalled()
    {
        // Arrange
        var message = "Warning message";

        // Act
        _loggerService.LogWarning(message);

        // Assert
        _mockLogger.Verify(x => x.Warning(message), Times.Once);
    }

    [Fact]
    public void ShouldLogDebug_WhenLogDebugCalled()
    {
        // Arrange
        var message = "Debug message";

        // Act
        _loggerService.LogDebug(message);

        // Assert
        _mockLogger.Verify(x => x.Debug(message), Times.Once);
    }

    [Fact]
    public void ShouldLogError_WhenLogErrorCalledWithRequest()
    {
        // Arrange
        var request = new
        {
            Id = 1,
            Name = "Test"
        };
        var errorMessage = "Error message";
        var requestType = request.GetType().ToString();
        var requestClass = requestType.Substring(requestType.LastIndexOf('.') + 1);

        // Act
        _loggerService.LogError(request, errorMessage);

        // Assert
        _mockLogger.Verify(x => x.Error($"{requestClass} handled with the error: {errorMessage}"), Times.Once);
    }

    [Fact]
    public void ShouldLogError_WhenLogErrorCalledWithoutRequest()
    {
        // Arrange
        var errorMessage = "Error message";

        // Act
        _loggerService.LogError(null, errorMessage);

        // Assert
        _mockLogger.Verify(x => x.Error(errorMessage), Times.Once);
    }
}