using filmio_api.Extensions;
using filmio_api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.UseMiddleware<RequestLoggingSetupMiddleware>();

DatabaseExtension.InitializeDatabase(app);

app.Run();