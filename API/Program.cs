using Logic;
using Infrastructure.Persistence;
using API;
using API.Middlewares;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddLogicalServices()
        .AddPersistence(builder.Configuration)
        .AddJWTAuthentication(builder.Configuration)
        .AddSwagger()
        .AddEndpointsApiExplorer()
        .AddControllers();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<NLogRequestPostedBodyMiddleware>(new NLogRequestPostedBodyMiddlewareOptions());
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    await app.InitializeDatabaseAsync();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Programa detenido por excepcion");
    throw;
}
finally
{
    LogManager.Shutdown();
}