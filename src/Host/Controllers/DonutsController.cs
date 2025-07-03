using Application.Commands.Donuts.Create;
using Application.Queries;
using Application.Queries.Donuts;
using Application.Queries.Donuts.DTOs;
using Application.ROP;
using Host.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController(mediator)
    {
        /// <summary>
        /// Crea una donita
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<Result<int>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDonutCommand command)
        {
            return BuildResponse(await Mediator.Send(command))
        }

        /// <summary>
        /// Lista las donitas
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<Result<List<DonutsListQuery>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(int page = 1, int pageSize = 10)
        {
            var query = new DonutsListQuery
            {
                Page = page,
                PageSize = pageSize
            };

            return BuildResponse(await Mediator.Send(query));
        }

        /// <summary>
        /// Obtiene una donita por su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<Result<DonutDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new FindQuery<int, DonutDTO>(id);
            return BuildResponse(await Mediator.Send(query));
        }
    }
}
