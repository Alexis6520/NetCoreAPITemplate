using Services;
using Services.Repositories;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            SetRepositories();
        }

        public IDemoItemRepository Items { get; set; }

        private void SetRepositories()
        {
            var props = GetType().GetProperties();
            var repoProps = props.Where(x => x.PropertyType.IsAssignableTo(typeof(IRepository<>)));

            foreach (var repoProp in repoProps)
            {
                var repo = Activator.CreateInstance(repoProp.PropertyType, _dbContext);
                repoProp.SetValue(this, repo);
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
