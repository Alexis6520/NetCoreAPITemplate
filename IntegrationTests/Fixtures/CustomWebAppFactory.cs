using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Persistence;
using System.Data.Common;

namespace IntegrationTests.Fixtures;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string ConnectionString = "Server=127.0.0.1;Database=krispy_kreme_test;User Id=krispy;Password=kreme";

    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static bool _dbInitialized = false;

    private readonly IConfiguration _config;

    public CustomWebAppFactory()
    {
        _config = new ConfigurationBuilder()
            .AddUserSecrets<CustomWebAppFactory>()
            .AddEnvironmentVariables()
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            if (_dbInitialized) return;

            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            using var dbContext = new ApplicationDbContext(opts);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            _dbInitialized = true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));

            if(dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

             if(dbConnectionDescriptor is not null) services.Remove(dbConnectionDescriptor);

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                return connection;
            });

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseNpgsql(connection);
            });
        });

        builder.UseEnvironment("Development");
        builder.UseConfiguration(_config);
    }
}
