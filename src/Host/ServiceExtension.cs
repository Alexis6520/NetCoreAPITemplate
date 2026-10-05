using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace Host;

public static class ServiceExtension
{
    public static IServiceCollection AddCustomSerilog(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSerilog((services, lc) =>
        {
            lc.ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                new ExpressionTemplate(
                    // Include trace and span ids when present.
                    "[{@t:HH:mm:ss} {@l:u3}{#if SourceContext is not null} {Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}{#end}" +
                    "{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                    theme: TemplateTheme.Code
                )
            );
        });

        return services;
    }
}
