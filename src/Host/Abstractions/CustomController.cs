using Application.ROP;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        protected ObjectResult BuildResponse<T>(Result<T> result)
        {
            object? body = result.Value is not Unity && result.Succeeded
                ? null
                : result;

            return StatusCode(
                (int)result.SatusCode,
                result);
        }

        protected ObjectResult BuildResponse<T>(T value)
        {
            return BuildResponse<T>(Result<T>.Success(value));
        }
    }
}
