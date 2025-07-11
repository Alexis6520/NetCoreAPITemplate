using Application.Queries.Donuts.DTOs;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Donuts
{
    public class DonutQueryHandler(
        ApplicationDbContext dbContext) : IRequestHandler<DonutsListQuery, List<DonutDTO>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<DonutDTO>> Handle(DonutsListQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Donuts
                .OrderBy(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DonutDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
