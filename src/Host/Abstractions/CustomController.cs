using Application.ROP;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Host.Abstractions
{
    /// <summary>
    /// Controlador base personalizado
    /// </summary>
    /// <param name="mediator"></param>
    [Route("api/[controller]")]
    public abstract class CustomController(IMediator mediator) : ControllerBase
    {
        protected IMediator Mediator { get; } = mediator;

        /// <summary>
        /// Construye una respuesta basada en un resultado que puede contener errores de dominio
        /// </summary>
        /// <typeparam name="T">Tipo de valor devuelto</typeparam>
        /// <param name="result">Resultado a convertir</param>
        /// <param name="statusCode">Código de estado HTTP en caso de éxito</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected ObjectResult BuildResponse<T>(Result<T> result, int statusCode = StatusCodes.Status200OK)
        {
            if (!result.Succeeded) return BuildErrorResponse(result);

            if (statusCode >= StatusCodes.Status400BadRequest)
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "Código inválido");

            Response<T>? response = result.Value is Unity ? null : new() { Value = result.Value };

            return StatusCode(
                statusCode,
                response);
        }

        /// <summary>
        /// Construye una respuesta basada en un resultado que puede contener errores de dominio y no devuelve un valor
        /// </summary>
        /// <param name="result">Resultado a convertir</param>
        /// <param name="statusCode">Código de estado HTTP en caso de éxito</param>
        /// <returns></returns>
        protected ObjectResult BuildResponse(Result<Unity> result, int statusCode = StatusCodes.Status204NoContent)
        {
            return BuildResponse<Unity>(result, statusCode);
        }

        private ObjectResult BuildErrorResponse<T>(Result<T> result)
        {
            int statusCode = StatusCodes.Status400BadRequest;
            if (result.Errors.Length == 1) statusCode = (int)result.Errors[0].StatusCode;

            return StatusCode(
                statusCode,
                new Response<T>
                {
                    Errors = result.Errors.Select(e => new ResponseError(e.Code, e.Message))
                });
        }
    }

    /// <summary>
    /// Para representar respuestas en el body
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Value { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ResponseError>? Errors { get; set; }
    }

    /// <summary>
    /// Representa un error en la respuesta
    /// </summary>
    /// <param name="code">Código de error</param>
    /// <param name="message">Mensaje de error</param>
    public class ResponseError(string code, string message)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;
    }
}
