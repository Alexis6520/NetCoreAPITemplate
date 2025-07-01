using Application.Queries.Donuts.DTOs;
using MediatR;

namespace Application.Queries.Donuts
{
    /// <summary>
    /// Consulta para listar donitas.
    /// </summary>
    public class DonutsListQuery : IRequest<List<DonutItemDTO>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
