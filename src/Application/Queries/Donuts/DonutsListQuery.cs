using Application.Queries.Donuts.DTOs;
using MediatR;

namespace Application.Queries.Donuts
{
    public class DonutsListQuery : IRequest<List<DonutDTO>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
