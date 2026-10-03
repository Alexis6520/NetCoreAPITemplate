using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class PersistenceServiceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("Default");

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'Default' not found.");
            }

            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
