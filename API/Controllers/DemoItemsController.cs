using Logic.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoItemsController(IMediator mediator, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Crea un artículo demo
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync(DemoItemCreateCommand command, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Obtiene todos los artículos demo
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<DemoItemDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var results = await _unitOfWork.DemoItems.GetAllAsync(cancellationToken);
            return Ok(results);
        }

        /// <summary>
        /// Actualiza un artículo demo
        /// </summary>
        /// <param name="id">Id del artículo</param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync(int id, DemoItemUpdateCommand command, CancellationToken cancellationToken = default)
        {
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
