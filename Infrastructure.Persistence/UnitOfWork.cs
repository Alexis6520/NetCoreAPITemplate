using AutoMapper;
using Services;
using Services.Repositories;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            SetRepositories();
        }

        public IDemoItemRepository DemoItems { get; set; }

        private void SetRepositories()
        {
            var props = GetType().GetProperties();
            var repoProps = props.Where(x => x.PropertyType.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IRepository<>)));
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var repoProp in repoProps)
            {
                var repoClass = assemblyTypes.First(x => x.IsClass && x.IsAssignableTo(repoProp.PropertyType));
                var repo = Activator.CreateInstance(repoClass, _dbContext, _mapper);
                repoProp.SetValue(this, repo);
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
