using Logic;
using Infrastructure.Persistence;
using API;
using System.Reflection;
using API.Middlewares;
using NLog;
using NLog.Web;
using Infrastructure;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddLogicalServices()
        .AddInfrastructure()
        .AddPersistence(builder.Configuration)
        .AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

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