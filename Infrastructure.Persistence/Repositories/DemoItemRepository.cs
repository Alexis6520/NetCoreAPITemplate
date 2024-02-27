using Domain;
using Services.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class DemoItemRepository(ApplicationDbContext dbContext) : IDemoItemRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task AddAsync(DemoItem entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
        }
    }
}
