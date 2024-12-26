namespace Filmio.BLL.Services.Interfaces.Logging;

public interface ILoggerService
{
    void LogInformation(string message);

    void LogWarning(string message);

    void LogDebug(string message);

    void LogError(object? request, string errorMessage);
}