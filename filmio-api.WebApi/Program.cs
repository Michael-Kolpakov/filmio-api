using filmio_api.Extensions;
using filmio_api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services));

builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.UseMiddleware<RequestLoggingSetupMiddleware>();

DatabaseExtension.InitializeDatabase(app);

app.Run();