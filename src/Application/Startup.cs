using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Startup
    {
        /// <summary>
        /// Agrega los servicios de la aplicación al contenedor de inyección de dependencias.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(Startup).Assembly));
            return services;
        }
    }
}
