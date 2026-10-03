using Host.Mediation;
using Persistence;
using Persistence.Entities;

namespace Host.Features.Donuts.Commands;

public record CreateDonutRequest(
    string Name,
    string? Description,
    decimal Price
) : IRequest<int>;

public class CreateDonutHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateDonutRequest, int>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<int> Execute(CreateDonutRequest request, CancellationToken cancellationToken = default)
    {
        var donut = new Donut
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        _dbContext.Donuts.Add(donut);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return donut.Id;
    }
}