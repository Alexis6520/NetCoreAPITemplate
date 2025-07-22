using Domain.Services;
using Infrastructure.EFCore.Postgre;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext, PostgreDbContext>();
            return services;
        }
    }
}
