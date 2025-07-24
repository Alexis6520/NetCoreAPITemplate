using Infrastructure.EFCore.Postgre;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
                    //using var context = CreateContext();
                    //context.Database.Migrate();
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

        private static PostgreDbContext CreateContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<CustomWebAppFactory>()
                .Build();

            return new PostgreDbContext(configuration);
        }
    }
}
