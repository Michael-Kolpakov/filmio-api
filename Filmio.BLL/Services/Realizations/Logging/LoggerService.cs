using Filmio.BLL.Services.Interfaces.Logging;
using Serilog;

namespace Filmio.BLL.Services.Realizations.Logging;

public class LoggerService : ILoggerService
{
    private readonly ILogger _logger;

    public LoggerService(ILogger logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogDebug(string message)
    {
        _logger.Debug(message);
    }

    public void LogError(object? request, string errorMessage)
    {
        if (request != null)
        {
            var requestType = request.GetType().ToString();
            var requestClass = requestType.Substring(requestType.LastIndexOf('.') + 1);

            _logger.Error($"{requestClass} handled with the error: {errorMessage}");
        }
        else
        {
            _logger.Error(errorMessage);
        }
    }
}