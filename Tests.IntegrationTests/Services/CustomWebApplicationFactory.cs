using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Infrastructure.IntegrationTests.Services
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(SetPersistence);
            builder.UseEnvironment("Development");
        }

        private static void SetPersistence(IServiceCollection services)
        {
            // Elimina el DbContext que usa por defecto e inyecta el que apunta a pruebas
            var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                options.UseSqlServer("Server=BATICOMPUTADORA;Database=APITemplateTest;Trusted_Connection=True;TrustServerCertificate=True");
            });
        }
    }
}
