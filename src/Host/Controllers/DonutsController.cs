using Application.Commands.Donuts.Create;
using Host.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController(mediator)
    {
        [HttpPost]
        [ProducesResponseType<Response<int>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDonutCommand command)
        {
            return BuildResponse(await Mediator.Send(command), StatusCodes.Status201Created);
        }
    }
}
