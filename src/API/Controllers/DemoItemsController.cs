using API.Wrappers;
using ApplicationServices.Commands.DemoItemCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DemoItemsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Crea un artículo demo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Response<int>>> CreateAsync(CreateDemoItemCommand command)
        {
            var id = await _mediator.Send(command);
            return Created("", new Response<int>(id));
        }
    }
}
