using Application;
using Host;
using Host.Middlewares;
using Infrastructure;
using NLog;
using NLog.Web;

var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddApplicationServices()
        .AddInfrastructure()
        .AddCustomSwagger();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddControllers();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (HostAbortedException) { }
catch (Exception ex)
{
    logger.Error("Programa detenido por excepción", ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}

public partial class Program { }