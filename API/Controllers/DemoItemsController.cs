using Logic.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoItemsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<int>> CreateAsync(DemoItemCreateCommand command, CancellationToken cancellationToken = default)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }
    }
}
