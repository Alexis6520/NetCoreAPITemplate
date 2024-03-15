using API.Responses;
using Domain.Settings;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
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

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();

            // Configura la autenticación por Json Web Token
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = BuildTokenValidationParameters(settings);
                x.Events = BuildJwtBearerEvents(); // Personaliza la respuesta en estos eventos
            });

            return services;
        }

        private static JwtBearerEvents BuildJwtBearerEvents()
        {
            return new JwtBearerEvents
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
        }

        private static TokenValidationParameters BuildTokenValidationParameters(JWTSettings settings)
        {
            var key = Encoding.UTF8.GetBytes(settings.SecretKey);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = settings.ValidIssuer,
                ValidAudiences = settings.ValidAudiences,
            };
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                // Describe el tipo de authorización
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                // Agrega la autorización como requisito 
                options.AddSecurityRequirement(BuildOpenApiSecurityRequirement());
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }

        private static OpenApiSecurityRequirement BuildOpenApiSecurityRequirement()
        {
            return new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                };
        }
    }
}
