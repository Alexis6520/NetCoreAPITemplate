using Application;
using Infrastructure;
using NLog;
using NLog.Web;

var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddApplicationServices()
        .AddInfrastructure();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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