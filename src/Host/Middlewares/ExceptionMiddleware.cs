using Application.RP;
using System.Net;
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Excepción no controlada");
                var result = new Result(HttpStatusCode.InternalServerError, Errors.SERVER_ERROR);
                context.Response.StatusCode = (int)result.StatusCode;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
