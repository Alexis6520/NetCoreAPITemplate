using Domain.Services;
using Infrastructure.EFCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext, SqlServerDbContext>();
            return services;
        }
    }
}
