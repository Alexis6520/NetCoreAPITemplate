using Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Services
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public CustomWebAppFactory()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    InitializeDatabase();
                    _databaseInitialized = true;
                }
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<CustomWebAppFactory>()
                .Build();

            builder.UseConfiguration(configuration);
            builder.UseEnvironment("Testing");
        }

        private void InitializeDatabase()
        {
            using var scope = Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
