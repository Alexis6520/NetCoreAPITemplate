using API.Responses;
using Logic.Exceptions;
using System.Text.Json;

namespace API.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate requestDelegate)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ErrorResponse();

                switch (ex)
                {
                    case WebAPIException e:
                        response.StatusCode = (int)e.StatusCode;
                        responseModel.Message = e.Message;
                        break;
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        responseModel.Message = "Hubo un problema al procesar la solicitud";
                        break;
                }

                var json = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(json);
            }
        }
    }
}
