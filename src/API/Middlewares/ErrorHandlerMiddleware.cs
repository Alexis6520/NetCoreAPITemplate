using API.Wrappers;
using ApplicationServices.Exceptions;
using System.Text.Json;

namespace API.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleError(ex, context);
            }
        }

        private async Task HandleError(Exception ex, HttpContext context)
        {
            ErrorResponse body;
            int statusCode;

            if (ex is CustomException e)
            {
                body = new ErrorResponse(e.Message, e.Errors);
                statusCode = (int)e.StatusCode;
            }
            else
            {
                var message = $"Error {Guid.NewGuid()}.";
                body = new ErrorResponse(message);
                statusCode = StatusCodes.Status500InternalServerError;
                _logger.LogError(ex, "{message}", message);
            }

            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(body);
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(json);
        }
    }
}
