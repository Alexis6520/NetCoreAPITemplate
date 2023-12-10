using API.Wrappers;
using ApplicationServices.Exceptions;
using System.Text.Json;

namespace API.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

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

        private static async Task HandleError(Exception ex, HttpContext context)
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
                body = new ErrorResponse("Error en el servidor.", null);
                statusCode = StatusCodes.Status500InternalServerError;
            }

            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(body);
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(json);
        }
    }
}
