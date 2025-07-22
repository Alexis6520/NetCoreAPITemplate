using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Donuts
{
    public class DonutQueryHandler(AppDbContext dbContext) : IRequestHandler<DonutsListQuery, List<Donut>>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<List<Donut>> Handle(DonutsListQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Donuts
                .AsNoTracking()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
