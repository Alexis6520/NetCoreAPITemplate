using Application.Commands.Donuts;
using Application.Queries.Donuts;
using Application.RP;
using Domain.Entities;
using Host.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController(mediator)
    {
        /// <summary>
        /// Crea una dona
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<Result<int>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDonutCommand command)
        {
            return BuildResponse(await Mediator.Send(command));
        }

        /// <summary>
        /// Lista de donas
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<Result<List<Donut>>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> GetList(int page = 1, int pageSize = 5)
        {
            var query = new DonutsListQuery
            {
                Page = page,
                PageSize = pageSize
            };

            return BuildResponse(await Mediator.Send(query));
        }
    }
}
