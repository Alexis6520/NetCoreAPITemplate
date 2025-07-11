using Application.Commands.Donuts.Create;
using Application.Queries.Donuts;
using Application.Queries.Donuts.DTOs;
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
            return BuildResponse(await Mediator.Send(command));
        }

        [HttpGet]
        [ProducesResponseType<Response<List<DonutDTO>>>(StatusCodes.Status200OK)]
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
