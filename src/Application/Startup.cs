using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(Startup).Assembly));
            return services;
        }
    }
}
