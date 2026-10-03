using System.Reflection;

namespace Host.Mediation;

public static class MediationServiceExtension
{
    public static IServiceCollection AddMediation(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x =>
                x.IsClass &&
                !x.IsAbstract &&
                x.GetInterfaces()
                    .Any(y =>
                        y == typeof(IUseCaseCollection) ||
                        y == typeof(IBaseRequestHandler)
                    )
            );

        foreach (Type type in types)
        {
            services.AddTransient(type);
        }

        return services;
    }
}
