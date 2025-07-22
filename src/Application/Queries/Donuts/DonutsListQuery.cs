using Domain.Entities;
using MediatR;

namespace Application.Queries.Donuts
{
    public class DonutsListQuery : IRequest<List<Donut>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
