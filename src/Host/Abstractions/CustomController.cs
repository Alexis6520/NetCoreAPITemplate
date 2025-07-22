using Application.RP;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Abstractions
{
    [Route("api/[controller]")]
    public abstract class CustomController(IMediator mediator) : ControllerBase
    {
        protected IMediator Mediator => mediator;

        protected ObjectResult BuildResponse(Result result)
        {
            var statusCode = (int)result.StatusCode;

            object? body = statusCode >= StatusCodes.Status400BadRequest ?
                result : null;

            return StatusCode(statusCode, body);
        }

        protected ObjectResult BuildResponse<T>(Result<T> result)
        {
            return StatusCode((int)result.StatusCode, result);
        }

        protected ObjectResult BuildResponse<T>(T value)
        {
            var result = new Result<T>(value);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
