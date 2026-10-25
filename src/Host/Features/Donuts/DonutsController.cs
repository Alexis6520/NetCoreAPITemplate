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

    /// <summary>
    /// Crea una donita del krispy kreme.
    /// </summary>
    /// <param name="request">Datos necesarios para crear la donita.</param>
    /// <param name="cancellationToken">Token para cancelar la operación de forma asíncrona.</param>
    /// <returns>Resultado de la acción con el identificador de la dona creada.</returns>
    [HttpPost]
    [ProducesResponseType<Response<int>>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateDonutRequest request, CancellationToken cancellationToken)
    {
        int id = await _useCases.Create
            .Execute(request, cancellationToken);

        return Created((string?)null, new Response<int>(id));
    }

    /// <summary>
    /// Obtiene una lista de donitas del krispy kreme.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación de forma asíncrona.</param>
    /// <returns>Lista de donitas disponibles.</returns>
    [HttpGet]
    [ProducesResponseType<Response<Donut[]>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var donuts = await _useCases.GetList
            .Execute(DonutsListRequest.Instance, cancellationToken);

        return Ok(new Response<Donut[]>(donuts));
    }
}
