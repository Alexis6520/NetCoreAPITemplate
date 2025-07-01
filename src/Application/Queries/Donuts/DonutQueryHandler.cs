using Application.Queries.Donuts.DTOs;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Donuts
{
    public class DonutQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<DonutsListQuery, List<DonutItemDTO>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<DonutItemDTO>> Handle(DonutsListQuery request, CancellationToken cancellationToken)
        {
            var list = await _dbContext.Donuts
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DonutItemDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}
