using API.Responses;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace API
{
    public static class Startup
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app, CancellationToken cancellationToken = default)
        {
            using var scope = app.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKeyBytes = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

            // Configura la autenticación por Json Web Token
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Personaliza la respuesta en estos eventos
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new ErrorResponse("No autorizado."));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new ErrorResponse("No tiene acceso a este recurso."));
                        return context.Response.WriteAsync(result);
                    }
                };
            });

            return services;
        }
    }
}
