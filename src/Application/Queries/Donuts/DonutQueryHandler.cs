using Application.Queries.Donuts.DTOs;
using Application.ROP;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Queries.Donuts
{
    public class DonutQueryHandler(ApplicationDbContext dbContext) :
        IRequestHandler<DonutsListQuery, List<DonutItemDTO>>,
        IRequestHandler<FindQuery<int, DonutDTO>, Result<DonutDTO>>
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

        public async Task<Result<DonutDTO>> Handle(FindQuery<int, DonutDTO> request, CancellationToken cancellationToken)
        {
            var donut = await _dbContext.Donuts
                .AsNoTracking()
                .Where(x => x.Id == request.Key)
                .Select(x => new DonutDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price
                })
                .FirstOrDefaultAsync(cancellationToken);

            return donut is not null
                ? Result<DonutDTO>.Success(donut)
                : Result<DonutDTO>.Failure(HttpStatusCode.NotFound, Errors.NOT_FOUND);
        }
    }
}
