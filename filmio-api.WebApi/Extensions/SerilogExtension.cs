using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace filmio_api.Extensions;

public static class SerilogExtension
{
    private const string _consoleLogTemplate = "[{Timestamp:HH:mm:ss.fff} [{Level}] {SourceContext} {Message}{NewLine}{Exception}";

    public static void AddSerilogLogging(
        this IServiceCollection _,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var projectName = Assembly.GetCallingAssembly().GetName().Name?.ToLowerInvariant();
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("ProjectName", projectName);

        if (environment.IsDevelopment())
        {
            loggerConfiguration = loggerConfiguration.WriteTo.Console(LogEventLevel.Information, _consoleLogTemplate);
        }

        var columnOptions = new Dictionary<string, ColumnWriterBase>
        {
            {
                "RequestPath",
                new RenderedMessageColumnWriter()
            }
        };

        loggerConfiguration = loggerConfiguration
            .WriteTo.PostgreSQL(
                connectionString: configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection"),
                columnOptions: columnOptions,
                tableName: "Logs",
                needAutoCreateTable: true);

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}