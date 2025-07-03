using Domain.Services;
using Infrastructure.EFCore.Postgre;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Startup
    {
        /// <summary>
        /// Agrega los servicios de infraestructura al contenedor de inyección de dependencias.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext, PostgreDbContext>();
            return services;
        }
    }
}
