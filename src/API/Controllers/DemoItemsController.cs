using API.Wrappers;
using ApplicationServices.Commands.DemoItemCommands;
using DomainServices.DTOs.DemoItemDTOs;
using DomainServices.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class DemoItemsController(IMediator mediator, IUnitOfWork context) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUnitOfWork _context = context;

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

        /// <summary>
        /// Buscador de artículos
        /// </summary>
        /// <param name="text">Texto a buscar en el artículo</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<Response<List<DemoItemSearchDTO>>>> SearchAsync(string text)
        {
            var results = await _context.DemoItems.SearchAsync(text);
            var response = new Response<List<DemoItemSearchDTO>>(results);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza un artículo demo
        /// </summary>
        /// <param name="id">Id de artículo</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync(int id, UpdateDemoItemCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
