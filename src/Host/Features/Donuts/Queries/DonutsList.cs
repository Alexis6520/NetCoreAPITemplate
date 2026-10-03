using Host.Mediation;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

namespace Host.Features.Donuts.Queries;

public record DonutsListRequest : IRequest<Donut[]>
{
    public static readonly DonutsListRequest Instance = new();
}

public class DonutsListHandler(ApplicationDbContext dbContext) : IRequestHandler<DonutsListRequest, Donut[]>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Donut[]> Execute(DonutsListRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Donuts
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToArrayAsync(cancellationToken);
    }
}
