using Domain.Services;
using UnitTests.Services;

namespace UnitTests.Abstractions
{
    public abstract class BaseTest<TService> : IDisposable
    {
        private AppDbContext? _dbContext;

        protected AppDbContext DbContext => _dbContext ??= new MemoryDbContext(typeof(TService).Name);

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
