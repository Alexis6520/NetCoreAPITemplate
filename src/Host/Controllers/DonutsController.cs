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
        [HttpPost]
        [ProducesResponseType<Result<int>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDonutCommand command)
        {
            return BuildResponse(await Mediator.Send(command));
        }

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

        [HttpGet("{id}")]
        [ProducesResponseType<Result<DonutDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new FindQuery<int, DonutDTO>(id);
            return BuildResponse(await Mediator.Send(query));
        }
    }
}
