using DomainServices.Common;
using DomainServices.Repositories;
using DomainServices.Services;
using System.Reflection;

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

        public IDemoItemRepository DemoItems { get; set; }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private void SetRepositories()
        {
            var propsInfo = GetType().GetProperties();
            var assembly = Assembly.GetExecutingAssembly();

            var reposClasses = assembly.GetTypes()
                .Where(x => x.IsClass && x.IsAssignableTo(typeof(IRepository<>)));

            foreach (var propInfo in propsInfo)
            {
                var repoClass = reposClasses
                    .FirstOrDefault(x => x.IsAssignableTo(propInfo.PropertyType));

                if (repoClass != null)
                {
                    object repoInstance = Activator.CreateInstance(repoClass, _dbContext);
                    propInfo.SetValue(this, repoInstance);
                }
            }
        }
    }
}
