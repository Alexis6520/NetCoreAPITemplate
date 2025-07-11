using Application.ROP;
using Host.Abstractions;
using System.Net.Mime;

namespace Host.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                logger.LogError(ex, "Excepción no controlada");

                var response = new Response<object>
                {
                    Errors = [Errors.INTERNAL_ERROR]
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this WebApplication app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
