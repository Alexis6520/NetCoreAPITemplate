using Host.Features.Donuts.Commands;
using Host.Features.Donuts.Queries;
using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;

namespace Host.Features.Donuts;

[Route("api/[controller]")]
[ApiController]
public class DonutsController(DonutUseCases donutUseCases) : ControllerBase
{
    private readonly DonutUseCases _useCases = donutUseCases;

    [HttpPost]
    public async Task<IActionResult> Create(CreateDonutRequest request, CancellationToken cancellationToken)
    {
        int id = await _useCases.Create
            .Execute(request, cancellationToken);

        return Created((string?)null, new Response<int>(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var donuts = await _useCases.GetList
            .Execute(DonutsListRequest.Instance, cancellationToken);

        return Ok(new Response<Donut[]>(donuts));
    }
}
