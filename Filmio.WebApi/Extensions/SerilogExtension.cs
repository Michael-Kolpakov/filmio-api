using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Filmio.WebApi.Extensions;

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
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("ProjectName", projectName);

        if (environment.IsDevelopment())
        {
            loggerConfiguration = loggerConfiguration.WriteTo.Console(LogEventLevel.Information, _consoleLogTemplate);
        }

        var columnOptions = new ColumnOptions();
        columnOptions.Store.Remove(StandardColumn.MessageTemplate);
        columnOptions.Store.Remove(StandardColumn.Properties);
        columnOptions.AdditionalColumns = new Collection<SqlColumn>
        {
            new ()
            {
                DataType = SqlDbType.NVarChar,
                ColumnName = "RequestPath"
            }
        };

        loggerConfiguration = loggerConfiguration
            .WriteTo.MSSqlServer(
                connectionString: configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection"),
                columnOptions: columnOptions,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    AutoCreateSqlTable = true,
                    TableName = "_Logs"
                });

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}