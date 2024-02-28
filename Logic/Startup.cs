using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Logic
{
    public static class Startup
    {
        public static IServiceCollection AddLogicalServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
                .AddAutoMapper(assembly);

            return services;
        }
    }
}
