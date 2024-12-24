using Serilog.Context;

namespace filmio_api.Middlewares;

public class RequestLoggingSetupMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingSetupMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogContext.PushProperty("RequestPath", context.Request.Path);
        await _next(context);
    }
}