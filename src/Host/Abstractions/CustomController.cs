using Application.ROP;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Host.Abstractions
{
    [Route("api/[controller]")]
    public abstract class CustomController(IMediator mediator) : ControllerBase
    {
        protected IMediator Mediator => mediator;

        protected ObjectResult BuildResponse<T>(Result<T> result)
        {
            var response = new Response<T>
            {
                Value = result.Value,
                Errors = !result.Succeeded ? result.Errors : null,
            };

            return StatusCode((int)result.StatusCode, response);
        }

        protected ObjectResult BuildResponse(Result<Unity> result)
        {
            Response<object>? response = null;

            if (!result.Succeeded)
            {
                response = new()
                {
                    Errors = result.Errors,
                };
            }

            return StatusCode((int)result.StatusCode, response);
        }
    }

    public class Response<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Value { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<Error>? Errors { get; set; }
    }
}
