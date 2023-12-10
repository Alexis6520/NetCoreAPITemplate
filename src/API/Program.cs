using API;
using API.Middlewares;
using ApplicationServices;
using Infrastructure;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddApplicationServices()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    var app = builder.Build();
    await app.InitializeDatabaseAsync();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.UseMiddleware<NLogRequestPostedBodyMiddleware>
        (new NLogRequestPostedBodyMiddlewareOptions());

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Error al iniciar.");
	throw;
}
finally
{
    LogManager.Shutdown();
}